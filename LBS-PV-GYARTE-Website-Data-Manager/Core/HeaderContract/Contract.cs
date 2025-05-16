using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Core.HeaderContract
{
    class Contract
    {
        public required HeaderRequirement[] HeaderRequirements { get; init; }

        public bool Validate(string headers)
        {
            throw new NotImplementedException();
        }
    }
}
