using System;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Gritsenko.Universal.Common
{
    public static class ColorHelper
    {
        public static SolidColorBrush GetColorFromHexa(string hexaColor)
        {
            try
            {
                if (string.IsNullOrEmpty(hexaColor))
                {
                    return null;
                }

                if (hexaColor.Length == 9)
                {
                    return new SolidColorBrush(
                        Color.FromArgb(
                            Convert.ToByte(hexaColor.Substring(1, 2), 16),
                            Convert.ToByte(hexaColor.Substring(3, 2), 16),
                            Convert.ToByte(hexaColor.Substring(5, 2), 16),
                            Convert.ToByte(hexaColor.Substring(7, 2), 16)
                            )
                        );
                }

                return new SolidColorBrush(
                    Color.FromArgb(
                        255,
                        Convert.ToByte(hexaColor.Substring(1, 2), 16),
                        Convert.ToByte(hexaColor.Substring(3, 2), 16),
                        Convert.ToByte(hexaColor.Substring(5, 2), 16)
                        )
                    );
            }
            catch (Exception ex)
            {
                var message = "Ужас в [ColorHelper.GetColorFromHexa]: " + ex.Message;
                Logger.LogException(ex, message);
            }

            return null;
        }
    }
}