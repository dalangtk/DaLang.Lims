using DaLang.Lims.Web.Common.Helpers;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Configs;
using DaLang.Lims.Web.Framework.Core.Consts;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Core.Helpers;
using DaLang.Lims.Web.Framework.Domain.Api;
using DaLang.Lims.Web.Framework.Domain.Org;
using DaLang.Lims.Web.Framework.Domain.Permission;
using DaLang.Lims.Web.Framework.Domain.PermissionApi;
using DaLang.Lims.Web.Framework.Domain.PkgPermission;
using DaLang.Lims.Web.Framework.Domain.Role;
using DaLang.Lims.Web.Framework.Domain.RoleOrg;
using DaLang.Lims.Web.Framework.Domain.RolePermission;
using DaLang.Lims.Web.Framework.Domain.Tenant;
using DaLang.Lims.Web.Framework.Domain.TenantPermission;
using DaLang.Lims.Web.Framework.Domain.TenantPkg;
using DaLang.Lims.Web.Framework.Domain.User;
using DaLang.Lims.Web.Framework.Domain.User.Dto;
using DaLang.Lims.Web.Framework.Domain.UserOrg;
using DaLang.Lims.Web.Framework.Domain.UserRole;
using DaLang.Lims.Web.Framework.Domain.UserStaff;
using DaLang.Lims.Web.Framework.Services.Auth;
using DaLang.Lims.Web.Framework.Services.Auth.Dto;
using DaLang.Lims.Web.Framework.Services.User.Dto;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DaLang.Lims.Web.Framework.Services.User;

/// <summary>
/// 用户服务
/// </summary>
[Order(10)]
[DynamicApi(Area = AdminConsts.AreaName)]
public partial class UserService : BaseService, IUserService, IDynamicApi
{
    private readonly IUserRepository _userRep;
    private readonly IUserOrgRepository _userOrgRep;
    private readonly IUserRoleRepository _userRoleRep;
    private readonly IUserStaffRepository _userStaffRep;
    private readonly AppConfig _appConfig;
    private readonly UserHelper _userHelper;
    private readonly Lazy<IPasswordHasher<UserEntity>> _passwordHasher;
    private readonly Lazy<IRoleRepository> _roleRep;
    private readonly Lazy<IRolePermissionRepository> _rolePermissionRep;
    private readonly Lazy<IFileService> _fileService;
    private readonly Lazy<IRoleOrgRepository> _roleOrgRep;
    private readonly Lazy<IApiRepository> _apiRep;
    private readonly Lazy<ITenantRepository> _tenantRep;
    private readonly Lazy<IOrgRepository> _orgRep;
    private readonly Lazy<IPermissionRepository> _permissionRep;

    public UserService(
        IUserRepository userRep,
        IUserOrgRepository userOrgRep,
        IUserRoleRepository userRoleRep,
        IUserStaffRepository userStaffRep,
        AppConfig appConfig,
        UserHelper userHelper,
        Lazy<IPasswordHasher<UserEntity>> passwordHasher,
        Lazy<IRoleRepository> roleRep,
        Lazy<IRolePermissionRepository> rolePermissionRep,
        Lazy<IFileService> fileService,
        Lazy<IRoleOrgRepository> roleOrgRep,
        Lazy<IApiRepository> apiRep,
        Lazy<ITenantRepository> tenantRep,
        Lazy<IOrgRepository> orgRep,
        Lazy<IPermissionRepository> permissionRep
    )
    {
        _appConfig = appConfig;
        _userHelper = userHelper;
        _passwordHasher = passwordHasher;
        _roleRep = roleRep;
        _rolePermissionRep = rolePermissionRep;
        _fileService = fileService;
        _userRep = userRep;
        _userOrgRep = userOrgRep;
        _roleOrgRep = roleOrgRep;
        _userRoleRep = userRoleRep;
        _userStaffRep = userStaffRep;
        _apiRep = apiRep;
        _tenantRep = tenantRep;
        _orgRep = orgRep;
        _permissionRep = permissionRep;
    }

    /// <summary>
    /// 查询用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<UserGetOutput> GetAsync(long id)
    {
        var userEntity = await _userRep.AsQueryable()
        .Where(a => a.Id == id)
        .Includes(a => a.Roles)
        .Includes(a => a.Orgs)
        .Includes(a => a.Staff)
        .Includes(a => a.ManagerUser)
        .FirstAsync();
        var output = Mapper.Map<UserGetOutput>(userEntity);
        return output;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<UserGetPageOutput>> GetPageAsync(PageInput<UserGetPageDto> input)
    {
        var dataPermission = User.DataPermission;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);

        var orgId = input.Filter?.OrgId;
        var list = await _userRep.GetQueryable(dynamicCondition)
            .WhereIF(dataPermission != null && dataPermission.OrgIds.Count > 0, a => SqlFunc.Subqueryable<UserOrgEntity>().Where(b => b.UserId == a.Id && dataPermission.OrgIds.Contains(b.OrgId)).Any())
            .WhereIF(dataPermission != null && dataPermission.DataScope == DataScope.Self, a => a.ProId == User.Id)
            .WhereIF(orgId.HasValue && orgId > 0, a => SqlFunc.Subqueryable<UserOrgEntity>().Where(b => b.UserId == a.Id && b.OrgId == orgId).Any())
            .Where(a => a.Type != UserType.Member)
            .Includes(a => a.Roles)
            .Select<UserGetPageOutput>(o => new UserGetPageOutput { RoleNames = o.Roles.Select(a => a.Name).ToArray() }, isAutoFill: true)
            .ToPagedListAsync(input.CurrentPage, input.PageSize);

        //var retList = Mapper.Map<List<UserGetPageOutput>>(list.Items);
        if (orgId.HasValue && orgId > 0)
        {
            var managerUserIds = await _userOrgRep.AsQueryable()
                .Where(a => a.OrgId == orgId && a.IsManager == true).ToListAsync(a => a.UserId);

            if (managerUserIds.Any())
            {
                var managerUsers = list.Items.Where(a => managerUserIds.Contains(a.Id));
                foreach (var managerUser in managerUsers)
                {
                    managerUser.IsManager = true;
                }
            }
        }

        var data = new PageOutput<UserGetPageOutput>()
        {
            List = list.Items.ToList(),
            Total = list.Total
        };

        return data;
    }

    /// <summary>
    /// 查询登录用户信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<AuthLoginOutput> GetLoginUserAsync(long id)
    {
        var output = await _userRep.AsQueryable()
            .Where(a => a.Id == id)
            .Select<AuthLoginOutput>()
            .FirstAsync();

        if (_appConfig.Tenant && output?.TenantId.Value > 0)
        {
            var tenant = await _tenantRep.Value.AsQueryable()
                .Where(a => a.Id == output.TenantId)
                .Select<AuthLoginTenantDto>()
                .FirstAsync();

            output.Tenant = tenant;
        }
        return output;
    }

    /// <summary>
    /// 获得数据权限
    /// </summary>
    /// <param name="apiPath"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<DataPermissionDto> GetDataPermissionAsync(string? apiPath)
    {
        if (!(User?.Id > 0))
        {
            return null;
        }

        return await Cache.GetOrSetAsync(CacheKeys.GetDataPermissionKey(User.Id, apiPath), async () =>
        {
            var user = await _userRep.AsQueryable()
            .Where(a => a.Id == User.Id)
            .Select(a => new { a.OrgId })
            .FirstAsync();

            if (user == null)
                return null;

            var orgId = user.OrgId;

            //查询角色
            var roleRepository = _roleRep.Value;
            var rolePermissionRepository = _rolePermissionRep.Value;

            List<long> rolePermissionIds = new();
            if (apiPath.NotNull())
            {
                rolePermissionIds = await rolePermissionRepository.Context
                  .Queryable<RolePermissionEntity, PermissionApiEntity, ApiEntity>((a, b, c) =>
                       a.PermissionId == b.PermissionId && b.ApiId == c.Id && c.Path == apiPath)
                  .Select((a, b, c) => a.RoleId)
                   .ToListAsync();
            }

            var roles = await roleRepository.AsQueryable()
            .InnerJoin<UserRoleEntity>((q, k) => q.Id == k.RoleId && k.UserId == User.Id)
            .WhereIF(apiPath.NotNull(), q => rolePermissionIds.Contains(q.Id))
            .ToListAsync(q => new { q.Id, q.DataScope });

            //数据范围
            DataScope dataScope = DataScope.Self;
            var customRoleIds = new List<long>();
            roles?.ToList().ForEach(role =>
            {
                if (role.DataScope == DataScope.Custom)
                {
                    customRoleIds.Add(role.Id);
                }
                else if (role.DataScope <= dataScope)
                {
                    dataScope = role.DataScope;
                }
            });

            //部门列表
            var orgIds = new List<long>();
            if (dataScope != DataScope.All)
            {
                //本部门
                if (dataScope == DataScope.Dept)
                {
                    orgIds.Add(orgId);
                }
                //本部门和下级部门
                else if (dataScope == DataScope.DeptWithChild)
                {
                    orgIds = await _orgRep.Value.AsQueryable()
                    .Where(a => a.Id == orgId)
                    //.AsTreeCte()
                    .Select(a => a.Id)
                    .ToListAsync();
                }

                //指定部门
                if (customRoleIds.Count > 0)
                {
                    if (dataScope == DataScope.Self)
                    {
                        dataScope = DataScope.Custom;
                    }

                    var customRoleOrgIds = await _roleOrgRep.Value.AsQueryable()
                    .Where(a => customRoleIds.Contains(a.RoleId))
                    .ToListAsync(a => a.OrgId);

                    orgIds = orgIds.Concat(customRoleOrgIds).ToList();
                }
            }

            return new DataPermissionDto
            {
                OrgId = orgId,
                OrgIds = orgIds.Distinct().ToList(),
                DataScope = (User.PlatformAdmin || User.TenantAdmin) ? DataScope.All : dataScope
            };
        });
    }

    /// <summary>
    /// 查询用户基本信息
    /// </summary>
    /// <returns></returns>
    [Login]
    public async Task<UserGetBasicOutput> GetBasicAsync()
    {
        if (!(User?.Id > 0))
        {
            throw ResultOutput.Exception("未登录");
        }

        var ret = await _userRep.GetAsync(User.Id);

        var user = ret.Adapt<UserGetBasicOutput>();
        if (user == null)
        {
            throw ResultOutput.Exception("用户不存在");
        }

        user.Mobile = DataMaskHelper.PhoneMask(user.Mobile);
        user.Email = DataMaskHelper.EmailMask(user.Email);

        return user;
    }

    /// <summary>
    /// 查询用户权限信息
    /// </summary>
    /// <returns></returns>
    public async Task<UserGetPermissionOutput> GetPermissionAsync()
    {
        var key = CacheKeys.UserPermission + User.Id;
        var result = await Cache.GetOrSetAsync(key, async () =>
        {
            //if (User.TenantAdmin)
            //{
            //    var tenantPermissions = await _apiRep.Value.AsQueryable().Select<ApiEntity>()
            //    .Where(a => _apiRep.Value.Context.Queryable<TenantPermissionEntity, PermissionApiEntity>((b, c) => b.PermissionId == c.PermissionId && b.TenantId == User.TenantId)
            //    .Where((b, c) => c.ApiId == a.Id).Any())
            //    .Select<UserGetPermissionOutput>()
            //    .ToListAsync();

            //    var pkgPermissions = await _apiRep.Value.AsQueryable()
            //    .Where(a => _apiRep.Value.Context.Queryable<TenantPkgEntity, PkgPermissionEntity, PermissionApiEntity>((b, c, d) => b.PkgId == c.PkgId && c.PermissionId == d.PermissionId && b.TenantId == User.TenantId)
            //    .Where((b, c, d) => d.ApiId == a.Id).Any())
            //    .Select<UserGetPermissionOutput>()
            //    .ToListAsync();

            //    return tenantPermissions.Union(pkgPermissions).Distinct().ToList();
            //}

            //var list = await _apiRep.Value.Context
            //.Queryable<ApiEntity, UserRoleEntity, RolePermissionEntity, PermissionApiEntity>((a, b, c, d) => b.RoleId == c.RoleId
            //&& b.UserId == User.Id && c.PermissionId == d.PermissionId && d.ApiId == a.Id)
            //.Select<UserGetPermissionOutput>()
            //.ToListAsync();
            //return list;

            var output = new UserGetPermissionOutput();
            if (User.TenantAdmin)
            {
                //租户接口
                var tenantApis = await _apiRep.Value.Context
                .Queryable<ApiEntity, TenantPermissionEntity, PermissionApiEntity>((a, b, c) =>
                b.PermissionId == c.PermissionId && b.TenantId == User.TenantId && c.ApiId == a.Id && a.IsValid && !a.IsDeleted
                && !b.IsDeleted && !c.IsDeleted)
                .Select<UserGetPermissionOutput.Models.ApiModel>()
                .ToListAsync();

                //租户权限点编码
                var tenantCodes = await _permissionRep.Value.Context.Queryable<PermissionEntity, TenantPermissionEntity>((a, b) => b.PermissionId == a.Id
                && b.TenantId == User.TenantId && !b.IsDeleted && !a.IsDeleted)
                .Select((a, b) => a.Code)
                .ToListAsync();

                //套餐接口
                var pkgApis = await _apiRep.Value.Context.Queryable<ApiEntity, TenantPkgEntity, PkgPermissionEntity, PermissionApiEntity>((a, b, c, d) =>
               b.PkgId == c.PkgId && c.PermissionId == d.PermissionId && b.TenantId == User.TenantId && d.ApiId == a.Id)
                .Select<UserGetPermissionOutput.Models.ApiModel>()
                .ToListAsync();

                //套餐权限点编码

                var pkgCodes = await _permissionRep.Value.Context.Queryable<PermissionEntity, TenantPkgEntity, PkgPermissionEntity>((a, b, c) => b.PkgId == c.PkgId && c.PermissionId == a.Id && b.TenantId == User.TenantId && a.Type == PermissionType.Dot && !string.IsNullOrWhiteSpace(a.Code))
                .Select((a, b, c) => a.Code)
                .ToListAsync();

                output.Apis = tenantApis.Union(pkgApis).Distinct().ToList();

                output.Codes = tenantCodes.Union(pkgCodes).Distinct().ToList();

                return output;
            }

            //角色接口
            output.Apis = await _apiRep.Value.Context
            .Queryable<ApiEntity, UserRoleEntity, RolePermissionEntity, PermissionApiEntity>((a, b, c, d) => b.RoleId == c.RoleId
            && b.UserId == User.Id && c.PermissionId == d.PermissionId && d.ApiId == a.Id)
            .Select<UserGetPermissionOutput.Models.ApiModel>()
            .ToListAsync();

            //角色权限点编码
            output.Codes = await _permissionRep.Value.Context.Queryable<PermissionEntity, UserRoleEntity, RolePermissionEntity>((a, b, c) => b.RoleId == c.RoleId && b.UserId == User.Id && c.PermissionId == a.Id && a.Type == PermissionType.Dot && !string.IsNullOrWhiteSpace(a.Code))
            .Select((a, b, c) => a.Code)
            .ToListAsync();

            output.Codes = output.Codes.Distinct().ToList();

            return output;
        });
        return result;
    }

    /// <summary>
    /// 新增用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task<long> AddAsync(UserAddInput input)
    {
        //检查密码
        if (input.Password.IsNull())
        {
            input.Password = _appConfig.DefaultPassword;
        }
        _userHelper.CheckPassword(input.Password);

        var where = Expressionable.Create<UserEntity>()
            .And(a => a.UserName == input.UserName)
            .OrIF(input.Mobile.NotNull(), a => a.Mobile == input.Mobile)
            .OrIF(input.Email.NotNull(), a => a.Email == input.Email);

        var existsUser = await _userRep.AsQueryable().Where(where.ToExpression())
            .Select(a => new { a.UserName, a.Mobile, a.Email })
            .FirstAsync();

        if (existsUser != null)
        {
            if (existsUser.UserName == input.UserName)
            {
                throw ResultOutput.Exception("账号已存在");
            }

            if (input.Mobile.NotNull() && existsUser.Mobile == input.Mobile)
            {
                throw ResultOutput.Exception("手机号已存在");
            }

            if (input.Email.NotNull() && existsUser.Email == input.Email)
            {
                throw ResultOutput.Exception("邮箱已存在");
            }
        }

        // 用户信息
        var entity = Mapper.Map<UserEntity>(input);
        entity.Type = UserType.DefaultUser;
        if (_appConfig.PasswordHasher)
        {
            entity.Password = _passwordHasher.Value.HashPassword(entity, input.Password);
            entity.PasswordEncryptType = PasswordEncryptType.PasswordHasher;
        }
        else
        {
            entity.Password = MD5Encrypt.Encrypt32(input.Password);
            entity.PasswordEncryptType = PasswordEncryptType.MD5Encrypt32;
        }
        var user = await _userRep.InsertReturnEntityAsync(entity);
        var userId = user.Id;

        //用户角色
        if (input.RoleIds != null && input.RoleIds.Any())
        {
            var roles = input.RoleIds.Select(roleId => new UserRoleEntity
            {
                UserId = userId,
                RoleId = roleId
            }).ToList();
            await _userRoleRep.InsertRangeAsync(roles);
        }

        // 员工信息
        var staff = input.Staff == null ? new UserStaffEntity() : Mapper.Map<UserStaffEntity>(input.Staff);
        staff.Id = userId;
        staff.Sex = Sex.Unknown;
        staff.IsDeleted = false;
        await _userStaffRep.InsertAsync(staff);

        //所属部门
        if (input.OrgIds != null && input.OrgIds.Any())
        {
            var orgs = input.OrgIds.Select(orgId => new UserOrgEntity
            {
                UserId = userId,
                OrgId = orgId
            }).ToList();
            await _userOrgRep.InsertRangeAsync(orgs);
        }

        return userId;
    }

    /// <summary>
    /// 修改用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task UpdateAsync(UserUpdateInput input)
    {
        if (input.Id == input.ManagerUserId)
        {
            throw ResultOutput.Exception("直属主管不能是自己");
        }

        var where = Expressionable.Create<UserEntity>()
            .And(a => a.UserName == input.UserName)
            .OrIF(input.Mobile.NotNull(), a => a.Mobile == input.Mobile)
            .OrIF(input.Email.NotNull(), a => a.Email == input.Email);

        var existsUser = await _userRep.AsQueryable()
            .Where(a => a.Id != input.Id).Where(where.ToExpression())
            .Select(a => new { a.UserName, a.Mobile, a.Email })
            .FirstAsync();

        if (existsUser != null)
        {
            if (existsUser.UserName == input.UserName)
            {
                throw ResultOutput.Exception($"账号已存在");
            }

            if (input.Mobile.NotNull() && existsUser.Mobile == input.Mobile)
            {
                throw ResultOutput.Exception($"手机号已存在");
            }

            if (input.Email.NotNull() && existsUser.Email == input.Email)
            {
                throw ResultOutput.Exception($"邮箱已存在");
            }
        }

        var user = await _userRep.GetAsync(input.Id);
        if (!(user?.Id > 0))
        {
            throw ResultOutput.Exception("用户不存在");
        }

        Mapper.Map(input, user);
        await _userRep.UpdateAsync(user);

        var userId = user.Id;

        // 用户角色
        await _userRoleRep.DeleteAsync(a => a.UserId == userId);
        if (input.RoleIds != null && input.RoleIds.Any())
        {
            var roles = input.RoleIds.Select(roleId => new UserRoleEntity
            {
                UserId = userId,
                RoleId = roleId
            }).ToList();
            await _userRoleRep.InsertRangeAsync(roles);
        }

        // 员工信息
        var staff = await _userStaffRep.GetAsync(userId);
        var existsStaff = staff != null;
        staff ??= new UserStaffEntity();
        Mapper.Map(input.Staff, staff);
        staff.Id = userId;
        if (existsStaff)
        {
            await _userStaffRep.UpdateAsync(staff);
        }
        else
        {
            await _userStaffRep.InsertAsync(staff);
        }

        //所属部门
        var orgIds = await _userOrgRep.AsQueryable().Where(a => a.UserId == userId).ToListAsync(a => a.OrgId);
        var insertOrgIds = input.OrgIds.Except(orgIds);

        var deleteOrgIds = orgIds.Except(input.OrgIds);
        if (deleteOrgIds != null && deleteOrgIds.Any())
        {
            await _userOrgRep.DeleteAsync(a => a.UserId == userId && deleteOrgIds.Contains(a.OrgId));
        }

        if (insertOrgIds != null && insertOrgIds.Any())
        {
            var orgs = insertOrgIds.Select(orgId => new UserOrgEntity
            {
                UserId = userId,
                OrgId = orgId
            }).ToList();
            await _userOrgRep.InsertRangeAsync(orgs);
        }

        await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(userId));
    }

    /// <summary>
    /// 新增会员
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual async Task<long> AddMemberAsync(UserAddMemberInput input)
    {
        if (input.Password.IsNull())
        {
            input.Password = _appConfig.DefaultPassword;
        }
        _userHelper.CheckPassword(input.Password);

        var where = Expressionable.Create<UserEntity>()
            .And(a => a.UserName == input.UserName)
            .OrIF(input.Mobile.NotNull(), a => a.Mobile == input.Mobile)
            .OrIF(input.Email.NotNull(), a => a.Email == input.Email);

        var existsUser = await _userRep.AsQueryable().Where(where.ToExpression())
            .Select(a => new { a.UserName, a.Mobile, a.Email })
            .FirstAsync();

        if (existsUser != null)
        {
            if (existsUser.UserName == input.UserName)
            {
                throw ResultOutput.Exception($"账号已存在");
            }

            if (input.Mobile.NotNull() && existsUser.Mobile == input.Mobile)
            {
                throw ResultOutput.Exception($"手机号已存在");
            }

            if (input.Email.NotNull() && existsUser.Email == input.Email)
            {
                throw ResultOutput.Exception($"邮箱已存在");
            }
        }

        // 用户信息
        var entity = Mapper.Map<UserEntity>(input);
        entity.Type = UserType.Member;
        if (_appConfig.PasswordHasher)
        {
            entity.Password = _passwordHasher.Value.HashPassword(entity, input.Password);
            entity.PasswordEncryptType = PasswordEncryptType.PasswordHasher;
        }
        else
        {
            entity.Password = MD5Encrypt.Encrypt32(input.Password);
            entity.PasswordEncryptType = PasswordEncryptType.MD5Encrypt32;
        }
        var user = await _userRep.InsertReturnEntityAsync(entity);

        return user.Id;
    }

    /// <summary>
    /// 修改会员
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task UpdateMemberAsync(UserUpdateMemberInput input)
    {
        var where = Expressionable.Create<UserEntity>()
            .And(a => a.UserName == input.UserName)
            .OrIF(input.Mobile.NotNull(), a => a.Mobile == input.Mobile)
            .OrIF(input.Email.NotNull(), a => a.Email == input.Email);

        var existsUser = await _userRep.AsQueryable().Where(a => a.Id != input.Id).Where(where.ToExpression())
            .Select(a => new { a.UserName, a.Mobile, a.Email })
            .FirstAsync();

        if (existsUser != null)
        {
            if (existsUser.UserName == input.UserName)
            {
                throw ResultOutput.Exception($"账号已存在");
            }

            if (input.Mobile.NotNull() && existsUser.Mobile == input.Mobile)
            {
                throw ResultOutput.Exception($"手机号已存在");
            }

            if (input.Email.NotNull() && existsUser.Email == input.Email)
            {
                throw ResultOutput.Exception($"邮箱已存在");
            }
        }

        var user = await _userRep.GetAsync(input.Id);
        if (!(user?.Id > 0))
        {
            throw ResultOutput.Exception("用户不存在");
        }

        Mapper.Map(input, user);
        await _userRep.UpdateAsync(user);
    }

    /// <summary>
    /// 更新用户基本信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Login]
    public async Task UpdateBasicAsync(UserUpdateBasicInput input)
    {
        var entity = await _userRep.GetAsync(User.Id);
        entity = Mapper.Map(input, entity);
        await _userRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 修改用户密码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Login]
    public async Task ChangePasswordAsync(UserChangePasswordInput input)
    {
        if (input.ConfirmPassword != input.NewPassword)
        {
            throw ResultOutput.Exception("新密码和确认密码不一致");
        }

        _userHelper.CheckPassword(input.NewPassword);

        var entity = await _userRep.GetAsync(User.Id);
        var oldPassword = MD5Encrypt.Encrypt32(input.OldPassword);
        if (oldPassword != entity.Password)
        {
            throw ResultOutput.Exception("旧密码不正确");
        }

        if (entity.PasswordEncryptType == PasswordEncryptType.PasswordHasher)
        {
            entity.Password = _passwordHasher.Value.HashPassword(entity, input.NewPassword);
        }
        else
        {
            entity.Password = MD5Encrypt.Encrypt32(input.NewPassword);
        }
        await _userRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<string> ResetPasswordAsync(UserResetPasswordInput input)
    {
        var password = input.Password;
        if (password.IsNull())
        {
            password = _appConfig.DefaultPassword;
        }
        else
        {
            _userHelper.CheckPassword(password);
        }
        if (password.IsNull())
        {
            password = "123asd";
        }

        var entity = await _userRep.GetAsync(input.Id);
        if (_appConfig.PasswordHasher)
        {
            entity.Password = _passwordHasher.Value.HashPassword(entity, password);
            entity.PasswordEncryptType = PasswordEncryptType.PasswordHasher;
        }
        else
        {
            entity.Password = MD5Encrypt.Encrypt32(password);
            entity.PasswordEncryptType = PasswordEncryptType.MD5Encrypt32;
        }
        await _userRep.UpdateAsync(entity);
        return password;
    }

    /// <summary>
    /// 设置主管
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task SetManagerAsync(UserSetManagerInput input)
    {
        var entity = await _userOrgRep.AsQueryable().Where(a => a.UserId == input.UserId && a.OrgId == input.OrgId).FirstAsync();
        entity.IsManager = input.IsManager;
        await _userOrgRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 设置启用
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task SetEnableAsync(UserSetEnableInput input)
    {
        var entity = await _userRep.GetAsync(input.UserId);
        if (entity.Type == UserType.PlatformAdmin)
        {
            throw ResultOutput.Exception("平台管理员禁止禁用");
        }
        if (entity.Type == UserType.TenantAdmin)
        {
            throw ResultOutput.Exception("企业管理员禁止禁用");
        }
        entity.IsValid = input.IsValid;
        await _userRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        var user = await _userRep.AsQueryable().Where(a => a.Id == id).Select(a => new { a.Type }).FirstAsync();
        if (user == null)
        {
            throw ResultOutput.Exception("用户不存在");
        }

        if (user.Type == UserType.PlatformAdmin)
        {
            throw ResultOutput.Exception($"平台管理员禁止删除");
        }

        if (user.Type == UserType.TenantAdmin)
        {
            throw ResultOutput.Exception($"企业管理员禁止删除");
        }

        //删除用户角色
        await _userRoleRep.AsUpdateable().SetColumns(a => a.IsDeleted == true).Where(a => a.UserId == id).ExecuteCommandAsync();
        //删除用户所属部门
        await _userOrgRep.AsUpdateable().SetColumns(a => a.IsDeleted == true).Where(a => a.UserId == id).ExecuteCommandAsync();
        //删除员工
        await _userStaffRep.AsUpdateable().SetColumns(a => a.IsDeleted == true).Where(a => a.Id == id).ExecuteCommandAsync();
        //删除用户
        await _userRep.AsUpdateable().SetColumns(a => a.IsDeleted == true).Where(a => a.Id == id).ExecuteCommandAsync();

        //删除用户数据权限缓存
        await Cache.DelByPatternAsync(CacheKeys.GetDataPermissionPattern(id));
    }
    /// <summary>
    /// 上传头像
    /// </summary>
    /// <param name="file"></param>
    /// <param name="autoUpdate"></param>
    /// <returns></returns>
    [HttpPost]
    [Login]
    public async Task<string> AvatarUpload(IFormFile file, bool autoUpdate = false)
    {
        var fileInfo = await _fileService.Value.UploadFileAsync(file);
        if (autoUpdate)
        {
            var entity = await _userRep.GetAsync(User.Id);
            entity.Avatar = fileInfo.LinkUrl;
            await _userRep.UpdateAsync(entity);
        }
        return fileInfo.LinkUrl;
    }

    /// <summary>
    /// 一键登录用户
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<dynamic> OneClickLoginAsync([Required] string userName)
    {
        if (userName.IsNull())
        {
            throw ResultOutput.Exception("请选择用户");
        }

        var userRep = _userRep;
        var user = await userRep.AsQueryable().Where(a => a.UserName == userName).FirstAsync();

        if (user == null)
        {
            throw ResultOutput.Exception("用户不存在");
        }

        var authLoginOutput = Mapper.Map<AuthLoginOutput>(user);
        if (_appConfig.Tenant)
        {
            var tenant = await _tenantRep.Value.AsQueryable().Where(a => a.Id == user.TenantId).Select<AuthLoginTenantDto>().FirstAsync();
            authLoginOutput.Tenant = tenant;
        }

        string token = AppInfo.GetRequiredService<IAuthService>().GetToken(authLoginOutput);

        return new { token };
    }
}