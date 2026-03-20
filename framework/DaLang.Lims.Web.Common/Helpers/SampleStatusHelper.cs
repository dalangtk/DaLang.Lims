using DaLang.Lims.Web.Common.Enums;
using DaLang.Lims.Web.Common.Extensions;

namespace DaLang.Lims.Web.Common.Helpers;

/// <summary>
/// 样本状态帮助类
/// </summary>
public class SampleStatusHelper
{
    public static string CheckStatus(int sampleStatus, OperationTypeEnum operType)
    {
        var currSampleStatusName = ((SampleStatusEnum)sampleStatus).ToDescription();
        var currOperationTypeName = operType.ToDescription();
        switch (operType)
        {
            case OperationTypeEnum.AddItem:
            case OperationTypeEnum.DeleteItem:
            case OperationTypeEnum.FirstCheck:
            case OperationTypeEnum.CancelTest:
                if (sampleStatus != (int)SampleStatusEnum.Testing
                    && sampleStatus != (int)SampleStatusEnum.ReportDelay)
                    return $"当前标本为{currSampleStatusName}状态，无法进行{currOperationTypeName}操作!";
                break;
            case OperationTypeEnum.SecondCheck:
                if (sampleStatus != (int)SampleStatusEnum.FirstCheck)
                    return $"当前标本为{currSampleStatusName}状态，无法进行{currOperationTypeName}操作!";
                break;
            case OperationTypeEnum.UnChecked:
                if (sampleStatus != SampleStatusEnum.FirstCheck.ToInt()
                    && sampleStatus != SampleStatusEnum.SecondCheck.ToInt()
                    && sampleStatus != SampleStatusEnum.Reported.ToInt()
                    && sampleStatus != SampleStatusEnum.Printed.ToInt())
                    return $"当前标本为{currSampleStatusName}状态，无法进行{currOperationTypeName}操作!";
                break;
        }
        return string.Empty;
    }
}
