using Hotel.Utilits;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

using System.Linq;

using System.Windows.Forms;

namespace Hotel.Orders
{
    public partial class ListOfServicesRendered : Form
    {
        private enum StatePage 
        {
            SHOWTABLE,
            DELETE
        }
        private const int tableRowCount = 0; //17;
        private const int tableColumnCount = 4;
        private int currentOrderID;
        public ListOfServicesRendered(int ID_order)
        {
            InitializeComponent();
            currentOrderID = ID_order;
        }

        private void ListOfServicesRendered_Load(object sender, EventArgs e)
        {
            FormControl();
        }

        private void FormControl(StatePage currentState = StatePage.SHOWTABLE)
        {
            Utility.SomeFunction someFunction;
            switch (currentState)
            {
                case StatePage.DELETE: someFunction = DeleteRow; break;
                default: someFunction = ShowMainTable; break;
            }
            Utility.ConnectToDataBase(someFunction);
        }

        private void ShowMainTable(OleDbCommand dbCommand)
        {
            dbCommand.CommandText = $"SELECT COUNT(*) FROM [ListOfServicesRendered] " +
                                    $"WHERE [ListOfServicesRendered].reference_ID_order = {currentOrderID}";
            int countData = Convert.ToInt32(dbCommand.ExecuteScalar());

            int tableRowCountCurrent = countData < tableRowCount ? tableRowCount : countData;
            dbCommand.CommandText = @"
                                      SELECT ID, NameService, FORMAT(DateService, 'dd.mm.yyyy') , Emp_FullName
                                      FROM ((([ListOfServicesRendered] 
                                      INNER JOIN [Services] ON [ListOfServicesRendered].reference_ID_services = [Services].ID_services)
                                      INNER JOIN [Employees] ON [ListOfServicesRendered].reference_ID_employee = [Employees].ID_employee)
                                      INNER JOIN [Orders] ON [ListOfServicesRendered].reference_ID_order = [Orders].ID_order)
                                      WHERE [Orders].ID_order = " + currentOrderID;

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

        private void button4_Click(object sender, EventArgs e)
        {
            AddRenderedService addRenderedService = new AddRenderedService(currentOrderID);
            addRenderedService.ShowDialog();
            FormControl();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int selectedItems = dataGridView1.SelectedRows.Count;
            StatePage currentState = StatePage.DELETE;
            string textMessage = selectedItems == 1 ? "Вы уверены, что хотите удалить оказанную услугу?" :
                                                      "Вы уверены, что хотите удалить выбранные оказанные услуги?";

            DialogResult result = MessageBox.Show(textMessage, "Уведомление",
                                                  MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                                  MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                FormControl(currentState);
            }
            FormControl();
        }

        private void DeleteRow(OleDbCommand dbCommand)
        {
            List<int> listSelectedRoows = dataGridView1.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();

            listSelectedRoows.Sort();
            int delet, ID_value;
            for (int i = listSelectedRoows.Count - 1; i >= 0; --i)
            {
                delet = listSelectedRoows[i];
                ID_value = (int)dataGridView1.Rows[delet].Cells[0].Value;
                dbCommand.CommandText = $"DELETE * FROM ListOfServicesRendered WHERE ID = {ID_value}";
                dbCommand.ExecuteNonQuery();
            }
            dataGridView1.Rows.Clear();
        }
    }
}
