using DaLang.Lims.BaseData.Contracts.BasePurpose;
using DaLang.Lims.BaseData.Contracts.Purpose.Dto;
using DaLang.Lims.BaseData.Domain.Combo;
using DaLang.Lims.BaseData.Domain.InstrumentItem;
using DaLang.Lims.BaseData.Domain.Item;
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


namespace DaLang.Lims.BaseData.Services.BasePurpose;

/// <summary>
/// 检验目的服务
/// </summary>
[DynamicApi(Area = BaseDataConsts.AreaName)]
public class BasePurposeService : BaseService, IBasePurposeService, IDynamicApi
{
    private IBasePurposeRepository _basePurposeRep;
    private IBasePurposeDetailRepository _basePurposeDetailRep;
    private IBasePurposePersonalizeRepository _basePurposePersonalizeRep;
    private IBasePurposeTenantSettingRepository _basePurposeTenantSettingRep;
    private IBaseComboRepository _comboRep;

    public BasePurposeService(IBasePurposeRepository basePurposeRep,
        IBasePurposeDetailRepository basePurposeDetailRep,
        IBasePurposePersonalizeRepository basePurposePersonalizeRep,
        IBasePurposeTenantSettingRepository basePurposeTenantSettingRep,
        IBaseComboRepository comboRep)
    {
        _basePurposeRep = basePurposeRep;
        _basePurposeDetailRep = basePurposeDetailRep;
        _basePurposePersonalizeRep = basePurposePersonalizeRep;
        _basePurposeTenantSettingRep = basePurposeTenantSettingRep;
        _comboRep = comboRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BasePurposeDto> GetAsync(long id)
    {
        var purpose = await _basePurposeRep.GetAsync(id);
        return purpose.Adapt<BasePurposeDto>();
    }
    /// <summary>
    /// 获取目的及机构设置
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BasePurposeWithTenantSettingDto> GetPurposeWithTenantSettingAsync(long id)
    {
        var purpose = await _basePurposeRep.GetAsync(id);
        if (purpose == null)
            throw ResultOutput.Exception("未找到目的！");

        var tenantSetting = await _basePurposeTenantSettingRep.GetFirstAsync(v => v.PurCode == purpose.PurCode);

        var output = new BasePurposeWithTenantSettingDto
        {
            BasePurpose = purpose.Adapt<BasePurposeDto>(),
            BasePurposeTenantSetting = tenantSetting.Adapt<BasePurposeTenantSettingDto>()
        };
        return output;
    }
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BasePurposeGetListDto>> GetPageAsync(PageInput<BasePurposeQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _basePurposeRep.GetQueryable(dynamicCondition)
            .WhereIF(filter != null && !string.IsNullOrEmpty(filter.GroupCode), a => a.GroupCode == filter.GroupCode)
            .WhereIF(filter != null && !string.IsNullOrEmpty(filter.PurCode), a => a.PurCode == filter.PurCode || a.PurName.Contains(filter.PurCode))
            .LeftJoin<BasePurposeTenantSettingEntity>((a, b) => a.PurCode == b.PurCode)
            .OrderBy(a => a.Sort)
            .Select((a, b) => new BasePurposeGetListDto
            {
                IsValid = b.IsEnable!.Value
            }, true)
            .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = new PageOutput<BasePurposeGetListDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }
    /// <summary>
    /// 分页查询2
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<BasePurposeGetListDto>> GetPageWithPersonalizeNameAsync(PageInput<BasePurposeQueryInput> input)
    {
        var filter = input.Filter;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);
        var list = await _basePurposeRep.GetQueryable(dynamicCondition)
            .LeftJoin<BasePurposePersonalizeEntity>((a, b) => a.PurCode == b.PurCode && b.IsValid && string.IsNullOrWhiteSpace(b.CustomerCode))
            .WhereIF(filter != null && !string.IsNullOrEmpty(filter.GroupCode), a => a.GroupCode == filter.GroupCode)
            .WhereIF(filter != null && !string.IsNullOrEmpty(filter.PurCode), a => a.PurCode == filter.PurCode)
            .OrderBy(a => a.Sort)
            .Select((a, b) => new BasePurposeGetListDto
            {
                PurName = b.PurNamePersonalize ?? a.PurName
            }, true)
            .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = new PageOutput<BasePurposeGetListDto> { List = list.Items.ToList(), Total = list.Total };
        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AdminTransaction]
    public async Task<long> AddAsync(SavePurposeInput input)
    {
        if (input == null)
            throw ResultOutput.Exception("参数有误");
        var purpose = input.Purpose;
        var purposeDetail = input.PurposeItemDetail;
        var purposePersonalizes = input.PurposePersonalizes;
        var purposeTenantSetting = input.PurposeTenantSetting;
        if (purpose == null || purposeDetail == null)
            throw ResultOutput.Exception("参数有误");

        var purEntity = Mapper.Map<BasePurposeEntity>(purpose);
        if (purEntity.Sort == 0)
        {
            var sort = await _basePurposeRep.AsQueryable().MaxAsync(a => a.Sort);
            purEntity.Sort = sort + 1;
        }

        var personalizeList = Mapper.Map<List<BasePurposePersonalizeEntity>>(purposePersonalizes);
        if (personalizeList.Exists(a => a.Sort == 0))
        {
            var sort = await _basePurposePersonalizeRep.AsQueryable().MaxAsync(a => a.Sort);
            personalizeList.FindAll(a => a.Sort == 0).ForEach(a =>
            {
                a.Sort = sort + 1;
                sort++;
            });
        }

        var id = await _basePurposeRep.InsertReturnSnowflakeIdAsync(purEntity);
        var purDetailEntity = Mapper.Map<List<BasePurposeDetailEntity>>(purposeDetail);
        await _basePurposeDetailRep.InsertRangeAsync(purDetailEntity);
        await _basePurposePersonalizeRep.InsertRangeAsync(personalizeList);
        var tenantSettingEntity = Mapper.Map<BasePurposeTenantSettingEntity>(purposeTenantSetting);
        await _basePurposeTenantSettingRep.InsertAsync(tenantSettingEntity);
        return id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    [AdminTransaction]
    public async Task UpdateAsync(SavePurposeInput input)
    {
        if (input == null)
            throw ResultOutput.Exception("参数有误");
        var purpose = input.Purpose;
        var purposeDetail = input.PurposeItemDetail;
        var purposePersonalizes = input.PurposePersonalizes;
        if (purpose == null)
            throw ResultOutput.Exception("参数有误");

        if (string.IsNullOrEmpty(purpose.PurCode))
            throw ResultOutput.Exception("目的代码不能为空！");

        var purEntity = await _basePurposeRep.GetAsync(purpose.Id);
        if (!(purEntity?.Id > 0))
            throw ResultOutput.Exception("检验目的不存在！");

        #region 处理目的明细
        if (purposeDetail != null)
        {
            var purDetailEntity = Mapper.Map<List<BasePurposeDetailEntity>>(purposeDetail);
            var originalPurDetailList = await _basePurposeDetailRep.GetListAsync(a => a.PurCode == purpose.PurCode);
            if (!originalPurDetailList.Any())
                await _basePurposeDetailRep.InsertRangeAsync(purDetailEntity);
            else
            {
                var needDelIds = new List<long>();
                foreach (var op in originalPurDetailList)
                {
                    var curr = purposeDetail.FirstOrDefault(p => p.ItemCode == op.ItemCode);
                    if (curr != null)
                        curr.Id = op.Id;
                    else
                        needDelIds.Add(op.Id);
                }
                var detailList = Mapper.Map<List<BasePurposeDetailEntity>>(purposeDetail);
                await _basePurposeDetailRep.AsSugarClient().Storageable(detailList).ExecuteCommandAsync();
                //这个项目代码没有了，需要删除明细
                await _basePurposeDetailRep
                    .SetUpdateable()
                    .SetColumns(a => a.IsDeleted == true)
                    .Where(a => needDelIds.Contains(a.Id))
                    .ExecuteCommandAsync();
            }
        }
        #endregion

        #region 处理目的定制
        if (purposePersonalizes != null)
        {
            var purposePersonalizeList = Mapper.Map<List<BasePurposePersonalizeEntity>>(purposePersonalizes);
            var originalPurPersonalizeList = await _basePurposePersonalizeRep.GetListAsync(a => a.PurCode == purpose.PurCode);
            if (!originalPurPersonalizeList.Any())
                await _basePurposePersonalizeRep.InsertRangeAsync(purposePersonalizeList);
            else
            {
                var needDelIds = new List<long>();
                foreach (var newPersonal in purposePersonalizeList)
                {
                    var op = originalPurPersonalizeList.FirstOrDefault(p => p.Id == newPersonal.Id);
                    if (op != null)
                    {
                        Mapper.Map(newPersonal, op);
                    }
                    else if (op == null && newPersonal.Id > 0)
                    {
                        needDelIds.Add(newPersonal.Id);
                    }
                    else
                    {
                        originalPurPersonalizeList.Add(newPersonal.Adapt<BasePurposePersonalizeEntity>());
                    }
                }
                if (needDelIds.Any())
                    originalPurPersonalizeList.RemoveAll(a => needDelIds.Contains(a.Id));

                await _basePurposePersonalizeRep.AsSugarClient().Storageable(originalPurPersonalizeList).ExecuteCommandAsync();
                //这个id没有了，需要删除明细
                await _basePurposePersonalizeRep
                    .SetUpdateable()
                    .SetColumns(a => a.IsDeleted == true)
                    .Where(a => needDelIds.Contains(a.Id))
                    .ExecuteCommandAsync();
            }
        }
        #endregion

        var purposeTenantSetting = input.PurposeTenantSetting;
        BasePurposeTenantSettingEntity settingEntity;
        var currTenantSetting = await _basePurposeTenantSettingRep.GetFirstAsync(v => v.PurCode.Equals(purpose.PurCode));
        if (currTenantSetting != null)
        {
            purposeTenantSetting.Id = currTenantSetting.Id;
            settingEntity = Mapper.Map<BasePurposeTenantSettingEntity>(purposeTenantSetting);
            await _basePurposeTenantSettingRep.UpdateAsync(settingEntity);
        }
        else
        {
            settingEntity = Mapper.Map<BasePurposeTenantSettingEntity>(purposeTenantSetting);
            await _basePurposeTenantSettingRep.InsertAsync(settingEntity);
        }

        Mapper.Map(purpose, purEntity);
        await _basePurposeRep.UpdateAsync(purEntity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _basePurposeRep.SetUpdateable()
            .SetColumns(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }

    /// <summary>
    /// 获取目的明细(已取个性化配置)
    /// </summary>
    /// <param name="purCodes"></param>
    /// <param name="customerCode">客户代码，不传只取机构目的定制</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    //[NonDynamicApi]
    //[NonAction]
    [HttpGet]
    public async Task<List<BasePurposeDto>> GetPurposeWithPersonalize(List<string> purCodes, string customerCode = "")
    {
        var list = await _basePurposeRep.AsQueryable()
             .LeftJoin<BasePurposePersonalizeEntity>((a, b) => a.PurCode == b.PurCode && b.IsValid && !b.IsDeleted)
             .Where((a, b) => b.CustomerCode == customerCode || SqlFunc.IsNullOrEmpty(b.CustomerCode))
             //.WhereIF(string.IsNullOrWhiteSpace(customerCode), (a, b) => SqlFunc.IsNullOrEmpty(b.CustomerCode))
             .WhereIF(purCodes.Any(), (a, b) => purCodes.Contains(a.PurCode))
             .Where((a, b) => a.IsValid && !a.IsDeleted)
             .Select((a, b) => new
             {
                 RowNum = SqlFunc.RowNumber("b.CustomerCode desc", a.PurCode),
                 a.Id,
                 a.GroupCode,
                 a.GroupName,
                 a.PurCode,
                 PurName = b.PurNamePersonalize ?? a.PurName,
                 a.PurNameAB,
                 a.PurNameEN,
                 SampleTypeCode = b.SampleTypeCode ?? a.SampleTypeCode,
                 a.ClinicalSence,
                 a.Suggestions,
                 a.Remark,
                 a.Sort,
                 a.IsValid,
                 b.CustomerCode
             }).MergeTable().Where(a => a.RowNum == 1)
             .Select(a => new BasePurposeDto
             {
                 Id = a.Id,
                 GroupCode = a.GroupCode,
                 GroupName = a.GroupName,
                 PurCode = a.PurCode,
                 PurName = a.PurName,
                 PurNameAB = a.PurNameAB,
                 PurNameEN = a.PurNameEN,
                 SampleTypeCode = a.SampleTypeCode,
                 SampleTypeName = string.IsNullOrWhiteSpace(a.SampleTypeCode) ? "" :
                   SqlFunc.Subqueryable<BaseSampleTypeEntity>()
                   .Where(z => SqlFunc.SplitIn(a.SampleTypeCode, z.SampleTypeCode))
                   .SelectStringJoin(z => z.SampleTypeName, ","),
                 ClinicalSence = a.ClinicalSence,
                 Suggestions = a.Suggestions,
                 Remark = a.Remark,
                 Sort = a.Sort,
                 IsValid = a.IsValid
             })
             .ToListAsync();
        return list;
    }
    [NonDynamicApi]
    [NonAction]
    public async Task<List<PurposeWithDetailListDto>> GetPurposeDetail(List<string> purCodes, string customerCode = "")
    {
        //var sql = _basePurposeRep.AsQueryable()
        //     .LeftJoin<BasePurposePersonalizeEntity>((a, b) => a.PurCode == b.PurCode && b.IsValid && !b.IsDeleted && (b.CustomerCode == customerCode || SqlFunc.IsNullOrEmpty(b.CustomerCode)))
        //     .InnerJoin<BasePurposeDetailEntity>((a, b, c) => a.PurCode == c.PurCode && c.IsValid && !c.IsDeleted)
        //     .InnerJoin<BaseInstrumentItemEntity>((a, b, c, d) => c.InstrumentItemCode == d.InstrumentItemCode && d.IsValid && !d.IsDeleted)
        //     .InnerJoin<BaseItemEntity>((a, b, c, d, e) => e.ItemCode == c.ItemCode && e.IsValid && !e.IsDeleted)
        //     .LeftJoin<BaseItemPersonalizeEntity>((a, b, c, d, e, f) => e.ItemCode == f.ItemCode && f.IsValid && !f.IsDeleted)
        //     //.Where((a, b, c, d, e, f) => b.CustomerCode == customerCode || SqlFunc.IsNullOrEmpty(b.CustomerCode))
        //     .Where((a, b) => a.IsValid && !a.IsDeleted && purCodes.Contains(a.PurCode))
        //     .Select((a, b, c, d, e, f) => new PurposeWithDetailListDto
        //     {
        //         RowNum = SqlFunc.RowNumber("b.CustomerCode desc", e.ItemCode),
        //         GroupCode = a.GroupCode,
        //         GroupName = a.GroupName,
        //         PurCode = a.PurCode,
        //         PurName = b.PurNamePersonalize ?? a.PurName,
        //         SampleTypeCode = b.SampleTypeCode ?? a.SampleTypeCode,
        //         InstrumentItemCode = d.InstrumentItemCode,
        //         InstrumentItemName = d.InstrumentItemName,
        //         ItemCode = f.ItemCode,
        //         ItemName = f.ItemNamePersonalize ?? e.ItemName
        //     }).MergeTable().Where(a => a.RowNum == 1).ToSql();


        var list = await _basePurposeRep.AsQueryable()
             .LeftJoin<BasePurposePersonalizeEntity>((a, b) => a.PurCode == b.PurCode && b.IsValid && !b.IsDeleted && (b.CustomerCode == customerCode || SqlFunc.IsNullOrEmpty(b.CustomerCode)))
             .InnerJoin<BasePurposeDetailEntity>((a, b, c) => a.PurCode == c.PurCode && c.IsValid && !c.IsDeleted)
             .InnerJoin<BaseInstrumentItemEntity>((a, b, c, d) => c.InstrumentItemCode == d.InstrumentItemCode && d.IsValid && !d.IsDeleted)
             .InnerJoin<BaseItemEntity>((a, b, c, d, e) => e.ItemCode == c.ItemCode && e.IsValid && !e.IsDeleted)
             .LeftJoin<BaseItemPersonalizeEntity>((a, b, c, d, e, f) => e.ItemCode == f.ItemCode && f.IsValid && !f.IsDeleted)
             .InnerJoin<BasePurposeTenantSettingEntity>((a, b, c, d, e, f, g) => a.PurCode == g.PurCode && g.IsValid && !g.IsDeleted)
             //.Where((a, b, c, d, e, f) => b.CustomerCode == customerCode || SqlFunc.IsNullOrEmpty(b.CustomerCode))
             .Where((a, b) => a.IsValid && !a.IsDeleted && purCodes.Contains(a.PurCode))
             .Select((a, b, c, d, e, f, g) => new PurposeWithDetailListDto
             {
                 RowNum = SqlFunc.RowNumber("b.CustomerCode desc", e.ItemCode),
                 GroupCode = a.GroupCode,
                 GroupName = a.GroupName,
                 PurCode = a.PurCode,
                 PurName = a.PurName,
                 PurNamePersonalize = b.PurNamePersonalize,
                 ExamPlan = g.ExamPlan,
                 SampleTypeCode = b.SampleTypeCode ?? a.SampleTypeCode,
                 InstrumentItemCode = d.InstrumentItemCode,
                 InstrumentItemName = d.InstrumentItemName,
                 ItemCode = f.ItemCode,
                 ItemName = e.ItemName,
                 ItemNamePersonalize = f.ItemNamePersonalize
             }).MergeTable().Where(a => a.RowNum == 1)
             .ToListAsync();
        return list;
    }
    /// <summary>
    /// 获取目的和套餐
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<PurposeAndComboDto>> GetPurposeAndComboList(QueryPurposeAndComboInput input)
    {
        List<PurposeAndComboDto> ret = new();
        var list = await _basePurposeRep.AsQueryable()
            .LeftJoin<BasePurposePersonalizeEntity>((a, b) => a.PurCode == b.PurCode && b.IsValid && (string.IsNullOrWhiteSpace(b.CustomerCode) || b.CustomerCode == input.CustomerCode))
            //.WhereIF(!string.IsNullOrWhiteSpace(input.CustomerCode), (a, b) => b.CustomerCode == input.CustomerCode)
            //.WhereIF(string.IsNullOrWhiteSpace(input.CustomerCode), (a, b) => SqlFunc.IsNullOrEmpty(b.CustomerCode))
            .WhereIF(!string.IsNullOrWhiteSpace(input.GroupCode), (a, b) => a.GroupCode == input.GroupCode)
            .WhereIF(!string.IsNullOrWhiteSpace(input.PurCode), (a, b) => a.PurCode.Contains(input.PurCode) || a.PurName.Contains(input.PurCode))
            .Where((a, b) => a.IsValid)
            .Take(10)
            .Select((a, b) => new PurposeAndComboDto
            {
                RowNum = SqlFunc.RowNumber("b.CustomerCode desc", a.PurCode),
                PurCode = a.PurCode,
                PurName = b.PurNamePersonalize ?? a.PurName,
                IsCombo = false
            }).MergeTable().Where(a => a.RowNum == 1)
            .ToListAsync();
        if (list.Any())
            ret.AddRange(list);

        if (input.QueryCombo && (!string.IsNullOrEmpty(input.PurCode) || !string.IsNullOrWhiteSpace(input.GroupCode)))
        {
            if (!string.IsNullOrWhiteSpace(input.GroupCode))
            {
                var comboList = await _comboRep.AsQueryable()
                    .Where(a => SqlFunc.Subqueryable<BaseComboDetailEntity>()
                                .InnerJoin<BasePurposeEntity>((b, c) => b.PurCode == c.PurCode && c.GroupCode != input.GroupCode)
                                .Where(b => b.ComboCode == a.ComboCode)
                                .NotAny())
                     .Where(a => a.ComboCode.StartsWith(input.PurCode) || a.ComboName.Contains(input.PurCode))
                     .Where(a => a.CustomerCode == input.CustomerCode || string.IsNullOrWhiteSpace(a.CustomerCode))
                     .Take(10)
                     .Select(a => new PurposeAndComboDto
                     {
                         PurCode = a.ComboCode,
                         PurName = a.ComboName,
                         IsCombo = true
                     })
                     .Distinct().ToListAsync();

                ret.AddRange(comboList);
            }
            else
            {
                var comboList = await _comboRep.AsQueryable()
                     .Where(a => a.ComboCode.StartsWith(input.PurCode) || a.ComboName.Contains(input.PurCode))
                     .Where(a => a.CustomerCode == input.CustomerCode || string.IsNullOrWhiteSpace(a.CustomerCode))
                     .Take(10)
                     .Select(a => new PurposeAndComboDto
                     {
                         PurCode = a.ComboCode,
                         PurName = a.ComboName,
                         IsCombo = true
                     }).ToListAsync();

                ret.AddRange(comboList);
            }
        }

        return ret;
    }
}