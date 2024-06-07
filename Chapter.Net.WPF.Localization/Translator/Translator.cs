// -----------------------------------------------------------------------------------------------------------------
// <copyright file="Translator.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System.Windows;

// ReSharper disable once CheckNamespace

namespace Chapter.Net.WPF.Localization;

/// <summary>
///     Provides ways to load translations from the application resources.
/// </summary>
public static class Translator
{
    /// <summary>
    ///     Loads the translation by its string key.
    /// </summary>
    /// <param name="key">The key of the translation.</param>
    /// <returns>The translations if available; otherwise the key.</returns>
    public static string GetString(string key)
    {
        var application = Application.Current;
        if (application == null)
            return key;

        if (application.Resources.Contains(key))
            return application.Resources[key] as string;

        return key;
    }

    /// <summary>
    ///     Loads the translation by its string key.
    /// </summary>
    /// <param name="key">The key of the translation.</param>
    /// <returns>The translations if available; otherwise the key.</returns>
    public static string GetString(ComponentResourceKey key)
    {
        var application = Application.Current;
        if (application == null)
            return key.ToString();

        if (application.Resources.Contains(key))
            return application.Resources[key] as string;

        return key.ToString();
    }

    /// <summary>
    ///     Loads the translation by its string key and applies formatting on it.
    /// </summary>
    /// <param name="key">The key of the translation.</param>
    /// <param name="formattings">The list of pairs 'replace' - 'with'.</param>
    /// <returns>The translations if available; otherwise the key.</returns>
    public static string GetString(string key, params string[] formattings)
    {
        var translation = GetString(key);
        return Format(translation, formattings);
    }

    /// <summary>
    ///     Loads the translation by its string key and applies formatting on it.
    /// </summary>
    /// <param name="key">The key of the translation.</param>
    /// <param name="formattings">The list of pairs 'replace' - 'with'.</param>
    /// <returns>The translations if available; otherwise the key.</returns>
    public static string GetString(ComponentResourceKey key, params string[] formattings)
    {
        var translation = GetString(key);
        return Format(translation, formattings);
    }

    /// <summary>
    ///     Applies formatting on the translation.
    /// </summary>
    /// <param name="translation">The translation.</param>
    /// <param name="formattings">The list of pairs 'replace' - 'with'.</param>
    /// <returns>The translation with the formattings.</returns>
    public static string Format(string translation, params string[] formattings)
    {
        for (int i = 0, j = 1; j < formattings.Length; i += 2, j += 2)
            translation = translation.Replace(formattings[i], formattings[j]);
        return translation;
    }
}