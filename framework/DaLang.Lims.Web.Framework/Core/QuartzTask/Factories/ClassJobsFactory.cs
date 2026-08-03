using System;
using System.Collections.Generic;

namespace DaLang.Lims.Web.Framework.Core.QuartzTask.Factories;

public class ClassJobsFactory
{
    public static List<string> ClassJobs { get; set; } = new List<string>();
    public static Dictionary<string, Type> JobServiceMap = new();
}
