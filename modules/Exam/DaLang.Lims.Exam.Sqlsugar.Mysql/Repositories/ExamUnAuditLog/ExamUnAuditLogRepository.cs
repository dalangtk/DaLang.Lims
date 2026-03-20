using DaLang.Lims.Exam.Domain.ExamUnAuditLog;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.Exam.Sqlsugar.Mysql.Repositories.ExamUnAuditLog;

public class ExamUnAuditLogRepository : AdminRepositoryBase<ExamUnAuditLogEntity>, IExamUnAuditLogRepository
{
    public ExamUnAuditLogRepository()
    {
    }
}
