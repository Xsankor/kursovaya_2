using Hotel.Address;
using Hotel.Utilits;
using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Hotel
{
    public partial class UpdateClient : Form
    {
        private enum State 
        {
            LOAD,
            UPDATE
        }
        private int currentClientID;
        private struct CurrentData
        {
            public string FullName { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string Patronymic { get; set; }
            public string PlaceOfResidence { get; set; }
            public DateTime DateOfBirth;
            public int id_typeClient;
        };

        private CurrentData currentData;

        private AddressResidential.CurrentDataAddress addressData;

        public UpdateClient(int ID)
        {
            InitializeComponent();
            this.currentClientID = ID;
            currentData = new CurrentData();
        }

        private void UpdateClient_Load(object sender, EventArgs e)
        {
            FormControl();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            State currentState = State.UPDATE;
            FormControl(currentState);
            this.Close();
        }

        private void FormControl(State currentState = State.LOAD)
        {
            Utility.SomeFunction someFunction;
            if (currentState == State.UPDATE)
            {
                someFunction = UpdateClientData;
            }
            else 
            {
                someFunction = LoadClientData;
            }
            Utility.ConnectToDataBase(someFunction);
        }
        private void LoadClientData(OleDbCommand dbCommand) 
        {
            Utility.SomeFunction someFunction = FillComboBox;
            Utility.ConnectToDataBase(someFunction);
            dbCommand.CommandText = @"SELECT ID_client, TitleType, LastName, FirstName, Patronymic, DateOfBirth, PlaceOfResidence 
                                      FROM Сlients INNER JOIN TypeClient 
                                      ON Сlients.ref_ID_type_client = TypeClient.ID_type_client
                                      WHERE ID_client = " + currentClientID;

            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader())
            {
                dbDataReader.Read();
                comboBox1.SelectedItem = dbDataReader["TitleType"].ToString();
                textBox3.Text = dbDataReader["LastName"].ToString();
                textBox4.Text = dbDataReader["FirstName"].ToString();
                textBox5.Text = dbDataReader["Patronymic"].ToString();
                textBox6.Text = dbDataReader["PlaceOfResidence"].ToString();
                dateTimePicker1.Value = Convert.ToDateTime(dbDataReader["DateOfBirth"]);
                dbDataReader.Close();
            }
        }

        private void UpdateClientData(OleDbCommand dbCommand) 
        {
            FillCurrentClientData();
            dbCommand.CommandText = @"UPDATE Сlients 
                                         SET FullName = @_FullName, ref_ID_type_client = @_ref_ID_type_client, LastName = @_LastName, 
                                             FirstName = @_FirstName, Patronymic = @_Patronymic, PlaceOfResidence = @_PlaceOfResidence, 
                                             DateOfBirth = @_DateOfBirth, ref_ID_country = @_ref_ID_country, ref_ID_Region = @_ref_ID_Region, 
                                             ref_ID_City = @_ref_ID_City, ref_ID_Street = @_ref_ID_Street,
                                             Number_home = @_Number_home, Letter_home = @_Letter_home, 
                                             Apartment_number = @_Apartment_number, Сase_number = @_Case_number
                                       WHERE ID_client = " + currentClientID; 

            dbCommand.Parameters.Add(new OleDbParameter("@_FullName", OleDbType.VarChar)).Value             = currentData.FullName;
            dbCommand.Parameters.Add(new OleDbParameter("@_ref_ID_type_client", OleDbType.Numeric)).Value   = currentData.id_typeClient;
            dbCommand.Parameters.Add(new OleDbParameter("@_LastName", OleDbType.VarChar)).Value             = currentData.LastName;
            dbCommand.Parameters.Add(new OleDbParameter("@_FirstName", OleDbType.VarChar)).Value            = currentData.FirstName;
            dbCommand.Parameters.Add(new OleDbParameter("@_Patronymic", OleDbType.VarChar)).Value           = currentData.Patronymic;
            dbCommand.Parameters.Add(new OleDbParameter("@_PlaceOfResidence", OleDbType.VarChar)).Value     = currentData.PlaceOfResidence;
            dbCommand.Parameters.Add(new OleDbParameter("@_DateOfBirth", OleDbType.DBDate)).Value           = currentData.DateOfBirth;
            dbCommand.Parameters.Add(new OleDbParameter("@_ref_ID_country", OleDbType.Numeric)).Value       = addressData.ID_Country;
            dbCommand.Parameters.Add(new OleDbParameter("@_ref_ID_Region", OleDbType.Numeric)).Value        = addressData.ID_Region;
            dbCommand.Parameters.Add(new OleDbParameter("@_ref_ID_City", OleDbType.Numeric)).Value          = addressData.ID_City;
            dbCommand.Parameters.Add(new OleDbParameter("@_ref_ID_Street", OleDbType.Numeric)).Value        = addressData.ID_Street;
            dbCommand.Parameters.Add(new OleDbParameter("@_Number_home", OleDbType.Numeric)).Value          = addressData.HomeNumber;
            dbCommand.Parameters.Add(new OleDbParameter("@_Letter_home", OleDbType.VarChar)).Value          = addressData.HouseLetter;
            dbCommand.Parameters.Add(new OleDbParameter("@_Apartment_number", OleDbType.Numeric)).Value     = addressData.ApartmentNumber;
            dbCommand.Parameters.Add(new OleDbParameter("@_Case_number", OleDbType.Numeric)).Value          = addressData.СaseNumber;

            int codeResult = dbCommand.ExecuteNonQuery();
            string message = codeResult == 1 ? "Данные обновлены!" : "Что-то пошло не так";
            System.Windows.Forms.MessageBox.Show(message);
        }

        private void FillCurrentClientData() 
        {
            Utility.SomeFunction someFunction = FillComboBox;
            Utility.ConnectToDataBase(someFunction);

            currentData.LastName = textBox3.Text;
            currentData.FirstName = textBox4.Text;
            currentData.Patronymic = textBox5.Text;
            currentData.PlaceOfResidence = textBox6.Text;
            currentData.DateOfBirth = dateTimePicker1.Value;
            Utility.SomeFunctionWithReturn someFunctionWithReturn = GetSelectedPosition;
            currentData.id_typeClient = Convert.ToInt32(Utility.ConnectToDataBase(someFunctionWithReturn));
            currentData.FullName = $"{currentData.LastName} {currentData.FirstName} {currentData.Patronymic}";
        }

        private void FillComboBox(OleDbCommand dbCommand)
        {
            dbCommand.CommandText = "SELECT TitleType FROM TypeClient";

            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader())
            {
                while (dbDataReader.Read())
                {
                    comboBox1.Items.Add(dbDataReader.GetValue(0).ToString());
                }
                dbDataReader.Close();
            }
        }

        private string GetSelectedPosition(OleDbCommand dbCommand)
        {
            string selectedType = comboBox1.SelectedItem.ToString();
            dbCommand.CommandText = $"SELECT ID_type_client FROM [TypeClient] WHERE TitleType = '{selectedType}'";
            string result;
            using (OleDbDataReader oleDbDataReader = dbCommand.ExecuteReader())
            {
                oleDbDataReader.Read();
                result = oleDbDataReader[0].ToString();
                oleDbDataReader.Close();
            }
            return result;
        }

        private void button6_Click(object sender, EventArgs e) // обзор
        {
            AddressResidential address = new AddressResidential(currentClientID);
            address.ShowDialog();
            addressData = address.currentDataAddress;
            textBox6.Text = address.Address.Length != 0 ? address.Address : textBox6.Text;
        }
    }
}
