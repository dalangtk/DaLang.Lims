using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain.User.Dto;
using DaLang.Lims.Web.Framework.Services.Auth.Dto;
using DaLang.Lims.Web.Framework.Services.User.Dto;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DaLang.Lims.Web.Framework.Services.User;

/// <summary>
/// 用户接口
/// </summary>
public interface IUserService
{
    Task<UserGetOutput> GetAsync(long id);

    Task<PageOutput<UserGetPageOutput>> GetPageAsync(PageInput<UserGetPageDto> input);

    Task<AuthLoginOutput> GetLoginUserAsync(long id);

    Task<DataPermissionDto> GetDataPermissionAsync(string? apiPath);

    Task<long> AddAsync(UserAddInput input);

    Task<long> AddMemberAsync(UserAddMemberInput input);

    Task UpdateAsync(UserUpdateInput input);

    Task DeleteAsync(long id);

    Task ChangePasswordAsync(UserChangePasswordInput input);

    Task<string> ResetPasswordAsync(UserResetPasswordInput input);

    Task SetManagerAsync(UserSetManagerInput input);

    Task UpdateBasicAsync(UserUpdateBasicInput input);

    Task<UserGetBasicOutput> GetBasicAsync();

    Task<UserGetPermissionOutput> GetPermissionAsync();

    Task<string> AvatarUpload(IFormFile file, bool autoUpdate = false);

    Task<dynamic> OneClickLoginAsync(string userName);
}