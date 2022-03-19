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
    public partial class HomePage : ContentPage
    {
        StackLayout[] lay_arr = null;

        public HomePage()
        {
            InitializeComponent();
        }

        private async void CreateTask(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateTaskPage());
        }

        async void ShowInfo(object sender, EventArgs e)
        {
            for (int i = 0; i < lay_arr.Length; i++)
            {
                if (lay_arr[i] == (sender as StackLayout))
                {
                    await DisplayAlert(Task.tasks[i].header, Task.tasks[i].text, "OK");
                }
            }
        }



        protected override void OnAppearing()
        {
            if (Preferences.Get("login", null) != null && Preferences.Get("room", null) != null)
            {
                StackLayout layout = new StackLayout();

                if (Preferences.Get("role", null) == "admin")
                {
                    Button create_task_button = new Button()
                    {
                        Text = "Создать задание",
                        FontSize = 25,
                        TextColor = Color.White,
                        BackgroundColor = Color.FromHex("#000080"),
                        CornerRadius = 20,
                        Margin = new Thickness(30, 100, 30, 0)
                    };

                    create_task_button.Clicked += CreateTask;

                    layout.Children.Add(create_task_button);
                    Content = layout;
                }
                else
                {
                    ScrollView scrollView = new ScrollView() { Padding = 10 };

                    if (Task.tasks.Length > 0)
                    {
                        StackLayout[] new_lay_arr = new StackLayout[Task.tasks.Length];
                        lay_arr = new_lay_arr;

                        for (int i = 0; i < Task.tasks.Length; i++)
                        {
                            StackLayout vertLayout = new StackLayout();

                            Frame task_frame = new Frame()
                            {
                                BorderColor = Color.Black,
                                CornerRadius = 10,
                                Padding = 10,
                                Content = vertLayout
                            };

                            Label header_label = new Label()
                            {
                                Text = Task.tasks[i].header,
                                TextColor = Color.Black,
                                FontSize = 30
                            };

                            Label priority_label = new Label()
                            {
                                Text = Task.tasks[i].priority.ToString(),
                                TextColor = Color.Black,
                                FontSize = 15
                            };

                            Label deadline_label = new Label()
                            {
                                Text = Task.tasks[i].deadline,
                                TextColor = Color.Black,
                                FontSize = 15
                            };

                            Label worker_label = new Label()
                            {
                                Text = Task.tasks[i].worker ?? "Над заданием никто не работает",
                                TextColor = Color.Black,
                                FontSize = 15
                            };

                            vertLayout.Children.Add(header_label);
                            vertLayout.Children.Add(priority_label);
                            vertLayout.Children.Add(deadline_label);
                            vertLayout.Children.Add(worker_label);

                            layout.Children.Add(task_frame);

                            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
                            tapGestureRecognizer.Tapped += ShowInfo;
                            vertLayout.GestureRecognizers.Add(tapGestureRecognizer);

                            lay_arr[i] = vertLayout;
                        }

                        scrollView.Content = layout;
                        Content = scrollView;
                    }
                }
            }
        }
    }
}