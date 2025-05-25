using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataManager.Core.HeaderContract
{
    readonly struct HeaderRequirement()
    {
        public required string HeaderName { get; init; }
        public Regex? ValueFormatRequrement { get; init; }
        public bool IsRequired { get; init; } = true;
    }
}
