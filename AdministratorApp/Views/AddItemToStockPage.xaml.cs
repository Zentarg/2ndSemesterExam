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
    /// Page for adding item to a specific stock.
    /// </summary>
    public sealed partial class AddItemToStockPage : Page
    {
        public AddItemToStockPage()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// Method for preventing non numeric input;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void FilterNonNumeric_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }
    }
}
