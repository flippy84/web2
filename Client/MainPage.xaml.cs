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
using Windows.UI.Popups;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Client
{
    public sealed partial class MainPage : Page
    {
        ClientAPI api = new ClientAPI("172.16.80.2");

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            /*var hej = VisualTreeHelper.GetChildrenCount((DependencyObject)FindName("MainContent"));

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount((DependencyObject)FindName("MainContent")); i++)
            {
                var child = VisualTreeHelper.GetChild((DependencyObject)FindName("MainContent"), i);
            }*/

            int x = 110;
            var tasks = api.GetTasks();
            for (int i = 0; i < tasks.Count; i++)
            {
                var task = tasks[i];
                var button = new Button
                {
                    Content = task.Title,
                    Margin = new Thickness(10, x * i, 0, 0),
                    Height = 50,
                    Width = 200,
                    Tag = task.TaskID,
                };

                button.Tapped += Button_Tapped;
                button.Holding += Button_Holding;

                MainContent.Children.Add(button);
            }
        }

        private void Button_Holding(object sender, HoldingRoutedEventArgs e)
        {
            var button = (Button)sender;
            var taskID = (int)button.Tag;
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var button = (Button)sender;
            var taskID = (int)button.Tag;
            Frame.Navigate(typeof(DetailsPage), taskID);
        }
    }
}
