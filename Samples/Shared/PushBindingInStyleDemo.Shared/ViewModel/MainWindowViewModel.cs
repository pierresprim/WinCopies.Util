﻿/*
 Original source code: Fredrik Hedblad (https://meleak.wordpress.com/2011/08/28/onewaytosource-binding-for-readonly-dependency-property/)

This is free and unencumbered software released into the public domain.

Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a compiled
binary, for any purpose, commercial or non-commercial, and by any
means.

In jurisdictions that recognize copyright laws, the author or authors
of this software dedicate any and all copyright interest in the
software to the public domain. We make this dedication for the benefit
of the public at large and to the detriment of our heirs and
successors. We intend this dedication to be an overt act of
relinquishment in perpetuity of all present and future rights to this
software under copyright law.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.

For more information, please refer to <http://unlicense.org> */

using System.Collections.ObjectModel;

namespace PushBindingInStyleDemo.ViewModel
{
    public abstract class ViewModelBase : WinCopies.Util.Data.ViewModelBase
    {
        private string m_displayName;

        public string DisplayName { get => m_displayName; set => UpdateValue(ref m_displayName, value, nameof(DisplayName)); }
    }

    public class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<ViewModelBase> m_pushBindingExamples;

        public ObservableCollection<ViewModelBase> PushBindingExamples { get => m_pushBindingExamples; set => UpdateValue(ref m_pushBindingExamples, value, nameof(PushBindingExamples)); }

        public MainWindowViewModel() => PushBindingExamples = new ObservableCollection<ViewModelBase> { new PushBindingsViewModel(), new PushBindingsInStyleViewModel() };
    }
}
