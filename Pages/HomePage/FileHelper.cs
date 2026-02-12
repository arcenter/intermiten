using Microsoft.Win32;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace intermiten.Pages
{
    public partial class HomePage : Page
    {
        private string save_path = $"{AppContext.BaseDirectory}\\data.json";

        public void FileSave()
        {
            File.WriteAllText(save_path, JsonSerializer.Serialize(checked_courses));
        }

        private void FileSaveAs()
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.Filter = "JSON (*.json)|*.json";
            saveFileDialog.InitialDirectory = Path.GetDirectoryName(save_path);
            saveFileDialog.FileName = Path.GetFileName(save_path);

            if (saveFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = saveFileDialog.FileName;
                string oldPath = save_path;
                try
                {
                    save_path = selectedFilePath;
                    File.WriteAllText(save_path, JsonSerializer.Serialize(checked_courses));
                }
                catch { save_path = oldPath; }
            }
        }
        
        private void FileOpen()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON (*.json)|*.json";
            openFileDialog.InitialDirectory = Path.GetDirectoryName(save_path);

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                string oldPath = save_path;
                try
                {
                    save_path = selectedFilePath;
                    LoadTimeTableData();
                }
                catch { save_path = oldPath; }
            }
        }

        private void ExportImage()
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.Filter = "PNG (*.png)|*.png";
            saveFileDialog.InitialDirectory = Path.GetDirectoryName(save_path);
            saveFileDialog.FileName = "timetable.png";

            if (saveFileDialog.ShowDialog() == true)
            {
                int dpi = 96 * 2;
                int width = (int)(timetableGrid.RenderSize.Width / 96 * dpi);
                int height = (int)(timetableGrid.RenderSize.Height / 96 * dpi);

                RenderTargetBitmap rtb = new(width, height, dpi, dpi, PixelFormats.Pbgra32);
                rtb.Render(timetableGrid);

                // -------------------------------------------------------------- Padding

                int padding = 100;

                // rtb = your original RenderTargetBitmap
                int srcW = rtb.PixelWidth;
                int srcH = rtb.PixelHeight;

                int dstW = srcW + padding * 2;
                int dstH = srcH + padding * 2;

                int bpp = (rtb.Format.BitsPerPixel + 7) / 8;
                int srcStride = srcW * bpp;
                byte[] srcPixels = new byte[srcStride * srcH];

                rtb.CopyPixels(srcPixels, srcStride, 0);

                // destination buffer (auto-black because byte[] defaults to 0)
                int dstStride = dstW * bpp;
                byte[] dstPixels = new byte[dstStride * dstH];

                for (int i = 0; i < dstPixels.Length; i += 4)
                {
                    dstPixels[i] = (byte)(18);
                    dstPixels[i + 1] = (byte)(18);
                    dstPixels[i + 2] = (byte)(18);
                    dstPixels[i + 3] = (byte)(255);
                }

                // copy row by row with offset
                for (int y = 0; y < srcH; y++)
                {
                    int srcIndex = y * srcStride;
                    int dstIndex = (y + padding) * dstStride + padding * bpp;
                    Buffer.BlockCopy(srcPixels, srcIndex, dstPixels, dstIndex, srcStride);
                }

                // create final bitmap
                WriteableBitmap final = new(
                    dstW,
                    dstH,
                    rtb.DpiX,
                    rtb.DpiY,
                    PixelFormats.Pbgra32,
                    null);

                final.WritePixels(
                    new Int32Rect(0, 0, dstW, dstH),
                    dstPixels,
                    dstStride,
                    0);

                // ----------------------------------------------------------------------

                PngBitmapEncoder encoder = new();
                encoder.Frames.Add(BitmapFrame.Create(final));
                using (FileStream stream = File.Create(saveFileDialog.FileName))
                    encoder.Save(stream);
            }
        }
    }
}
