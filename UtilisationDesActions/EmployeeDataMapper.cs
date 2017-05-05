using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilisationDesActions
{
    public class EmployeeDataMapper : IDisposable
    {
        private string _connectionString;

        public Func<int, Dictionary<string, object>> SelectEmployee; // { get; set; }
        public Func<Dictionary<string, object>, dynamic> UpdateEmployee; // { get; set; }
        public Func<Dictionary<string, object>, dynamic> InsertEmployee; // { get; set; }
        public Func<int, dynamic> DeleteEmployee; // { get; set; }

        public EmployeeDataMapper()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

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
                return new { Id = id, Notification = notification };
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
                        command.Parameters.AddWithValue("@Id", id);

                        result = command.ExecuteNonQuery() == 1;
                        if (result) notification = "Enregistrement supprimer avec succes.";
                        else notification = "un probleme est survenus";
                    }
                }

                return new { Success = result, Notification = notification };
            };
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~EmployeeDataMapper() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
