using Avalonia.ReactiveUI;
using ImageCompressor.ViewModels;

namespace ImageCompressor.Views;

public partial class ItemView : ReactiveUserControl<ItemVM>
{
    public ItemView()
    {
        InitializeComponent();
    }
}