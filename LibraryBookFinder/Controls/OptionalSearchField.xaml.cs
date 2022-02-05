﻿namespace LibraryBookFinder.Controls
{
    using System.Windows;
    using System.Windows.Controls;

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
                typeof(OptionalSearchField));
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
    }
}
