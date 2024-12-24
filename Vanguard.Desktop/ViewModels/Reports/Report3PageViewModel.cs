using System.Collections.ObjectModel;
using System.Net.Http.Json;
using Spire.Doc.Documents;
using Spire.Doc;
using Vanguard.App.Helpers;
using Vanguard.DataAccess.Models;
using System.Drawing;

namespace Vanguard.Desktop.ViewModels.Reports;
public partial class Report3PageViewModel : ObservableObject
{
    public ObservableCollection<Report3> Reports { get; set; } = new();

    public async Task LoadAsync()
    {
        Reports.Clear();

        var response = await ApiHelper.GetReport(3);

        if (response != null && response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<List<Report3>>();

            if (content != null)
            {
                foreach (var r in content)
                {
                    Reports.Add(r);
                }
            }
        }
    }

    [RelayCommand]
    public async Task ExportReport()
    {
        var doc = new Document();

        var section = doc.AddSection();

        var titleParagraph = section.AddParagraph();
        var dateParagraph = section.AddParagraph();
        var table = section.AddTable();

        titleParagraph.AppendText("Отчет о вызовах бригад и их бригадирах");
        titleParagraph.ApplyStyle(BuiltinStyle.Header);
        titleParagraph.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;

        dateParagraph.AppendText($"Составлен {DateTime.Now}");

        string[] header = ["Номер п/п", "Бригада", "Тип вызова", "ID Контракта", "Фамилия", "Имя", "Отчество", "Номер телефона"];
        table.ResetCells(Reports.Count + 1, 8);

        TableRow row = table.Rows[0];
        row.IsHeader = true;
        row.Height = 20;    //unit: point, 1point = 0.3528 mm
        row.HeightType = TableRowHeightType.Exactly;
        row.RowFormat.BackColor = Color.Gray;
        for (var i = 0; i < header.Length; i++)
        {
            row.Cells[i].CellFormat.VerticalAlignment = Spire.Doc.Documents.VerticalAlignment.Middle;
            Paragraph p = row.Cells[i].AddParagraph();
            p.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
            var txtRange = p.AppendText(header[i]);
            txtRange.CharacterFormat.Bold = true;
        }

        for (var r = 0; r < Reports.Count; r++)
        {
            var dataRow = table.Rows[r + 1];
            dataRow.Height = 20;
            dataRow.HeightType = TableRowHeightType.Exactly;
            dataRow.RowFormat.BackColor = Color.Empty;

            dataRow.Cells[0].CellFormat.VerticalAlignment = Spire.Doc.Documents.VerticalAlignment.Middle;
            dataRow.Cells[1].CellFormat.VerticalAlignment = Spire.Doc.Documents.VerticalAlignment.Middle;
            dataRow.Cells[2].CellFormat.VerticalAlignment = Spire.Doc.Documents.VerticalAlignment.Middle;
            dataRow.Cells[3].CellFormat.VerticalAlignment = Spire.Doc.Documents.VerticalAlignment.Middle;
            dataRow.Cells[4].CellFormat.VerticalAlignment = Spire.Doc.Documents.VerticalAlignment.Middle;
            dataRow.Cells[5].CellFormat.VerticalAlignment = Spire.Doc.Documents.VerticalAlignment.Middle;
            dataRow.Cells[6].CellFormat.VerticalAlignment = Spire.Doc.Documents.VerticalAlignment.Middle;
            dataRow.Cells[7].CellFormat.VerticalAlignment = Spire.Doc.Documents.VerticalAlignment.Middle;

            dataRow.Cells[0].AddParagraph().AppendText((r + 1).ToString());
            dataRow.Cells[1].AddParagraph().AppendText(Reports[r].Name);
            dataRow.Cells[2].AddParagraph().AppendText(Reports[r].CallType.ToString());
            dataRow.Cells[3].AddParagraph().AppendText(Reports[r].ContractId.ToString());
            dataRow.Cells[4].AddParagraph().AppendText(Reports[r].LastName.ToString());
            dataRow.Cells[5].AddParagraph().AppendText(Reports[r].FirstName.ToString());
            dataRow.Cells[6].AddParagraph().AppendText(Reports[r].MiddleName.ToString());
            dataRow.Cells[7].AddParagraph().AppendText(Reports[r].PhoneNumber);
        }

        doc.SaveToFile($"Report1-{DateTime.Now}.docx", FileFormat.Docx);
    }
}
