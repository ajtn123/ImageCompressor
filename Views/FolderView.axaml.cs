using Avalonia.ReactiveUI;
using ImageCompressor.ViewModels;

namespace ImageCompressor.Views;

public partial class FolderView : ReactiveUserControl<FolderVM>
{
    public FolderView()
    {
        InitializeComponent();
    }
}