
namespace DaLang.Lims.Pathology.Contracts.PathologyDiseaseDetail.Dto;

/// <summary>
/// 疾病明细新增输入
/// </summary>
public partial class BasePathologyDiseaseDetailAddInput
{
    public string DiseaseCode { get; set; }
    public string SubDiseaseCode { get; set; }
}