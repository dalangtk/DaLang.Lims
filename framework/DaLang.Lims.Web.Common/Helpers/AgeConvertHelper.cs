using System;

namespace DaLang.Lims.Web.Common.Helpers;

public class AgeConvertHelper
{
    /// <summary>
    /// Calculate total age in minutes from two age inputs with their respective units.
    /// </summary>
    /// <param name="age1"></param>
    /// <param name="ageUnit1"></param>
    /// <param name="age2"></param>
    /// <param name="ageUnit2"></param>
    /// <returns></returns>
    public static int CalculateAgeToMinute(string age1, string ageUnit1, string age2, string ageUnit2)
    {
        int totalMinutes1 = 0;
        int totalMinutes2 = 0;
        if (int.TryParse(age1.Trim(), out int firstAge))
            totalMinutes1 = ConvertAgeToMinutes(firstAge, ageUnit1);
        if (int.TryParse(age2.Trim(), out int secondAge))
            totalMinutes1 = ConvertAgeToMinutes(secondAge, ageUnit2);
        return totalMinutes1 + totalMinutes2;
    }
    public static int ConvertAgeToMinutes(int age, string ageUnit)
    {
        switch (ageUnit.Trim() ?? "")
        {
            case "104": // Hour
                return age * 60;
            case "103": // Day
                return age * 60 * 24;
            case "105": // Week
                return age * 60 * 24 * 7;
            case "102": // Month (assuming 30 days per month)
                return age * 60 * 24 * 30;
            case "101": // Year (assuming 365 days per year)
                return age * 60 * 24 * 365;
            default:
                throw new ArgumentException("Invalid age unit");
        }
    }
}
