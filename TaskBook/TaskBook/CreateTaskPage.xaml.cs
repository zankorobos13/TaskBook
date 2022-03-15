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
    public partial class CreateTaskPage : ContentPage
    {
        public CreateTaskPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            StackLayout layout = new StackLayout();

            Entry login_entry = new Entry()
            {
                Placeholder = "Заголовок",
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
                Placeholder = "dd MM YY hh mm ss",
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


            priority_layout.Children.Add(priority_label);
            priority_layout.Children.Add(priority_entry);

            dateTime_layout.Children.Add(dateTime_label);
            dateTime_layout.Children.Add(dateTime_entry);

            layout.Children.Add(login_frame);
            layout.Children.Add(text_frame);
            layout.Children.Add(priority_layout);
            layout.Children.Add(dateTime_layout);
            layout.Children.Add(create_task_button);
            

            Content = layout;
        }
    }
}