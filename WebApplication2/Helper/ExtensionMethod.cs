using System.Threading.Tasks;

namespace WebApplication2.Helper
{
    public static class ExtensionMethod
    {

        public static bool CheckType(this IFormFile file, string type="image")
        {
            return file.ContentType.Contains(type);
        }

        public static bool CheckSize(this IFormFile file, int mb)
        {
            return file.Length > mb * 1024 * 1024;
        }

        public static async Task<string> GenerateFileName(this IFormFile file, string folderPath)
        {
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

            string path = Path.Combine(folderPath, uniqueFileName);

            using FileStream stream = new FileStream(path, FileMode.Create);

            await file.CopyToAsync(stream);

            return uniqueFileName;

        }

        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

    }
}
