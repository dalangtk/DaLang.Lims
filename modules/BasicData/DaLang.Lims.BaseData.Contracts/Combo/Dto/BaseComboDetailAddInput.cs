namespace DaLang.Lims.BaseData.Contracts.Combo.Dto;

public class BaseComboDetailAddInput
{
    /// <summary>
    ///
    ///</summary>
    public string ComboCode { get; set; }
    /// <summary>
    ///目的代码
    ///</summary>
    public string PurCode { get; set; }
    /// <summary>
    ///目的名称
    ///</summary>
    public string PurName { get; set; }
    /// <summary>
    ///启用
    ///</summary>
    public bool IsValid { get; set; } = true;
}
