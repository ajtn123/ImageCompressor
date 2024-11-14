using Avalonia;
using Avalonia.ReactiveUI;
using ImageCompressor.ViewModels;
using ImageMagick;
using System;
using System.IO;
using System.Linq;
namespace ImageCompressor;
internal sealed class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        if (args.Length >= 1)
        {
            var file = new FileInfo(args[0]);
            var config = ConfigProvider.LoadConfig();
            var action = args.Length >= 2 ? args[1].ToLower() : config.ActtionType.ToString().ToLower();
            if (action == "optimization")
            {
                if (!Ext.OptimizableExts.Contains(file.Extension.ToLower().TrimStart('.'))) return;
                var optimizer = new ImageOptimizer();
                optimizer.LosslessCompress(file);
            }
            else if (action == "compression")
            {
                if (!Ext.ImageExts.Contains(file.Extension.ToLower().TrimStart('.'))) return;
                using var image = new MagickImage(file);
                using Stream stream = new MemoryStream();
                image.Format = config.Format;
                image.Quality = config.Quality;
                image.Write(stream);
                if (config.AlwaysSave || stream.Length < file.Length)
                {
                    using var fileStream = File.Create(file.FullName.Replace(file.Extension, $".magick.{config.Format.ToString().ToLower()}"));
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(fileStream);
                }
            }
        }
        else BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }
    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>().UsePlatformDetect().WithInterFont().LogToTrace().UseReactiveUI();
}
