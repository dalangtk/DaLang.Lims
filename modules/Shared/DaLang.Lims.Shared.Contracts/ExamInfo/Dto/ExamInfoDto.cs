using DaLang.Lims.Shared.Contracts.ExamResult.Dto;

namespace DaLang.Lims.Shared.Contracts.ExamInfo.Dto;

public class ExamInfoDto : ExamInfoUpdateInput
{
    /// <summary>
    /// 结果列表
    /// </summary>
    public List<ExamResultDto> ResultList { get; set; }
    /// <summary>
    /// 年龄描述
    /// </summary>
    public string AgeDesc
    {
        get
        {
            var ageDesc = string.Empty;

            if (!string.IsNullOrWhiteSpace(Age1))
            {
                ageDesc += Age1 + (string.IsNullOrWhiteSpace(AgeUnitName1) ? "" : AgeUnitName1);
            }
            else
            {
                ageDesc = "/";
                return ageDesc;
            }

            if (!string.IsNullOrWhiteSpace(Age2))
            {
                ageDesc += Age2 + (string.IsNullOrWhiteSpace(AgeUnitName1) ? " " : "") + (string.IsNullOrWhiteSpace(AgeUnitName2) ? "" : AgeUnitName2);
            }

            return ageDesc;
        }
    }
    /// <summary>
    /// 有危急值
    /// </summary>
    //public bool HasCritical => ResultList?.Exists(v => v.HLFlag == "HH" || v.HLFlag == "LL") == true;
    public bool HasCritical { get; set; } = false;
}
