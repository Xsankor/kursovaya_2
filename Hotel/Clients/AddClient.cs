using System;
using System.Data.OleDb;
using System.Windows.Forms;
using Hotel.Address;
using Hotel.Utilits;


namespace Hotel
{
    public partial class AddClient : Form
    {   
        private class CurrentData
        {
            public string FullName { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string Patronymic { get; set; }
            public string PlaceOfResidence { get; set; }
            public DateTime DateOfBirth;
            public int id_typeclient;
        };

        private AddressResidential.CurrentDataAddress addressData;

        public AddClient()
        {
            InitializeComponent();
            
            Utility.SomeFunction someFunction = FillComboBox;
            Utility.ConnectToDataBase(someFunction);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Utility.SomeFunction someFunction = CollectDataOfClient;
            Utility.ConnectToDataBase(someFunction);
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearFields();
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
            comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
        }

        private void CollectDataOfClient(OleDbCommand dbCommand)
        {
            CurrentData currentData = new CurrentData();

            Utility.SomeFunctionWithReturn someFunction = GetSelectedTypeClient;
            currentData.id_typeclient = Convert.ToInt32(Utility.ConnectToDataBase(someFunction));

            currentData.LastName = textBox3.Text;
            currentData.FirstName = textBox4.Text;
            currentData.Patronymic = textBox5.Text;
            currentData.PlaceOfResidence = textBox6.Text;
            currentData.DateOfBirth = dateTimePicker1.Value;

            int countError;

            CheckFields(out countError, ref currentData);

            if (countError > 0) return;

            currentData.FullName  = $"{currentData.LastName} {currentData.FirstName} {currentData.Patronymic}";
            dbCommand.CommandText = $" SELECT COUNT(*) " +
                                    $" FROM Сlients " +
                                    $" WHERE FullName LIKE '%{currentData.FullName}' AND LastName LIKE '%{currentData.LastName}' AND " +
                                    $" FirstName LIKE '%{currentData.FirstName}' AND DateOfBirth = @Date ";

            dbCommand.Parameters.Add(new OleDbParameter("@Date", OleDbType.DBDate)).Value = currentData.DateOfBirth;

            int codeResult = 0;
            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader())
            {
                dbDataReader.Read();
                codeResult = Convert.ToInt32(dbDataReader.GetValue(0));
                if (codeResult >= 1)
                {
                    MessageBox.Show("Такой клиент уже есть.");
                    return;
                }
                dbDataReader.Close();
            }

            dbCommand.Parameters.Clear();   
            dbCommand.CommandText = @"INSERT INTO Сlients(FullName, ref_ID_type_client, LastName, FirstName, Patronymic, 
                                      PlaceOfResidence, DateOfBirth, ref_ID_country, ref_ID_Region, ref_ID_City, ref_ID_Street,
                                      Number_home, Letter_home, Apartment_number, Case_number)
                                      VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

            dbCommand.Parameters.Add(new OleDbParameter("@FullName", OleDbType.VarChar)).Value              = currentData.FullName;
            dbCommand.Parameters.Add(new OleDbParameter("@ref_ID_type_client", OleDbType.Numeric)).Value    = currentData.id_typeclient;
            dbCommand.Parameters.Add(new OleDbParameter("@LastName", OleDbType.VarChar)).Value              = currentData.LastName;
            dbCommand.Parameters.Add(new OleDbParameter("@FirstName", OleDbType.VarChar)).Value             = currentData.FirstName;
            dbCommand.Parameters.Add(new OleDbParameter("@Patronymic", OleDbType.VarChar)).Value            = currentData.Patronymic;
            dbCommand.Parameters.Add(new OleDbParameter("@PlaceOfResidence", OleDbType.VarChar)).Value      = currentData.PlaceOfResidence;
            dbCommand.Parameters.Add(new OleDbParameter("@DateOfBirth", OleDbType.DBDate)).Value            = currentData.DateOfBirth;
            dbCommand.Parameters.Add(new OleDbParameter("@ref_ID_country", OleDbType.Numeric)).Value        = addressData.ID_Country;
            dbCommand.Parameters.Add(new OleDbParameter("@ref_ID_Region", OleDbType.Numeric)).Value         = addressData.ID_Region;
            dbCommand.Parameters.Add(new OleDbParameter("@ref_ID_City", OleDbType.Numeric)).Value           = addressData.ID_City;
            dbCommand.Parameters.Add(new OleDbParameter("@ref_ID_Street", OleDbType.Numeric)).Value         = addressData.ID_Street;
            dbCommand.Parameters.Add(new OleDbParameter("@Number_home", OleDbType.Numeric)).Value           = addressData.HomeNumber;
            dbCommand.Parameters.Add(new OleDbParameter("@Letter_home", OleDbType.VarChar)).Value           = addressData.HouseLetter;
            dbCommand.Parameters.Add(new OleDbParameter("@Apartment_number", OleDbType.Numeric)).Value      = addressData.ApartmentNumber;
            dbCommand.Parameters.Add(new OleDbParameter("@Case_number", OleDbType.Numeric)).Value           = addressData.СaseNumber;

            codeResult = dbCommand.ExecuteNonQuery();
            string message = codeResult == 1 ? "Добавлен новый клиент!" : "Что-то пошло не так";
            System.Windows.Forms.MessageBox.Show(message);

            ClearFields();
        }

        private void CheckFields(out int countError, ref CurrentData currentData) 
        {
            countError = 0;

            if (currentData.LastName.Length == 0)
            {
                ++countError;
                errorProvider3.SetError(textBox3, "Заполните поле!");
                errorProvider3.SetIconAlignment(textBox3, ErrorIconAlignment.MiddleRight);
            }
            else
                errorProvider3.Clear();

            if (currentData.FirstName.Length == 0)
            {
                ++countError;
                errorProvider4.SetError(textBox4, "Заполните поле!");
                errorProvider4.SetIconAlignment(textBox4, ErrorIconAlignment.MiddleRight);
            }
            else
                errorProvider4.Clear();

            if (!Utility.IsAdult(currentData.DateOfBirth))
            {
                ++countError;
                errorProvider1.SetError(dateTimePicker1, "Возраст пользователя должен быть не меньше 18 лет!");
                errorProvider1.SetIconAlignment(dateTimePicker1, ErrorIconAlignment.MiddleRight);
            }
            else
                errorProvider1.Clear();
        }

        private void ClearFields() 
        {
            comboBox1.SelectedIndex = 2;
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            dateTimePicker1.Value = DateTime.Now;
        }

        private string GetSelectedTypeClient(OleDbCommand dbCommand)
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

        private void button5_Click(object sender, EventArgs e)
        {
            AddressResidential address = new AddressResidential(0);
            address.ShowDialog();
            addressData = address.currentDataAddress;
            textBox6.Text = address.Address;
        }
    }
}
