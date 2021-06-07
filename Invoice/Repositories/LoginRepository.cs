using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using Invoice.Models;

namespace Invoice.Repositories
{
    public class LoginRepository
    {
        private const int PasswordId = 1;

        public IEnumerable<string> GetPassword()
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getPasswordQuery = 
                    $@"SELECT
                        P.Password
                      FROM Password P
                      WHERE Id = {PasswordId}
                    ";

                IEnumerable<string> getPassword = dbConnection.Query<string>(getPasswordQuery);

                return getPassword;
            }
        }

        public IEnumerable<string> GetIsPasswordCorrect()
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string getIsPasswordCorrectQuery =
                    $@"SELECT
                        P.IsPasswordCorrect
                      FROM Password P
                      WHERE Id = {PasswordId}
                    ";

                IEnumerable<string> getIsPasswordCorrect = dbConnection.Query<string>(getIsPasswordCorrectQuery);

                return getIsPasswordCorrect;
            }
        }

        public bool ChangePassword(PasswordModel password)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string changePasswordQuery =
                   $@"UPDATE 'Password'
                        SET Password = '{password.Password}'
                      WHERE Id = {PasswordId}
                   ";

                int affectedRows = dbConnection.Execute(changePasswordQuery);

                return affectedRows == 1;
            }
        }

        public void ChangeIsPasswordCorrect(bool IsPasswordCorrect)
        {
            using (var dbConnection = new SQLiteConnection(AppConfiguration.ConnectionString))
            {
                dbConnection.Open();

                string changeIsPasswordCorrectQuery = 
                    $@"UPDATE 'Password'
                        SET IsPasswordCorrect = '{IsPasswordCorrect}'
                      WHERE Id = {PasswordId}
                    ";

                dbConnection.Execute(changeIsPasswordCorrectQuery);
            }
        }
    }
}
