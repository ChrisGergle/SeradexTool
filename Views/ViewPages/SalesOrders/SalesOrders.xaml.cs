﻿using LSGDatabox.ViewModels;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LSGDatabox.Views.ViewPages.SalesOrders
{
    /// <summary>
    /// Interaction logic for SalesOrders.xaml
    /// </summary>
    public partial class SalesOrders : Page
    {
        Toolkit Utility = new Toolkit();

        public SalesOrders()
        {
            InitializeComponent();

        }

        DataTable Data = new DataTable("SalesOrderSummary");
        DataView View;

        //bool Loaded=false;



        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string fillString = "SELECT SalesOrder.SalesOrderID, SalesOrderNo, Estimate.EstimateNo, " +
                "SalesOrder.CustRefNo, c.[Name] as [Customer Name], con.[Name] as [Contact Name], " +
                "city.DescriptionShort as [City Name], st.StateProvCode as [State], SalesOrder.SubTotal, SalesOrder.TotalTaxes, " +
                "SalesOrder.EntryDate, SalesOrder.DueDate, e.UserName, SalesOrder.TermsCodeID " +
                "FROM SalesOrder " +
                "Inner Join Customers c on SalesOrder.CustomerID = c.CustomerID " +
                "Inner join CustomerShipTo b on SalesOrder.CustomerShipToID = b.CustomerShipToID " +
                "Inner Join Addresses a on b.AddressID = a.AddressID " +
                "Inner Join Cities city on a.CityID = city.CityID " +
                "Inner Join StateProv st on a.StateProvID = st.StateProvID " +
                "LEFT OUTER JOIN Estimate on Estimate.EstimateID = SalesOrder.EstimateID " +
                "Inner Join SalesReps sr on SalesOrder.SalesRepID = sr.SalesRepID " +
                "Inner Join Employees e on sr.EmployeeID = e.EmployeeID " +
                "Inner Join Contacts con on SalesOrder.ContactID = con.ContactID;";



            //Data = Utility.populateEstimatesTable();
            Data = Utility.useQuery(fillString);
            View = new DataView(Data);
            SalesOrderGrid.ItemsSource = View;
            SalesOrderGrid.Columns[0].Visibility = Visibility.Hidden;
        }

        string searchString;
        private void executeSearch(object sender, KeyEventArgs e)
        {
            switch (SearchParam.SelectedIndex)
            {
                case 0:
                    searchString = "SalesOrderNo LIKE \'*" + SearchBox.Text + "*\'";
                    break;

                case 1:
                    searchString = "[Customer Name] LIKE \'*" + SearchBox.Text + "*\'";
                    break;

                case 2:
                    searchString = "[EstimateNo] LIKE \'*" + SearchBox.Text + "*\'";
                    break;

                case 3:
                    searchString = "[City Name] LIKE \'*" + SearchBox.Text + "*\'";
                    break;
            }

            View.RowFilter = searchString;
        }

        private void GridDoubleClick(object sender, RoutedEventArgs e)
        {
            findCell("SalesOrderID", View, SalesOrderGrid);
        }

        private void findCell(string s, DataView v, DataGrid g)
        {
            try
            {
                int y = g.SelectedIndex;
                DataRow passToNextWindow = v[y].Row;
                string answer = View[y][s].ToString();
                Window openWindow = new SalesOrderDetails(answer, passToNextWindow);
                openWindow.Show();
            }
            catch (Exception)
            { //MessageBox.Show("Double-clicked something before selecting a Sales order. \nTry selecting your sales order first.", "NoSo Error");

            }



        }

    }
}
