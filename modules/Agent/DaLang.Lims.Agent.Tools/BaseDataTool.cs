using DaLang.Lims.BaseData.Contracts.Group.Dto;
using DaLang.Lims.BaseData.Domain.Group;
using System.ComponentModel;

namespace DaLang.Lims.Agent.Tools;

public class BaseDataTool : ITools
{
    private readonly IBaseGroupRepository _baseGroupRep;
    public BaseDataTool(IBaseGroupRepository baseGroupRep)
    {
        _baseGroupRep = baseGroupRep;
    }
    public BaseDataTool()
    {

    }
    public string ToolName => "basedatatool";

    public string Description => "query base data";

    [Description("获取所有专业组信息")]
    public async Task<string> GetAllGroups()
    {
        var res = await _baseGroupRep.AsQueryable().Select(v => new BaseGroupDto
        {
            GroupCode = v.GroupCode,
            GroupName = v.GroupName,
        }).ToListAsync();

        return System.Text.Json.JsonSerializer.Serialize(res);
    }
}
