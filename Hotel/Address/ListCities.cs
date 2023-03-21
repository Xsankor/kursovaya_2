using Hotel.Additional;
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

namespace Hotel.Address
{
    public partial class ListCities : Form
    {
        private bool _formIsClosed = false;
        private const int _tableRowCount = 0;
        private const int _tableColumnCount = 2;
        public ListCities()
        {
            InitializeComponent();
            FormLoad();
        }

        private void FormLoad()
        {
            Utility.ConnectToDataBase(ShowMainTable);
        }

        private void ShowMainTable(OleDbCommand dbCommand)
        {
            dbCommand.CommandText = "SELECT COUNT(*) FROM Cities";
            int countData = Convert.ToInt32(dbCommand.ExecuteScalar());
            int tableRowCountCurrent = countData < _tableRowCount ? _tableRowCount : countData;
            dbCommand.CommandText = @"SELECT * FROM [Cities]";

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

        private int GetSelectedID()
        {
            return (int)dataGridView1.SelectedRows[0].Cells[0].Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Utility.selected_ID_City = GetSelectedID();
            _formIsClosed = true;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Add addRegion = new Add(7);
            addRegion.ShowDialog();
            FormLoad();
        }
    }
}
