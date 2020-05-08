using System;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;

namespace AdministratorApp.Models
{
    public class NavigationHandler
    {

        public static Frame Frame { get; set; }
        public static string CurrentPageName { get; set; }

        public static void NavigateToPage(Type page)
        {
            Frame.Navigate(page);
            CurrentPageName = page.Name;
            OnNavigationOccured();
        }

        public static void NavigateBackwards()
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
            CurrentPageName = Frame.CurrentSourcePageType.Name;
            OnNavigationOccured();
        }

        public static void NavigateForwards()
        {
            if (Frame.CanGoForward)
                Frame.GoForward();
            CurrentPageName = Frame.CurrentSourcePageType.Name;
            OnNavigationOccured();
        }

        public static event EventHandler NavigationOccured = delegate{};

        public static void OnNavigationOccured()
        {
            NavigationOccured?.Invoke(null, EventArgs.Empty);
        }

    }
}
