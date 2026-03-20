using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DaLang.Lims.Web.Common.Extensions;

/// <summary>
/// 
/// </summary>
public static class ExprExtension
{
    /// <summary>
    /// 添加And条件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expr1"></param>
    /// <param name="expr2"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        return expr1.AndAlso<T>(expr2, Expression.AndAlso);
    }
    /// <summary>
    /// 添加Or条件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expr1"></param>
    /// <param name="expr2"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        return expr1.AndAlso<T>(expr2, Expression.OrElse);
    }

    /// <summary>
    /// 合并表达式以及参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expr1"></param>
    /// <param name="expr2"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    private static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2, Func<Expression, Expression, BinaryExpression> func)
    {
        var parameter = Expression.Parameter(typeof(T));

        var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
        var left = leftVisitor.Visit(expr1.Body);

        var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
        var right = rightVisitor.Visit(expr2.Body);

        return Expression.Lambda<Func<T, bool>>(
            func(left, right), parameter);
    }


    public class ReplaceExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression _oldValue;
        private readonly Expression _newValue;

        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public override Expression Visit(Expression node)
        {
            if (node == _oldValue)
                return _newValue;
            return base.Visit(node);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyName"></param>
    /// <param name="val"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Expression<Func<T, bool>> BuildBool<T>(string propertyName, object val)
    {
        var pe = Expression.Parameter(typeof(T), "p");
        var pi = typeof(T).GetProperty(propertyName);
        if (pi == null)
            return null;

        var me = Expression.MakeMemberAccess(pe, pi);
        var ce = Expression.Constant(val);
        BinaryExpression be = null;
        //if (binary != null)
        //{
        //    switch (binary)
        //    {
        //        case "GreaterThanOrEqual":
        //            be = Expression.GreaterThanOrEqual(me, ce);
        //            break;
        //        case "LessThanOrEqual":
        //            be = Expression.LessThanOrEqual(me, ce);
        //            break;
        //    }
        //}
        //else
        //{
        be = Expression.Equal(me, ce);
        //}
        return Expression.Lambda<Func<T, bool>>(be, pe);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Expression<Func<T, object>> BuildObject<T>(string propertyName)
    {
        //var pe = Expression.Parameter(typeof(T), "p");
        //var pi = typeof(T).GetProperty(propertyName);
        //if (pi == null)
        //    return null;

        //var me = Expression.MakeMemberAccess(pe, pi);
        ////var ce = Expression.Constant(val);
        ////var be = Expression.Equal(me, ce);
        //return Expression.Lambda<Func<T, object>>(pe);
        var propInfo = typeof(T).GetProperty(propertyName);

        if (propInfo == null)
            return null;

        var parameter = Expression.Parameter(typeof(T), "x");

        var property = Expression.Property(parameter, propInfo);

        var delegateType = typeof(Func<,>)
            .MakeGenericType(typeof(T), typeof(object));

        var lambda = GetExpressionLambdaMethod()
            .MakeGenericMethod(delegateType)
            .Invoke(null, new object[] { property, new[] { parameter } });

        return (Expression<Func<T, object>>)lambda;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Expression<Func<T, string>> BuildString<T>(string propertyName)
    {
        //var pe = Expression.Parameter(typeof(T), "p");
        //var pi = typeof(T).GetProperty(propertyName);
        //if (pi == null)
        //    return null;

        //var me = Expression.MakeMemberAccess(pe, pi);
        ////var ce = Expression.Constant(val);
        ////var be = Expression.Equal(me, ce);
        //return Expression.Lambda<Func<T, object>>(pe);
        var propInfo = typeof(T).GetProperty(propertyName);

        if (propInfo == null)
            return null;

        var parameter = Expression.Parameter(typeof(T), "x");

        var property = Expression.Property(parameter, propInfo);

        var delegateType = typeof(Func<,>)
            .MakeGenericType(typeof(T), typeof(string));

        var lambda = GetExpressionLambdaMethod()
            .MakeGenericMethod(delegateType)
            .Invoke(null, new object[] { property, new[] { parameter } });

        return (Expression<Func<T, string>>)lambda;
    }

    public static object Build<T>(string propertyName)
    {
        var propInfo = typeof(T).GetProperty(propertyName);

        if (propInfo == null)
            return null;

        var parameter = Expression.Parameter(typeof(T), "x");

        var property = Expression.Property(parameter, propInfo);

        //var delegateType = typeof(Func<,>)
        //    .MakeGenericType(typeof(T), propInfo.PropertyType);
        var delegateType = typeof(Func<,>)
            .MakeGenericType(typeof(T), typeof(object));

        var lambda = GetExpressionLambdaMethod()
            .MakeGenericMethod(delegateType)
            .Invoke(null, new object[] { property, new[] { parameter } });

        return lambda;
    }

    private static MethodInfo GetExpressionLambdaMethod()
    {
        return typeof(Expression)
            .GetMethods()
            .Where(m => m.Name == "Lambda")
            .Select(m => new
            {
                Method = m,
                Params = m.GetParameters(),
                Args = m.GetGenericArguments()
            })
            .Where(x => x.Params.Length == 2
                        && x.Args.Length == 1
            )
            .Select(x => x.Method)
            .First();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyPath"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Expression<Func<T, object>> GetSortLambda<T>(string propertyPath)
    {
        var param = Expression.Parameter(typeof(T), "p");
        var parts = propertyPath.Split(',');
        Expression parent = param;
        foreach (var part in parts)
        {
            parent = Expression.Property(parent, part);
        }

        if (!parent.Type.IsValueType) return Expression.Lambda<Func<T, object>>(parent, param);

        var converted = Expression.Convert(parent, typeof(object));
        return Expression.Lambda<Func<T, object>>(converted, param);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Expression<Func<T, bool>> True<T>() { return f => true; }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Expression<Func<T, bool>> False<T>() { return f => false; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="expr"></param>
    /// <param name="propertyName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool ParseExpression<T>(Expression<Func<T, bool>> expr, string propertyName)
    {
        var aa = new PropertyAccessFinder();
        aa.Visit(expr);

        var isExists = aa.Properties.ToList().Exists(a => a.Name == propertyName);

        return isExists;
    }
}

/// <summary>
/// 
/// </summary>
public class PropertyAccessFinder : ExpressionVisitor
{
    private readonly HashSet<PropertyInfo> _properties = new HashSet<PropertyInfo>();

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<PropertyInfo> Properties
    {
        get { return _properties; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    protected override Expression VisitMember(MemberExpression node)
    {
        var property = node.Member as PropertyInfo;
        if (property != null)
            _properties.Add(property);

        return base.VisitMember(node);
    }
}
