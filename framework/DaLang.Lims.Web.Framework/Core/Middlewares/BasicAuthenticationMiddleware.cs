using COSXML.Network;
using DaLang.Lims.Web.Common.Extensions;
using DaLang.Lims.Web.Framework.Core.Configs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DaLang.Lims.Web.Framework.Core.Middlewares;
public static class BasicAuthenticationScheme
{
    public const string DefaultScheme = "Basic";
}
public class BasicAuthenticationHandler(IOptionsMonitor<BasicAuthenticationConfig> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
    : AuthenticationHandler<BasicAuthenticationConfig>(options, logger, encoder)
{
    /// <summary>
    /// 认证
    /// </summary>
    /// <returns></returns>
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return AuthenticateResult.Fail("Missing Authorization Header");
        string username, password;
        try
        {
            AuthenticationHeaderValue authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            if (authHeader.Parameter == null)
            {
                return AuthenticateResult.Fail("Authorization Header is null");
            }
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
            username = credentials[0];
            password = credentials[1];
            var isValidUser = IsAuthorized(username, password);
            if (isValidUser == false)
            {
                return AuthenticateResult.Fail("Invalid username or password");
            }
        }
        catch
        {
            return AuthenticateResult.Fail("Invalid Authorization Header");
        }

        var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier,username),
                new Claim(ClaimTypes.Name,username),
            };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return await Task.FromResult(AuthenticateResult.Success(ticket));
    }

    /// <summary>
    /// 质询
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.Headers["WWW-Authenticate"] = $"Basic realm=\"{Options.Realm}\"";
        await base.HandleChallengeAsync(properties);
    }

    /// <summary>
    /// 认证失败
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        await base.HandleForbiddenAsync(properties);
    }

    private bool IsAuthorized(string username, string password)
    {
        return username.Equals(base.Options.UserName, StringComparison.InvariantCultureIgnoreCase)
               && password.Equals(base.Options.UserPassword);
    }
}

// HTTP基本认证Middleware
public static class BasicAuthentication
{
    /// <summary>
    /// 资源认证
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseBasicAuthentication(this IApplicationBuilder app)
    {
        var appConfig = AppInfo.GetOptions<AppConfig>();
        var optionsMonitor = appConfig.BasicAuthentication;
        if (optionsMonitor != null && optionsMonitor.ProtectPaths.Count > 0 && optionsMonitor.Enable)
        {
            Expression<Func<HttpContext, bool>> expression = u => false;
            foreach (var path in optionsMonitor.ProtectPaths)
                expression = expression.Or(x => x.Request.Path.StartsWithSegments(new PathString(path)));
            app.UseWhen(expression.Compile(), configuration: x => { x.UseMiddleware<BasicAuthenticationMiddleware>(); });
        }
        return app;
    }
}

public class BasicAuthenticationMiddleware(RequestDelegate next, ILogger<BasicAuthenticationMiddleware> logger)
{
    public async Task Invoke(HttpContext httpContext, IAuthenticationService authenticationService)
    {
        var authenticated = await authenticationService.AuthenticateAsync(httpContext, BasicAuthenticationScheme.DefaultScheme);
        logger.LogInformation("Access Status：" + authenticated.Succeeded);
        if (!authenticated.Succeeded)
        {
            await authenticationService.ChallengeAsync(httpContext, BasicAuthenticationScheme.DefaultScheme, new AuthenticationProperties { });
            return;
        }
        await next(httpContext);
    }
}
