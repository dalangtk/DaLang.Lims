using DaLang.Lims.Web.Framework.Repositories;
using DaLang.Lims.Web.Dev.Domain.CodeGroup;

namespace DaLang.Lims.Web.Dev.Repositories.CodeGroup
{
    public class CodeGroupRepository : AdminRepositoryBase<CodeGroupEntity>, ICodeGroupRepository
    {
        public CodeGroupRepository()
        {
        }
    }
}
