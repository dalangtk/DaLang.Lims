using DaLang.Lims.BaseData.Contracts.Purpose.Dto;
using DaLang.Lims.BaseData.Domain.Purpose;
using Mapster;

namespace DaLang.Lims.BaseData.Application.Purpose;

/// <summary>
/// 映射配置
/// </summary>
public class MapConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<BasePurposePersonalizeEntity, BasePurposePersonalizeDto>()
            .Map(dest => dest.SampleTypeCode,
            src => string.IsNullOrWhiteSpace(src.SampleTypeCode) ? new List<string>() : src.SampleTypeCode.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

        config
            .NewConfig<BasePurposePersonalizeDto, BasePurposePersonalizeEntity>()
            .Map(dest => dest.SampleTypeCode,
            src => src.SampleTypeCode == null || src.SampleTypeCode.Count == 0 ? string.Empty : string.Join(',', src.SampleTypeCode));
    }
}
