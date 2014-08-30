using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceTestStudio.Core
{
    /// <summary>
    /// Interface to generalize all controls in the application.
    /// </summary>
    public interface ITestStudioControl
    {
        void Initialize();

        string Label { get; set; }

        /// <summary>
        /// The binding object on the control.
        /// </summary>
        object Content { get; set; }

        /// <summary>
        /// The selected item if Content is a collection.
        /// </summary>
        object SelectedContentItem { get; set; }

    }
}
