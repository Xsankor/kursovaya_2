using Hotel.Utilits;
using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Hotel.Orders
{
    public partial class AddRenderedService : Form
    {
        private class CurrentData
        {
            public string ServiceName { get; set; }
            public string Emp_FullName { get; set; }
            
            public DateTime DateService;
            public int id_typeservice;
        };
        private int id_order, id_employee, id_client;
        public AddRenderedService(int ID_order)
        {
            InitializeComponent();
            Utility.SomeFunction someFunction = FillComboBox;
            Utility.ConnectToDataBase(someFunction);
            id_order = ID_order;
        }

        //Обзор
        private void button5_Click(object sender, EventArgs e)
        {
            ListEmployees listEmployees = new ListEmployees(Utility.StateShowList.SERVICE);
            listEmployees.ShowDialog();
            Utility.SomeFunction someFunction = GetSelectedData;
            Utility.ConnectToDataBase(someFunction);
        }
        //Очистить поля
        private void button3_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
        //Сохранить
        private void button2_Click(object sender, EventArgs e)
        {
            Utility.SomeFunction someFunction = GetCurrentClient;
            Utility.ConnectToDataBase(someFunction);
            someFunction = CollectData;
            Utility.ConnectToDataBase(someFunction);
        }

        private void GetSelectedData(OleDbCommand dbCommand)
        {
            string textResult = "";
            id_employee = Utility.selected_ID_Employee;
            if (id_employee < 0) return;
            dbCommand.CommandText = $"SELECT Emp_FullName FROM Employees WHERE ID_employee = {id_employee}";
            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader())
            {
                dbDataReader.Read();
                textResult = dbDataReader["Emp_FullName"].ToString();
            }
            textBox2.Text = textResult;
        }

        private void FillComboBox(OleDbCommand dbCommand)
        {
            dbCommand.CommandText = "SELECT NameService FROM Services";

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

        private void CollectData(OleDbCommand dbCommand)
        {
            CurrentData currentData = new CurrentData();

            Utility.SomeFunctionWithReturn someFunction = GetSelectedService;
            currentData.id_typeservice = Convert.ToInt32(Utility.ConnectToDataBase(someFunction));
            currentData.Emp_FullName = textBox2.Text;
            currentData.DateService = dateTimePicker1.Value;

            int countError = 0;
            if (currentData.Emp_FullName.Length == 0)
            {
                ++countError;
                errorProvider1.SetError(textBox2, "Заполните поле!");
                errorProvider1.SetIconAlignment(textBox2, ErrorIconAlignment.MiddleRight);
            }
            else
                errorProvider1.Clear();

            if (comboBox1.SelectedIndex < 0)
            {
                ++countError;
                errorProvider2.SetError(comboBox1, "Заполните поле!");
                errorProvider2.SetIconAlignment(comboBox1, ErrorIconAlignment.MiddleRight);
            }
            else
                errorProvider2.Clear();

            if (countError > 0) return;

            dbCommand.Parameters.Clear();
            dbCommand.CommandText = @"INSERT INTO ListOfServicesRendered(reference_ID_order, DateService, 
                                                                        reference_ID_client, reference_ID_employee, 
                                                                        reference_ID_services)
                                      VALUES(?, ?, ?, ?, ?)";

            dbCommand.Parameters.Add(new OleDbParameter("@reference_ID_order", OleDbType.Numeric)).Value = id_order;
            dbCommand.Parameters.Add(new OleDbParameter("@DateService", OleDbType.DBDate)).Value = currentData.DateService;
            dbCommand.Parameters.Add(new OleDbParameter("@reference_ID_client", OleDbType.VarChar)).Value = id_client;
            dbCommand.Parameters.Add(new OleDbParameter("@reference_ID_employee", OleDbType.VarChar)).Value = id_employee;
            dbCommand.Parameters.Add(new OleDbParameter("@reference_ID_services", OleDbType.VarChar)).Value = currentData.id_typeservice;

            int codeResult = dbCommand.ExecuteNonQuery();
            string message = codeResult == 1 ? "Услуга оказана!" : "Что-то пошло не так";
            System.Windows.Forms.MessageBox.Show(message);

            ClearFields();
        }

        private void ClearFields() 
        {
            comboBox1.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now;
            textBox2.Clear();
        }

        private void GetCurrentClient(OleDbCommand dbCommand)
        {
            dbCommand.CommandText = $"SELECT ref_ID_client FROM Orders WHERE ID_order = {id_order}";
            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader())
            {
                dbDataReader.Read();
                id_client = Convert.ToInt32(dbDataReader["ref_ID_client"]);
                dbDataReader.Close();
            }
        }

        private string GetSelectedService(OleDbCommand dbCommand)
        {
            string selectedType = comboBox1.SelectedItem.ToString();
            dbCommand.CommandText = $"SELECT ID_services FROM [Services] WHERE NameService = '{selectedType}'";
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
