using Avalonia;
using Avalonia.ReactiveUI;
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
        if (args.Length == 1)
        {
            var file = new FileInfo(args[0]);
            string[] ImageExts = [".png", ".jpg", ".jpeg", ".ico", ".icon", ".gif"];
            if (!ImageExts.Contains(file.Extension.ToLower())) return;
            var optimizer = new ImageOptimizer();
            optimizer.LosslessCompress(file);
        }
        else BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }
    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>().UsePlatformDetect().WithInterFont().LogToTrace().UseReactiveUI();
}
