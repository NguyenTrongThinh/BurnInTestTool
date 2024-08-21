using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BurnInTestTool
{

    public class BurnSettingUtils
    {
        static public string GetBurnSettingLocation()
        {
            string burnSettingFile = "burnSetting.cfg";
            string burnSettingFolder = "BurnInTestTool";
            string localAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string burnSettingLocation = Path.Combine(localAppDataFolder, burnSettingFolder);
            Directory.CreateDirectory(burnSettingLocation);
            string burnSettingFileLocation = Path.Combine(burnSettingLocation, burnSettingFile);
            return burnSettingFileLocation;
        }

        static public bool IsBurnSettingExist()
        {
            try
            {
                FileInfo fileInfo = new FileInfo(GetBurnSettingLocation());
                if (fileInfo.Exists)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }
        static public bool CreateDefaultSetting()
        {
            BurnSetting burnSetting = new BurnSetting();
            burnSetting.StressCpuEcu = new Stresscpuecu();
            burnSetting.StressCpuEcu.Duration = 3600;
            burnSetting.StressCpuEcu.Execute = true;
            burnSetting.StressCpuEcu.Threads = 4;
            burnSetting.StressCpuEcu.Percent = 80;

            burnSetting.StressCpuCtr = new Stresscpuctr();
            burnSetting.StressCpuCtr.Duration = 3600;
            burnSetting.StressCpuCtr.Execute = true;
            burnSetting.StressCpuCtr.Threads = 2;
            burnSetting.StressCpuCtr.Percent = 80;

            burnSetting.LedOn = new Ledon();
            burnSetting.LedOn.Duration = 3600;
            burnSetting.LedOn.Execute = true;

            burnSetting.NfcPolling = new Nfcpolling();
            burnSetting.NfcPolling.Duration = 3600;
            burnSetting.NfcPolling.Execute = true;

            burnSetting.LcdImage = new Lcdimage();
            burnSetting.LcdImage.Duration = 3600;
            burnSetting.LcdImage.Execute = true;
            burnSetting.LcdImage.File = "images_rf.png";

            burnSetting.AudioPlay = new Audioplay();
            burnSetting.AudioPlay.Duration = 3600;
            burnSetting.AudioPlay.Execute = true;
            burnSetting.AudioPlay.File = "sine.wav";

            string json = JsonConvert.SerializeObject(burnSetting);

            string burnSettingFilePath = GetBurnSettingLocation();
            try
            {
                File.WriteAllText(burnSettingFilePath, json);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }
        static public BurnSetting? GetBurnSetting()
        {
            try
            {
                string burnSettingFilePath = GetBurnSettingLocation();
                string filecontent = File.ReadAllText(burnSettingFilePath);
                BurnSetting? burnSetting = JsonConvert.DeserializeObject< BurnSetting>(filecontent);
                return burnSetting;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return null;
            }                       

        }
        static public void SaveBurnSetting(BurnSetting? burnSetting)
        {
            try
            {
                if (burnSetting != null)
                {
                    string burnSettingFilePath = GetBurnSettingLocation();
                    string json = JsonConvert.SerializeObject(burnSetting);
                    File.WriteAllText(burnSettingFilePath, json);
                }
                else
                    CreateDefaultSetting();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
    }

    public class BurnSetting
    {
        public Stresscpuecu? StressCpuEcu { get; set; }
        public Stresscpuctr? StressCpuCtr { get; set; }
        public Ledon? LedOn { get; set; }
        public Nfcpolling? NfcPolling { get; set; }
        public Lcdimage? LcdImage { get; set; }
        public Audioplay? AudioPlay { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is BurnSetting setting &&
                   EqualityComparer<Stresscpuecu?>.Default.Equals(StressCpuEcu, setting.StressCpuEcu) &&
                   EqualityComparer<Stresscpuctr?>.Default.Equals(StressCpuCtr, setting.StressCpuCtr) &&
                   EqualityComparer<Ledon?>.Default.Equals(LedOn, setting.LedOn) &&
                   EqualityComparer<Nfcpolling?>.Default.Equals(NfcPolling, setting.NfcPolling) &&
                   EqualityComparer<Lcdimage?>.Default.Equals(LcdImage, setting.LcdImage) &&
                   EqualityComparer<Audioplay?>.Default.Equals(AudioPlay, setting.AudioPlay);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StressCpuEcu, StressCpuCtr, LedOn, NfcPolling, LcdImage, AudioPlay);
        }
    }

    public class Stresscpuecu
    {
        public bool? Execute { get; set; }
        public int? Duration { get; set; }
        public int? Threads { get; set; }
        public int? Percent { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Stresscpuecu stresscpuecu &&
                   Execute == stresscpuecu.Execute &&
                   Duration == stresscpuecu.Duration &&
                   Threads == stresscpuecu.Threads &&
                   Percent == stresscpuecu.Percent;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Execute, Duration, Threads, Percent);
        }
    }

    public class Stresscpuctr
    {
        public bool? Execute { get; set; }
        public int? Duration { get; set; }
        public int? Threads { get; set; }
        public int? Percent { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Stresscpuctr stresscpuctr &&
                   Execute == stresscpuctr.Execute &&
                   Duration == stresscpuctr.Duration &&
                   Threads == stresscpuctr.Threads &&
                   Percent == stresscpuctr.Percent;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Execute, Duration, Threads, Percent);
        }
    }

    public class Ledon
    {
        public bool? Execute { get; set; }
        public int? Duration { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Ledon ledon &&
                   Execute == ledon.Execute &&
                   Duration == ledon.Duration;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Execute, Duration);
        }
    }

    public class Nfcpolling
    {
        public bool? Execute { get; set; }
        public int? Duration { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Nfcpolling nfcpolling &&
                   Execute == nfcpolling.Execute &&
                   Duration == nfcpolling.Duration;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Execute, Duration);
        }
    }

    public class Lcdimage
    {
        public bool? Execute { get; set; }
        public int? Duration { get; set; }
        public string? File { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Lcdimage lcdimage &&
                   Execute == lcdimage.Execute &&
                   Duration == lcdimage.Duration &&
                   File == lcdimage.File;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Execute, Duration, File);
        }
    }

    public class Audioplay
    {
        public bool? Execute { get; set; }
        public int? Duration { get; set; }
        public string? File { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Audioplay audioplay &&
                   Execute == audioplay.Execute &&
                   Duration == audioplay.Duration &&
                   File == audioplay.File;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Execute, Duration, File);
        }
    }

}