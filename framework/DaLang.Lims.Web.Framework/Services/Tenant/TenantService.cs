using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yitter.IdGenerator;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Configs;
using DaLang.Lims.Web.Framework.Core.Consts;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Core.Helpers;
using DaLang.Lims.Web.Framework.Domain.Org;
using DaLang.Lims.Web.Framework.Domain.Pkg;
using DaLang.Lims.Web.Framework.Domain.Role;
using DaLang.Lims.Web.Framework.Domain.RolePermission;
using DaLang.Lims.Web.Framework.Domain.Tenant;
using DaLang.Lims.Web.Framework.Domain.Tenant.Dto;
using DaLang.Lims.Web.Framework.Domain.TenantPkg;
using DaLang.Lims.Web.Framework.Domain.User;
using DaLang.Lims.Web.Framework.Domain.UserOrg;
using DaLang.Lims.Web.Framework.Domain.UserRole;
using DaLang.Lims.Web.Framework.Domain.UserStaff;
using DaLang.Lims.Web.Framework.Services.Pkg;
using DaLang.Lims.Web.Framework.Services.Tenant.Dto;
using DaLang.Lims.Web.Common.Helpers;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;

namespace DaLang.Lims.Web.Framework.Services.Tenant;

/// <summary>
/// 租户服务
/// </summary>
[Order(50)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class TenantService : BaseService, ITenantService, IDynamicApi
{
    private AppConfig _appConfig => LazyGetRequiredService<AppConfig>();
    private readonly ITenantRepository _tenantRep;
    private readonly ITenantPkgRepository _tenantPkgRep;
    private readonly IRoleRepository _roleRep;
    private readonly IUserRepository _userRep;
    private readonly IOrgRepository _orgRep;
    private readonly Lazy<IUserRoleRepository> _userRoleRep;
    private readonly Lazy<IRolePermissionRepository> _rolePermissionRep;
    private readonly Lazy<IUserStaffRepository> _userStaffRep;
    private readonly Lazy<IUserOrgRepository> _userOrgRep;
    private readonly Lazy<IPasswordHasher<UserEntity>> _passwordHasher;
    private readonly Lazy<UserHelper> _userHelper;

    public TenantService(
        ITenantRepository tenantRep,
        ITenantPkgRepository tenantPkgRep,
        IRoleRepository roleRep,
        IUserRepository userRep,
        IOrgRepository orgRep,
        Lazy<IUserRoleRepository> userRoleRep,
        Lazy<IRolePermissionRepository> rolePermissionRep,
        Lazy<IUserStaffRepository> userStaffRep,
        Lazy<IUserOrgRepository> userOrgRep,
        Lazy<IPasswordHasher<UserEntity>> passwordHasher,
        Lazy<UserHelper> userHelper)
    {
        _tenantRep = tenantRep;
        _tenantPkgRep = tenantPkgRep;
        _roleRep = roleRep;
        _userRep = userRep;
        _orgRep = orgRep;
        _userRoleRep = userRoleRep;
        _rolePermissionRep = rolePermissionRep;
        _userStaffRep = userStaffRep;
        _userOrgRep = userOrgRep;
        _passwordHasher = passwordHasher;
        _userHelper = userHelper;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<TenantGetOutput> GetAsync(long id)
    {
        var tenant = await _tenantRep.AsQueryable()
        .Where(a => a.Id == id)
        .Includes(a => a.Pkgs)
        .Select(a => new TenantGetOutput
        {
            Id = a.Id,
            Name = a.Org.Name,
            Code = a.Org.Code,
            Pkgs = a.Pkgs,
            UserName = a.User.UserName,
            RealName = a.User.Name,
            Phone = a.User.Mobile,
            Email = a.User.Email,
        })
        .FirstAsync();
        return tenant;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<TenantListOutput>> GetPageAsync(PageInput<TenantGetPageDto> input)
    {
        var key = input.Filter?.Name;
        var conditionStr = ChangeConditon(input.DynamicFilter);

        var list = await _tenantRep.GetQueryable(conditionStr)
        .WhereIF(key.NotNull(), a => a.Org.Name.Contains(key))
        .Select(a => new TenantListOutput
        {
            Id = a.Id,
            Name = a.Org.Name,
            Code = a.Org.Code,
            UserName = a.User.UserName,
            RealName = a.User.Name,
            Phone = a.User.Mobile,
            Email = a.User.Email,
            Pkgs = a.Pkgs,
        })
        .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = new PageOutput<TenantListOutput>()
        {
            List = list.Items.ToList(),
            Total = list.Total,
        };

        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task<long> AddAsync(TenantAddInput input)
    {
        if (input.Password.IsNull())
        {
            input.Password = _appConfig.DefaultPassword;
        }
        _userHelper.Value.CheckPassword(input.Password);

        var existsOrg = await _orgRep.AsQueryable()
        .Where(a => (a.Name == input.Name || a.Code == input.Code) && a.ParentId == 0)
        .Select(a => new { a.Name, a.Code })
        .FirstAsync();

        if (existsOrg != null)
        {
            if (existsOrg.Name == input.Name)
            {
                throw ResultOutput.Exception($"企业名称已存在");
            }

            if (existsOrg.Code == input.Code)
            {
                throw ResultOutput.Exception($"企业编码已存在");
            }
        }

        var where = Expressionable.Create<UserEntity>()
            .And(a => a.UserName == input.UserName)
            .OrIF(input.Phone.NotNull(), a => a.Mobile == input.Phone)
            .OrIF(input.Email.NotNull(), a => a.Email == input.Email);

        var existsUser = await _userRep.AsQueryable()
            .Where(where.ToExpression())
            .Select(a => new { a.UserName, a.Mobile, a.Email })
            .FirstAsync();

        if (existsUser != null)
        {
            if (existsUser.UserName == input.UserName)
            {
                throw ResultOutput.Exception($"企业账号已存在");
            }

            if (input.Phone.NotNull() && existsUser.Mobile == input.Phone)
            {
                throw ResultOutput.Exception($"企业手机号已存在");
            }

            if (input.Email.NotNull() && existsUser.Email == input.Email)
            {
                throw ResultOutput.Exception($"企业邮箱已存在");
            }
        }

        if (string.IsNullOrWhiteSpace(input.DbKey))
            input.DbKey = DbKeys.MainConfigId;

        //添加租户
        TenantEntity entity = Mapper.Map<TenantEntity>(input);
        TenantEntity tenant = await _tenantRep.InsertReturnEntityAsync(entity);
        long tenantId = tenant.Id;

        //添加租户套餐
        if (input.PkgIds != null && input.PkgIds.Any())
        {
            var pkgs = input.PkgIds.Select(pkgId => new TenantPkgEntity
            {
                TenantId = tenantId,
                PkgId = pkgId
            }).ToList();

            await _tenantPkgRep.InsertRangeAsync(pkgs);
        }

        //添加部门
        var org = new OrgEntity
        {
            TenantId = tenantId,
            Name = input.Name,
            Code = input.Code,
            Value = input.Code,
            ParentId = 0,
            MemberCount = 1,
            Sort = 1,
            IsValid = true
        };
        await _orgRep.InsertAsync(org);

        //添加用户
        var user = new UserEntity
        {
            TenantId = tenantId,
            UserName = input.UserName,
            Name = input.RealName,
            Mobile = input.Phone,
            Email = input.Email,
            Type = UserType.TenantAdmin,
            OrgId = org.Id,
            IsValid = true
        };
        if (_appConfig.PasswordHasher)
        {
            user.Password = _passwordHasher.Value.HashPassword(user, input.Password);
            user.PasswordEncryptType = PasswordEncryptType.PasswordHasher;
        }
        else
        {
            user.Password = MD5Encrypt.Encrypt32(input.Password);
            user.PasswordEncryptType = PasswordEncryptType.MD5Encrypt32;
        }
        await _userRep.InsertAsync(user);

        long userId = user.Id;

        //添加用户员工
        var emp = new UserStaffEntity
        {
            Id = userId,
            TenantId = tenantId
        };
        await _userStaffRep.Value.InsertAsync(emp);

        //添加用户部门
        var userOrg = new UserOrgEntity
        {
            UserId = userId,
            OrgId = org.Id
        };
        await _userOrgRep.Value.InsertAsync(userOrg);

        //添加角色分组和角色
        var roleGroupId = YitIdHelper.NextId();
        var roleId = YitIdHelper.NextId();
        var jobGroupId = YitIdHelper.NextId();
        var roles = new List<RoleEntity>{
                new RoleEntity
                {
                    Id = roleGroupId,
                    ParentId = 0,
                    TenantId = tenantId,
                    Type = RoleType.Group,
                    Name = "系统默认",
                    Sort = 1
                },
                new RoleEntity
                {
                    Id = roleId,
                    TenantId = tenantId,
                    Type = RoleType.Role,
                    Name = "主管理员",
                    Code = "main-admin",
                    ParentId = roleGroupId,
                    DataScope = DataScope.All,
                    Sort = 1
                },
                new RoleEntity
                {
                    Id= jobGroupId,
                    ParentId = 0,
                    TenantId = tenantId,
                    Type = RoleType.Group,
                    Name = "岗位",
                    Sort = 2
                },
                new RoleEntity
                {
                    TenantId = tenantId,
                    Type = RoleType.Role,
                    Name = "普通员工",
                    Code = "emp",
                    ParentId = jobGroupId,
                    DataScope = DataScope.Self,
                    Sort = 1
                }
            };
        await _roleRep.InsertRangeAsync(roles);

        //添加用户角色
        var userRole = new UserRoleEntity()
        {
            UserId = userId,
            RoleId = roleId
        };
        await _userRoleRep.Value.InsertAsync(userRole);

        //更新租户的用户和部门
        tenant.UserId = userId;
        tenant.OrgId = org.Id;
        await _tenantRep.UpdateAsync(tenant);

        return tenant.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(TenantUpdateInput input)
    {
        var tenant = await _tenantRep.GetAsync(input.Id);
        if (!(tenant?.Id > 0))
        {
            throw ResultOutput.Exception("租户不存在");
        }

        var existsOrg = await _orgRep.AsQueryable()
            .Where(a => a.Id != tenant.OrgId && (a.Name == input.Name || a.Code == input.Code))
            .Select(a => new { a.Name, a.Code })
            .FirstAsync();

        if (existsOrg != null)
        {
            if (existsOrg.Name == input.Name)
            {
                throw ResultOutput.Exception($"企业名称已存在");
            }

            if (existsOrg.Code == input.Code)
            {
                throw ResultOutput.Exception($"企业编码已存在");
            }
        }

        var where = Expressionable.Create<UserEntity>()
            .And(a => a.UserName == input.UserName)
            .OrIF(input.Phone.NotNull(), a => a.Mobile == input.Phone)
            .OrIF(input.Email.NotNull(), a => a.Email == input.Email);

        var existsUser = await _userRep.AsQueryable()
            .Where(a => a.Id != tenant.UserId).
            Where(where.ToExpression()).
            Select(a => new { a.Id, a.Name, a.UserName, a.Mobile, a.Email })
            .FirstAsync();

        if (existsUser != null)
        {
            if (existsUser.UserName == input.UserName)
            {
                throw ResultOutput.Exception($"企业账号已存在");
            }

            if (input.Phone.NotNull() && existsUser.Mobile == input.Phone)
            {
                throw ResultOutput.Exception($"企业手机号已存在");
            }

            if (input.Email.NotNull() && existsUser.Email == input.Email)
            {
                throw ResultOutput.Exception($"企业邮箱已存在");
            }
        }
        //更新用户
        await _userRep
            .AsUpdateable()
            .SetColumns(a => new UserEntity { Name = input.RealName, UserName = input.UserName, Mobile = input.Phone, Email = input.Email })
            .Where(a => a.Id == tenant.UserId)
            .ExecuteCommandAsync();

        //更新部门
        await _orgRep.AsUpdateable()
            .SetColumns(a => new OrgEntity { Name = input.Name, Code = input.Code })
            .Where(a => a.Id == tenant.OrgId)
            .ExecuteCommandAsync();

        //更新租户
        await _tenantRep.AsUpdateable()
            .SetColumns(a => new TenantEntity { Description = input.Description })
           .Where(a => a.Id == tenant.Id).ExecuteCommandAsync();

        //更新租户套餐
        await _tenantPkgRep.DeleteAsync(a => a.TenantId == tenant.Id);
        if (input.PkgIds != null && input.PkgIds.Any())
        {
            var pkgs = input.PkgIds.Select(pkgId => new TenantPkgEntity
            {
                TenantId = tenant.Id,
                PkgId = pkgId
            }).ToList();

            await _tenantPkgRep.InsertRangeAsync(pkgs);

            //清除租户下所有用户权限缓存
            await LazyGetRequiredService<PkgService>().ClearUserPermissionsAsync(new List<long> { tenant.Id });
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
        var tenantType = await _tenantRep.AsQueryable().Where(a => a.Id == id).Select(a => a.TenantType).FirstAsync();
        if (tenantType == (int)TenantType.Platform)
        {
            throw ResultOutput.Exception("平台租户禁止删除");
        }

        //删除角色权限
        await _rolePermissionRep.Value.AsUpdateable().Where(a => a.Role.TenantId == id).SetColumns(a => a.IsDeleted == true).ExecuteCommandAsync();

        //删除用户角色
        await _userRoleRep.Value.AsUpdateable().Where(a => a.User.TenantId == id).SetColumns(a => a.IsDeleted == true).ExecuteCommandAsync();

        //删除员工
        await _userStaffRep.Value.AsUpdateable().Where(a => a.TenantId == id).SetColumns(a => a.IsDeleted == true).ExecuteCommandAsync();

        //删除用户部门
        await _userOrgRep.Value.AsUpdateable().Where(a => a.User.TenantId == id).SetColumns(a => a.IsDeleted == true).ExecuteCommandAsync();

        //删除部门
        await _orgRep.AsUpdateable().Where(a => a.TenantId == id).SetColumns(a => a.IsDeleted == true).ExecuteCommandAsync();

        //删除用户
        await _userRep.AsUpdateable().Where(a => a.TenantId == id && a.Type != UserType.Member).SetColumns(a => a.IsDeleted == true).ExecuteCommandAsync();

        //删除角色
        await _roleRep.AsUpdateable().Where(a => a.TenantId == id).SetColumns(a => a.IsDeleted == true).ExecuteCommandAsync();

        //删除租户套餐
        await _tenantPkgRep.AsUpdateable().SetColumns(a => a.IsDeleted == true).Where(a => a.TenantId == id).ExecuteCommandAsync();

        //删除租户
        await _tenantRep.AsUpdateable().SetColumns(a => a.IsDeleted == true).Where(a => a.Id == id).ExecuteCommandAsync();

        //清除租户下所有用户权限缓存
        await LazyGetRequiredService<PkgService>().ClearUserPermissionsAsync(new List<long> { id });
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        var tenantType = await _tenantRep.AsQueryable().Where(a => a.Id == id).Select(a => a.TenantType).FirstAsync();
        if (tenantType == (int)TenantType.Platform)
        {
            throw ResultOutput.Exception("平台租户禁止删除");
        }

        //删除部门
        await _orgRep.AsUpdateable().SetColumns(a => a.IsDeleted == true).Where(a => a.TenantId == id).ExecuteCommandAsync();

        //删除用户
        await _userRep.AsUpdateable().SetColumns(a => a.IsDeleted == true).Where(a => a.TenantId == id && a.Type != UserType.Member).ExecuteCommandAsync();

        //删除角色
        await _roleRep.AsUpdateable().SetColumns(a => a.IsDeleted == true).Where(a => a.TenantId == id).ExecuteCommandAsync();

        //删除租户
        await _tenantRep.AsUpdateable().SetColumns(a => a.IsDeleted == true).Where(a => a.Id == id).ExecuteCommandAsync();

        //清除租户下所有用户权限缓存
        await LazyGetRequiredService<PkgService>().ClearUserPermissionsAsync(new List<long> { id });
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        var tenantType = await _tenantRep.AsQueryable().Where(a => ids.Contains(a.Id)).Select(a => a.TenantType).FirstAsync();
        if (tenantType == (int)TenantType.Platform)
        {
            throw ResultOutput.Exception("平台租户禁止删除");
        }

        //删除部门
        await _orgRep.AsUpdateable().SetColumns(a => a.IsDeleted == true).Where(a => ids.Contains(a.TenantId.Value)).ExecuteCommandAsync();

        //删除用户
        await _userRep.AsUpdateable().SetColumns(a => a.IsDeleted == true).Where(a => ids.Contains(a.TenantId.Value) && a.Type != UserType.Member).ExecuteCommandAsync();

        //删除角色
        await _roleRep.AsUpdateable().SetColumns(a => a.IsDeleted == true).Where(a => ids.Contains(a.TenantId.Value)).ExecuteCommandAsync();

        //删除租户
        await _tenantRep.AsUpdateable().SetColumns(a => a.IsDeleted == true).Where(a => ids.Contains(a.Id)).ExecuteCommandAsync();

        //清除租户下所有用户权限缓存
        await LazyGetRequiredService<PkgService>().ClearUserPermissionsAsync(ids.ToList());
    }

    /// <summary>
    /// 设置启用
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task SetEnableAsync(TenantSetEnableInput input)
    {
        var entity = await _tenantRep.GetAsync(input.TenantId);
        if (entity.TenantType == (int)TenantType.Platform)
        {
            throw ResultOutput.Exception("平台租户禁止禁用");
        }
        entity.IsValid = input.IsValid;
        await _tenantRep.UpdateAsync(entity);
    }
}