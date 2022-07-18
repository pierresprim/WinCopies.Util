using System.Windows;

namespace AttachedCommandBehaviorDemo
{
    public partial class DemoView : Window
    {
        public DemoView()
        {
            DataContext = new DemoViewModel();

            InitializeComponent();
        }
    }
}
