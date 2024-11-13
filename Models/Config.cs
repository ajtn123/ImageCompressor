using ImageMagick;
namespace ImageCompressor.Models;
public class Config
{
    public MagickFormat Format { get; set; } = MagickFormat.Avif;
    public uint Quality { get; set; } = 100;
}
