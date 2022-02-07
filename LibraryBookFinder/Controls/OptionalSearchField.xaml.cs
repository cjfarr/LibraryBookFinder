namespace LibraryBookFinder.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for OptionalSearchField.xaml
    /// </summary>
    public partial class OptionalSearchField : UserControl
    {
        public static readonly DependencyProperty SearchLabelProperty;
        public static readonly DependencyProperty SearchTextProperty;
        public static readonly DependencyProperty IsUsingFieldProperty;

        static OptionalSearchField()
        {
            SearchLabelProperty = DependencyProperty.Register(
                nameof(OptionalSearchField.SearchLabel),
                typeof(string),
                typeof(OptionalSearchField),
                new PropertyMetadata(string.Empty));

            SearchTextProperty = DependencyProperty.Register(
                nameof(OptionalSearchField.SearchText),
                typeof(string),
                typeof(OptionalSearchField),
                new PropertyMetadata(string.Empty));

            IsUsingFieldProperty = DependencyProperty.Register(
                nameof(OptionalSearchField.IsUsingField),
                typeof(bool),
                typeof(OptionalSearchField),
                new PropertyMetadata(false, new PropertyChangedCallback(OnIsUsingFieldCheckChanged)));
        }

        public OptionalSearchField()
        {
            this.InitializeComponent();
        }

        public string SearchLabel
        {
            get => (string)GetValue(SearchLabelProperty);
            set => SetValue(SearchLabelProperty, value);
        }

        public string SearchText
        {
            get => (string)GetValue(SearchTextProperty);
            set => SetValue(SearchTextProperty, value);
        }

        public bool IsUsingField
        {
            get => (bool)GetValue(IsUsingFieldProperty);
            set => SetValue(IsUsingFieldProperty, value);
        }

        public void FocusSearchTextBox()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle,
                new Action(delegate ()
                {
                    this.searchFieldTextBox.Focus();
                }));
        }

        private static void OnIsUsingFieldCheckChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            OptionalSearchField control = sender as OptionalSearchField;
            if (args.NewValue is bool isUsingField)
            {
                if (isUsingField)
                {
                    control.FocusSearchTextBox();
                }
                else
                {
                    control.SearchText = string.Empty;
                }
            }
        }
    }
}
