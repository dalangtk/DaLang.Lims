using DaLang.Lims.Web.Framework.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DaLang.Lims.Web.Framework.Core.Helpers;

public class DynamicFilterExpressionBuilder
{
    public static Expression<Func<T, bool>> BuildPredicate<T>(DynamicFilterInfo filterInfo)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var expression = BuildExpression<T>(filterInfo, parameter);
        return Expression.Lambda<Func<T, bool>>(expression, parameter);
    }

    private static Expression BuildExpression<T>(DynamicFilterInfo filterInfo, ParameterExpression parameter)
    {
        // 如果有子过滤器，处理组合逻辑
        if (filterInfo.Filters != null && filterInfo.Filters.Any())
        {
            return BuildFiltersExpression<T>(filterInfo, parameter);
        }

        // 处理单个字段的过滤条件
        return BuildFieldExpression<T>(filterInfo, parameter);
    }

    private static Expression BuildFiltersExpression<T>(DynamicFilterInfo filterInfo, ParameterExpression parameter)
    {
        Expression result = null;

        foreach (var subFilter in filterInfo.Filters)
        {
            var subExpression = BuildExpression<T>(subFilter, parameter);

            if (result == null)
            {
                result = subExpression;
            }
            else
            {
                result = filterInfo.Logic == DynamicFilterLogic.And
                    ? Expression.AndAlso(result, subExpression)
                    : Expression.OrElse(result, subExpression);
            }
        }

        return result ?? Expression.Constant(true);
    }

    private static Expression BuildFieldExpression<T>(DynamicFilterInfo filterInfo, ParameterExpression parameter)
    {
        if (string.IsNullOrEmpty(filterInfo.Field))
            return Expression.Constant(true);

        // 构建属性访问表达式
        var property = GetPropertyExpression(parameter, filterInfo.Field);

        // 处理空值情况
        if (filterInfo.Value == null)
        {
            return HandleNullValue(filterInfo.Operator, property);
        }

        // 根据操作符构建不同的表达式
        return filterInfo.Operator switch
        {
            DynamicFilterOperator.Equal => BuildEqualExpression(property, filterInfo.Value),
            DynamicFilterOperator.Like => BuildLikeExpression(property, filterInfo.Value),
            DynamicFilterOperator.In => BuildInExpression(property, filterInfo.Value),
            DynamicFilterOperator.NotIn => BuildNotInExpression(property, filterInfo.Value),
            DynamicFilterOperator.LikeLeft => BuildLikeLeftExpression(property, filterInfo.Value),
            DynamicFilterOperator.LikeRight => BuildLikeRightExpression(property, filterInfo.Value),
            DynamicFilterOperator.NoEqual => BuildNotEqualExpression(property, filterInfo.Value),
            DynamicFilterOperator.IsNullOrEmpty => BuildIsNullOrEmptyExpression(property),
            DynamicFilterOperator.IsNot => BuildIsNotNullOrEmptyExpression(property),
            DynamicFilterOperator.NoLike => BuildNotLikeExpression(property, filterInfo.Value),
            DynamicFilterOperator.GreaterThan => BuildGreaterThanExpression(property, filterInfo.Value),
            DynamicFilterOperator.GreaterThanOrEqual => BuildGreaterThanOrEqualExpression(property, filterInfo.Value),
            DynamicFilterOperator.LessThan => BuildLessThanExpression(property, filterInfo.Value),
            DynamicFilterOperator.LessThanOrEqual => BuildLessThanOrEqualExpression(property, filterInfo.Value),
            DynamicFilterOperator.Range => BuildRangeExpression(property, filterInfo.Value),
            DynamicFilterOperator.InLike => BuildInLikeExpression(property, filterInfo.Value),
            _ => Expression.Constant(true)
        };
    }

    private static Expression GetPropertyExpression(Expression parameter, string field)
    {
        var properties = field.Split('.');
        Expression expression = parameter;

        foreach (var prop in properties)
        {
            expression = Expression.PropertyOrField(expression, prop);
        }

        return expression;
    }

    private static Expression HandleNullValue(DynamicFilterOperator op, Expression property)
    {
        return op switch
        {
            DynamicFilterOperator.Equal => Expression.Equal(property, Expression.Constant(null)),
            DynamicFilterOperator.NoEqual => Expression.NotEqual(property, Expression.Constant(null)),
            DynamicFilterOperator.IsNullOrEmpty => BuildIsNullOrEmptyExpression(property),
            DynamicFilterOperator.IsNot => BuildIsNotNullOrEmptyExpression(property),
            _ => Expression.Constant(true)
        };
    }

    private static Expression BuildEqualExpression(Expression property, object value)
    {
        var val = GetFirstValue(value);
        var v = ((Newtonsoft.Json.Linq.JValue)val).Value;

        var constant = Expression.Constant(v);
        return Expression.Equal(property, constant);
    }

    private static Expression BuildNotEqualExpression(Expression property, object value)
    {
        var constant = Expression.Constant(GetFirstValue(value));
        return Expression.NotEqual(property, constant);
    }

    private static Expression BuildLikeExpression(Expression property, object value)
    {
        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        var constant = Expression.Constant(GetFirstValue(value));
        return Expression.Call(property, containsMethod, constant);
    }

    private static Expression BuildNotLikeExpression(Expression property, object value)
    {
        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        var constant = Expression.Constant(GetFirstValue(value));
        var containsCall = Expression.Call(property, containsMethod, constant);
        return Expression.Not(containsCall);
    }

    private static Expression BuildLikeLeftExpression(Expression property, object value)
    {
        var startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
        var constant = Expression.Constant(GetFirstValue(value));
        return Expression.Call(property, startsWithMethod, constant);
    }

    private static Expression BuildLikeRightExpression(Expression property, object value)
    {
        var endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
        var constant = Expression.Constant(GetFirstValue(value));
        return Expression.Call(property, endsWithMethod, constant);
    }

    private static Expression BuildInExpression(Expression property, object value)
    {
        if (value is not IEnumerable<object> values)
            return Expression.Constant(false);

        var containsMethod = GetContainsMethod(value);
        var constant = Expression.Constant(values);
        return Expression.Call(constant, containsMethod, property);
    }

    private static Expression BuildNotInExpression(Expression property, object value)
    {
        if (value is not IEnumerable<object> values)
            return Expression.Constant(true);

        var containsMethod = GetContainsMethod(value);
        var constant = Expression.Constant(values);
        var containsCall = Expression.Call(constant, containsMethod, property);
        return Expression.Not(containsCall);
    }

    private static Expression BuildInLikeExpression(Expression property, object value)
    {
        if (value is not IEnumerable<object> values)
            return Expression.Constant(false);

        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        Expression result = null;

        foreach (var val in values)
        {
            var constant = Expression.Constant(val);
            var likeExpression = Expression.Call(property, containsMethod, constant);

            if (result == null)
                result = likeExpression;
            else
                result = Expression.OrElse(result, likeExpression);
        }

        return result ?? Expression.Constant(false);
    }

    private static Expression BuildIsNullOrEmptyExpression(Expression property)
    {
        var stringEmpty = Expression.Constant(string.Empty);
        var isNull = Expression.Equal(property, Expression.Constant(null));
        var isEmpty = Expression.Equal(property, stringEmpty);
        return Expression.OrElse(isNull, isEmpty);
    }

    private static Expression BuildIsNotNullOrEmptyExpression(Expression property)
    {
        var stringEmpty = Expression.Constant(string.Empty);
        var isNotNull = Expression.NotEqual(property, Expression.Constant(null));
        var isNotEmpty = Expression.NotEqual(property, stringEmpty);
        return Expression.AndAlso(isNotNull, isNotEmpty);
    }

    private static Expression BuildGreaterThanExpression(Expression property, object value)
    {
        var constant = Expression.Constant(GetFirstValue(value));
        return Expression.GreaterThan(property, constant);
    }

    private static Expression BuildGreaterThanOrEqualExpression(Expression property, object value)
    {
        var constant = Expression.Constant(GetFirstValue(value));
        return Expression.GreaterThanOrEqual(property, constant);
    }

    private static Expression BuildLessThanExpression(Expression property, object value)
    {
        var constant = Expression.Constant(GetFirstValue(value));
        return Expression.LessThan(property, constant);
    }

    private static Expression BuildLessThanOrEqualExpression(Expression property, object value)
    {
        var constant = Expression.Constant(GetFirstValue(value));
        return Expression.LessThanOrEqual(property, constant);
    }

    private static Expression BuildRangeExpression(Expression property, object value)
    {
        if (value is not IEnumerable<object> values || values.Count() < 2)
            return Expression.Constant(true);

        var min = Expression.GreaterThanOrEqual(property, Expression.Constant(values.ElementAt(0)));
        var max = Expression.LessThanOrEqual(property, Expression.Constant(values.ElementAt(1)));
        return Expression.AndAlso(min, max);
    }

    private static MethodInfo GetContainsMethod(object value)
    {
        var valueType = value.GetType();
        var elementType = valueType.IsGenericType ? valueType.GetGenericArguments()[0] : typeof(object);
        var enumerableType = typeof(IEnumerable<>).MakeGenericType(elementType);
        return enumerableType.GetMethod("Contains", new[] { elementType });
    }

    private static object GetFirstValue(object value)
    {
        if (value is IEnumerable<object> values)
            return values.FirstOrDefault();
        return value;
    }
}
