using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using sweeep.Local;

namespace sweeep;

public partial class RecordTab : UserControl
{
    private ObservableCollection<Record> Records { get; } = new();
    private Record? _selectedRecord;

    public RecordTab()
    {
        InitializeComponent();
        RecordListView.ItemsSource = Records;
        RecordCache.Instance.GetAll().ForEach(record => Records.Add(record));
    }

    private void AddRecord_Click(object sender, RoutedEventArgs e)
    {
        var dateText = DatePicker.SelectedDate;
        var categoryText = CategoryTextBox.Text;
        var amountText = AmountTextBox.Text;
        var memoText = MemoTextBox.Text;

        if (!CategoryCache.Instance.GetAll().Select(w => w.Name).Contains(categoryText))
        {
            MessageBox.Show("You must enter exist category name");
            return;
        }

        // Create
        if (_selectedRecord == null)
        {
            // Create record
            var record = new Record
            {
                Date = dateText.Value,
                Amount = int.Parse(amountText),
                Category = categoryText,
                Memo = memoText
            };
            var savedRecord = RecordCache.Instance.Insert(record);
            Records.Add(savedRecord);
        }
        else
        {
            _selectedRecord.Date = DatePicker.SelectedDate.Value;
            _selectedRecord.Category = CategoryTextBox.Text;
            _selectedRecord.Amount = int.Parse(AmountTextBox.Text);
            _selectedRecord.Memo = MemoTextBox.Text;
            RecordCache.Instance.Edit(_selectedRecord);
        }

        DatePicker.SelectedDate = null;
        CategoryTextBox.Clear();
        AmountTextBox.Clear();
        MemoTextBox.Clear();

        _selectedRecord = null;
        AddRecordButton.Content = "Add Record";
    }

    // 숫자만 입력되도록
    private void AmountTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Regex.IsMatch(e.Text, "^[0-9]*$");
    }

    // 모든 필드에 값이 입력되어 있는지 확인
    private void InputFields_TextChanged(object sender, TextChangedEventArgs e)
    {
        UpdateAddRecordButtonState();
    }

    private void InputFields_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
        UpdateAddRecordButtonState();
    }

    private void UpdateAddRecordButtonState()
    {
        // DatePicker의 날짜와 모든 필드에 값이 입력되어 있는지 확인
        AddRecordButton.IsEnabled =
            DatePicker.SelectedDate.HasValue &&
            !string.IsNullOrWhiteSpace(CategoryTextBox.Text) &&
            !string.IsNullOrWhiteSpace(AmountTextBox.Text) &&
            !string.IsNullOrWhiteSpace(MemoTextBox.Text);
    }

    private void DeleteRecord_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button { CommandParameter: Record record } button)
        {
            Records.Remove(record); // 선택된 레코드를 삭제
            RecordCache.Instance.Delete(record.Id);
        }
    }

    private void EditRecord_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button { CommandParameter: Record record }) return;
        _selectedRecord = record; // 현재 수정 중인 레코드 저장
        DatePicker.SelectedDate = record.Date; // 수정할 레코드의 값을 입력 필드에 채우기
        CategoryTextBox.Text = record.Category;
        AmountTextBox.Text = record.Amount.ToString();
        MemoTextBox.Text = record.Memo;
        AddRecordButton.Content = "Edit Record";
    }
}