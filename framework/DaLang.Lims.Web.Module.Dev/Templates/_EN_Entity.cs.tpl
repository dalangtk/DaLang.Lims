@{
    var gen = Model as DaLang.Lims.Web.Dev.Domain.CodeGen.CodeGenEntity;
    if (gen == null) return;
    if (gen.Fields == null) return;
    if (gen.Fields.Count() == 0) return;

    var entityNamePc = gen.EntityName.NamingPascalCase();
    var areaGrouping = "";
    if(!string.IsNullOrWhiteSpace(gen.AreaGrouping)){
        areaGrouping = gen.AreaGrouping.NamingPascalCase();
    }
    var entityClassName = entityNamePc
        .PadEndIfNot("Entity")
        .PadEndIfNotEmpty(gen.BaseEntity, ": " + gen.BaseEntity);

    var commonFields = new String[] { "id", "ProId", "ProName", "ProTime", "ModId"
    , "ModName","ModTime","IsModified", "IsDeleted","TenantId"};
}
using System;
using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Entities;
@foreach(var ns in gen.GetUsings())
{
@:using @(ns);    
}

#pragma warning disable CS8618
namespace @(gen.Namespace).Domain.@(entityNamePc)
{
    /// <summary>
    /// @gen.BusName @("实体类")
    /// </summary>
    /// <remarks>@(gen.Comment)</remarks>
    @(gen.GetTableIndexAttributes())
    public partial class @(entityClassName)
    {
@if (gen.Fields != null)
{
    foreach (var col in gen.Fields)
    {
        if (col == null) continue;
        if (!String.IsNullOrWhiteSpace(gen.BaseEntity))
            if (commonFields.Any(a => a.ToLower() == col.ColumnName.ToLower()))
                continue;

        if(!col.IsIgnoreColumn())
        {

        @:/// <summary>
        @:/// @(col.Title)
        @:/// </summary>
        @:/// <remarks>@(col.Comment)</remarks>
        @:@col.SqlSugarColumnAttribute()
        @:@col.PropCs()

        }
    }

}
    }

}

#pragma warning restore CS8618

