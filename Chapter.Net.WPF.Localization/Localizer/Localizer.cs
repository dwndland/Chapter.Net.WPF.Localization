// -----------------------------------------------------------------------------------------------------------------
// <copyright file="Localizer.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Threading;

// ReSharper disable once CheckNamespace

namespace Chapter.Net.WPF.Localization;

/// <summary>
///     Provides way how to get and set the application culture and language.
/// </summary>
public static class Localizer
{
    /// <summary>
    ///     Gets or sets the current application culture.
    /// </summary>
    public static CultureInfo CurrentCulture
    {
        get => CultureInfo.CurrentCulture;
        set
        {
            var oldCulture = CultureInfo.CurrentCulture;
            if (!Equals(oldCulture, value))
            {
                CultureChanging?.Invoke(null, new CultureChangeEventArgs(oldCulture, value));
                CultureInfo.CurrentCulture = value;
                CultureInfo.DefaultThreadCurrentCulture = value;
                Thread.CurrentThread.CurrentCulture = value;
                CultureChanged?.Invoke(null, new CultureChangeEventArgs(oldCulture, value));
            }
        }
    }

    /// <summary>
    ///     Gets or sets the current application language.
    /// </summary>
    public static CultureInfo CurrentLanguage
    {
        get => CultureInfo.CurrentUICulture;
        set
        {
            var currentLanguage = CultureInfo.CurrentUICulture;
            if (!Equals(currentLanguage, value))
            {
                LanguageChanging?.Invoke(null, new CultureChangeEventArgs(currentLanguage, value));
                CultureInfo.CurrentUICulture = value;
                CultureInfo.DefaultThreadCurrentUICulture = value;
                Thread.CurrentThread.CurrentUICulture = value;
                LanguageChanged?.Invoke(null, new CultureChangeEventArgs(currentLanguage, value));
            }
        }
    }

    /// <summary>
    ///     Raised if the current language is about to change.
    /// </summary>
    public static event EventHandler<CultureChangeEventArgs> LanguageChanging;

    /// <summary>
    ///     Raised if the current language got changed.
    /// </summary>
    public static event EventHandler<CultureChangeEventArgs> LanguageChanged;

    /// <summary>
    ///     Raised if the current culture is about to change.
    /// </summary>
    public static event EventHandler<CultureChangeEventArgs> CultureChanging;

    /// <summary>
    ///     Raised if the current culture got changed.
    /// </summary>
    public static event EventHandler<CultureChangeEventArgs> CultureChanged;
}