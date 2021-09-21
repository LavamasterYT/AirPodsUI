using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AirPodsUI.Settings
{
    public class Dialog
    {
        public static async Task<ContentDialogResult> ShowDialogAsync(string title, string content, string cancelText, string primaryText = null, string secondaryText = null)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = title,
                Content = content,
                CloseButtonText = cancelText
            };

            if (!string.IsNullOrEmpty(primaryText))
            {
                dialog.PrimaryButtonText = primaryText;
            }
            if (!string.IsNullOrEmpty(secondaryText))
            {
                dialog.SecondaryButtonText = secondaryText;
            }

            return await dialog.ShowAsync();
        }
    }
}
