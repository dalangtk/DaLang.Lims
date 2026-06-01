@{
    var gen = Model as DaLang.Lims.Web.Dev.Domain.CodeGen.CodeGenEntity;
    if (gen == null) return;
    if (gen.Fields == null) return;
    if (gen.Fields.Count() == 0) return;

    var entityNamePc = gen.EntityName.NamingPascalCase();
}

namespace @(gen.Namespace).Contracts.@(entityNamePc).Dto
{
    /// <summary>
    /// @(gen.BusName)查询输出
    /// </summary>
    public partial class @(entityNamePc)Dto:@(entityNamePc)UpdateInput 
    {
   
    }
}