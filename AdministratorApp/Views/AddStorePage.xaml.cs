using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AdministratorApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddStorePage : Page
    {
        public AddStorePage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Disables confirm button if input text fields are empty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmIsVisible_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Name.Text) && !string.IsNullOrEmpty(Address.Text) && !string.IsNullOrEmpty(Phone.Text) && Manager.SelectedItem != null)
                ConfirmButton.IsEnabled = true;
            else ConfirmButton.IsEnabled = false;
        }

        /// <summary>
        /// Disables confirm button if manager combo box is empty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Manager_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Name.Text) && !string.IsNullOrEmpty(Address.Text) && !string.IsNullOrEmpty(Phone.Text) && Manager.SelectedItem != null)
                ConfirmButton.IsEnabled = true;
            else ConfirmButton.IsEnabled = false;
        }

    }
}
