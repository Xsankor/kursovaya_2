using Hotel.Utilits;
using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Hotel.Orders
{
    public partial class UpdateOrder : Form
    {
        private enum StatePage
        {
            LOAD,
            UPDATE
        }
        private enum StateData 
        {
            CLIENT,
            EMPLOYEE,
            ROOM
        }
        private int currentOrderID;
        private struct CurrentData
        {
            public string FullName { get; set; }
            public string Emp_FullName { get; set; }
            public int RoomNumber { get; set; }
            public DateTime DateArrival { get; set; }
            public DateTime DateDeparture { get; set; }
            public DateTime TimeArrival { get; set; }
            public DateTime TimeDeparture { get; set; }
            public int id_client { get; set; }
            public int id_employee { get; set; }
            public int id_room { get; set; }
            public int source_id_room { get; set; }
        };
        private CurrentData currentData;
        private string textQuery;
        public UpdateOrder(int ID)
        {
            InitializeComponent();
            this.currentOrderID = ID;
            currentData = new CurrentData();
        }

        private void UpdateOrder_Load(object sender, EventArgs e)
        {
            FormControl();
        }

        private void FormControl(StatePage currentState = StatePage.LOAD)
        {
            Utility.SomeFunction someFunction;
            if (currentState == StatePage.UPDATE)
            {
                someFunction = UpdateOrderData;
            }
            else
            {
                someFunction = LoadOrderData;
            }
            Utility.ConnectToDataBase(someFunction);
        }

        private void LoadOrderData(OleDbCommand dbCommand)
        {
            dbCommand.CommandText = @" SELECT ID_order, ArrivalDate, DepartureOfDate, FullName, Emp_FullName,
                                              RoomNumber, ArrivalTime, DepartureTime, ref_ID_client, ref_ID_employee, ref_ID_room
                                       FROM ((Orders
                                       INNER JOIN [Сlients] ON Orders.ref_ID_client = [Сlients].ID_client)
                                       INNER JOIN Employees ON Orders.ref_ID_employee = [Employees].ID_employee)
                                       INNER JOIN Rooms ON Orders.ref_ID_room = [Rooms].ID_room
                                       WHERE ID_order = " + currentOrderID;

            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader())
            {
                dbDataReader.Read();
                textBox1.Text = dbDataReader["FullName"].ToString();
                textBox2.Text = dbDataReader["Emp_FullName"].ToString();
                textBox3.Text = dbDataReader["RoomNumber"].ToString();
                dateTimePicker1.Value = Convert.ToDateTime(dbDataReader["ArrivalDate"]);
                dateTimePicker2.Value = Convert.ToDateTime(dbDataReader["DepartureOfDate"]);
                dateTimePicker3.Value = Convert.ToDateTime(dbDataReader["ArrivalTime"]);
                dateTimePicker4.Value = Convert.ToDateTime(dbDataReader["DepartureTime"]);
                currentData.id_client = Convert.ToInt32(dbDataReader["ref_ID_client"]);
                currentData.id_employee = Convert.ToInt32(dbDataReader["ref_ID_employee"]);
                currentData.id_room = Convert.ToInt32(dbDataReader["ref_ID_room"]);
                currentData.source_id_room = Convert.ToInt32(dbDataReader["ref_ID_room"]);
                dbDataReader.Close();
            }
        }

        private void UpdateOrderData(OleDbCommand dbCommand)
        {
            FillCurrentOrderData();

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

            dbCommand.CommandText = @"UPDATE Orders 
                                         SET ref_ID_client = @_id_client, ref_ID_employee = @_id_employee, ref_ID_room = @_id_room, 
                                             ArrivalDate = @_ArrivalDate, DepartureOfDate = @_DepartureDate, 
                                             ArrivalTime = @_ArrivalTime, DepartureTime = @_DepartureTime,
                                       WHERE ID_order = " + currentOrderID;

            dbCommand.Parameters.Add(new OleDbParameter("@_id_client", OleDbType.Numeric)).Value    = currentData.id_client;
            dbCommand.Parameters.Add(new OleDbParameter("@_id_employee", OleDbType.Numeric)).Value  = currentData.id_employee;
            dbCommand.Parameters.Add(new OleDbParameter("@_id_room", OleDbType.Numeric)).Value      = currentData.id_room;
            dbCommand.Parameters.Add(new OleDbParameter("@_ArrivalDate", OleDbType.Date)).Value     = currentData.DateArrival;
            dbCommand.Parameters.Add(new OleDbParameter("@_DepartureDate", OleDbType.Date)).Value   = currentData.DateDeparture;
            dbCommand.Parameters.Add(new OleDbParameter("@_ArrivalTime", OleDbType.DBTime)).Value   = currentData.TimeArrival.TimeOfDay;
            dbCommand.Parameters.Add(new OleDbParameter("@_DepartureTime", OleDbType.DBTime)).Value = currentData.TimeDeparture.TimeOfDay;

            int codeResult = dbCommand.ExecuteNonQuery();

            if (currentData.id_room != currentData.source_id_room) 
            {
                dbCommand.Parameters.Clear();
                dbCommand.CommandText = $"UPDATE Rooms SET RoomIsBusy = @_NotBusy WHERE ID_room = {currentData.source_id_room}";
                dbCommand.Parameters.Add(new OleDbParameter("@_NotBusy", OleDbType.Boolean)).Value = false;
                dbCommand.ExecuteNonQuery();
                dbCommand.Parameters.Clear();
                dbCommand.CommandText = $"UPDATE Rooms SET RoomIsBusy = @_Busy WHERE ID_room = {currentData.id_room}";
                dbCommand.Parameters.Add(new OleDbParameter("@_Busy", OleDbType.Boolean)).Value = true;
                dbCommand.ExecuteNonQuery();           
            }

            string message = codeResult == 1 ? "Данные обновлены!" : "Что-то пошло не так";
            System.Windows.Forms.MessageBox.Show(message);
        }

        private void FillCurrentOrderData()
        {
            currentData.FullName        = textBox1.Text;
            currentData.Emp_FullName    = textBox2.Text;
            currentData.RoomNumber      = Convert.ToInt32(textBox3.Text);
            currentData.DateArrival     = dateTimePicker1.Value;
            currentData.DateDeparture   = dateTimePicker2.Value;
            currentData.TimeArrival     = dateTimePicker3.Value;
            currentData.TimeDeparture   = dateTimePicker4.Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StatePage currentState = StatePage.UPDATE;
            FormControl(currentState);
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ListClients listClients = new ListClients();
            listClients.ShowDialog();
            if (Utility.selected_ID_Client < 0) return;
            StateData currentState = StateData.CLIENT;
            textBox1.Text = GetSelectedData(currentState);
            currentData.id_client = Utility.selected_ID_Client;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ListEmployees listEmployees = new ListEmployees(Utility.StateShowList.ORDER);
            listEmployees.ShowDialog();
            if (Utility.selected_ID_Employee < 0) return;
            StateData currentState = StateData.EMPLOYEE;
            textBox2.Text = GetSelectedData(currentState);
            currentData.id_employee = Utility.selected_ID_Employee;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ListRooms listRooms = new ListRooms();
            listRooms.ShowDialog();
            if (Utility.selected_ID_Room < 0) return;
            StateData currentState = StateData.ROOM;
            textBox3.Text = GetSelectedData(currentState);
            currentData.id_room = Utility.selected_ID_Room;
        }

        private string GetSelectedData(StateData state)
        {
            switch (state)
            {
                case StateData.CLIENT:
                    {
                        textQuery = $"SELECT FullName FROM Сlients WHERE ID_client = {Utility.selected_ID_Client}"; // ОЧЕНЬ ПЛОХО
                    }
                    break;
                case StateData.EMPLOYEE:
                    {
                        textQuery = $"SELECT Emp_FullName FROM Employees WHERE ID_employee = {Utility.selected_ID_Employee}"; // ОЧЕНЬ ПЛОХО
                    }
                    break;
                case StateData.ROOM:
                    {
                        textQuery = $"SELECT RoomNumber FROM Rooms WHERE ID_room = {Utility.selected_ID_Room}"; // ОЧЕНЬ ПЛОХО
                    }   
                    break;
            }

            Utility.SomeFunctionWithReturn someFunctionWithReturn = GetTextResult;
            string result = Utility.ConnectToDataBase(someFunctionWithReturn);
            return result;
        }

        private string GetTextResult(OleDbCommand dbCommand) 
        {
            string textResult;
            dbCommand.CommandText = textQuery;
            using (OleDbDataReader dbDataReader = dbCommand.ExecuteReader())
            {
            dbDataReader.Read();
            textResult = dbDataReader[0].ToString();
            }
            return textResult;
        }
    }
}
