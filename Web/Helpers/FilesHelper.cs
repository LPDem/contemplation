using System.IO;

namespace Contemplation
{
    public static class FilesHelper
    {
        public static string RestrictFileToFolder(string folder, string name)
        {
            return Path.GetFullPath(Path.Combine(folder, Path.GetFileName(name)));
        }
    }
}
