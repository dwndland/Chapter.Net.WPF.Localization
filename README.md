<img src="https://raw.githubusercontent.com/dwndland/Chapter.Net.WPF.Localization/master/Icon.png" alt="logo" width="64"/>

# Chapter.Net.WPF.Localization Library

## Overview
Provides objects and helpers for an easy localization of the WPF application.

## Features
- **Maintain current culture:** Use the Localizer to set and get the CurrentCulture and CurrentLanguage.
- **Culture and language change events:** After set CurrentCulture and CurrentLanguage will raise also Changing and Changed events.
- **Load translations:** Using the Translator to load translations by a key from the application resources.
- **Names placeholders:** For proper localization the translations can contain named placeholders the Translator can format.
- **FormatterTextBlock:** Formats the given translation.
- **FormatterConverter:** Puts the given bound values to the Translator.Format.
- **BBTextBlock:** This classes splits text with BB code into formatted parts.

## Getting Started

1. **Installation:**
    - Install the Chapter.Net.WPF.Localization library via NuGet Package Manager:
    ```bash
    dotnet add package Chapter.Net.WPF.Localization
    ```

2. **Current Culture:**
    - Set the current culture.
    ```csharp
    Localizer.CurrentCulture = new CultureInfo("de-DE");
    ```
    - Get informed about that be changed.
    ```csharp
    Localizer.CultureChanged += OnCultureChanged;

    private void OnCultureChanged(CultureChangeEventArgs e)
    {
        //e.OldCulture
        //e.NewCulture
    }
    ```

3. **Current Culture:**
    - Set the current language.
    ```csharp
    Localizer.CurrentLanguage = new CultureInfo("de-DE");
    ```
    - Get informed about that be changed.
    ```csharp
    Localizer.LanguageChanged += OnLanguageChanged;

    private void OnLanguageChanged(CultureChangeEventArgs e)
    {
        //e.OldCulture
        //e.NewCulture
    }
    ```

4. **Load translations:**
    - In XAML, use the build in WPF features
    ```xaml
    <TextBlock Text="{DynamicResource {x:Static TransKeys+Demo.Title}}" />
    ```
    - In C# code, use the translator
    ```csharp
    var title = Translator.GetString(TransKeys.Demo.Title);
    ```

5. **Names placeholders:**
    - For proper translations, give the translator a hint what a placeholder stands for:
    ```xaml
    <system:String x:Key="{x:Static TransKeys+Demo.Title}">Do you want to open the {fileName}?</system:String>
    ```
    ```csharp
    var title = Translator.GetString(TransKeys.Demo.Title, "{fileName}", fileName);
    ```
    - If you have the string loaded already and want to replace a placeholder multiple times, it can be optimized
    ```csharp
    var translation = Translator.GetString(TransKeys.Demo.Title);
    var title1 = translation.Format(translation, "{fileName}", fileName1);
    var title2 = translation.Format(translation, "{fileName}", fileName2);
    var title3 = translation.Format(translation, "{fileName}", fileName3);
    ```

6. **FormatterTextBlock:**
    - Takes the given string and executes placeholder formating on it:
    ```xaml
    <ListBox ItemsSource="{Binding Patients}">
        <ListBox.ItemTemplate>
            <DataTemplate>
                <localization:FormatterTextBlock Formatter="{DynamicResource NameFormatter}">
                    <localization:FormatterPair Replace="{}{firstName}" With="{Binding FirstName}" />
                    <localization:FormatterPair Replace="{}{lastName}" With="{Binding LastName}" />
                </localization:FormatterTextBlock>
            </DataTemplate>
        </ListBox.ItemTemplate>
    </ListBox>
    ```

7. **FormatterConverter:**
    - Puts the given bound values to the Translator.Format:
    ```xaml
    <localization:FormatterConverter x:Key="FormatterConverter" />
    
    <TextBlock x:Name="nameText" Tag="{DynamicResource NameFormatter}">
        <TextBlock.Text>
            <MultiBinding Converter="{StaticResource FormatterConverter}">
                <Binding Path="Tag" ElementName="nameText" />
                <Binding>
                    <Binding.Source>
                        <system:String>{firstName}</system:String>
                    </Binding.Source>
                </Binding>
                <Binding Path="FirstName" />
                <Binding>
                    <Binding.Source>
                        <system:String>{lastName}</system:String>
                    </Binding.Source>
                </Binding>
                <Binding Path="LastName" />
            </MultiBinding>
        </TextBlock.Text>
    </TextBlock>
    ```

8. **BBTextBlock:**
    - This classes splits text with BB code into formatted parts:
    ```xaml
    <localization:BBTextBlock BBText="This is [B]Bold[/B]." />
    ```

## Example
- Create dictionary (best in an own dll) with the translation
```xaml
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                    xmlns:system="clr-namespace:System;assembly=System.Runtime"
                    xmlns:translations="clr-namespace:MyApplication.Common.Translations">

    <system:String x:Key="Something">something [VERSION]</system:String>

</ResourceDictionary>
```
- Use the dictionary in your application
```xaml
<Application xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">

    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="/MyApplication;component/Translations.xaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>

</Application>
```
or
```csharp
var dic = Application.Current.Resources.MergedDictionaries;
dic.Insert(0, new ResourceDictionary { Source = new Uri("/MyApplication;component/Translations.xaml", UriKind.RelativeOrAbsolute) });
```
- Use the translation with formatting
```csharp
Translator.GetString("Something", "[VERSION]", version.ToString(3))
```

## FAQ
**Q:** Whats the difference between CurrentCulture and CurrentLanguage?  
**A:** CurrentCulture set the CultureInfo.CurrentCulture and more. That culture is to use for like formatting decimal places, date formats, time formats and time zones.  
The CurrentLanguage sets the CultureInfo.CurrentUICulture and defines for which language the translations shall be shown.  

**Q:** Why CurrentCulture and CurrentLanguage are separated from each other?  
**A:** It can happen that the application runs on a English Windows System with its format, but the user logging in on the application sees the texts in Japanese.  
In that cases its a best practice to show the culture like in windows, English US and change only the translations.  
Localizer.CurrentCulture = new CultureInfo("en-US");  
Localizer.CurrentLanguage = new CultureInfo("ja-JP");  

**Q:** I loaded my translation.dll into passolo but no string is shown from the Translation.xaml. Whats wrong?  
**A:** Passolo checkes the translations using their ID, so beside the x:Key, set also the x:Uid, then all translations are shown.

**Q:** Why should I use named placeholders?  
**A:** You can use placeholders like {0} of course, but its better for the translator to know what {0} can be, to find the proper translation in the right grammar.  
In that case a named placeholder gives pretty nice hints what it can be. Like {number}, {fileName}, {userName} or such.

## Links
* [NuGet](https://www.nuget.org/packages/Chapter.Net.WPF.Localization)
* [GitHub](https://github.com/dwndland/Chapter.Net.WPF.Localization)

## License
Copyright (c) David Wendland. All rights reserved.
Licensed under the MIT License. See LICENSE file in the project root for full license information.
