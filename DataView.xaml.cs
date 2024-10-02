using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for DataView.xaml
    /// </summary>
    /// 
    public class DataViewViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private DataViewDataClass? _boardData;

        public DataViewDataClass? BoardData
        {
            get
            {
                return _boardData;
            }
            set
            {
                if (_boardData != value)
                {
                    _boardData = value;
                    OnPropertyChanged(nameof(BoardData));  // Notify when the property changes
                }
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DataViewDataClass
    {
        private BoardID? id;

        public BoardID? ID { get => id; set => id = value; }
    }
    public partial class DataView : UserControl
    {
        private DataViewViewModel viewModel { get; set; }
        private DataViewDataClass dataViewDataClass { get; set; }
        public EventHandler? RemoveRequested;
        public DataView()
        {
            InitializeComponent();
            dataViewDataClass = new DataViewDataClass();
            viewModel = new DataViewViewModel();
            viewModel.BoardData = dataViewDataClass;
            DataContext = viewModel;
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
           
        }

        public void SetBoardID(BoardID boardID)
        {
            if (viewModel.BoardData is not null)
            {
                viewModel.BoardData.ID = boardID;
            }
        }
        public BoardID? GetBoardID()
        {
            return viewModel.BoardData?.ID??null;
        }

        private void RemoveRequest()
        {
            RemoveRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Log.Debug("User Control Loaded");
        }
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Log.Debug("User Control Unloaded");
            this.Loaded -= OnLoaded;
            this.Unloaded -= OnUnloaded;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveRequest();
        }

    }


}
