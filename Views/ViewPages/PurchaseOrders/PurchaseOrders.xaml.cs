﻿using Microsoft.IdentityModel.Tokens;
using SeradexToolv2.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Security.Cryptography.X509Certificates;
using System.Data;

namespace SeradexToolv2.Views.ViewPages.PurchaseOrders
{
    /// <summary>
    /// Interaction logic for PurchaseOrders.xaml
    /// </summary>
    public partial class PurchaseOrders : Page
    {

        Toolkit Utility = new Toolkit();                // Access to useful functions and query builders for security
        string searchString;                            // Used to pass from UI to Encapsulated functions safely
        bool loaded;                                    // Prevents loading PO list from DB multiple times on a single open

        // Create the table and view here, assign it on load, and leave it public to filter and adjust. NEVER PASS THIS BACK TO DB.
        DataTable Data = new DataTable("PurchaseOrderSummary");
        DataView View;

        public PurchaseOrders()
        {
            InitializeComponent();
        }

        // Page Loaded - Make initial query for Purchase Orders

        private void oSearchFor_KeyUp(object sender, KeyEventArgs e)
        {
            // For each keypress, pass a filter into the Data grid.
            // Note that bugs occur when nothing gets selected in the grid
            // and the doubleclick feature is invoked.
            // Set default selection to position 0
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Run query for PO data

            // Hardcode the query for security and saftey of software and database
            string queryString = "SELECT " +
                "po.PONo, vend.[Name] [Vendor Name], city.DescriptionShort [City], st.DescriptionShort [State], " + // PO Number, Vendor name, CHECK CITY VS SERADEX, CHECK STATE VS SERADEX, 
                "po.TotalTaxes, po.SubTotal, po.TotalTaxes+po.SubTotal [Grand Total], po.Reference, po.DateCreated, po.DueDate, po.Completed, " +
                "po.Comment" +
                "FROM [dbo].[PO] po" +
                "LEFT OUTER JOIN [dbo].[Vendors] vend on po.VendorID = vend.VendorID" +
                "LEFT OUTER JOIN [dbo].[Addresses] ad on po.AddressID = ad.AddressID" +
                "Inner Join [dbo].[Cities] city on ad.CityID = city.CityID" +
                "Inner Join [dbo].[StateProv] st on ad.StateProvID = st.StateProvID" +
                "ORDER BY po.PONo";

            if(loaded == false)
            {
                Data = Utility.useQuery(queryString);
                View = new DataView(Data);
                PurchaseOrderGrid.ItemsSource = View; //Push view to our XAML Datagrid
                loaded = true; //Prevents multiple loads of same data.
            }

        }

        // Private search function
        //     Sets up a view filter using searchString assignment
        //     Get oSearchBy and oSearchFor
        //     oSearchBy = column to filter by
        //     oSearchFor = parameters to filter by
        //     The "LIKE" is wildcarded for better results
        /* Prebuild switch statement
            switch(oSearchBy.SelectedIndex)
            {
                case 0: // Purchase Order
                    searchString = "[PurchaseOrder] LIKE \'*" + oSearchFor.Text + "*\'";
                    break;

                        case 0: // Purchase Order
                    searchString = "[PurchaseOrder] LIKE \'*" + oSearchFor.Text + "*\'";
                    break;

                        case 1: // Sales Order
                    searchString = "[SalesOrder] LIKE \'*" + oSearchFor.Text + "*\'";
                    break;

                        case 2: // Invoice
                    searchString = "[InvoiceNum] LIKE \'*" + oSearchFor.Text + "*\'";
                    break;

                        case 3: // Customer
                    searchString = "[Customer] LIKE \'*" + oSearchFor.Text + "*\'";
                    break;

                        case 4: // City
                    searchString = "[City] LIKE \'*" + oSearchFor.Text + "*\'";
                    break;

                        case 5: // State
                    searchString = "[State] LIKE \'*" + oSearchFor.Text + "*\'";
                    break;
            }
            View.RowFilter = searchString;
        */

        //private find cell on Grid (string s, DataView v, DataGrid g)
        //      Opens up the details page in a separate window.
        //      Scan through the row to find the PO number cell
        //      Pass that information on through to the next window.



    }
}