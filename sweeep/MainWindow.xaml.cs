using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace sweeep
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Record> Records { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();
            RecordListView.ItemsSource = Records;
        }

        private void AddRecord_Click(object sender, RoutedEventArgs e)
        {
            var dateText = DateTextBox.Text;
            var categoryText = CategoryTextBox.Text;
            var amountText = AmountTextBox.Text;
            var memoText = MemoTextBox.Text;

            // Create record
            var record = new Record
            {
                Date = dateText,
                Amount = amountText,
                Category = categoryText,
                Memo = memoText
            };
            Records.Add(record);

            DateTextBox.Clear();
            CategoryTextBox.Clear();
            AmountTextBox.Clear();
            MemoTextBox.Clear();
        }

        // 숫자만 입력되도록
        private void AmountTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]*$");
        }
        
        // 모든 필드에 값이 입력되어 있는지 확인
        private void InputFields_TextChanged(object sender, TextChangedEventArgs e)
        {
            AddRecordButton.IsEnabled = 
                !string.IsNullOrWhiteSpace(DateTextBox.Text) &&
                !string.IsNullOrWhiteSpace(CategoryTextBox.Text) &&
                !string.IsNullOrWhiteSpace(AmountTextBox.Text) &&
                !string.IsNullOrWhiteSpace(MemoTextBox.Text);
        }
    }
}