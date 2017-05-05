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
    public partial class EmployeeAvecFuncs : Form
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

        public EmployeeAvecFuncs()
        {
            InitializeComponent();

            _modifiable = false;
            _supprimable = false;
            _ajoutable = false;

            SelectEmployee = (id) =>
            {
                var requeteSelect = "Select Id,FirstName,LastName,Addresse,Date From Employee Where Id = @Id";                
                var result = new Dictionary<string, object>();// Employee = new { Id = 0, FirstName = "", LastName = "", Addresse = "", Date = "" }, Notification = ""} ;

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(requeteSelect, connection))
                    {
                        command.Parameters.AddWithValue("@Id", Convert.ToInt32(id));
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    result.Add("Id", Convert.ToString(reader["Id"]));
                                    result.Add("FirstName", Convert.ToString(reader["FirstName"]));
                                    result.Add("LastName", Convert.ToString(reader["LastName"]));
                                    result.Add("Addresse", Convert.ToString(reader["Addresse"]));
                                    result.Add("Date", Convert.ToString(reader["Date"]));
                                }
                            else result["Notification"] = "Aucun enregistrement ne correspond au critere votre recherche.";

                        } // == reader.Dispose(); // la methode dispose libere toutes les resources memoire utiliser pas l'objet.                    
                    } // == connection.Dispose(); // dans le cas d'un objet de connect "SqlConnection,OleDbConnection, ..." connection.Close()
                      // est appeler dans la methode Dispose() .
                }
                return result;
            };

            UpdateEmployee = (employee) =>
            {
                var requeteUpdate = "Update Employee Set FirstName=@FirstName, LastName=@LastName, Addresse=@Addresse, Date=@Date Where Id = @ID";
                var notification = "";
                var result = false;
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(requeteUpdate, connection))
                    {
                        command.Parameters.AddWithValue("@Id", Convert.ToInt32(employee["Id"]));
                        command.Parameters.AddWithValue("@FirstName", Convert.ToString(employee["FirstName"]));
                        command.Parameters.AddWithValue("@LastName", Convert.ToString(employee["LastName"]));
                        command.Parameters.AddWithValue("@Addresse", Convert.ToString(employee["Addresse"]));
                        command.Parameters.AddWithValue("@Date", Convert.ToString(employee["Date"]));

                        result = command.ExecuteNonQuery() == 1;
                        if (result) notification = "Enregistrement modifiee avec succees";
                        else notification = "Veuillez verifier les donnees que vous avez saisis";
                    }
                }
                return new { Success = result, Notification = notification };
            };

            InsertEmployee = (employee) => 
            {
                var requeteInsert = "Insert Into Employee (FirstName, LastName, Addresse, Date) Values (@FirstName,@LastName,@Addresse,@Date)";
                var notification = "";
                var id = 0;

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(requeteInsert, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", Convert.ToString(employee["FirstName"])); // l'utilisation des parametres diminue les vas d'erreurs 
                        command.Parameters.AddWithValue("@LastName", Convert.ToString(employee["LastName"]));   // et lors du declenchement d'une erreur on peux la localiser 
                        command.Parameters.AddWithValue("@Addresse", Convert.ToString(employee["Addresse"]));   // rapidement et facilement.
                        command.Parameters.AddWithValue("@Date", Convert.ToString(employee["Date"]));
                        
                        if (command.ExecuteNonQuery() == 1) notification = "Enregistrement Ajouter avec Succes."; // la verification du resultat de la methode ExecuteNonQuery()
                        else notification = "Veuiller verifier la validite des donnees saisis.";                  // permet de savoir si l'operation c'est derouler avec succes 
                                                                                                                          // la valeur de retour represente le nombre de colonnes affectes.
                        command.CommandText = "Select TOP 1 Id From Employee Order By Id Desc"; // cette requette permet de recuperer le dernier identifiant dans la table.

                        
                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                id = Convert.ToInt32(reader["Id"]);
                            }
                    }
                }
                return new { Id =id, Notification =notification };
            };

            DeleteEmployee = (id) =>
            {
                var requeteDelete = "Delete From Employee Where Id = @Id";
                var notification = "";
                bool result = false;

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(requeteDelete, connection))
                    {
                        command.Parameters.AddWithValue("@Id", Convert.ToInt32(txtId.Text));

                        result = command.ExecuteNonQuery() == 1;
                        if (result) notification = "Enregistrement supprimer avec succes.";
                        else notification = "un probleme est survenus";
                    }
                }

                return new { Success = result, Notification = notification };
            };
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

                var result = InsertEmployee(employee);

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

                var result = UpdateEmployee(employee);
                
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
            var result = DeleteEmployee(Convert.ToInt32(txtId.Text));
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
