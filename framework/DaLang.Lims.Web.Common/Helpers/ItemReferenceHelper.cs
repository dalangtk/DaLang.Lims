using System;
using System.Linq;

namespace DaLang.Lims.Web.Common.Helpers;

public class ItemReferenceHelper
{
    public static (string, string) CalcQuantitativeAbnormal(string itemResult, string reference, string flag = "Reference", int itemAccuracy = 0)
    {
        decimal rangeUp = -9999;
        decimal rangeDown = -9999;
        string abnormalFlag = "";
        string judgeCondition = "";

        var numberResult = ConvertItemResult(itemResult);
        if (reference.Contains('-'))
        {
            var lowStr = reference.Split('-')[0];
            var upStr = reference.Split('-')[1];
            if (decimal.TryParse(lowStr, out rangeDown) && decimal.TryParse(upStr, out rangeUp))
            {
                if (numberResult < rangeDown)
                {
                    judgeCondition = $"{numberResult} < {rangeDown}";
                    abnormalFlag = "L";
                }
                else if (numberResult > rangeUp)
                {
                    judgeCondition = $"{numberResult} > {rangeUp}";
                    abnormalFlag = "H";
                }
            }

        }
        else if (reference.StartsWith(">="))
        {
            var lowStr = reference.Replace(">=", "");
            rangeDown = decimal.Parse(lowStr);
            if (numberResult < rangeDown)
            {
                judgeCondition = $"{numberResult} < {rangeDown}";
                abnormalFlag = "L";
            }

        }
        else if (reference.StartsWith(">"))
        {
            var lowStr = reference.Replace(">", "");
            rangeDown = decimal.Parse(lowStr);
            if (numberResult <= rangeDown)
            {
                judgeCondition = $"{numberResult} <= {rangeDown}";
                abnormalFlag = "L";
            }
        }
        else if (reference.StartsWith("<="))
        {
            var upStr = reference.Replace("<=", "");
            rangeUp = decimal.Parse(upStr);
            if (numberResult > rangeUp)
            {
                judgeCondition = $"{numberResult} > {rangeUp}";
                abnormalFlag = "H";
            }
        }
        else if (reference.StartsWith("<"))
        {
            var upStr = reference.Replace("<=", "");
            rangeUp = decimal.Parse(upStr);
            if (numberResult > rangeUp)
            {
                judgeCondition = $"{numberResult} > {rangeUp}";
                abnormalFlag = "H";
            }
        }

        switch (flag)
        {
            case "Critical":
                abnormalFlag = abnormalFlag == "H" ? "HH" : abnormalFlag == "L" ? "LL" : "";
                break;
            case "Warning":
                abnormalFlag = string.IsNullOrWhiteSpace(abnormalFlag) ? "" : "W";
                break;
            default:
                break;
        }

        return (abnormalFlag, judgeCondition);
    }
    public static decimal ConvertItemResult(string itemResult)
    {
        var resultStr = StringHelper.ReplaceReferenceOperator(itemResult);
        resultStr = resultStr.Replace(">=", ">").Replace("<=", "<");
        decimal result = -9999;
        if (itemResult.StartsWith(">"))
        {
            result = decimal.Parse(resultStr.Replace(">", ""));
            result += 1 / 100000;
        }
        else if (itemResult.StartsWith("<"))
        {
            result = decimal.Parse(resultStr.Replace("<", ""));
            result -= 1 / 100000;
        }
        else if (itemResult.Contains(":"))
        {
            var leftStr = itemResult.Split(":")[0];
            var rightStr = itemResult.Split(":")[1];

            if (decimal.TryParse(leftStr, out decimal left) && decimal.TryParse(rightStr, out decimal right))
                return left + right;
        }
        else if (RegexHelper.IsScientificNotation(itemResult))
        {
            var tmpResult = double.Parse(itemResult);
            result = decimal.Parse(Math.Log10(tmpResult).ToString());
        }
        else
        {
            result = decimal.Parse(resultStr);
        }
        return result;
    }
    public static string ConvertResultAccuracy(string itemResult, int itemAccuracy = 0)
    {
        var resultStr = StringHelper.ReplaceReferenceOperator(itemResult);
        resultStr = resultStr.Replace(">=", ">").Replace("<=", "<");
        decimal result = -9999;
        if (itemResult.StartsWith(">"))
        {
            result = decimal.Parse(resultStr.Replace(">", ""));
        }
        else if (itemResult.StartsWith("<"))
        {
            result = decimal.Parse(resultStr.Replace("<", ""));
        }
        else if (decimal.TryParse(resultStr, out decimal r))
        {
            result = r;
        }

        result = Math.Round(result, itemAccuracy);
        return result.ToString($"F{itemAccuracy}");
    }
}
