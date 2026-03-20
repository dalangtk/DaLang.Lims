namespace DaLang.Lims.Exam.Contracts.ReportFiles.Dto;

public class ReportFilesDto
{
    /// <summary>
    ///Id
    ///</summary>
    public long Id { get; set; }
    /// <summary>
    ///
    ///</summary>
    public long ExamInfoId { get; set; }
    /// <summary>
    ///报告地址
    ///</summary>
    public string? FilePath { get; set; }
    /// <summary>
    /// 文件名
    /// </summary>
    public string? FileName { get; set; }
    /// <summary>
    ///启用
    ///</summary>
    public bool IsValid { get; set; } = true;
    /// <summary>
    ///创建者Id
    ///</summary>
    public long? ProId { get; set; }
    /// <summary>
    ///创建者姓名
    ///</summary>
    public string? ProName { get; set; }
    /// <summary>
    ///创建时间
    ///</summary>
    public DateTime ProTime { get; set; }
    /// <summary>
    ///修改者Id
    ///</summary>
    public long? ModId { get; set; }
    /// <summary>
    ///修改者姓名
    ///</summary>
    public string? ModName { get; set; }
    /// <summary>
    ///更新时间
    ///</summary>
    public DateTime? ModTime { get; set; }
    /// <summary>
    ///已修改
    ///</summary>
    public bool IsModified { get; set; } = false;
    /// <summary>
    ///已删除
    ///</summary>
    public bool IsDeleted { get; set; } = false;

}
