using DaLang.Lims.OpenSlide.OpenSlideNET;
using DaLang.Lims.Pathology.Domain.ExamPathologyDigitalSlicing;
using DaLang.Lims.Web.Framework.Core.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace DaLang.Lims.OpenSlide.Application;

[InjectSingleton]

public class ImageProvider
{
    //private ImageOptionItem[] _images;
    private DeepZoomGeneratorCache _cache;
    IServiceScopeFactory _scopeFactory;

    public ImageProvider(DeepZoomGeneratorCache cache, IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        _cache = cache;
    }

    public DeepZoomGeneratorCache Cache => _cache;

    public bool TryGetImagePath(string name, out string path)
    {
        path = null;
        long id = -1;
        if (!long.TryParse(name, out id))
            return false;
        using (var scope = _scopeFactory.CreateScope())
        {
            var slicingRep = scope.ServiceProvider.GetRequiredService<IExamPathologyDigitalSlicingRepository>();

            var slicing = slicingRep.GetById(id);
            if (slicing == null)
                return false;

            path = slicing.SlicingPath;
            return true;
        }
    }

    public RetainableDeepZoomGenerator RetainDeepZoomGenerator(string name, string path)
    {
        RetainableDeepZoomGenerator dz;
        if (_cache.TryGet(name, out dz))
        {
            dz.Retain();
            return dz;
        }
        dz = new RetainableDeepZoomGenerator(OpenSlideImage.Open(path));
        if (_cache.TrySet(name, dz))
        {
            dz.Retain();
            return dz;
        }
        dz.Retain();
        dz.Dispose();
        return dz;
    }

    private sealed class DummyDisposable : IDisposable
    {
        public static readonly DummyDisposable Instance = new DummyDisposable();
        public void Dispose()
        {
            // do nothing
        }
    }
}
