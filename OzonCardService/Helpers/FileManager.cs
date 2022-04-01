using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OzonCardService.Helpers
{
    public class FileManager
    {
        static string path = Path.Combine(Directory.GetCurrentDirectory(), "FileReports");

        public FileManager()
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public async Task<bool> Save(Guid id, IFormFile file)
        {
            try
            {
                var format = file.FileName.Split(".").Last().Trim().ToLower();
                using (var fileStream = new FileStream(
                    Path.Combine(path, string.Concat(id.ToString(), ".", format).TrimEnd()),
                    FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                return true;
            }
            catch (Exception)
            { return false; }
        }

        public async Task<bool> RemoveFile(Guid id, string format)
        {
            try
            {
                await Task.Run(() => File.Delete(
                    Path.Combine(path, string.Concat(id.ToString(), ".", format).TrimEnd())));

                return true;
            }
            catch (Exception)
            { return false; }
        }

        public string GetFile(string name)
        {
            var file = Path.Combine(path, name);
            if (File.Exists(file))
                return file;
            else throw new FileNotFoundException();
        }
    }
}
