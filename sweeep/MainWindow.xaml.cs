using System.Collections.ObjectModel;
using System.Windows;

namespace sweeep
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Record> Records { get; } = new();
        private Record? _selectedRecord; // 현재 수정 중인 레코드

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}