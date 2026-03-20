using OnceMi.AspNetCore.OSS;
using SqlSugar;
using System;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Entities;

namespace DaLang.Lims.Web.Framework.Domain;

/// <summary>
/// 文件
/// </summary>
[SugarTable(TableName = "sys_file")]
public partial class FileEntity : EntityTenant
{
    /// <summary>
    /// OSS供应商
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 1)]
    public OSSProvider? Provider { get; set; }

    /// <summary>
    /// 存储桶名称
    /// </summary>
    [SugarColumn(Length = 32, IsNullable = true, CreateTableFieldSort = 2)]
    public string BucketName { get; set; }

    /// <summary>
    /// 文件目录
    /// </summary>
    [SugarColumn(Length = 64, CreateTableFieldSort = 3)]
    public string FileDirectory { get; set; }

    /// <summary>
    /// 文件Guid
    /// </summary>
    [OrderGuid]
    [SugarColumn(Length = 128, CreateTableFieldSort = 4)]
    public Guid FileGuid { get; set; }

    /// <summary>
    /// 保存文件名
    /// </summary>
    [SugarColumn(Length = 128, CreateTableFieldSort = 5)]
    public string SaveFileName { get; set; }

    /// <summary>
    /// 文件名
    /// </summary>
    [SugarColumn(Length = 32, CreateTableFieldSort = 6)]
    public string FileName { get; set; }

    /// <summary>
    /// 文件扩展名
    /// </summary>
    [SugarColumn(Length = 16, CreateTableFieldSort = 7)]
    public string Extension { get; set; }

    /// <summary>
    /// 文件字节长度
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 8)]
    public long Size { get; set; }

    /// <summary>
    /// 文件大小格式化
    /// </summary>
    [SugarColumn(Length = 32, CreateTableFieldSort = 9)]
    public string SizeFormat { get; set; }

    /// <summary>
    /// 链接地址
    /// </summary>
    [SugarColumn(Length = 256, CreateTableFieldSort = 10)]
    public string LinkUrl { get; set; }

    /// <summary>
    /// md5码，防止上传重复文件
    /// </summary>
    [SugarColumn(Length = 64, CreateTableFieldSort = 11)]
    public string Md5 { get; set; } = string.Empty;
}