using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using MtmEquipmentApp.Context;
using MtmEquipmentApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace MtmEquipmentApp.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReportsUserControl.xaml
    /// </summary>
    public partial class ReportsUserControl : UserControl
    {
        public ReportsUserControl()
        {
            InitializeComponent();
            DataContext = new ReportsViewModel();
        }
        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            var data = GetReportData();

            if (data == null || data.Rows.Count == 0)
            {
                MessageBox.Show("Нет данных для экспорта.");
                return;
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Отчет");

                for (int i = 0; i < data.Columns.Count; i++)
                {
                    worksheet.Cell(1, i + 1).Value = data.Columns[i].ColumnName;
                }

                for (int i = 0; i < data.Rows.Count; i++)
                {
                    for (int j = 0; j < data.Columns.Count; j++)
                    {
                        worksheet.Cell(i + 2, j + 1).Value = data.Rows[i][j]?.ToString() ?? "";
                    }
                }

                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    FileName = "Report.xlsx"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    workbook.SaveAs(saveFileDialog.FileName);
                    MessageBox.Show("Экспорт завершен!");
                }
            }
        }

        // Метод для получения данных для экспорта
        private DataTable GetReportData()
        {
            var dt = new DataTable();
            dt.Columns.Add("Оборудование");
            dt.Columns.Add("Инв. номер");
            dt.Columns.Add("Подразделение");
            dt.Columns.Add("Статус");
            dt.Columns.Add("Инспекций", typeof(int));
            dt.Columns.Add("Есть дефекты", typeof(bool));

            using (var db = new AppDbContext())
            {
                var equipments = db.Equipment
                    .Include(e => e.Department)
                    .Include(e => e.Inspections)
                    .ToList();

                foreach (var eq in equipments)
                {
                    int inspectionsCount = eq.Inspections.Count;
                    bool hasDefects = eq.Inspections.Any(i => i.IsDefective);

                    dt.Rows.Add(
                        eq.Name,
                        eq.InventoryNumber,
                        eq.Department.Name,
                        eq.Status.ToString(),
                        inspectionsCount,
                        hasDefects
                    );
                }
            }

            return dt;
        }

    }
}

