#region Copyright Syncfusion Inc. 2001-2018.
// Copyright Syncfusion Inc. 2001-2018. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.WinForms.Controls;
using Syncfusion.WinForms.DataGrid.Styles;
using System.Windows.Forms;
using Syncfusion.WinForms.DataGrid.Enums;
using System.Drawing;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.Data;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System;
using System.IO;
using Syncfusion.WinForms.DataGrid.Interactivity;

namespace ColumnChooser
{   
    public partial class Form1 : Form
    {
        #region 

        /// <summary>
        /// Specifies the ColumnChooser control.
        /// </summary>
        private ColumnChooserExt columnChooser;

        /// <summary>
        /// Specifies the column chooser pop up.
        /// </summary>
        private ColumnChooserPopupExt columnChooserPopup;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the new instance of the Form1 class.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            var data = new OrderInfoCollection();
            sfDataGrid.DataSource = data.OrdersListDetails;
            sfDataGrid.AutoSizeColumnsMode = AutoSizeColumnsMode.AllCells;
            columnChooser = new ColumnChooserExt(this.sfDataGrid);
            columnChooser.Location = new Point(780, 120);
            this.Controls.Add(columnChooser);
            columnChooserPopup = new ColumnChooserPopupExt(sfDataGrid);
            columnChooser.Anchor = AnchorStyles.Right | AnchorStyles.Top;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Displays the CoumnChooserPopup control.
        /// </summary>
        /// <param name="sender">The object of the sender.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains event data.</param>
        private void OnShowColumnChooserPopupClick(object sender, System.EventArgs e)
        {
            columnChooserPopup.Show();
        }

        /// <summary>
        /// Updates the visibility of the column chooser control.
        /// </summary>
        /// <param name="sender">The object of the sender.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains event data.</param>
        private void OnShowColumnChooserCheckedChanged(object sender, EventArgs e)
        {
            columnChooser.Visible = chkShowColumnChooser.Checked;
        }

        #endregion
    }
    public class ColumnChooserPopupExt : Syncfusion.WinForms.DataGrid.Interactivity.ColumnChooserPopup
    {
        private SfDataGrid DataGrid { get; set; }
        public ColumnChooserPopupExt(SfDataGrid sfDataGrid) : base(sfDataGrid)
        {
            this.Controls.Remove(this.ColumnChooser);
            DataGrid = sfDataGrid;
            ColumnChooser = new ColumnChooserExt(sfDataGrid);

            ////UnComment this if you want to close the popup when OK button is clicked
            //ColumnChooser.OKButton.Click += Close_ColumnChooserPopup;

            ColumnChooser.CancelButton.Click += Close_ColumnChooserPopup;
            ColumnChooser.BorderStyle = BorderStyle.None;
            ColumnChooser.ColumnChooserLabel.Visible = false;
            this.Controls.Add(ColumnChooser);
        }

        private void Close_ColumnChooserPopup(object sender, EventArgs e)
        {
            if (this.DataGrid.GetTopLevelParentDataGrid().FindForm() != null)
                this.DataGrid.GetTopLevelParentDataGrid().FindForm().Focus();
            this.Hide();
        }
    }

    public class ColumnChooserExt : Syncfusion.WinForms.DataGrid.Interactivity.ColumnChooser
    {
        /// <summary>
        /// The grid attached with the column chooser.
        /// </summary>
        private SfDataGrid dataGrid;

        /// <summary>
        /// Initializes a new instance of the sfDataGrid
        /// </summary>
        /// <param name="sfDataGrid">The data grid which needs to be attached to the control.</param>
        public ColumnChooserExt(SfDataGrid sfDataGrid) : base(sfDataGrid)
        {
            dataGrid = sfDataGrid;
            dataGrid.Columns.CollectionChanged += Columns_CollectionChanged;
        }

        private void Columns_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.CheckedListBox.DataSource = dataGrid.Columns.Where(x => x.MappingName != "OrderID" && x.MappingName != "CustomerID");
        }

        protected override void AddCheckedListBox()
        {
            base.AddCheckedListBox();

            this.CheckedListBox.DataSource = dataGrid.Columns.Where(x => x.MappingName != "OrderID" && x.MappingName != "CustomerID");
        }

        protected override void OnSearchBoxTextChanged(object sender, EventArgs e)
        {
            base.OnSearchBoxTextChanged(sender, e);

            List<GridColumn> searchedItems = new List<GridColumn>();
            if (!string.IsNullOrEmpty(this.SearchTextBox.Text))
            {
                searchedItems.Clear();
                searchedItems = this.dataGrid.Columns.Where(item => (item.HeaderText.ToLower().Contains(this.SearchTextBox.Text.ToLower()) && item.MappingName != "OrderID" && item.MappingName != "CustomerID")).ToList();
                this.CheckedListBox.DataSource = null;
                this.CheckedListBox.DataSource = searchedItems;
            }
            else
                this.CheckedListBox.DataSource = dataGrid.Columns.Where(x => x.MappingName != "OrderID" && x.MappingName != "CustomerID");

            this.CheckedListBox.Invalidate();

        }
    }
}
