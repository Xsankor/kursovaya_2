using Hotel.Utilits;
using System;
using System.Data.OleDb;
using System.Resources;
using System.Windows.Forms;

namespace Hotel.Orders
{
    public partial class ListClients : Form
    {
        private bool _formIsClosed = false;
        private const int _tableRowCount = 0;
        private const int _tableColumnCount = 5;
        
        public ListClients()
        {
            InitializeComponent();  
        }

        private void ListClients_Load(object sender, EventArgs e)
        {
            Utility.ConnectToDataBase(ShowMainTable);
        }

        private void ShowMainTable(OleDbCommand dbCommand)
        {
            dbCommand.CommandText = "SELECT COUNT(*) FROM Сlients";
            int countData = Convert.ToInt32(dbCommand.ExecuteScalar());
            int tableRowCountCurrent = countData < _tableRowCount ? _tableRowCount : countData;
            dbCommand.CommandText = @"SELECT ID_client, FullName, TypeClient.TitleType, FORMAT(DateOfBirth, 'dd.mm.yyyy'), PlaceOfResidence 
                                      FROM Сlients INNER JOIN TypeClient 
                                      ON Сlients.ref_ID_type_client = TypeClient.ID_type_client
                                      ORDER BY DateOfBirth
                                     ";

            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader())
            {
                dbDataReader.Read();
                dataGridView1.RowCount = tableRowCountCurrent;
                tableRowCountCurrent = dbDataReader.FieldCount;
                dataGridView1.ColumnCount = _tableColumnCount;

                for (int i = 0; i < countData; ++i)
                {
                    for (int j = 0; j < _tableColumnCount; ++j)
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
            Utility.selected_ID_Client = GetSelectedID();
            _formIsClosed = true;
            this.Close();
        }

        private int GetSelectedID() 
        {
            return (int)dataGridView1.SelectedRows[0].Cells[0].Value;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Utility.selected_ID_Client = GetSelectedID();
            _formIsClosed = true;
            this.Close();
        }

        private void ListClients_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !_formIsClosed)
            {
                Utility.selected_ID_Room = -1;
            }
        }
    }
}
