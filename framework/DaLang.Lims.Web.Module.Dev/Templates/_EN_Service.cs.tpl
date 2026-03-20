@{
    var gen = Model as DaLang.Lims.Web.Dev.Domain.CodeGen.CodeGenEntity;
    if (gen == null) return;
    if (gen.Fields == null) return;
    if (gen.Fields.Count() == 0) return;

    var entityNamePc = gen.EntityName?.NamingPascalCase();
    var entityNameCc = gen.EntityName?.NamingCamelCase();
    var moduleNamePc = gen.ApiAreaName?.NamingPascalCase();
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using System.Collections.Generic;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using Microsoft.AspNetCore.Mvc;


using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Domain.Dict;
using SqlSugar;

using @(gen.Namespace).Domain.@(entityNamePc);
using @(gen.Namespace).Services.@(entityNamePc).Dto;
using @(gen.Namespace).Core.Consts;


namespace @(gen.Namespace).Services.@(entityNamePc)
{
    /// <summary>
    /// @(gen.BusName)服务
    /// </summary>
    [DynamicApi(Area = @(moduleNamePc)Consts.AreaName)]
    public class @(entityNamePc)Service : BaseService, I@(entityNamePc)Service, IDynamicApi
    {
        private I@(entityNamePc)Repository _@(entityNameCc)Rep;

        public @(entityNamePc)Service(I@(entityNamePc)Repository @(entityNameCc)Rep)
        {
            _@(entityNameCc)Rep = @(entityNameCc)Rep;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<@(entityNamePc)Dto> GetAsync(long id)
        {
            var output = await _@(entityNameCc)Rep.GetAsync(id);
            return output.Adapt<@(entityNamePc)Dto>();
        }
        
@if(gen.GenGetList){
        @:/// <summary>
        @:/// 列表查询
        @:/// </summary>
        @:/// <param name="input"></param>
        @:/// <returns></returns>
        @:[HttpPost]
        @:public async Task<IEnumerable<@(entityNamePc)GetListDto>> GetListAsync(@(entityNamePc)QueryInput input)
        @:{
        @:    var list = await _@(entityNameCc)Rep.AsQueryable()
            @foreach(var col in gen.Fields.Where(w=>w.WhetherQuery)){
                if(col.IsTextColumn()){
                @:.WhereIF(!string.IsNullOrEmpty(input.@(col.ColumnName.NamingPascalCase())), a=>a.@(col.ColumnName.NamingPascalCase()) == input.@(col.ColumnName.NamingPascalCase()))
                }else{
                @:.WhereIF(input.@(col.ColumnName.NamingPascalCase()) != null, a=>a.@(col.ColumnName.NamingPascalCase()) == input.@(col.ColumnName.NamingPascalCase()))
                }
            }
         @:       .OrderByDescending(a => a.Id)
         @:       .Select<@(entityNamePc)GetListDto>()
         @:       .ToListAsync();

            @if(gen.Fields.Any(a=>!string.IsNullOrWhiteSpace(a.DictTypeCode))) {

                            var usedDicCols = gen.Fields.Where(w => !string.IsNullOrWhiteSpace(w.DictTypeCode));

            @:var dictRepo = LazyGetRequiredService<IDictRepository>();
            @:var dictList = await dictRepo.Where(w => new string[] { @(string.Concat("\"" , string.Join("\", \"", usedDicCols.Select(s=>s.DictTypeCode)), "\"")) }
            @:    .Contains(w.DictType.Code)).ToListAsync();


            @:return list.Select(s =>
            @:{
                            foreach(var col in usedDicCols)
                            {

            @:    s.@(col.ColumnName.NamingPascalCase())DictName = dictList.FirstOrDefault(f => f.DictType.Code == "@(col.DictTypeCode)" && f.Value == @if(col.IsNumColumn())@("\"\" + ")s.@(col.ColumnName.NamingPascalCase()))?.Name;
                                
                            }
            @:   return s;
            @:});

            }else
            {
            @:return list;
            }
        @:}
}
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageOutput<@(entityNamePc)GetListDto>> GetPageAsync(PageInput<@(entityNamePc)QueryInput> input)
        {
            var filter = input.Filter;
            var dynamicCondition = ChangeConditon(input.DynamicFilter);
            var list = await _@(entityNameCc)Rep.GetQueryable(dynamicCondition)
            @foreach(var col in gen.Fields.Where(w=>w.WhetherQuery)){
                if(col.IsTextColumn()){
                    if(col.IsTextQueryContains()){
                @:.WhereIF(filter !=null && !string.IsNullOrEmpty(filter.@(col.ColumnName.NamingPascalCase())), a=> a.@(col.ColumnName.NamingPascalCase()) != null && a.@(col.ColumnName.NamingPascalCase()).Contains(filter.@(col.ColumnName.NamingPascalCase())))
                    }else{
                @:.WhereIF(filter !=null && !string.IsNullOrEmpty(filter.@(col.ColumnName.NamingPascalCase())), a=>a.@(col.ColumnName.NamingPascalCase()) == filter.@(col.ColumnName.NamingPascalCase()))
                    }

                }else if(col.IsNumColumn()){
                @:.WhereIF(filter !=null && filter.@(col.ColumnName.NamingPascalCase()) != null, a=>a.@(col.ColumnName.NamingPascalCase()) == filter.@(col.ColumnName.NamingPascalCase()))
                }
            }
                .OrderBy(c => c.Sort)
                .Select<@(entityNamePc)GetListDto>()
                .ToPagedListAsync(input.CurrentPage, input.PageSize);
        
            @if(gen.Fields.Any(a=>!string.IsNullOrWhiteSpace(a.DictTypeCode))) {

                            var usedDicCols = gen.Fields.Where(w => !string.IsNullOrWhiteSpace(w.DictTypeCode));

            @:var dictRepo = LazyGetRequiredService<IDictRepository>();
            @:var dictList = await dictRepo.Where(w => new string[] { @(string.Concat("\"" , string.Join("\", \"", usedDicCols.Select(s=>s.DictTypeCode)), "\"")) }
            @:    .Contains(w.DictType.Code)).ToListAsync();

            @:
            @:var retList = list.Items.Select(s =>
            @:{
                            foreach(var col in usedDicCols)
                            {

            @:    s.@(col.ColumnName.NamingPascalCase())DictName = dictList.FirstOrDefault(f => f.DictType.Code == "@(col.DictTypeCode)" && f.Value == @if(col.IsNumColumn())@("\"\" + ")s.@(col.ColumnName.NamingPascalCase()))?.Name;
                                
                            }
            @:
            @:   return s;
            @:}).ToList();

            }

            //关联查询代码
            @foreach (var col in gen.Fields.Where(w=>w.IsIncludeColumn()&&!string.IsNullOrWhiteSpace(w.IncludeEntityKey))){
                if(col.IncludeMode==0){
            @://数据转换-单个关联
            @:var @(col.ColumnName.NamingCamelCase())Rows = retList.Where(s => s.@(col.ColumnName) > 0).ToList();
            @:if (@(col.ColumnName.NamingCamelCase())Rows.Any())
            @:{
            @:    var @(col.ColumnName.NamingCamelCase())Repo = LazyGetRequiredService<Domain.@(col.IncludeEntity.Replace("Entity", "")).I@(col.IncludeEntity.Replace("Entity", ""))Repository>();
            @:    var @(col.ColumnName.NamingCamelCase())RowsIds = @(col.ColumnName.NamingCamelCase())Rows.Select(s => s.@(col.ColumnName)).Distinct().ToList();
            @:    var @(col.ColumnName.NamingCamelCase())RowsIdsData = await @(col.ColumnName.NamingCamelCase())Repo.Where(s => @(col.ColumnName.NamingCamelCase())RowsIds.Contains(s.Id)).ToListAsync(s => new { s.Id, s.@(col.IncludeEntityKey) });
            @:    @(col.ColumnName.NamingCamelCase())Rows.ForEach(s =>
            @:    {
            @:        s.@(col.ColumnName)_Text = @(col.ColumnName.NamingCamelCase())RowsIdsData.FirstOrDefault(s2 => s2.Id == s.@(col.ColumnName))?.@(col.IncludeEntityKey);
            @:    });
            @:}
            }else if(col.IncludeMode==1){
                
            @://数据转换-多个关联
            @:var @(col.ColumnName.NamingCamelCase())Rows = retList.Where(s => s.@(col.ColumnName)_Values != null && s.@(col.ColumnName)_Values.Any()).ToList();
            @:if (@(col.ColumnName.NamingCamelCase())Rows.Any())
            @:{
            @:    var @(col.ColumnName.NamingCamelCase())Repo = LazyGetRequiredService<Domain.@(col.IncludeEntity.Replace("Entity", "")).I@(col.IncludeEntity.Replace("Entity", ""))Repository>();
            @:    var @(col.ColumnName.NamingCamelCase())RowsIds =@(col.ColumnName.NamingCamelCase())Rows.SelectMany(s => s.@(col.ColumnName)_Values).Select(s => long.TryParse(s, out long s2) ? s2 : 0).Distinct().ToList();
            @:    var @(col.ColumnName.NamingCamelCase())RowsIdsData = await @(col.ColumnName.NamingCamelCase())Repo.Where(s => @(col.ColumnName.NamingCamelCase())RowsIds.Contains(s.Id)).ToListAsync(s => new { s.Id, s.@(col.IncludeEntityKey) });
            @:    @(col.ColumnName.NamingCamelCase())Rows.ForEach(s =>
            @:    {
            @:        s.@(col.ColumnName)_Texts = @(col.ColumnName.NamingCamelCase())RowsIdsData.Where(s2 => s.@(col.ColumnName)_Values.Contains(s2.Id.ToString())).OrderBy(s2 => s.@(col.ColumnName)_Values.IndexOf(s2.Id.ToString())).Select(s2 => s2.@(col.IncludeEntityKey)).ToList();
            @:    });
            @:}
            }

            }

            var data = new PageOutput<@(entityNamePc)GetListDto> { List = list.Items.ToList(), Total = list.Total };
        
            return data;
        }
        

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<long> AddAsync(@(entityNamePc)Dto input)
        {
            var entity = Mapper.Map<@(entityNamePc)Entity>(input);
            if (entity.Sort == 0)
            {
                var sort = await  _@(entityNameCc)Rep.AsQueryable().MaxAsync(a => a.Sort);
                entity.Sort = sort + 1;
            }
            var id = await _@(entityNameCc)Rep.InsertReturnSnowflakeIdAsync(entity);
            return id;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateAsync(@(entityNamePc)Dto input)
        {
            var entity = await _@(entityNameCc)Rep.GetAsync(input.Id);
            if (!(entity?.Id > 0))
                throw ResultOutput.Exception("@(gen.BusName)不存在！");

            Mapper.Map(input, entity);
            await _@(entityNameCc)Rep.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteAsync(long id)
        {
            return await _@(entityNameCc)Rep.SetColumnUpdateable(a => a.IsDeleted == true).Where(a => a.Id == id).ExecuteCommandAsync() > 0;
        }


@if(gen.GenBatchDelete){
        @:/// <summary>
        @:/// 批量删除
        @:/// </summary>
        @:/// <param name="ids"></param>
        @:/// <returns></returns>
        @:[HttpPut]
        @:public async Task<bool> BatchDeleteAsync(long[] ids)
        @:{
        @:    return await _@(entityNameCc)Rep.SetColumnUpdateable(a => a.IsDeleted == true).Where(a => ids.Contains(a.UserId)).ExecuteCommandAsync() > 0;
        @:}
}

@if(gen.GenSoftDelete){
        @:/// <summary>
        @:/// 软删除
        @:/// </summary>
        @:/// <param name="id"></param>
        @:/// <returns></returns>
        @:[HttpDelete]
        @:public async Task<bool> SoftDeleteAsync(long id)
        @:{
        @:    return await _@(entityNameCc)Rep.AsUpdateable().SetColumns(a => a.IsDeleted == true).Where(a => a.Id == id).ExecuteCommandAsync() > 0;
        @:}
}

@if (gen.GenBatchSoftDelete)
{
        @:/// <summary>
        @:/// 批量软删除
        @:/// </summary>
        @:/// <param name="ids"></param>
        @:/// <returns></returns>
        @:[HttpPut]
        @:public async Task<bool> BatchSoftDeleteAsync(long[] ids)
        @:{
        @:    return await _@(entityNameCc)Rep.AsUpdateable().SetColumns(a => a.IsDeleted == true).Where(a => ids.Contains(a.UserId)).ExecuteCommandAsync() > 0;
        @:}

}
    }
}