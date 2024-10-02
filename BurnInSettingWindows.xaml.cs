using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public class SettingViewModel : INotifyPropertyChanged
    {
        BurnSetting? _setting;

        public BurnSetting? Setting
        {
            get => _setting;
            set
            {
                if (_setting != value)
                {
                    _setting = value;
                    OnPropertyChanged(nameof(Setting));
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
    /// <summary>
    /// Interaction logic for BurnInSettingWindows.xaml
    /// </summary>
    public partial class BurnInSettingWindows : Window
    {
        public BurnInSettingWindows()
        {
            InitializeComponent();
            DataContext = new SettingViewModel();
            UpdateDefaultSetting();
        }

        private void UpdateDefaultSetting()
        {
            if (DataContext is SettingViewModel viewModel) { 
                if (BurnSettingUtils.IsBurnSettingExist() == false)
                {
                    bool retVal = BurnSettingUtils.CreateDefaultSetting();
                    Log.Debug("Create Default Setting Return: " + retVal);
                    if (retVal == false)
                    {
                        MessageShow.Show((string)Application.Current.Resources["msg_create_defaul_burn_setting_failed"], MessageShow.MessageIcon.Error);
                    }
                    else
                    {
                        viewModel.Setting = BurnSettingUtils.GetBurnSetting();
                    }
                }
                else
                {
                    viewModel.Setting = BurnSettingUtils.GetBurnSetting();
                }
            }
        }

        private void ButtonOK_Clicked(object sender, RoutedEventArgs e)
        {
            if (DataContext is SettingViewModel viewModel)
            {
                BurnSettingUtils.SaveBurnSetting(viewModel.Setting??null);
            }
            this.Close();
        }

        private void ButtonCancel_Clicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtNumberBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtNumberBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                if (!IsTextValid(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool IsTextValid(string text)
        {
            Regex regex = new Regex("[^0-9.-]+");
            return !regex.IsMatch(text);
        }

        private void txtNumberBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox is not null)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = "0"; // Set default value to 3600 if the TextBox is empty
                }
            }
        }
    }
}
