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
    public partial class AccountPage : ContentPage
    {
        public AccountPage()
        {
            InitializeComponent();
        }


        private async void Login(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }

        private async void Register(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }

        private async void CreateRoom(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateRoomPage());
        }

        private void LeaveRoom(object sender, EventArgs e)
        {
            Preferences.Set("room", null);
            OnAppearing();
        }

        private void Exit(object sender, EventArgs e)
        {
            Preferences.Set("login", null);
            OnAppearing();
        }


        protected override void OnAppearing()
        {
            StackLayout layout = new StackLayout();


            if (Preferences.Get("login", null) == null)
            {
                Label info_label = new Label()
                {
                    Text = "Для работы в приложении войдите в аккаунт или создайте новый",
                    TextColor = Color.Black,
                    FontSize = 25,
                    Margin = new Thickness(20, 20, 20, 0),
                    HorizontalTextAlignment = TextAlignment.Center,

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

                Button register_button = new Button()
                {
                    Text = "Зарегистрироваться",
                    FontSize = 25,
                    TextColor = Color.White,
                    BackgroundColor = Color.FromHex("#000080"),
                    CornerRadius = 20,
                    Margin = new Thickness(30, 50, 30, 0)
                };

                login_button.Clicked += Login;
                register_button.Clicked += Register;

                layout.Children.Add(info_label);
                layout.Children.Add(login_button);
                layout.Children.Add(register_button);

            }
            else
            {
                Label login_label = new Label()
                {
                    Text = Preferences.Get("login", null),
                    TextColor = Color.Black,
                    FontAttributes = FontAttributes.Italic,
                    Padding = new Thickness(30, 10, 0, 0)
                };

                login_label.FontSize = 50 - login_label.Text.Length;

                layout.Children.Add(login_label);

                if (Preferences.Get("room", null) == null)
                {
                    Button enter_room_button = new Button()
                    {
                        Text = "Войти в комнату",
                        FontSize = 25,
                        TextColor = Color.White,
                        BackgroundColor = Color.FromHex("#000080"),
                        CornerRadius = 20,
                        Margin = new Thickness(30, 70, 30, 0)
                    };

                    Button create_room_button = new Button()
                    {
                        Text = "Создать комнату",
                        FontSize = 25,
                        TextColor = Color.White,
                        BackgroundColor = Color.FromHex("#000080"),
                        CornerRadius = 20,
                        Margin = new Thickness(30, 70, 30, 0)
                    };

                    create_room_button.Clicked += CreateRoom;

                    layout.Children.Add(enter_room_button);
                    layout.Children.Add(create_room_button);
                }
                else
                {
                    Label room_label = new Label()
                    {
                        Text = "Комната: " + Preferences.Get("room", null),
                        FontAttributes = FontAttributes.Italic,
                        TextColor = Color.Black,
                        Padding = new Thickness(30, 10, 0, 0)
                    };

                    room_label.FontSize = 40 - room_label.Text.Length;

                    Label role_label = new Label()
                    {
                        Text = "Роль: " + Preferences.Get("role", null),
                        FontAttributes = FontAttributes.Italic,
                        TextColor = Color.Black,
                        Padding = new Thickness(30, 10, 0, 0)
                    };

                    role_label.FontSize = 40 - role_label.Text.Length;

                    Button leave_room_button = new Button()
                    {
                        Text = "Покинуть комнату",
                        FontSize = 25,
                        TextColor = Color.White,
                        BackgroundColor = Color.FromHex("#000080"),
                        CornerRadius = 20,
                        Margin = new Thickness(30, 70, 30, 0)
                    };

                    leave_room_button.Clicked += LeaveRoom;

                    layout.Children.Add(room_label);
                    layout.Children.Add(role_label);
                    layout.Children.Add(leave_room_button);

                }
                

                Button exit_button = new Button()
                {
                    Text = "Выйти",
                    FontSize = 25,
                    TextColor = Color.White,
                    BackgroundColor = Color.FromHex("#000080"),
                    CornerRadius = 20,
                    Margin = new Thickness(30, 70, 30, 0)
                };

                exit_button.Clicked += Exit;

                layout.Children.Add(exit_button);
            }

            Content = layout;
        }
    }

}