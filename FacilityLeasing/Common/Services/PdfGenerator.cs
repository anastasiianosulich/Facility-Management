using FacilityLeasing.Common.Interfaces;
using FacilityLeasing.Dtos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace FacilityLeasing.Common.Services;

public class PdfGenerator : IPdfGenerator
{
    public byte[] GenerateContractPdf(EquipmentPlacementContractDto contract)
    {
        var pdfDocument = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1, Unit.Inch);
                page.DefaultTextStyle(x => x.FontSize(12).FontFamily(Fonts.Verdana));
                page.Content().Column(col =>
                {
                    col.Item().Text($"Contract ID: {contract.Id}");
                    col.Item().Text($"Production Facility: {contract.ProductionFacilityName}");
                    col.Item().Text($"Equipment Type: {contract.ProcessEquipmentTypeName}");
                    col.Item().Text($"Equipment Units: {contract.EquipmentUnits}");
                });
            });
        });

        using var memoryStream = new MemoryStream();
        pdfDocument.GeneratePdf(memoryStream);
        return memoryStream.ToArray();
    }
}
