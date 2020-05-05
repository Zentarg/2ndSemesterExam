using System;
using Windows.UI.Xaml.Controls;

namespace AdministratorApp.Models
{
    public static class NavigationHandler
    {

        public static Frame Frame { get; set; }
        public static string CurrentPageName { get; set; }

        public static void NavigateToPage(Type page)
        {
            Frame.Navigate(page);
            CurrentPageName = page.Name;
        }

        public static void NavigateBackwards()
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
            CurrentPageName = Frame.CurrentSourcePageType.Name;
        }

        public static void NavigateForwards()
        {
            if (Frame.CanGoForward)
                Frame.GoForward();
            CurrentPageName = Frame.CurrentSourcePageType.Name;
        }

    }
}
