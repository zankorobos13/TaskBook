using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskBook
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsersListPage : ContentPage
    {
        public UsersListPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            string[] users = DB.GetUsers();

            StackLayout layout = new StackLayout() { Padding = 10 };

            if (users.Length > 0)
            {
                ScrollView scrollView = new ScrollView();

                for (int i = 0; i < users.Length; i++)
                {
                    Label username_label = new Label()
                    {
                        Text = users[i],
                        FontSize = 26,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Color.Black
                    };

                    Frame frame = new Frame()
                    {
                        BorderColor = Color.Black,
                        CornerRadius = 5,
                        Content = username_label
                    };

                    layout.Children.Add(frame);
                }
                scrollView.Content = layout;
                Content = scrollView;
            }
            else
            {
                Content = layout;
            }
            


            
        }
    }
}