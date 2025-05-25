using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Core.Json
{
    interface IJsonContentSerializable
    {
        /// <summary>
        /// When overridden in a derived class, serializes the current instance as an HTTP-request body in a JSON format.
        /// </summary>
        /// <returns>A JSON-formatted HTTP-body.</returns>
        JsonContent SerializeContent();
    }
}
