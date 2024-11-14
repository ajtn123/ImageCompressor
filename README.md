# ImageCompressor

Compressor Image using Magick.NET library.

📄 - Encode image in specified format and quality. (Default: .avif, 100)

🏞️ - Optimize image file-size losslessly, do not change its format. (Only for: png, jpg, ico, gif)

`ImageCompressor.exe <Path> [<Action>(Compression|Optimization)]` Could be used in commandline, default is same as 🏞️.

If AlwaysSave is false (default), then the compression result will not be saved if its file-size is larger than the original image.

Built on .net 8 and Avalonia.
