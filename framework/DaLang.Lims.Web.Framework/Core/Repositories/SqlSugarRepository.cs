
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DaLang.Lims.Web.Framework.Db.SqlSugar;
using System.Linq;
using DaLang.Lims.Web.Framework.Core.Entities;

namespace DaLang.Lims.Web.Framework.Core.Repositories;

/// <summary>
/// SqlSugar 实体仓储
/// </summary>
/// <typeparam name="T"></typeparam>
public class SqlSugarRepository<T> : SimpleClient<T>, ISqlSugarRepository<T> where T : class, new()
{
    public SqlSugarRepository()
    {
        var iTenant = SqlSugarSetup.ITenant; // App.GetRequiredService<ISqlSugarClient>().AsTenant();
        base.Context = (iTenant as SqlSugarScope).ScopedContext;

        // 若实体贴有多库特性，则返回指定库连接
        if (typeof(T).IsDefined(typeof(TenantAttribute), false))
        {
            base.Context = iTenant.GetConnectionScopeWithAttr<T>();
            return;
        }

        //// 若实体贴有日志表特性，则返回日志库连接
        //if (typeof(T).IsDefined(typeof(LogTableAttribute), false))
        //{
        //    if (iTenant.IsAnyConnection(SqlSugarConst.LogConfigId))
        //        base.Context = iTenant.GetConnectionScope(SqlSugarConst.LogConfigId);
        //    return;
        //}

        //// 若实体贴有系统表特性，则返回默认库连接
        //if (typeof(T).IsDefined(typeof(SysTableAttribute), false))
        //    return;

        //// 若未贴任何表特性或当前未登录或是默认租户Id，则返回默认库连接
        //var tenantId = App.User?.FindFirst(ClaimConst.TenantId)?.Value;
        //if (string.IsNullOrWhiteSpace(tenantId) || tenantId == SqlSugarConst.MainConfigId) return;

        //// 根据租户Id切换库连接, 为空则返回默认库连接
        //var sqlSugarScopeProviderTenant = App.GetRequiredService<SysTenantService>().GetTenantDbConnectionScope(long.Parse(tenantId));
        //if (sqlSugarScopeProviderTenant == null) return;
        //base.Context = sqlSugarScopeProviderTenant;
    }

    #region 分表操作

    public async Task<bool> SplitTableInsertAsync(T input)
    {
        return await base.AsInsertable(input).SplitTable().ExecuteCommandAsync() > 0;
    }

    public async Task<bool> SplitTableInsertAsync(List<T> input)
    {
        return await base.AsInsertable(input).SplitTable().ExecuteCommandAsync() > 0;
    }

    public async Task<bool> SplitTableUpdateAsync(T input)
    {
        return await base.AsUpdateable(input).SplitTable().ExecuteCommandAsync() > 0;
    }

    public async Task<bool> SplitTableUpdateAsync(List<T> input)
    {
        return await base.AsUpdateable(input).SplitTable().ExecuteCommandAsync() > 0;
    }

    public async Task<bool> SplitTableDeleteableAsync(T input)
    {
        return await base.Context.Deleteable(input).SplitTable().ExecuteCommandAsync() > 0;
    }

    public async Task<bool> SplitTableDeleteableAsync(List<T> input)
    {
        return await base.Context.Deleteable(input).SplitTable().ExecuteCommandAsync() > 0;
    }

    public Task<T> SplitTableGetFirstAsync(Expression<Func<T, bool>> whereExpression)
    {
        return base.AsQueryable().SplitTable().FirstAsync(whereExpression);
    }

    public Task<bool> SplitTableIsAnyAsync(Expression<Func<T, bool>> whereExpression)
    {
        return base.Context.Queryable<T>().Where(whereExpression).SplitTable().AnyAsync();
    }

    public Task<List<T>> SplitTableGetListAsync()
    {
        return Context.Queryable<T>().SplitTable().ToListAsync();
    }

    public Task<List<T>> SplitTableGetListAsync(Expression<Func<T, bool>> whereExpression)
    {
        return Context.Queryable<T>().Where(whereExpression).SplitTable().ToListAsync();
    }

    public Task<List<T>> SplitTableGetListAsync(Expression<Func<T, bool>> whereExpression, string[] tableNames)
    {
        return Context.Queryable<T>().Where(whereExpression).SplitTable(t => t.InTableNames(tableNames)).ToListAsync();
    }

    public async Task<T> GetAsync(long id)
    {
        var list = new List<Dictionary<string, object>>();
        var dic = new Dictionary<string, object>
        {
            { "Id", id }
        };
        list.Add(dic);

        return await Context.Queryable<T>().WhereColumns(list).FirstAsync();
    }

    public ISugarQueryable<T> GetQueryable(string dynamicCondition = "")
    {
        var queryable = Context.Queryable<T>();

        if (!string.IsNullOrWhiteSpace(dynamicCondition))
        {
            var conditions = Context.Utilities.JsonToConditionalModels(dynamicCondition);
            queryable = queryable.Where(conditions);
        }
        return queryable;
    }

    public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereExpression, string selectFields = "", Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
    {
        var queryable = Context.Queryable<T>().Where(whereExpression);
        if (!string.IsNullOrWhiteSpace(selectFields))
            queryable.Select(selectFields);
        if (orderByExpression != null)
            queryable.OrderBy(orderByExpression, orderByType);
        return await queryable.ToListAsync();
    }
    #endregion 分表操作
    /// <summary>
    /// 批量插入返回id集合
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public async Task<List<long>> InsertListReturnPKAsync(List<T> list)
    {
        return await Context.Insertable<T>(list).ExecuteReturnPkListAsync<long>();
    }
}