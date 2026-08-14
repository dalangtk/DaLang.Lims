using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaLang.Lims.Agent.Contracts.Dto
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ComposeAttribute : Attribute
    {
        public Type[] SourceTypes { get; }

        public ComposeAttribute(params Type[] sourceTypes)
        {
            SourceTypes = sourceTypes;
        }
    }
}
