using System.Data;

namespace DaLang.Lims.Web.Framework.Core.Db
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// 开启事物
        /// </summary>
        void BeginTransaction();
        /// <summary>
        /// 开启事物
        /// </summary>
        /// <param name="iso"></param>
        void BeginTransaction(IsolationLevel iso);

        /// <summary>
        /// 提交事物
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// 回滚事物
        /// </summary>
        void RollbackTransaction();
    }
}
