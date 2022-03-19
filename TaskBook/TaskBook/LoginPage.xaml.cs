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

            StackLayout password_layout = new StackLayout() { Orientation = StackOrientation.Horizontal };

            Entry password_entry = new Entry()
            {
                Placeholder = "Пароль",
                TextColor = Color.Black,
                Margin = new Thickness(20, 0, 0, 0),
                FontSize = 20,
                IsPassword = true,
                WidthRequest = 250
            };

            Image visibility_status_icon = new Image()
            {
                Source = "vis.png",
                ScaleX = 0.8,
                ScaleY = 0.8,
                Margin = 0
            };

            password_layout.Children.Add(password_entry);
            password_layout.Children.Add(visibility_status_icon);

            Frame password_frame = new Frame()
            {
                BorderColor = Color.Black,
                CornerRadius = 50,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(30, 50, 30, 0),
                Padding = new Thickness(0, 5, 0, 5),
                Content = password_layout
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

            login_button.Clicked += Login;

            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += ChangeVisibilityStatus;
            visibility_status_icon.GestureRecognizers.Add(tapGestureRecognizer);

            layout.Children.Add(info_label);
            layout.Children.Add(login_frame);
            layout.Children.Add(password_frame);
            layout.Children.Add(login_button);

            Content = layout;

            async void Login(object sender, EventArgs e)
            {
                string login = login_entry.Text;
                string password = password_entry.Text;

                string login_status = DB.Login(login, password);

                if (login_status == "ok")
                {
                    Preferences.Set("login", login);

                    if (Preferences.Get("room", null) != null && Preferences.Get("role", null) == "user")
                    {
                        Task.ClearTaks();
                        DB.GetTasks();
                    }

                    await DisplayAlert("Успех!", "Вы успешно вошли в аккаунт", "OK");
                    await Navigation.PopAsync();
                }
                else if (login_status == "no")
                    await DisplayAlert("Ошибка!", "Неверный логин или пароль", "OK");
                else
                    await DisplayAlert("Ошибка!", "Ошибка подключения к базе данных, проверьте интернет соединение", "OK");
            }

            void ChangeVisibilityStatus(object sender, EventArgs e)
            {
                string visibility_status = visibility_status_icon.Source.ToString();

                if (visibility_status == "File: vis.png")
                {
                    visibility_status_icon.Source = "invis.png";
                    password_entry.IsPassword = false;
                }
                else
                {
                    visibility_status_icon.Source = "vis.png";
                    password_entry.IsPassword = true;
                }

            }
        }

        
    }
}