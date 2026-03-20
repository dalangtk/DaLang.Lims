using DaLang.Lims.BaseData.Contracts.Instrument;
using DaLang.Lims.BaseData.Contracts.Instrument.Dto;
using DaLang.Lims.BaseData.Domain.Instrument;
using DaLang.Lims.Web.BaseData.Core.Consts;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;


namespace DaLang.Lims.Web.BaseData.Services.BaseInstrument
{
    /// <summary>
    /// 仪器服务
    /// </summary>
    [DynamicApi(Area = BaseDataConsts.AreaName)]
    public class BaseInstrumentService : BaseService, IBaseInstrumentService, IDynamicApi
    {
        private IBaseInstrumentRepository _baseInstrumentRep;

        public BaseInstrumentService(IBaseInstrumentRepository baseInstrumentRep)
        {
            _baseInstrumentRep = baseInstrumentRep;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseInstrumentDto> GetAsync(long id)
        {
            var output = await _baseInstrumentRep.GetAsync(id);
            return output.Adapt<BaseInstrumentDto>();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageOutput<BaseInstrumentGetListDto>> GetPageAsync(PageInput<BaseInstrumentQueryInput> input)
        {
            var filter = input.Filter;
            var dynamicCondition = ChangeConditon(input.DynamicFilter);
            var list = await _baseInstrumentRep.GetQueryable(dynamicCondition)
                .WhereIF(!string.IsNullOrWhiteSpace(filter.InstrumentCode), a => a.InstrumentCode.Contains(filter.InstrumentCode))
                .OrderBy(c => c.Sort)
                .Select<BaseInstrumentGetListDto>()
                .ToPagedListAsync(input.CurrentPage, input.PageSize);

            var data = new PageOutput<BaseInstrumentGetListDto> { List = list.Items.ToList(), Total = list.Total };

            return data;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<long> AddAsync(BaseInstrumentDto input)
        {
            if (string.IsNullOrWhiteSpace(input?.InstrumentCode))
                throw ResultOutput.Exception("仪器代码不可为空");
            input.InstrumentCode = input.InstrumentCode.ToUpper().Trim();
            var isExists = await _baseInstrumentRep.IsAnyAsync(a => a.InstrumentCode == input.InstrumentCode);
            if (isExists)
                throw ResultOutput.Exception($"仪器代码{input.InstrumentCode}已存在！");

            var entity = Mapper.Map<BaseInstrumentEntity>(input);
            if (entity.Sort == 0)
            {
                var sort = await _baseInstrumentRep.AsQueryable().MaxAsync(a => a.Sort);
                entity.Sort = sort + 1;
            }
            var id = await _baseInstrumentRep.InsertReturnSnowflakeIdAsync(entity);
            return id;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateAsync(BaseInstrumentDto input)
        {
            var entity = await _baseInstrumentRep.GetAsync(input.Id);
            if (!(entity?.Id > 0))
                throw ResultOutput.Exception("仪器不存在！");

            Mapper.Map(input, entity);
            await _baseInstrumentRep.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteAsync(long id)
        {
            return await _baseInstrumentRep.AsUpdateable().SetColumns(a => a.IsDeleted == true).Where(a => a.Id == id).ExecuteCommandAsync() > 0;
        }
    }
}