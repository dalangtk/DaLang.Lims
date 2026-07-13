using DaLang.Lims.OpenSlide.ImageExtensions;
using DaLang.Lims.OpenSlide.OpenSlideNET;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Text;

namespace DaLang.Lims.OpenSlide.Application;

[DynamicApi(Area = "openslide")]
public class OpenSlideService : ControllerBase, IDynamicApi
{
    ImageProvider _provider;
    DeepZoomGeneratorCache _generatorCache;
    public OpenSlideService(ImageProvider provider, DeepZoomGeneratorCache generatorCache)
    {
        _provider = provider;
        _generatorCache = generatorCache;
    }

    [HttpGet("/storage/{*path}")]
    [AllowAnonymous]
    [NonFormatResult]
    [NoOprationLog]
    public async Task GetImageFiles()
    {
        if (!HttpContext.Request.Path.StartsWithSegments("/storage", out PathString remaining))
        {
            return;
        }

        if (!TryParseDeepZoom(HttpContext.Request.Path, out var result))
            return;

        if (result.format != "jpeg")
            return;

        if (!_provider.TryGetImagePath(result.name, out string path))
        {
            HttpContext.Response.StatusCode = 404;
            await HttpContext.Response.WriteAsync("FileNotFound.");
            return;
        }

        RetainableDeepZoomGenerator dz = _provider.RetainDeepZoomGenerator(result.name, path);
        try
        {
            HttpContext.Response.ContentType = "image/jpeg";
            await dz.GetTileAsJpegToStreamAsync(result.level, result.col, result.row, HttpContext.Response.Body);
        }
        finally
        {
            dz.Release();
        }
        return;
    }
    [HttpGet("/storage/{name}.dzi")]
    [AllowAnonymous]
    [NonFormatResult]
    public IActionResult GetDzi(string name)
    {
        if (!_provider.TryGetImagePath(name, out string path))
        {
            return NotFound();
        }

        RetainableDeepZoomGenerator dz = _provider.RetainDeepZoomGenerator(name, path);
        try
        {
            return Content(dz.GetDzi(), "application/xml", Encoding.UTF8);
        }
        finally
        {
            dz.Release();
        }

    }

    private static bool TryParseDeepZoom(string expression, out (string name, int level, int col, int row, string format) result)
    {
        if (expression.Length < 4 || expression[0] != '/')
        {
            result = default;
            return false;
        }
        expression = expression.Replace("/storage", "");

        // seg: {name}_files/{level}/{col}_{row}.jpeg
        StringSegment seg = new StringSegment(expression, 1, expression.Length - 1);
        int iPos = seg.IndexOf('/');
        if (iPos <= 0)
        {
            result = default;
            return false;
        }

        StringSegment segName = seg.Subsegment(0, iPos);
        if (segName.Length < 6 || !segName.EndsWith("_files", StringComparison.Ordinal))
        {
            result = default;
            return false;
        }
        string resultName = segName.Substring(0, segName.Length - 6);

        // seg: {level}/{col}_{row}.jpeg
        seg = seg.Subsegment(iPos + 1);
        iPos = seg.IndexOf('/');
        if (iPos <= 0)
        {
            result = default;
            return false;
        }

        if (!int.TryParse(seg.Substring(0, iPos), out var resultLevel))
        {
            result = default;
            return false;
        }

        // seg: {col}_{row}.jpeg
        seg = seg.Subsegment(iPos + 1);
        iPos = seg.IndexOf('_');
        if (seg.IndexOf('/') >= 0 || iPos <= 0)
        {
            result = default;
            return false;
        }

        if (!int.TryParse(seg.Substring(0, iPos), out var resultCol))
        {
            result = default;
            return false;
        }

        // seg: {row}.jpeg
        seg = seg.Subsegment(iPos + 1);
        iPos = seg.IndexOf('.');
        if (iPos <= 0)
        {
            result = default;
            return false;
        }

        if (!int.TryParse(seg.Substring(0, iPos), out var resultRow))
        {
            result = default;
            return false;
        }

        // seg: jpeg
        seg = seg.Subsegment(iPos + 1);

        result = (name: resultName, level: resultLevel, col: resultCol, row: resultRow, format: seg.ToString());
        return true;
    }
}
