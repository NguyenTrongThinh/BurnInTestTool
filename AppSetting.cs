using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BurnInTestTool
{
    internal class AppSetting
    {
        static public void setLanguage(string? language)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(language ?? "en-US");
            ResourceDictionary resDict = new ResourceDictionary()
            {
                Source = new Uri($"/Dictionary-{language}.xaml", UriKind.Relative)
            };
            Application.Current.Resources.MergedDictionaries.Add(resDict);
            Properties.Settings.Default.lang = language ?? "en-US";
            Properties.Settings.Default.Save();
        }
        static public string GetApplicationLogLocation()
        {
            string logFile = "applicationlog.txt";
            string logFolder = "BurnInTestTool";
            string localAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string logLocation = Path.Combine(localAppDataFolder, logFolder);
            Directory.CreateDirectory(logLocation);
            string logFileLocation = Path.Combine(logLocation, logFile);
            return logFileLocation;
        }
    }
}
