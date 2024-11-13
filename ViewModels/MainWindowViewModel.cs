using ImageCompressor.Models;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Linq;
using System.Windows.Input;
namespace ImageCompressor.ViewModels;
public class MainWindowViewModel : ViewModelBase
{
    private bool isWorking = false;
    private string path = "";
    private string status = "";

    public MainWindowViewModel()
    {
        ShowConfigWindow = new();
        SearchCmd = ReactiveCommand.Create(() => Search(Path));
        ConfigCmd = ReactiveCommand.CreateFromTask(async () =>
        {
            var cofig = await ShowConfigWindow.Handle(Config);
            if (cofig != null) Config = cofig;
        });
    }
    public Config Config { get; set; } = ConfigProvider.LoadConfig();
    public bool IsWorking { get => isWorking; set => this.RaiseAndSetIfChanged(ref isWorking, value); }
    public ObservableCollection<FileInfo> Works { get; set; } = [];
    public string Status { get => status; set => this.RaiseAndSetIfChanged(ref status, value); }
    public string Path { get => path; set => this.RaiseAndSetIfChanged(ref path, value); }
    public ObservableCollection<FolderVM> FolderVMs { get; set; } = [];
    public ICommand SearchCmd { get; set; }
    public ICommand ConfigCmd { get; set; }
    public Interaction<Config, Config> ShowConfigWindow { get; } = new();
    public void WorkStrat(FileInfo file, ActionType action)
    {
        Works.Add(file);
        IsWorking = true;
        Status = $"[{Works.Count}] {file.FullName} {action}";
    }
    public void WorkEnd(FileInfo file, ActionType action, bool result = true)
    {
        Works.Remove(file);
        if (Works.Count == 0) IsWorking = false;
        Status = $"[{Works.Count}] {file.FullName} {action} {result}";
    }
    public void Search(string path)
    {
        try { FolderVMs.Add(new FolderVM(new Folder(path), this)); Status = $"{path} loaded"; }
        catch { Status = $"{path} load failed"; }
    }
}
