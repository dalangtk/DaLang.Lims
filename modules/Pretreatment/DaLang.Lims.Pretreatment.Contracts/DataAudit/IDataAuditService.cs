using DaLang.Lims.Pretreatment.Contracts.DataAudit.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaLang.Lims.Pretreatment.Contracts.DataAudit;

public interface IDataAuditService
{
    Task<List<DataAuditOutput>> DataAudit(List<long> auditIds);
}
