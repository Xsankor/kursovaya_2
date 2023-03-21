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

namespace Hotel.Additional
{
    public partial class Additional : Form
    {
        private DataGridView[] dataGridArray = new DataGridView[9];
        private int selectedTabIndex;
        private int ID_value;
        public Additional()
        {
            InitializeComponent();
            dataGridArray[0] = dataGridView1;
            dataGridArray[1] = dataGridView2;
            dataGridArray[2] = dataGridView3;
            dataGridArray[3] = dataGridView4;
            dataGridArray[4] = dataGridView5;
            dataGridArray[5] = dataGridView6;
            dataGridArray[6] = dataGridView7;
            dataGridArray[7] = dataGridView8;
            dataGridArray[8] = dataGridView9;

        }
        private void Additional_Load(object sender, EventArgs e)
        {
            tabControl1.SizeMode = TabSizeMode.Fixed;
            //tabControl1.ItemSize = new Size((tabControl1.Width / tabControl1.TabCount), 0);
            FormLoad();
        }

        private void ShowMainTable(OleDbCommand dbCommand)
        {
            int tableRowCount = 0;
            int tableColumnCount = 0;
            string[] arrayQuery = { "SELECT COUNT(*) FROM [TypeClient]",
                                    "SELECT COUNT(*) FROM [Position]",
                                    "SELECT COUNT(*) FROM [TypeRooms]",
                                    "SELECT COUNT(*) FROM [Services]",
                                    "SELECT COUNT(*) FROM [Rooms]",
                                    "SELECT COUNT(*) FROM [Countries]",
                                    "SELECT COUNT(*) FROM [Regions]",
                                    "SELECT COUNT(*) FROM [Cities]",
                                    "SELECT COUNT(*) FROM [Streets]" };

        string[] arrayCommandText = { @"SELECT ID_type_client, TitleType, 
                                            IIf([MustPrepaid] = FALSE, 'Без предоплаты', 'Нужна предоплата') 
                                            FROM TypeClient",
                                          "SELECT * FROM [Position]",
                                          "SELECT * FROM [TypeRooms]",
                                          "SELECT * FROM [Services]",
                                          @"SELECT ID_room, RoomNumber, 
                                            IIf([RoomIsBusy] = FALSE, 'СВОБОДЕН', 'ЗАНЯТ'),
                                            RoomType, CountRooms
                                            FROM [Rooms] 
                                            INNER JOIN [TypeRooms] 
                                            ON [Rooms].ref_ID_type_room = [TypeRooms].ID_type_room",
                                          "SELECT * FROM [Countries]",
                                          "SELECT * FROM [Regions]",
                                          "SELECT * FROM [Cities]",
                                          "SELECT * FROM [Streets]",};

            for (int k = 0; k < dataGridArray.Length; ++k)
            {
                if ((k >= 0 && k < 4) || k == 6) tableColumnCount = 3;
                if (k == 4) tableColumnCount = 5;
                if (k == 5 || k > 6) tableColumnCount = 2;
                dbCommand.CommandText = arrayQuery[k];
                int countData = Convert.ToInt32(dbCommand.ExecuteScalar());
                int tableRowCountCurrent = countData < tableRowCount ? tableRowCount : countData;
                dbCommand.CommandText = arrayCommandText[k];

                using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader())
                {
                    dbDataReader.Read();

                    dataGridArray[k].RowCount = tableRowCountCurrent;
                    for (int i = 0; i < countData; ++i)
                    {
                        for (int j = 0; j < tableColumnCount; ++j)
                        {
                            dataGridArray[k].Rows[i].Cells[j].Value = dbDataReader.GetValue(j);
                        }
                        dbDataReader.Read();
                    }
                    dbDataReader.Close();
                }
            }
        }

        //Добавить
        private void button1_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 4 && dataGridView5.RowCount >= 20)
            {
                MessageBox.Show("Максимальное количество номеров!");
                return;
            }
            Add add = new Add(tabControl1.SelectedIndex);
            add.ShowDialog();
            FormLoad();
        }
        //Изменить
        private void button2_Click(object sender, EventArgs e)
        {
            selectedTabIndex = tabControl1.SelectedIndex;
            int updateIndex = dataGridArray[selectedTabIndex].SelectedCells[0].RowIndex;
            int ID_value = (int)dataGridArray[selectedTabIndex].Rows[updateIndex].Cells[0].Value;

            Update update = new Update(selectedTabIndex, ID_value);
            update.ShowDialog();
            FormLoad();
        }
        //Удалить
        private void button3_Click(object sender, EventArgs e)
        {
            Utility.SomeFunction someFunction = DeleteRow;
            Utility.ConnectToDataBase(someFunction);
            FormLoad();
        }

        private void FormLoad() 
        {
            Utility.SomeFunction someFunction = ShowMainTable;
            Utility.ConnectToDataBase(someFunction);
        }

        private void DeleteRow(OleDbCommand dbCommand)
        {
            string[] arrayQuery = { "DELETE * FROM [TypeClient] WHERE ID_type_client = ",
                                    "DELETE * FROM [Position] WHERE ID_position = ",
                                    "DELETE * FROM [TypeRooms] WHERE ID_type_room = ",
                                    "DELETE * FROM [Services] WHERE ID_services = ",
                                    "DELETE * FROM [Rooms] WHERE ID_room = ",
                                    "DELETE * FROM [Countries] WHERE ID_country = ",
                                    "DELETE * FROM [Regions] WHERE ID_Region = ",
                                    "DELETE * FROM [Cities] WHERE ID_City = ",
                                    "DELETE * FROM [Streets] WHERE ID_Street = " };
            selectedTabIndex = tabControl1.SelectedIndex;
            
            ID_value = (int)dataGridArray[selectedTabIndex].SelectedRows[0].Cells[0].Value;
            Utility.SomeFunctionForCheck someFunction = CanIDeleteData;
            if (Utility.ConnectToDataBase(someFunction)) return;
            dbCommand.CommandText = arrayQuery[selectedTabIndex] + ID_value;
            dbCommand.ExecuteNonQuery();
            dataGridArray[selectedTabIndex].Rows.Clear();
        }

        private bool CanIDeleteData(OleDbCommand dbCommand)
        {
            string[] arrayCheck = { "SELECT COUNT(*) FROM [Сlients] WHERE ref_ID_type_client = ",
                                    "SELECT COUNT(*) FROM [Employees] WHERE ref_ID_position = ",
                                    "SELECT COUNT(*) FROM [Rooms] WHERE ref_ID_type_room = ",
                                    "SELECT COUNT(*) FROM [ListOfServicesRendered] WHERE reference_ID_services = ",
                                    "SELECT COUNT(*) FROM [Orders] WHERE ref_ID_room = ",
                                    "SELECT COUNT(*) FROM [Сlients] WHERE ref_ID_country = ",
                                    "SELECT COUNT(*) FROM [Сlients] WHERE ref_ID_Region = ",
                                    "SELECT COUNT(*) FROM [Сlients] WHERE ref_ID_City = ",
                                    "SELECT COUNT(*) FROM [Сlients] WHERE ref_ID_Street = ",};

            dbCommand.CommandText = arrayCheck[selectedTabIndex] + ID_value;
            bool result;
            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader())
            {
                dbDataReader.Read();
                result = (int)dbDataReader[0] > 0 ? true : false;
                if(result) MessageBox.Show($"Данная запись связанна с другой.");
                dbDataReader.Close();
            }
            return result;
        }
    }
}
