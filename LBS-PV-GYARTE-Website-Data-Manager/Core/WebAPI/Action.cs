using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Core.WebAPI
{
    class Action
    {
        public required HttpMethod Method { get; init; }

        public Action(object jsonSerailizable)
        {

        }
    }
}
