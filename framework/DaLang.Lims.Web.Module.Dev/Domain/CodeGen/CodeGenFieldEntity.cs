using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Entities;

namespace DaLang.Lims.Web.Dev.Domain.CodeGen;

[SugarTable(TableName = "cg_config_field")]
public partial class CodeGenFieldEntity : EntityBase
{
    [SugarColumn(CreateTableFieldSort = 1)]
    public long CodeGenId { get; set; }

    /// <summary>
    /// 库定位器名
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 2)]
    public string DbKey { get; set; }

    /// <summary>
    /// 字段名
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 3)]
    public string ColumnName { get; set; } = "";

    /// <summary>
    /// 数据库列名(物理字段名)
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 4)]
    public string? ColumnRawName { get; set; }

    /// <summary>
    /// .NET数据类型
    /// </summary>
    [SugarColumn(DefaultValue = "string", CreateTableFieldSort = 5)]
    public string NetType { get; set; } = "string";

    /// <summary>
    /// 数据库中类型（物理类型）
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 6)]
    public string? DbType { get; set; }

    /// <summary>
    /// 字段描述
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 7)]
    public string? Comment { get; set; }

    /// <summary>
    /// 默认值
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 8)]
    public string? DefaultValue { get; set; }
    /// <summary>
    /// 字段标题
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 9)]
    public string Title { get; set; } = "";

    /// <summary>
    /// 主键
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 10)]
    public bool IsPrimary { get; set; }

    /// <summary>
    /// 可空
    /// </summary>
    [SugarColumn(DefaultValue = "1", CreateTableFieldSort = 11)]
    public bool IsNullable { get; set; }

    /// <summary>
    /// 长度
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 12)]
    public string? Length { get; set; }

    /// <summary>
    /// 编辑器
    /// </summary>
    [SugarColumn(DefaultValue = "el-input", IsNullable = true, CreateTableFieldSort = 13)]
    public string Editor { get; set; } = "el-input";

    /// <summary>
    /// 同步表结构时的列排序
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 14)]
    public int Position { get; set; }
    /// <summary>
    /// 是否通用字段
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 15)]
    public bool WhetherCommon { get; set; }

    /// <summary>
    /// 列表是否缩进（字典）
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 16)]
    public bool WhetherRetract { get; set; }

    /// <summary>
    /// 是否是查询条件
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 17)]
    public bool WhetherQuery { get; set; }
    /// <summary>
    /// 增
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 18)]
    public bool WhetherAdd { get; set; }
    /// <summary>
    /// 改
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 19)]
    public bool WhetherUpdate { get; set; }
    /// <summary>
    /// 分布显示
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 20)]
    public bool WhetherTable { get; set; }
    /// <summary>
    /// 列表
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 21)]
    public bool WhetherList { get; set; }

    /// <summary>
    /// 索引方式
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 22)]
    public string? IndexMode { get; set; }

    /// <summary>
    /// 唯一键
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 23)]
    public bool IsUnique { get; set; }

    /// <summary>
    /// 查询方式
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 24)]
    public string? QueryType { get; set; }


    /// <summary>
    /// 字典编码
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 25)]
    public string? DictTypeCode { get; set; }

    /// <summary>
    /// 外联实体名
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 26)]
    public string? IncludeEntity { get; set; }

    /// <summary>
    /// 外联对应关系 0 1对1 1 1对多
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 27)]
    public int IncludeMode { get; set; }

    /// <summary>
    /// 外联实体关联键
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 28)]
    public string? IncludeEntityKey { get; set; }

    /// <summary>
    /// 显示文本字段
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 29)]
    public string? DisplayColumn { get; set; }
    /// <summary>
    /// 选中值字段
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 30)]
    public string? ValueColumn { get; set; }

    /// <summary>
    /// 父级字段
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 31)]
    public string? PidColumn { get; set; }

    /// <summary>
    /// 作用类型（字典）
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 32)]
    public string? EffectType { get; set; }

    /// <summary>
    /// 前端规则检测触发时机
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 33)]
    public string? FrontendRuleTrigger { get; set; }

    [SugarColumn(IsNullable = true, CreateTableFieldSort = 34)]
    public int Sort { get; set; } = 0;
}
