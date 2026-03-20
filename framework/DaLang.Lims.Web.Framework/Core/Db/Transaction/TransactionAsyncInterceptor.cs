using Castle.DynamicProxy;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Configs;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Web.Framework.Core.Db.Transaction;

public class TransactionAsyncInterceptor : IAsyncInterceptor
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DbConfig _dbConfig;

    public TransactionAsyncInterceptor(IUnitOfWork unitOfWork, DbConfig dbConfig)
    {
        _unitOfWork = unitOfWork;
        _dbConfig = dbConfig;
    }

    private bool TryBegin(IInvocation invocation)
    {
        var method = invocation.MethodInvocationTarget ?? invocation.Method;
        var attribute = method.GetCustomAttributes(typeof(TransactionAttribute), false).FirstOrDefault();
        if (attribute is TransactionAttribute transaction)
        {
            IsolationLevel? isolationLevel = transaction.IsolationLevel == 0 ? null : transaction.IsolationLevel;

            if (isolationLevel != null)
            {
                _unitOfWork.BeginTransaction(isolationLevel.Value);
            }
            else
            {
                _unitOfWork.BeginTransaction();
            }
            return true;
        }

        return false;
    }

    private async Task InternalInterceptAsynchronous(IInvocation invocation)
    {
        //string methodName =
        //    $"{invocation.MethodInvocationTarget.DeclaringType?.FullName}.{invocation.Method.Name}()";
        //int? hashCode = _unitOfWork.GetHashCode();

        invocation.Proceed();

        try
        {
            //异步不能返回null，要返回一个task
            if (invocation.ReturnValue != null)
            {
                await (Task)invocation.ReturnValue;
            }
            _unitOfWork.CommitTransaction();
        }
        catch (Exception ex)
        {
            _unitOfWork.RollbackTransaction();
            throw;
        }
        finally
        {

        }
    }

    private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
    {
        TResult result;
        if (TryBegin(invocation))
        {
            try
            {
                invocation.Proceed();
                result = await (Task<TResult>)invocation.ReturnValue;
                if (result is IResultOutput res && !res.Success)
                {
                    _unitOfWork.RollbackTransaction();
                }
                else
                {
                    _unitOfWork.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }
        else
        {
            invocation.Proceed();
            result = await (Task<TResult>)invocation.ReturnValue;
        }
        return result;
    }

    /// <summary>
    /// 拦截同步执行的方法
    /// </summary>
    /// <param name="invocation"></param>
    public void InterceptSynchronous(IInvocation invocation)
    {
        if (TryBegin(invocation))
        {
            try
            {
                invocation.Proceed();
                var result = invocation.ReturnValue;
                if (result is IResultOutput res && !res.Success)
                {
                    _unitOfWork.RollbackTransaction();
                }
                else
                {
                    _unitOfWork.CommitTransaction();
                }
            }
            catch
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }
        else
        {
            invocation.Proceed();
        }
    }

    /// <summary>
    /// 拦截返回结果
    /// </summary>
    /// <param name="invocation"></param>
    public void InterceptAsynchronous(IInvocation invocation)
    {
        if (TryBegin(invocation))
        {
            invocation.ReturnValue = InternalInterceptAsynchronous(invocation);
        }
        else
        {
            invocation.Proceed();
        }
    }

    /// <summary>
    /// 拦截返回结果
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="invocation"></param>
    public void InterceptAsynchronous<TResult>(IInvocation invocation)
    {
        invocation.ReturnValue = InternalInterceptAsynchronous<TResult>(invocation);
    }
}