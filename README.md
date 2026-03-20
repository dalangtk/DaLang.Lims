

<div align="center">

# DaLang.Lims

![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)

![License](https://img.shields.io/badge/license-GPL-green)

![Database](https://img.shields.io/badge/database-MySQL-orange)

**现代化、可扩展的实验室信息管理系统**

</div>

---

## 📖 项目简介

DaLang LIMS 基于 **.NET 8.0** + **Vue 3** 构建的现代化实验室信息管理系统，提供完整的实验室业务流程管理解决方案，采用前后端分离架构，支持跨平台运行。系统涵盖系统设置、基础数据管理、标本前处理、检验分析、报告生成等核心功能模块。

**此项目为后台Api接口，前端及配套报告单工具项目地址：**
- vue3前端: https://gitee.com/shabigou/dalang-lims-ui
- 报告预览/生成器： https://gitee.com/shabigou/dalang-report-designer

---

## 🏗️ 技术架构

### 技术栈

| 类别 | 技术选型 |
|------|----------|
| 后端框架 | .NET Core 8.0 |
| 数据库 | MySQL + SqlSugar ORM |
| API 文档 | Knife4j/Swagger |
| 缓存 | Redis + MemoryCache |
| 权限认证 | JWT + Claim 认证 |

### 项目结构
```
DaLang.Lims
├── framework/                          # 框架层
│   ├── DaLang.Lims.Web.ApiUI          # API 文档 UI
│   ├── DaLang.Lims.Web.Common         # 公共工具类
│   ├── DaLang.Lims.Web.DynamicApi     # 动态 API 框架
│   ├── DaLang.Lims.Web.Framework      # 核心框架
│   └── DaLang.Lims.Web.Module.Dev     # 开发工具模块
├── hosts/                              # 主机层
│   └── DaLang.Lims.Web.Host           # Web 启动项目
└── modules/                            # 业务模块
    ├── BasicData/                     # 基础数据模块
    ├── Exam/                          # 检验管理模块
    ├── Pretreatment/                  # 前处理模块
    ├── ReportTemplate/                # 报告模板模块
    ├── Statistics/                    # 统计分析模块
    └── Shared/                        # 共享模块
```

### 模块分层设计

每个业务模块采用标准分层架构：
```
ModuleName
├── Contracts/          # 接口契约层（DTO、Service 接口）
├── Application/        # 应用服务层（业务逻辑实现）
├── Domain/            # 领域层（实体、仓储接口）
├── Core/              # 核心层（常量、枚举、配置）
└── Sqlsugar.Mysql/    # 基础设施层（仓储实现）
```

## 📦 核心功能模块

### 1. 系统设置 🔧

| 功能 | 描述 |
|------|------|
| 用户管理 | 用户账号、角色、权限配置 |
| 组织架构 | 租户、部门、岗位管理 |
| 权限控制 | 菜单权限、API 权限、数据权限 |
| 系统日志 | 操作日志、登录日志、异常日志 |
| 字典管理 | 系统字典、字典类型维护 |
| 参数配置 | 系统参数、业务参数配置 |

### 2. 基础数据 📋

| 功能 | 描述 |
|------|------|
| 检验项目 | 检验项目、参考范围、个性化设置 |
| 上机项目 | 上机项目设置 |
| 检验目的 | 检验目的设置 |
| 检验套餐 | 检验套餐设置 |
| 仪器管理 | 检验仪器设置 |
| 样本类型 | 样本类型定义、采集要求 |
| 客户管理 | 客户信息管理 |
| 用户组别 | 检验、审核权限 |
| 规则管理 | 审核规则、问询规则 |
| 序列管理 | 样本号编号规则 |

### 3. 前处理管理 🧪

| 功能 | 描述 |
|------|------|
| 样本录入 | 样本直接录入 |
| 样本分拣 | 分拣规则、样本分拣 |
| 样本流转 | 样本交接 |
| 数据导入 | Excel 导入、导入配置管理 |
| 分血处理 | 样本分血、分血记录 |

### 4. 标本检验 🔬

| 功能 | 描述 |
|------|------|
| 检验任务 | 任务分配、任务执行 |
| 结果录入 | 手工录入 |
| 结果审核 | 人工审核、多级审核 |
| 危急值管理 | 危急值判定 |
| 报告生成 | 报告模板、报告发布 |

### 5. 报告与统计 📊

| 功能 | 描述 |
|------|------|
| 报告模板 | 模板设计、模板管理 |
| 报告查询 | 历史报告查询、报告下载 |
| 样本查询 | 样本状态查询、样本统计 |

### 6. 待完成功能列表

- [ ] 物流模块
- [ ] 前处理
  - [ ] 双输录入
  - [ ] 数据对接
  - [ ] 条码打印
- [ ] 检验
  - [ ] 加做
  - [ ] 复查
  - [ ] 批量操作
- [ ] 查询统计
  - [ ] 报告合并打印
  - [ ] 工作量统计

---

## 🚀 核心特性

### 动态 API 框架

项目内置 `DaLang.Lims.Web.DynamicApi` 动态 API 框架，支持：

- ✅ 自动路由生成（支持多种命名规范）
- ✅ 自动 HTTP 动词识别（GET/POST/PUT/DELETE）
- ✅ 统一响应格式封装
- ✅ 接口自动注册与发现
- ✅ 支持 Area 分组管理

```csharp
// 使用示例
[DynamicApi(Area = "BaseData")]
public class BaseItemService : BaseService,IBaseItemService,IDynamicApi
{
    public async Task<BaseItemDto> GetAsync(string id) { }
    public async Task<List<BaseItemDto>> GetListAsync(BaseItemQueryInput input) { }
}
```

### 权限安全

- JWT Token 认证
- Claim 权限模型
- API 访问控制
- 数据权限隔离
- 操作日志审计

### 缓存机制

- 多级缓存支持（Memory + Redis）
- 缓存键统一管理
- 缓存自动失效
- 分布式缓存支持

### 数据库支持

- SqlSugar ORM 框架
- MySQL 数据库
- 多租户数据隔离
- 仓储模式封装
- 支持多数据库扩展

---

## 🛠️ 快速开始

### 环境要求

- .NET 8.0 SDK
- MySQL 8.0+
- Redis 6.0+
- Node.js 16+ (前端)

### 安装步骤

1. **克隆项目**
```bash
git clone https://gitee.com/shabigou/dalang-lims.git
cd dalang-lims
dotnet restore
```

2. **配置数据库**
```
修改 hosts/DaLang.Lims.Web.Host/Configs/dbconfig.json
{
  "ConnectionString": "server=localhost;port=3306;database=dlims;uid=root;pwd=yourpassword;"
}
```
3. **初始化数据库**
```
执行 hosts/InitData/dlims.sql
```
4. **运行项目**
```bash
cd hosts/DaLang.Lims.Web.Host
dotnet run
```
5. **API文档**
项目启动后访问 http://localhost:8000/admin/index.html

## 📁 目录说明

| 目录 | 说明 |
|------|------|
| framework/ | 核心框架代码，提供基础能力 |
| hosts/ | 应用程序入口，包含配置和启动代码 |
| modules/ | 业务模块，按功能域划分 |
| modules/*/Contracts/ | 接口契约，定义 DTO 和服务接口 |
| modules/*/Application/ | 应用服务，实现业务逻辑 |
| modules/*/Domain/ | 领域模型，定义实体和仓储接口 |
| modules/*/Sqlsugar.Mysql/ | 数据访问，实现仓储 |

## 🔌 扩展开发
**添加新模块**
- 在 modules/ 下创建新模块目录
- 按照分层结构创建子目录
```
// 在 dbconfig.json 中配置程序集
"assemblyNames": [
  "DaLang.Lims.Web.Framework",
  "DaLang.Lims.Web.Dev",
  "DaLang.Lims.BaseData.Domain",
  "DaLang.Lims.Shared.Domain",
  "DaLang.Lims.Pretreatment.Domain",
  "DaLang.Lims.Exam.Domain",
  "DaLang.Lims.ReportTemplate.Domain"
]
```

## 效果图
![login]( https://gitee.com/shabigou/dalang-lims/raw/master/images/01login.png )
![homepage]( https://gitee.com/shabigou/dalang-lims/raw/master/images/02homepage.png )
![user]( https://gitee.com/shabigou/dalang-lims/raw/master/images/03user.png )
![role]( https://gitee.com/shabigou/dalang-lims/raw/master/images/04role.png )
![dict]( https://gitee.com/shabigou/dalang-lims/raw/master/images/05dict.png )
![operlog]( https://gitee.com/shabigou/dalang-lims/raw/master/images/06operlog.png )
![item]( https://gitee.com/shabigou/dalang-lims/raw/master/images/07item.png )
![sampleinput]( https://gitee.com/shabigou/dalang-lims/raw/master/images/08sampleinput.png )
![import]( https://gitee.com/shabigou/dalang-lims/raw/master/images/09import.png )
![sorting]( https://gitee.com/shabigou/dalang-lims/raw/master/images/10sorting.png )
![handover]( https://gitee.com/shabigou/dalang-lims/raw/master/images/11handover.png )
![exam]( https://gitee.com/shabigou/dalang-lims/raw/master/images/12exam.png )
![reportdesigner]( https://gitee.com/shabigou/dalang-lims/raw/master/images/13reportdesigner.png )
![report]( https://gitee.com/shabigou/dalang-lims/raw/master/images/14report.png )

## 📄 许可证
本项目采用GPL许可证，详见 LICENSE 文件。

## 🤝 贡献指南
欢迎提交 Issue 和 Pull Request！

```
⭐ 如果这个项目对你有帮助，请给一个 Star 支持一下！
```