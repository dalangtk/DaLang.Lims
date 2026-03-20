using System;
using System.Collections.Generic;

namespace DaLang.Lims.Web.Framework.Core.Dto
{
    [Serializable]
    public class DynamicFilterInfo
    {
        /// <summary>
        /// 属性名：Name<para></para>
        /// 导航属性：Parent.Name<para></para>
        /// 多表：b.Name<para></para>
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// 操作符
        /// </summary>
        public DynamicFilterOperator Operator { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Filters 下的逻辑运算符
        /// </summary>
        public DynamicFilterLogic Logic { get; set; }
        /// <summary>
        /// 子过滤条件，它与当前的逻辑关系是 And<para></para>
        /// 注意：当前 Field 可以留空
        /// </summary>
        public List<DynamicFilterInfo> Filters { get; set; }

        /// <summary>
        /// 记录分拣记录控件类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 多选框数据源
        /// </summary>
        public List<LabelValueType> AllOptions { get; set; }
        public bool Multipleable { get; set; }
        public bool Filterable { get; set; }
        public bool Remoteable { get; set; }
        public string RemoteMethod { get; set; }
    }
    public class LabelValueType
    {
        public string Label { get; set; }
        public object value { get; set; }
    }
    public enum DynamicFilterLogic { And, Or }
    public enum DynamicFilterOperator
    {
        Equal,
        Like,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        In,
        NotIn,
        LikeLeft,
        LikeRight,
        NoEqual,
        IsNullOrEmpty,
        IsNot,
        NoLike,
        EqualNull,
        InLike,
        Range
    }
}
