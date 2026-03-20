namespace DaLang.Lims.Pretreatment.Contracts.DataAudit.Dto;

public class DataAuditOutput
{
    public string CustomerCode { get; set; }

    public string Barcode { get; set; }
    public string PatientName { get; set; }
    public bool AuditStatus { get; set; } = false;
    /// <summary>
    /// 审核失败类型，0目的对照缺失，1 其他
    /// </summary>
    public int AuditFailedType { get; set; }
    public string ErrMsg { get; set; }
    public List<CustomerPurMatchMainDto> NeedMatchPurList { get; set; } = new List<CustomerPurMatchMainDto>();
}
public class CustomerPurMatchMainDto
{
    public string CustomerCode { get; set; }
    public string CustomerPurCode { get; set; }
    public string CustomerPurName { get; set; }
}
