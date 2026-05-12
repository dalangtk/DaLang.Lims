namespace DaLang.Lims.Pathology.Contracts.PathologySetting.Dto;

/// <summary>
/// 病理配置更新输入
/// </summary>
public class PathologySettingUpdateInput : PathologySettingAddInput
{
    /// <summary>
    ///Id
    ///</summary>
    public long Id { get; set; }
}
