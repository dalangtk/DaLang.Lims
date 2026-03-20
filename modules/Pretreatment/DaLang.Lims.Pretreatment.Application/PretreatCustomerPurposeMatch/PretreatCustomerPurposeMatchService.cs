using DaLang.Lims.BaseData.Contracts.BasePurpose;
using DaLang.Lims.BaseData.Domain.Combo;
using DaLang.Lims.Pretreatment.Contracts.PretreatCustomerPurposeMatch;
using DaLang.Lims.Pretreatment.Contracts.PretreatCustomerPurposeMatch.Dto;
using DaLang.Lims.Pretreatment.Core.Consts;
using DaLang.Lims.Pretreatment.Domain.PretreatCustomerPurposeMatch;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace DaLang.Lims.Pretreatment.Services.PretreatCustomerPurposeMatch;

/// <summary>
/// 目的对照服务
/// </summary>
[DynamicApi(Area = PretreatmentConsts.AreaName)]
public class PretreatCustomerPurposeMatchService : BaseService, IPretreatCustomerPurposeMatchService, IDynamicApi
{
    private IPretreatCustomerPurposeMatchRepository _pretreatCustomerPurposeMatchRep;
    private IBasePurposeService _purposeService;
    private IBaseComboRepository _comboRep;

    public PretreatCustomerPurposeMatchService(IPretreatCustomerPurposeMatchRepository pretreatCustomerPurposeMatchRep,
        IBasePurposeService purposeService,
         IBaseComboRepository comboRep)
    {
        _pretreatCustomerPurposeMatchRep = pretreatCustomerPurposeMatchRep;
        _purposeService = purposeService;
        _comboRep = comboRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PurposeMatchDto> GetAsync(long id)
    {
        var output = await _pretreatCustomerPurposeMatchRep.GetAsync(id);
        return output.Adapt<PurposeMatchDto>();
    }
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<PurposeMatchGetListDto>> GetPageAsync(PageInput<PurposeMatchQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _pretreatCustomerPurposeMatchRep.GetQueryable(dynamicCondition)
            .OrderBy(c => c.Sort)
            .Select<PurposeMatchGetListDto>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = new PageOutput<PurposeMatchGetListDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(PurposeMatchDto input)
    {
        var entity = Mapper.Map<PretreatCustomerPurposeMatchEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _pretreatCustomerPurposeMatchRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        var id = await _pretreatCustomerPurposeMatchRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task UpdateAsync(PurposeMatchDto input)
    {
        var entity = await _pretreatCustomerPurposeMatchRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("目的对照不存在！");

        Mapper.Map(input, entity);
        await _pretreatCustomerPurposeMatchRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _pretreatCustomerPurposeMatchRep
            .SetUpdateable()
            .SetColumns(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
    /// <summary>
    /// 获取客户所有送检的目的
    /// </summary>
    /// <param name="customerCode"></param>
    /// <returns></returns>
    public async Task<List<PurposeMatchMainDto>> GetCustomerPurMatchMainList(string customerCode)
    {
        var data = await _pretreatCustomerPurposeMatchRep.AsQueryable()
            .Where(a => a.CustomerCode == customerCode)
            .Distinct()
            .Select(a => new PurposeMatchMainDto
            {
                CustomerCode = a.CustomerCode,
                CustomerPurCode = a.CustomerPurCode,
                CustomerPurName = a.CustomerPurName
            }).ToListAsync();
        return data;
    }
    /// <summary>
    /// 获取客户指定目的代码的对照明细
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<PurposeMatchDetailDto>> GetCustomerPurMatchDetailList(PurposeMatchMutilQueryInput input)
    {
        var data = await _pretreatCustomerPurposeMatchRep.AsQueryable()
            .Where(a => a.CustomerCode == input.CustomerCode && input.CustomerPurCodes.Contains(a.CustomerPurCode))
            .Distinct()
            .Select(a => new PurposeMatchDetailDto
            {
                CustomerCode = a.CustomerCode,
                CustomerPurCode = a.CustomerPurCode,
                CustomerPurName = a.CustomerPurName,
                CentralPurCode = a.CentralPurCode,
                CentralPurName = a.CentralPurName
            }).ToListAsync();
        return data;
    }
    /// <summary>
    /// 快速对照
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<bool> QuickPurposeMatch(List<QuickPurmatchInput> input)
    {
        if (input == null)
            throw ResultOutput.Exception("参数不能为空！");

        List<PurposeMatchDto> list = new();
        foreach (var currMatch in input)
        {
            var purCodes = currMatch.CentralPurList.FindAll(v => !v.IsCombo).Select(v => v.CentralPurCode).ToList();
            var comboCodes = currMatch.CentralPurList.FindAll(v => v.IsCombo).Select(v => v.CentralPurCode).ToList();

            if (purCodes.Any())
            {
                var currPurposes = await _purposeService.GetPurposeWithPersonalize(purCodes, currMatch.CustomerCode);
                purCodes.ForEach(v =>
                {
                    list.Add(new PurposeMatchDto
                    {
                        CustomerCode = currMatch.CustomerCode,
                        CustomerPurCode = currMatch.CustomerPurCode,
                        CustomerPurName = currMatch.CustomerPurName,
                        CentralPurCode = v,
                        CentralPurName = currPurposes.FirstOrDefault(a => a.PurCode == v)?.PurName,
                        IsCombo = 0,
                    });
                });
            }
            else if (comboCodes.Any())
            {
                var currCombos = await _comboRep.AsQueryable()
                    .Where(a => comboCodes.Contains(a.ComboCode) && a.IsValid == true)
                    .ToListAsync();
                comboCodes.ForEach(v =>
                {
                    list.Add(new PurposeMatchDto
                    {
                        CustomerCode = currMatch.CustomerCode,
                        CustomerPurCode = currMatch.CustomerPurCode,
                        CustomerPurName = currMatch.CustomerPurName,
                        CentralPurCode = v,
                        CentralPurName = currCombos.FirstOrDefault(a => a.ComboCode == v)?.ComboName,
                        IsCombo = 1,
                    });
                });
            }
        }
        var saveList = list.Adapt<List<PretreatCustomerPurposeMatchEntity>>();
        await _pretreatCustomerPurposeMatchRep.InsertRangeAsync(saveList);
        return true;
    }
}