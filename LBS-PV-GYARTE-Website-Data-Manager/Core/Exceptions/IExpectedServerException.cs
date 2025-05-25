using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Core.Exceptions
{
    interface IExpectedServerException
    {
        string ServerExceptionMessage { get; }
    }
}
