using DaLang.Lims.Tools.Util;
using Seagull.BarTender.Print;

namespace DaLang.Lims.Tools.Extensions;

public class BarTenderExtension
{
    private Engine _engine = null;
    private LabelFormatDocument _format = null;
    private string _templateName = null;

    public string TemplateName => _templateName;

    public BarTenderExtension()
    {
        _engine = new Engine(true);
    }

    public BarTenderExtension(string templateName)
    {
        _engine = new Engine(true);

        SetPrintTemplate(templateName);
    }

    public void Dispose()
    {
        _engine?.Stop();
        _engine?.Dispose();
    }

    public void SetPrintTemplate(string templateName)
    {
        if (!string.IsNullOrWhiteSpace(_templateName))
        {
            if (templateName == _templateName)
                return;

            _engine.Documents.Close(_templateName, SaveOptions.DoNotSaveChanges);
        }

        _templateName = templateName;
        _format = _engine.Documents.Open(templateName);
    }

    public bool Print(string printerName, string jobName = "", int copies = 1)
    {
        if (_format.PrintSetup.SupportsIdenticalCopies)
            _format.PrintSetup.IdenticalCopiesOfLabel = 1;

        if (_format.PrintSetup.SupportsSerializedLabels)
            _format.PrintSetup.NumberOfSerializedLabels = copies;

        if (string.IsNullOrWhiteSpace(printerName))
            printerName = AppSettings.Configuration["SerialNoPrinter"] ?? "";

        _format.PrintSetup.PrinterName = printerName;
        int waitOut = 10000; // 10秒 超时

        var result = _format.Print(jobName, waitOut, out var messages);
        return result == Result.Success;
    }
}
