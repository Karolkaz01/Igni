﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin
{
    public interface IPlugin
    {
        void InitializePluggin();
        void PerformPluggin();
        bool RunContinues();
    }
}
