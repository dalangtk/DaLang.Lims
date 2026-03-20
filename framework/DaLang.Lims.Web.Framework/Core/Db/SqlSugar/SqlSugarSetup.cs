
using DaLang.Lims.Web.Framework.Core;
using DaLang.Lims.Web.Framework.Core.Configs;
using DaLang.Lims.Web.Framework.Core.Db;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Entities;
using DaLang.Lims.Web.Framework.Core.Repositories;
using DaLang.Lims.Web.Framework.Domain.ModifledLog;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Yitter.IdGenerator;
using static DaLang.Lims.Web.Framework.Core.Configs.DbConfig;
using ITenant = SqlSugar.ITenant;

namespace DaLang.Lims.Web.Framework.Db.SqlSugar;

public static class SqlSugarSetup
{
    // 多租户实例
    public static ITenant ITenant { get; set; }

    /// <summary>
    /// SqlSugar 上下文初始化
    /// </summary>
    /// <param name="services"></param>
    public static void AddSqlSugar(this IServiceCollection services)
    {
        // 注册雪花Id
        var snowIdOpt = AppInfo.GetOptions<AppConfig>().IdGenerator;
        YitIdHelper.SetIdGenerator(snowIdOpt);

        // 自定义 SqlSugar 雪花ID算法
        SnowFlakeSingle.WorkId = snowIdOpt.WorkerId;
        StaticConfig.CustomSnowFlakeFunc = YitIdHelper.NextId;

        var dbOptions = AppInfo.GetOptions<DbConfig>();
        dbOptions.ConnectionConfigs.ToList().ForEach(o => SetDbConfig(o, services));

        SqlSugarScope sqlSugar = new(dbOptions.ConnectionConfigs.Adapt<List<ConnectionConfig>>(), db =>
        {
            dbOptions.ConnectionConfigs.ToList().ForEach(config =>
            {
                var dbProvider = db.GetConnectionScope(config.ConfigId);
                SetDbAop(dbProvider, true);
                //SetDbAop(dbProvider, dbOptions.EnableConsoleSql);
                //SetDbDiffLog(dbProvider, config);
            });
        });
        ITenant = sqlSugar;

        services.AddSingleton<ISqlSugarClient>(sqlSugar); // 单例注册
        services.AddScoped(typeof(SqlSugarRepository<>)); // 仓储注册
        //services.AddUnitOfWork<SqlSugarUnitOfWork>(); // 事务与工作单元注册
        services.AddTransient<IUnitOfWork, SqlSugarUnitOfWork>();

        // 初始化数据库表结构及种子数据
        dbOptions.ConnectionConfigs.ToList().ForEach(config =>
        {
            InitDatabase(sqlSugar, config);
        });
    }

    /// <summary>
    /// 配置连接属性
    /// </summary>
    /// <param name="config"></param>
    public static void SetDbConfig(DbConnectionConfig config, IServiceCollection services)
    {
        var configureExternalServices = new ConfigureExternalServices
        {
            EntityNameService = (type, entity) => // 处理表
            {
                entity.IsDisabledDelete = true; // 禁止删除非 sqlsugar 创建的列
                // 只处理贴了特性[SugarTable]表
                if (!type.GetCustomAttributes<SugarTable>().Any())
                    return;
                if (config.DbSettings.EnableUnderLine && !entity.DbTableName.Contains('_'))
                    entity.DbTableName = UtilMethods.ToUnderLine(entity.DbTableName); // 驼峰转下划线
            },
            EntityService = (type, column) => // 处理列
            {
                // 只处理贴了特性[SugarColumn]列
                if (!type.GetCustomAttributes<SugarColumn>().Any())
                    return;
                if (new NullabilityInfoContext().Create(type).WriteState is NullabilityState.Nullable)
                    column.IsNullable = true;
                if (config.DbSettings.EnableUnderLine && !column.IsIgnore && !column.DbColumnName.Contains('_'))
                    column.DbColumnName = UtilMethods.ToUnderLine(column.DbColumnName); // 驼峰转下划线
            },
            DataInfoCacheService = new SqlSugarCache(),
        };
        config.ConfigureExternalServices = configureExternalServices;
        config.InitKeyType = InitKeyType.Attribute;
        config.IsAutoCloseConnection = true;
        config.MoreSettings = new ConnMoreSettings
        {
            IsAutoRemoveDataCache = true, // 启用自动删除缓存，所有增删改会自动调用.RemoveDataCache()
            IsAutoDeleteQueryFilter = true, // 启用删除查询过滤器
            IsAutoUpdateQueryFilter = true, // 启用更新查询过滤器
            SqlServerCodeFirstNvarchar = true // 采用Nvarchar
        };
    }

    /// <summary>
    /// 配置Aop
    /// </summary>
    /// <param name="db"></param>
    /// <param name="enableConsoleSql"></param>
    public static void SetDbAop(SqlSugarScopeProvider db, bool enableConsoleSql)
    {
        // 设置超时时间
        db.Ado.CommandTimeOut = 30;

        // 打印SQL语句
        if (enableConsoleSql)
        {
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                //// 若参数值超过100个字符则进行截取
                //foreach (var par in pars)
                //{
                //    if (par.DbType != System.Data.DbType.String || par.Value == null) continue;
                //    if (par.Value.ToString().Length > 100)
                //        par.Value = string.Concat(par.Value.ToString()[..100], "......");
                //}

                var log = $"【{DateTime.Now}——执行SQL】\r\n{UtilMethods.GetNativeSql(sql, pars)}\r\n";
                var originColor = Console.ForegroundColor;
                if (sql.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                    Console.ForegroundColor = ConsoleColor.Green;
                if (sql.StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase) || sql.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase))
                    Console.ForegroundColor = ConsoleColor.Yellow;
                if (sql.StartsWith("DELETE", StringComparison.OrdinalIgnoreCase))
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(log);
                Console.ForegroundColor = originColor;
                //App.PrintToMiniProfiler("SqlSugar", "Info", log);
            };
            db.Aop.OnError = ex =>
            {
                if (ex.Parametres == null) return;
                var log = $"【{DateTime.Now}——错误SQL】\r\n {ex.Message} \r\n{UtilMethods.GetNativeSql(ex.Sql, (SugarParameter[])ex.Parametres)}\r\n";
                var originColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(log);
                Console.ForegroundColor = originColor;
                //App.PrintToMiniProfiler("SqlSugar", "Error", log);
            };
            db.Aop.OnLogExecuted = (sql, pars) =>
            {
                //// 若参数值超过100个字符则进行截取
                //foreach (var par in pars)
                //{
                //    if (par.DbType != System.Data.DbType.String || par.Value == null) continue;
                //    if (par.Value.ToString().Length > 100)
                //        par.Value = string.Concat(par.Value.ToString()[..100], "......");
                //}

                // 执行时间超过5秒时
                if (db.Ado.SqlExecutionTime.TotalSeconds > 5)
                {
                    var fileName = db.Ado.SqlStackTrace.FirstFileName; // 文件名
                    var fileLine = db.Ado.SqlStackTrace.FirstLine; // 行号
                    var firstMethodName = db.Ado.SqlStackTrace.FirstMethodName; // 方法名
                    var log = $"【{DateTime.Now}——超时SQL】\r\n【所在文件名】：{fileName}\r\n【代码行数】：{fileLine}\r\n【方法名】：{firstMethodName}\r\n" + $"【SQL语句】：{UtilMethods.GetNativeSql(sql, pars)}";
                    var originColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(log);
                    Console.ForegroundColor = originColor;
                    //App.PrintToMiniProfiler("SqlSugar", "Slow", log);
                }
            };
        }
        // 数据审计
        db.Aop.DataExecuting = (oldValue, entityInfo) =>
        {
            // 新增/插入
            if (entityInfo.OperationType == DataFilterType.InsertByObject)
            {
                // 若主键是长整型且空则赋值雪花Id
                if (entityInfo.EntityColumnInfo.IsPrimarykey && entityInfo.EntityColumnInfo.PropertyInfo.PropertyType == typeof(long))
                {
                    var id = entityInfo.EntityColumnInfo.PropertyInfo.GetValue(entityInfo.EntityValue);
                    if (id == null || (long)id == 0)
                        entityInfo.SetValue(YitIdHelper.NextId());
                }
                // 若创建时间为空则赋值当前时间
                else if (entityInfo.PropertyName == nameof(EntityBase.ProTime) && entityInfo.EntityColumnInfo.PropertyInfo.GetValue(entityInfo.EntityValue) == null)
                {
                    entityInfo.SetValue(DateTime.Now);
                }
                // 若当前用户非空（web线程时）
                if (AppInfo.User != null && AppInfo.User.TenantId != null)
                {
                    if (entityInfo.PropertyName == nameof(EntityTenant.TenantId))
                    {
                        var tenantId = ((dynamic)entityInfo.EntityValue).TenantId;
                        if (tenantId == null || tenantId == 0)
                            entityInfo.SetValue(AppInfo.User.TenantId.Value);
                    }
                    else if (entityInfo.PropertyName == nameof(EntityTenant.ProId))
                    {
                        var createUserId = ((dynamic)entityInfo.EntityValue).ProId;
                        if (createUserId == 0 || createUserId == null)
                            entityInfo.SetValue(AppInfo.User.Id);
                    }
                    else if (entityInfo.PropertyName == nameof(EntityBase.ProName))
                    {
                        var createUserName = ((dynamic)entityInfo.EntityValue).ProName;
                        if (string.IsNullOrEmpty(createUserName))
                            entityInfo.SetValue(AppInfo.User.UserName);
                    }
                    //else if (entityInfo.PropertyName == nameof(EntityBaseData.CreateOrgId))
                    //{
                    //    var createOrgId = ((dynamic)entityInfo.EntityValue).CreateOrgId;
                    //    if (createOrgId == 0 || createOrgId == null)
                    //        entityInfo.SetValue(App.User.FindFirst(ClaimConst.OrgId)?.Value);
                    //}
                    //else if (entityInfo.PropertyName == nameof(EntityBaseData.CreateOrgName))
                    //{
                    //    var createOrgName = ((dynamic)entityInfo.EntityValue).CreateOrgName;
                    //    if (string.IsNullOrEmpty(createOrgName))
                    //        entityInfo.SetValue(App.User.FindFirst(ClaimConst.OrgName)?.Value);
                    //}
                }
            }
            // 编辑/更新
            else if (AppInfo.User != null && entityInfo.OperationType == DataFilterType.UpdateByObject)
            {
                if (entityInfo.PropertyName == nameof(EntityBase.ModTime))
                    entityInfo.SetValue(DateTime.Now);
                else if (entityInfo.PropertyName == nameof(EntityBase.ModId))
                    entityInfo.SetValue(AppInfo.User.Id);
                else if (entityInfo.PropertyName == nameof(EntityBase.ModName))
                    entityInfo.SetValue(AppInfo.User.UserName);
            }
        };

        db.Aop.OnDiffLogEvent = it =>
        {
            //操作前记录  包含： 字段描述 列名 值 表名 表描述
            var editBeforeData = it.BeforeData;//插入Before为null，之前还没进库
                                               //操作后记录   包含： 字段描述 列名 值  表名 表描述
            var editAfterData = it.AfterData;
            var sql = it.Sql;
            var parameter = it.Parameters;
            var data = it.BusinessData;//这边会显示你传进来的对象
            var time = it.Time;
            var diffType = it.DiffType;//enum insert 、update and delete  

            var modifledList = GetModifled(editBeforeData, editAfterData, diffType);

        };

        // 超管排除其他过滤器
        if (AppInfo.User?.SuperAdmin == true)
            return;

        // 配置假删除过滤器
        db.QueryFilter.AddTableFilter<IDeletedFilter>(u => u.IsDeleted == false);

        // 配置租户过滤器
        var tenantId = AppInfo.User?.TenantId;
        if (tenantId != null && tenantId > 0)
            db.QueryFilter.AddTableFilter<ITenantIdFilter>(u => u.TenantId == tenantId);

        // 配置用户机构（数据范围）过滤器
        SqlSugarFilter.SetOrgEntityFilter(db);

        // 配置自定义过滤器
        SqlSugarFilter.SetCustomEntityFilter(db);
    }
    static readonly List<string> IgnoreColumns = new()
    {
        "proid",
        "proname",
        "protime",
        "modid",
        "modname",
        "modtime",
        "ismodified"
    };
    public static List<ModifiedLogEntity> GetModifled(List<DiffLogTableInfo> beforeData, List<DiffLogTableInfo> afterData, DiffType diffType)
    {
        List<ModifiedLogEntity> modifiedList = new();
        string dataId = null;
        if (beforeData != null)
        {
            var keyCoulumn = beforeData[0].Columns.FirstOrDefault(p => p.IsPrimaryKey == true);
            if (keyCoulumn != null)
            {
                dataId = keyCoulumn.Value.ToString();
            }
        }
        else if (afterData != null)
        {
            var keyCoulumn = afterData[0].Columns.FirstOrDefault(p => p.IsPrimaryKey == true);
            if (keyCoulumn != null)
            {
                dataId = keyCoulumn.Value.ToString();
            }
        }
        if (beforeData != null && afterData != null)
        {
            var befroeColumns = beforeData[0].Columns;
            var afterCloums = afterData[0].Columns;
            foreach (var item in befroeColumns)
            {
                if (IgnoreColumns.Contains(item.ColumnName.ToLower()))
                    continue;
                var afterItem = afterCloums.FirstOrDefault(p => p.ColumnName == item.ColumnName && !p.Value.Equals(item.Value));
                if (afterItem != null)
                {
                    //sb.Append($"[字段:{item.ColumnDescription},修改前:{item.Value}，修改后:{afterItem.Value}]");
                    modifiedList.Add(new ModifiedLogEntity
                    {
                        TableName = beforeData[0].TableName,
                        DataId = dataId,
                        FieldName = $"{item.ColumnDescription}-{item.ColumnName}",
                        OriginalValue = item.Value.ToString(),
                        NewValue = afterItem.Value.ToString(),
                        ModifyType = (int)diffType
                    });
                }
            }
        }

        return modifiedList;
    }

    /// <summary>
    /// 开启库表差异化日志
    /// </summary>
    /// <param name="db"></param>
    /// <param name="config"></param>
    private static void SetDbDiffLog(SqlSugarScopeProvider db, DbConnectionConfig config)
    {
        if (!config.DbSettings.EnableDiffLog)
            return;

        db.Aop.OnDiffLogEvent = async u =>
        {
            //var logDiff = new SysLogDiff
            //{
            //    // 操作后记录（字段描述、列名、值、表名、表描述）
            //    AfterData = JSON.Serialize(u.AfterData),
            //    // 操作前记录（字段描述、列名、值、表名、表描述）
            //    BeforeData = JSON.Serialize(u.BeforeData),
            //    // 传进来的对象（如果对象为空，则使用首个数据的表名作为业务对象）
            //    BusinessData = u.BusinessData == null ? u.AfterData.FirstOrDefault()?.TableName : JSON.Serialize(u.BusinessData),
            //    // 枚举（insert、update、delete）
            //    DiffType = u.DiffType.ToString(),
            //    Sql = UtilMethods.GetNativeSql(u.Sql, u.Parameters),
            //    Parameters = JSON.Serialize(u.Parameters),
            //    Elapsed = u.Time == null ? 0 : (long)u.Time.Value.TotalMilliseconds
            //};
            //var logDb = ITenant.IsAnyConnection(SqlSugarConst.LogConfigId) ? ITenant.GetConnectionScope(SqlSugarConst.LogConfigId) : db;
            //await logDb.CopyNew().Insertable(logDiff).ExecuteCommandAsync();
            //Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine(DateTime.Now + $"\r\n*****开始差异日志*****\r\n{Environment.NewLine}{JSON.Serialize(logDiff)}{Environment.NewLine}*****结束差异日志*****\r\n");
        };
    }

    /// <summary>
    /// 初始化数据库
    /// </summary>
    /// <param name="db"></param>
    /// <param name="config"></param>
    private static void InitDatabase(SqlSugarScope db, DbConnectionConfig config)
    {
        SqlSugarScopeProvider dbProvider = db.GetConnectionScope(config.ConfigId);

        // 初始化/创建数据库
        if (config.DbSettings.EnableInitDb)
        {
            if (config.DbType != DbType.Oracle)
                dbProvider.DbMaintenance.CreateDatabase();
        }

        //初始化表结构
        var dbConfig = AppInfo.GetOptions<DbConfig>();
        var entityTypes = GetEntityTypes(dbConfig.AssemblyNames)?.ToList();
        if (config.TableSettings.EnableInitTable)
        {
            //entityTypes = entityTypes.FindAll(a => a.Name == "ApiEntity");

            //var entityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass && u.IsDefined(typeof(SugarTable), false))
            //    .Where(u => !u.GetCustomAttributes<IgnoreTableAttribute>().Any())
            //    .WhereIF(config.TableSettings.EnableIncreTable, u => u.IsDefined(typeof(IncreTableAttribute), false)).ToList();

            //if (config.ConfigId.ToString() == SqlSugarConst.MainConfigId) // 默认库（有系统表特性、没有日志表和租户表特性）
            //    entityTypes = entityTypes.Where(u => u.GetCustomAttributes<SysTableAttribute>().Any() || (!u.GetCustomAttributes<LogTableAttribute>().Any() && !u.GetCustomAttributes<TenantAttribute>().Any())).ToList();
            //else if (config.ConfigId.ToString() == SqlSugarConst.LogConfigId) // 日志库
            //    entityTypes = entityTypes.Where(u => u.GetCustomAttributes<LogTableAttribute>().Any()).ToList();
            //else
            //    entityTypes = entityTypes.Where(u => u.GetCustomAttribute<TenantAttribute>()?.configId.ToString() == config.ConfigId.ToString()).ToList(); // 自定义的库

            foreach (var entityType in entityTypes)
            {
                var tableName = entityType.GetCustomAttribute<SugarTable>().TableName;
                if (!db.DbMaintenance.IsAnyTable(tableName, false))
                {
                    if (entityType.GetCustomAttribute<SplitTableAttribute>() == null)
                        dbProvider.CodeFirst.InitTables(entityType);
                    else
                        dbProvider.CodeFirst.SplitTables().InitTables(entityType);
                }
            }
        }

        // 初始化种子数据
        if (config.SeedSettings.EnableInitSeed)
        {
            foreach (var tbName in config.SeedSettings.SyncDataIncludeTables)
            {
                var cnt = dbProvider.Queryable<object>().AS(tbName).Count();
                if (cnt > 0)
                    continue;

                var table = entityTypes.FirstOrDefault(u =>
                {
                    var attrs = u.GetCustomAttributes<SugarTable>()?.ToList();
                    if (attrs != null && attrs.Exists(b => b.TableName == tbName))
                    {
                        return true;
                    }
                    return false;
                });

                if (table != null)
                {
                    var instance = Activator.CreateInstance(table);
                    var entityInfo = dbProvider.EntityMaintenance.GetEntityInfo(table);
                    if (entityInfo.Columns.Any(u => u.IsPrimarykey))
                    {
                        var tableName = table.GetCustomAttribute<SugarTable>().TableName;
                        var seedData = GetSeedData(tableName, table);
                        var storage = dbProvider.StorageableByObject(seedData).ToStorage();
                        storage.AsInsertable.ExecuteCommand();
                    }
                }
            }
        }
    }
    public static Type[] GetEntityTypes(string[] assemblyNames)
    {
        if (!(assemblyNames?.Length > 0))
        {
            return null;
        }

        var entityTypes = new List<Type>();

        foreach (var assemblyName in assemblyNames)
        {
            var assembly = Assembly.Load(assemblyName);
            foreach (Type type in assembly.GetExportedTypes())
            {
                foreach (Attribute attribute in type.GetCustomAttributes())
                {
                    if (attribute is SugarTable tableAttribute)
                    {
                        //if (tableAttribute.IsDisabledUpdateAll == false)
                        //{
                        entityTypes.Add(type);
                        //}
                    }
                }
            }
        }

        return entityTypes.ToArray();
    }
    /// <summary>
    /// 初始化租户业务数据库
    /// </summary>
    /// <param name="iTenant"></param>
    /// <param name="config"></param>
    public static void InitTenantDatabase(ITenant iTenant, DbConfig config)
    {
        //SetDbConfig(config);

        //if (!iTenant.IsAnyConnection(config.ConfigId.ToString()))
        //    iTenant.AddConnection(config);
        //var db = iTenant.GetConnectionScope(config.ConfigId.ToString());
        //db.DbMaintenance.CreateDatabase();

        //// 获取所有业务表-初始化租户库表结构（排除系统表、日志表、特定库表）
        //var entityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass && u.IsDefined(typeof(SugarTable), false) &&
        //    !u.IsDefined(typeof(SysTableAttribute), false) && !u.IsDefined(typeof(LogTableAttribute), false) && !u.IsDefined(typeof(TenantAttribute), false)).ToList();
        //if (!entityTypes.Any()) return;

        //foreach (var entityType in entityTypes)
        //{
        //    var splitTable = entityType.GetCustomAttribute<SplitTableAttribute>();
        //    if (splitTable == null)
        //        db.CodeFirst.InitTables(entityType);
        //    else
        //        db.CodeFirst.SplitTables().InitTables(entityType);
        //}
    }
    public static object GetSeedData(string tableName, Type type, string path = "InitData/Admin")
    {
        var fileName = $"{tableName}.json";
        var filePath = Path.Combine(AppContext.BaseDirectory, $"{path}/{fileName}").ToPath();
        if (!File.Exists(filePath))
        {
            var msg = $"数据文件{filePath}不存在";
            Console.WriteLine(msg);
            throw new Exception(msg);
        }
        var jsonData = Common.Helpers.FileHelper.ReadFile(filePath);

        Type listType = typeof(List<>).MakeGenericType(type);
        // 使用JsonConvert.DeserializeObject来将JSON字符串转换成List<>  
        object listObj = JsonConvert.DeserializeObject(jsonData, listType);

        return listObj;
    }
}