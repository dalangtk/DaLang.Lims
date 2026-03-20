namespace DaLang.Lims.BaseData.Contracts.UserGroup.Dto;

public class BaseUserGroupAddInput
{
    /// <summary>
    ///用户Id
    ///</summary>
    public long UserId { get; set; }
    /// <summary>
    ///组别代码
    ///</summary>
    public string GroupCode { get; set; }
    /// <summary>
    ///检验
    ///</summary>
    public int CanTest { get; set; } = 0;
    /// <summary>
    ///初审
    ///</summary>
    public int CanFirstCheck { get; set; } = 0;
    /// <summary>
    ///复审
    ///</summary>
    public int CanSecondCheck { get; set; } = 0;
    /// <summary>
    ///反审
    ///</summary>
    public int CanUnCheck { get; set; } = 0;
    /// <summary>
    ///打印后反审
    ///</summary>
    public int CanPrintedUnCheck { get; set; } = 0;
    /// <summary>
    ///取消检测
    ///</summary>
    public int CanCancelTest { get; set; } = 0;
    /// <summary>
    ///反取消检测
    ///</summary>
    public int CanDisCancel { get; set; } = 0;
    /// <summary>
    ///修改信息
    ///</summary>
    public int CanModifiedInfo { get; set; } = 0;
    /// <summary>
    ///启用
    ///</summary>
    public bool IsValid { get; set; } = true;
}
