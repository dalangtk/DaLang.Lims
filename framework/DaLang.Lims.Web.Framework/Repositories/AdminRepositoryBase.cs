using DaLang.Lims.Web.Framework.Core.Repositories;

namespace DaLang.Lims.Web.Framework.Repositories
{
    /// <summary>
    /// 权限库基础仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class AdminRepositoryBase<TEntity> : SqlSugarRepository<TEntity> where TEntity : class, new()
    {
        public AdminRepositoryBase()
        {

        }        
    }
}