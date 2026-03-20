using DaLang.Lims.BaseData.Domain.ExamPlan;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.BaseData.Sqlsugar.Mysql.Repositories.ExamPlan;

public class BaseExamPlanRepository : AdminRepositoryBase<BaseExamPlanEntity>, IBaseExamPlanRepository
{
    public BaseExamPlanRepository()
    {
    }
}
