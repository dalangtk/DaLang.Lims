@{
    var gen = Model as DaLang.Lims.Web.Dev.Domain.CodeGen.CodeGenEntity;
    if (gen == null) return;
    var entityNamePc = "" + gen.EntityName.NamingPascalCase();
}
using DaLang.Lims.Web.Framework.Core.Repositories;

namespace @(gen.Namespace).Domain.@(entityNamePc)
{
    public interface I@(entityNamePc)Repository : ISqlSugarRepository<@(entityNamePc)Entity>
    {
    }
}
