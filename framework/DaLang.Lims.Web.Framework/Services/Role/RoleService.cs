using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Consts;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain;
using DaLang.Lims.Web.Framework.Domain.Role;
using DaLang.Lims.Web.Framework.Domain.Role.Dto;
using DaLang.Lims.Web.Framework.Domain.RoleOrg;
using DaLang.Lims.Web.Framework.Domain.RolePermission;
using DaLang.Lims.Web.Framework.Domain.User;
using DaLang.Lims.Web.Framework.Domain.UserRole;
using DaLang.Lims.Web.Framework.Services.Role.Dto;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;

namespace DaLang.Lims.Web.Framework.Services.Role;

/// <summary>
/// 角色服务
/// </summary>
[Order(20)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class RoleService : BaseService, IRoleService, IDynamicApi
{
    private IRoleRepository _roleRep;
    private IUserRepository _userRep;
    private IUserRoleRepository _userRoleRep;
    private IRolePermissionRepository _rolePermissionRep;
    private IRoleOrgRepository _roleOrgRep;

    public RoleService(
        IRoleRepository roleRep,
        IUserRepository userRep,
        IUserRoleRepository userRoleRep,
        IRolePermissionRepository rolePermissionRep,
        IRoleOrgRepository roleOrgRep
    )
    {
        _roleRep = roleRep;
        _userRep = userRep;
        _userRoleRep = userRoleRep;
        _rolePermissionRep = rolePermissionRep;
        _roleOrgRep = roleOrgRep;
    }

    /// <summary>
    /// 添加角色部门
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="orgIds"></param>
    /// <returns></returns>
    private async Task AddRoleOrgAsync(long roleId, long[] orgIds)
    {
        if (orgIds != null && orgIds.Any())
        {
            var roleOrgs = orgIds.Select(orgId => new RoleOrgEntity
            {
                RoleId = roleId,
                OrgId = orgId
            }).ToList();
            await _roleOrgRep.InsertRangeAsync(roleOrgs);
        }
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<RoleGetOutput> GetAsync(long id)
    {
        var roleEntity = await _roleRep.AsQueryable()
        .Where(a => a.Id == id)
        .Includes(a => a.Orgs)
        .Select(a => new RoleGetOutput { Orgs = a.Orgs })
        .FirstAsync();

        var output = Mapper.Map<RoleGetOutput>(roleEntity);

        return output;
    }

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<List<RoleGetListOutput>> GetListAsync([FromQuery] RoleGetListInput input)
    {
        var list = await _roleRep.AsQueryable()
        .WhereIF(input.Name.NotNull(), a => a.Name.Contains(input.Name))
        .OrderBy(a => new { a.ParentId, a.Sort })
        .Select<RoleGetListOutput>()
        .ToListAsync();

        return list;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<RoleGetPageOutput>> GetPageAsync(PageInput<RoleGetPageDto> input)
    {
        var key = input.Filter?.Name;

        var list = await _roleRep.AsQueryable()
        //.WhereDynamicFilter(input.DynamicFilter)
        .WhereIF(key.NotNull(), a => a.Name.Contains(key))
        .Select<RoleGetPageOutput>()
        .ToPagedListAsync(input.CurrentPage, input.PageSize);
        //.Count(out var total)
        //.OrderByDescending(true, c => c.Id)
        //.Page(input.CurrentPage, input.PageSize)
        //.ToListAsync<RoleGetPageOutput>();

        var data = new PageOutput<RoleGetPageOutput>()
        {
            List = list.Items.ToList(),
            Total = list.Total
        };

        return data;
    }

    /// <summary>
    /// 查询角色用户列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<List<RoleGetRoleUserListOutput>> GetRoleUserListAsync([FromQuery] RoleGetRoleUserListInput input)
    {
        var list = await _userRep.Context.Queryable<UserEntity, UserRoleEntity>((a, b) => a.Id == b.UserId)
            .Where((a, b) => b.RoleId == input.RoleId)
            .WhereIF(input.Name.NotNull(), (a, b) => a.Name.Contains(input.Name))
            .OrderByDescending((a, b) => a.Id)
            .Select<RoleGetRoleUserListOutput>()
            .ToListAsync();

        return list;
    }

    /// <summary>
    /// 添加角色用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task AddRoleUserAsync(RoleAddRoleUserListInput input)
    {
        var roleId = input.RoleId;
        var userIds = await _userRoleRep.AsQueryable().Where(a => a.RoleId == roleId).ToListAsync(a => a.UserId);
        var insertUserIds = input.UserIds.Except(userIds);
        if (insertUserIds != null && insertUserIds.Any())
        {
            var userRoleList = insertUserIds.Select(userId => new UserRoleEntity
            {
                UserId = userId,
                RoleId = roleId
            }).ToList();
            await _userRoleRep.InsertRangeAsync(userRoleList);
        }

        var clearUserIds = userIds.Concat(input.UserIds).Distinct();
        foreach (var userId in clearUserIds)
        {
            await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(userId));
        }
    }

    /// <summary>
    /// 移除角色用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task RemoveRoleUserAsync(RoleAddRoleUserListInput input)
    {
        var userIds = input.UserIds;
        if (userIds != null && userIds.Any())
        {
            await _userRoleRep.AsUpdateable().Where(a => a.RoleId == input.RoleId && input.UserIds.Contains(a.UserId)).SetColumns(a => a.IsDeleted == true).ExecuteCommandAsync();
        }

        foreach (var userId in userIds)
        {
            await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(userId));
        }
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(RoleAddInput input)
    {
        if (await _roleRep.AsQueryable().AnyAsync(a => a.ParentId == input.ParentId && a.Name == input.Name))
        {
            throw ResultOutput.Exception($"此{(input.Type == RoleType.Group ? "分组" : "角色")}已存在");
        }

        if (input.Code.NotNull() && await _roleRep.AsQueryable().AnyAsync(a => a.ParentId == input.ParentId && a.Code == input.Code))
        {
            throw ResultOutput.Exception($"此{(input.Type == RoleType.Group ? "分组" : "角色")}编码已存在");
        }

        var entity = Mapper.Map<RoleEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _roleRep.AsQueryable().Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }

        await _roleRep.InsertAsync(entity);
        if (input.DataScope == DataScope.Custom)
        {
            await AddRoleOrgAsync(entity.Id, input.OrgIds);
        }

        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(RoleUpdateInput input)
    {
        var entity = await _roleRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception("角色不存在");
        }

        if (await _roleRep.AsQueryable().AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Name == input.Name))
        {
            throw ResultOutput.Exception($"此{(input.Type == RoleType.Group ? "分组" : "角色")}已存在");
        }

        if (input.Code.NotNull() && await _roleRep.AsQueryable().AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Code == input.Code))
        {
            throw ResultOutput.Exception($"此{(input.Type == RoleType.Group ? "分组" : "角色")}编码已存在");
        }

        Mapper.Map(input, entity);
        await _roleRep.UpdateAsync(entity);
        await _roleOrgRep.DeleteAsync(a => a.RoleId == entity.Id);
        if (input.DataScope == DataScope.Custom)
        {
            await AddRoleOrgAsync(entity.Id, input.OrgIds);
        }

        var userIds = await _userRoleRep.AsQueryable().Where(a => a.RoleId == entity.Id).ToListAsync(a => a.UserId);
        foreach (var userId in userIds)
        {
            await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(userId));
        }
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        var roleIdList = await _roleRep.GetChildIdListAsync(id);
        var userIds = await _userRoleRep.AsQueryable().Where(a => roleIdList.Contains(a.RoleId)).ToListAsync(a => a.UserId);

        //删除用户角色
        await _userRoleRep.DeleteAsync(a => a.UserId == id);
        //删除角色权限
        await _rolePermissionRep.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        //删除角色
        await _roleRep.DeleteAsync(a => roleIdList.Contains(a.Id));

        foreach (var userId in userIds)
        {
            await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(userId));
        }
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        var roleIdList = await _roleRep.GetChildIdListAsync(ids);
        var userIds = await _userRoleRep.AsQueryable().Where(a => roleIdList.Contains(a.RoleId)).ToListAsync(a => a.UserId);

        //删除用户角色
        await _userRoleRep.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        //删除角色权限
        await _rolePermissionRep.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        //删除角色
        await _roleRep.AsUpdateable().Where(a => roleIdList.Contains(a.Id)).SetColumns(a => a.IsDeleted == true).ExecuteCommandAsync();

        foreach (var userId in userIds)
        {
            await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(userId));
        }
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        var roleIdList = await _roleRep.GetChildIdListAsync(id);
        var userIds = await _userRoleRep.AsQueryable().Where(a => roleIdList.Contains(a.RoleId)).ToListAsync(a => a.UserId);
        await _userRoleRep.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        await _rolePermissionRep.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        await _roleRep.AsUpdateable().Where(a => roleIdList.Contains(a.Id)).SetColumns(a => a.IsDeleted == true).ExecuteCommandAsync();
        foreach (var userId in userIds)
        {
            await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(userId));
        }
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        var roleIdList = await _roleRep.GetChildIdListAsync(ids);
        var userIds = await _userRoleRep.AsQueryable().Where(a => ids.Contains(a.RoleId)).ToListAsync(a => a.UserId);
        await _userRoleRep.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        await _rolePermissionRep.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        await _roleRep.AsUpdateable().Where(a => roleIdList.Contains(a.Id)).SetColumns(a => a.IsDeleted == true).ExecuteCommandAsync();
        foreach (var userId in userIds)
        {
            await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(userId));
        }
    }

    /// <summary>
    /// 设置数据权限
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task SetDataScopeAsync(RoleSetDataScopeInput input)
    {
        var entity = await _roleRep.GetAsync(input.RoleId);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception("角色不存在");
        }

        Mapper.Map(input, entity);
        await _roleRep.UpdateAsync(entity);
        await _roleOrgRep.DeleteAsync(a => a.RoleId == entity.Id);
        if (input.DataScope == DataScope.Custom)
        {
            await AddRoleOrgAsync(entity.Id, input.OrgIds);
        }

        var userIds = await _userRoleRep.AsQueryable().Where(a => a.RoleId == entity.Id).ToListAsync(a => a.UserId);
        foreach (var userId in userIds)
        {
            await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(userId));
        }
    }
}