using DiffMatchPatch;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using kmd.Core.Services.Contracts;
using kmd.Storage.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace kmd.Core.Comparer
{
    public class ComparerViewModel : ViewModelBase
    {
        private readonly IFilePickerService _filePickerService;
        private readonly IDialogService _dialogService;
        private readonly IDarkThemeResolver _darkThemeResolver;

        private readonly diff_match_patch _differ = new diff_match_patch()
        {
            Diff_Timeout = 5.0f // 5 seconds
        };

        public ICommand PickFile1 { get; }
        public ICommand PickFile2 { get; }

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                Set(ref _isBusy, value);
            }
        }

        private bool _isBusy;

        public StorageFile File1
        {
            get
            {
                return _file1;
            }
            set
            {
                Set(ref _file1, value);
                if (value != null && File2 != null) CompareAsync();
            }
        }

        private StorageFile _file1;

        public StorageFile File2
        {
            get
            {
                return _file2;
            }
            set
            {
                Set(ref _file2, value);
                if (value != null && File1 != null) CompareAsync();
            }
        }

        private StorageFile _file2;

        public string File1DiffHtml
        {
            get
            {
                return _file1DiffHtml;
            }
            set
            {
                Set(ref _file1DiffHtml, value);
            }
        }

        private string _file1DiffHtml;

        public string File2DiffHtml
        {
            get
            {
                return _file2DiffHtml;
            }
            set
            {
                Set(ref _file2DiffHtml, value);
            }
        }

        private string _file2DiffHtml;

        public ComparerViewModel(IFilePickerService filePickerService,
            IDialogService dialogService,
            IDarkThemeResolver darkThemeResolver)
        {
            _darkThemeResolver = darkThemeResolver ?? throw new ArgumentNullException(nameof(darkThemeResolver));
            _filePickerService = filePickerService ?? throw new ArgumentNullException(nameof(filePickerService));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            PickFile1 = new RelayCommand(async () => { File1 = (StorageFile)await _filePickerService.PickSingleAsync(); });
            PickFile2 = new RelayCommand(async () => { File2 = (StorageFile)await _filePickerService.PickSingleAsync(); });
        }

        private async Task CompareAsync()
        {
            if (File1 == null || File2 == null) return;
            IsBusy = true;

            try
            {
                var file1Content = await FileIO.ReadTextAsync(File1, Windows.Storage.Streams.UnicodeEncoding.Utf8);
                var file2Content = await FileIO.ReadTextAsync(File2, Windows.Storage.Streams.UnicodeEncoding.Utf8);

                var diffs1 = _differ.diff_main(file1Content, file2Content);
                File1DiffHtml = diffs1.ToPrettyHtml(_darkThemeResolver.IsDarkMode());

                var diffs2 = _differ.diff_main(file2Content, file1Content);
                File2DiffHtml = diffs2.ToPrettyHtml(_darkThemeResolver.IsDarkMode());
            }
            catch (Exception exc)
            {
                await _dialogService.ShowError("Wrong File types.", "Oops", "Ok", null);
            }

            IsBusy = false;
        }
    }
}
