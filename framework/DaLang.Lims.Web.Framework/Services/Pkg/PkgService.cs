using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Consts;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain.Org;
using DaLang.Lims.Web.Framework.Domain.Pkg;
using DaLang.Lims.Web.Framework.Domain.PkgPermission;
using DaLang.Lims.Web.Framework.Domain.RolePermission;
using DaLang.Lims.Web.Framework.Domain.Tenant;
using DaLang.Lims.Web.Framework.Domain.TenantPkg;
using DaLang.Lims.Web.Framework.Domain.User;
using DaLang.Lims.Web.Framework.Services.Pkg.Dto;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Entities;

namespace DaLang.Lims.Web.Framework.Services.Pkg;

/// <summary>
/// 套餐服务
/// </summary>
[Order(51)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class PkgService : BaseService, IDynamicApi
{
    private readonly IPkgRepository _pkgRep;
    private readonly Lazy<ITenantRepository> _tenantRep;
    private readonly ITenantPkgRepository _tenantPkgRep;
    private readonly Lazy<IPkgPermissionRepository> _pkgPermissionRep;
    private readonly Lazy<IRolePermissionRepository> _rolePermissionRep;
    private readonly Lazy<IUserRepository> _userRep;

    public PkgService(
        IPkgRepository pkgRep,
        Lazy<ITenantRepository> tenantRep,
        ITenantPkgRepository tenantPkgRep,
        Lazy<IPkgPermissionRepository> pkgPermissionRep,
        Lazy<IRolePermissionRepository> rolePermissionRep,
        Lazy<IUserRepository> userRep
    )
    {
        _pkgRep = pkgRep;
        _tenantRep = tenantRep;
        _tenantPkgRep = tenantPkgRep;
        _pkgPermissionRep = pkgPermissionRep;
        _rolePermissionRep = rolePermissionRep;
        _userRep = userRep;
    }

    /// <summary>
    /// 清除租户下所有用户权限缓存
    /// </summary>
    /// <param name="tenantIds"></param>
    [NonAction]
    public async Task ClearUserPermissionsAsync(List<long> tenantIds)
    {
        //using var _ = _userRepository.DataFilter.Disable(FilterNames.Tenant);
        var userIds = await _userRep.Value.AsQueryable().Where(a => tenantIds.Contains(a.TenantId.Value)).ToListAsync(a => a.Id);
        if (userIds.Any())
        {
            foreach (var userId in userIds)
            {
                await Cache.DelAsync(CacheKeys.UserPermission + userId);
            }
        }
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PkgGetOutput> GetAsync(long id)
    {
        return await _pkgRep.AsQueryable()
        .Where(a => a.Id == id)
        .Select<PkgGetOutput>()
        .FirstAsync();
    }

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<List<PkgGetListOutput>> GetListAsync([FromQuery] PkgGetListInput input)
    {
        var list = await _pkgRep.AsQueryable()
        .WhereIF(input.Name.NotNull(), a => a.Name.Contains(input.Name))
        .OrderBy(a => new { a.ParentId, a.Sort })
        .Select<PkgGetListOutput>()
        .ToListAsync();

        return list;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<PkgGetPageOutput>> GetPageAsync(PageInput<PkgGetPageDto> input)
    {
        var key = input.Filter?.Name;

        var list = await _pkgRep.AsQueryable()
        //.WhereDynamicFilter(input.DynamicFilter)
        .WhereIF(key.NotNull(), a => a.Name.Contains(key))
        .Select<PkgGetPageOutput>()
        .ToPagedListAsync(input.CurrentPage, input.PageSize);
        //.Count(out var total)
        //.OrderByDescending(true, c => c.Id)
        //.Page(input.CurrentPage, input.PageSize)
        //.ToListAsync<PkgGetPageOutput>();

        var data = new PageOutput<PkgGetPageOutput>()
        {
            List = list.Items.ToList(),
            Total = list.Total
        };

        return data;
    }

    /// <summary>
    /// 查询套餐租户列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<List<PkgGetPkgTenantListOutput>> GetPkgTenantListAsync([FromQuery] PkgGetPkgTenantListInput input)
    {
        //using var _ = _tenantRepository.DataFilter.Disable(FilterNames.Tenant);

        var list = await _tenantRep.Value.Context.Queryable<TenantEntity, TenantPkgEntity, OrgEntity>((a, b, c) =>
            a.Id == b.TenantId && a.OrgId == c.Id && b.PkgId == input.PkgId)
            .WhereIF(input.TenantName.NotNull(), (a, b, c) => c.Name.Contains(input.TenantName))
            .OrderByDescending((a, b, c) => b.Id)
            .Select((a, b, c) => new PkgGetPkgTenantListOutput { Id = a.Id, Name = c.Name, Code = c.Code })
            .ToListAsync();

        return list;
    }

    /// <summary>
    /// 查询套餐租户分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<PkgGetPkgTenantListOutput>> GetPkgTenantPageAsync(PageInput<PkgGetPkgTenantListInput> input)
    {
        //using var _ = _tenantRepository.DataFilter.Disable(FilterNames.Tenant);

        var list = await _tenantRep.Value.Context.Queryable<TenantEntity, TenantPkgEntity, OrgEntity>((a, b, c) =>
            a.Id == b.TenantId && a.OrgId == c.Id && b.PkgId == input.Filter.PkgId)
            .WhereIF(input.Filter.TenantName.NotNull(), (a, b, c) => c.Name.Contains(input.Filter.TenantName))
            .Select((a, b, c) => new PkgGetPkgTenantListOutput { Id = a.Id, Name = c.Name, Code = c.Code })
            .ClearFilter<ITenantIdFilter>()
            .ToPagedListAsync(input.CurrentPage, input.PageSize);
        //.Count(out var total)
        //.OrderByDescending((a, b, c) => b.Id)
        //.Page(input.CurrentPage, input.PageSize)
        //.ToListAsync((a, b, c) => new PkgGetPkgTenantListOutput { Id = a.Id, Name = c.Name, Code = c.Code });

        var data = new PageOutput<PkgGetPkgTenantListOutput>()
        {
            List = list.Items.ToList(),
            Total = list.Total
        };

        return data;
    }

    /// <summary>
    /// 查询套餐权限列表
    /// </summary>
    /// <param name="pkgId">套餐编号</param>
    /// <returns></returns>
    public async Task<List<long>> GetPkgPermissionListAsync(long pkgId)
    {
        var permissionIds = await _pkgPermissionRep.Value
            .AsQueryable().Where(d => d.PkgId == pkgId)
            .ToListAsync(a => a.PermissionId);

        return permissionIds;
    }

    /// <summary>
    /// 设置套餐权限
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SetPkgPermissionsAsync(PkgSetPkgPermissionsInput input)
    {
        //查询套餐权限
        var permissionIds = await _pkgPermissionRep.Value.AsQueryable().Where(d => d.PkgId == input.PkgId).ToListAsync(m => m.PermissionId);

        //批量删除套餐权限
        var deleteIds = permissionIds.Where(d => !input.PermissionIds.Contains(d));
        if (deleteIds.Any())
        {
            //删除套餐权限
            await _pkgPermissionRep.Value.DeleteAsync(m => m.PkgId == input.PkgId && deleteIds.Contains(m.PermissionId));
            //删除套餐下关联的角色权限
            await _rolePermissionRep.Value.DeleteAsync(a => deleteIds.Contains(a.PermissionId));
        }

        //批量插入套餐权限
        var pkgPermissions = new List<PkgPermissionEntity>();
        var insertPermissionIds = input.PermissionIds.Where(d => !permissionIds.Contains(d));
        if (insertPermissionIds.Any())
        {
            foreach (var permissionId in insertPermissionIds)
            {
                pkgPermissions.Add(new PkgPermissionEntity()
                {
                    PkgId = input.PkgId,
                    PermissionId = permissionId,
                });
            }
            await _pkgPermissionRep.Value.InsertRangeAsync(pkgPermissions);
        }


        var tenantIds = await _tenantPkgRep.AsQueryable().Where(a => a.PkgId == input.PkgId).ToListAsync(a => a.TenantId.Value);
        //清除租户下所有用户权限缓存
        await ClearUserPermissionsAsync(tenantIds);
    }

    /// <summary>
    /// 添加套餐租户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task AddPkgTenantAsync(PkgAddPkgTenantListInput input)
    {
        var pkgId = input.PkgId;
        var tenantIds = await _tenantPkgRep.AsQueryable().Where(a => a.PkgId == pkgId).ToListAsync(a => a.TenantId.Value);
        var insertTenantIds = input.TenantIds.Except(tenantIds);
        if (insertTenantIds != null && insertTenantIds.Any())
        {
            var tenantPkgList = insertTenantIds.Select(tenantId => new TenantPkgEntity
            {
                TenantId = tenantId,
                PkgId = pkgId
            }).ToList();
            await _tenantPkgRep.InsertRangeAsync(tenantPkgList);

            //清除租户下所有用户权限缓存
            await ClearUserPermissionsAsync(insertTenantIds.ToList());
        }
    }

    /// <summary>
    /// 移除套餐租户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task RemovePkgTenantAsync(PkgAddPkgTenantListInput input)
    {
        var tenantIds = input.TenantIds;
        if (tenantIds != null && tenantIds.Any())
        {
            var entities = await _tenantPkgRep.AsQueryable().Where(a => a.PkgId == input.PkgId && input.TenantIds.Contains(a.TenantId.Value)).ToListAsync();
            entities.ForEach(a => a.IsDeleted = true);
            await _tenantPkgRep.UpdateRangeAsync(entities);

            //清除租户下所有用户权限缓存
            await ClearUserPermissionsAsync(tenantIds.ToList());
        }
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(PkgAddInput input)
    {
        if (await _pkgRep.AsQueryable().AnyAsync(a => a.ParentId == input.ParentId && a.Name == input.Name))
        {
            throw ResultOutput.Exception($"此套餐名已存在");
        }

        if (input.Code.NotNull() && await _pkgRep.AsQueryable().AnyAsync(a => a.ParentId == input.ParentId && a.Code == input.Code))
        {
            throw ResultOutput.Exception($"此套餐编码已存在");
        }

        var entity = Mapper.Map<PkgEntity>(input);
        if (entity.Sort == 0)
        {
            var sort = await _pkgRep.AsQueryable().Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }

        await _pkgRep.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(PkgUpdateInput input)
    {
        var entity = await _pkgRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception("套餐不存在");
        }

        if (await _pkgRep.AsQueryable().AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Name == input.Name))
        {
            throw ResultOutput.Exception($"此套餐名已存在");
        }

        if (input.Code.NotNull() && await _pkgRep.AsQueryable().AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Code == input.Code))
        {
            throw ResultOutput.Exception($"此套餐编码已存在");
        }

        Mapper.Map(input, entity);
        await _pkgRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        var pkgIdList = await _pkgRep.GetChildIdListAsync(id);
        var tenantIds = await _tenantPkgRep.AsQueryable().Where(a => pkgIdList.Contains(a.PkgId)).ToListAsync(a => a.TenantId.Value);

        //删除租户套餐
        await _tenantPkgRep.DeleteAsync(a => a.TenantId == id);
        //删除套餐权限
        await _pkgPermissionRep.Value.DeleteAsync(a => pkgIdList.Contains(a.PkgId));
        //删除套餐
        await _pkgRep.DeleteAsync(a => pkgIdList.Contains(a.Id));

        //清除租户下所有用户权限缓存
        await ClearUserPermissionsAsync(tenantIds);
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        var pkgIdList = await _pkgRep.GetChildIdListAsync(ids);
        var tenantIds = await _tenantPkgRep.AsQueryable().Where(a => pkgIdList.Contains(a.PkgId)).ToListAsync(a => a.TenantId.Value);

        //删除租户套餐
        await _tenantPkgRep.DeleteAsync(a => pkgIdList.Contains(a.PkgId));
        //删除套餐权限
        await _pkgPermissionRep.Value.DeleteAsync(a => pkgIdList.Contains(a.PkgId));
        //删除套餐

        var entities = await _pkgRep.AsQueryable().Where(a => pkgIdList.Contains(a.Id)).ToListAsync();
        entities.ForEach(a => a.IsDeleted = true);
        await _pkgRep.UpdateRangeAsync(entities);

        //清除租户下所有用户权限缓存
        await ClearUserPermissionsAsync(tenantIds);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        var pkgIdList = await _pkgRep.GetChildIdListAsync(id);
        var tenantIds = await _tenantPkgRep.AsQueryable().Where(a => pkgIdList.Contains(a.PkgId)).ToListAsync(a => a.TenantId.Value);
        await _tenantPkgRep.DeleteAsync(a => pkgIdList.Contains(a.PkgId));
        await _pkgPermissionRep.Value.DeleteAsync(a => pkgIdList.Contains(a.PkgId));

        var entites = await _pkgRep.AsQueryable().Where(a => pkgIdList.Contains(a.Id)).ToListAsync();
        entites.ForEach(a => a.IsDeleted = true);

        await _pkgRep.UpdateRangeAsync(entites);

        //清除租户下所有用户权限缓存
        await ClearUserPermissionsAsync(tenantIds);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        var pkgIdList = await _pkgRep.GetChildIdListAsync(ids);
        var tenantIds = await _tenantPkgRep.AsQueryable().Where(a => ids.Contains(a.PkgId)).ToListAsync(a => a.TenantId.Value);
        await _tenantPkgRep.DeleteAsync(a => pkgIdList.Contains(a.PkgId));
        await _pkgPermissionRep.Value.DeleteAsync(a => pkgIdList.Contains(a.PkgId));

        var entites = await _pkgRep.AsQueryable().Where(a => pkgIdList.Contains(a.Id)).ToListAsync();
        entites.ForEach(a => a.IsDeleted = true);
        await _pkgRep.UpdateRangeAsync(entites);
        //清除租户下所有用户权限缓存
        await ClearUserPermissionsAsync(tenantIds);
    }
}