// -----------------------------------------------------------------------------------------------------------------
// <copyright file="CultureChangeEventArgs.cs" company="dwndland">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;

// ReSharper disable once CheckNamespace

namespace Chapter.Net.WPF.Localization;

/// <summary>
///     The event parameter raised with the changing and changed events within the <see cref="Localizer" />.
/// </summary>
public sealed class CultureChangeEventArgs : EventArgs
{
    internal CultureChangeEventArgs(CultureInfo oldCulture, CultureInfo newCulture)
    {
        OldCulture = oldCulture;
        NewCulture = newCulture;
    }

    /// <summary>
    ///     Gets the previous culture.
    /// </summary>
    public CultureInfo OldCulture { get; }

    /// <summary>
    ///     Gets the new culture.
    /// </summary>
    public CultureInfo NewCulture { get; }
}