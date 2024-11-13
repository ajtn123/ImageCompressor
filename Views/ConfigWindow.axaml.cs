using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using ImageCompressor.ViewModels;
using ImageMagick;
using ReactiveUI;
using System;
using System.Linq;
namespace ImageCompressor.Views;
public partial class ConfigWindow : ReactiveWindow<ConfigVM>
{
    public ConfigWindow()
    {
        InitializeComponent();
        this.WhenActivated(a => ViewModel!.ApplyCmd.Subscribe(Close));
        FormatButton.ItemsSource = Enum.GetValues(typeof(MagickFormat)).Cast<MagickFormat>();
    }
}