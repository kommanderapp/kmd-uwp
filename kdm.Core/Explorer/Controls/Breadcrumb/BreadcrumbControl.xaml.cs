using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace kmd.Core.Explorer.Controls
{
    public partial class BreadcrumbControl : UserControl
    {
        public BreadcrumbControl()
        {
            this.InitializeComponent();
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(BreadcrumbControl),
                new PropertyMetadata(null, OnItemsSourcePropertyChanged));

        private static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as BreadcrumbControl;
            control?.OnItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
        }

        private void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            if (oldValue is INotifyCollectionChanged value) value.CollectionChanged -= ItemsOnCollectionChanged;

            this.Items = new ObservableCollection<object>(newValue as IEnumerable<object>);
            if (newValue is INotifyCollectionChanged)
                (newValue as INotifyCollectionChanged).CollectionChanged += ItemsOnCollectionChanged;
            Initialize();
        }

        private void ItemsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        if (e.NewItems != null)
                        {
                            var index = e.NewStartingIndex;
                            foreach (var item in e.NewItems)
                            {
                                this.Items.Insert(index, item);
                                index++;
                            }
                        }
                        break;
                    }
                case NotifyCollectionChangedAction.Move:
                    {
                        var item = this.Items[e.OldStartingIndex];
                        this.Items.RemoveAt(e.OldStartingIndex);
                        this.Items.Insert(e.NewStartingIndex, item);
                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        if (e.OldItems != null)
                        {
                            foreach (var item in e.OldItems)
                            {
                                this.Items.Remove(item);
                            }
                        }
                        break;
                    }
                case NotifyCollectionChangedAction.Replace:
                    break;

                case NotifyCollectionChangedAction.Reset:
                    this.Items.Clear();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            Initialize();
        }

        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<object>), typeof(BreadcrumbControl), new PropertyMetadata(new ObservableCollection<object>()));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(BreadcrumbControl), new PropertyMetadata(null));

        public DataTemplate SeperatorTemplate
        {
            get { return (DataTemplate)GetValue(SeperatorTemplateProperty); }
            set { SetValue(SeperatorTemplateProperty, value); }
        }

        public readonly DependencyProperty SeperatorTemplateProperty =
            DependencyProperty.Register(nameof(SeperatorTemplate), typeof(DataTemplate), typeof(BreadcrumbControl), new PropertyMetadata(null));

        public DataTemplate OverFlowTemplate
        {
            get { return (DataTemplate)GetValue(OverFlowTemplateProperty); }
            set { SetValue(OverFlowTemplateProperty, value); }
        }

        public readonly DependencyProperty OverFlowTemplateProperty =
            DependencyProperty.Register(nameof(OverFlowTemplate), typeof(DataTemplate), typeof(BreadcrumbControl), new PropertyMetadata(null));

        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        public readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register(nameof(DisplayMemberPath), typeof(string), typeof(BreadcrumbControl), new PropertyMetadata(null));

        public string Seperator
        {
            get { return (string)GetValue(SeperatorProperty); }
            set { SetValue(SeperatorProperty, value); }
        }

        public readonly DependencyProperty SeperatorProperty =
            DependencyProperty.Register(nameof(Seperator), typeof(string), typeof(BreadcrumbControl), new PropertyMetadata("/"));

        public string OverFlow
        {
            get { return (string)GetValue(OverFlowProperty); }
            set { SetValue(OverFlowProperty, value); }
        }

        public readonly DependencyProperty OverFlowProperty =
            DependencyProperty.Register(nameof(OverFlow), typeof(string), typeof(BreadcrumbControl), new PropertyMetadata("..."));

        public Style ItemStyle
        {
            get { return (Style)GetValue(ItemStyleProperty); }
            set { SetValue(ItemStyleProperty, value); }
        }

        public readonly DependencyProperty ItemStyleProperty =
            DependencyProperty.Register(nameof(ItemStyle), typeof(Style), typeof(BreadcrumbControl), new PropertyMetadata(null));

        public ICommand ItemCommand
        {
            get { return (ICommand)GetValue(ItemCommandProperty); }
            set { SetValue(ItemCommandProperty, value); }
        }

        public readonly DependencyProperty ItemCommandProperty =
          DependencyProperty.Register("ItemCommand", typeof(ICommand), typeof(BreadcrumbControl), new PropertyMetadata(null));

        protected void Initialize()
        {
            this.StackPanel.Children.Clear();

            if (!this.Items.Any()) return;

            foreach (var item in this.Items)
            {
                SetItem(this.StackPanel.Children, item, this.Items, this.ItemTemplate, this.DisplayMemberPath);
                SetSeperatorItem(this.StackPanel.Children, this.SeperatorTemplate, this.Seperator);
            }

            // Update layout to be able to measure the actual width
            this.UpdateLayout();

            var items = this.StackPanel.Children.OfType<BreadcrumbItem>().ToList();
            var seperators = this.StackPanel.Children.OfType<BreadcrumbSeperator>().ToList();
            int i = 0;
            while (this.StackPanel.ActualWidth > this.ActualWidth)
            {
                if (i < items.Count - 1)
                {
                    var item = items?[i];

                    if (item == null) break;

                    SetOverflowItem(item, this.OverFlowTemplate, this.OverFlow);
                    i++;
                }
                else
                {
                    var item = items.FirstOrDefault(b => b.Visibility == Visibility.Visible);
                    var seperator = seperators.FirstOrDefault(b => b.Visibility == Visibility.Visible);
                    seperator.Visibility = Visibility.Collapsed;
                    item.Visibility = Visibility.Collapsed;
                }

                this.UpdateLayout();
            }
        }

        private static void SetSeperatorItem(UIElementCollection collection, DataTemplate seperatorTemplate, string seperator)
        {
            if (seperatorTemplate != null)
            {
                collection.Add(new BreadcrumbSeperator()
                {
                    ContentTemplate = seperatorTemplate,
                    VerticalAlignment = VerticalAlignment.Center
                });
            }
            else
            {
                collection.Add(new BreadcrumbSeperator()
                {
                    Content = seperator,
                    VerticalAlignment = VerticalAlignment.Center
                });
            }
        }

        private void SetOverflowItem(BreadcrumbItem item, DataTemplate overflowTemplate, string overflow)
        {
            item.Content = null;
            item.ContentTemplate = null;

            if (overflowTemplate != null)
                item.ContentTemplate = overflowTemplate;
            else
                item.Content = overflow;
        }

        private void SetItem(UIElementCollection collection, object item, IList<object> items,
            DataTemplate itemTemplate, string displayMemberPath)
        {
            var content = CreateButton();

            if (itemTemplate != null)
            {
                content.ContentTemplate = itemTemplate;
                content.DataContext = item;
            }
            else
            {
                content.SetBinding(ContentProperty, new Binding()
                {
                    Source = item,
                    Path = new PropertyPath(displayMemberPath)
                });
            }

            content.Click += (sender, args) => OnItemSelected(new BreadcrumbEventArgs(item, items.IndexOf(item)));
            collection.Add(content);
        }

        private BreadcrumbItem CreateButton()
        {
            return new BreadcrumbItem()
            {
                Style = this.ItemStyle
            };
        }

        protected virtual void OnItemSelected(BreadcrumbEventArgs e)
        {
            if (this.ItemCommand == null) return;
            if (this.ItemCommand.CanExecute(null)) this.ItemCommand.Execute(e.Item);
        }
    }
}