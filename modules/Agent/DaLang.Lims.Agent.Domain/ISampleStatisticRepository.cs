using DaLang.Lims.Agent.Contracts.Dto;
using DaLang.Lims.BaseData.Contracts.Group.Dto;
using DaLang.Lims.Shared.Domain.ExamInfo;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.Agent.Domain;

public interface ISampleStatisticRepository
{
    Task<List<BaseGroupDto>> GetAllGroups();
    Task<List<CustomerSampleCountOutput>> GetReportCountGroupByCusomerAndTestate(CustomerSampleCountInput input);
}
