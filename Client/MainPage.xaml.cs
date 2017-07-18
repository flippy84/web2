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
using Windows.UI;

namespace Client
{
    public sealed partial class MainPage : Page
    {
        ClientAPI api = new ClientAPI("172.16.80.2");
        User CurrentUser;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            CurrentUser = (Application.Current as App).CurrentUser;
            base.OnNavigatedTo(e);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            int y = 110;
            var tasks = api.GetTasks();
            tasks.Sort((a, b) => (int)(a.BeginDateTime - b.BeginDateTime).TotalMilliseconds);
            DateTime start = tasks[0].BeginDateTime;
            DateTime end = tasks.Last().DeadlineDateTime;

            for (int i = 0; i < tasks.Count; i++)
            {
                var task = tasks[i];
                var button = new Button
                {
                    Content = task.Title,
                    Margin = new Thickness((task.BeginDateTime - start).TotalHours * 10 + 10, y * i, 0, 0),
                    Height = 50,
                    Width = (task.DeadlineDateTime - task.BeginDateTime).TotalHours * 10,
                    Tag = task.TaskID,
                    Name = task.TaskID.ToString()
                };

                if (task.assignments.Exists(x => x.UserID == CurrentUser.UserID) && task.assignments.Count == 1)
                {
                    button.Background = new SolidColorBrush(Colors.Green);
                }
                else if (task.assignments.Exists(x => x.UserID == CurrentUser.UserID) && task.assignments.Count > 1)
                {
                    button.Background = new SolidColorBrush(Colors.Red);
                }
                else if (task.assignments.Count > 0)
                {
                    button.Background = new SolidColorBrush(Colors.Blue);
                }

                button.Tapped += Button_Tapped;
                button.Holding += Button_Holding;

                MainContent.Children.Add(button);
                MainContent.Width = (end - start).TotalHours * 10 + 20;
            }
        }

        private void Button_Holding(object sender, HoldingRoutedEventArgs e)
        {
            var button = (Button)sender;
            var taskID = (int)button.Tag;

            var menyflyout = new MenuFlyout();

            var claim = new MenuFlyoutItem
            {
                Text = "Claim"
            };
            claim.Tapped += Claim_Tapped;
            claim.Tag = button.Tag;

            var disclaim = new MenuFlyoutItem
            {
                Text = "Disclaim"
            };
            disclaim.Tapped += Disclaim_Tapped;
            disclaim.Tag = button.Tag;

            menyflyout.Items.Add(claim);
            menyflyout.Items.Add(disclaim);
            menyflyout.ShowAt(button);
        }

        private void Claim_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var api = new ClientAPI("172.16.80.2");
            var taskID = (int)((MenuFlyoutItem)sender).Tag;
            api.CreateAssignment(taskID, CurrentUser.UserID);

            Button button = (Button)FindName(taskID.ToString());
            switch (api.GetTask(taskID).assignments.Count)
            {
                case 1:
                    button.Background = new SolidColorBrush(Colors.Green);
                    break;
                default:
                    button.Background = new SolidColorBrush(Colors.Red);
                    break;
            }
        }

        private void Disclaim_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var api = new ClientAPI("172.16.80.2");
            var taskID = (int)((MenuFlyoutItem)sender).Tag;
            api.DeleteAssignment(taskID, CurrentUser.UserID);

            Button button = (Button)FindName(taskID.ToString());
            switch (api.GetTask(taskID).assignments.Count)
            {
                case 0:
                    button.Background = new SolidColorBrush(Colors.LightGray);
                    break;
                default:
                    button.Background = new SolidColorBrush(Colors.Blue);
                    break;
            }
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var button = (Button)sender;
            var taskID = (int)button.Tag;
            Frame.Navigate(typeof(DetailsPage), taskID);
        }
    }
}
