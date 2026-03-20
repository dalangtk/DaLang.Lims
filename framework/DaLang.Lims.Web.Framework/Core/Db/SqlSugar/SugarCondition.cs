using System.Collections.Generic;

namespace DaLang.Lims.Web.Framework.Core.Db.SqlSugar
{
    public class SugarCondition
    {
        public List<ConditionallistObj> ConditionalList { get; set; } = new List<ConditionallistObj>();
    }

    public class ConditionallistObj
    {
        public int Key { get; set; }
        public Value Value { get; set; }
    }

    public class Value
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public int? ConditionalType { get; set; }
        public List<ConditionallistObj> ConditionalList { get; set; } = new List<ConditionallistObj>();
    }

}
