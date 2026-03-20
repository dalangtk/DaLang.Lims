using DaLang.Lims.Pretreatment.Contracts.PretreatDataImportConfig.Dto;

namespace DaLang.Lims.Pretreatment.Contracts.DataImport.Dto;

public class PreUploadOutput
{
    public List<PretreatDataImportConfigDto> ExcelConfigs { get; set; }
    public List<PretreatDataImportConfigDto> GeneralConfigs { get; set; }
}
