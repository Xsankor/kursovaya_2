using Hotel.Utilits;
using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Hotel.Orders
{
    public partial class ListRooms : Form
    {
        private bool formIsClosed = false;
        private const int tableRowCount = 0;//17;
        private const int tableColumnCount = 5;
        public ListRooms()
        {
            InitializeComponent();
        }

        private void ListRooms_Load(object sender, EventArgs e)
        {
            Utility.ConnectToDataBase(ShowMainTable);
        }

        private void ShowMainTable(OleDbCommand dbCommand)
        {
            dbCommand.CommandText = "SELECT COUNT(*) FROM Rooms WHERE RoomIsBusy = FALSE";
            int countData = Convert.ToInt32(dbCommand.ExecuteScalar());
            int tableRowCountCurrent = countData < tableRowCount ? tableRowCount : countData;
            dbCommand.CommandText = @"SELECT ID_room, RoomNumber, IIf([RoomIsBusy] = FALSE, 'СВОБОДЕН') As IsBusy, CountRooms, RoomType  
                                      FROM Rooms INNER JOIN TypeRooms
                                      ON Rooms.ref_ID_type_room = TypeRooms.ID_type_room
                                      WHERE RoomIsBusy = FALSE
                                     ";

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

        private void button1_Click(object sender, EventArgs e)
        {
            Utility.selected_ID_Room = GetSelectedID();
            formIsClosed = true;
            this.Close();
        }

        private int GetSelectedID()
        {
            return (int)dataGridView1.SelectedRows[0].Cells[0].Value;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Utility.selected_ID_Room = GetSelectedID();
            formIsClosed = true;
            this.Close();
        }

        private void ListRooms_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !formIsClosed)
            {
                Utility.selected_ID_Room = -1;
            }
        }
    }
}
