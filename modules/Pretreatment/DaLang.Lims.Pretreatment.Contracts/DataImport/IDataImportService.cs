using DaLang.Lims.Pretreatment.Contracts.DataImport.Dto;
using DaLang.Lims.Pretreatment.Contracts.PretreatDataImportConfig.Dto;
using DaLang.Lims.Pretreatment.Contracts.PretreatSampleInfo.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DaLang.Lims.Pretreatment.Contracts.DataImport;

public interface IDataImportService
{
    Task<List<PretreatDataImportConfigDto>> DataPreImport([Required] IFormFile file);
    Task<List<PretreatSampleInfoDto>> DataImport([Required] IFormFile file, string importConfigsJson = "");
    Task<PageOutput<PretreatSampleInfoDto>> GetImportedSampleInfoAsync(PageInput<QueryImportDataInput> pageInput);
    Task<bool> DeleteInfoAsync(List<long> delIds);
    Task<List<string>> CheckBarcodeUsed(List<string> barcodes);
}
