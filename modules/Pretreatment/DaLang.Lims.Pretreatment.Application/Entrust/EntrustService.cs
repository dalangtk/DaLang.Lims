using DaLang.Lims.BaseData.Domain.BaseAskRule;
using DaLang.Lims.BaseData.Domain.Purpose;
using DaLang.Lims.Pretreatment.Contracts.Entrust;
using DaLang.Lims.Pretreatment.Contracts.Entrust.Dto;
using DaLang.Lims.Pretreatment.Core.Consts;
using DaLang.Lims.Web.BaseData.Domain.BaseAskRuleDetail;
using DaLang.Lims.Web.BaseData.Domain.EntrustPurpose;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services;
using System.Collections.Generic;

namespace DaLang.Lims.Pretreatment.Services.Entrust;

/// <summary>
/// 委托服务
/// </summary>

[DynamicApi(Area = PretreatmentConsts.AreaName)]
public class EntrustService : BaseService, IEntrustService, IDynamicApi
{
    private IBasePurposeRepository _purposeRep;
    private IBaseEntrustPurposeRepository _entrustPurposeRep;
    private IBaseAskRuleRepository _askRuleRep;
    private IBaseAskRuleDetailRepository _askRuleDetailRep;
    public EntrustService(IBaseEntrustPurposeRepository entrustPurposeRep,
        IBasePurposeRepository purposeRep,
        IBaseAskRuleRepository askRuleRep,
        IBaseAskRuleDetailRepository askRuleDetailRep)
    {
        _entrustPurposeRep = entrustPurposeRep;
        _purposeRep = purposeRep;
        _askRuleRep = askRuleRep;
        _askRuleDetailRep = askRuleDetailRep;
    }
    /// <summary>
    /// 计算委托信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<List<CalcEntrustDto>> CalcEntrustAsync(CalcEntrustInput input)
    {
        if (input == null || !input.PurCodeList.Any())
            throw ResultOutput.Exception("参数有误!");
        var purList = await _purposeRep.GetListAsync(v => input.PurCodeList.Contains(v.PurCode));

        var allPurCodes = purList.Select(v => v.PurCode).ToList();
        var notExists = input.PurCodeList.Except(allPurCodes);
        if (notExists.Any())
            throw ResultOutput.Exception($"{notExists.First()}找不到检验目的！");

        //var entrustPurpose = await _entrustPurposeRep.GetEntrustPurpose(input.PurCode, input.SampleTypeCode, input.ReceiveTime, input.CustomerCode);
        var entrustPurposeList = await _entrustPurposeRep.GetListAsync(v => input.PurCodeList.Contains(v.PurCode));
        if (input.ThrowNotExists)
        {
            var entrustPurCodes = entrustPurposeList.Select(v => v.PurCode).ToList();
            notExists = input.PurCodeList.Except(entrustPurCodes);
            if (notExists.Any())
                throw ResultOutput.Exception($"{notExists.First()}找不到委托目的规则！");
        }

        List<CalcEntrustDto> ret = new();
        var week = (int)DateTime.Now.DayOfWeek;
        if (week == 0)
            week = 7;

        foreach (var entrustPurpose in entrustPurposeList)
        {
            var curr = new CalcEntrustDto();
            if (entrustPurpose.BeginTime <= input.ReceiveTime && input.ReceiveTime <= entrustPurpose.EndTime)
            {
                if ((!string.IsNullOrWhiteSpace(entrustPurpose.CustomerCode)
                    && entrustPurpose.IsCustomerReverse == false
                    && entrustPurpose.CustomerCode.Contains(input.CustomerCode!))
                    || (string.IsNullOrWhiteSpace(entrustPurpose.CustomerCode)
                    && entrustPurpose.IsCustomerReverse == false)
                    || (!string.IsNullOrWhiteSpace(entrustPurpose.CustomerCode)
                    && entrustPurpose.IsCustomerReverse == true
                    && !entrustPurpose.CustomerCode.Contains(input.CustomerCode!)))
                {
                    var askRule = await _askRuleRep.GetFirstAsync(v => v.AskRuleCode == entrustPurpose.AskRuleCode);
                    if (askRule != null)
                    {
                        var askRuleDetail = await _askRuleDetailRep.GetListAsync(v => v.AskRuleCode == askRule.AskRuleCode, orderByExpression: v => v.Sort);
                        var validRuleDtail = askRuleDetail.FirstOrDefault(v => v.EntrustCycle!.Contains(week.ToString()));
                        if (validRuleDtail == null)
                            continue;

                        var askDate = DateTime.Now.Date.AddDays(validRuleDtail.AskDay ?? 0);
                        if (!string.IsNullOrWhiteSpace(validRuleDtail.AskTime))
                        {
                            if (TimeSpan.TryParse(validRuleDtail.AskTime, out TimeSpan ts))
                            {
                                askDate = DateTime.Parse($"{askDate:yyyy-MM-dd} {validRuleDtail.AskTime}");
                            }
                        }

                        curr.EntrustHospitalCode = entrustPurpose.EntrustHospitalCode!;
                        curr.EntrustHospitalName = entrustPurpose.EntrustHospitalName!;
                        curr.IsEntrust = true;
                        curr.PurCode = entrustPurpose.PurCode;
                        curr.EstimatedAskTime = askDate;
                    }
                }
            }
            ret.Add(curr);
        }
        return ret;
    }
}
