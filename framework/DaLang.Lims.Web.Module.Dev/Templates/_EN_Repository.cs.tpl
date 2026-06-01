@{
    var gen = Model as DaLang.Lims.Web.Dev.Domain.CodeGen.CodeGenEntity;
    if (gen == null) return;
    var entityNamePC = gen.EntityName.NamingPascalCase();
}

using @(gen.Namespace).Domain.@(entityNamePC);
using DaLang.Lims.Web.Framework.Core.Db.Transaction;
using DaLang.Lims.Web.Framework.Repositories;

namespace @(gen.Namespace).Repositories.@(entityNamePC)
{
    public class @(entityNamePC)Repository : AdminRepositoryBase<@(entityNamePC)Entity>, I@(entityNamePC)Repository
    {
        public @(entityNamePC)Repository()
        {
        }
    }
}
