using System.Reflection;

namespace GodRayPlugin
{
    internal class ShaderResourceLoader
    {
        public static byte[] GetShaderResource(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"GodRay.Shaders.{name}";
            using var stream = assembly.GetManifestResourceStream(resourceName) ??
                throw new Exception($"Resource {resourceName} not found.");
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }
    }
}