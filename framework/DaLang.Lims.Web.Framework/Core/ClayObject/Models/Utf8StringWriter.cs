using System.IO;
using System.Text;

namespace DaLang.Lims.Web.Framework.Core.ClayObject;

/// <summary>
///     <c>UTF-8</c> 格式的 <see cref="StringWriter" />
/// </summary>
internal sealed class Utf8StringWriter : StringWriter
{
    /// <inheritdoc />
    public override Encoding Encoding => Encoding.UTF8;
}