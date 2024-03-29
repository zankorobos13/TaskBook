﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskBook
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskListPage : ContentPage
    {
        public StackLayout[] lay_arr = null;

        public TaskListPage()
        {
            InitializeComponent();
        }

        private async void Sort(object sender, EventArgs e)
        {
            string sort_method = await DisplayActionSheet("Метод сортировки", "Отмена", "Выйти", "Сначала новые", "Сначала старые", "По приоритету (уб)", "По приоритету (возр)", "Дедлайн (уб)", "Дедлайн (возр)", "Свободные");

            if (sort_method == "Сначала новые")
            {
                Task.SortByCreation();
            }
            else if (sort_method == "Сначала старые")
            {
                Task.SortByCreation(false);
            }
            else if (sort_method == "По приоритету (уб)")
            {
                Task.SortByPriority();
            }
            else if (sort_method == "По приоритету (возр)")
            {
                Task.SortByPriority(false);
            }
            else if (sort_method == "Дедлайн (уб)")
            {
                Task.SortByDateTime();
            }
            else if (sort_method == "Дедлайн (возр)")
            {
                Task.SortByDateTime(false);
            }
            else if (sort_method == "Свободные")
            {
                Task.SortByFree();
            }

            OnAppearing();
        }

        private async void ShowInfo(object sender, EventArgs e)
        {
            for (int i = 0; i < lay_arr.Length; i++)
            {
                if (lay_arr[i] == (sender as StackLayout))
                {
                    Task.current_task = Task.tasks[i];
                    await Navigation.PushAsync(new TaskInfoPage());
                }
            }
        }

        async protected override void OnAppearing()
        {
            
            StackLayout layout = new StackLayout();
            ScrollView scrollView = new ScrollView() { Padding = 10 };

            Button sort_button = new Button()
            {
                Text = "Сортировать",
                FontSize = 25,
                TextColor = Color.White,
                BackgroundColor = Color.FromHex("#000080"),
                CornerRadius = 20
            };

            sort_button.Clicked += Sort;

            layout.Children.Add(sort_button);

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
                        HasShadow = false,
                        Content = vertLayout
                    };

                    Label header_label = new Label()
                    {
                        Text = Task.tasks[i].header,
                        TextColor = Color.Black,
                        FontSize = 30,
                        FontAttributes = FontAttributes.Bold
                    };

                    Label priority_label = new Label()
                    {
                        Text = "Приоритет: " + Task.tasks[i].priority.ToString(),
                        TextColor = Color.Black,
                        FontSize = 18
                    };

                    string[] deadline_arr = Task.tasks[i].deadline.Split(' ');

                    Label deadline_label = new Label()
                    {
                        Text = "Дедлайн: " + deadline_arr[0] + "." + deadline_arr[1] + "." + deadline_arr[2] + " " + deadline_arr[3] + ":" + deadline_arr[4],
                        TextColor = Color.Black,
                        FontSize = 18
                    };


                    Label worker_label = new Label()
                    {
                        FontSize = 18
                    };

                    worker_label.Text = Task.tasks[i].worker == null ? "Над заданием никто не работает" : "Исполнитель: " + Task.tasks[i].worker;
                    worker_label.TextColor = Task.tasks[i].worker == null ? Color.Red : Color.Green;

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
            else
            {
                layout.Padding = 10;
                Content = layout;
            }
            
        }
    }
}