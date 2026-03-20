using DaLang.Lims.Exam.Domain.ReportFiles;
using DaLang.Lims.Shared.Contracts.ApplyPurpose.Dto;
using DaLang.Lims.Shared.Contracts.ExamSampleTrack.Dto;
using DaLang.Lims.Shared.Domain.ApplyInfo;
using DaLang.Lims.Shared.Domain.ApplyPurpose;
using DaLang.Lims.Shared.Domain.ExamInfo;
using DaLang.Lims.Shared.Domain.ExamSampleTrack;
using DaLang.Lims.Statistics.Contracts.ReportQuery;
using DaLang.Lims.Statistics.Contracts.ReportQuery.Dto;
using DaLang.Lims.Statistics.Core.ReportQuery;
using DaLang.Lims.Web.Common.Enums;
using DaLang.Lims.Web.Common.Extensions;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Repositories;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace DaLang.Lims.Statistics.Application.ReportQuery;

/// <summary>
/// 报告服务
/// </summary>
[DynamicApi(Area = StatisticsConsts.AreaName)]
public class ReportQueryService : BaseService, IReportQueryService, IDynamicApi
{
    private readonly AdminRepositoryBase<ApplyInfoEntity> _applyRep;
    private readonly AdminRepositoryBase<ExamInfoEntity> _examInfoRep;
    private readonly AdminRepositoryBase<ApplyPurposeEntity> _applyPurposeRep;
    private readonly AdminRepositoryBase<ExamSampleTrackEntity> _sampleTrackRep;

    public ReportQueryService(AdminRepositoryBase<ApplyInfoEntity> applyRep,
        AdminRepositoryBase<ExamInfoEntity> examInfoRep,
        AdminRepositoryBase<ApplyPurposeEntity> applyPurposeRep,
        AdminRepositoryBase<ExamSampleTrackEntity> sampleTrackRep)
    {
        _applyRep = applyRep;
        _examInfoRep = examInfoRep;
        _applyPurposeRep = applyPurposeRep;
        _sampleTrackRep = sampleTrackRep;
    }

    /// <summary>
    /// 查询报告
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<ReportQueryOutput>> QueryReport(PageInput<ReportQueryInput> input)
    {
        var filter = input.Filter;
        if (string.IsNullOrWhiteSpace(filter.Barcode) && (filter.Begin == null || filter.End == null))
            throw ResultOutput.Exception("请选择报告日期！");

        #region 弃用
        //var list = await _applyRep.AsQueryable()
        //    .LeftJoin<ExamInfoEntity>((a, b) => a.Barcode == b.Barcode && b.SampleStatus != (int)SampleStatusEnum.ReportCancel && !b.IsDeleted)
        //    .LeftJoin<ReportFilesEntity>((a, b, c) => b.Id == c.ExamInfoId && !c.IsDeleted && c.IsValid)
        //    .WhereIF(!string.IsNullOrWhiteSpace(input.Barcode), (a) => a.Barcode == input.Barcode)
        //    .Select((a, b, c) => new ReportQueryDto
        //    {
        //        Id = b.Id,
        //        GroupCode = b.GroupCode,
        //        GroupName = b.GroupName,
        //        Barcode = a.Barcode,
        //        SampleNo = b.SampleNo,
        //        CustomerCode = a.CustomerCode,
        //        CustomerName = a.CustomerName,
        //        CustomerBarcode = a.CustomerBarcode,
        //        PatientId = a.PatientId,
        //        PatientName = a.PatientName,
        //        GenderName = a.GenderName,
        //        Age1 = a.Age1,
        //        AgeUnit1 = a.AgeUnit1,
        //        AgeUnitName1 = a.AgeUnitName1,
        //        Age2 = a.Age2,
        //        AgeUnit2 = a.Age2,
        //        AgeUnitName2 = a.AgeUnitName2,
        //        Department = a.Department,
        //        Ward = a.Ward,
        //        Doctor = a.Doctor,
        //        BedNo = a.BedNo,
        //        Remark = b.Remark,
        //        PurCodes = b.PurCodes,
        //        PurNames = b.PurNames,
        //        SampleTypeCode = b.SampleTypeCode,
        //        SampleTypeName = b.SampleTypeName,
        //        CollectTime = a.CollectTime,
        //        ReceiveTime = a.ReceiveTime,
        //        SampleStatus = b.SampleStatus,
        //        SampleStatusName = b.SampleStatusName,
        //        ResultDescription = b.ResultDescription,
        //        Suggestion = b.Suggestion,
        //        ReportUrl = c.FilePath,
        //        InfoPurCodes = a.PurCodes,
        //        InfoPurNames = a.PurNames,
        //        PurposeList = SqlFunc.Subqueryable<ApplyPurposeEntity>()
        //            .Where(d => d.Barcode == a.Barcode && d.AddType != 2
        //            && d.SampleStatus != (int)SampleStatusEnum.ReportCancel)
        //            .ToList(d => new ApplyPurposeDto
        //            {
        //                PurCode = d.PurCode,
        //                PurName = d.PurName,
        //                PurNamePersonalize = d.PurNamePersonalize,
        //                SampleStatus = d.SampleStatus
        //            })
        //    }, true)
        //    .ToListAsync();

        //var ret = new List<ReportQueryOutput>();
        //foreach (var g in list.GroupBy(v => v.Barcode))
        //{
        //    var max = g.First().PurposeList.Select(v => v.SampleStatus).Distinct().Max();
        //    if (max < SampleStatusEnum.Testing.ToInt())
        //    {
        //        g.First().PurCodes = g.First().InfoPurCodes;
        //        g.First().PurNames = g.First().InfoPurNames;
        //        g.First().SampleStatus = SampleStatusEnum.WaitTesting.ToInt();
        //        g.First().SampleStatusName = SampleStatusEnum.WaitTesting.ToDescription();
        //        ret.Add(g.First().Adapt<ReportQueryOutput>());
        //    }
        //    else
        //    {
        //        var noTestPurs = g.First().PurposeList.FindAll(v => v.SampleStatus < SampleStatusEnum.Testing.ToInt());
        //        if (noTestPurs.Any())
        //        {
        //            var gi = new ReportQueryOutput
        //            {
        //                Barcode = g.First().Barcode,
        //                CustomerCode = g.First().CustomerCode,
        //                CustomerName = g.First().CustomerName,
        //                CustomerBarcode = g.First().CustomerBarcode,
        //                PatientId = g.First().PatientId,
        //                PatientName = g.First().PatientName,
        //                GenderName = g.First().GenderName,
        //                Age1 = g.First().Age1,
        //                AgeUnit1 = g.First().AgeUnit1,
        //                AgeUnitName1 = g.First().AgeUnitName1,
        //                Age2 = g.First().Age2,
        //                AgeUnit2 = g.First().Age2,
        //                AgeUnitName2 = g.First().AgeUnitName2,
        //                Department = g.First().Department,
        //                Ward = g.First().Ward,
        //                Doctor = g.First().Doctor,
        //                BedNo = g.First().BedNo,
        //                Remark = g.First().Remark,
        //                CollectTime = g.First().CollectTime,
        //                ReceiveTime = g.First().ReceiveTime
        //            };
        //            gi.SampleStatus = SampleStatusEnum.WaitTesting.ToInt();
        //            gi.SampleStatusName = SampleStatusEnum.WaitTesting.ToDescription();
        //            gi.PurCodes = string.Join(",", noTestPurs.Select(v => v.PurCode).Distinct());
        //            gi.PurNames = string.Join(",", noTestPurs.Select(v => v.PurNamePersonalize).Distinct());
        //            ret.Add(gi);
        //        }

        //        foreach (var gi in g.Where(v => v.SampleStatus >= SampleStatusEnum.Testing.ToInt()))
        //        {
        //            ret.Add(gi.Adapt<ReportQueryOutput>());
        //        }
        //    }
        //} 
        #endregion

        var queryable = _examInfoRep.AsQueryable()
            .LeftJoin<ReportFilesEntity>((a, b) => a.Id == b.ExamInfoId && !b.IsDeleted && b.IsValid)
            .WhereIF(!string.IsNullOrWhiteSpace(filter.Barcode), (a) => a.Barcode == filter.Barcode);

        if (string.IsNullOrWhiteSpace(filter.Barcode))
        {
            if (filter.TimeType == TimeTypeEnum.ReceiveTime)
                queryable = queryable.Where((a, b) => SqlFunc.Between(a.ReceiveTime, filter.Begin, filter.End));
            else if (filter.TimeType == TimeTypeEnum.ReportTime)
                queryable = queryable.Where((a, b) => SqlFunc.Between(a.SecondAuditTime, filter.Begin, filter.End));
            else if (filter.TimeType == TimeTypeEnum.TestTime)
                queryable = queryable.Where((a, b) => SqlFunc.Between(a.TestDate, filter.Begin, filter.End));
            if (!string.IsNullOrWhiteSpace(filter.Customer))
                queryable = queryable.Where((a, b) => a.CustomerCode == filter.Customer);
        }

        var list = await queryable
            .Where((a, b) => a.SampleStatus != (int)SampleStatusEnum.ReportCancel && !a.IsDeleted)
            .Select((a, b) => new ReportQueryDto
            {
                Id = a.Id,
                GroupCode = a.GroupCode,
                GroupName = a.GroupName,
                Barcode = a.Barcode,
                SampleNo = a.SampleNo,
                CustomerCode = a.CustomerCode,
                CustomerName = a.CustomerName,
                CustomerBarcode = a.CustomerBarcode,
                PatientId = a.PatientId,
                PatientName = a.PatientName,
                GenderName = a.GenderName,
                Age1 = a.Age1,
                AgeUnit1 = a.AgeUnit1,
                AgeUnitName1 = a.AgeUnitName1,
                Age2 = a.Age2,
                AgeUnit2 = a.Age2,
                AgeUnitName2 = a.AgeUnitName2,
                Department = a.Department,
                Ward = a.Ward,
                Doctor = a.Doctor,
                BedNo = a.BedNo,
                Remark = a.Remark,
                PurCodes = a.PurCodes,
                PurNames = a.PurNames,
                SampleTypeCode = a.SampleTypeCode,
                SampleTypeName = a.SampleTypeName,
                CollectTime = a.CollectTime,
                ReceiveTime = a.ReceiveTime,
                SampleStatus = a.SampleStatus,
                SampleStatusName = a.SampleStatusName,
                ResultDescription = a.ResultDescription,
                Suggestion = a.Suggestion,
                ReportUrl = b.FilePath,
                InfoPurCodes = a.PurCodes,
                InfoPurNames = a.PurNames
            }, true)
            .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = new PageOutput<ReportQueryOutput> { List = list.Items.ToList().Adapt<List<ReportQueryOutput>>(), Total = list.Total };
        return data;
    }

    /// <summary>
    /// 打印报告
    /// </summary>
    /// <param name="examId"></param>
    /// <returns></returns>
    [AdminTransaction]
    [HttpGet]
    public async Task<bool> PrintReport(long examId)
    {
        var examInfo = await _examInfoRep.GetFirstAsync(v => v.Id == examId);

        var sampleStatus = SampleStatusEnum.Printed.ToInt();
        var sampleStatusName = SampleStatusEnum.Printed.ToDescription();

        if (examInfo.TaskId != null && examInfo.TaskId > 0)
        {
            await _applyPurposeRep.AsUpdateable()
                                  .SetColumns(v => v.SampleStatus == sampleStatus)
                                  .SetColumns(v => v.SampleStatusName == sampleStatusName)
                                  .Where(v => v.TaskId == examInfo.TaskId)
                                  .ExecuteCommandAsync();
        }
        await _examInfoRep.AsUpdateable()
                          .SetColumns(v => v.SampleStatus == sampleStatus)
                          .SetColumns(v => v.SampleStatusName == sampleStatusName)
                          .Where(v => v.Id == examId)
                          .ExecuteCommandAsync();

        var sampleTrack = new ExamSampleTrackDto
        {
            Barcode = examInfo.Barcode,
            GroupCode = examInfo.GroupCode,
            GroupName = examInfo.GroupName,
            TestDate = examInfo.TestDate,
            SampleNo = examInfo.SampleNo,
            OperationType = OperationTypeEnum.PrintReport,
            TrackContent = $"打印报告.",
        };

        await _sampleTrackRep.InsertAsync(sampleTrack.Adapt<ExamSampleTrackEntity>());

        return true;
    }
}
