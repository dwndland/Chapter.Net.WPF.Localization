﻿// -----------------------------------------------------------------------------------------------------------------
// <copyright file="FormatterTextBlock.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

// ReSharper disable once CheckNamespace

namespace Chapter.Net.WPF.Localization
{
    /// <summary>
    ///     Formats the given translation.
    /// </summary>
    /// <example>
    ///     <code lang="XAML">
    /// <![CDATA[
    /// <ListBox ItemsSource="{Binding Patients}">
    ///     <ListBox.ItemTemplate>
    ///         <DataTemplate>
    ///             <localization:FormatterTextBlock Formatter="{DynamicResource NameFormatter}">
    ///                 <localization:FormatterPair Replace="{}{firstName}" With="{Binding FirstName}" />
    ///                 <localization:FormatterPair Replace="{}{lastName}" With="{Binding LastName}" />
    ///             </localization:FormatterTextBlock>
    ///         </DataTemplate>
    ///     </ListBox.ItemTemplate>
    /// </ListBox>
    /// ]]>
    /// </code>
    /// </example>
    [ContentProperty(nameof(Pairs))]
    public class FormatterTextBlock : TextBlock
    {
        /// <summary>
        ///     The Formatter dependency property.
        /// </summary>
        public static readonly DependencyProperty FormatterProperty =
            DependencyProperty.Register("Formatter", typeof(string), typeof(FormatterTextBlock), new PropertyMetadata(OnTextChanged));

        /// <summary>
        ///     The Pairs dependency property.
        /// </summary>
        public static readonly DependencyProperty PairsProperty =
            DependencyProperty.Register("Pairs", typeof(IList), typeof(FormatterTextBlock), new PropertyMetadata(OnPairsChanged));

        /// <summary>
        ///     Creates a new FormatterTextBlock.
        /// </summary>
        public FormatterTextBlock()
        {
            Pairs = new FormatterPairCollection();
            Loaded += OnLoaded;
        }

        /// <summary>
        ///     The translation to format.
        /// </summary>
        public string Formatter
        {
            get => (string)GetValue(FormatterProperty);
            set => SetValue(FormatterProperty, value);
        }

        /// <summary>
        ///     The list of pairs.
        /// </summary>
        public IList Pairs
        {
            get => (IList)GetValue(PairsProperty);
            set => SetValue(PairsProperty, value);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateTranslation();
        }

        private static void OnPairsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (FormatterTextBlock)d;

            if (e.OldValue is INotifyCollectionChanged oldValue)
                oldValue.CollectionChanged -= control.OnPairsCollectionChanged;
            if (e.NewValue is INotifyCollectionChanged newValue)
                newValue.CollectionChanged += control.OnPairsCollectionChanged;

            control.SubScribe(control.Pairs?.Cast<FormatterPair>() ?? new FormatterPair[0]);
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (FormatterTextBlock)d;
            control.UpdateTranslation();
        }

        private void OnPairsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    SubScribe(e.NewItems?.Cast<FormatterPair>() ?? new FormatterPair[0]);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    UnsubScribe(e.OldItems?.Cast<FormatterPair>() ?? new FormatterPair[0]);
                    break;
                case NotifyCollectionChangedAction.Reset:
                case NotifyCollectionChangedAction.Replace:
                    SubScribe(e.NewItems?.Cast<FormatterPair>() ?? new FormatterPair[0]);
                    UnsubScribe(e.OldItems?.Cast<FormatterPair>() ?? new FormatterPair[0]);
                    break;
            }

            UpdateTranslation();
        }

        private void SubScribe(IEnumerable<FormatterPair> pairs)
        {
            foreach (var pair in pairs)
                pair.DataChanged += OnItemDataChanged;
        }

        private void UnsubScribe(IEnumerable<FormatterPair> pairs)
        {
            foreach (var pair in pairs)
                pair.DataChanged -= OnItemDataChanged;
        }

        private void OnItemDataChanged(object sender, EventArgs e)
        {
            UpdateTranslation();
        }

        private void UpdateTranslation()
        {
            var pairs = Pairs?.Cast<FormatterPair>().ToList() ?? new List<FormatterPair>();
            if (pairs.Count == 0 || pairs.Any(x => x.Replace == null || x.With == null))
                return;

            var formattingPairs = pairs.Select(x => new[] { x.Replace, x.With }).SelectMany(x => x).ToArray();
            Text = Format(Formatter, formattingPairs);
        }

        private string Format(string translation, params string[] formattings)
        {
            for (int i = 0, j = 1; j < formattings.Length; i += 2, j += 2)
                translation = translation.Replace(formattings[i], formattings[j]);
            return translation;
        }
    }
}