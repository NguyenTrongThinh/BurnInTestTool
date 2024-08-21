using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for MessageShow.xaml
    /// </summary>
    public partial class MessageShow : Window
    {
        public bool? MessageResult { get; private set; }

        public enum MessageIcon
        {
            Info,
            Warning,
            Error
        }

        public MessageShow()
        {
            InitializeComponent();
        }

        private void setMessage(string message)
        {
            MessageTextBlock.Text = message;
        }

        private void setImage(MessageIcon icon)
        {
            switch(icon)
            {
                case MessageIcon.Info:
                    imgInfo.Source = new BitmapImage(
               new Uri("pack://application:,,,/images/icons8-info-48.png"));
                    break;
                case MessageIcon.Warning:
                    imgInfo.Source = new BitmapImage(
               new Uri("pack://application:,,,/images/icons8-warning-48.png"));
                    break;
                case MessageIcon.Error:
                    imgInfo.Source = new BitmapImage(
               new Uri("pack://application:,,,/images/icons8-error-48.png"));
                    break;
                default:
                    imgInfo.Source = new BitmapImage(
               new Uri("pack://application:,,,/images/icons8-info-48.png"));
                    break;
            }
           
        }

        public static bool? Show(string message)
        {
            MessageShow messageBox = new MessageShow();
            messageBox.setMessage(message);
            return messageBox.ShowDialog();
        }

        public static bool? Show(string message, MessageIcon icon)
        {
            MessageShow messageBox = new MessageShow();
            messageBox.setMessage(message);
            messageBox.setImage(icon);
            return messageBox.ShowDialog();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            MessageResult = true;
            this.Close();
        }
    }
}
