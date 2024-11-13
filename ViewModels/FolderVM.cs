using ImageCompressor.Models;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ImageCompressor.ViewModels;
public class FolderVM : ViewModelBase
{
    private bool isExpanded = true;
    private string expanderContent = "➖";

    public FolderVM(Folder folder, MainWindowViewModel mwvm)
    {
        SubFolders = [];
        ItemVMs = [];
        Name = folder.Dir.Name;
        foreach (var sd in folder.SubDirs)
            SubFolders.Add(new FolderVM(sd, mwvm));
        foreach (var file in folder.Items)
            ItemVMs.Add(new ItemVM(file, mwvm));
        SwitchItem = ReactiveCommand.Create(() =>
        {
            IsExpanded = !IsExpanded;
            ExpanderContent = isExpanded ? "➖" : "➕";
        });
    }
    public string Name { get; set; }
    public ObservableCollection<FolderVM> SubFolders { get; set; }
    public ObservableCollection<ItemVM> ItemVMs { get; set; }
    public bool IsExpanded { get => isExpanded; set => this.RaiseAndSetIfChanged(ref isExpanded, value); }
    public ICommand SwitchItem { get; set; }
    public string ExpanderContent { get => expanderContent; set => this.RaiseAndSetIfChanged(ref expanderContent, value); }
}
