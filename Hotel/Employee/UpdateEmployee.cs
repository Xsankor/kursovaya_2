using Hotel.Utilits;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Hotel.Employee
{
    public partial class UpdateEmployee : Form
    {
        private enum State
        {
            LOAD,
            UPDATE
        }
        private int currentEmployeeID;
        private struct CurrentData
        {
            public string FullName { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string Patronymic { get; set; }
            public int id_position;
        };

        private CurrentData currentData;
        public UpdateEmployee(int ID)
        {
            InitializeComponent();
            this.currentEmployeeID = ID;
            currentData = new CurrentData();
        }

        private void UpdateEmployee_Load(object sender, EventArgs e)
        {
            FormControl();
        }

        private void FormControl(State currentState = State.LOAD)
        {
            Utility.SomeFunction someFunction;
            if (currentState == State.UPDATE)
            {
                someFunction = UpdateEmployeeData;
            }
            else
            {
                someFunction = LoadEmployeeData;
            }
            Utility.ConnectToDataBase(someFunction);
        }

        private void LoadEmployeeData(OleDbCommand dbCommand)
        {
            Utility.SomeFunction someFunction = FillComboBox;
            Utility.ConnectToDataBase(someFunction);
            dbCommand.CommandText = @"SELECT ID_employee, TitlePositions, Emp_LastName, Emp_FirstName, Emp_Patronymic 
                                      FROM Employees INNER JOIN [Position] 
                                      ON Employees.ref_ID_position = [Position].ID_position
                                      WHERE ID_employee = " + currentEmployeeID;

            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader())
            {
                dbDataReader.Read();
                comboBox1.SelectedItem = dbDataReader["TitlePositions"].ToString();
                textBox3.Text = dbDataReader["Emp_LastName"].ToString();
                textBox4.Text = dbDataReader["Emp_FirstName"].ToString();
                textBox5.Text = dbDataReader["Emp_Patronymic"].ToString();
                dbDataReader.Close();
            }
        }

        private void UpdateEmployeeData(OleDbCommand dbCommand)
        {
            FillCurrentEmployeeData();
            dbCommand.CommandText = @"UPDATE Employees 
                                         SET Emp_FullName = @_FullName, ref_ID_position = @_ref_ID_position, Emp_LastName = @_LastName, 
                                             Emp_FirstName = @_FirstName, Emp_Patronymic = @_Patronymic
                                       WHERE ID_employee = @_currentEmployeeID";
            dbCommand.Parameters.Add(new OleDbParameter("@_FullName", OleDbType.VarChar)).Value = currentData.FullName;
            dbCommand.Parameters.Add(new OleDbParameter("@_ref_ID_position", OleDbType.Numeric)).Value = currentData.id_position;
            dbCommand.Parameters.Add(new OleDbParameter("@_LastName", OleDbType.VarChar)).Value = currentData.LastName;
            dbCommand.Parameters.Add(new OleDbParameter("@_FirstName", OleDbType.VarChar)).Value = currentData.FirstName;
            dbCommand.Parameters.Add(new OleDbParameter("@_Patronymic", OleDbType.VarChar)).Value = currentData.Patronymic;
            dbCommand.Parameters.Add(new OleDbParameter("@_currentEmployeeID", OleDbType.Numeric)).Value = currentEmployeeID;

            int codeResult = dbCommand.ExecuteNonQuery();
            string message = codeResult == 1 ? "Данные обновлены!" : "Что-то пошло не так";
            System.Windows.Forms.MessageBox.Show(message);
        }

        private void FillCurrentEmployeeData()
        {
            Utility.SomeFunction someFunction = FillComboBox;
            Utility.ConnectToDataBase(someFunction);

            currentData.LastName = textBox3.Text;
            currentData.FirstName = textBox4.Text;
            currentData.Patronymic = textBox5.Text;
            Utility.SomeFunctionWithReturn someFunctionForPosition = GetSelectedPosition;
            currentData.id_position = Convert.ToInt32(Utility.ConnectToDataBase(someFunctionForPosition));
            currentData.FullName = $"{currentData.LastName} {currentData.FirstName} {currentData.Patronymic}";
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
        }

        private void button2_Click(object sender, EventArgs e)
        {
            State currentState = State.UPDATE;
            FormControl(currentState);
            this.Close();
        }

        private string GetSelectedPosition(OleDbCommand dbCommand)
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
