using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace kmd.Core.Explorer.Controls.ContentDialogs
{
    public enum NameCollisionDialogResult
    {
        Replace, Rename, Skip, ReplaceAll, RenameAll, SkipAll
    }

    public sealed partial class NameCollisionDialog : ContentDialog
    {
        public NameCollisionDialog(string filename, bool isForSingleFile = true)
        {
            InitializeComponent();

            Filename = string.IsNullOrEmpty(filename) ? throw new ArgumentException(nameof(filename)) : filename;
            ShowCheckbox = !isForSingleFile;
            PrimaryButtonText = "Ok";
        }

        public string Filename { get; set; }
        public List<string> Options => new List<string> { "Replace", "Rename", "Skip" };
        public bool? ForAll { get; set; } = false;
        public bool ShowCheckbox { get; set; }
        public NameCollisionDialogResult Result { get; private set; }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ForAll == true)
                Result = (NameCollisionDialogResult)((sender as ListView).SelectedIndex + 3);
            else
                Result = (NameCollisionDialogResult)(sender as ListView).SelectedIndex;
        }
    }
}
