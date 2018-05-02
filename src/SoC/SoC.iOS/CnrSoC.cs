﻿using System;
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
        [DllImport(Constants.SystemLibrary)]
        static internal extern int sysctlbyname([MarshalAs(UnmanagedType.LPStr)] string property, IntPtr output, IntPtr oldLen, IntPtr newp, uint newlen);

        public CnrSoC()
        {
        }

        public string Model => GetCpuInfo(GetSystemProperty("hw.model")).Name;

        public float CurrentFrequency => throw new NotImplementedException();

        public float MinFrequency => throw new NotImplementedException();

        public float MaxFrequency
        {
            get 
            {
                float.TryParse(GetCpuInfo(GetSystemProperty("hw.model")).CPUClock, out float result);
                return result;
            }
        }

        public int Cores => (int)NSProcessInfo.ProcessInfo.ActiveProcessorCount;

        public Task<List<AdditionalInformation>> GetAdditionalInformationAsync(CancellationTokenSource cts = null)
        {
            return Task.Factory.StartNew<List<AdditionalInformation>>(() =>
            {
                var data = GetCpuInfo(GetSystemProperty("hw.model"));
                var list = new List<AdditionalInformation>();
                list.Add(new AdditionalInformation { Title = nameof(data.Architecture), Value = data.Architecture });
                list.Add(new AdditionalInformation { Title = nameof(data.Capacity), Value = data.Capacity });
                list.Add(new AdditionalInformation { Title = nameof(data.L1Cache), Value = data.L1Cache });
                list.Add(new AdditionalInformation { Title = nameof(data.L2Cache), Value = data.L2Cache });
                list.Add(new AdditionalInformation { Title = nameof(data.L3Cache), Value = data.L3Cache });
                return list;
            });
        }

        public Task<UsageInformation> GetUsageAsync(CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
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
                    return new CpuInfo { CPUClock = "412", Capacity = "32", L1Cache = "16+16", L2Cache = "", L3Cache = "", Name = "Samsung S5L8900", Architecture = "ARMv6" };
                case "iPhone1,2":
                    return new CpuInfo { CPUClock = "412", Capacity = "32", L1Cache = "16+16", L2Cache = "", L3Cache = "", Name = "Samsung S5L8720", Architecture = "ARMv6" };
                case "iPhone2,1":
                    return new CpuInfo { CPUClock = "600", Capacity = "32", L1Cache = "32+32", L2Cache = "256", L3Cache = "", Name = "Samsung S5PC100", Architecture = "ARMv7" };
                case "iPhone3,1":
                case "iPhone3,2":
                case "iPhone3,3":
                    return new CpuInfo { CPUClock = "800", Capacity = "32", L1Cache = "32+32", L2Cache = "512", L3Cache = "", Name = "Apple A4", Architecture = "ARMv7" };
                case "iPhone4,1":
                    return new CpuInfo { CPUClock = "800", Capacity = "32", L1Cache = "32+32", L2Cache = "1024", L3Cache = "", Name = "Apple A5", Architecture = "ARMv7" };
                case "iPhone5,1":
                case "iPhone5,2":
                case "iPhone5,3":
                case "iPhone5,4":
                    return new CpuInfo { CPUClock = "1300", Capacity = "32", L1Cache = "32+32", L2Cache = "1024", L3Cache = "", Name = "Apple A6", Architecture = "ARMv7s" };
                case "iPhone6,1":
                case "iPhone6,2":
                case "iPhone6,3":
                    return new CpuInfo { CPUClock = "1300", Capacity = "64", L1Cache = "64+64", L2Cache = "1024", L3Cache = "4096", Name = "Apple A7", Architecture = "ARMv8" };
                case "iPhone7,1":
                case "iPhone7,2":
                    return new CpuInfo { CPUClock = "1400", Capacity = "64", L1Cache = "64+64", L2Cache = "1024", L3Cache = "4096", Name = "Apple A8", Architecture = "ARMv8" };
                case "iPhone8,1":
                case "iPhone8,2":
                case "iPhone8,4":
                    return new CpuInfo { CPUClock = "1850", Capacity = "64", L1Cache = "64+64", L2Cache = "3072", L3Cache = "8192", Name = "Apple A9", Architecture = "ARMv8" };
                case "iPhone9,1":
                case "iPhone9,2":
                case "iPhone9,3":
                case "iPhone9,4":
                case "iPhone9,5":
                case "iPhone9,6":
                    return new CpuInfo { CPUClock = "2340", Capacity = "64", L1Cache = "64+64", L2Cache = "3072", L3Cache = "", Name = "Apple A10 Fusion", Architecture = "ARMv8" };
                case "iPhone10,5":
                    return new CpuInfo { CPUClock = "2390", Capacity = "64", L1Cache = "64+64", L2Cache = "8192", L3Cache = "", Name = "Apple A11 Bionic", Architecture = "ARMv8-A" };
                
                default:
                    return null;
            }
        }
    }
}
