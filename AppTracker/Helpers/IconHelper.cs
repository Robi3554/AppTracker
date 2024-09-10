using System.Drawing;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows;

public static class IconHelper
{
    public static BitmapSource GetIconBitmapSource(string filePath)
    {
        try
        {
            using (var icon = Icon.ExtractAssociatedIcon(filePath))
            {
                if (icon != null)
                {
                    return Imaging.CreateBitmapSourceFromHIcon(
                        icon.Handle,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting icon: {ex.Message}");
        }
        return null;
    }
}
