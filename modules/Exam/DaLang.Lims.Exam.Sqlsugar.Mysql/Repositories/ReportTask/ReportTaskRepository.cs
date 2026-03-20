using DaLang.Lims.Exam.Domain.ReportTask;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.Exam.Sqlsugar.Mysql.Repositories.ReportTask;

public class ReportTaskRepository : AdminRepositoryBase<ReportTaskEntity>, IReportTaskRepository
{
    public ReportTaskRepository()
    {
    }
}
