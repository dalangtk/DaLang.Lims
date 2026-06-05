namespace DaLang.Lims.Exam.Contracts.ExamImages.Dto;

public class ExamImagesAddInput
{
    /// <summary>
    ///
    ///</summary>
    public long? ExamInfoId { get; set; }
    /// <summary>
    /// 文件名
    /// </summary>
    public string? FileName { get; set; }
    /// <summary>
    ///文件地址
    ///</summary>
    public string? FileUrl { get; set; }
    /// <summary>
    ///缩放代码
    ///</summary>
    public string? ZoomCode { get; set; }
    /// <summary>
    ///缩放名称
    ///</summary>
    public string? ZoomName { get; set; }
    /// <summary>
    ///抗体代码
    ///</summary>
    public string? AntiBodyCode { get; set; }
    /// <summary>
    ///抗体名称
    ///</summary>
    public string? AntiBodyName { get; set; }
    /// <summary>
    /// 图片类型
    /// </summary>
    public int ImageType { get; set; }
    /// <summary>
    ///是否显示
    ///</summary>
    public bool IsShow { get; set; } = true;
    /// <summary>
    ///排序
    ///</summary>
    public int Sort { get; set; }

}
