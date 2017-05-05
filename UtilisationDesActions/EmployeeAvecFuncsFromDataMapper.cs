using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UtilisationDesActions
{
    public partial class EmployeeAvecFuncsFromDataMapper : Form
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            //@"Data Source=.\SQLExpress;Initial Catalog=ADONetCourse;Integrated Security=true";

        private bool _modifiable;
        private bool _supprimable;
        private bool _ajoutable;

        private Func<int, Dictionary<string, object>> SelectEmployee;
        private Func<Dictionary<string, object>, dynamic> UpdateEmployee;
        private Func<Dictionary<string, object>, dynamic> InsertEmployee;
        private Func<int, dynamic> DeleteEmployee;

        public EmployeeAvecFuncsFromDataMapper()
        {
            InitializeComponent();

            _modifiable = false;
            _supprimable = false;
            _ajoutable = false;

            var mapper = new EmployeeDataMapper();

            SelectEmployee = mapper.SelectEmployee;

            UpdateEmployee = mapper.UpdateEmployee;

            InsertEmployee = mapper.InsertEmployee;

            DeleteEmployee = mapper.DeleteEmployee;
        }

        private void txtChercher_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)                
            {
                var id = Convert.ToInt32(txtChercher.Text);

                var result = 
                    SelectEmployee(id);

                if (result.ContainsKey("Notification")) lblNotification.Text = Convert.ToString(result["Notification"]);
                else
                {
                    RefreshControlsValues(result);
                }
            }
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            if(btnAjouter.Text == "Enregistrer")
            {
                var employee = new Dictionary<string, object>()
                {
                    { "Id", txtId.Text },
                    { "FirstName", txtFirstName.Text },
                    { "LastName", txtLastName.Text },
                    { "Addresse", txtAddresse.Text },
                    { "Date", txtDate.Text }
                };

                var result =
                    InsertEmployee(employee);

                txtId.Text = Convert.ToString(result.Id);
                lblNotification.Text = result.Notification;

                btnAjouter.Text = "Ajouter";
            }
            else
            {
                RefreshControlsValues(new Dictionary<string, object>()
                {
                    { "Id", "" },
                    { "FirstName", "" },
                    { "LastName", "" },
                    { "Addresse", "" },
                    { "Date", "" }
                });               

                btnAjouter.Text = "Enregistrer";
            }
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            if (btnModifier.Text == "Enregistrer")
            {
                var employee = new Dictionary<string, object>()
                {
                    { "Id", Convert.ToInt32(txtId.Text) },
                    { "FirstName", txtFirstName.Text },
                    { "LastName", txtLastName.Text },
                    { "Addresse", txtAddresse.Text },
                    { "Date", txtDate.Text }
                };

                var result = 
                    UpdateEmployee(employee);
                
                lblNotification.Text = result.Notification;

                btnModifier.Text = "Modifier";
            }
            else
            {
                btnModifier.Text = "Enregistrer";
            }
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            var result = 
                DeleteEmployee(Convert.ToInt32(txtId.Text));
            lblNotification.Text = result.Notification;
        }

        private void RefreshControlsValues(Dictionary<string, object> values)
        {
            txtId.Text = Convert.ToString(values["Id"]);
            txtFirstName.Text = Convert.ToString(values["FirstName"]);
            txtLastName.Text = Convert.ToString(values["LastName"]);
            txtAddresse.Text = Convert.ToString(values["Addresse"]);
            txtDate.Text = Convert.ToString(values["Date"]);
        }
    }
}
