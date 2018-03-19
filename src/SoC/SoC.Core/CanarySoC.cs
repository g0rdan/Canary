using System;
namespace Canary.SoC
{
    public static class CanarySoC
    {
        static Lazy<ICnrSoC> _implementation = new Lazy<ICnrSoC>(() => CreateVibrate(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        public static ICnrSoC Current
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

        static ICnrSoC CreateVibrate()
        {
#if NETSTANDARD2_0
            return null;
#else
            return new CnrSoC();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the Xam.Plugins.Vibrate NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
