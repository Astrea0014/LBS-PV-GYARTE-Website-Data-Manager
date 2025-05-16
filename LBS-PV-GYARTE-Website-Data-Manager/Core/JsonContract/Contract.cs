using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Core.JsonContract
{
    class Contract
    {
        public required FieldRequirement[] FieldRequirements { get; init; }
        public required FieldRequirementGroup[] FieldRequirementGroups { get; init; }

        public bool Validate(string json)
        {
            throw new NotImplementedException();
        }
    }
}
