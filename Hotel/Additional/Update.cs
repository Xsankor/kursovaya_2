using Hotel.Utilits;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Hotel.Additional
{
    public partial class Update : Form
    {
        private class CurrentData
        {
            public string Info_1 { get; set; }
            public bool Info_2 { get; set; }
            public string Info_3 { get; set; }
            public string Info_4 { get; set; }
            public string Info_5 { get; set; }
        };
        private int _selectedTabIndex;
        private CurrentData currentData;
        private string _textQuery;
        private string _textCommandText;
        private int _currentID;
        public Update(int selectedTabIndex, int current_ID)
        {
            currentData = new CurrentData();
            _selectedTabIndex = selectedTabIndex;
            _currentID = current_ID;
            InitializeComponent();
        }

        private void Update_Load(object sender, EventArgs e)
        {
            string text_one = "", text_two = "", text_three = "";
            switch (_selectedTabIndex)
            {
                case 0:
                    {
                        text_one = "Тип клиента";
                        text_two = "Предоплата";
                        textBox1.Visible = false;
                        comboBox1.Visible = true;
                        _textQuery = "SELECT * FROM [TypeClient] WHERE ID_type_client = ";
                        _textCommandText = @"UPDATE [TypeClient] 
                                                SET TitleType = @_TitleType, MustPrepaid = @_MustPrepaid
                                              WHERE ID_type_client = @_currentID";
                    }
                    break;
                case 1:
                    {
                        text_one = "Должность";
                        text_two = "Зарплата";
                        _textQuery = "SELECT * FROM [Position] WHERE ID_position = ";
                        _textCommandText = @"UPDATE [Position] 
                                                SET TitlePositions = @_TitlePositions, Salary = @_Salary
                                              WHERE ID_position = @_currentID";
                    }
                    break;
                case 2:
                    {
                        text_one = "Тип комнаты";
                        text_two = "Доп. информация";
                        _textQuery = "SELECT * FROM [TypeRooms] WHERE ID_type_room = ";
                        _textCommandText = @"UPDATE [TypeRooms] 
                                                SET RoomType = @_RoomType, AdditionalInformation = @_AdditionalInformation
                                              WHERE ID_type_room = @_currentID";
                    }
                    break;
                case 3:
                    {
                        text_one = "Название услуги";
                        text_two = "Стоимость";
                        _textQuery = "SELECT * FROM [Services] WHERE ID_services = ";
                        _textCommandText = @"UPDATE [Services] 
                                                SET NameService = @_NameService, ServiceCost = @_ServiceCost
                                              WHERE ID_services = @_currentID";
                    }
                    break;
                case 4:
                    {
                        text_one = "Номер номера";
                        text_two = "Тип номера";
                        text_three = "Количество комнат";
                        label1.Visible = true;
                        textBox2.Visible = true;
                        textBox1.Visible = false;
                        comboBox1.Visible = true;
                        comboBox1.Items.Clear();
                        Utility.SomeFunction someFunctionFill = FillComboBox;
                        Utility.ConnectToDataBase(someFunctionFill);
                        textBox3.MaxLength = 3;
                        _textQuery = " SELECT ID_room, RoomNumber, RoomType, CountRooms " +
                                     " FROM [Rooms] INNER JOIN [TypeRooms] ON [Rooms].ref_ID_type_room = [TypeRooms].ID_type_room " +
                                     " WHERE ID_room = ";
                        _textCommandText = @"UPDATE [Rooms] 
                                                SET RoomNumber = @_RoomNumber, 
                                                    ref_ID_type_room = @_ref_ID_type_room, 
                                                    CountRooms = @_CountRooms
                                              WHERE ID_room = @_currentID";
                    }
                    break;
                case 5:
                    {
                        text_one = "Страна";
                        textBox1.Visible = false;

                        _textQuery = "SELECT * FROM Countries WHERE ID_country = ";
                        _textCommandText = @"UPDATE Countries 
                                                SET Country = @_Country
                                              WHERE ID_country = @_currentID";
                    }
                    break;
                case 6:
                    {
                        text_one = "Регион";
                        text_two = "Код региона";

                        _textQuery = "SELECT * FROM Regions WHERE ID_Region = ";
                        _textCommandText = @"UPDATE Regions 
                                                SET Region = @_Region, Region_num = @_Region_num
                                              WHERE ID_Region = @_currentID";
                    }
                    break;
                case 7:
                    {
                        text_one = "Город";
                        textBox1.Visible = false;

                        _textQuery = "SELECT * FROM Cities WHERE ID_City = ";
                        _textCommandText = @"UPDATE Cities 
                                                SET City = @_City
                                              WHERE ID_City = @_currentID";
                    }
                    break;
                case 8:
                    {
                        text_one = "Услуги";
                        textBox1.Visible = false;

                        _textQuery = "SELECT * FROM Streets WHERE ID_Street = ";
                        _textCommandText = @"UPDATE Streets 
                                                SET Street = @_Street
                                              WHERE ID_Street = @_currentID";
                    }
                    break;
            }
            label3.Text = text_one;
            label4.Text = text_two;
            label1.Text = text_three;

            Utility.SomeFunction someFunction = LoadData;
            Utility.ConnectToDataBase(someFunction);
        }

        private void LoadData(OleDbCommand dbCommand)
        {
            dbCommand.CommandText = _textQuery + _currentID;

            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader())
            {
                dbDataReader.Read();
                
                textBox3.Text = dbDataReader[1].ToString();
                if (_selectedTabIndex == 0) 
                {
                    comboBox1.SelectedIndex = Convert.ToBoolean(dbDataReader[2]) ? 0 : 1;
                }
                if (_selectedTabIndex == 4) 
                {
                    comboBox1.SelectedItem = dbDataReader["RoomType"].ToString();
                    textBox2.Text = dbDataReader["CountRooms"].ToString();
                }
                if (_selectedTabIndex == 1 || _selectedTabIndex == 2 || _selectedTabIndex == 3 || _selectedTabIndex == 6)
                {
                    textBox1.Text = dbDataReader[2].ToString();
                }
                dbDataReader.Close();
            }
        }

        private void UpdateClientData(OleDbCommand dbCommand)
        {
            FillCurrentClientData();
            dbCommand.CommandText = _textCommandText;

            switch (_selectedTabIndex)
            {
                case 0:
                    {
                        dbCommand.Parameters.Add(new OleDbParameter("@_TitleType", OleDbType.VarChar)).Value = currentData.Info_1;
                        dbCommand.Parameters.Add(new OleDbParameter("@_MustPrepaid", OleDbType.Boolean)).Value = currentData.Info_2;
                    }
                    break;
                case 1:
                    {
                        dbCommand.Parameters.Add(new OleDbParameter("@_TitlePositions", OleDbType.VarChar)).Value = currentData.Info_1;
                        dbCommand.Parameters.Add(new OleDbParameter("@_Salary", OleDbType.Numeric)).Value = currentData.Info_3;
                    }
                    break;
                case 2:
                    {
                        dbCommand.Parameters.Add(new OleDbParameter("@_RoomType", OleDbType.VarChar)).Value = currentData.Info_1;
                        dbCommand.Parameters.Add(new OleDbParameter("@_AdditionalInformation", OleDbType.VarChar)).Value = currentData.Info_3;
                    }
                    break;
                case 3:
                    {
                        dbCommand.Parameters.Add(new OleDbParameter("@_NameService", OleDbType.VarChar)).Value = currentData.Info_1;
                        dbCommand.Parameters.Add(new OleDbParameter("@_ServiceCost", OleDbType.Numeric)).Value = currentData.Info_3;
                    }
                    break;
                case 4:
                    {
                        dbCommand.Parameters.Add(new OleDbParameter("@_RoomNumber", OleDbType.Numeric)).Value = Convert.ToInt32(currentData.Info_1);
                        dbCommand.Parameters.Add(new OleDbParameter("@_ref_ID_type_room", OleDbType.Numeric)).Value = Convert.ToInt32(currentData.Info_5);
                        dbCommand.Parameters.Add(new OleDbParameter("@_CountRooms", OleDbType.Numeric)).Value = Convert.ToInt32(currentData.Info_4);
                    }
                    break;
                case 5:
                    {
                        dbCommand.Parameters.Add(new OleDbParameter("@_Country", OleDbType.VarChar)).Value = currentData.Info_1;
                    }
                    break;
                case 6:
                    {
                        dbCommand.Parameters.Add(new OleDbParameter("@_Region", OleDbType.VarChar)).Value = currentData.Info_1;
                        dbCommand.Parameters.Add(new OleDbParameter("@_Region_num", OleDbType.Numeric)).Value = currentData.Info_3;
                    }
                    break;
                case 7:
                    {
                        dbCommand.Parameters.Add(new OleDbParameter("@_City", OleDbType.VarChar)).Value = currentData.Info_1;
                    }
                    break;
                case 8:
                    {
                        dbCommand.Parameters.Add(new OleDbParameter("@_Street", OleDbType.VarChar)).Value = currentData.Info_1;
                    }
                    break;
            }
            dbCommand.Parameters.Add(new OleDbParameter("@_currentID", OleDbType.Numeric)).Value = _currentID;
            int codeResult = dbCommand.ExecuteNonQuery();
            string message = codeResult == 1 ? "Данные обновлены!" : "Что-то пошло не так";
            System.Windows.Forms.MessageBox.Show(message);
        }

        private void FillCurrentClientData()
        {
            currentData.Info_1 = textBox3.Text;
            if (_selectedTabIndex == 0)
            {
                currentData.Info_2 = comboBox1.SelectedIndex == 0 ? true : false;
            }
            if (_selectedTabIndex == 4) 
            {
                currentData.Info_4 = textBox2.Text;
                Utility.SomeFunctionWithReturn someFunction = GetSelectedPosition;
                currentData.Info_5 = Utility.ConnectToDataBase(someFunction);
            }
            if(_selectedTabIndex == 1 || _selectedTabIndex == 2 || _selectedTabIndex == 3 || _selectedTabIndex == 6) 
            {
                currentData.Info_3 = textBox1.Text;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Utility.SomeFunction someFunction = UpdateClientData;
            Utility.ConnectToDataBase(someFunction);
            this.Close();
        }

        private void FillComboBox(OleDbCommand dbCommand)
        {
            dbCommand.CommandText = "SELECT RoomType FROM [TypeRooms]";
            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader())
            {
                comboBox1.Items.Clear();
                while (dbDataReader.Read())
                {
                    comboBox1.Items.Add(dbDataReader.GetValue(0).ToString());
                }
                comboBox1.SelectedIndex = -1;
                dbDataReader.Close();
            }
        }

        private string GetSelectedPosition(OleDbCommand dbCommand)
        {
            string selectedType = comboBox1.SelectedItem.ToString();
            dbCommand.CommandText = $"SELECT ID_type_room FROM [TypeRooms] WHERE RoomType = '{selectedType}'";
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
