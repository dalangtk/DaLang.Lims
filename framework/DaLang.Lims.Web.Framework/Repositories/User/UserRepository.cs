using DaLang.Lims.Web.Framework.Domain.User;

namespace DaLang.Lims.Web.Framework.Repositories;

public class UserRepository : AdminRepositoryBase<UserEntity>, IUserRepository
{
    public UserRepository()
    {

    }
}