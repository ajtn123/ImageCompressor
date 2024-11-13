using ImageCompressor.Models;
using ImageMagick;
using ReactiveUI;
using System;
using System.IO;
using System.Reactive;
using System.Text.Json;
using System.Windows.Input;
namespace ImageCompressor.ViewModels;
public class ConfigVM : ViewModelBase
{

    private uint quality;
    private MagickFormat format;

    public ConfigVM(Config config)
    {
        quality = config.Quality;
        format = config.Format;
        ApplyCmd = ReactiveCommand.Create(() =>
        {
            var config = new Config { Quality = Quality, Format = Format };
            ConfigProvider.SaveConfig(config);
            return config;
        });
    }

    public uint Quality
    {
        get => quality; set
        {
            if (value > 100)
                value = 100;
            if (value < 0)
                value = 0;
            this.RaiseAndSetIfChanged(ref quality, value);
        }
    }
    public MagickFormat Format
    {
        get => format; set
        {
            this.RaiseAndSetIfChanged(ref format, value);
        }
    }
    public ReactiveCommand<Unit, Config> ApplyCmd { get; set; }
}
public static class ConfigProvider
{
    public static Config LoadConfig()
    {
        if (File.Exists(Environment.CurrentDirectory + @"\Config.json"))
        {
            var json = JsonSerializer.Deserialize<Config>(File.ReadAllText(Environment.CurrentDirectory + @"\Config.json"));
            if (json != null) return json;
        }
        return new Config();
    }
    public static async void SaveConfig(Config config)
    {
        await using FileStream createStream = File.Create(Environment.CurrentDirectory + @"\Config.json");
        await JsonSerializer.SerializeAsync(createStream, config);
    }
}