using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceTestStudio.Core
{
    /// <summary>
    /// Interface to make parent/container methods accessible.
    /// </summary>
    public interface ITestStudioDisplay
    {
        void Close();
        void Focus();
    }
}
