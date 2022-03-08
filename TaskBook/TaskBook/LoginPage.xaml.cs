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
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        
        protected override void OnAppearing()
        {
            StackLayout layout = new StackLayout();

            Label info_label = new Label()
            {
                Text = "ВХОД",
                TextColor = Color.Black,
                FontSize = 35,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 100, 0, 0)
            };

            Entry login_entry = new Entry()
            {
                Placeholder = "Логин",
                TextColor = Color.Black,
                Margin = new Thickness(20, 0, 20, 0),
                FontSize = 20
            };

            Frame login_frame = new Frame()
            {
                BorderColor = Color.Black,
                CornerRadius = 50,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(30, 70, 30, 0),
                Padding = new Thickness(0, 5, 0, 5),
                Content = login_entry
            };

            Entry password_entry = new Entry()
            {
                Placeholder = "Пароль",
                TextColor = Color.Black,
                Margin = new Thickness(20, 0, 20, 0),
                FontSize = 20,
                IsPassword = true
            };

            Frame password_frame = new Frame()
            {
                BorderColor = Color.Black,
                CornerRadius = 50,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(30, 50, 30, 0),
                Padding = new Thickness(0, 5, 0, 5),
                Content = password_entry
            };

            Button login_button = new Button()
            {
                Text = "Войти",
                FontSize = 25,
                TextColor = Color.White,
                BackgroundColor = Color.FromHex("#000080"),
                CornerRadius = 20,
                Margin = new Thickness(30, 70, 30, 0)
            };

            
            layout.Children.Add(info_label);
            layout.Children.Add(login_frame);
            layout.Children.Add(password_frame);
            layout.Children.Add(login_button);

            Content = layout;
        }
    }
}