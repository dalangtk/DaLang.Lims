using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Dev.Core.Consts;
using DaLang.Lims.Web.Dev.Domain.CodeGroupDetail;
using DaLang.Lims.Web.Dev.Services.CodeGroupDetail.Dto;


namespace DaLang.Lims.Web.Dev.Services.CodeGroupDetail
{
    /// <summary>
    /// 模板明细服务
    /// </summary>
    [DynamicApi(Area = DevConsts.AreaName)]
    public class CodeGroupDetailService : BaseService, ICodeGroupDetailService, IDynamicApi
    {
        private ICodeGroupDetailRepository _codeGroupDetailRepository => LazyGetRequiredService<ICodeGroupDetailRepository>();

        public CodeGroupDetailService()
        {
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CodeGroupDetailGetOutput> GetAsync(long id)
        {
            var output = await _codeGroupDetailRepository.GetByIdAsync(id);
            return output.Adapt<CodeGroupDetailGetOutput>();
        }

        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IEnumerable<CodeGroupDetailGetListOutput>> GetListAsync(CodeGroupDetailGetListInput input)
        {
            var list = await _codeGroupDetailRepository.AsQueryable()
                .WhereIF(!string.IsNullOrEmpty(input.Name), a => a.Name == input.Name)
                .WhereIF(input.GroupId != null, a => a.GroupId == input.GroupId)
                .OrderByDescending(a => a.Id)
                .Select<CodeGroupDetailGetListOutput>()
                .ToListAsync();
            return list;
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageOutput<CodeGroupDetailGetPageOutput>> GetPageAsync(PageInput<CodeGroupDetailGetPageInput> input)
        {
            var filter = input.Filter;
            var conditionStr = ChangeConditon(input.DynamicFilter);
            var list = await _codeGroupDetailRepository.GetQueryable(conditionStr)
                .WhereIF(filter != null && !string.IsNullOrEmpty(filter.Name), a => a.Name != null && a.Name.Contains(filter.Name))
                .WhereIF(filter != null && filter.GroupId != null, a => a.GroupId == filter.GroupId)
                .Select<CodeGroupDetailGetPageOutput>()
                .OrderByDescending(c => c.Id)
                .ToPagedListAsync(input.CurrentPage, input.PageSize);

            //关联查询代码
            //数据转换-单个关联
            var groupIdRows = list.Items.Where(s => s.GroupId > 0).ToList();
            if (groupIdRows.Any())
            {
                var groupIdRepo = LazyGetRequiredService<Domain.CodeGroup.ICodeGroupRepository>();
                var groupIdRowsIds = groupIdRows.Select(s => s.GroupId).Distinct().ToList();
                var groupIdRowsIdsData = await groupIdRepo.AsQueryable().Where(s => groupIdRowsIds.Contains(s.Id)).ToListAsync(s => new { s.Id, s.Name });
                groupIdRows.ForEach(s =>
                {
                    s.GroupId_Text = groupIdRowsIdsData.FirstOrDefault(s2 => s2.Id == s.GroupId)?.Name;
                });
            }
            //数据转换-多个关联
            var groupIdsRows = list.Items.Where(s => s.GroupIds_Values != null && s.GroupIds_Values.Any()).ToList();
            if (groupIdsRows.Any())
            {
                var groupIdsRepo = LazyGetRequiredService<Domain.CodeGroup.ICodeGroupRepository>();
                var groupIdsRowsIds = groupIdsRows.SelectMany(s => s.GroupIds_Values).Select(s => long.TryParse(s, out long s2) ? s2 : 0).Distinct().ToList();
                var groupIdsRowsIdsData = await groupIdsRepo.AsQueryable().Where(s => groupIdsRowsIds.Contains(s.Id)).ToListAsync(s => new { s.Id, s.Name });
                groupIdsRows.ForEach(s =>
                {
                    s.GroupIds_Texts = groupIdsRowsIdsData.Where(s2 => s.GroupIds_Values.Contains(s2.Id.ToString())).OrderBy(s2 => s.GroupIds_Values.IndexOf(s2.Id.ToString())).Select(s2 => s2.Name).ToList();
                });
            }

            var data = new PageOutput<CodeGroupDetailGetPageOutput> { List = list.Items.ToList(), Total = list.Total };

            return data;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<long> AddAsync(CodeGroupDetailAddInput input)
        {
            var entity = Mapper.Map<CodeGroupDetailEntity>(input);
            var id = await _codeGroupDetailRepository.InsertReturnSnowflakeIdAsync(entity);

            return id;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateAsync(CodeGroupDetailUpdateInput input)
        {
            var entity = await _codeGroupDetailRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                throw ResultOutput.Exception("模板明细不存在！");
            }

            Mapper.Map(input, entity);
            await _codeGroupDetailRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteAsync(long id)
        {
            return await _codeGroupDetailRepository.UpdateSetColumnsTrueAsync(a => new CodeGroupDetailEntity { IsDeleted = true }, a => a.Id == id);
        }



        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> SoftDeleteAsync(long id)
        {
            return await _codeGroupDetailRepository.UpdateSetColumnsTrueAsync(a => new CodeGroupDetailEntity { IsDeleted = true }, a => a.Id == id);
        }

        /// <summary>
        /// 批量软删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<bool> BatchSoftDeleteAsync(long[] ids)
        {
            return await _codeGroupDetailRepository.UpdateSetColumnsTrueAsync(a => new CodeGroupDetailEntity { IsDeleted = true }, a => ids.Contains(a.Id));
        }
    }
}