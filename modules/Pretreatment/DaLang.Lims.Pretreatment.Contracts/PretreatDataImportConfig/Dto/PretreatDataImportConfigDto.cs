namespace DaLang.Lims.Pretreatment.Contracts.PretreatDataImportConfig.Dto;

///<summary>
///导入配置查询结果输出
///</summary>
public partial class PretreatDataImportConfigDto
{
    /// <summary>
    ///Id
    ///</summary>
    public long Id { get; set; }
    /// <summary>
    ///客户代码
    ///</summary>
    public string? CustomerCode { get; set; }
    /// <summary>
    ///列名
    ///</summary>
    public string CellName { get; set; }
    /// <summary>
    ///字段名
    ///</summary>
    public string FieldName { get; set; }
    /// <summary>
    ///转换函数
    ///</summary>
    public string? TranslateFunction { get; set; }
    /// <summary>
    ///校验正则，非空，数字等
    ///</summary>
    public string? VerifyRegular { get; set; }
    /// <summary>
    ///校验数据格式
    ///</summary>
    public string? VerifyDataType { get; set; }
    /// <summary>
    /// 必须存在
    /// </summary>
    public bool IsMustExists { get; set; } = false;
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
    /// <summary>
    ///启用
    ///</summary>
    public bool IsValid { get; set; }
    /// <summary>
    /// 系统对照
    /// </summary>
    public bool? IsSystemCompare { get; set; } = null;

    /// <summary>
    /// excel是否有这列
    /// </summary>
    public bool IsExcelExists { get; set; }
}
