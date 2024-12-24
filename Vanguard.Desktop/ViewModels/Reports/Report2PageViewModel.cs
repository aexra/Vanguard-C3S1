using System.Collections.ObjectModel;
using System.Net.Http.Json;
using Spire.Doc.Documents;
using Spire.Doc;
using Vanguard.App.Helpers;
using Vanguard.DataAccess.Models;
using System.Drawing;

namespace Vanguard.Desktop.ViewModels.Reports;
public partial class Report2PageViewModel : ObservableObject
{
    public ObservableCollection<Report2> Reports { get; set; } = new();

    public async Task LoadAsync()
    {
        Reports.Clear();

        var response = await ApiHelper.GetReport(2);

        if (response != null && response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<List<Report2>>();

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

        titleParagraph.AppendText("Отчет о самых прибыльных и защищенных организациях-клиентах");
        titleParagraph.ApplyStyle(BuiltinStyle.Header);
        titleParagraph.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;

        dateParagraph.AppendText($"Составлен {DateTime.Now}");

        string[] header = ["Номер п/п", "Название", "Сумма (руб.)", "Количество тревог"];
        table.ResetCells(Reports.Count + 1, 4);

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

            dataRow.Cells[0].AddParagraph().AppendText((r + 1).ToString());
            dataRow.Cells[1].AddParagraph().AppendText(Reports[r].Name);
            dataRow.Cells[2].AddParagraph().AppendText(Reports[r].Sum.ToString());
            dataRow.Cells[3].AddParagraph().AppendText(Reports[r].Count.ToString());
        }

        doc.SaveToFile($"Report2-{DateTime.Now}.docx", FileFormat.Docx);
    }
}
