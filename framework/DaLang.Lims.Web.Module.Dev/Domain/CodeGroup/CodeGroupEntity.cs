using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Entities;

#pragma warning disable CS8632

namespace DaLang.Lims.Web.Dev.Domain.CodeGroup
{
    /// <summary>
    /// 模板组 实体类
    /// </summary>
    /// <remarks></remarks>
    [SugarTable(TableName="dev_code_group")]
    public partial class CodeGroupEntity: EntityBase
    {
        /// <summary>
        /// 模板组名称
        /// </summary>
        /// <remarks></remarks>
        [SugarColumn(IsNullable = true, CreateTableFieldSort = 1)]
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <remarks></remarks>
        [SugarColumn(IsNullable = true, CreateTableFieldSort = 2)]
        public string? Remark { get; set; }
    }

}

#pragma warning restore CS8632

