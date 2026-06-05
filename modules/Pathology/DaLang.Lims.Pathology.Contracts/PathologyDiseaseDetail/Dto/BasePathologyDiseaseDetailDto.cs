
namespace DaLang.Lims.Pathology.Contracts.PathologyDiseaseDetail.Dto;

/// <summary>
/// 疾病明细查询输出
/// </summary>
public partial class BasePathologyDiseaseDetailDto : BasePathologyDiseaseDetailUpdateInput
{
    public string? DiseaseName { get; set; }
    public string? SubDiseaseName { get; set; }
}