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
    public partial class CreateTaskPage : ContentPage
    {
        public CreateTaskPage()
        {
            InitializeComponent();
        }

        

        protected override void OnAppearing()
        {
            StackLayout layout = new StackLayout();

            Entry header_entry = new Entry()
            {
                Placeholder = "Заголовок",
                TextColor = Color.Black,
                Margin = new Thickness(20, 0, 20, 0),
                FontSize = 20
            };

            Frame header_frame = new Frame()
            {
                BorderColor = Color.Black,
                CornerRadius = 50,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(30, 50, 30, 0),
                Padding = new Thickness(0, 5, 0, 5),
                Content = header_entry
            };

            Editor text = new Editor()
            {
                Placeholder = "Текст задания",
                FontSize = 20,
                WidthRequest = 215,
                HeightRequest = 300
            };

            Frame text_frame = new Frame()
            {
                BorderColor = Color.Black,
                CornerRadius = 30,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(30, 20, 30, 0),
                Padding = new Thickness(10, 5, 10, 5),
                Content = text
            };

            StackLayout priority_layout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Margin = new Thickness(30, 15, 30, 0)
            };

            Label priority_label = new Label()
            {
                Text = "Приоритет: ",
                FontSize = 25,
                TextColor = Color.Black,
                Margin = new Thickness(0, 10, 0, 0)
            };

            Entry priority_entry = new Entry()
            {
                Placeholder = "0 - 100",
                Keyboard = Keyboard.Numeric,
                TextColor = Color.Black,
                FontSize = 25,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(50, 0, 0, 0)
            };

            StackLayout dateTime_layout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Margin = new Thickness(30, 5, 30, 0)
            };

            Label dateTime_label = new Label()
            {
                Text = "Дедлайн: ",
                FontSize = 25,
                TextColor = Color.Black,
                Margin = new Thickness(0, 10, 0, 0)
            };

            Entry dateTime_entry = new Entry()
            {
                Placeholder = "dd MM YY hh mm",
                TextColor = Color.Black,
                FontSize = 20,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(0, 5, 0, 0)
            };

            Button create_task_button = new Button()
            {
                Text = "Создать задание",
                FontSize = 25,
                TextColor = Color.White,
                BackgroundColor = Color.FromHex("#000080"),
                CornerRadius = 20,
                Margin = new Thickness(30, 20, 30, 0)
            };

            create_task_button.Clicked += CreateTask;

            priority_layout.Children.Add(priority_label);
            priority_layout.Children.Add(priority_entry);

            dateTime_layout.Children.Add(dateTime_label);
            dateTime_layout.Children.Add(dateTime_entry);

            layout.Children.Add(header_frame);
            layout.Children.Add(text_frame);
            layout.Children.Add(priority_layout);
            layout.Children.Add(dateTime_layout);
            layout.Children.Add(create_task_button);
            

            Content = layout;

            async void CreateTask(object sender, EventArgs e)
            {
                Task task = new Task()
                {
                    header = header_entry.Text ?? "",
                    text = text.Text ?? "",
                    worker = null,
                    room = Preferences.Get("room", null),
                    completed_status = false
                };

                if (task.header.Trim(' ').Length == 0)
                {
                    await DisplayAlert("Ошибка!", "Пустой заголовок задания", "OK");
                    // header_frame.BorderColor = Color.Red;
                    return;
                }
                else if (task.text.Trim(' ').Length == 0)
                {
                    await DisplayAlert("Ошибка!", "Пустой текст задания", "OK");
                    return;
                }
                else if (task.header.Length >= 90)
                {
                    await DisplayAlert("Ошибка!", "Заголовок задания должен содеражать менее 90 символов", "OK");
                    return;
                }

                bool isOk = true;

                try
                {
                    int priority = int.Parse(priority_entry.Text);

                    if (priority >= 0 && priority <= 100)
                        task.priority = priority;
                    else
                        await DisplayAlert("Ошибка!", "Недопустимое значение приоритета", "OK");
                }
                catch (Exception)
                {
                    await DisplayAlert("Ошибка!", "Недопустимое значение приоритета", "OK");
                    isOk = false;
                }


                if (isOk)
                {
                    try
                    {
                        string[] dateTimeSplitArr = (dateTime_entry.Text ?? "").Trim(' ').Split(' ');

                        if (dateTimeSplitArr.Length > 5)
                            throw new Exception();

                        DateTime dateTime = new DateTime(int.Parse(dateTimeSplitArr[0]), int.Parse(dateTimeSplitArr[1]), int.Parse(dateTimeSplitArr[2]), int.Parse(dateTimeSplitArr[3]), int.Parse(dateTimeSplitArr[4]), 0);

                        task.deadline = dateTime_entry.Text;
                    }
                    catch (Exception)
                    {
                        await DisplayAlert("Ошибка!", "Недопустимое значение даты и времени", "OK");
                        isOk = false;
                    }
                }
             
                if (isOk)
                {
                    string create_task_status = Task.CreateTask(task);

                    if (create_task_status == "ok")
                    {
                        await DisplayAlert("Успех!", "Задание успешно создано", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Ошибка!", "Ошибка подключения к базе данных, невозможно создать задание", "OK");
                    }
                }
            }
        }
    }
}