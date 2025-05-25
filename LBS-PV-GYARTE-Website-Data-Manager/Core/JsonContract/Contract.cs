using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Core.JsonContract
{
    class Contract : IContract
    {
        public required FieldRequirement[] FieldRequirements { get; init; }
        public required FieldRequirementGroup[] FieldRequirementGroups { get; init; }

        public Task<bool> ValidateAsync(string json)
        {
            throw new NotImplementedException();
        }
    }
}
