using System.Collections.Generic;
using System.Linq;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Domain.Api;

namespace DaLang.Lims.Web.Framework.Core.Logs;

/// <summary>
/// Api帮助类
/// </summary>
//[InjectSingleton]
[InjectScoped]
public class ApiHelper
{
    private List<ApiHelperDto> _apis;
    private static readonly object _lockObject = new();

    private readonly IApiRepository _apiRepository;
    public ApiHelper(IApiRepository apiRepository)
    {
        _apiRepository = apiRepository;
    }

    public List<ApiHelperDto> GetApis()
    {
        if (_apis != null && _apis.Any())
            return _apis;

        lock (_lockObject)
        {
            if (_apis != null && _apis.Any())
                return _apis;

            _apis = new List<ApiHelperDto>();

            var apis = _apiRepository.AsQueryable().Select(a => new { a.Id, a.ParentId, a.Label, a.Path }).ToList();

            foreach (var api in apis)
            {
                var parentLabel = apis.FirstOrDefault(a => a.Id == api.ParentId)?.Label;

                _apis.Add(new ApiHelperDto
                {
                    Label = parentLabel.NotNull() ? $"{parentLabel} / {api.Label}" : api.Label,
                    Path = api.Path?.ToLower().Trim('/')
                });
            }

            return _apis;
        }
    }
}

public class ApiHelperDto
{
    /// <summary>
    /// 接口名称
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 接口地址
    /// </summary>
    public string Path { get; set; }
}