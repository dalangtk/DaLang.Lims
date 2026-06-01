@{
    var gen = Model as DaLang.Lims.Web.Dev.Domain.CodeGen.CodeGenEntity;
    if (gen == null) return;
    if (gen.Fields == null) return;
    if (gen.Fields.Count() == 0) return;

    var entityNamePc = gen.EntityName.NamingPascalCase();
}
using System.ComponentModel.DataAnnotations;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Core.Entities;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
@foreach (var ns in gen.GetUsings())
{
@:using @(ns);    
}
using @(gen.Namespace).Contracts.@(entityNamePc).Dto;

namespace @(gen.Namespace).Contracts.@(entityNamePc)
{
    /// <summary>
    /// @(gen.BusName)服务
    /// </summary>
    public interface I@(entityNamePc)Service
    {
        /// <summary>
        /// 查询
        /// </summary>
        Task<@(entityNamePc)Dto> GetAsync(long id);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        Task<PageOutput<@(entityNamePc)Dto>> GetPageAsync(PageInput<@(entityNamePc)QueryInput> input);
        
        @if(gen.GenGetList){
            @:
            @:/// <summary>
            @:/// 列表查询
            @:/// </summary>
            @:Task<IEnumerable<@(entityNamePc)GetListDto>> GetListAsync(@(entityNamePc)QueryInput input);
        }
        /// <summary>
        /// 新增
        /// </summary>
        Task<long> AddAsync(@(entityNamePc)AddInput input);
        
        /// <summary>
        /// 编辑
        /// </summary>
        Task UpdateAsync(@(entityNamePc)UpdateInput input);
        
        /// <summary>
        /// 删除
        /// </summary>
        Task<bool> DeleteAsync(long id);

        @if(gen.GenBatchDelete){
            @:/// <summary>
            @:/// 批量删除
            @:/// </summary>
            @:Task<bool> BatchDeleteAsync(long[] ids);
        }
    }
}
