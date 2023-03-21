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

namespace Hotel.Orders
{
    public partial class ListEmployees : Form
    {
        private bool formIsClosed = false;
        private const int tableRowCount = 0;//17;
        private const int tableColumnCount = 3;

        private Utility.StateShowList _state;
        public ListEmployees(Utility.StateShowList currentState)
        {
            InitializeComponent();
            _state = currentState;
        }

        private void ListEmployees_Load(object sender, EventArgs e)
        {
            Utility.ConnectToDataBase(ShowMainTable);
        }
        private void ShowMainTable(OleDbCommand dbCommand)
        {
            string filter = "";
            if (_state == Utility.StateShowList.ORDER)
            {
                filter = " ref_ID_position IN(2, 4)";
            }
            else 
            {
                filter = " ref_ID_position IN(2, 4, 3, 11, 12)";
            }
            dbCommand.CommandText = "SELECT COUNT(*) FROM Employees WHERE " + filter;
            int countData = Convert.ToInt32(dbCommand.ExecuteScalar());
            int tableRowCountCurrent = countData < tableRowCount ? tableRowCount : countData;
            dbCommand.CommandText = @"SELECT ID_employee, Emp_FullName, TitlePositions 
                                      FROM Employees INNER JOIN [Position] 
                                      ON Employees.ref_ID_position = [Position].ID_position
                                      WHERE " + filter;

            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader())
            {
                dbDataReader.Read();
                dataGridView1.RowCount = tableRowCountCurrent;
                tableRowCountCurrent = dbDataReader.FieldCount;
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

        private int GetSelectedID()
        {
            return (int)dataGridView1.SelectedRows[0].Cells[0].Value;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Utility.selected_ID_Employee = GetSelectedID();
            formIsClosed = true;
            this.Close();
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            Utility.selected_ID_Employee = GetSelectedID();
            formIsClosed = true;
            this.Close();
        }

        private void ListEmployees_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !formIsClosed)
            {
                Utility.selected_ID_Employee = -1;
            }
        }
    }
}
