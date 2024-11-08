using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using sweeep.Local;

namespace sweeep;

public partial class SettingTab : UserControl
{
    private ObservableCollection<Category> Categories { get; } = new();
    private Category? _selectedCategory;

    public SettingTab()
    {
        InitializeComponent();

        CategoryListView.ItemsSource = Categories;
        CategoryCache.Instance.GetAll().ForEach(category => Categories.Add(category));
    }

    // 모든 필드에 값이 입력되어 있는지 확인
    private void InputFields_TextChanged(object sender, TextChangedEventArgs e)
    {
        UpdateAddRecordButtonState();
    }

    private void UpdateAddRecordButtonState()
    {
        // DatePicker의 날짜와 모든 필드에 값이 입력되어 있는지 확인
        AddCategoryButton.IsEnabled =
            !string.IsNullOrWhiteSpace(CategoryTextBox.Text);
    }

    private void AddRecord_Click(object sender, RoutedEventArgs e)
    {
        var categoryText = CategoryTextBox.Text;

        if (_selectedCategory == null)
        {
            if (Categories.ToList().Select(w => w.Name).Contains(categoryText))
            {
                MessageBox.Show("You must enter different name of category");
                return;
            }

            Categories.Add(new Category(name: categoryText));
            CategoryCache.Instance.Insert(categoryText);
        }
        else
        {
            CategoryCache.Instance.Delete(_selectedCategory.Name);
            CategoryCache.Instance.Insert(CategoryTextBox.Text);
            _selectedCategory.Name = CategoryTextBox.Text;
        }

        CategoryTextBox.Clear();
        _selectedCategory = null;
        AddCategoryButton.Content = "Add Category";
    }

    private void DeleteCategory_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button { CommandParameter: Category category })
        {
            Categories.Remove(category); // 선택된 레코드를 삭제
            CategoryCache.Instance.Delete(category.Name);
        }
    }

    private void EditCategory_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button { CommandParameter: Category category }) return;
        _selectedCategory = category;
        CategoryTextBox.Text = category.Name;
        AddCategoryButton.Content = "Edit Category";
    }
}