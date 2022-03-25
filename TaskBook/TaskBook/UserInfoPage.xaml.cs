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
    public partial class UserInfoPage : ContentPage
    {
        public Task[] tasks = null;
        public StackLayout[] lay_arr = null;

        public UserInfoPage()
        {
            InitializeComponent();
        }

        private async void ShowInfo(object sender, EventArgs e)
        {
            for (int i = 0; i < lay_arr.Length; i++)
            {
                if (lay_arr[i] == (sender as StackLayout))
                {
                    Task.current_task = tasks[i];
                    await Navigation.PushAsync(new TaskInfoPage());
                }
            }
        }

        private async void IncreaseUser(object sender, EventArgs e)
        {
            if (await DisplayAlert("Вы уверены?", "Вы действительно хотите повысить этого пользователя? Действие невозможно отменить", "Да", "Нет"))
            {
                if (DB.IncreaseUser(Preferences.Get("current_user", null)) == "ok")
                {
                    await DisplayAlert("Успех!", "Пользователь повышен", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Ошибка!", "Ошибка подключения к базе данных", "OK");
                }
            }
        }


        protected override void OnAppearing()
        {
            tasks = DB.GetTasks(Preferences.Get("current_user", null));

            StackLayout layout = new StackLayout() { Padding = 10 };

            Label username_label = new Label()
            {
                Text = Preferences.Get("current_user", null),
                FontSize = 26,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };

            Button increase_button = new Button()
            {
                Text = "Повысить до админа",
                FontSize = 24,
                TextColor = Color.White,
                BackgroundColor = Color.FromHex("#000080"),
                CornerRadius = 20,
                Margin = new Thickness(0, 10, 0, 10)
            };

            increase_button.Clicked += IncreaseUser;

            layout.Children.Add(username_label);
            layout.Children.Add(increase_button);

            if (tasks.Length > 0)
            {
                StackLayout[] new_lay_arr = new StackLayout[tasks.Length];
                lay_arr = new_lay_arr;

                ScrollView scrollView = new ScrollView();

                for (int i = 0; i < tasks.Length; i++)
                {
                    StackLayout vertLayout = new StackLayout();

                    Frame task_frame = new Frame()
                    {
                        BorderColor = Color.Black,
                        CornerRadius = 10,
                        Padding = 10,
                        HasShadow = false,
                        Content = vertLayout
                    };

                    Label header_label = new Label()
                    {
                        Text = tasks[i].header,
                        TextColor = Color.Black,
                        FontSize = 30,
                        FontAttributes = FontAttributes.Bold
                    };

                    Label priority_label = new Label()
                    {
                        Text = "Приоритет: " + tasks[i].priority.ToString(),
                        TextColor = Color.Black,
                        FontSize = 18
                    };

                    string[] d = tasks[i].deadline.Split(' ');
                    DateTime d1 = new DateTime(2000 + int.Parse(d[2]), int.Parse(d[1]), int.Parse(d[0]), int.Parse(d[3]), int.Parse(d[4]), 0);

                    Label deadline_label = new Label()
                    {
                        Text = "Дедлайн: " + d[0] + "." + d[1] + "." + d[2] + " " + d[3] + ":" + d[4],
                        TextColor = Color.Black,
                        FontSize = 18
                    };

                    Label status_label = new Label()
                    {
                        TextColor = Color.Black,
                        FontSize = 18
                    };

                    status_label.Text = DateTime.Now > d1 ? "Статус: Не выполнено вовремя" : (tasks[i].completed_status ? "Статус: Выполнено" : "Статус: В процессе выполнения");
                    status_label.TextColor = DateTime.Now > d1 ? Color.FromHex("#8B0000") : (tasks[i].completed_status ? Color.FromHex("#006400") : Color.FromHex("#4682B4"));


                    vertLayout.Children.Add(header_label);
                    vertLayout.Children.Add(priority_label);
                    vertLayout.Children.Add(deadline_label);
                    vertLayout.Children.Add(status_label);

                    layout.Children.Add(task_frame);

                    TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += ShowInfo;
                    vertLayout.GestureRecognizers.Add(tapGestureRecognizer);

                    lay_arr[i] = vertLayout;
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