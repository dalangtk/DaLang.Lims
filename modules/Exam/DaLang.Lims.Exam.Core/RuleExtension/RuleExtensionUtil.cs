using DaLang.Lims.Shared.Contracts.ExamResult.Dto;

namespace DaLang.Lims.Exam.Core.RuleExtension;

public static class RuleExtensionUtil
{
    public static bool CheckItemExists(List<ExamResultDto> examResults, string itemCodes)
    {
        var itemCodeArray = itemCodes.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var resultItemCodes = examResults.Select(er => er.ItemCode).ToList();

        return resultItemCodes.All(ic => itemCodeArray.Contains(ic));
    }
}
