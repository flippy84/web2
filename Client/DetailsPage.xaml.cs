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

namespace Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailsPage : Page
    {
        User CurrentUser;
        int TaskID;

        public DetailsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            CurrentUser = (Application.Current as App).CurrentUser;
            TaskID = (int)e.Parameter;

            var api = new ClientAPI(Settings.Host);
            var task = api.GetTask(TaskID);

            Title.Text = task.Title;
            Requirements.Text = task.Requirements;
            Begin.Text = task.BeginDateTime.ToString();
            Deadline.Text = task.DeadlineDateTime.ToString();

            Assignments.Text = "";
            foreach (var assignment in task.assignments)
            {
                var user = api.GetUser(assignment.UserID);
                Assignments.Text += user + Environment.NewLine;
            }

            base.OnNavigatedTo(e);
        }

        private void UpdateAssignments()
        {
            var api = new ClientAPI(Settings.Host);
            var assignments = api.GetAssignments(TaskID);

            Assignments.Text = "";
            foreach (var assignment in assignments)
            {
                var user = api.GetUser(assignment.UserID);
                Assignments.Text += user + Environment.NewLine;
            }
        }

        private void Claim_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var api = new ClientAPI(Settings.Host);
            api.CreateAssignment(TaskID, CurrentUser.UserID);
            UpdateAssignments();
        }

        private void Disclaim_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var api = new ClientAPI(Settings.Host);
            api.DeleteAssignment(TaskID, CurrentUser.UserID);
            UpdateAssignments();
        }
    }
}
