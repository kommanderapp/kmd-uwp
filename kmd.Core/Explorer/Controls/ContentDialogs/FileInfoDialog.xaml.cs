using kmd.Core.Explorer.Contracts;
using System;
using Windows.Storage.FileProperties;

namespace kmd.Core.Explorer.Controls.ContentDialogs
{
    public sealed partial class FileInfoDialog
    {
        public FileInfoDialog(IExplorerItem item)
        {
            InitializeComponent();

            File = item;
        }

        public string Size => GetSizeFromByteCount(BasicProperties.Size);
        public BasicProperties BasicProperties
        {
            get
            {
                var task = File.StorageItem.GetBasicPropertiesAsync().AsTask();
                task.Wait();

                return task.Result;
            }
        }
        public IExplorerItem File { get; set; }

        private string GetSizeFromByteCount(ulong count)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = BasicProperties.Size;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return string.Format("{0:0.##} {1}", len, sizes[order]);
        }
    }
}
