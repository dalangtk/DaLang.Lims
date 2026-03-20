using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaLang.Lims.Exam.Contracts.ExamCriticalValue.Dto
{
    /// <summary>
    /// 危急值查询输入
    /// </summary>
    public class ExamCriticalValueQueryInput
    {
        public string GroupCode { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
    }
}
