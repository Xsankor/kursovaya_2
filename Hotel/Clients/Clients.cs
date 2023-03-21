using System;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using Hotel.Utilits;
using System.Linq;
using System.Collections.Generic;
using System.Data.Common;

namespace Hotel
{
    public partial class Clients : Form
    {
        private const int tableRowCount = 0; //26
        private const int tableColumnCount = 8;
        private int ID_value;
        private enum State 
        {
            SHOWTABLE,
            SEARCH,
            DELETE
        }
        public Clients()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Utility.SomeFunction fillComboBox = FillComboBox;
            Utility.ConnectToDataBase(fillComboBox);
            FormControl();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddClient addClinet = new AddClient();
            addClinet.ShowDialog();
            FormControl();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int selectedItems = dataGridView2.SelectedRows.Count;
            State currentState = State.DELETE;
            string textMessage = selectedItems == 1 ? "Вы уверены, что хотите удалить выбранного клиента?" :
                                                      "Вы уверены, что хотите удалить выбранных клиентов?";

            DialogResult result = MessageBox.Show(textMessage, "Уведомление", 
                                                  MessageBoxButtons.YesNo, MessageBoxIcon.Question, 
                                                  MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes) 
            {
                FormControl(currentState);
            }
            FormControl();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int update = dataGridView2.SelectedCells[0].RowIndex;
            if (dataGridView2.Rows[update].Cells[0].Value == null) return;
            int ID_value = (int)dataGridView2.Rows[update].Cells[0].Value;

            UpdateClient updateClient= new UpdateClient(ID_value);
            updateClient.ShowDialog();
            FormControl();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            State currentState = State.SEARCH;
            FormControl(currentState);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ReportClient reportClient = new ReportClient();
            reportClient.ShowDialog();
        }

        private void FormControl(State currentState = State.SHOWTABLE) 
        {
            Utility.SomeFunction someFunction;
            switch (currentState) 
            {
                case State.SEARCH:  someFunction = SearchClient;    break;
                case State.DELETE:  someFunction = DeleteRow;       break;
                default:            someFunction = ShowMainTable;   break;
            }
            Utility.ConnectToDataBase(someFunction);
        }

        private void ShowMainTable(OleDbCommand dbCommand) 
        {
            dbCommand.CommandText = "SELECT COUNT(*) FROM Сlients";
            int countData = Convert.ToInt32(dbCommand.ExecuteScalar());
            int tableRowCountCurrent = countData < tableRowCount ? tableRowCount : countData;
            dbCommand.CommandText = @"SELECT ID_client, FullName, TypeClient.TitleType, LastName, FirstName, Patronymic, FORMAT(DateOfBirth, 'dd.mm.yyyy'), PlaceOfResidence 
                                      FROM Сlients INNER JOIN TypeClient 
                                      ON Сlients.ref_ID_type_client = TypeClient.ID_type_client
                                      ORDER BY DateOfBirth                          
                                     ";

            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader()) 
            {
                dbDataReader.Read();
                dataGridView2.RowCount = tableRowCountCurrent;
                tableRowCountCurrent = dbDataReader.FieldCount;
                dataGridView2.ColumnCount = tableColumnCount;

                for (int i = 0; i < countData; ++i)
                {
                    for (int j = 0; j < tableColumnCount; ++j)
                    {
                        dataGridView2.Rows[i].Cells[j].Value = dbDataReader.GetValue(j);
                    }
                    dbDataReader.Read();
                }
                dbDataReader.Close();
            }
        }

        private void SearchClient(OleDbCommand dbCommand)
        {
            string resultFilter = CreateFilter();
            if (resultFilter.Length > 0 && resultFilter.Substring(resultFilter.Length - 3, 3) == "AND")
            {
                resultFilter = resultFilter.Remove(resultFilter.Length - 3);
            }
            dbCommand.CommandText = $"SELECT COUNT(*) FROM [Сlients] " + resultFilter;
            int countData = Convert.ToInt32(dbCommand.ExecuteScalar());

            string querryLine = @" SELECT ID_client, FullName, TypeClient.TitleType, LastName, FirstName, Patronymic, FORMAT(DateOfBirth, 'dd.MM.yyyy'), PlaceOfResidence
                                   FROM Сlients INNER JOIN TypeClient
                                   ON Сlients.ref_ID_type_client = TypeClient.ID_type_client " + resultFilter;

            dbCommand.CommandText = querryLine;

            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader())
            {
                dataGridView2.Rows.Clear();
                dbDataReader.Read();
                if (!dbDataReader.HasRows)
                {
                    MessageBox.Show("Нет данных");
                    dbDataReader.Close();
                    return;
                }

                dataGridView2.ColumnCount = tableColumnCount;
                dataGridView2.RowCount = countData;
                int i = 0;
                do
                {
                    if (i > countData) 
                    {
                        i = dataGridView2.Rows.Add();
                    }
                    for (int j = 0; j < tableColumnCount; ++j)
                    {
                        dataGridView2.Rows[i].Cells[j].Value = dbDataReader.GetValue(j);
                    }
                    ++i;
                } while (dbDataReader.Read());
                
                dbDataReader.Close();
            }
        }

        private void DeleteRow(OleDbCommand dbCommand) 
        {   
            List<int> listSelectedRoows = dataGridView2.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();

            listSelectedRoows.Sort();
            int delet;
            Utility.SomeFunctionForCheck someFunction = CanIDeleteData;
            for (int i = listSelectedRoows.Count - 1; i >= 0; --i) 
            {
                delet = listSelectedRoows[i];
                ID_value = (int)dataGridView2.Rows[delet].Cells[0].Value;
                if (Utility.ConnectToDataBase(someFunction)) continue;
                dbCommand.CommandText = "DELETE FROM Сlients WHERE ID_client = " + ID_value;
                dbCommand.ExecuteNonQuery();
            }
            dataGridView2.Rows.Clear();
        }

        private bool CanIDeleteData(OleDbCommand dbCommand) 
        {
            dbCommand.CommandText = "SELECT COUNT(*) FROM [Orders] WHERE ref_ID_client = " + ID_value;
            bool result;
            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader()) 
            {
                dbDataReader.Read();
                result = Convert.ToInt32(dbDataReader[0]) > 0 ? true : false;
                dbDataReader.Close();
            }
            if(result) MessageBox.Show($"У клиента есть запись в заказах.");
            return result;
        }

        private string CreateFilter() 
        {
            string fullName = textBox1.Text ?? "";
            int selectedTypeClient = comboBox1.SelectedIndex + 1;
            string lastName = textBox3.Text;
            string firstName = textBox4.Text;
            string patronymic = textBox5.Text;
            DateTime dateOfBirth = dateTimePicker1.Value.Date;

            StringBuilder filter = new StringBuilder("");
            filter.Append(fullName == "" ? "" : $" FullName LIKE '{fullName}%' AND");
            filter.Append(selectedTypeClient == 0 ? "" : $" ref_ID_type_client = {selectedTypeClient} AND");
            filter.Append(lastName == "" ? "" : $" LastName LIKE '{lastName}%' AND");
            filter.Append(firstName == "" ? "" : $" FirstName LIKE '{firstName}%' AND");
            filter.Append(patronymic == "" ? "" : $" Patronymic LIKE '{patronymic}%' AND");
            filter.Append(!Utility.IsAdult(dateOfBirth) ? "" : $" DateOfBirth = #{dateOfBirth.ToString("MM.dd.yyyy").Replace('.', '/')}#");
            filter.Insert(0, (filter.Length > 0 ? "WHERE" : ""));

            return filter.ToString();
        }

        private void FillComboBox(OleDbCommand dbCommand)
        {
            dbCommand.CommandText = "SELECT TitleType FROM TypeClient";

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
