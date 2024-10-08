using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnInTestTool
{
        public class SysInfoData
    {
        public int Id { get; set; }
        public string? Mac { get; set; }
        public Ecusysinfo? EcuSysInfo { get; set; }
        public Ctrsysinfo? CtrSysInfo { get; set; }
        public Pn51xxsysinfo? Pn51xxSysInfo { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is SysInfoData data &&
                   Id == data.Id &&
                   Mac == data.Mac &&
                   EqualityComparer<Ecusysinfo?>.Default.Equals(EcuSysInfo, data.EcuSysInfo) &&
                   EqualityComparer<Ctrsysinfo?>.Default.Equals(CtrSysInfo, data.CtrSysInfo) &&
                   EqualityComparer<Pn51xxsysinfo?>.Default.Equals(Pn51xxSysInfo, data.Pn51xxSysInfo);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Mac, EcuSysInfo, CtrSysInfo, Pn51xxSysInfo);
        }
    }

    public class Ecusysinfo
    {
        public int Id { get; set; }
        public string? EcuUuid { get; set; }
        public string? FlasherVersion { get; set; }
        public float Voltage { get; set; }
        public int Current { get; set; }
        public float Power { get; set; }
        public float CpuUsage { get; set; }
        public float UsedMemory { get; set; }
        public float TotalMemory { get; set; }
        public int[]? CpuFrequency { get; set; }
        public float CpuTemperature { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Ecusysinfo ecusysinfo &&
                   EcuUuid == ecusysinfo.EcuUuid &&
                   FlasherVersion == ecusysinfo.FlasherVersion &&
                   Voltage == ecusysinfo.Voltage &&
                   Current == ecusysinfo.Current &&
                   Power == ecusysinfo.Power &&
                   CpuUsage == ecusysinfo.CpuUsage &&
                   UsedMemory == ecusysinfo.UsedMemory &&
                   TotalMemory == ecusysinfo.TotalMemory &&
                   EqualityComparer<int[]?>.Default.Equals(CpuFrequency, ecusysinfo.CpuFrequency) &&
                   CpuTemperature == ecusysinfo.CpuTemperature;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(EcuUuid);
            hash.Add(FlasherVersion);
            hash.Add(Voltage);
            hash.Add(Current);
            hash.Add(Power);
            hash.Add(CpuUsage);
            hash.Add(UsedMemory);
            hash.Add(TotalMemory);
            hash.Add(CpuFrequency);
            hash.Add(CpuTemperature);
            return hash.ToHashCode();
        }
    }

    public class Ctrsysinfo
    {
        public int Id { get; set; }
        public string? FlasherVersion { get; set; }
        public string? CtrUuid { get; set; }
        public float CpuUsage { get; set; }
        public float UsedMemory { get; set; }
        public float TotalMemory { get; set; }
        public int[]? CpuFrequency { get; set; }
        public float CpuTemperature { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Ctrsysinfo ctrsysinfo &&
                   FlasherVersion == ctrsysinfo.FlasherVersion &&
                   CtrUuid == ctrsysinfo.CtrUuid &&
                   CpuUsage == ctrsysinfo.CpuUsage &&
                   UsedMemory == ctrsysinfo.UsedMemory &&
                   TotalMemory == ctrsysinfo.TotalMemory &&
                   EqualityComparer<int[]?>.Default.Equals(CpuFrequency, ctrsysinfo.CpuFrequency) &&
                   CpuTemperature == ctrsysinfo.CpuTemperature;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FlasherVersion, CtrUuid, CpuUsage, UsedMemory, TotalMemory, CpuFrequency, CpuTemperature);
        }
    }

    public class Pn51xxsysinfo
    {
        public int Id { get; set; }
        public string? Version { get; set; }
        public float Vddpa { get; set; }
        public float Iddpa { get; set; }
        public int Temperature { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Pn51xxsysinfo xxsysinfo &&
                   Version == xxsysinfo.Version &&
                   Vddpa == xxsysinfo.Vddpa &&
                   Iddpa == xxsysinfo.Iddpa &&
                   Temperature == xxsysinfo.Temperature;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Version, Vddpa, Iddpa, Temperature);
        }
    }

}

