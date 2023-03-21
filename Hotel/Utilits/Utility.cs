using System;
using System.Configuration;
using System.Data.Common;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Hotel.Utilits
{
    public static class Utility
    {
        public enum StateShowList 
        {
            ORDER,
            SERVICE
        }

        public static StateShowList stateShowList;

        public static int selected_ID_Client = -1;
        public static int selected_ID_Employee = -1;
        public static int selected_ID_Room = -1;

        public static int selected_ID_Country = -1;
        public static int selected_ID_Region = -1;
        public static int selected_ID_City = -1;
        public static int selected_ID_Street = -1;

        public delegate void SomeFunction(OleDbCommand dbCommand);
        public delegate string SomeFunctionWithReturn(OleDbCommand dbCommand);
        public delegate bool SomeFunctionForCheck(OleDbCommand dbCommand);

        public static void ConnectToDataBase(SomeFunction fn) 
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            using (OleDbCommand dbCommand = connection.CreateCommand())
            {
                connection.Open();
                fn(dbCommand);
                connection.Close();
            }
        }

        public static string ConnectToDataBase(SomeFunctionWithReturn fn)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
            string result;
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            using (OleDbCommand dbCommand = connection.CreateCommand())
            {
                connection.Open();
                result = fn(dbCommand);
                connection.Close();
            }

            return result;
        }

        public static bool ConnectToDataBase(SomeFunctionForCheck fn)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
            bool result;
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            using (OleDbCommand dbCommand = connection.CreateCommand())
            {
                connection.Open();
                result = fn(dbCommand);
                connection.Close();
            }

            return result;
        }
        public static bool IsAdult(DateTime dateOfBirth)
        {
            return CalculateAge(dateOfBirth) >= 18;
        }
        private static int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;

            int age = today.Year - birthDate.Year;
            if (birthDate.AddYears(age).Date > today.Date)
            {
                --age;
            }
            return age;
        }

        public static void SetEntityTable(ref DataGridView dataGridView, int row, int column) 
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; ++j)
                {
                    if (j == column - 1) 
                    {
                        dataGridView.Rows[i].Cells[j] = new DataGridViewTextBoxCell();
                    }
                    dataGridView.Rows[i].Cells[j].Value = "";
                }
            }
        }
    }
}
