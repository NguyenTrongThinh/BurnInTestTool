using Serilog;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BurnInTestTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppSetting.setLanguage(Properties.Settings.Default.lang);
            Closing += new CancelEventHandler(this.cancelEventHandler);
            Log.Logger = new LoggerConfiguration()
           .WriteTo.File(AppSetting.GetApplicationLogLocation(), fileSizeLimitBytes: 1048576, rollOnFileSizeLimit: true, retainedFileCountLimit: 5)
           .WriteTo.Console()
           .MinimumLevel.Debug()
           .CreateLogger();
            Log.Information("Application started.");
           
        }

        private void cancelEventHandler(object? sender, CancelEventArgs e)
        {
            Log.Information("Application Exited");
            Log.CloseAndFlush();
        }

        private void MnuExportBb_Click(object sender, RoutedEventArgs e)
        {
            Log.Debug("User Select Export Database Functions");
        }
      
        private void mnuLang_Click(object sender, RoutedEventArgs e)
        {
            AppSetting.setLanguage(((MenuItem)sender).Tag.ToString());
        }

        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void mnuBurnSetting_Click(object sender, RoutedEventArgs e)
        {
            BurnInSettingWindows burnInSettingWindows = new BurnInSettingWindows();
            burnInSettingWindows.Owner = this;
            burnInSettingWindows.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            burnInSettingWindows.ShowDialog();
        }

        private void mnuAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindows aboutWindows = new AboutWindows();
            aboutWindows.Owner = this;
            aboutWindows.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            aboutWindows.ShowDialog();  
        }
    }
}