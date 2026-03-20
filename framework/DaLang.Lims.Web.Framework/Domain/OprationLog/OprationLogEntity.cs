using SqlSugar;

namespace DaLang.Lims.Web.Framework.Domain.OprationLog;

/// <summary>
/// 操作日志
/// </summary>
[SugarTable(TableName = "sys_opration_log")]
public partial class OprationLogEntity : LogAbstract
{
    /// <summary>
    /// 接口名称
    /// </summary>
    [SugarColumn(Length = 32, IsNullable = true, CreateTableFieldSort = 11)]
    public string ApiLabel { get; set; }

    /// <summary>
    /// 接口地址
    /// </summary>
    [SugarColumn(Length = 128, IsNullable = true, CreateTableFieldSort = 12)]
    public string ApiPath { get; set; }

    /// <summary>
    /// 接口提交方法
    /// </summary>
    [SugarColumn(Length = 16, IsNullable = true, CreateTableFieldSort = 13)]
    public string ApiMethod { get; set; }

    /// <summary>
    /// 操作参数
    /// </summary>
    [SugarColumn(Length = 512, IsNullable = true, CreateTableFieldSort = 14)]
    public string Params { get; set; }
}