using Mapster;
using Microsoft.AspNetCore.Mvc;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Dev.Core.Consts;
using DaLang.Lims.Web.Dev.Domain.CodeGroup;
using DaLang.Lims.Web.Dev.Services.CodeGroup.Dto;


namespace DaLang.Lims.Web.Dev.Services.CodeGroup
{
    /// <summary>
    /// 模板组服务
    /// </summary>
    [DynamicApi(Area = DevConsts.AreaName)]
    public class CodeGroupService : BaseService, ICodeGroupService, IDynamicApi
    {
        private ICodeGroupRepository _codeGroupRepository => LazyGetRequiredService<ICodeGroupRepository>();

        public CodeGroupService()
        {
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CodeGroupGetOutput> GetAsync(long id)
        {
            var output = await _codeGroupRepository.GetByIdAsync(id);
            return output.Adapt<CodeGroupGetOutput>();
        }

        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<CodeGroupGetListOutput>> GetListAsync(CodeGroupGetListInput input)
        {
            var list = await _codeGroupRepository.AsQueryable()
                .WhereIF(!string.IsNullOrEmpty(input.Name), a => a.Name == input.Name)
                .OrderByDescending(a => a.Id)
                .Select<CodeGroupGetListOutput>()
                .ToListAsync();
            return list;
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageOutput<CodeGroupGetPageOutput>> GetPageAsync(PageInput<CodeGroupGetPageInput> input)
        {
            var filter = input.Filter;
            var conditionStr = ChangeConditon(input.DynamicFilter);

            var list = await _codeGroupRepository.GetQueryable(conditionStr)
                .WhereIF(filter != null && !string.IsNullOrEmpty(filter.Name), a => a.Name != null && a.Name.Contains(filter.Name))

                .OrderByDescending(c => c.Id)

                .Select<CodeGroupGetPageOutput>()
                .ToPagedListAsync(input.CurrentPage, input.PageSize);

            //关联查询代码
            var data = new PageOutput<CodeGroupGetPageOutput> { List = list.Items.ToList(), Total = list.Total };

            return data;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<long> AddAsync(CodeGroupAddInput input)
        {
            var entity = Mapper.Map<CodeGroupEntity>(input);
            var id = await _codeGroupRepository.InsertReturnSnowflakeIdAsync(entity);

            return id;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateAsync(CodeGroupUpdateInput input)
        {
            var entity = await _codeGroupRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                throw ResultOutput.Exception("模板组不存在！");
            }

            Mapper.Map(input, entity);
            await _codeGroupRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteAsync(long id)
        {
            return await _codeGroupRepository.UpdateSetColumnsTrueAsync(a => new CodeGroupEntity { IsDeleted = true }, a => a.Id == id);
        }



        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> SoftDeleteAsync(long id)
        {
            return await _codeGroupRepository.UpdateSetColumnsTrueAsync(a => new CodeGroupEntity { IsDeleted = true }, a => a.Id == id);
        }

    }
}