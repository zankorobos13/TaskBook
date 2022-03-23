using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskBook
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsersListPage : ContentPage
    {
        public StackLayout[] lay_arr = null;
        public string[] users = null;

        public UsersListPage()
        {
            InitializeComponent();
        }


        private async void ShowInfo(object sender, EventArgs e)
        {
            for (int i = 0; i < lay_arr.Length; i++)
            {
                if (lay_arr[i] == (sender as StackLayout))
                {
                    Preferences.Set("current_user", users[i]);
                    await Navigation.PushAsync(new UserInfoPage());
                }
            }
        }

        protected override void OnAppearing()
        {
            users = DB.GetUsers();

            StackLayout layout = new StackLayout() { Padding = 10 };

            StackLayout[] new_lay_arr = new StackLayout[users.Length];
            lay_arr = new_lay_arr;

            if (users.Length > 0)
            {
                ScrollView scrollView = new ScrollView();
                
                for (int i = 0; i < users.Length; i++)
                {
                    StackLayout lay = new StackLayout() 
                    {
                        Padding = 0,
                        Margin = 0,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand 
                    };

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
                        Content = lay
                    };

                    lay.Children.Add(username_label);
                    layout.Children.Add(frame);

                    TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += ShowInfo;
                    lay.GestureRecognizers.Add(tapGestureRecognizer);

                    lay_arr[i] = lay;
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