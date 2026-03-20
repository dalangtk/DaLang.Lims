using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace DaLang.Lims.Web.Common.Helpers;

public static class EntityConvertHelper
{
    /// <summary>
    /// datatable转list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public static List<T> ToEntityList<T>(this DataTable dataTable) where T : new()
    {
        var dataList = new List<T>();
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (DataRow row in dataTable.Rows)
        {
            var obj = new T();
            foreach (var property in properties)
            {
                if (dataTable.Columns.Contains(property.Name))
                {
                    Type propertyType = property.PropertyType;
                    object value = row[property.Name];
                    if (value != DBNull.Value)
                    {
                        // 当Property是Nullable类型时，需要获取其UnderlyingType
                        property.SetValue(obj, Convert.ChangeType(value, Nullable.GetUnderlyingType(propertyType) ?? propertyType));
                    }
                }
            }
            dataList.Add(obj);
        }
        return dataList;
    }
}
