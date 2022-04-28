using OzonCard.Context.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace OzonCardService.Services.Quartzs.Workers
{
    public interface IServiceDatabase
    {
        Task CreateBackup(string path, int countBackup);
        Task RemoveOldFile(string path, int countDays);
        Task RemoveOldTokensRefresh(int countDays);
    }



    public class ServiceDatabase : IServiceDatabase
    {
        private readonly static ILogger log = Log.ForContext(typeof(ServiceDatabase));
        readonly IServiceRepository _repository;
        public ServiceDatabase(IServiceRepository repository)
        {
            _repository = repository;
        }
        public async Task CreateBackup(string path, int countBackup)
        {
            log.Information("Start backup data base: time start: {0}", DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss"));
            path = Path.Combine(path, "Automatic");
            PreBackup(path);
            if (await OnBackup(path))
            {
                PostBackup(path, countBackup);
            }
        }
        public async Task RemoveOldFile(string path, int countDays)
        {
            log.Information("Remove old files");
            if (await _repository.RemoveOldFile(countDays))
            {
                var files = Directory.GetFiles(path);
                var date = DateTime.UtcNow.AddDays(-countDays);
                var count = 0;
                foreach (var file in files)
                    if (new FileInfo(file).CreationTimeUtc < date)
                    {
                        Directory.Delete(file);
                        count++;
                    }
                log.Information($"Remove {count} old files with date created < {date}");
            }

        }

        void PreBackup(string path)
        {
            log.Debug("PreBackup: check/create directori in {0}", path);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        async Task<bool> OnBackup(string path)
        {
            try
            {
                return await _repository.CreateBackup(path);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error on creating backups:");
                return false;
            }
        }

        bool PostBackup(string path, int countBackup)
        {
            try
            {
                //архивирование всех бекапов коме последнего и удаление файла после архивации
                Dictionary<string, DateTime> dic_files = Directory.GetFiles(path)
                    .Where(f => f.EndsWith(".bak"))
                    .ToDictionary(d => d, d => Directory.GetCreationTimeUtc(d));
                var data = dic_files.Max(f => f.Value);
                dic_files.Remove(dic_files.First(x => x.Value == data).Key);
                foreach (var file in dic_files)
                {
                    Compress(file.Key);
                    File.Delete(file.Key);
                }
                //удаление старых архивов
                dic_files = Directory.GetFiles(path)
                    .Where(f => f.EndsWith(".gz"))
                    .ToDictionary(d => d, d => Directory.GetCreationTimeUtc(d));
                while (countBackup < dic_files.Count())
                {
                    data = dic_files.Min(f => f.Value);
                    var file = dic_files.First(x => x.Value == data).Key;
                    dic_files.Remove(file);
                    File.Delete(file);
                }
                log.Debug($"PostBackup: created .bak.gz");
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error post creating backups:");
                return false;
            }
        }

        void Compress(string sourceFile)
        {
            string compressedFile = string.Format(sourceFile + ".gz");
            // поток для чтения исходного файла
            using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
            {
                // поток для записи сжатого файла
                using (FileStream targetStream = File.Create(compressedFile))
                {
                    // поток архивации
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream); // копируем байты из одного потока в другой
                        log.Debug("Compress file {0} completed. Original size: {1}  compressed size: {2}.",
                            sourceFile, sourceStream.Length.ToString(), targetStream.Length.ToString());
                    }
                }
            }
        }

        public async Task RemoveOldTokensRefresh(int countDays)
        {
            log.Information("Remove old TokensRefresh");
            await _repository.RemoveOldTokensRefresh(countDays);

        }
    }
}
