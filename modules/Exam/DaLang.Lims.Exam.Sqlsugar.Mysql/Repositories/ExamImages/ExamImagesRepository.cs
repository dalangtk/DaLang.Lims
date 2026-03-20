using DaLang.Lims.Exam.Domain.ExamImages;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.Exam.Sqlsugar.Mysql.Repositories.ExamImages;

public class ExamImagesRepository : AdminRepositoryBase<ExamImagesEntity>, IExamImagesRepository
{
    public ExamImagesRepository()
    {
    }
}
