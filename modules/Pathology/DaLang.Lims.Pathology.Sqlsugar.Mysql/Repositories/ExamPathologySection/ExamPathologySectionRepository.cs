
using DaLang.Lims.Pathology.Domain.ExamPathologySection;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.Pathology.Repositories.ExamPathologySection;

public class ExamPathologySectionRepository : AdminRepositoryBase<ExamPathologySectionEntity>, IExamPathologySectionRepository
{
    public ExamPathologySectionRepository()
    {
    }
}
