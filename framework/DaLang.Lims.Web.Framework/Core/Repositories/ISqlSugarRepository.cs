using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DaLang.Lims.Web.Framework.Core.Repositories;

/// <summary>
/// 分表操作仓储接口
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ISqlSugarRepository<T> : ISugarRepository, ISimpleClient<T> where T : class, new()
{
    /// <summary>
    /// 创建数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<bool> SplitTableInsertAsync(T input);

    /// <summary>
    /// 批量创建数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<bool> SplitTableInsertAsync(List<T> input);

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<bool> SplitTableUpdateAsync(T input);

    /// <summary>
    /// 批量更新数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<bool> SplitTableUpdateAsync(List<T> input);

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<bool> SplitTableDeleteableAsync(T input);

    /// <summary>
    /// 批量删除数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<bool> SplitTableDeleteableAsync(List<T> input);

    /// <summary>
    /// 获取第一条
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <returns></returns>
    Task<T> SplitTableGetFirstAsync(Expression<Func<T, bool>> whereExpression);

    /// <summary>
    /// 判断是否存在
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <returns></returns>
    Task<bool> SplitTableIsAnyAsync(Expression<Func<T, bool>> whereExpression);

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <returns></returns>
    Task<List<T>> SplitTableGetListAsync();

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <returns></returns>
    Task<List<T>> SplitTableGetListAsync(Expression<Func<T, bool>> whereExpression);

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <param name="tableNames">表名</param>
    /// <returns></returns>
    Task<List<T>> SplitTableGetListAsync(Expression<Func<T, bool>> whereExpression, string[] tableNames);
    /// <summary>
    /// 根据id获取数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<T> GetAsync(long id);

    /// <summary>
    /// 获取SugarQueryable，如有动态条件，将拼接传入动态条件
    /// </summary>
    /// <param name="dynamicCondition"></param>
    /// <returns></returns>
    ISugarQueryable<T> GetQueryable(string dynamicCondition = "");
    /// <summary>
    /// 
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <param name="selectFields"></param>
    /// <param name="orderByExpression"></param>
    /// <param name="orderByType"></param>
    /// <returns></returns>
    Task<List<T>> GetListAsync(Expression<Func<T, bool>> whereExpression, string selectFields = "", Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc);
    /// <summary>
    /// 批量插入返回id集合
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    Task<List<long>> InsertListReturnPKAsync(List<T> list);
}