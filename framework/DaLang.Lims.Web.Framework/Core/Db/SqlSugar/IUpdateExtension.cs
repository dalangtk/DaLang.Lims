using SqlSugar;
using System;
using System.Linq.Expressions;

namespace DaLang.Lims.Web.Framework.Core.Db.SqlSugar;

public static class IUpdateExtension
{
    public static IUpdateable<T> SetUpdateable<T>(this IUpdateable<T> updateable) where T : class, new()
    {
        var tType = typeof(T).GetType();
        if (tType.GetProperty("ModId") != null)
            updateable.SetColumns("ModId", AppInfo.User.Id);
        if (tType.GetProperty("ModName") != null)
            updateable.SetColumns("ModName", AppInfo.User.UserName);
        if (tType.GetProperty("ModTime") != null)
            updateable.SetColumns("ModTime", DateTime.Now);
        return updateable;
    }
    public static IUpdateable<T> SetUpdateable<T>(this ISimpleClient<T> client) where T : class, new()
    {
        var updateable = client.AsUpdateable();
        var tType = typeof(T);
        if (tType.GetProperty("ModId") != null)
            updateable.SetColumns("ModId", AppInfo.User.Id);
        if (tType.GetProperty("ModName") != null)
            updateable.SetColumns("ModName", AppInfo.User.UserName);
        if (tType.GetProperty("ModTime") != null)
            updateable.SetColumns("ModTime", DateTime.Now);
        return updateable;
    }
    public static IUpdateable<T> GetUpdateable<T>(this ISimpleClient<T> client, T t, bool enableDiffLog = false) where T : class, new()
    {
        var updateable = client.AsUpdateable(t);
        if (enableDiffLog)
            updateable = updateable.EnableDiffLogEvent();
        return updateable;
    }

    public static IUpdateable<T> SetColumnUpdateable<T>(this ISimpleClient<T> client, Expression<Func<T, bool>> columns) where T : class, new()
    {
        var updateable = client.AsUpdateable();
        var tType = typeof(T);
        if (tType.GetProperty("ModId") != null)
            updateable.SetColumns("ModId", AppInfo.User.Id);
        if (tType.GetProperty("ModName") != null)
            updateable.SetColumns("ModName", AppInfo.User.UserName);
        if (tType.GetProperty("ModTime") != null)
            updateable.SetColumns("ModTime", DateTime.Now);

        updateable.SetColumns(columns);
        return updateable;
    }
}
