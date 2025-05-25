using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Core.HeaderContract
{
    class Contract : IContract
    {
        public required HeaderRequirement[] HeaderRequirements { get; init; }

        public Task<bool> ValidateAsync(string headers)
        {
            throw new NotImplementedException();
        }
    }
}
