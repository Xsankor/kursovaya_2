using Hotel.Employee;
using Hotel.Utilits;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Diagnostics.PerformanceData;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Hotel.Orders
{
    public partial class Orders : Form
    {
        private const int tableRowCount = 0;//26;
        private const int tableColumnCount = 7;
        private enum State
        {
            SHOWTABLE,
            SEARCH,
            DELETE
        }
        public Orders()
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void Orders_Load(object sender, EventArgs e)
        {
            State currentState = State.SHOWTABLE;
            FormControl(currentState);
        }

        private void FormControl(State currentState = State.SHOWTABLE)
        {
            Utility.SomeFunction someFunction;
            switch (currentState)
            {
                case State.SEARCH: someFunction = SearchOrders; break;
                case State.DELETE: someFunction = DeleteRow; break;
                default: someFunction = ShowMainTable; break;
            }
            Utility.ConnectToDataBase(someFunction);
        }

        private void FillTable(OleDbCommand dbCommand, int countData, int tableRowCountCurrent)
        {
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

                dataGridView1.RowCount = tableRowCountCurrent;
                dataGridView1.ColumnCount = tableColumnCount;
                
                for (int i = 0; i < countData; ++i)
                {
                    for (int j = 0; j < tableColumnCount; ++j)
                    {
                        if (j == tableColumnCount - 1)
                        {
                            dataGridView1.Rows[i].Cells[j] = new DataGridViewButtonCell();
                            dataGridView1.Rows[i].Cells[j].Value = "Просмотр";
                        }
                        else
                        {
                            dataGridView1.Rows[i].Cells[j].Value = dbDataReader.GetValue(j);
                        }
                    }
                    dbDataReader.Read();
                }
                dbDataReader.Close();
            }
        }

        private void ShowMainTable(OleDbCommand dbCommand)
        {
            dbCommand.CommandText = "SELECT COUNT(*) FROM [Orders]";
            int countData = Convert.ToInt32(dbCommand.ExecuteScalar());
            
            int tableRowCountCurrent = countData < tableRowCount ? tableRowCount : countData;
            dbCommand.CommandText = @"
                                        SELECT ID_order, FullName, Format([ArrivalDate],'dd/mm/yyyy') + ' ' + Format([ArrivalTime], 'hh:MM'), 
                                               Format([DepartureOfDate],'dd/mm/yyyy') + ' ' + Format([DepartureTime], 'hh:MM'), RoomNumber, Emp_FullName, ID_order 
                                        FROM ((([Orders] 
                                        INNER JOIN Сlients ON Orders.ref_ID_client = Сlients.ID_client)
                                        INNER JOIN [Employees] ON [Orders].ref_ID_employee = [Employees].ID_employee)
                                        INNER JOIN [Rooms] ON [Orders].ref_ID_room = [Rooms].ID_room)
                                     ";
            if (countData == 0)
            {
                Utility.SetEntityTable(ref dataGridView1, tableRowCount, tableColumnCount);
            }
            else 
            {
                FillTable(dbCommand, countData, tableRowCountCurrent);
            }
            
        }

        private void DeleteRow(OleDbCommand dbCommand)
        {
            List<int> listSelectedRoows = dataGridView1.SelectedRows.Cast<DataGridViewRow>().Select(x => x.Index).ToList();

            listSelectedRoows.Sort();
            int delet, ID_value, value;
            for (int i = listSelectedRoows.Count - 1; i >= 0; --i)
            {
                dbCommand.Parameters.Clear();
                delet = listSelectedRoows[i];
                if (dataGridView1.Rows[delet].Cells[0].Value == null) continue;

                value = (int)dataGridView1.Rows[delet].Cells[4].Value;
                dbCommand.CommandText = $"UPDATE Rooms SET RoomIsBusy = @_isBusy WHERE RoomNumber = {value}";
                dbCommand.Parameters.Add(new OleDbParameter("@_isBusy", OleDbType.Boolean)).Value = false;
                dbCommand.ExecuteNonQuery();

                ID_value = (int)dataGridView1.Rows[delet].Cells[0].Value;
                dbCommand.CommandText = "DELETE * FROM Orders WHERE ID_order = " + ID_value;
                dbCommand.ExecuteNonQuery();
            }
            dataGridView1.Rows.Clear();
        }

        private void SearchOrders(OleDbCommand dbCommand)
        {
            string resultFilter = CreateFilter();

            if (resultFilter.Length > 0 && resultFilter.Substring(resultFilter.Length - 3, 3) == "AND")
            {
                resultFilter = resultFilter.Remove(resultFilter.Length - 3);
            }
            dbCommand.CommandText = $"SELECT COUNT(*) FROM [Orders] " + resultFilter;
            int countData = Convert.ToInt32(dbCommand.ExecuteScalar());
            int tableRowCountCurrent = countData < tableRowCount ? tableRowCount : countData;
            string querryLine = @"
                                   SELECT ID_order, FullName, ArrivalDate, DepartureOfDate, RoomNumber, Emp_FullName, ID_order  
                                   FROM ((([Orders] 
                                   INNER JOIN Сlients ON Orders.ref_ID_client = Сlients.ID_client)
                                   INNER JOIN [Employees] ON [Orders].ref_ID_employee = [Employees].ID_employee)
                                   INNER JOIN [Rooms] ON [Orders].ref_ID_room = [Rooms].ID_room)
                                 " + resultFilter;

            dbCommand.CommandText = querryLine;

            FillTable(dbCommand, countData, tableRowCountCurrent);
        }

        private string CreateFilter()
        {
            DateTime arrivalDate = dateTimePicker1.Value.Date;

            StringBuilder filter = new StringBuilder("");
            filter.Append($" ArrivalDate = #{arrivalDate.ToString("MM.dd.yyyy").Replace('.', '/')}#");
            filter.Insert(0, (filter.Length > 0 ? " WHERE " : ""));

            return filter.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            State currentState = State.SEARCH;
            FormControl(currentState);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddOrder addOrder= new AddOrder();
            addOrder.ShowDialog();
            FormControl();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int update = dataGridView1.SelectedCells[0].RowIndex;

            int ID_value = (int)dataGridView1.Rows[update].Cells[0].Value;

            UpdateOrder updateEmployee = new UpdateOrder(ID_value);
            updateEmployee.ShowDialog();
            FormControl();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == tableColumnCount - 1)
            {
                int ID_currentOrder = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                ListOfServicesRendered listOfServicesRendered = new ListOfServicesRendered(ID_currentOrder);
                listOfServicesRendered.ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int selectedItems = dataGridView1.SelectedRows.Count;
            State currentState = State.DELETE;
            string textMessage = selectedItems == 1 ? "Вы уверены, что хотите удалить заселенного клиента?" :
                                                      "Вы уверены, что хотите удалить выбранных заселенных клиентов?";

            DialogResult result = MessageBox.Show(textMessage, "Уведомление",
                                                  MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                                  MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                FormControl(currentState);
            }
            FormControl();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ReportOrder reportOrders = new ReportOrder();
            reportOrders.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FormControl();
        }
    }
}
