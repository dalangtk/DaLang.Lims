using DaLang.Lims.ReportTemplate.Domain.ReportTemplate;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.ReportTemplate.Repositories.ReportTemplate;

public class ReportTemplateRepository : AdminRepositoryBase<ReportTemplateEntity>, IReportTemplateRepository
{
    public ReportTemplateRepository()
    {
    }
}
