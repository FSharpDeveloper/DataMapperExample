using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UtilisationDesActions
{
    public partial class Employee : Form
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            //@"Data Source=.\SQLExpress;Initial Catalog=ADONetCourse;Integrated Security=true";

        private string _requeteSelect = "Select Id,FirstName,LastName,Addresse,Date From Employee Where Id = @Id";
        private string _requeteInsert = "Insert Into Employee (FirstName, LastName, Addresse, Date) Values (@FirstName,@LastName,@Addresse,@Date)";
        private string _requeteUpdate = "Update Employee Set FirstName=@FirstName, LastName=@LastName, Addresse=@Addresse, Date=@Date Where Id = @ID";
        private string _requeteDelete = "Delete From Employee Where Id = @Id";

        private bool _modifiable;
        private bool _supprimable;
        private bool _ajoutable;

        private Action SelectEmployee;
        private Action UpdateEmployee;
        private Action InsertEmployee;
        private Action DeleteEmployee;

        public Employee()
        {
            InitializeComponent();

            _modifiable = false;
            _supprimable = false;
            _ajoutable = false;

            SelectEmployee = () =>
            {

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(_requeteSelect, connection))
                    {
                        command.Parameters.AddWithValue("@Id", Convert.ToInt32(txtChercher.Text));
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    txtId.Text = Convert.ToString(reader["Id"]);
                                    txtFirstName.Text = Convert.ToString(reader["FirstName"]);
                                    txtLastName.Text = Convert.ToString(reader["LastName"]);
                                    txtAddresse.Text = Convert.ToString(reader["Addresse"]);
                                    txtDate.Text = Convert.ToString(reader["Date"]);
                                }
                            else lblNotification.Text = "Aucun enregistrement ne correspond au critere votre recherche.";
                        } // == reader.Dispose(); // la methode dispose libere toutes les resources memoire utiliser pas l'objet.
                    }
                } // == connection.Dispose(); // dans le cas d'un objet de connect "SqlConnection,OleDbConnection, ..." connection.Close()
                  // est appeler dans la methode Dispose() .

            };

            UpdateEmployee = () =>
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(_requeteUpdate, connection))
                    {
                        command.Parameters.AddWithValue("@Id", Convert.ToInt32(txtId.Text));
                        command.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                        command.Parameters.AddWithValue("@LastName", txtLastName.Text);
                        command.Parameters.AddWithValue("@Addresse", txtAddresse.Text);
                        command.Parameters.AddWithValue("@Date", txtDate.Text);


                        if (command.ExecuteNonQuery() == 1) lblNotification.Text = "Enregistrement modifiee avec succees";
                        else lblNotification.Text = "Veuillez verifier les donnees que vous avez saisis";
                    }
                }
            };

            InsertEmployee = () => 
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(_requeteInsert, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", txtFirstName.Text); // l'utilisation des parametres diminue les vas d'erreurs 
                        command.Parameters.AddWithValue("@LastName", txtLastName.Text);   // et lors du declenchement d'une erreur on peux la localiser 
                        command.Parameters.AddWithValue("@Addresse", txtAddresse.Text);   // rapidement et facilement.
                        command.Parameters.AddWithValue("@Date", txtDate.Text);

                        if (command.ExecuteNonQuery() == 1) lblNotification.Text = "Enregistrement Ajouter avec Succes."; // la verification du resultat de la methode ExecuteNonQuery()
                        else lblNotification.Text = "Veuiller verifier la validite des donnees saisis.";                  // permet de savoir si l'operation c'est derouler avec succes 
                                                                                                                          // la valeur de retour represente le nombre de colonnes affectes.
                        command.CommandText = "Select TOP 1 Id From Employee Order By Id Desc"; // cette requette permet de recuperer le dernier identifiant dans la table.

                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                txtId.Text = Convert.ToString(reader["Id"]);
                            }
                    }
                }
            };

            DeleteEmployee = () =>
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(_requeteDelete, connection))
                    {
                        command.Parameters.AddWithValue("@Id", Convert.ToInt32(txtId.Text));

                        if (command.ExecuteNonQuery() == 1) lblNotification.Text = "Enregistrement supprimer avec succes.";
                        else lblNotification.Text = "un probleme est survenus";
                    }
                }
            };
        }

        private void txtChercher_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)                
            {

                //SelectEmployee();
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(_requeteSelect, connection))
                    {
                        command.Parameters.AddWithValue("@Id", Convert.ToInt32(txtChercher.Text));
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    txtId.Text = Convert.ToString(reader["Id"]);
                                    txtFirstName.Text = Convert.ToString(reader["FirstName"]);
                                    txtLastName.Text = Convert.ToString(reader["LastName"]);
                                    txtAddresse.Text = Convert.ToString(reader["Addresse"]);
                                    txtDate.Text = Convert.ToString(reader["Date"]);
                                }
                            else lblNotification.Text = "Aucun enregistrement ne correspond au critere votre recherche.";
                        } // == reader.Dispose(); // la methode dispose libere toutes les resources memoire utiliser pas l'objet.
                    }
                } // == connection.Dispose(); // dans le cas d'un objet de connect "SqlConnection,OleDbConnection, ..." connection.Close()
                  // est appeler dans la methode Dispose() .
            }
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            if(btnAjouter.Text == "Enregistrer")
            {
                InsertEmployee();
                //using(var connection = new SqlConnection(_connectionString))
                //{
                //    connection.Open();
                //    using(var command = new SqlCommand(_requeteInsert, connection))
                //    {
                //        command.Parameters.AddWithValue("@FirstName", txtFirstName.Text); // l'utilisation des parametres diminue les vas d'erreurs 
                //        command.Parameters.AddWithValue("@LastName", txtLastName.Text);   // et lors du declenchement d'une erreur on peux la localiser 
                //        command.Parameters.AddWithValue("@Addresse", txtAddresse.Text);   // rapidement et facilement.
                //        command.Parameters.AddWithValue("@Date", txtDate.Text);

                //        if (command.ExecuteNonQuery() == 1) lblNotification.Text = "Enregistrement Ajouter avec Succes."; // la verification du resultat de la methode ExecuteNonQuery()
                //        else lblNotification.Text = "Veuiller verifier la validite des donnees saisis.";                  // permet de savoir si l'operation c'est derouler avec succes 
                //                                                                                                          // la valeur de retour represente le nombre de colonnes affectes.
                //        command.CommandText = "Select TOP 1 Id From Employee Order By Id Desc"; // cette requette permet de recuperer le dernier identifiant dans la table.

                //        using (var reader = command.ExecuteReader())
                //            while (reader.Read())
                //            {
                //                txtId.Text = Convert.ToString(reader["Id"]);
                //            }
                //    }
                //}

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
                UpdateEmployee();
                //using (var connection = new SqlConnection(_connectionString))
                //{
                //    connection.Open();
                //    using (var command = new SqlCommand(_requeteUpdate, connection))
                //    {
                //        command.Parameters.AddWithValue("@Id", Convert.ToInt32(txtId.Text));
                //        command.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                //        command.Parameters.AddWithValue("@LastName", txtLastName.Text);
                //        command.Parameters.AddWithValue("@Addresse", txtAddresse.Text);
                //        command.Parameters.AddWithValue("@Date", txtDate.Text);

                        
                //        if (command.ExecuteNonQuery() == 1) lblNotification.Text = "Enregistrement modifiee avec succees";
                //        else lblNotification.Text = "Veuillez verifier les donnees que vous avez saisis";
                //    }
                //}

                btnModifier.Text = "Modifier";
            }
            else
            {
                btnModifier.Text = "Enregistrer";
            }
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            DeleteEmployee();
            //using (var connection = new SqlConnection(_connectionString))
            //{
            //    connection.Open();
            //    using (var command = new SqlCommand(_requeteDelete, connection))
            //    {
            //        command.Parameters.AddWithValue("@Id", Convert.ToInt32(txtId.Text));
                    
            //        if (command.ExecuteNonQuery() == 1) lblNotification.Text = "Enregistrement supprimer avec succes.";
            //        else lblNotification.Text = "un probleme est survenus";
            //    }
            //}
        }
    }
}
