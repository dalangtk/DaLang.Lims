using DaLang.Lims.Exam.Domain.ExamCriticalValue;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.Exam.Sqlsugar.Mysql.Repositories.ExamCriticalValue;

public class ExamCriticalValueRepository : AdminRepositoryBase<ExamCriticalValueEntity>, IExamCriticalValueRepository
{
    public ExamCriticalValueRepository()
    {
    }
}
