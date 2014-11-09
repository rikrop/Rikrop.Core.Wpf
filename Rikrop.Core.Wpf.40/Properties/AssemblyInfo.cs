﻿using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Rikrop.Core.Wpf")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Rikrop.Core.Wpf")]
[assembly: AssemblyCopyright("Copyright ©  2012")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

//In order to begin building localizable applications, set 
//<UICulture>CultureYouAreCodingWith</UICulture> in your .csproj file
//inside a <PropertyGroup>.  For example, if you are using US english
//in your source files, set the <UICulture> to en-US.  Then uncomment
//the NeutralResourceLanguage attribute below.  Update the "en-US" in
//the line below to match the UICulture setting in the project file.

//[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]


[assembly:ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                             //(used if a resource is not found in the page, 
                             // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                      //(used if a resource is not found in the page, 
                                      // app, or any theme specific resource dictionaries)
)]


// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: InternalsVisibleTo("Rikrop.Core.Wpf.Test")]

[assembly: XmlnsPrefix("http://Rikrop/Core/wpf/", "wpf")]
[assembly: XmlnsDefinition("http://Rikrop/Core/wpf/", "Rikrop.Core.Wpf.Behaviors")]
[assembly: XmlnsDefinition("http://Rikrop/Core/wpf/", "Rikrop.Core.Wpf.Controls")]
[assembly: XmlnsDefinition("http://Rikrop/Core/wpf/", "Rikrop.Core.Wpf.Controls.Watermark")]
[assembly: XmlnsDefinition("http://Rikrop/Core/wpf/", "Rikrop.Core.Wpf.Controls.Validation.VGrid")]
[assembly: XmlnsDefinition("http://Rikrop/Core/wpf/", "Rikrop.Core.Wpf.Controls.Validation.VBorder")]
[assembly: XmlnsDefinition("http://Rikrop/Core/wpf/", "Rikrop.Core.Wpf.Controls.TabControl")]
[assembly: XmlnsDefinition("http://Rikrop/Core/wpf/", "Rikrop.Core.Wpf.Controls.ErrorReport")]
[assembly: XmlnsDefinition("http://Rikrop/Core/wpf/", "Rikrop.Core.Wpf.Controls.ApplyCancelButtons")]
[assembly: XmlnsDefinition("http://Rikrop/Core/wpf/", "Rikrop.Core.Wpf.Controls.RrcPopupDialog")]
[assembly: XmlnsDefinition("http://Rikrop/Core/wpf/", "Rikrop.Core.Wpf.Converters")]
[assembly: XmlnsDefinition("http://Rikrop/Core/wpf/", "Rikrop.Core.Wpf.StyleSelectors")]
[assembly: XmlnsDefinition("http://Rikrop/Core/wpf/", "Rikrop.Core.Wpf.Themes")]
[assembly: XmlnsDefinition("http://Rikrop/Core/wpf/", "Rikrop.Core.Wpf.Collections")]