using AngleSharp.Dom;
using Lazy.SlideCaptcha.Core.Validator;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities.Encoders;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Auth;
using DaLang.Lims.Web.Framework.Core.Captcha;
using DaLang.Lims.Web.Framework.Core.Configs;
using DaLang.Lims.Web.Framework.Core.Consts;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain.Permission;
using DaLang.Lims.Web.Framework.Domain.PkgPermission;
using DaLang.Lims.Web.Framework.Domain.RolePermission;
using DaLang.Lims.Web.Framework.Domain.Tenant;
using DaLang.Lims.Web.Framework.Domain.TenantPermission;
using DaLang.Lims.Web.Framework.Domain.TenantPkg;
using DaLang.Lims.Web.Framework.Domain.User;
using DaLang.Lims.Web.Framework.Domain.UserRole;
using DaLang.Lims.Web.Framework.Services.Auth.Dto;
using DaLang.Lims.Web.Framework.Services.LoginLog;
using DaLang.Lims.Web.Framework.Services.LoginLog.Dto;
using DaLang.Lims.Web.Framework.Services.User;
using DaLang.Lims.Web.Common.Extensions;
using DaLang.Lims.Web.Common.Helpers;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using static Lazy.SlideCaptcha.Core.ValidateResult;
using DaLang.Lims.Web.Framework.Domain.View;
using DaLang.Lims.Web.Framework.Services.Permission.Dto;

namespace DaLang.Lims.Web.Framework.Services.Auth;

/// <summary>
/// 认证授权服务
/// </summary>
[DynamicApi(Area = AdminConsts.AreaName)]
public class AuthService : BaseService, IAuthService, IDynamicApi
{

    private readonly Lazy<IOptions<AppConfig>> _appConfig;
    private readonly Lazy<IOptions<JwtConfig>> _jwtConfig;
    private readonly Lazy<IUserRepository> _userRep;
    private readonly Lazy<ITenantRepository> _tenantRep;
    private readonly Lazy<IPermissionRepository> _permissionRep;
    private readonly Lazy<IPasswordHasher<UserEntity>> _passwordHasher;
    private readonly Lazy<ISlideCaptcha> _captcha;
    private readonly Lazy<IHttpContextAccessor> _accessor;
    private readonly ITenantPkgRepository _tenantPkgRep;

    public AuthService(
        Lazy<IOptions<AppConfig>> appConfig,
        Lazy<IOptions<JwtConfig>> jwtConfig,
        Lazy<IUserRepository> userRep,
        Lazy<ITenantRepository> tenantRep,
        Lazy<IPermissionRepository> permissionRep,
        Lazy<IPasswordHasher<UserEntity>> passwordHasher,
        Lazy<ISlideCaptcha> captcha,
        Lazy<IHttpContextAccessor> accessor,
        ITenantPkgRepository tenantPkgRep
    )
    {
        _appConfig = appConfig;
        _jwtConfig = jwtConfig;
        _userRep = userRep;
        _tenantRep = tenantRep;
        _permissionRep = permissionRep;
        _passwordHasher = passwordHasher;
        _captcha = captcha;
        _accessor = accessor;
        _tenantPkgRep = tenantPkgRep;
    }

    /// <summary>
    /// 获得token
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <returns></returns>
    [NonAction]
    public string GetToken(AuthLoginOutput user)
    {
        if (user == null)
        {
            return string.Empty;
        }

        var claims = new List<Claim>()
       {
            new Claim(ClaimAttributes.UserId, user.Id.ToString(), ClaimValueTypes.Integer64),
            new Claim(ClaimAttributes.UserName, user.UserName),
            new Claim(ClaimAttributes.Name, user.Name),
            new Claim(ClaimAttributes.UserType, user.Type.ToInt().ToString(), ClaimValueTypes.Integer32),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToTimestamp().ToString(), ClaimValueTypes.Integer64),
        };

        if (_appConfig.Value.Value.Tenant)
        {
            claims.AddRange(new[]
            {
                new Claim(ClaimAttributes.TenantId, user.TenantId.ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimAttributes.TenantType, user.Tenant?.TenantType.ToInt().ToString(), ClaimValueTypes.Integer32),
                new Claim(ClaimAttributes.DbKey, user.Tenant?.DbKey ?? "")
            });
        }

        var token = LazyGetRequiredService<IUserToken>().Create(claims.ToArray());



        return token;
    }

    /// <summary>
    /// 查询密钥
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    [NoOprationLog]
    public async Task<AuthGetPasswordEncryptKeyOutput> GetPasswordEncryptKeyAsync()
    {
        //写入Redis
        var guid = Guid.NewGuid().ToString("N");
        var key = CacheKeys.PassWordEncrypt + guid;
        //创建key
        byte[] keyBytes = Encoding.Default.GetBytes(StringHelper.GenerateRandom(16));
        string keyHexString = BitConverter.ToString(keyBytes);
        var encryptKey = keyHexString.Replace("-", "").ToLower();
        //创建iv
        byte[] ivBytes = Encoding.Default.GetBytes(StringHelper.GenerateRandom(16));
        string ivHexString = BitConverter.ToString(ivBytes);
        var iv = ivHexString.Replace("-", "").ToLower();
        //输出
        var passwordKeyOutput = new AuthGetPasswordEncryptKeyOutput { Key = guid, EncryptKey = encryptKey, Iv = iv };
        //写缓存
        await Cache.SetAsync(key, passwordKeyOutput, TimeSpan.FromMinutes(5));
        return passwordKeyOutput;
    }

    /// <summary>
    /// 查询用户个人信息
    /// </summary>
    /// <returns></returns>
    [Login]
    public async Task<AuthUserProfileDto> GetUserProfileAsync()
    {
        if (!(User?.Id > 0))
        {
            throw ResultOutput.Exception("未登录");
        }

        var userRep = _userRep.Value;

        //using (userRep.DataFilter.Disable(FilterNames.Self, FilterNames.Data))
        //{
        var profile = await userRep.GetAsync(User.Id);

        return profile.Adapt<AuthUserProfileDto>();
        //}
    }
    //public List<AuthUserMenuDto> FlattenTreeLinq(List<PermissionEntity> nodes)
    //{
    //    if (nodes == null) return new List<AuthUserMenuDto>();

    //    var allList =  nodes.SelectMany(node => new List<PermissionEntity>{ node } .Concat(FlattenTreeLinq(node.Childs ?? new List<PermissionEntity>()))
    //    ).ToList();


    //}
    public List<AuthUserMenuDto> FlattenTreeRecursive(List<PermissionEntity> nodes)
    {
        var result = new List<AuthUserMenuDto>();

        if (nodes == null) return result;

        foreach (var node in nodes)
        {
            var tmp = node.Adapt<AuthUserMenuDto>();
            tmp.ViewPath = node.View?.Path;
            result.Add(tmp);
            if (node.Childs != null && node.Childs.Any())
                result.AddRange(FlattenTreeRecursive(node.Childs));
        }

        return result;
    }

    /// <summary>
    /// 查询用户菜单列表
    /// </summary>
    /// <returns></returns>
    [Login]
    public async Task<List<AuthUserMenuDto>> GetUserMenusAsync()
    {
        if (!(User?.Id > 0))
        {
            throw ResultOutput.Exception("未登录");
        }

        //using (_userRep.Value.DataFilter.Disable(FilterNames.Self, FilterNames.Data))
        //{
        var permissionRep = _permissionRep.Value;
        var menuSelect = permissionRep.AsQueryable();

        if (!User.PlatformAdmin)
        {
            var db = permissionRep.Context;
            if (User.TenantAdmin)
            {
                // menuSelect = menuSelect.Where(a =>
                //    db.Queryable<TenantPermissionEntity>()
                //    .Where(b => b.PermissionId == a.Id && b.TenantId == User.TenantId)
                //    .Any()
                //    ||
                //    db.Queryable<TenantPkgEntity, PkgPermissionEntity>((b, c) => b.PkgId == c.PkgId && b.TenantId == User.TenantId && c.PermissionId == a.Id)
                //    //.Where((b, c) => b.PkgId == c.PkgId && b.TenantId == User.TenantId && c.PermissionId == a.Id)
                //    .Any()
                //);
                // menuSelect= menuSelect.Where(a =>
                //    SqlFunc.Subqueryable<TenantPkgEntity>()
                //    .InnerJoin<PkgPermissionEntity>((b, c) => b.PkgId == c.PkgId && b.TenantId == User.TenantId)
                //    .Where((b, c) => c.PermissionId == a.Id)
                //    .Any()
                //);

                //var permissions = await menuSelect.InnerJoin<PkgPermissionEntity>((a, b) => a.Id == b.PermissionId)
                //    .InnerJoin<TenantPkgEntity>((a, b, c) => b.PkgId == c.PkgId && c.TenantId == User.TenantId)
                //    .ToListAsync();

                var permissionList = await _tenantPkgRep.AsQueryable().InnerJoin<PkgPermissionEntity>((a, b) => a.PkgId == b.PkgId && a.TenantId == User.TenantId)
                      .InnerJoin<PermissionEntity>((a, b, c) => b.PermissionId == c.Id)
                      .Select((a, b, c) => c.Id).ToListAsync();

                var menuList1 = await menuSelect
                                    //.Where(a => new[] { PermissionType.Group, PermissionType.Menu }.Contains(a.Type))
                                    .Includes(a => a.View)
                                    .Includes(a => a.Childs)
                                    .Where(a => a.Type != PermissionType.Api)
                                    //.Select(a => new AuthUserMenuDto { ViewPath = a.View.Path }, true)

                                    .ToTreeAsync(p => p.Childs, p => p.ParentId, 0, childIds: permissionList.Select(x => (object)x).ToArray());

                var ret = FlattenTreeRecursive(menuList1);
                ret.RemoveAll(v => permissionList.Contains(v.Id));
                return ret.DistinctBy(a => a.Id).OrderBy(a => a.ParentId).ThenBy(a => a.Sort).ToList();
            }
            else
            {
                //TODO 需要把parent级别也取出来
                // menuSelect = menuSelect.Where(a =>
                //    SqlFunc.Subqueryable<RolePermissionEntity>()
                //    .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == User.Id)
                //    .Where(b => b.PermissionId == a.Id)
                //    .Any()
                //);

                menuSelect = menuSelect.Where(a =>
                   SqlFunc.Subqueryable<RolePermissionEntity>()
                   .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == User.Id)
                   .Where(b => b.PermissionId == a.Id)
                   .Any());

                var currPermission = menuSelect.ToList();
                if (currPermission.Any())
                {
                    var tmpIds = currPermission.Select(a => a.Id).ToArray();
                    object[] ids = new object[tmpIds.Length];
                    tmpIds.CopyTo(ids, 0);

                    //var permissions = permissionRep.AsQueryable()
                    //    //.Where(a => new[] { PermissionType.Group, PermissionType.Menu }.Contains(a.Type))
                    //    .Includes(a => a.View)
                    //    .Includes(a => a.Childs)
                    //    .ToParentList(it => it.ParentId, ids);


                    //object[] inIds = [161227168632902, 161227168792645, 161227168792646, 187375358951493, 187389970825285, 187390547820613, 187391371018309, 187391980761157, 551712205733957];
                    var permissions = permissionRep.AsQueryable()
                        //.Where(a => new[] { PermissionType.Group, PermissionType.Menu }.Contains(a.Type))
                        .Includes(a => a.View)
                        //.Includes(a => a.Childs)
                        .ToTree(it => it.Childs, it => it.ParentId, rootValue: 0, childIds: ids);


                    var list = new List<PermissionEntity>();
                    foreach (var item in permissions)
                    {
                        list.Add(item);
                        AddNodeToList(item, list);
                    }

                    void AddNodeToList(PermissionEntity node, List<PermissionEntity> list)
                    {
                        list.Add(node);
                        if (node.Childs != null)
                        {
                            foreach (var child in node.Childs)
                            {
                                AddNodeToList(child, list);
                            }
                        }
                    }

                    list = list.FindAll(a => new[] { PermissionType.Group, PermissionType.Menu }.Contains(a.Type)).DistinctBy(a => a.Id).OrderBy(a => a.ParentId).ThenBy(a => a.Sort).ToList();

                    var retList = new List<AuthUserMenuDto>();
                    list.ForEach(a =>
                    {
                        var curr = a.Adapt<AuthUserMenuDto>();
                        curr.ViewPath = a.View?.Path;
                        retList.Add(curr);
                    });
                    return retList.DistinctBy(a => a.Id).OrderBy(a => a.ParentId).ThenBy(a => a.Sort).ToList();
                }
            }

            //menuSelect = menuSelect.ToTree(o => o.Childs, o => o.ParentId, 0, o => o.Id);
        }

        //var a = menuSelect
        //    .Where(a => new[] { PermissionType.Group, PermissionType.Menu }.Contains(a.Type))
        //    .Includes(a => a.View).ToSql();

        //var menuList = await menuSelect
        //    .Where(a => new[] { PermissionType.Group, PermissionType.Menu }.Contains(a.Type))
        //    .Includes(a => a.View)
        //    .Includes(a => a.Childs)
        //    //.Select(a => new AuthUserMenuDto { ViewPath = a.View.Path }, true)
        //    .ToTreeAsync(p => p.Childs, p => p.ParentId, 0);

        //var list222 = menuList.Select(a => new AuthUserMenuDto { ViewPath = a.View.Path });


        //.Select(a => new AuthUserMenuDto { ViewPath = a.View.Path }, true)

        //var b = await menuSelect
        //    .Where(a => new[] { PermissionType.Group, PermissionType.Menu }.Contains(a.Type))
        //    .Includes(a => a.View)
        //    //.Includes(a => a.Childs)
        //    .ToListAsync();


        var menuList = await menuSelect
            .Where(a => new[] { PermissionType.Group, PermissionType.Menu }.Contains(a.Type))
            .Includes(a => a.View)
            .Includes(a => a.Childs)
            .Select(a => new AuthUserMenuDto { ViewPath = a.View.Path }, true)
            .ToListAsync();

        //var list222 = new List<AuthUserMenuDto>();// menuList.Select(a => new AuthUserMenuDto { ViewPath = a.View.Path });

        return menuList.DistinctBy(a => a.Id).OrderBy(a => a.ParentId).ThenBy(a => a.Sort).ToList();

        //}
    }


    /// <summary>
    /// 查询用户权限列表
    /// </summary>
    /// <returns></returns>
    [Login]
    public async Task<AuthGetUserPermissionsOutput> GetUserPermissionsAsync()
    {
        if (!(User?.Id > 0))
        {
            throw ResultOutput.Exception("未登录");
        }

        var userRep = _userRep.Value;
        var permissionRep = _permissionRep.Value;

        //using (userRep.DataFilter.Disable(FilterNames.Self, FilterNames.Data))
        //{
        var user = await userRep.GetAsync(User.Id);
        var authGetUserPermissionsOutput = new AuthGetUserPermissionsOutput
        {
            //用户信息
            User = user.Adapt<AuthUserProfileDto>()
        };

        var dotSelect = permissionRep.AsQueryable().Where(a => a.Type == PermissionType.Dot);

        if (!User.PlatformAdmin)
        {
            var db = permissionRep.Context;
            if (User.TenantAdmin)
            {
                //dotSelect = dotSelect.Where(a =>
                //   db.Queryable<TenantPermissionEntity>()
                //   .Where(b => b.PermissionId == a.Id && b.TenantId == User.TenantId)
                //   .Any()
                //   ||
                //   db.Queryable<TenantPkgEntity>()
                //   .InnerJoin<PkgPermissionEntity>((b, c) => b.PkgId == c.PkgId)
                //   .Where((b, c) => b.TenantId == User.TenantId && c.PermissionId == a.Id)
                //   .Any()
                //);
                dotSelect = dotSelect.Where(a =>
                   SqlFunc.Subqueryable<TenantPermissionEntity>()
                   .Where(b => b.PermissionId == a.Id && b.TenantId == User.TenantId)
                   .Any()
                   ||
                   SqlFunc.Subqueryable<TenantPkgEntity>()
                   .InnerJoin<PkgPermissionEntity>((b, c) => b.PkgId == c.PkgId)
                   .Where((b, c) => b.TenantId == User.TenantId && c.PermissionId == a.Id)
                   .Any()
                );
            }
            else
            {
                dotSelect = dotSelect.Where(a =>
                    SqlFunc.Subqueryable<RolePermissionEntity>()
                    .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == User.Id)
                    .Where(b => b.PermissionId == a.Id)
                    .Any()
                );
            }
        }

        //用户权限点
        authGetUserPermissionsOutput.Permissions = await dotSelect.ToListAsync(a => a.Code);

        return authGetUserPermissionsOutput;
        //}
    }

    /// <summary>
    /// 查询用户信息
    /// </summary>
    /// <returns></returns>
    [Login]
    public async Task<AuthGetUserInfoOutput> GetUserInfoAsync()
    {
        if (!(User?.Id > 0))
        {
            throw ResultOutput.Exception("未登录");
        }

        var userRep = _userRep.Value;
        var permissionRep = _permissionRep.Value;

        //using (userRep.DataFilter.Disable(FilterNames.Self, FilterNames.Data))
        //{
        var user = await userRep.GetAsync(User.Id);
        var authGetUserInfoOutput = new AuthGetUserInfoOutput
        {
            //用户信息
            User = user.Adapt<AuthUserProfileDto>()
        };

        var menuSelect = permissionRep.AsQueryable();
        var dotSelect = menuSelect.Where(a => a.Type == PermissionType.Dot);

        if (!User.PlatformAdmin)
        {
            var db = permissionRep.Context;
            if (User.TenantAdmin)
            {
                menuSelect = menuSelect.Where(a =>
                   db.Queryable<TenantPermissionEntity>()
                   .Where(b => b.PermissionId == a.Id && b.TenantId == User.TenantId)
                   .Any()
                   ||
                   db.Queryable<TenantPkgEntity>()
                   .InnerJoin<PkgPermissionEntity>((b, c) => b.PkgId == c.PkgId)
                   .Where((b, c) => b.TenantId == User.TenantId && c.PermissionId == a.Id)
                   .Any()
               );

                dotSelect = dotSelect.Where(a =>
                   db.Queryable<TenantPermissionEntity>()
                   .Where(b => b.PermissionId == a.Id && b.TenantId == User.TenantId)
                   .Any()
                   ||
                   db.Queryable<TenantPkgEntity>()
                   .InnerJoin<PkgPermissionEntity>((b, c) => b.PkgId == c.PkgId)
                   .Where((b, c) => b.TenantId == User.TenantId && c.PermissionId == a.Id)
                   .Any()
                );
            }
            else
            {
                menuSelect = menuSelect.Where(a =>
                   db.Queryable<RolePermissionEntity>()
                   .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == User.Id)
                   .Where(b => b.PermissionId == a.Id)
                   .Any()
               );

                dotSelect = dotSelect.Where(a =>
                    db.Queryable<RolePermissionEntity>()
                    .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == User.Id)
                    .Where(b => b.PermissionId == a.Id)
                    .Any()
                );
            }

            //menuSelect = menuSelect.AsTreeCte(up: true);
        }

        var menuList = await menuSelect
            .Where(a => new[] { PermissionType.Group, PermissionType.Menu }.Contains(a.Type))
            .ToListAsync(a => new AuthUserMenuDto { ViewPath = a.View.Path });

        //用户菜单
        authGetUserInfoOutput.Menus = menuList.DistinctBy(a => a.Id).OrderBy(a => a.ParentId).ThenBy(a => a.Sort).ToList();

        //用户权限点
        authGetUserInfoOutput.Permissions = await dotSelect.ToListAsync(a => a.Code);

        return authGetUserInfoOutput;
        //}
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [NoOprationLog]
    public async Task<dynamic> LoginAsync(AuthLoginInput input)
    {
        var userRep = _userRep.Value;

        var stopwatch = Stopwatch.StartNew();

        #region 验证码校验

        if (_appConfig.Value.Value.VarifyCode.Enable)
        {
            if (input.CaptchaId.IsNull() || input.CaptchaData.IsNull())
            {
                throw ResultOutput.Exception("请完成安全验证");
            }
            var validateResult = _captcha.Value.Validate(input.CaptchaId, JsonConvert.DeserializeObject<SlideTrack>(input.CaptchaData));
            if (validateResult.Result != ValidateResultType.Success)
            {
                throw ResultOutput.Exception($"安全{validateResult.Message}，请重新登录");
            }
        }

        #endregion

        #region 密码解密

        if (input.PasswordKey.NotNull())
        {
            var passwordEncryptKey = CacheKeys.PassWordEncrypt + input.PasswordKey;
            var existsPasswordKey = await Cache.ExistsAsync(passwordEncryptKey);
            if (existsPasswordKey)
            {
                var secretKey = await Cache.GetAsync<AuthGetPasswordEncryptKeyOutput>(passwordEncryptKey);
                if (secretKey.EncryptKey.IsNull())
                {
                    throw ResultOutput.Exception("解密失败");
                }
                input.Password = SM4Encryption.Decrypt(input.Password, Hex.Decode(secretKey.EncryptKey), Hex.Decode(secretKey.Iv), "CBC", true).TrimEnd('\0');//SM4解密后会有\0符号，需要去除。
                await Cache.DelAsync(passwordEncryptKey);
            }
            else
            {
                throw ResultOutput.Exception("解密失败");
            }
        }

        #endregion

        #region 登录
        var user = await userRep.AsQueryable()
            .Where(a => a.UserName == input.UserName)
            .Includes(a => a.Tenant)
            .FirstAsync();
        var valid = user?.Id > 0;
        if (valid)
        {
            if (user.PasswordEncryptType == PasswordEncryptType.PasswordHasher)
            {
                var passwordVerificationResult = _passwordHasher.Value.VerifyHashedPassword(user, user.Password, input.Password);
                valid = passwordVerificationResult == PasswordVerificationResult.Success || passwordVerificationResult == PasswordVerificationResult.SuccessRehashNeeded;
            }
            else
            {
                var password = MD5Encrypt.Encrypt32(input.Password);
                valid = user.Password == password;
                //valid = true;
            }
        }

        if (!valid)
        {
            throw ResultOutput.Exception("用户名或密码错误");
        }

        if (!user.IsValid)
        {
            throw ResultOutput.Exception("账号已停用，禁止登录");
        }
        #endregion

        #region 获得token
        var authLoginOutput = Mapper.Map<AuthLoginOutput>(user);
        if (_appConfig.Value.Value.Tenant)
        {
            var tenant = await _tenantRep.Value.AsQueryable().FirstAsync();
            if (!(tenant != null && tenant.IsValid))
            {
                throw ResultOutput.Exception("企业已停用，禁止登录");
            }
            authLoginOutput.Tenant = tenant.Adapt<AuthLoginTenantDto>();
        }
        string token = GetToken(authLoginOutput);
        #endregion

        stopwatch.Stop();

        #region 添加登录日志

        var loginLogAddInput = new LoginLogAddInput
        {
            TenantId = authLoginOutput.TenantId,
            Name = authLoginOutput.Name,
            ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
            Status = true,
            ProId = authLoginOutput.Id,
            ProName = user.UserName,
        };

        await LazyGetRequiredService<ILoginLogService>().AddAsync(loginLogAddInput);

        #endregion 添加登录日志

        return new { token };
    }

    /// <summary>
    /// 手机号登录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [NoOprationLog]
    public async Task<dynamic> MobileLoginAsync(AuthMobileLoginInput input)
    {
        var userRep = _userRep.Value;

        //using var _ = userRep.DataFilter.DisableAll();
        //using var __ = userRep.DataFilter.Enable(FilterNames.Delete);

        var stopwatch = Stopwatch.StartNew();

        #region 短信验证码验证
        if (input.CodeId.IsNull() || input.Code.IsNull())
        {
            throw ResultOutput.Exception("验证码错误");
        }
        var codeKey = CacheKeys.GetSmsCodeKey(input.Mobile, input.CodeId);
        var code = await Cache.GetAsync(codeKey);
        if (code.IsNull())
        {
            throw ResultOutput.Exception("验证码错误");
        }
        await Cache.DelAsync(codeKey);
        if (code != input.Code)
        {
            throw ResultOutput.Exception("验证码错误");
        }

        #endregion

        #region 登录
        var user = await userRep.AsQueryable().Where(a => a.Mobile == input.Mobile).FirstAsync();
        if (!(user?.Id > 0))
        {
            throw ResultOutput.Exception("账号不存在");
        }

        if (!user.IsValid)
        {
            throw ResultOutput.Exception("账号已停用，禁止登录");
        }
        #endregion

        #region 获得token
        var authLoginOutput = Mapper.Map<AuthLoginOutput>(user);
        if (_appConfig.Value.Value.Tenant)
        {
            var tenant = await _tenantRep.Value.AsQueryable().FirstAsync();
            if (!(tenant != null && tenant.IsValid))
            {
                throw ResultOutput.Exception("企业已停用，禁止登录");
            }
            authLoginOutput.Tenant = tenant.Adapt<AuthLoginTenantDto>();
        }
        string token = GetToken(authLoginOutput);
        #endregion

        stopwatch.Stop();

        #region 添加登录日志

        var loginLogAddInput = new LoginLogAddInput
        {
            TenantId = authLoginOutput.TenantId,
            Name = authLoginOutput.Name,
            ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
            Status = true,
            ProId = authLoginOutput.Id,
            ProName = user.UserName,
        };

        await LazyGetRequiredService<ILoginLogService>().AddAsync(loginLogAddInput);

        #endregion 添加登录日志

        return new { token };
    }

    /// <summary>
    /// 刷新Token
    /// 以旧换新
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<dynamic> Refresh([BindRequired] string token)
    {
        var jwtSecurityToken = LazyGetRequiredService<IUserToken>().Decode(token);
        var userClaims = jwtSecurityToken?.Claims?.ToArray();
        if (userClaims == null || userClaims.Length == 0)
        {
            throw ResultOutput.Exception("无法解析token");
        }

        var refreshExpires = userClaims.FirstOrDefault(a => a.Type == ClaimAttributes.RefreshExpires)?.Value;
        if (refreshExpires.IsNull() || refreshExpires.ToLong() <= DateTime.Now.ToTimestamp())
        {
            throw ResultOutput.Exception("登录信息已过期");
        }

        var userId = userClaims.FirstOrDefault(a => a.Type == ClaimAttributes.UserId)?.Value;
        if (userId.IsNull())
        {
            throw ResultOutput.Exception("登录信息已失效");
        }

        //验签
        var securityKey = _jwtConfig.Value.Value.SecurityKey;
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKey)), SecurityAlgorithms.HmacSha256);
        var input = jwtSecurityToken.RawHeader + "." + jwtSecurityToken.RawPayload;
        if (jwtSecurityToken.RawSignature != JwtTokenUtilities.CreateEncodedSignature(input, signingCredentials))
        {
            throw ResultOutput.Exception("验签失败");
        }

        var user = await LazyGetRequiredService<IUserService>().GetLoginUserAsync(userId.ToLong());
        if (!(user?.Id > 0))
        {
            throw ResultOutput.Exception("账号不存在");
        }
        if (!user.IsValid)
        {
            throw ResultOutput.Exception("账号已停用，禁止登录");
        }

        if (_appConfig.Value.Value.Tenant)
        {
            if (!(user.Tenant != null && user.Tenant.IsValid))
            {
                throw ResultOutput.Exception("企业已停用，禁止登录");
            }
        }

        string newToken = GetToken(user);
        return new { token = newToken };
    }

    /// <summary>
    /// 是否开启验证码
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    [NoOprationLog]
    public bool IsCaptcha()
    {
        return _appConfig.Value.Value.VarifyCode.Enable;
    }
}