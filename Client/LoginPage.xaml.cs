﻿using System;
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
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();

            var users = new ClientAPI(Settings.Host).GetUsers();
            foreach(var user in users)
            {
                UserComboBox.Items.Add(user);
            }
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var user = (User)UserComboBox.SelectedItem;
            (Application.Current as App).CurrentUser = user;
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
