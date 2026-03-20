namespace DaLang.Lims.Pretreatment.Contracts.DataImport.Dto;

public class PretreatImportedConfigDto
{
    /// <summary>
    ///Id
    ///</summary>
    public long Id { get; set; }
    /// <summary>
    ///租户Id
    ///</summary>
    public long TenantId { get; set; }
    /// <summary>
    ///文件id
    ///</summary>
    public long? FileName { get; set; }
    /// <summary>
    ///导入配置json
    ///</summary>
    public string? ConfigJson { get; set; }
    /// <summary>
    ///创建者Id
    ///</summary>
    public long? ProId { get; set; }
    /// <summary>
    ///创建者姓名
    ///</summary>
    public string? ProName { get; set; }
    /// <summary>
    ///创建时间
    ///</summary>
    public DateTime ProTime { get; set; }
    /// <summary>
    ///修改者Id
    ///</summary>
    public long? ModId { get; set; }
    /// <summary>
    ///修改者姓名
    ///</summary>
    public string? ModName { get; set; }
    /// <summary>
    ///更新时间
    ///</summary>
    public DateTime? ModTime { get; set; }
    /// <summary>
    ///已修改
    ///</summary>
    public bool IsModified { get; set; }
}
