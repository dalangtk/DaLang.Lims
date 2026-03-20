
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using DaLang.Lims.Web.Framework.Domain.Pkg;

namespace DaLang.Lims.Web.Framework.Services.Tenant.Dto;

public class TenantGetOutput : TenantUpdateInput
{
    /// <summary>
    /// 套餐列表
    /// </summary>
    [JsonIgnore]
    public ICollection<PkgEntity> Pkgs { get; set; }

    /// <summary>
    /// 套餐Ids
    /// </summary>
    public override long[] PkgIds => Pkgs?.Select(a => a.Id)?.ToArray();
}