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

namespace Hotel.Orders
{
    public partial class ReportOrder : Form
    {
        public ReportOrder()
        {
            InitializeComponent();
        }

        private void ReportOrder_Load(object sender, EventArgs e)
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
            //reportViewer1.LocalReport.ReportPath = @"..\..\..\..\Hotel\Orders\ReportOrders.rdlc";
            reportViewer1.LocalReport.ReportPath = @".\Orders\ReportOrders.rdlc";

            System.Data.DataSet dataSet = new System.Data.DataSet("DataSet1");
            string CommandText = "SELECT * FROM [Orders]";
            string TableName = "OrderReport";
            GetClientsData(ref dataSet, ref CommandText, ref TableName);

            CommandText = "SELECT * FROM [TypeClient]";
            TableName = "TypeClient";
            GetClientsData(ref dataSet, ref CommandText, ref TableName);

            CommandText = "SELECT * FROM [Сlients]";
            TableName = "Сlients";
            GetClientsData(ref dataSet, ref CommandText, ref TableName);

            ReportDataSource source  = new ReportDataSource("DataSet1", dataSet.Tables["OrderReport"]);
            ReportDataSource source1 = new ReportDataSource("DataSet3", dataSet.Tables["TypeClient"]);
            ReportDataSource source2 = new ReportDataSource("DataSet4", dataSet.Tables["Сlients"]);

            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(source);
            reportViewer1.LocalReport.DataSources.Add(source1);
            reportViewer1.LocalReport.DataSources.Add(source2);

            reportViewer1.RefreshReport();
        }
    }
}
