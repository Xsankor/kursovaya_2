using Hotel.Utilits;
using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Hotel.Orders
{
    public partial class AddOrder : Form
    {
        private int selectedRoomNumber = 0;
        private class CurrentData
        {
            public string FullName { get; set; }
            public string Emp_FullName { get; set; }
            public string RoomNumber { get; set; }

            public DateTime DateArrival;
            public DateTime DateDeparture;
            public DateTime TimeArrival;
            public DateTime TimeDeparture;
        };

        private enum State 
        {
            CLIENT,
            EMPLOYEE,
            ROOM
        }
        private string _textQuery;
        public AddOrder(int currentRoomNumber = 0)
        {
            InitializeComponent();
            selectedRoomNumber = currentRoomNumber;
        }

        private void AddOrder_Load(object sender, EventArgs e)
        {
            dateTimePicker3.Value = DateTime.Now;
            dateTimePicker4.Value = DateTime.Now;

            if (selectedRoomNumber != 0)
            {
                Utility.SomeFunction someFunction = SetupRoom;
                Utility.ConnectToDataBase(someFunction);
                button6.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Utility.SomeFunction someFunction = CollectDataOfOrder;
            Utility.ConnectToDataBase(someFunction);
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ListClients listClients = new ListClients();
            listClients.ShowDialog();
            if (Utility.selected_ID_Client < 0) return;
            State currentState = State.CLIENT;
            textBox1.Text = GetSelectedData(currentState);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ListEmployees listEmployees = new ListEmployees(Utility.StateShowList.ORDER);
            listEmployees.ShowDialog();
            if (Utility.selected_ID_Employee < 0) return;
            State currentState = State.EMPLOYEE;
            textBox2.Text = GetSelectedData(currentState);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ListRooms listRooms = new ListRooms();
            listRooms.ShowDialog();
            if (Utility.selected_ID_Room < 0) return;
            State currentState = State.ROOM;
            textBox3.Text = GetSelectedData(currentState);
        }

        private void ClearFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            dateTimePicker3.Value = DateTime.Now;
            dateTimePicker4.Value = DateTime.Now;
            button6.Enabled = true;
        }

        private string GetSelectedData(State state) 
        {
            switch (state) 
            {
                case State.CLIENT:
                    _textQuery = $"SELECT FullName FROM Сlients WHERE ID_client = {Utility.selected_ID_Client}";
                    break;
                case State.EMPLOYEE:
                    _textQuery = $"SELECT Emp_FullName FROM Employees WHERE ID_employee = {Utility.selected_ID_Employee}";
                    break;
                case State.ROOM:
                    _textQuery = $"SELECT RoomNumber FROM Rooms WHERE ID_room = {Utility.selected_ID_Room}";
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

        private void CollectDataOfOrder(OleDbCommand dbCommand)
        {
            CurrentData currentData = new CurrentData();

            currentData.FullName        = textBox1.Text;
            currentData.Emp_FullName    = textBox2.Text;
            currentData.RoomNumber      = textBox3.Text;
            currentData.DateArrival     = dateTimePicker1.Value;
            currentData.DateDeparture   = dateTimePicker2.Value;
            currentData.TimeArrival     = dateTimePicker3.Value;
            currentData.TimeDeparture   = dateTimePicker4.Value;

            if (currentData.DateDeparture < currentData.DateArrival)
            {
                MessageBox.Show("Дата записи не может быть больше даты выписки!");
                return;
            }
            else 
            {
                if (currentData.DateDeparture == currentData.DateArrival && 
                    currentData.TimeArrival >= currentData.TimeDeparture) 
                {
                    MessageBox.Show("Время записи не может быть больше или равно времени выписки!");
                    return;
                }
            }

            int countError = 0;
            if (currentData.FullName.Length == 0)
            {
                ++countError;
                errorProvider1.SetError(textBox1, "Заполните поле!");
                errorProvider1.SetIconAlignment(textBox1, ErrorIconAlignment.MiddleRight);
            }
            else
                errorProvider3.Clear();

            if (currentData.Emp_FullName.Length == 0)
            {
                ++countError;
                errorProvider2.SetError(textBox2, "Заполните поле!");
                errorProvider2.SetIconAlignment(textBox2, ErrorIconAlignment.MiddleRight);
            }
            else
                errorProvider2.Clear();

            if (currentData.RoomNumber.Length == 0)
            {
                ++countError;
                errorProvider3.SetError(textBox3, "Заполните поле!");
                errorProvider3.SetIconAlignment(textBox3, ErrorIconAlignment.MiddleRight);
            }
            else
                errorProvider3.Clear();

            if (countError > 0) return;

            dbCommand.Parameters.Clear();
            dbCommand.CommandText = @"INSERT INTO Orders(ref_ID_client, ref_ID_employee, ref_ID_room, 
                                                         ArrivalDate, ArrivalTime, DepartureOfDate, DepartureTime)
                                      VALUES(?, ?, ?, ?, ?, ?, ?)";

            dbCommand.Parameters.Add(new OleDbParameter("@ref_ID_client", OleDbType.Numeric)).Value = Utility.selected_ID_Client;
            dbCommand.Parameters.Add(new OleDbParameter("@ref_ID_employee", OleDbType.Numeric)).Value = Utility.selected_ID_Employee;
            dbCommand.Parameters.Add(new OleDbParameter("@ID_room", OleDbType.Numeric)).Value = Utility.selected_ID_Room;
            dbCommand.Parameters.Add(new OleDbParameter("@ArrivalDate", OleDbType.Date)).Value = currentData.DateArrival.Date;
            dbCommand.Parameters.Add(new OleDbParameter("@ArrivalTime", OleDbType.DBTime)).Value = currentData.TimeArrival.TimeOfDay;
            dbCommand.Parameters.Add(new OleDbParameter("@DepartureOfDate", OleDbType.Date)).Value = currentData.DateDeparture.Date;
            dbCommand.Parameters.Add(new OleDbParameter("@DepartureTime", OleDbType.DBTime)).Value = currentData.TimeDeparture.TimeOfDay;

            int codeResult = dbCommand.ExecuteNonQuery();
            string message = codeResult == 1 ? "Клиент заселён!" : "Что-то пошло не так";

            dbCommand.CommandText = $"UPDATE Rooms SET RoomIsBusy = @_isBusy WHERE ID_room = {Utility.selected_ID_Room}";
            dbCommand.Parameters.Add(new OleDbParameter("@_isBusy", OleDbType.Boolean)).Value = true;
            dbCommand.ExecuteNonQuery();
            System.Windows.Forms.MessageBox.Show(message);

            ClearFields();
        }

        private void SetupRoom(OleDbCommand dbCommand) 
        {
            dbCommand.CommandText = $"SELECT ID_room FROM Rooms WHERE RoomNumber = {selectedRoomNumber}";

            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader()) 
            {
                dbDataReader.Read();
                Utility.selected_ID_Room = Convert.ToInt32(dbDataReader["ID_room"]);
                dbDataReader.Close();
            }

            textBox3.Text = selectedRoomNumber.ToString();
        }
    }
}
