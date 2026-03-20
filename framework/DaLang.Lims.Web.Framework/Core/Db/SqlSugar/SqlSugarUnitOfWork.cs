using SqlSugar;
using System.Data;
using DaLang.Lims.Web.Framework.Core.Db;

namespace DaLang.Lims.Web.Framework.Db.SqlSugar;

/// <summary>
/// SqlSugar 事务和工作单元
/// </summary>
public sealed class SqlSugarUnitOfWork : IUnitOfWork
{
    /// <summary>
    /// SqlSugar 对象
    /// </summary>
    private readonly ISqlSugarClient _sqlSugarClient;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sqlSugarClient"></param>
    public SqlSugarUnitOfWork(ISqlSugarClient sqlSugarClient)
    {
        _sqlSugarClient = sqlSugarClient;
    }
    /// <summary>
    /// 开启事物
    /// </summary>
    public void BeginTransaction()
    {
        _sqlSugarClient.AsTenant().BeginTran();
    }
    /// <summary>
    /// 开启事物
    /// </summary>
    public void BeginTransaction(IsolationLevel iso)
    {
        _sqlSugarClient.AsTenant().BeginTran(iso);
    }
    /// <summary>
    /// 提交事物
    /// </summary>
    public void CommitTransaction()
    {
        _sqlSugarClient.AsTenant().CommitTran();
    }

    /// <summary>
    /// 回滚事物
    /// </summary>
    public void RollbackTransaction()
    {
        _sqlSugarClient.AsTenant().RollbackTran();
    }
}