using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebServiceTestStudio.Core;

namespace WebServiceTestStudio.UserInterface
{
    public class TestStudioPropertyGrid : PropertyGrid, ITestStudioControl
    {
        public TestStudioPropertyGrid()
        {
            Initialize();
            this.SelectedObjectsChanged += new System.EventHandler(this.propertyGrid_SelectedObjectsChanged);
        }

        private void propertyGrid_SelectedObjectsChanged(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.Control control in this.Controls[2].Controls)
                control.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.propertyGrid_PreviewKeyDown);
        }

        private void propertyGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                var gridItem = SelectedGridItem;
                if (gridItem.Expanded)
                {
                    if (gridItem.GridItems.Count > 0)
                        gridItem.GridItems[0].Select();
                }
                else
                {
                    bool found = false;
                    GridItem nextFocus = gridItem;
                    GridItem parent = gridItem.Parent;
                    while (!found && parent != null)
                    {
                        var enumerator = parent.GridItems.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            if (gridItem == enumerator.Current)
                            {
                                if (enumerator.MoveNext())
                                {
                                    nextFocus = (GridItem)enumerator.Current;
                                    found = true;
                                    break;
                                }
                            }

                        }

                        gridItem = parent;
                        parent = parent.Parent;
                    }

                    nextFocus.Select();
                    if (nextFocus.Expandable && !nextFocus.Expanded)
                        nextFocus.Expanded = true;
                }

                // use time to make editable
                timer = new Timer();
                timer.Interval = 1;
                timer.Tick += new EventHandler(timer_Tick);
                timer.Start();

            }
        }

        Timer timer;

        void timer_Tick(object sender, EventArgs e)
        {
            SendKeys.Send("{TAB}");
            timer.Stop();
        }


        #region ITestStudioControl Members

        public object Content
        {
            get 
            { 
                return base.SelectedObject; 
            }
            set
            {
                base.SelectedObject = value;
            }
        }

        public void Initialize()
        {
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertySort = PropertySort.NoSort;
            this.ToolbarVisible = false;            
        }

        public object SelectedContentItem
        {
            get { return SelectedGridItem.Value; }
            set { var x = value; }
        }

        public string Label
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        #endregion
    }
}
