using System;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using Hotel.Utilits;
using System.Collections.Generic;
using System.Linq;
using Hotel.Orders;

namespace Hotel.Employee
{
    
    public partial class Employees : Form
    {
        private int ID_value;
        private const int tableRowCount = 0; //26
        private const int tableColumnCount = 6;
        private enum State
        {
            SHOWTABLE,
            SEARCH,
            DELETE
        }
        public Employees()
        {
            InitializeComponent();
        }

        private void Employees_Load(object sender, EventArgs e)
        {
            Utility.SomeFunction fillComboBox = FillComboBox;
            Utility.ConnectToDataBase(fillComboBox);
            State currentState = State.SHOWTABLE;
            FormControl(currentState);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddEmployee addEmployee = new AddEmployee();
            addEmployee.ShowDialog();
            FormControl();
        }

        private void FormControl(State currentState = State.SHOWTABLE)
        {
            Utility.SomeFunction someFunction;
            switch (currentState)
            {
                case State.SEARCH: someFunction = SearchEmployee; break;
                case State.DELETE: someFunction = DeleteRow; break;
                default: someFunction = ShowMainTable; break;
            }
            Utility.ConnectToDataBase(someFunction);
        }

        private void ShowMainTable(OleDbCommand dbCommand)
        {
            dbCommand.CommandText = "SELECT COUNT(*) FROM [Employees]";
            int countData = Convert.ToInt32(dbCommand.ExecuteScalar());
            int tableRowCountCurrent = countData < tableRowCount ? tableRowCount : countData;
            dbCommand.CommandText = @"
                                      SELECT ID_employee, Emp_FullName, [Position].TitlePositions, 
                                             Emp_LastName, Emp_FirstName, Emp_Patronymic 
                                      FROM [Employees] INNER JOIN [Position] 
                                      ON Employees.ref_ID_position = [Position].ID_position
                                     ";
            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader())
            {
                dbDataReader.Read();
                dataGridView1.RowCount = tableRowCountCurrent;
                dataGridView1.ColumnCount = tableColumnCount;

                for (int i = 0; i < countData; ++i)
                {
                    for (int j = 0; j < tableColumnCount; ++j)
                    {
                        dataGridView1.Rows[i].Cells[j].Value = dbDataReader.GetValue(j);
                    }
                    dbDataReader.Read();
                }
                dbDataReader.Close();
            }
        }

        private void SearchEmployee(OleDbCommand dbCommand)
        {
            string resultFilter = CreateFilter();

            if (resultFilter.Length > 0 && resultFilter.Substring(resultFilter.Length - 3, 3) == "AND")
            {
                resultFilter = resultFilter.Remove(resultFilter.Length - 3);
            }

            dbCommand.CommandText = $"SELECT COUNT(*) FROM [Employees] " + resultFilter;
            int countData = Convert.ToInt32(dbCommand.ExecuteScalar());            

            string querryLine = @" SELECT ID_employee, Emp_FullName, TitlePositions, Emp_LastName, Emp_FirstName, Emp_Patronymic
                                   FROM Employees INNER JOIN [Position]
                                   ON Employees.ref_ID_position = [Position].ID_position " + resultFilter;

            dbCommand.CommandText = querryLine;

            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader())
            {
                dataGridView1.Rows.Clear();
                dbDataReader.Read();
                if (!dbDataReader.HasRows)
                {
                    MessageBox.Show("Нет данных");
                    dbDataReader.Close();
                    return;
                }

                dataGridView1.ColumnCount = tableColumnCount;
                dataGridView1.RowCount = countData;
                int i = 0;
                do
                {
                    if (i > countData)
                    {
                        i = dataGridView1.Rows.Add();
                    }
                    for (int j = 0; j < tableColumnCount; ++j)
                    {
                        dataGridView1.Rows[i].Cells[j].Value = dbDataReader.GetValue(j);
                    }
                    ++i;
                } while (dbDataReader.Read());

                dbDataReader.Close();
            }
        }

        private void DeleteRow(OleDbCommand dbCommand)
        {
            List<int> listSelectedRoows = dataGridView1.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();

            listSelectedRoows.Sort();
            int delet;
            Utility.SomeFunctionForCheck someFunction = CanIDeleteData; 
            for (int i = listSelectedRoows.Count - 1; i >= 0; --i)
            {
                delet = listSelectedRoows[i];
                ID_value = (int)dataGridView1.Rows[delet].Cells[0].Value;
                if (Utility.ConnectToDataBase(someFunction)) continue;
                dbCommand.CommandText = "DELETE FROM Employees WHERE ID_employee = " + ID_value;
                dbCommand.ExecuteNonQuery();
            }
            dataGridView1.Rows.Clear();
        }
        private bool CanIDeleteData(OleDbCommand dbCommand)
        {
            dbCommand.CommandText = "SELECT COUNT(*) FROM [Orders] WHERE ref_ID_employee = " + ID_value;
            bool result;
            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader())
            {
                dbDataReader.Read();
                result = (int)dbDataReader[0] > 0 ? true : false;
                MessageBox.Show($"У сотрудника есть запись в заказах.");
                dbDataReader.Close();
            }
            return result;
        }
        private string CreateFilter()
        {
            string fullName = textBox1.Text ?? "";
            int selectedPosition = comboBox1.SelectedIndex + 1;
            string lastName = textBox3.Text;
            string firstName = textBox4.Text;
            string patronymic = textBox5.Text;

            StringBuilder filter = new StringBuilder("");
            filter.Append(fullName == "" ? "" : $"Emp_FullName LIKE '{fullName}%' AND");
            filter.Append(selectedPosition == 0 ? "" : $" ref_ID_position = {selectedPosition} AND");
            filter.Append(lastName == "" ? "" : $" Emp_LastName LIKE '{lastName}%' AND");
            filter.Append(firstName == "" ? "" : $" Emp_FirstName LIKE '{firstName}%' AND");
            filter.Append(patronymic == "" ? "" : $" Emp_Patronymic LIKE '{patronymic}%' AND");
            filter.Insert(0, (filter.Length > 0 ? "WHERE " : ""));

            return filter.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int update = dataGridView1.SelectedCells[0].RowIndex;

            int ID_value = (int)dataGridView1.Rows[update].Cells[0].Value;

            UpdateEmployee updateEmployee = new UpdateEmployee(ID_value);
            updateEmployee.ShowDialog();
            FormControl();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            State currentState = State.DELETE;
            DialogResult result = System.Windows.Forms.MessageBox.Show("Вы уверены, что нужно удалить выбранную строку?", "Уведомление",
                                                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                                                        MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            if (result == DialogResult.Yes)
            {
                FormControl(currentState);
            }
            FormControl();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            State currentState = State.SEARCH;
            FormControl(currentState);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ReportEmployee reportEmployee = new ReportEmployee();
            reportEmployee.ShowDialog();
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
    }
}
