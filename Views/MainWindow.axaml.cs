using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using ImageCompressor.Models;
using ImageCompressor.ViewModels;
using ReactiveUI;
using System.Threading.Tasks;
namespace ImageCompressor.Views;
public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(a => ViewModel!.ShowConfigWindow.RegisterHandler(ShowConfig));
        OpenButton.Click += OpenButton_Click;
    }
    private async Task ShowConfig(IInteractionContext<Config, Config> interaction)
    {
        var configVM = new ConfigVM(interaction.Input);
        var configWindow = new ConfigWindow { DataContext = configVM };
        var config = await configWindow.ShowDialog<Config>(this);
        interaction.SetOutput(config);
    }
    private async void OpenButton_Click(object sender, RoutedEventArgs args)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = GetTopLevel(this);
        // Start async operation to open the dialog.
        var folders = await topLevel!.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            //Title = "Open Folder",
            AllowMultiple = true
        });
        foreach (var folder in folders)
        {
            ViewModel.Search(folder.Path.ToString().Remove(0, 8));
        }
    }
}