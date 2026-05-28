@{
    var gen = Model as DaLang.Lims.Web.Dev.Domain.CodeGen.CodeGenEntity;
    if (gen == null) return;
    if (gen.Fields == null) return;
    if (gen.Fields.Count() == 0) return;

    var entityNamePc = gen.EntityName.NamingPascalCase();
}

namespace @(gen.Namespace).Contracts.@(entityNamePc).Dto
{
@if(gen.GenAdd){
@:    /// <summary>@(gen.BusName)新增输入</summary>
@:    public partial class @(entityNamePc)AddInput {
        @foreach (var col in gen.Fields.Where(w=>w.WhetherAdd))
        {
            if (!col.IsIgnoreColumn())
            {
@:        /// <summary>@(col.Title)</summary>
                if (!col.IsNullable)
                {
@:        [Required(ErrorMessage = "@((!String.IsNullOrEmpty(col.Title)?col.Title:col.ColumnName)+"不能为空")")]
                }
@:        @col.PropCsByInput()                                                    
             }
        }
@:    }
}
}