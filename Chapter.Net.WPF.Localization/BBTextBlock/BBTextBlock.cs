// -----------------------------------------------------------------------------------------------------------------
// <copyright file="BBTextBlock.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Xaml;

// ReSharper disable once CheckNamespace

namespace Chapter.Net.WPF.Localization;

/// <summary>
///     This classes splits text with BB code into formatted parts.
///     Supported:
///     [B][/B] = Bold
///     [U][/U] = Underline
///     [I][/I] = Italic
///     [SUP][/SUP] = Superscript
///     [SUB][/SUB] = Subcript
///     [BR] = Linebreak
/// </summary>
/// <example>
///     <code lang="XAML">
/// <![CDATA[
/// <localization:BBTextBlock BBText="This is [B]Bold[/B]." />
/// ]]>
/// </code>
/// </example>
public class BBTextBlock : ContentControl
{
    /// <summary>
    ///     The DependencyProperty for the BBText property.
    /// </summary>
    public static readonly DependencyProperty BBTextProperty =
        DependencyProperty.Register(nameof(BBText), typeof(string), typeof(BBTextBlock), new PropertyMetadata(OnPropertyChanged));

    /// <summary>
    ///     The DependencyProperty for the ScriptFontSize property.
    /// </summary>
    public static readonly DependencyProperty ScriptFontSizeProperty =
        DependencyProperty.Register(nameof(ScriptFontSize), typeof(double), typeof(BBTextBlock), new PropertyMetadata(11d, OnPropertyChanged));

    /// <summary>
    ///     The DependencyProperty for the TextWrapping property.
    /// </summary>
    public static readonly DependencyProperty TextWrappingProperty =
        DependencyProperty.Register(nameof(TextWrapping), typeof(TextWrapping), typeof(BBTextBlock), new PropertyMetadata(TextWrapping.NoWrap, OnPropertyChanged));

    /// <summary>
    ///     The DependencyProperty for the TextAlignment property.
    /// </summary>
    public static readonly DependencyProperty TextAlignmentProperty =
        DependencyProperty.Register(nameof(TextAlignment), typeof(TextAlignment), typeof(BBTextBlock), new PropertyMetadata(TextAlignment.Left, OnPropertyChanged));

    private readonly List<Formatter> _formatters;

    /// <summary>
    ///     Creates a new instance of <see cref="BBTextBlock" />.
    /// </summary>
    public BBTextBlock()
    {
        _formatters =
        [
            new StartEndFormatter("[B]", "<Bold>", "[/B]", "</Bold>"),
            new StartEndFormatter("[U]", "<Underline>", "[/U]", "</Underline>"),
            new StartEndFormatter("[I]", "<Italic>", "[/I]", "</Italic>"),
            new StartEndFormatter("[SUP]", "<Run FontSize=\"\" BaselineAlignment=\"Superscript\">", "[/SUP]", "</Run>"),
            new StartEndFormatter("[SUB]", "<Run FontSize=\"\" BaselineAlignment=\"Subscript\">", "[/SUB]", "</Run>"),
            new SingleFormatter("[BR]", "<LineBreak />")
        ];

        Loaded += OnLoaded;
    }

    /// <summary>
    ///     Gets or sets the BBText to display.
    /// </summary>
    public string BBText
    {
        get => (string)GetValue(BBTextProperty);
        set => SetValue(BBTextProperty, value);
    }

    /// <summary>
    ///     Gets or sets the font size for the super-, and subscript text.
    /// </summary>
    public double ScriptFontSize
    {
        get => (double)GetValue(ScriptFontSizeProperty);
        set => SetValue(ScriptFontSizeProperty, value);
    }

    /// <summary>
    ///     Gets or sets the wrapping for the text.
    /// </summary>
    public TextWrapping TextWrapping
    {
        get => (TextWrapping)GetValue(TextWrappingProperty);
        set => SetValue(TextWrappingProperty, value);
    }

    /// <summary>
    ///     Gets or sets the alignment for the text.
    /// </summary>
    public TextAlignment TextAlignment
    {
        get => (TextAlignment)GetValue(TextAlignmentProperty);
        set => SetValue(TextAlignmentProperty, value);
    }

    private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (BBTextBlock)d;
        if (!control.IsLoaded)
            return;

        control.UpdateText();
    }

    private void UpdateText()
    {
        try
        {
            var text = BBText;
            if (text == null)
                return;

            foreach (var formatter in _formatters) text = formatter.Format(text);

            text = text.Replace("FontSize=\"\"", $"FontSize=\"{ScriptFontSize}px\"");

            Content = XamlServices.Parse(
                $"<TextBlock xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" TextWrapping=\"{TextWrapping}\" TextAlignment=\"{TextAlignment}\" >" +
                text +
                "</TextBlock>");
        }
        catch (Exception ex)
        {
            var text = BBText;
            if (text == null)
                return;

            foreach (var formatter in _formatters) text = formatter.Format(text);

            Content = new TextBlock
            {
                Text = text,
                TextWrapping = TextWrapping,
                TextAlignment = TextAlignment
            };
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        UpdateText();
    }

    private class SingleFormatter : Formatter
    {
        public SingleFormatter(string replace, string with)
        {
            Replace = replace;
            With = with;
        }

        public string Replace { get; }
        public string With { get; }

        public override string Format(string input)
        {
            input = input.Replace(Replace, With);
            return input;
        }
    }

    private class StartEndFormatter : Formatter
    {
        public StartEndFormatter(string begin, string beginReplace, string end, string endReplace)
        {
            Begin = begin;
            BeginReplace = beginReplace;
            End = end;
            EndReplace = endReplace;
        }

        public string Begin { get; }
        public string BeginReplace { get; }
        public string End { get; }
        public string EndReplace { get; }

        public override string Format(string input)
        {
            input = input.Replace(Begin, BeginReplace);
            input = input.Replace(End, EndReplace);
            return input;
        }
    }

    private abstract class Formatter
    {
        public abstract string Format(string input);
    }
}