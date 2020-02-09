using System.IO;
using System.Reflection;

namespace Core.Tools
{
    public class EmbeddedResource
    {
        public static Stream GetStream(Assembly executingAssembly, string resourceName)
        {
            var stream = executingAssembly.GetManifestResourceStream(resourceName);
            return stream;
        }
    }
}
