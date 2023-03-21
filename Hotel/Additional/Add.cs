using Hotel.Utilits;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
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
    public partial class Add : Form
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
        private int _countError = 0;
        public Add(int selectedTabIndex)
        {
            currentData = new CurrentData();
            InitializeComponent();
            _selectedTabIndex = selectedTabIndex;
        }

        private void Add_Load(object sender, EventArgs e)
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
                    }
                    break;
                case 1:
                    {
                        text_one = "Должность";
                        text_two = "Зарплата";
                    }
                    break;
                case 2:
                    {
                        text_one = "Тип комнаты";
                        text_two = "Доп. информация";
                    }
                    break;
                case 3:
                    {
                        text_one = "Название услуги";
                        text_two = "Стоимость";
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
                        Utility.SomeFunction someFunction = FillComboBox;
                        Utility.ConnectToDataBase(someFunction);
                        textBox3.MaxLength = 3;
                    }
                    break;
                case 5:
                    {
                        text_one = "Страна";
                        textBox1.Visible = false;
                    }
                    break;
                case 6:
                    {
                        text_one = "Регион";
                        text_two = "Код региона";
                    }
                    break;
                case 7:
                    {
                        text_one = "Город";
                        textBox1.Visible = false;
                    }
                    break;
                case 8:
                    {
                        text_one = "Услуги";
                        textBox1.Visible = false;
                    }
                    break;
            }
            label3.Text = text_one;
            label4.Text = text_two;
            label1.Text = text_three;
        }

        private bool CollectData()
        {
            CheckTextBox(out _countError);

            if (_countError > 0) return false;

            currentData.Info_1 = textBox3.Text;
            currentData.Info_3 = textBox1.Text;
            currentData.Info_4 = textBox2.Text;

            if(comboBox1.SelectedIndex >= 0) 
            {
                Utility.SomeFunctionWithReturn someFunction = GetSelectedType;
                currentData.Info_5 = Utility.ConnectToDataBase(someFunction).ToString();
            }

            return true;
        }

        private void CheckTextBox(out int _countError) 
        {
            _countError = 0;

            if (textBox3.Text.Length == 0)
            {
                ++_countError;
                errorProvider1.SetError(textBox3, "Заполните поле!");
                errorProvider1.SetIconAlignment(textBox3, ErrorIconAlignment.MiddleRight);
            }
            else
                errorProvider1.Clear();

            if ((_selectedTabIndex == 4 || _selectedTabIndex == 0) && comboBox1.SelectedIndex == -1)
            {
                ++_countError;
                errorProvider2.SetError(comboBox1, "Необходимо выбрать!");
                errorProvider2.SetIconAlignment(comboBox1, ErrorIconAlignment.MiddleRight);
            }
            else 
                errorProvider2.Clear();

            if (_selectedTabIndex == 4 && textBox2.Text.Length == 0)
            {
                ++_countError;
                errorProvider3.SetError(textBox2, "Заполните поле!");
                errorProvider3.SetIconAlignment(textBox2, ErrorIconAlignment.MiddleRight);
            }
            else
                errorProvider3.Clear();

            if ((_selectedTabIndex != 2 && _selectedTabIndex != 5 && _selectedTabIndex != 7 && _selectedTabIndex != 8) 
                && textBox1.Text.Length == 0)
            {
                ++_countError;
                errorProvider2.SetError(textBox1, "Заполните поле!");
                errorProvider2.SetIconAlignment(textBox1, ErrorIconAlignment.MiddleRight);
            }
            else
                errorProvider2.Clear();
        }

        private void CreateRequestAdd() 
        {
            switch (_selectedTabIndex)
            {
                case 0:
                    {
                        _textQuery = $" SELECT COUNT(*) " +
                                     $" FROM TypeClient " +
                                     $" WHERE TitleType LIKE '%{currentData.Info_1}'";
                        _textCommandText = "INSERT INTO TypeClient(TitleType, MustPrepaid) VALUES(?, ?)";
                    }
                    break;
                case 1:
                    {
                        _textQuery = $" SELECT COUNT(*) " +
                                     $" FROM [Position] " +
                                     $" WHERE TitlePositions LIKE '%{currentData.Info_1}'";
                        _textCommandText = "INSERT INTO [Position](TitlePositions, Salary) VALUES(?, ?)";
                    }
                    break;
                case 2:
                    {
                        _textQuery = $" SELECT COUNT(*) " +
                                     $" FROM TypeRooms " +
                                     $" WHERE RoomType LIKE '%{currentData.Info_1}'";
                        _textCommandText = "INSERT INTO TypeRooms(RoomType, AdditionalInformation) VALUES(?, ?)";
                    }
                    break;
                case 3:
                    {
                        _textQuery = $" SELECT COUNT(*) " +
                                     $" FROM Services " +
                                     $" WHERE NameService LIKE '%{currentData.Info_1}'";
                        _textCommandText = "INSERT INTO Services(NameService, ServiceCost) VALUES(?, ?)";
                    }
                    break;
                case 4:
                    {
                        _textQuery = " SELECT COUNT(*) FROM Rooms " +
                                     " WHERE RoomNumber = " + (currentData.Info_1 == "" ? -1 : Convert.ToInt32(currentData.Info_1));
                        _textCommandText = "INSERT INTO Rooms(RoomNumber, RoomIsBusy, ref_ID_type_room, CountRooms) VALUES(?, ?, ?, ?)";
                    }
                    break;
                case 5:
                    {
                        _textQuery = $" SELECT COUNT(*) " +
                                     $" FROM Countries " +
                                     $" WHERE Country LIKE '%{currentData.Info_1}'";
                        _textCommandText = "INSERT INTO Countries(Country) VALUES(?)";
                    }
                    break;
                case 6:
                    {
                        _textQuery = $" SELECT COUNT(*) " +
                                     $" FROM Regions " +
                                     $" WHERE Region LIKE '%{currentData.Info_1}'";
                        _textCommandText = "INSERT INTO Regions(Region, Region_num) VALUES(?, ?)";
                    }
                    break;
                case 7:
                    {
                        _textQuery = $" SELECT COUNT(*) " +
                                     $" FROM Cities " +
                                     $" WHERE City LIKE '%{currentData.Info_1}'";
                        _textCommandText = "INSERT INTO Cities(City) VALUES(?)";
                    }
                    break;
                case 8:
                    {
                        _textQuery = $" SELECT COUNT(*) " +
                                     $" FROM Streets " +
                                     $" WHERE Street LIKE '%{currentData.Info_1}'";
                        _textCommandText = "INSERT INTO Streets(Street) VALUES(?)";
                    }
                    break;
            }
        }

        private void ApplyData(OleDbCommand dbCommand)
        {
            if (!CollectData()) return;
            CreateRequestAdd();

            dbCommand.CommandText = _textQuery;
            int codeResult = 0;
            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader())
            {
                dbDataReader.Read();
                codeResult = Convert.ToInt32(dbDataReader.GetValue(0));
                if (codeResult >= 1)
                {
                    MessageBox.Show("Такая запись уже есть.");
                    return;
                }
                dbDataReader.Close();
            }
            dbCommand.Parameters.Clear();
            dbCommand.CommandText = _textCommandText;

            switch (_selectedTabIndex) 
            {
                case 0: 
                {
                    dbCommand.Parameters.Add(new OleDbParameter("@TitleType", OleDbType.VarChar)).Value = currentData.Info_1;
                    dbCommand.Parameters.Add(new OleDbParameter("@MustPrepaid", OleDbType.Boolean)).Value = currentData.Info_2;
                }break;
                case 1:
                {
                    dbCommand.Parameters.Add(new OleDbParameter("@TitlePositions", OleDbType.VarChar)).Value = currentData.Info_1;
                    dbCommand.Parameters.Add(new OleDbParameter("@Salary", OleDbType.Decimal)).Value = currentData.Info_3;
                }break;
                case 2:
                {
                    dbCommand.Parameters.Add(new OleDbParameter("@RoomType", OleDbType.VarChar)).Value = currentData.Info_1;
                    dbCommand.Parameters.Add(new OleDbParameter("@AdditionalInformation", OleDbType.VarChar)).Value = currentData.Info_3;
                }break;
                case 3:
                {
                    dbCommand.Parameters.Add(new OleDbParameter("@NameService", OleDbType.VarChar)).Value = currentData.Info_1;
                    dbCommand.Parameters.Add(new OleDbParameter("@ServiceCost", OleDbType.Numeric)).Value = currentData.Info_3;
                }break;
                case 4:
                {
                    dbCommand.Parameters.Add(new OleDbParameter("@RoomNumber", OleDbType.Numeric)).Value = Convert.ToInt32(currentData.Info_1);
                    dbCommand.Parameters.Add(new OleDbParameter("@RoomIsBusy", OleDbType.Boolean)).Value = false;
                    dbCommand.Parameters.Add(new OleDbParameter("@ref_ID_type_room", OleDbType.Numeric)).Value = Convert.ToInt32(currentData.Info_5);
                    dbCommand.Parameters.Add(new OleDbParameter("@CountRooms", OleDbType.Numeric)).Value = Convert.ToInt32(currentData.Info_4);
                }break;
                case 5: 
                {
                    dbCommand.Parameters.Add(new OleDbParameter("@Country", OleDbType.VarChar)).Value = currentData.Info_1;
                }break;
                case 6: 
                {
                    dbCommand.Parameters.Add(new OleDbParameter("@Region", OleDbType.VarChar)).Value = currentData.Info_1;
                    dbCommand.Parameters.Add(new OleDbParameter("@Region_num", OleDbType.Numeric)).Value = currentData.Info_3;
                }break;
                case 7: 
                {
                    dbCommand.Parameters.Add(new OleDbParameter("@City", OleDbType.VarChar)).Value = currentData.Info_1;
                }break;
                case 8: 
                {
                    dbCommand.Parameters.Add(new OleDbParameter("@Street", OleDbType.VarChar)).Value = currentData.Info_1;
                }break;
            }

            codeResult = dbCommand.ExecuteNonQuery();
            string message = codeResult == 1 ? "Добавлена новая запись!" : "Что-то пошло не так";
            System.Windows.Forms.MessageBox.Show(message);

            ClearFields();
        }

        private void ClearFields()
        {
            textBox3.Clear();
            textBox1.Clear();
            comboBox1.SelectedIndex = -1;
            textBox2.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Utility.SomeFunction someFunction = ApplyData;
            Utility.ConnectToDataBase(someFunction);
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_selectedTabIndex != 1 && _selectedTabIndex != 3 && _selectedTabIndex == 4) return;
            char number = e.KeyChar;

            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_selectedTabIndex != 4) return;
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
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

        private string GetSelectedType(OleDbCommand dbCommand)
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
