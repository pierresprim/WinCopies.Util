//* Copyright © Pierre Sprimont, 2019
// *
// * This file is part of the WinCopies Framework.
// *
// * The WinCopies Framework is free software: you can redistribute it and/or modify
// * it under the terms of the GNU General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * The WinCopies Framework is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

////<!--<ResourceDictionary x:Class="WinCopies.Util.Generic"
////             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
////             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
////             xmlns:local="clr-namespace:WinCopies.Util"
////                    xmlns:sys="clr-namespace:System;assembly=mscorlib">
////    --><!--<sys:String x:Key="DeclaringTypeIsNotInObjectInheritanceHierarchyException" >'{0}' is not in the inheritance hierarchy of '{1}'.</sys:String>--><!--
////</ResourceDictionary>-->

//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//CS7

//namespace WinCopies.Util
//{
//    [Obsolete("This class is being replaced by the Resources static class and will be removed in later versions.")]
//    public static class Generic
//    {

//        public static Dictionary<string, System.Windows.ResourceDictionary> ResourceDictionaries { get; } = new Dictionary<string, System.Windows.ResourceDictionary>();

//        public static T GetResource<T>(object key) => Resources.GetResource<T>((string)key);

//        #region Resources

//        /// <summary>
//        /// Gets the NewTab resource.
//        /// </summary>
//        public static string NewTab => GetResource<string>(nameof(NewTab) + "WPF");

//        /// <summary>
//        /// Gets the NewWindow resource.
//        /// </summary>
//        public static string NewWindow => GetResource<string>(nameof(NewWindow) + "WPF");

//        /// <summary>
//        /// Gets the NewWindowInNewInstance resource.
//        /// </summary>
//        public static string NewWindowInNewInstance => GetResource<string>(nameof(NewWindowInNewInstance) + "WPF");

//        /// <summary>
//        /// Gets the CloseTab resource.
//        /// </summary>
//        public static string CloseTab => GetResource<string>(nameof(CloseTab) + "WPF");

//        /// <summary>
//        /// Gets the CloseAllTabs resource.
//        /// </summary>
//        public static string CloseAllTabs => GetResource<string>(nameof(CloseAllTabs) + "WPF");

//        /// <summary>
//        /// Gets the CloseWindow resource.
//        /// </summary>
//        public static string CloseWindow => GetResource<string>(nameof(CloseWindow) + "WPF");

//        /// <summary>
//        /// Gets the NewFolder resource.
//        /// </summary>
//        public static string NewFolder => GetResource<string>(nameof(NewFolder) + "WPF");

//        /// <summary>
//        /// Gets the NewArchive resource.
//        /// </summary>
//        public static string NewArchive => GetResource<string>(nameof(NewArchive) + "WPF");

//        /// <summary>
//        /// Gets the ShowFileProperties resource.
//        /// </summary>
//        public static string ShowFileProperties => GetResource<string>(nameof(ShowFileProperties) + "WPF");

//        /// <summary>
//        /// Gets the Rename resource.
//        /// </summary>
//        public static string Rename => GetResource<string>(nameof(Rename) + "WPF");

//        /// <summary>
//        /// Gets the DeletePermanently resource.
//        /// </summary>
//        public static string DeletePermanently => GetResource<string>(nameof(DeletePermanently) + "WPF");

//        /// <summary>
//        /// Gets the <see cref="DeselectAll"/> resource.
//        /// </summary>
//        public static string DeselectAll => GetResource<string>(nameof(DeselectAll) + "WPF");

//        /// <summary>
//        /// Gets the <see cref="ReverseSelection"/> resource.
//        /// </summary>
//        public static string ReverseSelection => GetResource<string>(nameof(ReverseSelection) + "WPF");

//        /// <summary>
//        /// Gets the DeclaringTypesNotCorrespond resource.
//        /// </summary>
//        public static string DeclaringTypesNotCorrespond => GetResource<string>(nameof(DeclaringTypesNotCorrespond));

//        /// <summary>
//        /// Gets the FieldOrPropertyNotFound resource.
//        /// </summary>
//        public static string FieldOrPropertyNotFound => GetResource<string>(nameof(FieldOrPropertyNotFound));

//        /// <summary>
//        /// Gets the ArrayWithMoreThanOneDimension resource.
//        /// </summary>
//        public static string ArrayWithMoreThanOneDimension => GetResource<string>(nameof(ArrayWithMoreThanOneDimension));

//        /// <summary>
//        /// Gets the OneOrMoreSameKey resource.
//        /// </summary>
//        public static string OneOrMoreSameKey => GetResource<string>(nameof(OneOrMoreSameKey));

//        /// <summary>
//        /// Gets the NoValidEnumValue resource.
//        /// </summary>
//        public static string NoValidEnumValue => GetResource<string>(nameof(NoValidEnumValue));

//        /// <summary>
//        /// Gets the StringParameterEmptyOrWhiteSpaces resource.
//        /// </summary>
//        public static string StringParameterEmptyOrWhiteSpaces => GetResource<string>(nameof(StringParameterEmptyOrWhiteSpaces));

//        /// <summary>
//        /// Gets the BackgroundWorkerIsBusy resource.
//        /// </summary>
//        public static string BackgroundWorkerIsBusy => GetResource<string>(nameof(BackgroundWorkerIsBusy));

//        /// <summary>
//        /// Gets the InvalidEnumValue resource.
//        /// </summary>
//        public static string InvalidEnumValue => GetResource<string>(nameof(InvalidEnumValue));

//        /// <summary>
//        /// Gets the ReadOnlyCollection resource.
//        /// </summary>
//        public static string ReadOnlyCollection => GetResource<string>(nameof(ReadOnlyCollection));

//        #endregion

//        public static System.Windows.ResourceDictionary ResourceDictionary { get; } = null;

//        // static Generic() => ResourceDictionary = AddNewDictionary("/WinCopies.Util;component/Generic.xaml");

//        // public Generic() => ResourceDictionary = AddNewDictionary("/WinCopies.Util;component/Generic.xaml");

//        public static System.Windows.ResourceDictionary AddNewDictionary(string dictionaryUri, string key = null)

//        {

//            if (key == null)

//                key = Assembly.GetCallingAssembly().GetName().Name;

//            var resourceDictionary = new System.Windows.ResourceDictionary
//            {
//                Source = new Uri(dictionaryUri, UriKind.RelativeOrAbsolute)
//            };

//            ResourceDictionaries.Add(key, resourceDictionary);

//            return resourceDictionary;

//        }

//        public static System.Windows.ResourceDictionary GetResourceDictionary(string key = null)

//        {

//            if (key == null)

//                key = Assembly.GetCallingAssembly().GetName().Name;

//            return ResourceDictionaries[key];

//        }

//        public static object GetResource(object resourceKey, string resourceDictionaryKey = null) => GetResourceDictionary(resourceDictionaryKey)[resourceKey];
//    }
//}

//#endif
