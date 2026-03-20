using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain.Dict;
using DaLang.Lims.Web.Framework.Services;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Dev.Core.Consts;
using DaLang.Lims.Web.Dev.Domain.CodeGroupDemo;
using DaLang.Lims.Web.Dev.Services.CodeGroupDemo.Dto;


namespace DaLang.Lims.Web.Dev.Services.CodeGroupDemo
{
    /// <summary>
    /// 模板演示服务
    /// </summary>
    [DynamicApi(Area = DevConsts.AreaName)]
    public class CodeGroupDemoService : BaseService, ICodeGroupDemoService, IDynamicApi
    {
        private ICodeGroupDemoRepository _codeGroupDemoRepository => LazyGetRequiredService<ICodeGroupDemoRepository>();

        public CodeGroupDemoService()
        {
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CodeGroupDemoGetOutput> GetAsync(long id)
        {
            var output = await _codeGroupDemoRepository.GetByIdAsync(id);
            return output.Adapt<CodeGroupDemoGetOutput>();
        }

        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<CodeGroupDemoGetListOutput>> GetListAsync(CodeGroupDemoGetListInput input)
        {
            var list = await _codeGroupDemoRepository.AsQueryable()
                .WhereIF(!string.IsNullOrEmpty(input.InputText), a => a.InputText == input.InputText)
                .WhereIF(input.InputNumber != null, a => a.InputNumber == input.InputNumber)
                .WhereIF(input.InputDate != null, a => a.InputDate == input.InputDate)
                .WhereIF(input.InputSwitch != null, a => a.InputSwitch == input.InputSwitch)
                .WhereIF(!string.IsNullOrEmpty(input.InputSelectCustom), a => a.InputSelectCustom == input.InputSelectCustom)
                .WhereIF(!string.IsNullOrEmpty(input.InputSelectDict), a => a.InputSelectDict == input.InputSelectDict)
                .WhereIF(input.InputBussinessSingle != null, a => a.InputBussinessSingle == input.InputBussinessSingle)
                .OrderByDescending(a => a.Id)
                .Select<CodeGroupDemoGetListOutput>()
                .ToListAsync();
            var dictRepo = LazyGetRequiredService<IDictRepository>();
            var dictList = await dictRepo.AsQueryable().Where(w => new string[] { "sex" }
                .Contains(w.DictType.Code)).ToListAsync();
            return list.Select(s =>
            {
                s.InputSelectDictDictName = dictList.FirstOrDefault(f => f.DictType.Code == "sex" && f.Value == s.InputSelectDict)?.Name;
                return s;
            });
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageOutput<CodeGroupDemoGetPageOutput>> GetPageAsync(PageInput<CodeGroupDemoGetPageInput> input)
        {
            var filter = input.Filter;
            var conditionStr = ChangeConditon(input.DynamicFilter);
            var list = await _codeGroupDemoRepository.GetQueryable(conditionStr)
                .WhereIF(filter != null && !string.IsNullOrEmpty(filter.InputText), a => a.InputText != null && a.InputText.Contains(filter.InputText))
                .WhereIF(filter != null && filter.InputNumber != null, a => a.InputNumber == filter.InputNumber)
                .WhereIF(filter != null && !string.IsNullOrEmpty(filter.InputSelectCustom), a => a.InputSelectCustom == filter.InputSelectCustom)
                .WhereIF(filter != null && !string.IsNullOrEmpty(filter.InputSelectDict), a => a.InputSelectDict == filter.InputSelectDict)
                .WhereIF(filter != null && filter.InputBussinessSingle != null, a => a.InputBussinessSingle == filter.InputBussinessSingle)
                .OrderByDescending(c => c.Id)
                .Select<CodeGroupDemoGetPageOutput>()
                .ToPagedListAsync(input.CurrentPage, input.PageSize);

            //.Count(out var total)
            //.Page(input.CurrentPage, input.PageSize)
            //.ToListAsync<CodeGroupDemoGetPageOutput>();

            var dictRepo = LazyGetRequiredService<IDictRepository>();
            var dictList = await dictRepo.AsQueryable().Where(w => new string[] { "sex" }
                .Contains(w.DictType.Code)).ToListAsync();

            var tmpList = list.Items.Select(s =>
            {
                s.InputSelectDictDictName = dictList.FirstOrDefault(f => f.DictType.Code == "sex" && f.Value == s.InputSelectDict)?.Name;

                return s;
            }).ToList();

            //关联查询代码
            //数据转换-单个关联
            var inputBussinessSingleRows = tmpList.Where(s => s.InputBussinessSingle > 0).ToList();
            if (inputBussinessSingleRows.Any())
            {
                var inputBussinessSingleRepo = LazyGetRequiredService<Domain.CodeGroup.ICodeGroupRepository>();
                var inputBussinessSingleRowsIds = inputBussinessSingleRows.Select(s => s.InputBussinessSingle).Distinct().ToList();
                var inputBussinessSingleRowsIdsData = await inputBussinessSingleRepo.AsQueryable().Where(s => inputBussinessSingleRowsIds.Contains(s.Id)).ToListAsync(s => new { s.Id, s.Name });
                inputBussinessSingleRows.ForEach(s =>
                {
                    s.InputBussinessSingle_Text = inputBussinessSingleRowsIdsData.FirstOrDefault(s2 => s2.Id == s.InputBussinessSingle)?.Name;
                });
            }
            //数据转换-多个关联
            var inputBussinessMultipleRows = tmpList.Where(s => s.InputBussinessMultiple_Values != null && s.InputBussinessMultiple_Values.Any()).ToList();
            if (inputBussinessMultipleRows.Any())
            {
                var inputBussinessMultipleRepo = LazyGetRequiredService<Domain.CodeGroup.ICodeGroupRepository>();
                var inputBussinessMultipleRowsIds = inputBussinessMultipleRows.SelectMany(s => s.InputBussinessMultiple_Values).Select(s => long.TryParse(s, out long s2) ? s2 : 0).Distinct().ToList();
                var inputBussinessMultipleRowsIdsData = await inputBussinessMultipleRepo.AsQueryable().Where(s => inputBussinessMultipleRowsIds.Contains(s.Id)).ToListAsync(s => new { s.Id, s.Name });
                inputBussinessMultipleRows.ForEach(s =>
                {
                    s.InputBussinessMultiple_Texts = inputBussinessMultipleRowsIdsData.Where(s2 => s.InputBussinessMultiple_Values.Contains(s2.Id.ToString())).OrderBy(s2 => s.InputBussinessMultiple_Values.IndexOf(s2.Id.ToString())).Select(s2 => s2.Name).ToList();
                });
            }

            var data = new PageOutput<CodeGroupDemoGetPageOutput> { List = tmpList, Total = list.Total };

            return data;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<long> AddAsync(CodeGroupDemoAddInput input)
        {
            var entity = Mapper.Map<CodeGroupDemoEntity>(input);
            var id = await _codeGroupDemoRepository.InsertReturnSnowflakeIdAsync(entity);

            return id;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateAsync(CodeGroupDemoUpdateInput input)
        {
            var entity = await _codeGroupDemoRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                throw ResultOutput.Exception("模板演示不存在！");
            }

            Mapper.Map(input, entity);
            await _codeGroupDemoRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteAsync(long id)
        {
            return await _codeGroupDemoRepository.AsUpdateable().SetColumns(a => a.IsDeleted).Where(a => a.Id == id).ExecuteCommandAsync() > 0;
        }



        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> SoftDeleteAsync(long id)
        {
            return await _codeGroupDemoRepository.AsUpdateable().SetColumns(a => a.IsDeleted).Where(a => a.Id == id).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 批量软删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<bool> BatchSoftDeleteAsync(long[] ids)
        {
            return await _codeGroupDemoRepository.AsUpdateable().SetColumns(a => a.IsDeleted).Where(a => ids.Contains(a.Id)).ExecuteCommandAsync() > 0;
        }
    }
}