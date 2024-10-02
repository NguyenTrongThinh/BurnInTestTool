using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnInTestTool
{
 

    public class SystemInformationDataClass
    {
        private string? id;
        private Ecusysinfo? ecuSysInfo;
        private Ctrsysinfo? ctrSysInfo;
        private Pn5190sysinfo? pn5190SysInfo;

        public string? Id { get => id; set => id = value; }
        public Ecusysinfo? EcuSysInfo { get => ecuSysInfo; set => ecuSysInfo = value; }
        public Ctrsysinfo? CtrSysInfo { get => ctrSysInfo; set => ctrSysInfo = value; }
        public Pn5190sysinfo? Pn5190SysInfo { get => pn5190SysInfo; set => pn5190SysInfo = value; }

        public override bool Equals(object? obj)
        {
            return obj is SystemInformationDataClass @class &&
                   id == @class.id &&
                   EqualityComparer<Ecusysinfo?>.Default.Equals(ecuSysInfo, @class.ecuSysInfo) &&
                   EqualityComparer<Ctrsysinfo?>.Default.Equals(ctrSysInfo, @class.ctrSysInfo) &&
                   EqualityComparer<Pn5190sysinfo?>.Default.Equals(pn5190SysInfo, @class.pn5190SysInfo) &&
                   Id == @class.Id &&
                   EqualityComparer<Ecusysinfo?>.Default.Equals(EcuSysInfo, @class.EcuSysInfo) &&
                   EqualityComparer<Ctrsysinfo?>.Default.Equals(CtrSysInfo, @class.CtrSysInfo) &&
                   EqualityComparer<Pn5190sysinfo?>.Default.Equals(Pn5190SysInfo, @class.Pn5190SysInfo);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id, ecuSysInfo, ctrSysInfo, pn5190SysInfo, Id, EcuSysInfo, CtrSysInfo, Pn5190SysInfo);
        }
    }

        public class Ecusysinfo
        {
        private string? ecuUuid;
        private string? flasherVersion;
        private float voltage;
        private float current;
        private float power;
        private float cpuUsage;
        private float usedMemory;
        private float totalMemory;
        private int[]? cpuFrequency;
        private float cpuTemperature;

        public string? EcuUuid { get => ecuUuid; set => ecuUuid = value; }
        public string? FlasherVersion { get => flasherVersion; set => flasherVersion = value; }
        public float Voltage { get => voltage; set => voltage = value; }
        public float Current { get => current; set => current = value; }
        public float Power { get => power; set => power = value; }
        public float CpuUsage { get => cpuUsage; set => cpuUsage = value; }
        public float UsedMemory { get => usedMemory; set => usedMemory = value; }
        public float TotalMemory { get => totalMemory; set => totalMemory = value; }
        public int[]? CpuFrequency { get => cpuFrequency; set => cpuFrequency = value; }
        public float CpuTemperature { get => cpuTemperature; set => cpuTemperature = value; }

        public override bool Equals(object? obj)
        {
            return obj is Ecusysinfo ecusysinfo &&
                   ecuUuid == ecusysinfo.ecuUuid &&
                   flasherVersion == ecusysinfo.flasherVersion &&
                   voltage == ecusysinfo.voltage &&
                   current == ecusysinfo.current &&
                   power == ecusysinfo.power &&
                   cpuUsage == ecusysinfo.cpuUsage &&
                   usedMemory == ecusysinfo.usedMemory &&
                   totalMemory == ecusysinfo.totalMemory &&
                   EqualityComparer<int[]?>.Default.Equals(cpuFrequency, ecusysinfo.cpuFrequency) &&
                   cpuTemperature == ecusysinfo.cpuTemperature &&
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
            hash.Add(ecuUuid);
            hash.Add(flasherVersion);
            hash.Add(voltage);
            hash.Add(current);
            hash.Add(power);
            hash.Add(cpuUsage);
            hash.Add(usedMemory);
            hash.Add(totalMemory);
            hash.Add(cpuFrequency);
            hash.Add(cpuTemperature);
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
        private string? flasherVersion;
        private string? ctrUuid;
        private float cpuUsage;
        private float usedMemory;
        private float totalMemory;
        private int[]? cpuFrequency;
        private float cpuTemperature;

        public string? FlasherVersion { get => flasherVersion; set => flasherVersion = value; }
        public string? CtrUuid { get => ctrUuid; set => ctrUuid = value; }
        public float CpuUsage { get => cpuUsage; set => cpuUsage = value; }
        public float UsedMemory { get => usedMemory; set => usedMemory = value; }
        public float TotalMemory { get => totalMemory; set => totalMemory = value; }
        public int[]? CpuFrequency { get => cpuFrequency; set => cpuFrequency = value; }
        public float CpuTemperature { get => cpuTemperature; set => cpuTemperature = value; }

        public override bool Equals(object? obj)
        {
            return obj is Ctrsysinfo ctrsysinfo &&
                   flasherVersion == ctrsysinfo.flasherVersion &&
                   ctrUuid == ctrsysinfo.ctrUuid &&
                   cpuUsage == ctrsysinfo.cpuUsage &&
                   usedMemory == ctrsysinfo.usedMemory &&
                   totalMemory == ctrsysinfo.totalMemory &&
                   EqualityComparer<int[]?>.Default.Equals(cpuFrequency, ctrsysinfo.cpuFrequency) &&
                   cpuTemperature == ctrsysinfo.cpuTemperature &&
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
            HashCode hash = new HashCode();
            hash.Add(flasherVersion);
            hash.Add(ctrUuid);
            hash.Add(cpuUsage);
            hash.Add(usedMemory);
            hash.Add(totalMemory);
            hash.Add(cpuFrequency);
            hash.Add(cpuTemperature);
            hash.Add(FlasherVersion);
            hash.Add(CtrUuid);
            hash.Add(CpuUsage);
            hash.Add(UsedMemory);
            hash.Add(TotalMemory);
            hash.Add(CpuFrequency);
            hash.Add(CpuTemperature);
            return hash.ToHashCode();
        }
    }

        public class Pn5190sysinfo
        {
        private string? version;
        private float vddpa;
        private float iddpa;
        private float pn5190Temperature;

        public string? Version { get => version; set => version = value; }
        public float Vddpa { get => vddpa; set => vddpa = value; }
        public float Iddpa { get => iddpa; set => iddpa = value; }
        public float Pn5190Temperature { get => pn5190Temperature; set => pn5190Temperature = value; }

        public override bool Equals(object? obj)
        {
            return obj is Pn5190sysinfo sysinfo &&
                   version == sysinfo.version &&
                   vddpa == sysinfo.vddpa &&
                   iddpa == sysinfo.iddpa &&
                   pn5190Temperature == sysinfo.pn5190Temperature &&
                   Version == sysinfo.Version &&
                   Vddpa == sysinfo.Vddpa &&
                   Iddpa == sysinfo.Iddpa &&
                   Pn5190Temperature == sysinfo.Pn5190Temperature;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(version, vddpa, iddpa, pn5190Temperature, Version, Vddpa, Iddpa, Pn5190Temperature);
        }
    }

    }

