using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace Canary.SoC
{
    /// <summary>
    /// TODO: http://blakespot.com/ios_device_specifications_grid.html
    /// </summary>
    public class CnrSoC : ICnrSoC
    {
        /// <summary>
        /// The key for getting information about model name (like "iphone 9,3") from sysctl
        /// </summary>
        const string MODEL_KEY = "hw.model";

        [DllImport(Constants.SystemLibrary)]
        static internal extern int sysctlbyname([MarshalAs(UnmanagedType.LPStr)] string property, IntPtr output, IntPtr oldLen, IntPtr newp, uint newlen);
        
        public string Model => GetCpuInfo(GetSystemProperty(MODEL_KEY)).Name;      
        /// <summary>
        /// Doesn't support in iOS
        /// </summary>
        public float CurrentFrequency => 0f;      
		/// <summary>
        /// Doesn't support in iOS
        /// </summary>
        public float MinFrequency => 0f;

        public float MaxFrequency
        {
            get 
            {
                float.TryParse(GetCpuInfo(GetSystemProperty(MODEL_KEY)).CPUClock, out float result);
                return result;
            }
        }

        public int Cores => (int)NSProcessInfo.ProcessInfo.ActiveProcessorCount;

        public Task<List<AdditionalInformation>> GetAdditionalInformationAsync(CancellationToken token = default(CancellationToken))
        {
            return Task.Factory.StartNew<List<AdditionalInformation>>(() =>
            {
                var data = GetCpuInfo(GetSystemProperty(MODEL_KEY));
                var list = new List<AdditionalInformation>();
                list.Add(new AdditionalInformation { Title = nameof(data.Architecture), Value = data.Architecture });
                list.Add(new AdditionalInformation { Title = nameof(data.Capacity), Value = data.Capacity });
                list.Add(new AdditionalInformation { Title = nameof(data.L1Cache), Value = data.L1Cache });
                list.Add(new AdditionalInformation { Title = nameof(data.L2Cache), Value = data.L2Cache });
                list.Add(new AdditionalInformation { Title = nameof(data.L3Cache), Value = data.L3Cache });
                return list;
            }, token);
        }

        public Task<UsageInformation> GetUsageAsync(CancellationToken token = default(CancellationToken))
        {
			return null;
        }

        /// <summary>
        /// Gets the system property.
        /// </summary>
        static string GetSystemProperty(string property)
        {
            var pLen = Marshal.AllocHGlobal(sizeof(int));
            sysctlbyname(property, IntPtr.Zero, pLen, IntPtr.Zero, 0);
            var length = Marshal.ReadInt32(pLen);
            var pStr = Marshal.AllocHGlobal(length);
            sysctlbyname(property, pStr, pLen, IntPtr.Zero, 0);
            return Marshal.PtrToStringAnsi(pStr);
        }

        CpuInfo GetCpuInfo(string modelName)
        {
            switch (modelName)
            {
                case "iPhone1,1":
                case "iPod1,1":
                    return new CpuInfo { CPUClock = "412", Capacity = "32", L1Cache = "16+16", L2Cache = "", L3Cache = "", Name = "Samsung S5L8900", Architecture = "ARMv6" };
                case "iPhone1,2":
                    return new CpuInfo { CPUClock = "412", Capacity = "32", L1Cache = "16+16", L2Cache = "", L3Cache = "", Name = "Samsung S5L8720", Architecture = "ARMv6" };
                case "iPod2,1":
                    return new CpuInfo { CPUClock = "532", Capacity = "32", L1Cache = "16+16", L2Cache = "", L3Cache = "", Name = "Samsung S5L8720", Architecture = "ARMv6" };
                case "iPhone2,1":
                case "iPod3,1":
                    return new CpuInfo { CPUClock = "600", Capacity = "32", L1Cache = "32+32", L2Cache = "256", L3Cache = "", Name = "Samsung S5PC100", Architecture = "ARMv7" };
                case "iPad1,1":
                    return new CpuInfo { CPUClock = "1000", Capacity = "32", L1Cache = "32+32", L2Cache = "512", L3Cache = "", Name = "Apple A4", Architecture = "ARMv7" };
                case "iPhone3,1":
                case "iPhone3,2":
                case "iPhone3,3":
                case "iPod4,1":
                case "AppleTV2,1":
                    return new CpuInfo { CPUClock = "800", Capacity = "32", L1Cache = "32+32", L2Cache = "512", L3Cache = "", Name = "Apple A4", Architecture = "ARMv7" };
                case "iPhone4,1":
				case "AppleTV3,1":
				case "iPod5,1":
                    return new CpuInfo { CPUClock = "800", Capacity = "32", L1Cache = "32+32", L2Cache = "1024", L3Cache = "", Name = "Apple A5", Architecture = "ARMv7" };
                case "iPad2,1":
                case "iPad2,2":
                case "iPad2,3":
				case "iPad2,5":
				case "iPad2,6":
				case "iPad2,7":
                    return new CpuInfo { CPUClock = "1000", Capacity = "32", L1Cache = "32+32", L2Cache = "1024", L3Cache = "", Name = "Apple A5", Architecture = "ARMv7" };
                case "iPad3,1":
                case "iPad3,2":
                case "iPad3,3":
                    return new CpuInfo { CPUClock = "1000", Capacity = "32", L1Cache = "32+32", L2Cache = "1024", L3Cache = "", Name = "Apple A5X", Architecture = "ARMv7" };
				case "iPad3,4":
				case "iPad3,5":
				case "iPad3,6":
					return new CpuInfo { CPUClock = "1400", Capacity = "32", L1Cache = "32+32", L2Cache = "1024", L3Cache = "", Name = "Apple A6X", Architecture = "ARMv7s" };
                case "iPhone5,1":
                case "iPhone5,2":
                case "iPhone5,3":
                case "iPhone5,4":
                    return new CpuInfo { CPUClock = "1300", Capacity = "32", L1Cache = "32+32", L2Cache = "1024", L3Cache = "", Name = "Apple A6", Architecture = "ARMv7s" };
                case "iPhone6,1":
                case "iPhone6,2":
                case "iPhone6,3":
				case "iPad4,4":
				case "iPad4,5":
				case "iPad4,6":
				case "iPad4,7":
				case "iPad4,8":
				case "iPad4,9":
                    return new CpuInfo { CPUClock = "1300", Capacity = "64", L1Cache = "64+64", L2Cache = "1024", L3Cache = "4096", Name = "Apple A7", Architecture = "ARMv8" };
				case "iPad4,1":
				case "iPad4,2":
				case "iPad4,3":
					return new CpuInfo { CPUClock = "1400", Capacity = "64", L1Cache = "64+64", L2Cache = "1024", L3Cache = "4096", Name = "Apple A7", Architecture = "ARMv8" };
                case "iPhone7,1":
                case "iPhone7,2":
				case "AppleTV5,3":
				case "iPad5,1":
				case "iPad5,2":
                    return new CpuInfo { CPUClock = "1400", Capacity = "64", L1Cache = "64+64", L2Cache = "1024", L3Cache = "4096", Name = "Apple A8", Architecture = "ARMv8" };
				case "iPod7,1":
					return new CpuInfo { CPUClock = "1100", Capacity = "64", L1Cache = "64+64", L2Cache = "1024", L3Cache = "4096", Name = "Apple A8", Architecture = "ARMv8" };               
				case "iPad5,3":
				case "iPad5,4":
					return new CpuInfo { CPUClock = "1500", Capacity = "64", L1Cache = "64+64", L2Cache = "2048", L3Cache = "4096", Name ="Apple A8X", Architecture = "ARMv8" };
                case "iPhone8,1":
                case "iPhone8,2":
                case "iPhone8,4":
                    return new CpuInfo { CPUClock = "1850", Capacity = "64", L1Cache = "64+64", L2Cache = "3072", L3Cache = "8192", Name = "Apple A9", Architecture = "ARMv8" };
				case "iPad6,11":
				case "iPad6,12":
					return new CpuInfo { CPUClock = "1850", Capacity = "64", L1Cache = "64+64", L2Cache = "2048", L3Cache = "4096", Name = "Apple A9", Architecture = "ARMv8" };               
				case "iPad6,3":
				case "iPad6,4":
				case "iPad6,7":
				case "iPad6,8":
					return new CpuInfo { CPUClock = "2260", Capacity = "64", L1Cache = "64+64", L2Cache = "3072", L3Cache = "", Name = "Apple A9X", Architecture = "ARMv8-A" };
                case "iPhone9,1":
                case "iPhone9,2":
                case "iPhone9,3":
                case "iPhone9,4":
                case "iPhone9,5":
                case "iPhone9,6":
                    return new CpuInfo { CPUClock = "2340", Capacity = "64", L1Cache = "64+64", L2Cache = "3072", L3Cache = "", Name = "Apple A10 Fusion", Architecture = "ARMv8" };
				case "iPad7,4":
					return new CpuInfo { CPUClock = "2360", Capacity = "64", L1Cache = "64+64", L2Cache = "8192", L3Cache = "", Name = "Apple A10X Fusion", Architecture = "ARMv8-A" };
                case "iPhone10,5":
                    return new CpuInfo { CPUClock = "2390", Capacity = "64", L1Cache = "64+64", L2Cache = "8192", L3Cache = "", Name = "Apple A11 Bionic", Architecture = "ARMv8-A" };
                default:
					return default(CpuInfo);
            }
        }
    }
}
