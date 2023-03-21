using Hotel.Utilits;
using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Hotel.Employee
{
    public partial class AddEmployee : Form
    {
        private class CurrentData
        {
            public string FullName { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string Patronymic { get; set; }
            public int id_typeclient;
        };
        public AddEmployee()
        {
            InitializeComponent();
            Utility.SomeFunction someFunction = FillComboBox;
            Utility.ConnectToDataBase(someFunction);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Utility.SomeFunction someFunction = CollectDataOfClient;
            Utility.ConnectToDataBase(someFunction);
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void FillComboBox(OleDbCommand dbCommand)
        {
            dbCommand.CommandText = "SELECT TitlePositions FROM [Position]";

            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader())
            {
                while (dbDataReader.Read())
                {
                    comboBox1.Items.Add(dbDataReader.GetValue(0).ToString());
                }
                dbDataReader.Close();
            }
            comboBox1.SelectedIndex = -1;
        }

        private void ClearFields()
        {
            comboBox1.SelectedIndex = -1;
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }

        private void CollectDataOfClient(OleDbCommand dbCommand)
        {
            CurrentData currentData = new CurrentData();

            Utility.SomeFunctionWithReturn someFunction = GetSelectedTypeClient;
            currentData.id_typeclient = Convert.ToInt32(Utility.ConnectToDataBase(someFunction));

            currentData.LastName = textBox3.Text;
            currentData.FirstName = textBox4.Text;
            currentData.Patronymic = textBox5.Text;

            int countError = 0;
            if (currentData.LastName.Length == 0)
            {
                ++countError;
                errorProvider1.SetError(textBox3, "Заполните поле!");
                errorProvider1.SetIconAlignment(textBox3, ErrorIconAlignment.MiddleRight);
            }
            else
                errorProvider1.Clear();

            if (currentData.FirstName.Length == 0)
            {
                ++countError;
                errorProvider2.SetError(textBox4, "Заполните поле!");
                errorProvider2.SetIconAlignment(textBox4, ErrorIconAlignment.MiddleRight);
            }
            else
                errorProvider2.Clear();

            if (comboBox1.SelectedIndex == -1)
            {
                ++countError;
                errorProvider3.SetError(comboBox1, "Выберите должность!");
                errorProvider3.SetIconAlignment(comboBox1, ErrorIconAlignment.MiddleRight);
            } else
                errorProvider3.Clear();

            if (countError > 0) return;

            currentData.FullName = $"{currentData.LastName} {currentData.FirstName} {currentData.Patronymic}";
            dbCommand.CommandText = $" SELECT COUNT(*) " +
                                    $" FROM [Employees] " +
                                    $" WHERE Emp_FullName LIKE '%{currentData.FullName}' AND Emp_LastName LIKE '%{currentData.LastName}' AND " +
                                    $" Emp_FirstName LIKE '%{currentData.FirstName}'";

            int codeResult = 0;
            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader())
            {
                dbDataReader.Read();
                codeResult = Convert.ToInt32(dbDataReader.GetValue(0));
                if (codeResult >= 1)
                {
                    MessageBox.Show("Такой сотрудник уже есть.");
                    return;
                }
                dbDataReader.Close();
            }

            dbCommand.Parameters.Clear();
            dbCommand.CommandText = @"INSERT INTO [Employees](Emp_FullName, ref_ID_position, Emp_LastName, Emp_FirstName, Emp_Patronymic)
                                      VALUES(?, ?, ?, ?, ?)";

            dbCommand.Parameters.Add(new OleDbParameter("@Emp_FullName", OleDbType.VarChar)).Value = currentData.FullName;
            dbCommand.Parameters.Add(new OleDbParameter("@ref_ID_position", OleDbType.Numeric)).Value = currentData.id_typeclient;
            dbCommand.Parameters.Add(new OleDbParameter("@Emp_LastName", OleDbType.VarChar)).Value = currentData.LastName;
            dbCommand.Parameters.Add(new OleDbParameter("@Emp_FirstName", OleDbType.VarChar)).Value = currentData.FirstName;
            dbCommand.Parameters.Add(new OleDbParameter("@Emp_Patronymic", OleDbType.VarChar)).Value = currentData.Patronymic;

            codeResult = dbCommand.ExecuteNonQuery();
            string message = codeResult == 1 ? "Добавлен новый сотрудник!" : "Что-то пошло не так";
            System.Windows.Forms.MessageBox.Show(message);

            ClearFields();
        }

        private string GetSelectedTypeClient(OleDbCommand dbCommand)
        { 
            string selectedType = comboBox1.SelectedItem.ToString();
            dbCommand.CommandText = $"SELECT ID_position FROM [Position] WHERE TitlePositions = '{selectedType}'";
            string result;
            using (OleDbDataReader oleDbDataReader = dbCommand.ExecuteReader())
            {
                oleDbDataReader.Read();
                result = oleDbDataReader[0].ToString();
                oleDbDataReader.Close();
            }
            return result;
        }
    }
}
