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
        public HomePage()
        {
            InitializeComponent();
        }

        private async void CreateTask(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateTaskPage());
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
                }
                else
                {

                }

                Content = layout;
            }
        }
    }
}