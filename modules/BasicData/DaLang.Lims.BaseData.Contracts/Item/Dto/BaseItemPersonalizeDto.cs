namespace DaLang.Lims.BaseData.Contracts.Item.Dto;

public class BaseItemPersonalizeDto
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// 项目代码
    /// </summary>
    public string ItemCode { get; set; }
    /// <summary>
    /// 个性化项目名称
    /// </summary>
    public string? ItemNamePersonalize { get; set; } = null;
    /// <summary>
    /// 是否计算项0否1是
    /// </summary>
    public bool? IsCalculcate { get; set; }
    /// <summary>
    /// 通用计算公式
    /// </summary>
    public string? CalcExpression { get; set; }
    /// <summary>
    /// 方法学
    /// </summary>
    public string? MethodCode { get; set; }
    /// <summary>
    /// 项目单位
    /// </summary>
    public string? ItemUnit { get; set; }
    /// <summary>
    /// 结果精度
    /// </summary>
    public int? ResultAccuracy { get; set; }
    /// <summary>
    /// 默认结果
    /// </summary>
    public string? DefaultValue { get; set; }
    /// <summary>
    /// 结果调整系数
    /// </summary>
    public string? ResultCoefficient { get; set; }
    /// <summary>
    /// 报告显示
    /// </summary>
    public bool? IsReportShow { get; set; }
    /// <summary>
    /// 结果复核
    /// </summary>
    public bool? IsReviewResult { get; set; }
    /// <summary>
    /// 必须参考范围
    /// </summary>
    public bool? IsMustReference { get; set; }
    /// <summary>
    /// 结果说明
    /// </summary>
    public string? ResultExplain { get; set; }
    /// <summary>
    /// 打印排序
    /// </summary>
    public string? PrintOrder { get; set; }
    /// <summary>
    /// 方法依据
    /// </summary>
    public string? MethodBasis { get; set; }
    /// <summary>
    /// 项目统计类别
    /// </summary>
    public string? ItemStaticType { get; set; }
    /// <summary>
    /// 项目系列
    /// </summary>
    public string? ItemSeriesType { get; set; }
    /// <summary>
    /// 拼音码
    /// </summary>
    public string? PinYin { get; set; }
    /// <summary>
    /// 五笔码
    /// </summary>
    public string? WuBi { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    public bool IsValid { get; set; }

}
