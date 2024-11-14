using ImageMagick;
using ReactiveUI;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
namespace ImageCompressor.ViewModels;
public class ItemVM : ViewModelBase
{
    private bool isCompressing = false;
    private bool isOptimizing = false;
    private readonly MainWindowViewModel MWVM;
    public ItemVM(FileInfo file, MainWindowViewModel mwvm)
    {
        Name = file.Name;
        MWVM = mwvm;
        if (Ext.ImageExts.Contains(file.Extension.ToLower()))
        {
            IsImage = true;
            Compress = ReactiveCommand.Create(() => Com(file));
        }
        if (Ext.OptimizableExts.Contains(file.Extension.ToLower()))
        {
            IsOptimizable = true;
            Optimize = ReactiveCommand.Create(() => Opt(file));
        }
    }

    public string Name { get; set; }
    public bool IsImage { get; set; } = false;
    public bool IsOptimizable { get; set; } = false;
    public bool IsCompressing { get => isCompressing; set => this.RaiseAndSetIfChanged(ref isCompressing, value); }
    public bool IsOptimizing { get => isOptimizing; set => this.RaiseAndSetIfChanged(ref isOptimizing, value); }

    public ICommand? Compress { get; set; }
    public ICommand? Optimize { get; set; }
    public async void Com(FileInfo file)
    {
        IsCompressing = true;
        MWVM.WorkStrat(file, ActionType.Compression);
        using var image = new MagickImage(file);
        await Task.Run(() =>
        {
            using Stream stream = new MemoryStream();
            image.Format = MWVM.Config.Format;
            image.Quality = MWVM.Config.Quality;
            image.Write(stream);
            if (stream.Length <= file.Length)
            {
                using var fileStream = File.Create(file.FullName.Replace(file.Extension, $".magick.{MWVM.Config.Format.ToString()}"));
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
                MWVM.WorkEnd(file, ActionType.Compression, true);
            }
            else
            {
                MWVM.WorkEnd(file, ActionType.Compression, false);
            }
            IsCompressing = false;
        });
    }
    public async void Opt(FileInfo file)
    {
        IsOptimizing = true;
        MWVM.WorkStrat(file, ActionType.Optimization);
        var optimizer = new ImageOptimizer();
        await Task.Run(() =>
        {
            bool result = optimizer.LosslessCompress(file);
            IsOptimizing = false;
            MWVM.WorkEnd(file, ActionType.Compression, result);
        });
    }
}
public static class Ext
{
    public static readonly string[] ImageExts = [".png", ".jpg", ".jpeg", ".ico", ".icon", ".gif", ".tif", ".tiff", ".webp", ".heic", ".heif", ".avif"];
    public static readonly string[] OptimizableExts = [".png", ".jpg", ".jpeg", ".ico", ".icon", ".gif"];
}
public enum ActionType
{
    Compression = 1,
    Optimization = 2
}
public class Status(FileInfo file, ActionType action, bool status)
{
    public FileInfo file = file;
    public ActionType action = action;
    public bool status = status;
}
