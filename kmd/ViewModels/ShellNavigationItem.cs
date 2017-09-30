using GalaSoft.MvvmLight;

using kmd.Services;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace kmd.ViewModels
{
    public class ShellNavigationItem : ViewModelBase
    {
        public ShellNavigationItem(string label, Symbol symbol, string viewModelName)
            : this(label, viewModelName)
        {
            Symbol = symbol;
        }

        public ShellNavigationItem(string label, IconElement icon, string viewModelName)
            : this(label, viewModelName)
        {
            _iconElement = icon;
        }

        public ShellNavigationItem(string label, string viewModelName)
        {
            Label = label;
            ViewModelName = viewModelName;

            ThemeSelectorService.OnThemeChanged += (s, e) =>
            {
                if (!IsSelected)
                {
                    SelectedForeground = GetStandardTextColorBrush();
                }
            };
        }

        public IconElement Icon
        {
            get
            {
                var foregroundBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath("SelectedForeground"),
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                };

                if (_iconElement != null)
                {
                    BindingOperations.SetBinding(_iconElement, IconElement.ForegroundProperty, foregroundBinding);

                    return _iconElement;
                }

                var fontIcon = new FontIcon { FontSize = 16, Glyph = SymbolAsChar.ToString() };

                BindingOperations.SetBinding(fontIcon, FontIcon.ForegroundProperty, foregroundBinding);

                return fontIcon;
            }
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                Set(ref _isSelected, value);

                SelectedVis = value ? Visibility.Visible : Visibility.Collapsed;

                SelectedForeground = IsSelected
                    ? Application.Current.Resources["SystemControlForegroundAccentBrush"] as SolidColorBrush
                    : GetStandardTextColorBrush();
            }
        }

        public string Label { get; set; }

        public SolidColorBrush SelectedForeground
        {
            get { return _selectedForeground ?? (_selectedForeground = GetStandardTextColorBrush()); }

            set { Set(ref _selectedForeground, value); }
        }

        public Visibility SelectedVis
        {
            get { return _selectedVis; }

            set { Set(ref _selectedVis, value); }
        }

        public Symbol Symbol { get; set; }

        public char SymbolAsChar
        {
            get { return (char)Symbol; }
        }

        public string ViewModelName { get; set; }

        public override string ToString()
        {
            return Label;
        }

        private IconElement _iconElement = null;
        private bool _isSelected;
        private SolidColorBrush _selectedForeground = null;
        private Visibility _selectedVis = Visibility.Collapsed;

        private SolidColorBrush GetStandardTextColorBrush()
        {
            return ThemeSelectorService.GetSystemControlForegroundForTheme();
        }
    }
}
