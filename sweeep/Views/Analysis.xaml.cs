using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using sweeep.Local;

namespace sweeep
{
    public partial class Analysis : UserControl
    {
        private List<Record> _records;

        public Analysis()
        {
            InitializeComponent();

            // 데이터 불러오기
            _records = RecordCache.Instance.GetAll();

            // 날짜별 합계 계산 (LineChart 대신)
            var dateData = _records
                .GroupBy(r => r.Date.Date)
                .Select(r => new
                {
                    Date = r.Key,
                    TotalAmount = r.Sum(record => record.Amount)
                })
                .OrderBy(g => g.Date)
                .ToList();

            // ListView에 데이터를 바인딩
            LineDataList.ItemsSource = dateData.Select(data => new
            {
                Date = data.Date.ToString("yyyy-MM-dd"),
                Amount = data.TotalAmount
            }).ToList();

            // 카테고리별 합계 계산 (PieChart 대신)
            var categoryData = _records
                .GroupBy(r => r.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalAmount = g.Sum(r => r.Amount)
                })
                .ToList();

            // ListView에 데이터를 바인딩
            PieDataList.ItemsSource = categoryData.Select(data => new
            {
                Category = data.Category,
                Amount = data.TotalAmount
            }).ToList();
        }
    }
}