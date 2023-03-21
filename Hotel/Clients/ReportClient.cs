using Microsoft.Reporting.WinForms;
using System;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.Configuration;

namespace Hotel
{
    public partial class ReportClient : Form
    {
        public ReportClient()
        {
            InitializeComponent();
        }

        private void ReportClient_Load(object sender, EventArgs e)
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
            DateTime date1 = dateTimePicker1.Value.Date;
            DateTime date2 = dateTimePicker2.Value.Date;

            //reportViewer1.LocalReport.ReportPath = @"..\..\..\..\Hotel\Clients\ReportClient.rdlc";
            reportViewer1.LocalReport.ReportPath = @".\Clients\ReportClient.rdlc";
            System.Data.DataSet dataSet = new System.Data.DataSet("DataSet1");
            string text;
            if (date1 == DateTime.Now.Date && date2 == DateTime.Now.Date)
            {
                text = "SELECT * FROM [Сlients] ";
            }
            else 
            {
                text = "SELECT * FROM [Сlients] WHERE DateOfBirth " +
                      $" BETWEEN #{date1.ToString("MM.dd.yyyy").Replace('.', '/')}# AND #{date2.ToString("MM.dd.yyyy").Replace('.', '/')}# ";
            }
            string CommandText = text; 
            string TableName = "ClientsReport";
            GetClientsData(ref dataSet, ref CommandText, ref TableName);

            CommandText = "SELECT * FROM [TypeClient]";
            TableName = "TypeClient";
            GetClientsData(ref dataSet, ref CommandText, ref TableName);

            ReportDataSource source = new ReportDataSource("DataSet1", dataSet.Tables["ClientsReport"]);
            ReportDataSource source1 = new ReportDataSource("DataSet2", dataSet.Tables["TypeClient"]);

            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(source);
            reportViewer1.LocalReport.DataSources.Add(source1);

            reportViewer1.RefreshReport();
        }
    }
}
