﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Services
{
    public interface ISmartResourceDestroyerServiceFactory
    {
        ISmartResourceDestroyerService<T> Create<T>(T resource, Action<T> disposeCallback);
    }
}
