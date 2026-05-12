using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Configs;
using DaLang.Lims.Web.Framework.Core.Consts;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain.Permission;
using DaLang.Lims.Web.Framework.Domain.PermissionApi;
using DaLang.Lims.Web.Framework.Domain.PkgPermission;
using DaLang.Lims.Web.Framework.Domain.Role;
using DaLang.Lims.Web.Framework.Domain.RolePermission;
using DaLang.Lims.Web.Framework.Domain.Tenant;
using DaLang.Lims.Web.Framework.Domain.TenantPermission;
using DaLang.Lims.Web.Framework.Domain.TenantPkg;
using DaLang.Lims.Web.Framework.Domain.User;
using DaLang.Lims.Web.Framework.Domain.UserRole;
using DaLang.Lims.Web.Framework.Services.Permission.Dto;
using Mapster;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaLang.Lims.Web.Framework.Services.Permission;

/// <summary>
/// 权限服务
/// </summary>
[Order(40)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class PermissionService : BaseService, IPermissionService, IDynamicApi
{
    private readonly IPermissionRepository _permissionRep;
    private readonly IPermissionApiRepository _permissionApiRep;
    private readonly Lazy<AppConfig> _appConfig;
    private readonly Lazy<IRoleRepository> _roleRep;
    private readonly Lazy<IUserRepository> _userRep;
    private readonly Lazy<IRolePermissionRepository> _rolePermissionRep;
    private readonly Lazy<ITenantPermissionRepository> _tenantPermissionRep;
    private readonly Lazy<IUserRoleRepository> _userRoleRep;
    public PermissionService(
        IPermissionRepository permissionRep,
        IPermissionApiRepository permissionApiRep,
        Lazy<AppConfig> appConfig,
        Lazy<IRoleRepository> roleRep,
        Lazy<IUserRepository> userRep,
        Lazy<IRolePermissionRepository> rolePermissionRep,
        Lazy<ITenantPermissionRepository> tenantPermissionRep,
        Lazy<IUserRoleRepository> userRoleRep
    )
    {
        _permissionRep = permissionRep;
        _permissionApiRep = permissionApiRep;
        _appConfig = appConfig;
        _roleRep = roleRep;
        _userRep = userRep;
        _rolePermissionRep = rolePermissionRep;
        _tenantPermissionRep = tenantPermissionRep;
        _userRoleRep = userRoleRep;
    }

    /// <summary>
    /// 清除权限下关联的用户权限缓存
    /// </summary>
    /// <param name="permissionIds"></param>
    /// <returns></returns>
    private async Task ClearUserPermissionsAsync(List<long> permissionIds)
    {
        var userIds = await _userRoleRep.Value
            .Context
            .Queryable<UserRoleEntity>().Where(a => SqlFunc.Subqueryable<RolePermissionEntity>().Where(b => a.RoleId == b.RoleId && permissionIds.Contains(b.PermissionId)).Any()).Select(a => a.UserId).ToListAsync();
        foreach (var userId in userIds)
        {
            await Cache.DelAsync(CacheKeys.UserPermission + userId);
        }
    }

    /// <summary>
    /// 查询分组
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PermissionGetGroupOutput> GetGroupAsync(long id)
    {
        var ret = await _permissionRep.GetAsync(id);
        var result = ret.Adapt<PermissionGetGroupOutput>();
        return result;
    }

    /// <summary>
    /// 查询菜单
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PermissionGetMenuOutput> GetMenuAsync(long id)
    {
        var ret = await _permissionRep.GetAsync(id);
        var result = ret.Adapt<PermissionGetMenuOutput>();
        return result;
    }

    /// <summary>
    /// 查询接口
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PermissionGetApiOutput> GetApiAsync(long id)
    {
        var ret = await _permissionRep.GetAsync(id);
        var result = ret.Adapt<PermissionGetApiOutput>();
        return result;
    }

    /// <summary>
    /// 查询权限点
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PermissionGetDotOutput> GetDotAsync(long id)
    {
        var permissions = await _permissionRep.AsQueryable().Includes(a => a.Apis).Where(a => a.Id == id).FirstAsync();
        var output = permissions.Adapt<PermissionGetDotOutput>();
        if (permissions.Apis != null && permissions.Apis.Count > 0)
            output.ApiIds = permissions.Apis.Select(a => a.Id).ToList();
        return output;
    }

    /// <summary>
    /// 查询权限列表
    /// </summary>
    /// <param name="key"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public async Task<List<PermissionListOutput>> GetListAsync(string key, DateTime? start, DateTime? end)
    {
        if (end.HasValue)
        {
            end = end.Value.AddDays(1);
        }

        var data = await _permissionRep.AsQueryable()
            .WhereIF(key.NotNull(), a => a.Path.Contains(key) || a.Label.Contains(key))
            .WhereIF(start.HasValue && end.HasValue, a => SqlFunc.Between(a.ProTime, start.Value, end.Value))
            //.Where(a=>a.Id == 616630205673541)
            .Includes(a => a.View)
            .Includes(a => a.Apis)
            .OrderBy(a => new { a.ParentId, a.Sort })
            //.Select(a => new PermissionListOutput
            //{
            //    ViewPath = a.View.Path,
            //    //ApiPaths = string.Join(";", SqlFunc.Subqueryable<PermissionApiEntity>().Where(b => b.PermissionId == a.Id).ToList(b => b.Api.Path))
            //    ApiPaths = string.Join(";", a.Apis.Select(a => a.Path))
            //})
            .ToListAsync();

        List<PermissionListOutput> ret = new();
        foreach (var item in data)
        {
            var tmp = item.Adapt<PermissionListOutput>();
            tmp.ViewPath = item.View?.Path;
            tmp.ApiPaths = string.Join(";", item.Apis?.Select(a => a.Path));
            ret.Add(tmp);
        }


        return ret;
    }

    /// <summary>
    /// 查询授权权限列表
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<dynamic>> GetPermissionListAsync()
    {
        var permissions = await _permissionRep.AsQueryable()
            .Where(a => a.IsValid == true)
            .WhereIF(_appConfig.Value.Tenant && User.TenantType == TenantType.Tenant, a =>
                _tenantPermissionRep.Value.AsQueryable()
                .Where(b => b.PermissionId == a.Id && b.TenantId == User.TenantId)
                .Any()

                ||

                _permissionRep.Context.Queryable<TenantPkgEntity>()
                .InnerJoin<PkgPermissionEntity>((b, c) => b.PkgId == c.PkgId)
                .Where((b, c) => b.TenantId == User.TenantId && c.PermissionId == a.Id)
                .Any()
            )
            //.AsTreeCte(up: true)
            .ToListAsync(a => new { a.Id, a.ParentId, a.Label, a.Type, a.Sort });

        var menus = permissions.DistinctBy(a => a.Id).OrderBy(a => a.ParentId).ThenBy(a => a.Sort)
            .Select(a => new
            {
                a.Id,
                a.ParentId,
                a.Label,
                Row = a.Type == PermissionType.Menu
            });

        return menus;
    }

    /// <summary>
    /// 查询角色权限列表
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task<List<long>> GetRolePermissionListAsync(long roleId = 0)
    {
        var permissionIds = await _rolePermissionRep.Value.AsQueryable()
            .Where(d => d.RoleId == roleId)
            .Select(a => a.PermissionId)
            .ToListAsync();

        return permissionIds;
    }

    /// <summary>
    /// 查询租户权限列表
    /// </summary>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    [Obsolete("请使用查询套餐权限列表PkgService.GetPkgPermissionListAsync")]
    public async Task<List<long>> GetTenantPermissionListAsync(long tenantId)
    {
        var permissionIds = await _tenantPermissionRep.Value
            .AsQueryable().Where(d => d.TenantId == tenantId)
            .ToListAsync(a => a.PermissionId);

        return permissionIds;
    }

    /// <summary>
    /// 新增分组
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddGroupAsync(PermissionAddGroupInput input)
    {
        var entity = Mapper.Map<PermissionEntity>(input);
        entity.Type = PermissionType.Group;

        if (entity.Sort == 0)
        {
            var sort = await _permissionRep.AsQueryable().Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }

        await _permissionRep.InsertAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// 新增菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddMenuAsync(PermissionAddMenuInput input)
    {
        var entity = Mapper.Map<PermissionEntity>(input);
        entity.Type = PermissionType.Menu;
        if (entity.Sort == 0)
        {
            var sort = await _permissionRep.AsQueryable().Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        await _permissionRep.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 新增接口
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddApiAsync(PermissionAddApiInput input)
    {
        var entity = Mapper.Map<PermissionEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _permissionRep.AsQueryable().Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        await _permissionRep.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 新增权限点
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task<long> AddDotAsync(PermissionAddDotInput input)
    {
        var entity = Mapper.Map<PermissionEntity>(input);
        entity.Type = input.IsApiDot ? PermissionType.Api : PermissionType.Dot;
        if (entity.Sort == 0)
        {
            var sort = await _permissionRep.AsQueryable().Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        await _permissionRep.InsertAsync(entity);

        if (input.ApiIds != null && input.ApiIds.Any())
        {
            var permissionApis = input.ApiIds.Select(a => new PermissionApiEntity { PermissionId = entity.Id, ApiId = a }).ToList();
            await _permissionApiRep.InsertRangeAsync(permissionApis);
        }

        return entity.Id;
    }

    /// <summary>
    /// 修改分组
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateGroupAsync(PermissionUpdateGroupInput input)
    {
        var entity = await _permissionRep.GetAsync(input.Id);
        entity = Mapper.Map(input, entity);
        await _permissionRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 修改菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateMenuAsync(PermissionUpdateMenuInput input)
    {
        var entity = await _permissionRep.GetAsync(input.Id);
        entity = Mapper.Map(input, entity);
        await _permissionRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 修改接口
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateApiAsync(PermissionUpdateApiInput input)
    {
        var entity = await _permissionRep.GetAsync(input.Id);
        entity = Mapper.Map(input, entity);
        await _permissionRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 修改权限点
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task UpdateDotAsync(PermissionUpdateDotInput input)
    {
        var entity = await _permissionRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception("权限点不存在！");
        }

        Mapper.Map(input, entity);
        await _permissionRep.UpdateAsync(entity);
        await _permissionApiRep.DeleteAsync(a => a.PermissionId == entity.Id);

        if (input.ApiIds != null && input.ApiIds.Any())
        {
            var permissionApis = input.ApiIds.Select(a => new PermissionApiEntity { PermissionId = entity.Id, ApiId = a });
            var ret = await _permissionApiRep.InsertRangeAsync(permissionApis.ToList());
        }

        //清除用户权限缓存
        await ClearUserPermissionsAsync(new List<long> { entity.Id });
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        //递归查询所有权限点
        var ids = _permissionRep.AsQueryable()
        .Where(a => a.Id == id)
        //.AsTreeCte()
        .ToList(a => a.Id);

        //删除权限
        var entities = await _permissionRep.GetListAsync(a => ids.Contains(a.Id));
        entities.ForEach(a => a.IsDeleted = true);
        await _permissionRep.UpdateRangeAsync(entities);

        //清除用户权限缓存
        await ClearUserPermissionsAsync(ids);
    }

    /// <summary>
    /// 保存角色权限
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task AssignAsync(PermissionAssignInput input)
    {
        //分配权限的时候判断角色是否存在
        var exists = await _roleRep.Value.AsQueryable()
            //.DisableGlobalFilter(FilterNames.Tenant)
            .Where(o => o.Id == input.RoleId)
            .AnyAsync();
        if (!exists)
        {
            throw ResultOutput.Exception("该角色不存在或已被删除！");
        }

        //查询角色权限
        var permissionIds = await _rolePermissionRep.Value.AsQueryable().Where(d => d.RoleId == input.RoleId).ToListAsync(m => m.PermissionId);

        //批量删除权限
        var deleteIds = permissionIds.Where(d => !input.PermissionIds.Contains(d));
        if (deleteIds.Any())
        {
            await _rolePermissionRep.Value.DeleteAsync(m => m.RoleId == input.RoleId && deleteIds.Contains(m.PermissionId));
        }

        //批量插入权限
        var insertRolePermissions = new List<RolePermissionEntity>();
        var insertPermissionIds = input.PermissionIds.Where(d => !permissionIds.Contains(d));

        //防止租户非法授权，查询主库租户权限范围
        if (_appConfig.Value.Tenant && User.TenantType == TenantType.Tenant)
        {
            var tenantPermissionIds = await _tenantPermissionRep.Value.AsQueryable().Select<TenantPermissionEntity>()
                .Where(a => a.TenantId == User.TenantId).ToListAsync(a => a.PermissionId);

            var pkgPermissionIds = await _permissionRep.Context.Queryable<PkgPermissionEntity>()
                .Where(a =>
                    _permissionRep.Context.Queryable<TenantPkgEntity>()
                    .Where((b) => b.PkgId == a.PkgId && b.TenantId == User.TenantId)
                    .Any()
                )
                .ToListAsync(a => a.PermissionId);

            insertPermissionIds = insertPermissionIds.Where(d => tenantPermissionIds.Contains(d) || pkgPermissionIds.Contains(d));
        }

        if (insertPermissionIds.Any())
        {
            foreach (var permissionId in insertPermissionIds)
            {
                insertRolePermissions.Add(new RolePermissionEntity()
                {
                    RoleId = input.RoleId,
                    PermissionId = permissionId,
                });
            }
            await _rolePermissionRep.Value.InsertRangeAsync(insertRolePermissions);
        }

        //清除角色下关联的用户权限缓存
        var userIds = await _userRoleRep.Value.AsQueryable().Where(a => a.RoleId == input.RoleId).ToListAsync(a => a.UserId);
        foreach (var userId in userIds)
        {
            await Cache.DelAsync(CacheKeys.UserPermission + userId);
        }
    }

    /// <summary>
    /// 保存租户权限
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    [Obsolete("请使用设置套餐权限PkgService.SetPkgPermissionsAsync")]
    public virtual async Task SaveTenantPermissionsAsync(PermissionSaveTenantPermissionsInput input)
    {
        //查询租户权限
        var permissionIds = await _tenantPermissionRep.Value.AsQueryable().Where(d => d.TenantId == input.TenantId).ToListAsync(m => m.PermissionId);

        //批量删除租户权限
        var deleteIds = permissionIds.Where(d => !input.PermissionIds.Contains(d));
        if (deleteIds.Any())
        {
            await _tenantPermissionRep.Value.DeleteAsync(m => m.TenantId == input.TenantId && deleteIds.Contains(m.PermissionId));
            //删除租户下关联的角色权限
            await _rolePermissionRep.Value.DeleteAsync(a => deleteIds.Contains(a.PermissionId));
        }

        //批量插入租户权限
        var tenatPermissions = new List<TenantPermissionEntity>();
        var insertPermissionIds = input.PermissionIds.Where(d => !permissionIds.Contains(d));
        if (insertPermissionIds.Any())
        {
            foreach (var permissionId in insertPermissionIds)
            {
                tenatPermissions.Add(new TenantPermissionEntity()
                {
                    TenantId = input.TenantId,
                    PermissionId = permissionId,
                });
            }
            await _tenantPermissionRep.Value.InsertRangeAsync(tenatPermissions);
        }

        //清除租户下所有用户权限缓存
        //using var _ = _userRepository.DataFilter.Disable(FilterNames.Tenant);
        var userIds = await _userRep.Value.AsQueryable().Where(a => a.TenantId == input.TenantId).ToListAsync(a => a.Id);
        if (userIds.Any())
        {
            foreach (var userId in userIds)
            {
                await Cache.DelAsync(CacheKeys.UserPermission + userId);
            }
        }
    }
}