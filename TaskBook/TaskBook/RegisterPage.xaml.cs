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
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            StackLayout layout = new StackLayout();

            Label info_label = new Label()
            {
                Text = "РЕГИСТРАЦИЯ",
                TextColor = Color.Black,
                FontSize = 35,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 80, 0, 0)
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
                Margin = new Thickness(30, 50, 30, 0),
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

            Image visibility_status_icon1 = new Image()
            {
                Source = "vis.png",
                ScaleX = 0.8,
                ScaleY = 0.8,
                Margin = 0
            };

            password_layout.Children.Add(password_entry);
            password_layout.Children.Add(visibility_status_icon1);

            Frame password_frame = new Frame()
            {
                BorderColor = Color.Black,
                CornerRadius = 50,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(30, 50, 30, 0),
                Padding = new Thickness(0, 5, 0, 5),
                Content = password_layout
            };

            StackLayout password_rep_layout = new StackLayout() { Orientation = StackOrientation.Horizontal };

            Entry password_rep_entry = new Entry()
            {
                Placeholder = "Повтор пароля",
                TextColor = Color.Black,
                Margin = new Thickness(20, 0, 0, 0),
                FontSize = 20,
                IsPassword = true,
                WidthRequest = 250
            };

            Image visibility_status_icon2 = new Image()
            {
                Source = "vis.png",
                ScaleX = 0.8,
                ScaleY = 0.8,
                Margin = 0
            };

            password_rep_layout.Children.Add(password_rep_entry);
            password_rep_layout.Children.Add(visibility_status_icon2);

            Frame password_rep_frame = new Frame()
            {
                BorderColor = Color.Black,
                CornerRadius = 50,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(30, 50, 30, 0),
                Padding = new Thickness(0, 5, 0, 5),
                Content = password_rep_layout
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


            TapGestureRecognizer tapGestureRecognizer1 = new TapGestureRecognizer();
            tapGestureRecognizer1.Tapped += ChangeVisibilityStatus1;
            visibility_status_icon1.GestureRecognizers.Add(tapGestureRecognizer1);

            TapGestureRecognizer tapGestureRecognizer2 = new TapGestureRecognizer();
            tapGestureRecognizer2.Tapped += ChangeVisibilityStatus2;
            visibility_status_icon2.GestureRecognizers.Add(tapGestureRecognizer2);


            layout.Children.Add(info_label);
            layout.Children.Add(login_frame);
            layout.Children.Add(password_frame);
            layout.Children.Add(password_rep_frame);
            layout.Children.Add(login_button);

            Content = layout;

            void ChangeVisibilityStatus1(object sender, EventArgs e)
            {
                string visibility_status = visibility_status_icon1.Source.ToString();

                if (visibility_status == "File: vis.png")
                {
                    visibility_status_icon1.Source = "invis.png";
                    password_entry.IsPassword = false;
                }
                else
                {
                    visibility_status_icon1.Source = "vis.png";
                    password_entry.IsPassword = true;
                }
            }

            void ChangeVisibilityStatus2(object sender, EventArgs e)
            {
                string visibility_status = visibility_status_icon2.Source.ToString();

                if (visibility_status == "File: vis.png")
                {
                    visibility_status_icon2.Source = "invis.png";
                    password_rep_entry.IsPassword = false;
                }
                else
                {
                    visibility_status_icon2.Source = "vis.png";
                    password_rep_entry.IsPassword = true;
                }
            }
        }

        
    }
}