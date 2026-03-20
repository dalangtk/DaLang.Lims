@{
    var gen = Model as DaLang.Lims.Web.Dev.Domain.CodeGen.CodeGenEntity;
    if (gen == null) return;
    if (gen.Fields == null) return;
    if (gen.Fields.Count() == 0) return;

    var areaName = "" + gen.ApiAreaName;
    var entityName = "" + gen.EntityName;
    var entityNameLower = entityName.ToLower();

    var areaNamePc = areaName.NamingPascalCase();
    var entityNamePc = entityName.NamingPascalCase();

    var areaNameKc = areaName.NamingKebabCase();// KebabCase(areaName);
    var entityNameKc = entityName.NamingKebabCase();// KebabCase(entityName);

    var areaNameCc = areaName.NamingCamelCase();// camelCase(areaName);
    var entityNameCc = entityName.NamingCamelCase();// camelCase(entityName);
    var areaGrouping = "";
    if(!string.IsNullOrWhiteSpace(gen.AreaGrouping)){
        areaGrouping = "/" + gen.AreaGrouping.ToLower();
    }

    var at = "@";
    var apiName = entityName + "Api";

    var queryColumns = gen.Fields.Where(w => w.WhetherQuery);
    var commonFields = new String[] { "id", "ProId", "ProName", "ProTime", "ModId"
    , "ModName","ModTime","IsModified", "IsDeleted","TenantId"};
}

/**
 * 查询@(gen.BusName)列表入参
 */
export interface @(entityNamePc)QueryListInput {
    @if(queryColumns.Any()){
        foreach(var c in queryColumns){
            @:    @(c.ColumnName.NamingCamelCase()): @(CodeGenFieldEntityExtension.GetDefaultValuestringScript(c)),
        }
    }
}
/**
 * 新增@(gen.BusName)入参
 */
export interface @(entityNamePc)AddInput {
   @if(gen.Fields.Any()){
        foreach(var c in gen.Fields){
            if (commonFields.Any(a => a.ToLower() == c.ColumnName.ToLower()))
                continue;

            if(c.NetType.Contains("string")){
                @:@(c.ColumnName.NamingCamelCase())?: string @(c.IsNullable == true ? "| null" : "")
            }
            if(c.NetType.Contains("int") || c.NetType.Contains("long")|| c.NetType.Contains("decimal")){
                @:@(c.ColumnName.NamingCamelCase())?: number @(c.IsNullable == true ? "| null" : "")
            }
            if(c.NetType.Contains("bool")){
                @:@(c.ColumnName.NamingCamelCase())?: boolean @(c.IsNullable == true ? "| null" : "")
            }
            if(c.NetType.Contains("DateTime")){
                @:@(c.ColumnName.NamingCamelCase())?: Date @(c.IsNullable == true ? "| null" : "")
            }
        }
    }
}
/**
 * 更新@(gen.BusName)入参
 */
export interface @(entityNamePc)UpdateInput extends @(entityNamePc)AddInput {
  id: number
}
/**
 * 获取单个@(gen.BusName)返回
 */
export interface @(entityNamePc)Output extends @(entityNamePc)UpdateInput {
  
}

/**
 * 获取@(gen.BusName)列表返回
 */
export interface @(entityNamePc)ListOutput extends @(entityNamePc)Output {
  proId: number
  proName?: string | null
  proTime?: Date | null
  modId: number
  modName?: string | null
  modTime?: Date | null
}