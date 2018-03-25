using System;
using System.Collections.Generic;
using System.Text;

namespace Canary.Battery.Abstraction
{
    public class CanaryException : Exception
    {
        public CanaryException(string message) : base(message)
        {
        }
    }
}
