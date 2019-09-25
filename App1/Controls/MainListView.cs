using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace ListViewVirtualizationDemo.Controls
{
    public sealed class MainListView : Control
    {
        public MainListView()
        {
            this.DefaultStyleKey = typeof(ListView);

            this.GettingFocus += MainListView_GettingFocus;
        }

        private void MainListView_GettingFocus(Windows.UI.Xaml.UIElement sender, Windows.UI.Xaml.Input.GettingFocusEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("MainListView GettingFocus");
        }
    }
}
