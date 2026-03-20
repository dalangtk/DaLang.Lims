using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Pretreatment.Domain.PretreatSampleAmount
{
    /// <summary>
    /// 样本数量 实体类
    /// </summary>
    /// <remarks></remarks>
    [SugarTable(TableName = "pretreat_sample_amount")]
    public partial class PretreatSampleAmountEntity : EntityTenant
    {
        /// <summary>
        /// 客户代码
        /// </summary>
        /// <remarks>客户代码</remarks>
        [SugarColumn(ColumnName = "CustomerCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
        public string CustomerCode { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        /// <remarks>客户名称</remarks>
        [SugarColumn(ColumnName = "CustomerName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
        public string CustomerName { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        /// <remarks>条码</remarks>
        [SugarColumn(ColumnName = "Barcode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
        public string Barcode { get; set; }
        /// <summary>
        /// 数据来源 0excel导入 1api对接 2直接录入
        /// </summary>
        /// <remarks>数据来源</remarks>
        [SugarColumn(ColumnName = "DataSource", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2)]
        public int DataSource { get; set; }
        /// <summary>
        /// 标本数量
        /// </summary>
        /// <remarks>标本数量</remarks>
        [SugarColumn(ColumnName = "SampleCnt", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 4)]
        public int SampleCnt { get; set; }
        /// <summary>
        /// 信息录入状态 1一次 2二次 3待校对 4已校对
        /// </summary>
        /// <remarks>信息录入状态</remarks>
        [SugarColumn(ColumnName = "InfoStatus", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2)]
        public int? InfoStatus { get; set; }
        /// <summary>
        /// 一次信息录入人id
        /// </summary>
        /// <remarks>一次信息录入人id</remarks>
        [SugarColumn(ColumnName = "FirstInfoInputId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
        public long? FirstInfoInputId { get; set; }
        /// <summary>
        /// 一次信息录入人
        /// </summary>
        /// <remarks>一次信息录入人</remarks>
        [SugarColumn(ColumnName = "FirstInfoInputName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
        public long? FirstInfoInputName { get; set; }
        /// <summary>
        /// 一次信息录入时间
        /// </summary>
        /// <remarks>一次信息录入时间</remarks>
        [SugarColumn(ColumnName = "FirstInfoInputTime", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
        public DateTime? FirstInfoInputTime { get; set; }
        /// <summary>
        /// 二次信息录入人id
        /// </summary>
        /// <remarks>二次信息录入人id</remarks>
        [SugarColumn(ColumnName = "SecondInfoInputId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
        public long? SecondInfoInputId { get; set; }
        /// <summary>
        /// 二次信息录入人
        /// </summary>
        /// <remarks>二次信息录入人</remarks>
        [SugarColumn(ColumnName = "SecondInfoInputName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
        public string? SecondInfoInputName { get; set; }
        /// <summary>
        /// 二次信息录入时间
        /// </summary>
        /// <remarks>二次信息录入时间</remarks>
        [SugarColumn(ColumnName = "SecondInfoInputTime", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
        public DateTime? SecondInfoInputTime { get; set; }
        /// <summary>
        /// 项目录入状态 1一次 2二次 3待校对 4已校对
        /// </summary>
        /// <remarks>项目录入状态</remarks>
        [SugarColumn(ColumnName = "ItemStatus", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2)]
        public int? ItemStatus { get; set; }
        /// <summary>
        /// 一次项目录入人id
        /// </summary>
        /// <remarks>一次项目录入人id</remarks>
        [SugarColumn(ColumnName = "FirstItemInputId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
        public long? FirstItemInputId { get; set; }
        /// <summary>
        /// 一次项目录入人
        /// </summary>
        /// <remarks>一次项目录入人</remarks>
        [SugarColumn(ColumnName = "FirstItemInputName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
        public string? FirstItemInputName { get; set; }
        /// <summary>
        /// 一次项目录入时间
        /// </summary>
        /// <remarks>一次项目录入时间</remarks>
        [SugarColumn(ColumnName = "FirstItemInputTime", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
        public DateTime? FirstItemInputTime { get; set; }
        /// <summary>
        /// 二次项目录入人id
        /// </summary>
        /// <remarks>二次项目录入人id</remarks>
        [SugarColumn(ColumnName = "SecondItemInputId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
        public long? SecondItemInputId { get; set; }
        /// <summary>
        /// 二次项目录入人
        /// </summary>
        /// <remarks>二次项目录入人</remarks>
        [SugarColumn(ColumnName = "SecondItemInputName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
        public string? SecondItemInputName { get; set; }
        /// <summary>
        /// 二次项目录入时间
        /// </summary>
        /// <remarks>二次项目录入时间</remarks>
        [SugarColumn(ColumnName = "SecondItemInputTime", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
        public DateTime? SecondItemInputTime { get; set; }
        /// <summary>
        /// 信息待处理
        /// </summary>
        /// <remarks>信息待处理</remarks>
        [SugarColumn(ColumnName = "IsInfoWaitProcess", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2)]
        public int? IsInfoWaitProcess { get; set; }
        /// <summary>
        /// 信息待处理原因
        /// </summary>
        /// <remarks>信息待处理原因</remarks>
        [SugarColumn(ColumnName = "InfoWaitProcessReason", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 128)]
        public string? InfoWaitProcessReason { get; set; }
        /// <summary>
        /// 信息加入待处理Id
        /// </summary>
        /// <remarks>信息加入待处理Id</remarks>
        [SugarColumn(ColumnName = "InfoWaitProcessId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
        public long? InfoWaitProcessId { get; set; }
        /// <summary>
        /// 信息加入待处理人
        /// </summary>
        /// <remarks>信息加入待处理人</remarks>
        [SugarColumn(ColumnName = "InfoWaitProcessName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
        public string? InfoWaitProcessName { get; set; }
        /// <summary>
        /// 信息加入待处理时间
        /// </summary>
        /// <remarks>信息加入待处理时间</remarks>
        [SugarColumn(ColumnName = "InfoWaitProcessTime", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
        public DateTime? InfoWaitProcessTime { get; set; }
        /// <summary>
        /// 项目待处理
        /// </summary>
        /// <remarks>项目待处理</remarks>
        [SugarColumn(ColumnName = "IsItemWaitProcess", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2)]
        public int? IsItemWaitProcess { get; set; }
        /// <summary>
        /// 项目待处理原因
        /// </summary>
        /// <remarks>项目待处理原因</remarks>
        [SugarColumn(ColumnName = "ItemWaitProcessReason", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 128)]
        public string? ItemWaitProcessReason { get; set; }
        /// <summary>
        /// 项目加入待处理Id
        /// </summary>
        /// <remarks>项目加入待处理Id</remarks>
        [SugarColumn(ColumnName = "ItemWaitProcessId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
        public long? ItemWaitProcessId { get; set; }
        /// <summary>
        /// 项目加入待处理人
        /// </summary>
        /// <remarks>项目加入待处理人</remarks>
        [SugarColumn(ColumnName = "ItemWaitProcessName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
        public string? ItemWaitProcessName { get; set; }
        /// <summary>
        /// 项目加入待处理时间
        /// </summary>
        /// <remarks>项目加入待处理时间</remarks>
        [SugarColumn(ColumnName = "ItemWaitProcessTime", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
        public DateTime? ItemWaitProcessTime { get; set; }
        /// <summary>
        /// 信息校对人Id
        /// </summary>
        /// <remarks>信息校对人Id</remarks>
        [SugarColumn(ColumnName = "InfoCheckId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
        public long? InfoCheckId { get; set; }
        /// <summary>
        /// 信息校对人
        /// </summary>
        /// <remarks>信息校对人</remarks>
        [SugarColumn(ColumnName = "InfoCheckName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
        public string? InfoCheckName { get; set; }
        /// <summary>
        /// 信息校对时间
        /// </summary>
        /// <remarks>信息校对时间</remarks>
        [SugarColumn(ColumnName = "InfoCheckTime", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
        public DateTime? InfoCheckTime { get; set; }
        /// <summary>
        /// 项目校对人Id
        /// </summary>
        /// <remarks>项目校对人Id</remarks>
        [SugarColumn(ColumnName = "ItemCheckId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
        public long? ItemCheckId { get; set; }
        /// <summary>
        /// 项目校对人
        /// </summary>
        /// <remarks>项目校对人</remarks>
        [SugarColumn(ColumnName = "ItemCheckName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
        public string? ItemCheckName { get; set; }
        /// <summary>
        /// 项目校对时间
        /// </summary>
        /// <remarks>项目校对时间</remarks>
        [SugarColumn(ColumnName = "ItemCheckTime", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
        public DateTime? ItemCheckTime { get; set; }
    }

}

#pragma warning restore CS8618

