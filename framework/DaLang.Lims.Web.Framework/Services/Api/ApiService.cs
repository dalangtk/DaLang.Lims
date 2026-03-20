using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.XPath;
using System.Xml;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Configs;
using DaLang.Lims.Web.Framework.Core.Consts;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain.Api;
using DaLang.Lims.Web.Framework.Domain.Api.Dto;
using DaLang.Lims.Web.Framework.Repositories;
using DaLang.Lims.Web.Framework.Services.Api.Dto;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Common.Extensions;
using DaLang.Lims.Web.Common.Helpers;
using DaLang.Lims.Web.Framework.Resources;
using DaLang.Lims.Web.Framework.Core.Entities;

namespace DaLang.Lims.Web.Framework.Services.Api;

/// <summary>
/// 接口服务
/// </summary>
[Order(90)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class ApiService : BaseService, IApiService, IDynamicApi
{
    private readonly AdminRepositoryBase<ApiEntity> _apiRep;
    private readonly Lazy<AppConfig> _appConfig;
    private readonly AdminLocalizer _adminLocalizer;

    public ApiService(AdminRepositoryBase<ApiEntity> apiRep,
        Lazy<AppConfig> appConfig,
        AdminLocalizer adminLocalizer)
    {
        _apiRep = apiRep;
        _appConfig = appConfig;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ApiGetOutput> GetAsync(long id)
    {
        var result = await _apiRep.GetAsync(id);
        return result.Adapt<ApiGetOutput>();
    }

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<List<ApiListOutput>> GetListAsync(string key)
    {
        var data = await _apiRep.AsQueryable()
            .WhereIF(key.NotNull(), a => a.Path.Contains(key) || a.Label.Contains(key))
            .OrderBy(a => a.ParentId)
            .OrderBy(a => a.Sort)
            .ToListAsync();

        return data.Adapt<List<ApiListOutput>>();
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<SqlSugarPagedList<ApiEntity>> GetPageAsync(PageInput<ApiGetPageDto> input)
    {
        var key = input.Filter?.Label;

        var data = await _apiRep.AsQueryable()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        //.WhereDynamicFilter(input.DynamicFilter)
        //.WhereIf(key.NotNull(), a => a.Path.Contains(key) || a.Label.Contains(key))
        //.Count(out var total)
        //.OrderBy(a => a.ParentId)
        //.OrderBy(a => a.Sort)
        //.Page(input.CurrentPage, input.PageSize)
        //.ToListAsync();

        //var data = new PageOutput<ApiEntity>()
        //{
        //    List = list,
        //    Total = total
        //};

        return data;
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(ApiAddInput input)
    {
        var path = input.Path;

        var entity = await _apiRep.AsQueryable()
            .Where(w => w.Path.Equals(path)).FirstAsync();

        if (entity?.Id > 0)
        {
            Mapper.Map(input, entity);
            entity.IsDeleted = false;
            entity.IsValid = true;
            await _apiRep.UpdateAsync(entity);
            //await _apiRep.UpdateDiy.DisableGlobalFilter(FilterNames.Delete).SetSource(entity).ExecuteAffrowsAsync();

            return entity.Id;
        }
        entity = Mapper.Map<ApiEntity>(input);

        if (entity.Sort == 0)
        {
            var sort = await _apiRep.AsQueryable().Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }

        return await _apiRep.InsertReturnSnowflakeIdAsync(entity);
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(ApiUpdateInput input)
    {
        var entity = await _apiRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["接口不存在"]);
        }

        Mapper.Map(input, entity);
        await _apiRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(long id)
    {
        await _apiRep.AsUpdateable().SetColumns(a => a.IsDeleted == true).Where(a => a.Id == id).ExecuteCommandAsync();
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task BatchDeleteAsync(long[] ids)
    {
        await _apiRep.AsUpdateable().SetColumns(a => a.IsDeleted == true).Where(a => ids.Contains(a.Id)).ExecuteCommandAsync(); ;
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task SoftDeleteAsync(long id)
    {
        await _apiRep.AsUpdateable().SetColumns(a => a.IsDeleted == true).Where(a => a.Id == id).ExecuteCommandAsync();
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _apiRep.AsUpdateable().SetColumns(a => a.IsDeleted == true).Where(o => ids.Contains(o.Id)).ExecuteCommandAsync();
    }

    /// <summary>
    /// 同步
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SyncAsync(ApiSyncInput input)
    {
        if (!(input?.Apis?.Count > 0))
            return;

        //查询分组下所有模块的api
        var groupPaths = input.Apis.FindAll(a => a.ParentPath.IsNull()).Select(a => a.Path);
        var groups = await _apiRep.AsQueryable().Where(a => a.ParentId == 0 && groupPaths.Contains(a.Path)).ToListAsync();
        var groupIds = groups.Select(a => a.Id);
        var modules = await _apiRep.AsQueryable().Where(a => groupIds.Contains(a.ParentId)).ToListAsync();
        var moduleIds = modules.Select(a => a.Id);
        var apis = await _apiRep.AsQueryable().Where(a => moduleIds.Contains(a.ParentId)).ToListAsync();

        apis = groups.Concat(modules).Concat(apis).ToList();
        var paths = apis.Select(a => a.Path).ToList();

        //path处理
        foreach (var api in input.Apis)
        {
            api.Path = api.Path?.Trim().ToLower();
            api.ParentPath = api.ParentPath?.Trim().ToLower();
        }

        #region 执行插入
        //执行父级api插入
        var parentApis = input.Apis.FindAll(a => a.ParentPath.IsNull());
        var pApis = (from a in parentApis where !paths.Contains(a.Path) select a).ToList();
        if (pApis.Count > 0)
        {
            var insertPApis = Mapper.Map<List<ApiEntity>>(pApis);
            foreach (var a in insertPApis)
            {
                var tmp = await _apiRep.AsInsertable(a).ExecuteReturnEntityAsync();
                apis.Add(tmp);
            }
        }

        //执行子级api插入
        var childApis = input.Apis.FindAll(a => a.ParentPath.NotNull());
        var cApis = (from a in childApis where !paths.Contains(a.Path) select a).ToList();
        if (cApis.Count > 0)
        {
            var insertCApis = Mapper.Map<List<ApiEntity>>(cApis);

            foreach (var a in insertCApis)
            {
                var tmp = await _apiRep.AsInsertable(a).ExecuteReturnEntityAsync();
                apis.Add(tmp);
            }
        }
        #endregion 执行插入

        #region 修改和禁用
        {
            //父级api修改
            ApiEntity a;
            List<string> labels;
            string label;
            string desc;
            for (int i = 0, len = parentApis.Count; i < len; i++)
            {
                ApiSyncDto api = parentApis[i];
                a = apis.Find(a => a.Path == api.Path);
                if (a?.Id > 0)
                {
                    labels = api.Label?.Split("\r\n")?.ToList();
                    label = labels != null && labels.Count > 0 ? labels[0] : string.Empty;
                    desc = labels != null && labels.Count > 1 ? string.Join("\r\n", labels.GetRange(1, labels.Count - 1)) : string.Empty;
                    a.ParentId = 0;
                    a.Label = label;
                    a.Description = desc;
                    a.Sort = i + 1;
                    a.IsValid = true;
                    a.IsDeleted = false;
                }
            }
        }

        {
            //子级api修改
            ApiEntity a;
            ApiEntity pa;
            List<string> labels;
            string label;
            string desc;
            for (int i = 0, len = childApis.Count; i < len; i++)
            {
                ApiSyncDto api = childApis[i];
                a = apis.Find(a => a.Path == api.Path);
                pa = apis.Find(a => a.Path == api.ParentPath);
                if (a?.Id > 0)
                {
                    labels = api.Label?.Split("\r\n")?.ToList();
                    label = labels != null && labels.Count > 0 ? labels[0] : string.Empty;
                    desc = labels != null && labels.Count > 1 ? string.Join("\r\n", labels.GetRange(1, labels.Count - 1)) : string.Empty;

                    a.ParentId = pa.Id;
                    a.Label = label;
                    a.Description = desc;
                    a.HttpMethods = api.HttpMethods;
                    a.Sort = i + 1;
                    a.IsValid = true;
                    a.IsDeleted = false;
                }
            }
        }

        {
            //模块和api禁用
            var inputPaths = input.Apis.Select(a => a.Path).ToList();
            var disabledApis = (from a in apis where !inputPaths.Contains(a.Path) select a).ToList();
            if (disabledApis.Count > 0)
            {
                foreach (var api in disabledApis)
                {
                    api.IsValid = false;
                }
            }
        }
        #endregion 修改和禁用

        //批量更新
        _apiRep.Context.QueryFilter.Clear<IDeletedFilter>();
        await _apiRep.AsUpdateable(apis)
        .UpdateColumns(a =>
        new ApiEntity
        {
            ParentId = a.ParentId,
            Label = a.Label,
            HttpMethods = a.HttpMethods,
            Description = a.Description,
            Sort = a.Sort,
            IsValid = a.IsValid,
            IsDeleted = a.IsDeleted,
            ModTime = a.ModTime
        })
        .ExecuteCommandAsync();
    }

    /// <summary>
    /// 获得项目列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [NoOprationLog]
    public List<ProjectConfig> GetProjects()
    {
        return _appConfig.Value.Swagger.Projects;
    }

    static int _CodeBaseNotSupportedException = 0;
    public static Dictionary<string, string> GetSummaryList(Type type)
    {
        return LocalGetComment(type, 0);

        Dictionary<string, string> LocalGetComment(Type localType, int level)
        {
            if (localType.Assembly.IsDynamic) return null;
            //动态生成的程序集，访问不了 Assembly.Location/Assembly.CodeBase
            var regex = new Regex(@"\.(dll|exe)", RegexOptions.IgnoreCase);
            var xmlPath = regex.Replace(localType.Assembly.Location, ".xml");
            if (File.Exists(xmlPath) == false)
            {
                if (_CodeBaseNotSupportedException == 1) return null;
                try
                {
                    if (string.IsNullOrEmpty(localType.Assembly.Location)) return null;
                }
                catch (NotSupportedException) //NotSupportedException: CodeBase is not supported on assemblies loaded from a single-file bundle.
                {
                    Interlocked.Exchange(ref _CodeBaseNotSupportedException, 1);
                    return null;
                }

                xmlPath = regex.Replace(localType.Assembly.Location, ".xml");
                if (xmlPath.StartsWith("file:///") && Uri.TryCreate(xmlPath, UriKind.Absolute, out var tryuri))
                    xmlPath = tryuri.LocalPath;
                if (File.Exists(xmlPath) == false) return null;
            }

            var dic = new Dictionary<string, string>();
            StringReader sReader = null;
            try
            {
                sReader = new StringReader(File.ReadAllText(xmlPath));
            }
            catch
            {
                return dic;
            }
            using (var xmlReader = XmlReader.Create(sReader))
            {
                XPathDocument xpath = null;
                try
                {
                    xpath = new XPathDocument(xmlReader);
                }
                catch
                {
                    return null;
                }
                var xmlNav = xpath.CreateNavigator();

                var className = (localType.IsNested ? $"{localType.Namespace}.{localType.DeclaringType.Name}.{localType.Name}" : $"{localType.Namespace}.{localType.Name}").Trim('.');
                var node = xmlNav.SelectSingleNode($"/doc/members/member[@name='T:{className}']/summary");
                if (node != null)
                {
                    var comment = node.InnerXml.Trim(' ', '\r', '\n', '\t');
                    if (string.IsNullOrEmpty(comment) == false) dic.Add("", comment); //class注释
                }

                if (localType.IsEnum)
                {
                    var fields = Enum.GetValues(localType).Cast<Enum>().Select(x => x.ToString()).ToList();
                    foreach (var field in fields)
                    {
                        node = xmlNav.SelectSingleNode($"/doc/members/member[@name='F:{className}.{field}']/summary");
                        if (node != null)
                        {
                            var comment = node.InnerXml.Trim(' ', '\r', '\n', '\t');
                            if (string.IsNullOrEmpty(comment) == false) dic.Add(field, comment); //field注释
                        }
                    }
                }
            }
            return dic;
        }
    }

}