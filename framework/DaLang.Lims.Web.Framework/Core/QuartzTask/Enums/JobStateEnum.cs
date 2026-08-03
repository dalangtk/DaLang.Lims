using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaLang.Lims.Web.Framework.Core.QuartzTask.Enums
{
    public enum JobStateEnum
    {
        New = 1,
        Delete = 2,
        Modified = 3,
        Pause = 4,
        Stop=5,
        Start=6,
        Run=7
    }
}
