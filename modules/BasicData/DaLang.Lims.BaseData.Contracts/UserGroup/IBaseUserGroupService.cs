using DaLang.Lims.BaseData.Contracts.Group.Dto;
using DaLang.Lims.BaseData.Contracts.UserGroup.Dto;

namespace DaLang.Lims.BaseData.Contracts.UserGroup;

/// <summary>
/// 用户组别服务
/// </summary>
public interface IBaseUserGroupService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BaseUserGroupDto> GetAsync(long id);

    /// <summary>
    /// 根据组别查询列表
    /// </summary>
    Task<List<BaseUserGroupDto>> GetListByGroupCodeAsync(BaseUserGroupQueryInput input);

    /// <summary>
    /// 根据用户查询列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<List<BaseUserGroupDto>> GetListByUserIdAsync(BaseUserGroupQueryInput input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BaseUserGroupDto input);

    /// <summary>
    /// 批量新增
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    Task<bool> AddListAsync(List<BaseUserGroupDto> list);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BaseUserGroupDto input);

    /// <summary>
    /// 批量更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task UpdateListAsync(List<BaseUserGroupDto> input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);

    /// <summary>
    /// 获取检验权限组别
    /// </summary>
    /// <returns></returns>
    Task<List<BaseGroupDto>> GetUserCanTestGroup();

    /// <summary>
    /// 判断检验权限
    /// </summary>
    /// <param name="groupCode"></param>
    /// <returns></returns>
    Task<bool> CheckUserTestPermission(string groupCode);
}
