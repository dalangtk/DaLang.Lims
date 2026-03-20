using System;
using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Entities;

#pragma warning disable CS8618
namespace DaLang.Lims.Web.BaseData.Domain.BaseAskRuleDetail
{
    /// <summary>
    /// 问询规则明细 实体类
    /// </summary>
    /// <remarks>问询规则明细</remarks>
    [SugarTable(TableName="base_ask_rule_detail")]
    public partial class BaseAskRuleDetailEntity: EntityTenant
    {
        /// <summary>
        /// 问询规则代码
        /// </summary>
        /// <remarks>问询规则代码</remarks>
        [SugarColumn(ColumnName="AskRuleCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType="varchar", Length=8)]
        public string AskRuleCode { get; set; }
        /// <summary>
        /// 委托周期
        /// </summary>
        /// <remarks>委托周期</remarks>
        [SugarColumn(ColumnName="EntrustCycle", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType="varchar", Length=16)]
        public string? EntrustCycle { get; set; }
        /// <summary>
        /// 问询日期
        /// </summary>
        /// <remarks>问询日期</remarks>
        [SugarColumn(ColumnName="AskDay", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType="int", DecimalDigits = 2)]
        public int? AskDay { get; set; }
        /// <summary>
        /// 问询时间
        /// </summary>
        /// <remarks>问询时间</remarks>
        [SugarColumn(ColumnName="AskTime", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType="varchar", Length=8)]
        public string? AskTime { get; set; }
        /// <summary>
        /// 包含节假日
        /// </summary>
        /// <remarks>包含节假日</remarks>
        [SugarColumn(ColumnName="IsIncludeHoliday", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType="tinyint", DefaultValue="0")]
        public bool IsIncludeHoliday { get; set; } = false;
        /// <summary>
        /// 排序
        /// </summary>
        /// <remarks>排序</remarks>
        [SugarColumn(ColumnName="Sort", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType="int", DecimalDigits = 11)]
        public int Sort { get; set; }
        /// <summary>
        /// 启用
        /// </summary>
        /// <remarks>启用</remarks>
        [SugarColumn(ColumnName="IsValid", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType="tinyint", DefaultValue="true")]
        public bool IsValid { get; set; } = true;
    }

}

#pragma warning restore CS8618

