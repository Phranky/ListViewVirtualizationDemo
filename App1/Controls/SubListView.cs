using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace ListViewVirtualizationDemo.Controls
{
    public sealed class SubListView : ListView
    {
        public SubListView()
        {
            DefaultStyleKey = typeof(ListView);

            this.GettingFocus += SubListView_GettingFocus; ;
        }

        private void SubListView_GettingFocus(Windows.UI.Xaml.UIElement sender, Windows.UI.Xaml.Input.GettingFocusEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("GotFocus");
        }
    }
}
