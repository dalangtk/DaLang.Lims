using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Services.Parameter.Dto;
using System;
using System.Threading.Tasks;

namespace DaLang.Lims.Web.Framework.Services.Parameter;

/// <summary>
/// 系统参数服务
/// </summary>
public interface IParameterService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<ParameterDto> GetAsync(long id);
    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<ParameterGetListDto>> GetPageAsync(PageInput<ParameterQueryInput> input);
    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(ParameterDto input);
    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(ParameterDto input);
    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
    /// <summary>
    /// 查询参数值  sys_param:
    /// </summary>
    /// <param name="paramName"></param>
    /// <param name="defaultValue"></param>
    /// <param name="expireTime"></param>
    /// <returns></returns>
    Task<string> GetParamValue(string paramName, string defaultValue = "", TimeSpan? expireTime = null);
}