using DaLang.Lims.Pretreatment.Contracts.SampleInput.Dto;

namespace DaLang.Lims.Pretreatment.Contracts.SampleInput;

public interface ISampleInputService
{
    /// <summary>
    /// 判断条码是否已使用
    /// </summary>
    /// <param name="barcode"></param>
    /// <returns></returns>
    Task<bool> CheckBarcodeInUse(string barcode);
    /// <summary>
    /// 保存信息和项目
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<InputSaveSuccessOutput> SaveInfoAndItem(InputSaveDto input);
}
