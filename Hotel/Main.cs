using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;
using Hotel.Employee;
using Hotel.Orders;
using Hotel.Utilits;
using Hotel.Additional;

namespace Hotel
{
    public partial class Main : Form
    {
        private int clickedRoomNumber;
        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FormControl();
        }
        private void FormControl() 
        {
            Utility.SomeFunction someFunction = GetStatusRoom;
            Utility.ConnectToDataBase(someFunction);
        }
        private void GetStatusRoom(OleDbCommand dbCommand) 
        {
            dbCommand.CommandText = "SELECT COUNT(*) FROM Rooms";
            int countRooms = Convert.ToInt32(dbCommand.ExecuteScalar());
            dbCommand.CommandText = "SELECT RoomIsBusy, RoomNumber FROM Rooms";
            using(OleDbDataReader dbDataReader = dbCommand.ExecuteReader()) 
            {
                flowLayoutPanel1.Controls.Clear();
                for (int i = 0; i < countRooms; ++i) 
                {
                    flowLayoutPanel1.Controls.Add(new Button());
                }
                foreach(Button btn in flowLayoutPanel1.Controls) 
                {
                    dbDataReader.Read();
                    btn.Size = new Size(width: 117, height: 52);
                    if (dbDataReader["RoomIsBusy"].ToString() == "False")
                    {
                        btn.BackColor = Color.White;
                        btn.Click += new System.EventHandler(this.SetupBotton);
                    }
                    else 
                    {
                        btn.BackColor = Color.Firebrick;
                    } 
                    btn.Text = dbDataReader["RoomNumber"].ToString();
                    btn.Font = new Font("Times New Roman", 12f, FontStyle.Bold);
                    
                }
                dbDataReader.Close();
            }
        }

        private void SetupBotton(object sender, EventArgs e) 
        {
            Button button = (Button)sender;
            clickedRoomNumber = Convert.ToInt32(button.Text);
            AddOrder addOrder = new AddOrder(clickedRoomNumber);
            addOrder.ShowDialog();
            FormControl();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clients clientsForm = new Clients();
            clientsForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Employees employeeForm = new Employees();
            employeeForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Hotel.Orders.Orders ordersForm = new Hotel.Orders.Orders();
            ordersForm.ShowDialog();
            FormControl();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Hotel.Additional.Additional additional = new Hotel.Additional.Additional();
            additional.ShowDialog();
        }
    }
}
