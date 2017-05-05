using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TravailBasic
{
    public partial class Employee : Form
    {
        // Declaration des Variables
        private string connectionString = @"Data Source=.\SQLExpress;Initial Catalog=ADONetCourse;Integrated Security=true";

        private string _requeteSelect = "Select Id,FirstName,LastName,Addresse,Date From Employee Where Id = @Id";
        private string _requeteInsert = "Insert Into Employee (FirstName, LastName, Addresse, Date) Values (@FirstName,@LastName,@Addresse,@Date)";
        private string _requeteUpdate = "Update Employee Set FirstName=@FirstName, LastName=@LastName, Addresse=@Addresse, Date=@Date Where Id = @ID";
        private string _requeteDelete = "Delete From Employee Where Id = @Id";

        private SqlConnection _connection;
        private SqlCommand _command;
        private SqlDataReader _reader;
        //private SqlDataAdapter _adapter;

        private bool _modifiable;
        private bool _supprimable;
        private bool _ajoutable;

        public Employee()
        {
            InitializeComponent();

            _modifiable = false;
            _supprimable = false;
            _ajoutable = false;

            _connection = new SqlConnection(connectionString);
        }

        private void txtChercher_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)                
            {

                _connection.Open();
                _command = new SqlCommand(_requeteSelect, _connection);
                _command.Parameters.AddWithValue("@Id", Convert.ToInt32(txtChercher.Text));

                _reader = _command.ExecuteReader();

                if (_reader.HasRows)
                    while (_reader.Read())
                    {
                        txtId.Text = Convert.ToString(_reader["Id"]);
                        txtFirstName.Text = Convert.ToString(_reader["FirstName"]);
                        txtLastName.Text = Convert.ToString(_reader["LastName"]);
                        txtAddresse.Text = Convert.ToString(_reader["Addresse"]);
                        txtDate.Text = Convert.ToString(_reader["Date"]);
                    }
                else lblNotification.Text = "Aucun enregistrement ne correspond au critere votre recherche.";

                _reader = null; // _reader.Dispose();
                _command = null; // _command.Dispose();
                _connection.Close();
            }
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            //_ajoutable = true;
            //_modifiable = false;
            //_supprimable = false;
            if(btnAjouter.Text == "Enregistrer")
            {
                _connection.Open();

                _command = new SqlCommand(_requeteInsert, _connection);
                
                _command.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                _command.Parameters.AddWithValue("@LastName", txtLastName.Text);
                _command.Parameters.AddWithValue("@Addresse", txtAddresse.Text);
                _command.Parameters.AddWithValue("@Date", txtDate.Text);

                int result = _command.ExecuteNonQuery();

                if (result == 1) lblNotification.Text = "Enregistrement Ajouter avec Succes.";
                else lblNotification.Text = "Veuiller verifier la validite des donnees saisis.";

                _command.CommandText = "Select TOP 1 Id From Employee Order By Id Desc";

                _reader = _command.ExecuteReader();
                while (_reader.Read())
                {
                    txtId.Text = Convert.ToString(_reader["Id"]);
                }
                _reader = null;
                _command = null;

                _connection.Close();

                btnAjouter.Text = "Ajouter";
            }
            else
            {
                txtId.Text = "";
                txtFirstName.Text = "";
                txtLastName.Text = "";
                txtAddresse.Text = "";
                txtDate.Text = "";

                btnAjouter.Text = "Enregistrer";
            }
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            if (btnModifier.Text == "Enregistrer")
            {
                _connection.Open();
                _command = new SqlCommand(_requeteUpdate, _connection);

                _command.Parameters.AddWithValue("@Id", Convert.ToInt32(txtId.Text));
                _command.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                _command.Parameters.AddWithValue("@LastName", txtLastName.Text);
                _command.Parameters.AddWithValue("@Addresse", txtAddresse.Text);
                _command.Parameters.AddWithValue("@Date", txtDate.Text);

                int result = _command.ExecuteNonQuery();
                if (result == 1) lblNotification.Text = "Enregistrement modifiee avec succees";
                else lblNotification.Text = "Veuillez verifier les donnees que vous avez saisis";

                _connection.Close();
                _command = null;
                _reader = null;

                btnModifier.Text = "Modifier";
            }
            else
            {
                btnModifier.Text = "Enregistrer";
            }
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            _connection.Open();
            _command = new SqlCommand(_requeteDelete, _connection);
            _command.Parameters.AddWithValue("@Id", Convert.ToInt32(txtId.Text));

            int result = _command.ExecuteNonQuery();
            if (result == 1) lblNotification.Text = "Enregistrement supprimer avec succes.";
            else lblNotification.Text = "un probleme est survenus";

            _command = null;
            _connection.Close();
        }
    }
}
