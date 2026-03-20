using System;
using DaLang.Lims.Web.Framework.Core.Consts;

namespace DaLang.Lims.Web.Framework.Core.Attributes;

/// <summary>
/// 启用权限库事务
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class AdminTransactionAttribute : TransactionAttribute
{
    public AdminTransactionAttribute() : base(DbKeys.MainConfigId)
    {
    }
}