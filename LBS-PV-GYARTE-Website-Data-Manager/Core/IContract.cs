﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Core
{
    interface IContract
    {
        Task<bool> ValidateAsync(string data);
    }
}
