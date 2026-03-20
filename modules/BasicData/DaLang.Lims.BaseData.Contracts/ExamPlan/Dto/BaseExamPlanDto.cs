namespace DaLang.Lims.BaseData.Contracts.ExamPlan.Dto
{
    ///<summary>
    ///检测计划查询结果输出
    ///</summary>
    public partial class BaseExamPlanDto
    {
        /// <summary>
        ///Id
        ///</summary>
        public long Id { get; set; }
        /// <summary>
        ///检测计划代码
        ///</summary>
        public string ExamPlanCode { get; set; }
        /// <summary>
        ///检测计划名称
        ///</summary>
        public string ExamPlanName { get; set; }
        /// <summary>
        ///计划类型
        ///</summary>
        public string PlanType { get; set; }
        /// <summary>
        ///检测周期
        ///</summary>
        public string? PlanCycle { get; set; }
        /// <summary>
        ///包含节假日
        ///</summary>
        public bool IsIncludeHoliday { get; set; }
        /// <summary>
        ///备注
        ///</summary>
        public string? Remark { get; set; }
        /// <summary>
        ///排序
        ///</summary>
        public int Sort { get; set; }
        /// <summary>
        ///启用
        ///</summary>
        public bool IsValid { get; set; }
    }
}
