﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebServiceTestStudio.Core;

namespace WebServiceTestStudio.UserInterface
{
    public class TestStudioDataGrid : DataGridView, ITestStudioControl
    {
        public TestStudioDataGrid()
        {
        }



        #region ITestStudioControl Members

        public object Content
        {
            get { return DataSource; }
            set { DataSource = value; }
        }

        public void Initialize()
        {
            Dock = DockStyle.Fill;
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            //AllowUserToAddRows = false;            
        }

        public object SelectedContentItem
        {
            get
            {
                if (SelectedRows.Count > 0)
                    return SelectedRows[0].DataBoundItem;

                return null;
            }
            set
            {

            }
        }

        public string Label
        {
            get { return ((Control)this).Text; }
            set { ((Control)this).Text = value; }
        }

        #endregion
    }
}
