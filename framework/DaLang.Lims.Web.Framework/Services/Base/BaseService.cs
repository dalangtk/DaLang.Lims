using DaLang.Lims.Web.Framework.Core;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Auth;
using DaLang.Lims.Web.Framework.Core.Cache;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Common.Extensions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;
using SqlSugar;

namespace DaLang.Lims.Web.Framework.Services;

public abstract class BaseService : IBaseService
{
    protected readonly object ServiceProviderLock = new object();
    protected IDictionary<Type, object> CachedServices = new Dictionary<Type, object>();
    private ICacheTool _cache;
    private ILoggerFactory _loggerFactory;
    private IMapper _mapper;
    private IUser _user;

    /// <summary>
    /// 缓存
    /// </summary>
    public ICacheTool Cache => LazyGetRequiredService(ref _cache);

    /// <summary>
    /// 日志工厂
    /// </summary>
    public ILoggerFactory LoggerFactory => LazyGetRequiredService(ref _loggerFactory);

    /// <summary>
    /// 映射
    /// </summary>
    public IMapper Mapper => LazyGetRequiredService(ref _mapper);

    public IServiceProvider ServiceProvider { get; set; } = AppInfo.GetService<IServiceProvider>();

    /// <summary>
    /// 用户信息
    /// </summary>
    public IUser User => LazyGetRequiredService(ref _user);

    /// <summary>
    /// 日志
    /// </summary>
    protected ILogger Logger => _lazyLogger.Value;

    private Lazy<ILogger> _lazyLogger => new Lazy<ILogger>(() => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance, true);

    protected TService LazyGetRequiredService<TService>(ref TService reference)
    {
        if (reference == null)
        {
            lock (ServiceProviderLock)
            {
                if (reference == null)
                {
                    reference = ServiceProvider.GetRequiredService<TService>();
                }
            }
        }

        return reference;
    }

    /// <summary>
    /// 获得懒加载服务
    /// </summary>
    /// <typeparam name="TService">服务接口</typeparam>
    /// <returns></returns>
    [NonAction]
    public virtual TService LazyGetRequiredService<TService>()
    {
        return (TService)LazyGetRequiredService(typeof(TService));
    }

    /// <summary>
    /// 根据服务类型获得懒加载服务
    /// </summary>
    /// <param name="serviceType">服务类型</param>
    /// <returns></returns>
    [NonAction]
    public virtual object LazyGetRequiredService(Type serviceType)
    {
        return CachedServices.GetOrAdd(serviceType, () => ServiceProvider.GetRequiredService(serviceType));
    }

    /// <summary>
    /// 动态搜索条件转换sqlsugar搜索条件
    /// </summary>
    /// <param name="di"></param>
    /// <returns></returns>
    [NonAction]
    public string ChangeConditon(DynamicFilterInfo di)
    {
        if (di == null)
            return null;

        string json;
        var sc = new SugarCondition();
        var jsonConfig = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        if (di.Filters == null || di.Filters.Count <= 0)
        {
            if (!string.IsNullOrWhiteSpace(di.Field) && !string.IsNullOrWhiteSpace(di.Value?.ToString()))
            {
                var cObj = new ConditionallistObj();
                cObj.Key = 0;
                cObj.Value = new Value
                {
                    FieldName = di.Field,
                    FieldValue = di.Value?.ToString(),
                    ConditionalList = null,
                    ConditionalType = 0
                };
                sc.ConditionalList.Add(cObj);
            }
            json = JsonConvert.SerializeObject(new List<SugarCondition> { sc }, jsonConfig);
        }
        else
        {
            sc.ConditionalList = new List<ConditionallistObj>();
            var idx = 0;
            foreach (var f in di.Filters)
            {
                var cObj = new ConditionallistObj();
                cObj.Key = idx == 0 ? -1 : di.Logic == DynamicFilterLogic.And ? 0 : 1;
                cObj.Value = new Value
                {
                    FieldName = f.Field,
                    FieldValue = f.Value?.ToString(),
                    ConditionalList = null,
                    ConditionalType = 0
                };
                sc.ConditionalList.Add(cObj);

                if (f.Filters != null && f.Filters.Count > 0)
                {
                    cObj.Value.ConditionalList = new List<ConditionallistObj>();
                    cObj.Value.ConditionalType = null;
                    cObj.Value.FieldValue = null;
                    cObj.Value.FieldName = null;

                    AddSubContion(f.Filters, cObj.Value);
                }

                idx++;
            }
            json = JsonConvert.SerializeObject(new List<SugarCondition> { sc }, jsonConfig);
        }

        return json;
    }
    private void AddSubContion(List<DynamicFilterInfo> filters, Value v)
    {
        var idx2 = 0;
        foreach (var subf in filters)
        {
            var currValue = new ConditionallistObj
            {
                Key = idx2 == 0 ? -1 : subf.Logic == DynamicFilterLogic.And ? 0 : 1,
                Value = new Value
                {
                    FieldName = subf.Field,
                    FieldValue = subf.Value?.ToString(),
                    ConditionalType = 0,
                    ConditionalList = null,
                }
            };
            v.ConditionalList.Add(currValue);
            if (subf.Filters != null && filters.Count > 0)
            {
                currValue.Value.ConditionalList = new List<ConditionallistObj>();
                AddSubContion(subf.Filters, currValue.Value);
            }
            idx2++;
        }
    }

    ///// <summary>
    ///// 动态搜索条件转换sqlsugar搜索条件
    ///// </summary>
    ///// <param name="di"></param>
    ///// <returns></returns>
    //[NonAction]
    //public string ChangeConditon(DynamicFilterInfo di)
    //{
    //    if (di == null)
    //        return null;

    //    string json;
    //    var sc = new SugarCondition();
    //    var jsonConfig = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
    //    if (di.Filters == null || di.Filters.Count <= 0)
    //    {
    //        if (!string.IsNullOrWhiteSpace(di.Field) && !string.IsNullOrWhiteSpace(di.Value?.ToString()))
    //        {
    //            var cObj = new ConditionallistObj();
    //            cObj.Key = 0;
    //            cObj.Value = new Value
    //            {
    //                FieldName = di.Field,
    //                FieldValue = di.Value?.ToString(),
    //                ConditionalList = null,
    //                ConditionalType = 0
    //            };
    //            sc.ConditionalList.Add(cObj);
    //        }
    //        json = JsonConvert.SerializeObject(new List<SugarCondition> { sc }, jsonConfig);
    //    }
    //    else
    //    {
    //        foreach (var condition in di.Filters)
    //        {
    //            var cObj = new ConditionallistObj();
    //            if (condition.Logic == DynamicFilterLogic.Or)
    //            {
    //                cObj.Key = 0;
    //                var childConditionList = new List<ConditionallistObj>();
    //                var idx = 0;
    //                foreach (var subCondition in condition.Filters)
    //                {
    //                    var subCObj = new ConditionallistObj();
    //                    subCObj.Key = condition.Filters.Count == 1 ? 1 : idx == 0 ? 0 : 1;
    //                    subCObj.Value = new Value
    //                    {
    //                        FieldName = subCondition.Field,
    //                        FieldValue = subCondition.Value?.ToString(),
    //                        ConditionalList = null,
    //                        ConditionalType = 0
    //                    };
    //                    idx++;
    //                    childConditionList.Add(subCObj);
    //                }
    //                cObj.Value = new Value
    //                {
    //                    FieldName = condition.Field,
    //                    FieldValue = condition.Value?.ToString(),
    //                    ConditionalList = childConditionList
    //                };
    //                sc.ConditionalList.Add(cObj);
    //            }
    //            else if (condition.Logic == DynamicFilterLogic.And)
    //            {
    //                if (condition.Filters != null && condition.Filters.Count > 0)
    //                {
    //                    foreach (var c in condition.Filters)
    //                    {
    //                        cObj.Key = 0;
    //                        cObj.Value = new Value
    //                        {
    //                            FieldName = c.Field,
    //                            FieldValue = c.Value?.ToString(),
    //                            ConditionalList = null,
    //                            ConditionalType = 0
    //                        };
    //                        sc.ConditionalList.Add(cObj);
    //                    }
    //                }
    //                else
    //                {
    //                    cObj.Key = 0;
    //                    cObj.Value = new Value
    //                    {
    //                        FieldName = condition.Field,
    //                        FieldValue = condition.Value?.ToString(),
    //                        ConditionalList = null,
    //                        ConditionalType = 0
    //                    };
    //                    sc.ConditionalList.Add(cObj);
    //                }
    //            }
    //        }
    //        json = JsonConvert.SerializeObject(new List<SugarCondition> { sc }, jsonConfig);
    //    }

    //    return json;
    //}

    /// <summary>
    /// 获取sort设置的步长
    /// </summary>
    /// <param name="type">实体类型</param>
    /// <returns></returns>
    [NonAction]
    public int GetSortGrowStep(Type type)
    {
        var field = type.GetProperty("Sort");
        var attr = field.GetCustomAttribute(typeof(SortGrowStepAttribute));
        if (attr != null)
            return (attr as SortGrowStepAttribute).Step;

        return 1;
    }
}