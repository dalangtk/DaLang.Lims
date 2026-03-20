using DaLang.Lims.BaseData.Domain.Customer;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.BaseData.Sqlsugar.Mysql.Repositories.Customer;

public class BaseCustomerRepository : AdminRepositoryBase<BaseCustomerEntity>, IBaseCustomerRepository
{
    public BaseCustomerRepository()
    {
    }
}
