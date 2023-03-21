using Hotel.Utilits;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Hotel.Address
{
    public partial class AddressResidential : Form
    {
        public struct CurrentDataAddress
        {
            public string Country { get; set; }
            public string Region { get; set; }
            public string City { get; set; }
            public string Street { get; set; }
            public int ID_Country { get; set; }
            public int ID_Region { get; set; }
            public int ID_City { get; set; }
            public int ID_Street { get; set; }
            public int HomeNumber { get; set; }
            public string HouseLetter { get; set; }
            public int СaseNumber { get; set; }
            public int ApartmentNumber { get; set; }
        }
        private string _textQuery;
        private enum State
        {
            COUNTRY,
            REGION,
            CITY,
            STREET
        }
        public CurrentDataAddress currentDataAddress = new CurrentDataAddress();

        public string Address = string.Empty;

        private int _current_id;
        private int _isNullIDCountry;
        public AddressResidential(int current_ID)
        {
            _current_id = current_ID;
            InitializeComponent();

            if (_current_id == 0) return;

            Utility.SomeFunction checkAddressClient = CheckAddress;
            Utility.ConnectToDataBase(checkAddressClient);
            if (_isNullIDCountry == -1) return; 

            Utility.SomeFunction someFunction = FormLoad;
            Utility.ConnectToDataBase(someFunction);
        }

        private void FormLoad(OleDbCommand dbCommand) 
        {
            dbCommand.CommandText = @"SELECT Country, Region, City, Street,
                                      Number_home, Letter_home, Apartment_number, Сase_number,
                                      ref_ID_country, ref_ID_Region, ref_ID_City, ref_ID_Street
                                      FROM ((((Сlients 
                                      INNER JOIN Countries 
                                      ON Сlients.ref_ID_country = Countries.ID_country)
                                      INNER JOIN Regions
                                      ON Сlients.ref_ID_Region = Regions.ID_Region)
                                      INNER JOIN Cities 
                                      ON Сlients.ref_ID_City = Cities.ID_City)
                                      INNER JOIN Streets 
                                      ON Сlients.ref_ID_Street = Streets.ID_Street) 
                                      WHERE ID_client = " + _current_id;

            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader())
            {
                dbDataReader.Read();
                textBox2.Text = dbDataReader["Country"].ToString();
                textBox1.Text = dbDataReader["Region"].ToString();
                textBox3.Text = dbDataReader["City"].ToString();
                textBox4.Text = dbDataReader["Street"].ToString();
                numericUpDown1.Value    = (int)dbDataReader["Number_home"];
                comboBox1.SelectedValue = dbDataReader["Letter_home"];
                numericUpDown2.Value    = dbDataReader["Сase_number"].ToString().Length == 0 ? 0 : (int)dbDataReader["Сase_number"];
                numericUpDown3.Value    = (int)dbDataReader["Apartment_number"];

                currentDataAddress.ID_Country   = (int)dbDataReader["ref_ID_country"];
                currentDataAddress.ID_Region    = (int)dbDataReader["ref_ID_Region"];
                currentDataAddress.ID_City      = (int)dbDataReader["ref_ID_City"];
                currentDataAddress.ID_Street    = (int)dbDataReader["ref_ID_Street"];
                dbDataReader.Close();
            }
        }

        private void CheckAddress(OleDbCommand dbCommand) 
        {
            dbCommand.CommandText = @"SELECT IsNull(ref_ID_country)
                                      FROM Сlients WHERE ID_client = " + _current_id;
            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader()) 
            {
                dbDataReader.Read();
                _isNullIDCountry = Convert.ToInt32(dbDataReader.GetValue(0));
                dbDataReader.Close();
            }
        }

        private string GetSelectedData(State state)
        {
            switch (state)
            {
                case State.COUNTRY:
                    _textQuery = $"SELECT Country FROM Countries WHERE ID_country = {currentDataAddress.ID_Country}";
                    break;
                case State.REGION:
                    _textQuery = $"SELECT Region, Region_num FROM Regions WHERE ID_Region = {currentDataAddress.ID_Region}";
                    break;
                case State.CITY:
                    _textQuery = $"SELECT City FROM Cities WHERE ID_City = {currentDataAddress.ID_City}";
                    break;
                case State.STREET:
                    _textQuery = $"SELECT Street FROM Streets WHERE ID_Street = {currentDataAddress.ID_Street}";
                    break;
            }

            Utility.SomeFunctionWithReturn someFunctionWithReturn = GetTextResult;
            string result = Utility.ConnectToDataBase(someFunctionWithReturn);
            return result;
        }

        private string GetTextResult(OleDbCommand dbCommand)
        {
            string textResult;
            dbCommand.CommandText = _textQuery;
            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader())
            {
                dbDataReader.Read();
                textResult = dbDataReader[0].ToString();
            }
            return textResult;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ListСountries listСountries = new ListСountries();
            listСountries.ShowDialog();
            if (Utility.selected_ID_Country < 0) return;
            currentDataAddress.ID_Country= Utility.selected_ID_Country;
            State currentState = State.COUNTRY;
            textBox2.Text = GetSelectedData(currentState);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ListRegions listRegions = new ListRegions();
            listRegions.ShowDialog();
            if (Utility.selected_ID_Region < 0) return;
            currentDataAddress.ID_Region = Utility.selected_ID_Region;
            State currentState = State.REGION;
            textBox1.Text = GetSelectedData(currentState);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ListCities listCities = new ListCities();
            listCities.ShowDialog();
            if (Utility.selected_ID_City < 0) return;
            currentDataAddress.ID_City = Utility.selected_ID_City;
            State currentState = State.CITY;
            textBox3.Text = GetSelectedData(currentState);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ListStreets listStreets = new ListStreets();
            listStreets.ShowDialog();
            if (Utility.selected_ID_Street < 0) return;
            currentDataAddress.ID_Street = Utility.selected_ID_Street;
            State currentState = State.STREET;
            textBox4.Text = GetSelectedData(currentState);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CollectDataOfOrder();
            Utility.SomeFunction someFunction = UpdateClientAddress;
            Utility.ConnectToDataBase(someFunction);
            this.Close();
        }

        private void CollectDataOfOrder()
        {
            int countError = 0;
            if (textBox2.Text.Length == 0)
            {
                ++countError;
                errorProvider1.SetError(textBox2, "Нужно выбрать страну!");
                errorProvider1.SetIconAlignment(textBox2, ErrorIconAlignment.MiddleRight);
            }
            else
                errorProvider3.Clear();

            if (textBox1.Text.Length == 0)
            {
                ++countError;
                errorProvider2.SetError(textBox1, "Нужно выбрать регион!");
                errorProvider2.SetIconAlignment(textBox1, ErrorIconAlignment.MiddleRight);
            }
            else
                errorProvider2.Clear();

            if (textBox3.Text.Length == 0)
            {
                ++countError;
                errorProvider3.SetError(textBox3, "Нужно выбрать город!");
                errorProvider3.SetIconAlignment(textBox3, ErrorIconAlignment.MiddleRight);
            }
            else
                errorProvider3.Clear();

            if (textBox4.Text.Length == 0)
            {
                ++countError;
                errorProvider4.SetError(textBox4, "Нужно выбрать улицу!");
                errorProvider4.SetIconAlignment(textBox4, ErrorIconAlignment.MiddleRight);
            }
            else
                errorProvider4.Clear();

            if (numericUpDown1.Value == 0)
            {
                ++countError;
                errorProvider5.SetError(numericUpDown1, "Нужно заполнить номер дома!");
                errorProvider5.SetIconAlignment(numericUpDown1, ErrorIconAlignment.MiddleRight);
            }
            else
                errorProvider5.Clear();

            if (countError > 0) return;

            currentDataAddress.Country          = textBox2.Text;
            currentDataAddress.Region           = textBox1.Text;
            currentDataAddress.City             = textBox3.Text;
            currentDataAddress.Street           = textBox4.Text;
            currentDataAddress.HomeNumber       = (int)numericUpDown1.Value;
            currentDataAddress.HouseLetter      = comboBox1.SelectedText;
            currentDataAddress.СaseNumber       = (int)numericUpDown2.Value;
            currentDataAddress.ApartmentNumber  = (int)numericUpDown3.Value;

            Address = CreateAddress(ref currentDataAddress);
        }

        private void UpdateClientAddress(OleDbCommand dbCommand)
        {
            dbCommand.CommandText = @"UPDATE Сlients 
                                         SET ref_ID_country = @_ref_ID_country, ref_ID_Region = @_ref_ID_Region, 
                                             ref_ID_City = @_ref_ID_City, ref_ID_Street = @_ref_ID_Street,
                                             Number_home = @_Number_home, Letter_home = @_Letter_home, 
                                             Apartment_number = @_Apartment_number, Сase_number = @_Case_number
                                       WHERE ID_client = " + _current_id;

            dbCommand.Parameters.Add(new OleDbParameter("@_ref_ID_country", OleDbType.Numeric)).Value   = currentDataAddress.ID_Country;
            dbCommand.Parameters.Add(new OleDbParameter("@_ref_ID_Region", OleDbType.Numeric)).Value    = currentDataAddress.ID_Region;
            dbCommand.Parameters.Add(new OleDbParameter("@_ref_ID_City", OleDbType.Numeric)).Value      = currentDataAddress.ID_City;
            dbCommand.Parameters.Add(new OleDbParameter("@_ref_ID_Street", OleDbType.Numeric)).Value    = currentDataAddress.ID_Street;
            dbCommand.Parameters.Add(new OleDbParameter("@_Number_home", OleDbType.Numeric)).Value      = currentDataAddress.HomeNumber;
            dbCommand.Parameters.Add(new OleDbParameter("@_Letter_home", OleDbType.VarChar)).Value      = currentDataAddress.HouseLetter;
            dbCommand.Parameters.Add(new OleDbParameter("@_Apartment_number", OleDbType.Numeric)).Value = currentDataAddress.ApartmentNumber;
            dbCommand.Parameters.Add(new OleDbParameter("@_Case_number", OleDbType.Numeric)).Value      = currentDataAddress.СaseNumber;

            int codeResult = dbCommand.ExecuteNonQuery();
            string message = codeResult == 1 ? "Адрес изменен!" : "Что-то пошло не так";
            System.Windows.Forms.MessageBox.Show(message);
        }

        private void ClearFields()
        {
            textBox2.Clear();
            textBox1.Clear();
            textBox3.Clear();
            textBox4.Clear();
            numericUpDown1.Value    = 1;
            comboBox1.SelectedValue = -1;
            numericUpDown2.Value    = 0;
            numericUpDown3.Value    = 0;
        }

        private string CreateAddress(ref CurrentDataAddress currentDataAddress) 
        {
            StringBuilder addressLine = new StringBuilder();
            addressLine.Append($"Страна: {currentDataAddress.Country},");
            addressLine.Append($"Регион: {currentDataAddress.Region},");
            addressLine.Append($"Город: {currentDataAddress.City},");
            addressLine.Append($"Улица: {currentDataAddress.Street},");
            addressLine.Append($"Номер дома: {currentDataAddress.HomeNumber}");
            addressLine.Append(currentDataAddress.HouseLetter.Length == 0 ? "" : $",Литера дома: {currentDataAddress.HouseLetter}");
            addressLine.Append(currentDataAddress.ApartmentNumber == 0 ? "" : $",Номер квартиры: {currentDataAddress.ApartmentNumber}");

            return addressLine.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void AddressResidential_Load(object sender, EventArgs e)
        {

        }
    }
}
