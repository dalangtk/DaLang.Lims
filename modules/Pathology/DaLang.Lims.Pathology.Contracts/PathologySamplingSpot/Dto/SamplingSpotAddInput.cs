namespace DaLang.Lims.Pathology.Contracts.PathologySamplingSpot.Dto;

/// <summary>
/// 取材部位新增输入
/// </summary>
public class SamplingSpotAddInput
{
    /// <summary>
    ///取材部位代码
    ///</summary>
    public string? SamplingSpotCode { get; set; }
    /// <summary>
    ///取材部位名称
    ///</summary>
    public string? SamplingSpotName { get; set; }
    /// <summary>
    ///性别
    ///</summary>
    public string? Gender { get; set; }
    /// <summary>
    ///拼音
    ///</summary>
    public string? PinYin { get; set; }
    /// <summary>
    ///排序
    ///</summary>
    public int Sort { get; set; }
    /// <summary>
    ///启用
    ///</summary>
    public bool IsValid { get; set; } = true;
}
