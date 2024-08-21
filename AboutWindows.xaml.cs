using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BurnInTestTool
{
    /// <summary>
    /// Interaction logic for AboutWindows.xaml
    /// </summary>
    public partial class AboutWindows : Window
    {
        public static readonly DependencyProperty ApplicationVersionProperty =
          DependencyProperty.Register("ApplicationVersion", typeof(string),typeof(AboutWindows));

        public string ApplicationVersion
        {
            get { return (string)GetValue(ApplicationVersionProperty); }
            set
            {
                SetValue(ApplicationVersionProperty, value);
            }
        }
        public AboutWindows()
        {
            InitializeComponent();
            MessageAbout.Text = GetAppVersion()??"null";
            Log.Debug("Application Version " + ApplicationVersion);
        }
        private string? GetAppVersion()
        {
            return System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version?.ToString();
        }
    }
}
