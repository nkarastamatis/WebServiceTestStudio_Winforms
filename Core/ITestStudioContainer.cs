using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceTestStudio.Core
{
    public interface ITestStudioContainer
    {
        void Initialize();
        void AddChild(ITestStudioControl child);
        ITestStudioControl SelectedChild { get; }
    }
}
