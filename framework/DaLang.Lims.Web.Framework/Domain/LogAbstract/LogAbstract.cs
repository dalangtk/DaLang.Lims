using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Entities;

namespace DaLang.Lims.Web.Framework.Domain;

/// <summary>
/// 日志
/// </summary>
public abstract class LogAbstract : EntityTenant
{

    /// <summary>
    /// 姓名
    /// </summary>
    [SugarColumn(Length = 32, CreateTableFieldSort = 1)]
    public string Name { get; set; }

    /// <summary>
    /// IP
    /// </summary>
    [SugarColumn(Length = 32, CreateTableFieldSort = 2)]
    public string IP { get; set; }

    /// <summary>
    /// 浏览器
    /// </summary>
    [SugarColumn(Length = 64, CreateTableFieldSort = 3)]
    public string Browser { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    [SugarColumn(Length = 32, CreateTableFieldSort = 4)]
    public string Os { get; set; }

    /// <summary>
    /// 设备
    /// </summary>
    [SugarColumn(Length = 32, CreateTableFieldSort = 5)]
    public string Device { get; set; }

    /// <summary>
    /// 浏览器信息
    /// </summary>
    [SugarColumn(Length = 256, CreateTableFieldSort = 6)]
    public string BrowserInfo { get; set; }

    /// <summary>
    /// 耗时（毫秒）
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 7)]
    public long ElapsedMilliseconds { get; set; }

    /// <summary>
    /// 操作状态
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 8)]
    public bool Status { get; set; }

    /// <summary>
    /// 操作消息
    /// </summary>
    [SugarColumn(Length = 64, IsNullable = true, CreateTableFieldSort = 9)]
    public string Msg { get; set; }

    /// <summary>
    /// 操作结果
    /// </summary>
    [SugarColumn(Length = 32, IsNullable = true, CreateTableFieldSort = 10)]
    public string Result { get; set; }
}