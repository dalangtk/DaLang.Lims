using MiniExcelLibs;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace DaLang.Lims.Web.Common.Helpers;

public static class ExcelHelper
{
    public static DataTable QueryTable(string fileName, bool useHeaderRow = true, string sheetName = null)
    {
        var table = MiniExcel.QueryAsDataTable(fileName, useHeaderRow, sheetName);
        return table;
    }
    public static DataTable QueryTableByStream(Stream stream, bool useHeaderRow = true, string sheetName = null)
    {
        var table = MiniExcel.QueryAsDataTable(stream, useHeaderRow, sheetName);
        return table;
    }
    public static IEnumerable<T> QueryList<T>(string fileName) where T : class, new()
    {
        var res = MiniExcel.Query<T>(fileName);
        return res;
    }
}
