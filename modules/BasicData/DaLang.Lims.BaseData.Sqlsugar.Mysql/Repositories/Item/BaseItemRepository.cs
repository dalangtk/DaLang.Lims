using DaLang.Lims.BaseData.Domain.Item;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.Web.BaseData.Repositories.Item;

public class BaseItemRepository : AdminRepositoryBase<BaseItemEntity>, IBaseItemRepository
{
    public BaseItemRepository()
    {
    }
}
