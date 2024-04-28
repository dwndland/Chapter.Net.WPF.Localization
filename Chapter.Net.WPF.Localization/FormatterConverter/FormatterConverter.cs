// -----------------------------------------------------------------------------------------------------------------
// <copyright file="FormatterConverter.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

// ReSharper disable once CheckNamespace

namespace Chapter.Net.WPF.Localization
{
    /// <summary>
    ///     Puts the given bound values to the Translator.Format.
    /// </summary>
    /// <example>
    ///     <code lang="XAML">
    /// <![CDATA[
    /// <localization:FormatterConverter x:Key="FormatterConverter" />
    /// 
    /// <TextBlock x:Name="nameText" Tag="{DynamicResource NameFormatter}">
    ///     <TextBlock.Text>
    ///         <MultiBinding Converter="{StaticResource FormatterConverter}">
    ///             <Binding Path="Tag" ElementName="nameText" />
    ///             <Binding>
    ///                 <Binding.Source>
    ///                     <system:String>{firstName}</system:String>
    ///                 </Binding.Source>
    ///             </Binding>
    ///             <Binding Path="FirstName" />
    ///             <Binding>
    ///                 <Binding.Source>
    ///                     <system:String>{lastName}</system:String>
    ///                 </Binding.Source>
    ///             </Binding>
    ///             <Binding Path="LastName" />
    ///         </MultiBinding>
    ///     </TextBlock.Text>
    /// </TextBlock >
    /// ]]>
    /// </code>
    /// </example>
    [ValueConversion(typeof(string[]), typeof(string))]
    public class FormatterConverter : IMultiValueConverter
    {
        /// <summary>
        ///     Puts the given bound values to the Translator.Format.
        /// </summary>
        /// <param name="values">The translation and replace pairs.</param>
        /// <param name="targetType">Unused target type.</param>
        /// <param name="parameter">Unused parameter.</param>
        /// <param name="culture">Unused culture.</param>
        /// <returns>The formatted translation</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length == 0 || values.Any(x => x == null))
                return string.Empty;

            if (values.Length == 1)
                return values[0];

            var translation = values[0].ToString();
            var pairs = values.Skip(1).Select(x => x.ToString()).ToArray();
            return Translator.Format(translation, pairs);
        }

        /// <summary>
        ///     Not implemented ConvertBack.
        /// </summary>
        /// <param name="value">Unused value.</param>
        /// <param name="targetTypes">Unused target types.</param>
        /// <param name="parameter">Unused parameter.</param>
        /// <param name="culture">Unused culture.</param>
        /// <returns>Nothing but an exception.</returns>
        /// <exception cref="NotImplementedException">ConvertBack is not implemented.</exception>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}