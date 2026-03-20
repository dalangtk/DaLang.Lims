using DaLang.Lims.BaseData.Contracts.Combo;
using DaLang.Lims.BaseData.Contracts.Combo.Dto;
using DaLang.Lims.BaseData.Domain.Combo;
using DaLang.Lims.BaseData.Domain.Purpose;
using DaLang.Lims.BaseData.Domain.SampleType;
using DaLang.Lims.Web.BaseData.Core.Consts;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;


namespace DaLang.Lims.BaseData.Services.Combo;

/// <summary>
/// 套餐管理服务
/// </summary>
[DynamicApi(Area = BaseDataConsts.AreaName)]
public class BaseComboService : BaseService, IBaseComboService, IDynamicApi
{
    private IBaseComboRepository _baseComboRep;
    private IBaseComboDetailRepository _baseComboDetailRep;
    private IBasePurposeRepository _basePurposeRep;

    public BaseComboService(IBaseComboRepository baseComboRep,
        IBaseComboDetailRepository baseComboDetailRep,
        IBasePurposeRepository basePurposeRep)
    {
        _baseComboRep = baseComboRep;
        _baseComboDetailRep = baseComboDetailRep;
        _basePurposeRep = basePurposeRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseComboDto> GetAsync(long id)
    {
        var output = await _baseComboRep.GetAsync(id);
        return output.Adapt<BaseComboDto>();
    }

    /// <summary>
    /// 列表查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IEnumerable<BaseComboDto>> GetListAsync(BaseComboQueryInput input)
    {
        var list = await _baseComboRep.AsQueryable()
            .OrderByDescending(a => a.Id)
            .Select<BaseComboDto>()
            .ToListAsync();
        return list;
    }
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BaseComboDto>> GetPageAsync(PageInput<BaseComboQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _baseComboRep.GetQueryable(dynamicCondition)
            .OrderBy(c => c.Sort)
            .Select<BaseComboDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = new PageOutput<BaseComboDto> { List = list.Items.ToList(), Total = list.Total };

        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AdminTransaction]
    public async Task<long> AddAsync(ComboAddOrUpdateInput input)
    {
        var entity = Mapper.Map<BaseComboEntity>(input.Combo);
        if (entity.Sort == 0)
        {
            var sort = await _baseComboRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }

        var maxComboCode = await _baseComboRep.AsQueryable().MaxAsync(a => a.ComboCode);
        if (string.IsNullOrWhiteSpace(maxComboCode))
            entity.ComboCode = "TC" + "1".PadLeft(4, '0');
        else
            entity.ComboCode = "TC" + (int.Parse(maxComboCode.Replace("TC", "")) + 1).ToString().PadLeft(4, '0');

        var purCodes = input.ComboDetail;

        var purs = await _basePurposeRep.AsQueryable()
             .InnerJoin<BasePurposeTenantSettingEntity>((a, b) => a.PurCode == b.PurCode && b.IsEnable == true)
             .Where((a, b) => purCodes.Contains(a.PurCode))
             .Select((a, b) => new
             {
                 a.PurCode,
                 a.PurName
             }).ToListAsync();

        var id = await _baseComboRep.InsertReturnSnowflakeIdAsync(entity);
        var comboDetails = new List<BaseComboDetailAddInput>();
        purs.ForEach(v =>
        {
            comboDetails.Add(new BaseComboDetailAddInput
            {
                ComboCode = entity.ComboCode,
                PurCode = v.PurCode,
                PurName = v.PurName,
                IsValid = true
            });
        });
        await _baseComboDetailRep.InsertRangeAsync(comboDetails.Adapt<List<BaseComboDetailEntity>>());
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(ComboAddOrUpdateInput input)
    {
        var entity = await _baseComboRep.GetAsync(input.Combo.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("套餐不存在！");

        Mapper.Map(input.Combo, entity);

        var comboDetail = await _baseComboDetailRep.GetListAsync(v => v.ComboCode == entity.ComboCode);
        List<long> delIds = new List<long>();
        foreach (var detail in comboDetail)
        {
            if (!input.ComboDetail.Exists(v => v == detail.PurCode))
                delIds.Add(detail.Id);
        }

        var newPurCodes = input.ComboDetail.Except(comboDetail.Select(v => v.PurCode).Distinct());
        if (newPurCodes.Any())
        {
            var purs = await _basePurposeRep.AsQueryable()
              .InnerJoin<BasePurposeTenantSettingEntity>((a, b) => a.PurCode == b.PurCode && b.IsEnable == true)
              .Where((a, b) => newPurCodes.Contains(a.PurCode))
              .Select((a, b) => new
              {
                  a.PurCode,
                  a.PurName
              }).ToListAsync();

            var comboDetails = new List<BaseComboDetailAddInput>();
            purs.ForEach(v =>
            {
                comboDetails.Add(new BaseComboDetailAddInput
                {
                    ComboCode = entity.ComboCode,
                    PurCode = v.PurCode,
                    PurName = v.PurName,
                    IsValid = true
                });
            });
            await _baseComboDetailRep.InsertRangeAsync(comboDetails.Adapt<List<BaseComboDetailEntity>>());
        }

        if (delIds.Any())
        {
            await _baseComboDetailRep
                .SetColumnUpdateable(a => a.IsDeleted == true)
                .Where(a => delIds.Contains(a.Id))
                .ExecuteCommandAsync();
        }

        await _baseComboRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _baseComboRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
    /// <summary>
    /// 获取套餐明细
    /// </summary>
    /// <param name="comboCode"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<BaseComboDetailDto>> GetComboDetail(string comboCode)
    {
        return await _baseComboDetailRep.AsQueryable()
            .InnerJoin<BasePurposeEntity>((a, b) => a.PurCode == b.PurCode)
            .Where(a => a.ComboCode == comboCode && a.IsDeleted == false)
            .Select((a, b) => new BaseComboDetailDto
            {
                Id = a.Id,
                PurCode = a.PurCode,
                PurName = a.PurName,
                GroupName = b.GroupName
            })
            .ToListAsync();
    }

    /// <summary>
    /// 获取套餐及对应的目的代码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<BaseComboWithPurcodesDto>> GetComboWithPurcodesListAsync(BaseComboQueryInput input)
    {
        var ret = await _baseComboRep.AsQueryable()
            .LeftJoin<BaseSampleTypeEntity>((a, b) => a.SampleTypeCode == b.SampleTypeCode && b.IsValid && !b.IsDeleted)
              .WhereIF(!string.IsNullOrWhiteSpace(input.ComboCode), a => a.ComboName.Contains(input.ComboCode!))
              .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerCode), a => a.CustomerCode == input.CustomerCode!)
              .WhereIF(string.IsNullOrWhiteSpace(input.CustomerCode), a => string.IsNullOrWhiteSpace(a.CustomerCode))
              .Select((a, b) => new BaseComboWithPurcodesDto
              {
                  ComboName = a.ComboName,
                  SampleTypeName = b.SampleTypeName,
                  PurposeList = SqlFunc.Subqueryable<BaseComboDetailEntity>().Where(z => z.ComboCode == a.ComboCode && z.IsValid && !z.IsDeleted).ToList(z => z.PurCode)
              }, true)
              .ToListAsync();
        return ret;
    }
}