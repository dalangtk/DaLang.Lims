using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Entities;

#pragma warning disable CS8632

namespace DaLang.Lims.Web.Dev.Domain.CodeGroupDetail
{
    /// <summary>
    /// 模板明细 实体类
    /// </summary>
    /// <remarks></remarks>
    [SugarTable(TableName = "dev_code_groupdetail")]
    public partial class CodeGroupDetailEntity : EntityBase
    {
        /// <summary>
        /// 模板名称
        /// </summary>
        /// <remarks></remarks>
        [SugarColumn(IsNullable = true, CreateTableFieldSort = 1)]
        public string Name { get; set; }
        /// <summary>
        /// 模板分组
        /// </summary>
        /// <remarks></remarks>
        [SugarColumn(IsNullable = true, CreateTableFieldSort = 2)]
        public long GroupId { get; set; }
        /// <summary>
        /// 生成路径
        /// </summary>
        /// <remarks></remarks>
        [SugarColumn(IsNullable = true, CreateTableFieldSort = 3)]
        public string? Path { get; set; }
        /// <summary>
        /// 模板分组
        /// </summary>
        /// <remarks></remarks>
        [SugarColumn(IsNullable = true, CreateTableFieldSort = 4)]
        public string? GroupIds { get; set; }
        /// <summary>
        /// 模板内容
        /// </summary>
        /// <remarks></remarks>
        [SugarColumn(IsNullable = true, CreateTableFieldSort = 5)]
        public string Content { get; set; }
    }

}

#pragma warning restore CS8632

