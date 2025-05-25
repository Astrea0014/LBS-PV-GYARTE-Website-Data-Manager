using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataManager.Core.WebAPI;

namespace DataManager.Core
{
    static class Global
    {
        public static readonly Client Client = new Client();

        public static async Task SubmitMasterAndAuthenticateAsync(string master, CancellationToken cancellationToken)
        {
            if (Client.IsAuthenticated)
                return;

            await Client.BeginSessionAsync(master, cancellationToken);
        }
    }
}
