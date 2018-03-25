using Canary.Battery.Abstraction;
using System;
namespace Canary.Battery
{
    public static class CanaryBattery
    {
        static Lazy<ICnrBattery> _implementation = new Lazy<ICnrBattery>(() => CreateVibrate(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        public static ICnrBattery Current
        {
            get
            {
                var ret = _implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        static ICnrBattery CreateVibrate()
        {
#if NETSTANDARD2_0
            return null;
#else
            return new CnrBattery();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the Xam.Plugins.Vibrate NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
