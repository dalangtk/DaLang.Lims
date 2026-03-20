using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Entities;

namespace DaLang.Lims.Web.Dev.Domain.CodeGen;


[SugarTable(TableName = "cg_config")]
public class CodeGenEntity : EntityBase
{
    /// <summary>
    /// 作者姓名
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 1)]
    public string AuthorName { get; set; }

    /// <summary>
    /// 是否移除表前缀
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 2)]
    public bool TablePrefix { get; set; } = true;

    /// <summary>
    /// 生成方式 1 CodeFirst 2 DbFirst
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 3)]
    public string? GenerateType { get; set; }

    /// <summary>
    /// 库定位器名
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 4)]
    public string DbKey { get; set; }

    /// <summary>
    /// 数据库名(保留字段)
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 5)]
    public string? DbName { get; set; }

    /// <summary>
    /// 数据库类型
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 6)]
    public string? DbType { get; set; }

    /// <summary>
    /// 数据库表名
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 7)]
    public string TableName { get; set; }

    /// <summary>
    /// 命名空间
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 8)]
    public string Namespace { get; set; }

    /// <summary>
    /// 实体名称
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 9)]
    public string EntityName { get; set; }

    /// <summary>
    /// 业务名
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 10)]
    public string BusName { get; set; }

    /// <summary>
    /// Api分区名称
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 11)]
    public string? ApiAreaName { get; set; }
    /// <summary>
    /// API区域二级分组名称
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 12)]
    public string? AreaGrouping { get; set; }
    /// <summary>
    /// 基类名称
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 13)]
    public string? BaseEntity { get; set; }

    /// <summary>
    /// 菜单编码
    /// </summary>
    [SugarColumn(DefaultValue = "/app", IsNullable = true, CreateTableFieldSort = 14)]
    public string MenuPid { get; set; } = "/app";

    /// <summary>
    /// 菜单后缀
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 15)]
    public string MenuAfterText { get; set; }

    /// <summary>
    /// 后端输出目录
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 16)]
    public string BackendOut { get; set; }

    /// <summary>
    /// 前端输出目录
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 17)]
    public string? FrontendOut { get; set; }

    /// <summary>
    /// 数据迁移输出目录
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 18)]
    public string DbMigrateSqlOut { get; set; }

    /// <summary>
    /// 备注说明
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 19)]
    public string? Comment { get; set; }

    /// <summary>
    /// 实体导入的命令空间
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 20)]
    public string? Usings { get; set; }

    /// <summary>
    /// 生成Entity实体类
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 21)]
    public bool GenEntity { get; set; }
    /// <summary>
    /// 生成Repository仓储类
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 22)]
    public bool GenRepository { get; set; }
    /// <summary>
    /// 生成Service服务类
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 23)]
    public bool GenService { get; set; }

    /// <summary>
    /// 生成新增服务
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 24)]
    public bool GenAdd { get; set; } = true;
    /// <summary>
    /// 生成更新服务
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 25)]
    public bool GenUpdate { get; set; } = true;
    /// <summary>
    /// 新增删除服务
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 26)]
    public bool GenDelete { get; set; } = true;

    /// <summary>
    /// 生成列表查询服务
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 27)]
    public bool GenGetList { get; set; }
    /// <summary>
    /// 生成软删除服务
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 27)]
    public bool GenSoftDelete { get; set; }
    /// <summary>
    /// 生成批量删除服务
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 28)]
    public bool GenBatchDelete { get; set; }
    /// <summary>
    /// 生成批量软删除服务
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 29)]
    public bool GenBatchSoftDelete { get; set; }
    /// <summary>
    /// 生成界面类型，单表/主子表
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 30)]
    public string GenViewType { get; set; } = "Single";

    [Navigate(NavigateType.OneToMany, nameof(CodeGenFieldEntity.CodeGenId), nameof(Id))]
    [SugarColumn(IsIgnore = true)]
    public List<CodeGenFieldEntity>? Fields { get; set; }
}