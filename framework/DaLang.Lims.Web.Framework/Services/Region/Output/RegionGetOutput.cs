using System.Collections.Generic;

namespace DaLang.Lims.Web.Framework.Services.Region;

public class RegionGetOutput : RegionUpdateInput
{
    public List<long> ParentIdList { get; set; }
}