using DaLang.Lims.BaseData.Contracts.Group.Dto;
using DaLang.Lims.BaseData.Contracts.UserGroup;
using DaLang.Lims.BaseData.Contracts.UserGroup.Dto;
using DaLang.Lims.BaseData.Domain.Group;
using DaLang.Lims.BaseData.Domain.UserGroup;
using DaLang.Lims.Web.BaseData.Core.Consts;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain.User;
using DaLang.Lims.Web.Framework.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;


namespace DaLang.Lims.BaseData.Services.UserGroup;

/// <summary>
/// 用户组别服务
/// </summary>
[DynamicApi(Area = BaseDataConsts.AreaName)]
public class BaseUserGroupService : BaseService, IBaseUserGroupService, IDynamicApi
{
    private IBaseUserGroupRepository _baseUserGroupRep;

    public BaseUserGroupService(IBaseUserGroupRepository baseUserGroupRep)
    {
        _baseUserGroupRep = baseUserGroupRep;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<BaseUserGroupDto> GetAsync(long id)
    {
        var output = await _baseUserGroupRep.GetAsync(id);
        return output.Adapt<BaseUserGroupDto>();
    }

    /// <summary>
    /// 根据组别查询列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<BaseUserGroupDto>> GetListByGroupCodeAsync(BaseUserGroupQueryInput input)
    {
        var list = await _baseUserGroupRep.AsQueryable()
            .InnerJoin<UserEntity>((a, b) => a.UserId == b.Id && b.IsValid && !b.IsDeleted)
            .InnerJoin<BaseGroupEntity>((a, b, c) => a.GroupCode == c.GroupCode && c.IsValid)
            .Where((a, b) => a.GroupCode == input.GroupCode)
             .Select((a, b, c) => new BaseUserGroupDto { Name = b.Name, GroupName = c.GroupName }, true)
            .ToListAsync();

        return list;
    }
    /// <summary>
    /// 根据用户查询列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<BaseUserGroupDto>> GetListByUserIdAsync(BaseUserGroupQueryInput input)
    {
        var list = await _baseUserGroupRep.AsQueryable()
            .InnerJoin<UserEntity>((a, b) => a.UserId == b.Id && !b.IsDeleted)
            .InnerJoin<BaseGroupEntity>((a, b, c) => a.GroupCode == c.GroupCode && c.IsValid)
             .Where((a, b) => a.UserId == input.UserId)
             .Select((a, b, c) => new BaseUserGroupDto { Name = b.Name, GroupName = c.GroupName }, true)
            .ToListAsync();

        return list;
    }
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<long> AddAsync(BaseUserGroupDto input)
    {
        var entity = Mapper.Map<BaseUserGroupEntity>(input);
        var id = await _baseUserGroupRep.InsertReturnSnowflakeIdAsync(entity);
        return id;
    }

    /// <summary>
    /// 批量新增
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> AddListAsync(List<BaseUserGroupDto> list)
    {
        var entityList = Mapper.Map<List<BaseUserGroupEntity>>(list);
        var ret = await _baseUserGroupRep.InsertRangeAsync(entityList);
        return ret;
    }
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateAsync(BaseUserGroupDto input)
    {
        var entity = await _baseUserGroupRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("用户组别不存在！");

        Mapper.Map(input, entity);
        await _baseUserGroupRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量更新
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task UpdateListAsync(List<BaseUserGroupDto> input)
    {
        var ids = input.Select(x => x.Id).ToList();
        var list = await _baseUserGroupRep.GetListAsync(v => ids.Contains(v.Id));
        foreach (var curr in list)
        {
            var newEntity = input.FirstOrDefault(x => x.Id == curr.Id);
            if (newEntity != null)
            {
                curr.CanTest = newEntity.CanTest;
                curr.CanFirstCheck = newEntity.CanFirstCheck;
                curr.CanSecondCheck = newEntity.CanSecondCheck;
                curr.CanUnCheck = newEntity.CanUnCheck;
                curr.CanPrintedUnCheck = newEntity.CanPrintedUnCheck;
                curr.CanCancelTest = newEntity.CanCancelTest;
                curr.CanDisCancel = newEntity.CanDisCancel;
                curr.CanModifiedInfo = newEntity.CanModifiedInfo;
            }
        }

        await _baseUserGroupRep.UpdateRangeAsync(list);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(long id)
    {
        return await _baseUserGroupRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
    /// <summary>
    /// 获取检验权限组别
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<BaseGroupDto>> GetUserCanTestGroup()
    {
        //if (input.UserId == null || input.UserId <= 0)
        var userId = AppInfo.User.Id;
        if (userId <= 0)
            return new List<BaseGroupDto>();

        var list = await _baseUserGroupRep.AsQueryable()
                    .InnerJoin<BaseGroupEntity>((a, b) => a.GroupCode == b.GroupCode && b.IsValid)
                    .Where(a => a.UserId == userId && a.CanTest == 1 && !a.IsDeleted)
                    .Select((a, b) => new BaseGroupDto { GroupCode = b.GroupCode, GroupName = b.GroupName })
                    .OrderBy(a => a.GroupCode)
                    .ToListAsync();
        return list;
    }

    /// <summary>
    /// 判断检验权限
    /// </summary>
    /// <param name="groupCode"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<bool> CheckUserTestPermission(string groupCode)
    {
        var userId = AppInfo.User.Id;
        if (userId <= 0)
            return false;

        var ret = await _baseUserGroupRep.AsQueryable()
                    .Where(a => a.UserId == userId && a.CanTest == 1 && a.GroupCode == groupCode && !a.IsDeleted)
                    .AnyAsync();
        return ret;
    }
}