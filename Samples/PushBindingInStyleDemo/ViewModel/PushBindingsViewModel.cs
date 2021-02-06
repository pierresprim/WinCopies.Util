/*
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
    public class PushBindingsViewModel : ViewModelBase
    {
        public PushBindingsViewModel()
        {
            DisplayName = "PushBinding Examples";

            MyItems = new ObservableCollection<ListItemViewModel>();

            for (int i = 1; i < 6; i++)

                MyItems.Add(new ListItemViewModel { Name = $"ListItem {i}" });
        }

        #region Properties

        private double m_height;

        public double Height
        {
            get => m_height;

            set
            {
                m_height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        private double m_width;

        public double Width
        {
            get => m_width;

            set
            {
                m_width = value;
                OnPropertyChanged(nameof(Width));
            }
        }

        private ObservableCollection<ListItemViewModel> m_myItems;

        public ObservableCollection<ListItemViewModel> MyItems
        {
            get => m_myItems;

            set
            {
                m_myItems = value;
                OnPropertyChanged(nameof(MyItems));
            }
        }

        #endregion // Properties
    }
}
