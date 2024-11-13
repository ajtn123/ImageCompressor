using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace ImageCompressor.Models;
public class Folder
{
    public Folder(string path)
    {
        Dir = new DirectoryInfo(path);
        SubDirs = [];
        Items = [.. Dir.GetFiles(".").Where(fi => (fi.Attributes & (FileAttributes.Hidden | FileAttributes.System)) == 0)];
        var sds = Directory.EnumerateDirectories(path, "*", SearchOption.TopDirectoryOnly);
        foreach (var sd in sds)
        {
            SubDirs.Add(new Folder(sd));
        }
    }
    public DirectoryInfo Dir { get; set; }
    public List<Folder> SubDirs { get; set; }
    public List<FileInfo> Items { get; set; }
}
