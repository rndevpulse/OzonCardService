using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.File.Archive;
using System.IO.Compression;

namespace OzonCard.Logger
{
    public class InitialSerilogger
    {
        string formatter = @"{Timestamp:dd.MM.yyyy HH:mm:ss.fff} [{Level:u3}] [{SourceContext}] {Message}{NewLine}{Exception}";
        TimeSpan flushToDiskInterval = TimeSpan.FromSeconds(10);
        ArchiveHooks hooks = new ArchiveHooks(CompressionLevel.Fastest, @"logs\Old");
        int retainedFileCountLimit = 7;

      
        public InitialSerilogger()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.Development.json", true, true)
                .AddJsonFile($"appsettings.production.json", true)
                .Build();


            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                //правило для фулового лога
                .WriteTo.File(@"logs\Full_.log", LogEventLevel.Verbose,
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: formatter,
                    hooks: hooks,
                    retainedFileCountLimit: retainedFileCountLimit,
                    flushToDiskInterval: flushToDiskInterval)
                //правило для лога менеджеров
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(Matching.FromSource("OzonCard.BizClient"))
                    .WriteTo.File(@"logs\BizClient.log", LogEventLevel.Verbose,
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: formatter,
                        hooks: hooks,
                        retainedFileCountLimit: retainedFileCountLimit,
                        flushToDiskInterval: flushToDiskInterval))
                //правило для лога контроллеров
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(Matching.FromSource("OzonCardService.Controllers"))
                    .WriteTo.File(@"logs\Controllers_.log", LogEventLevel.Verbose,
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: formatter,
                        hooks: hooks,
                        retainedFileCountLimit: retainedFileCountLimit,
                        flushToDiskInterval: flushToDiskInterval))
                //правило для лога контекста базы
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(Matching.FromSource("OzonCard.Context"))
                    .WriteTo.File(@"logs\Context.log", LogEventLevel.Verbose,
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: formatter,
                        hooks: hooks,
                        retainedFileCountLimit: retainedFileCountLimit,
                        flushToDiskInterval: flushToDiskInterval))
                //правило для лога ошибок
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level >= LogEventLevel.Warning)
                    .WriteTo.File(@"logs\Error_.log",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: formatter,
                        hooks: hooks,
                        retainedFileCountLimit: retainedFileCountLimit,
                        flushToDiskInterval: flushToDiskInterval))
                .CreateLogger();



        }
    }
}