using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Linq;

class Program
{
	static void Main(string[] args)
	{
		string inputFolder = @"C:\Projects\SkyMAUIData";     // change
		string outputFolder = @"C:\Projects\SkyMAUIData";   // change
		int quality = 75; // 1–100

		if (!Directory.Exists(outputFolder))
			Directory.CreateDirectory(outputFolder);

		var pngFiles = Directory.GetFiles(inputFolder, "*.png", SearchOption.TopDirectoryOnly);

		Console.WriteLine($"Found {pngFiles.Length} PNG files.");

		foreach (var file in pngFiles)
		{
			try
			{
				string fileName = Path.GetFileNameWithoutExtension(file);
				string outputPath = Path.Combine(outputFolder, fileName + ".jpg");

				using var image = Image.Load<Rgba32>(file);

				// Handle transparency (fill with white background)
				using var background = new Image<Rgb24>(image.Width, image.Height, Color.White);
				background.Mutate(ctx => ctx.DrawImage(image, 1f));

				var encoder = new JpegEncoder
				{
					Quality = quality
				};

				background.Save(outputPath, encoder);

				Console.WriteLine($"Converted: {fileName}.png -> {fileName}.jpg");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {file} - {ex.Message}");
			}
		}

		Console.WriteLine("Done!");
	}
}