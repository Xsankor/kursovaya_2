using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotel.Employee
{
    public partial class ReportEmployee : Form
    {
        public ReportEmployee()
        {
            InitializeComponent();
        }

        private void ReportEmployee_Load(object sender, EventArgs e)
        {
            CollectDataAndView();
        }
        private void GetClientsData(ref System.Data.DataSet dataset, ref string commandText, ref string tableName)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                OleDbCommand command = new OleDbCommand(commandText, connection);
                OleDbDataAdapter dbCommandAdapter = new OleDbDataAdapter(command);
                dbCommandAdapter.Fill(dataset, tableName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CollectDataAndView();
        }

        private void CollectDataAndView()
        {
            //reportViewer1.LocalReport.ReportPath = @"..\..\..\..\Hotel\Employee\ReportEmployee.rdlc";

            reportViewer1.LocalReport.ReportPath = @".\Employee\ReportEmployee.rdlc";

            System.Data.DataSet dataSet = new System.Data.DataSet("DataSet1");
            string CommandText = "SELECT * FROM Employees";
            string TableName = "EmployeeReport";
            GetClientsData(ref dataSet, ref CommandText, ref TableName);
            
            CommandText = "SELECT * FROM [Position]";
            TableName = "Position";
            GetClientsData(ref dataSet, ref CommandText, ref TableName);

            ReportDataSource source = new ReportDataSource("DataSet1", dataSet.Tables["EmployeeReport"]);
            ReportDataSource source1 = new ReportDataSource("DataSet2", dataSet.Tables["Position"]);

            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(source);
            reportViewer1.LocalReport.DataSources.Add(source1);

            reportViewer1.RefreshReport();
        }
    }
}
