﻿using MC.Track.TestSuite.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Services
{
    public class GenericScopingFactory : IGenericScopingFactory
    {
        public IScoper Create(Action dispose)
        {
            return new Scoper(dispose);
        }
    }
}
