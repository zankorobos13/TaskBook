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
    public partial class TaskInfoPage : ContentPage
    {
        public TaskInfoPage()
        {
            InitializeComponent();
        }

        private async void CompleteTask(object sender, EventArgs e)
        {
            if (await DisplayAlert("Вы уверены?", "Вы действительно хотите завершить задание? Отменить это действие будет невозможно", "Да", "Нет") && DB.CompleteTask(Task.current_task.header) == "ok")
            {
                await DisplayAlert("Успех!", "Вы успешно завершили задание", "OK");
                await Navigation.PopAsync();
            }
            else if (DB.CompleteTask(Task.current_task.header) == "error")
            {
                await DisplayAlert("Ошибка!", "Ошибка подключения к базе данных", "OK");
            }
        }

        private async void DeleteTask(object sender, EventArgs e)
        {
            if (await DisplayAlert("Вы уверены?", "Вы действительно хотите удалить задание? Отменить это действие будет невозможно", "Да", "Нет") && DB.DeleteTask(Task.current_task.header) == "ok")
            {
                await DisplayAlert("Успех!", "Вы успешно удалили задание", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Ошибка!", "Ошибка подключения к базе данных", "OK");
            }
        }

        private async void AcceptTask(object sender, EventArgs e)
        {
            if (DB.AcceptTask(Task.current_task.header) == "ok")
            {
                foreach (Task item in Task.tasks)
                {
                    if (item.header == Task.current_task.header)
                    {
                        Task.current_task = item;
                        OnAppearing();
                    }
                }
            }
            else
            {
                await DisplayAlert("Ошибка!", "Ошибка подключения к базе данных", "OK");
            }
        }

        private async void DeclineTask(object sender, EventArgs e)
        {
            if (DB.DeclineTask(Task.current_task.header) == "ok")
            {
                foreach (Task item in Task.tasks)
                {
                    if (item.header == Task.current_task.header)
                    {
                        Task.current_task = item;
                        OnAppearing();
                    }
                }
            }
            else
            {
                await DisplayAlert("Ошибка!", "Ошибка подключения к базе данных", "OK");
            }
        }

        protected override void OnAppearing()
        {
            StackLayout layout = new StackLayout() { Padding = new Thickness(30, 10, 30, 10) };

            Label header_label = new Label()
            {
                Text = Task.current_task.header,
                TextColor = Color.Black,
                FontSize = 27,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            Label text_label = new Label()
            {
                Text = Task.current_task.text,
                TextColor = Color.Black,
                FontSize = 18
            };

            Frame text_frame = new Frame()
            {
                BorderColor = Color.Black,
                CornerRadius = 10,
                Padding = 5,
                Content = text_label
            };

            text_frame.HeightRequest = 370 - ((Task.current_task.header.Length / 21 + 1) * 25);


            Label priority_label = new Label()
            {
                Text = "Приоритет: " + Task.current_task.priority.ToString(),
                TextColor = Color.Black,
                FontSize = 20
            };

            string[] deadline_arr = Task.current_task.deadline.Split(' ');

            Label deadline_label = new Label()
            {
                Text = "Дедлайн: " + deadline_arr[0] + "." + deadline_arr[1] + "." + deadline_arr[2] + " " + deadline_arr[3] + ":" + deadline_arr[4],
                TextColor = Color.Black,
                FontSize = 20
            };

            Label worker_label = new Label()
            {
                TextColor = Color.Black,
                FontSize = 20
            };

            worker_label.Text = Task.current_task.worker == null ? "Над заданием никто не работает" : "Исполнитель: " + (Task.current_task.worker == Preferences.Get("login", null) ? "Вы" : Task.current_task.worker);


            layout.Children.Add(header_label);
            layout.Children.Add(text_frame);
            layout.Children.Add(priority_label);
            layout.Children.Add(deadline_label);
            layout.Children.Add(worker_label);

            if (Preferences.Get("role", null) == "user")
            {
                Button button = new Button()
                {
                    FontSize = 23,
                    TextColor = Color.White,
                    CornerRadius = 20,
                    Margin = new Thickness(0, 10, 0, 0)
                };

                if (Task.current_task.worker != Preferences.Get("login", null))
                {
                    button.Text = "Принять задание";
                    button.BackgroundColor = Color.FromHex("#006400");
                    button.Clicked += AcceptTask;
                    layout.Children.Add(button);

                }
                else
                {
                    button.Text = "Отказаться от задания";
                    button.BackgroundColor = Color.FromHex("#8B0000");
                    button.Clicked += DeclineTask;
                    layout.Children.Add(button);

                    Button complete_button = new Button()
                    {
                        Text = "Завершить задание",
                        FontSize = 23,
                        TextColor = Color.White,
                        BackgroundColor = Color.FromHex("#006400"),
                        CornerRadius = 20,
                        Margin = new Thickness(0, 10, 0, 0)
                    };

                    complete_button.Clicked += CompleteTask;

                    layout.Children.Add(complete_button);

                }
            }
            else
            {
                Label status_label = new Label()
                {
                    Text = "Статус: ",
                    FontAttributes = FontAttributes.Bold,
                    HorizontalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    FontSize = 30
                };

                if (Task.current_task.completed_status)
                {
                    status_label.Text += "выполнено";
                    status_label.TextColor = Color.FromHex("#006400");
                }
                else
                {
                    if (Task.current_task.worker != null)
                    {
                        status_label.Text += "в процессе выполнения";
                        status_label.TextColor = Color.FromHex("#4682B4");
                    }
                    else
                    {
                        status_label.Text += "не принято";
                        status_label.TextColor = Color.FromHex("#8B0000");
                    }
                }


                /*if (Task.current_task.worker != null)
                {
                    if (Task.current_task.completed_status == true)
                    {
                        status_label.Text += "выполнено";
                        status_label.TextColor = Color.FromHex("#006400");
                    }
                    else
                    {
                        status_label.Text += "в процессе выполнения";
                        status_label.TextColor = Color.FromHex("#4682B4");
                    }
                }
                else
                {
                    status_label.Text += "не принято";
                    status_label.TextColor = Color.FromHex("#8B0000");
                }*/

                Button delete_task_button = new Button()
                {
                    Text = "Удалить задание",
                    FontSize = 23,
                    TextColor = Color.White,
                    BackgroundColor = Color.FromHex("#8B0000"),
                    CornerRadius = 20,
                    Margin = new Thickness(0, 10, 0, 0)
                };

                delete_task_button.Clicked += DeleteTask;

                layout.Children.Add(status_label);
                layout.Children.Add(delete_task_button);
            }

            

            Content = layout;
            
        }
    }
}