using System.Collections.Generic;
using System.Linq;
using global::MediAid.Models;
using global::MediAid.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;
using static Xamarin.Forms.Device;

namespace MediAid.Views
{
    public class PillsSelectListPage : ContentPage
    {

        private DrugsListPageViewModel viewModel;
        private ListView mainList;
        private Reminder reminder;

        public List<WrappedSelection<Drug>> WrappedItems = new List<WrappedSelection<Drug>>();

        public PillsSelectListPage(ref Reminder reminder)
        {
            this.reminder = reminder;
            Title = "Select Pills";
            BindingContext = viewModel = new DrugsListPageViewModel();


            mainList = new ListView()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HasUnevenRows = true,
                ItemTemplate = new DataTemplate(typeof(PillsTemplate)),
            };
            mainList.ItemSelected += (sender, e) =>
            {
                if (e.SelectedItem == null) return;
                var o = (WrappedSelection<Drug>)e.SelectedItem;
                o.IsSelected = !o.IsSelected;
                ((ListView)sender).SelectedItem = null;
            };
            Content = mainList;
            ToolbarItems.Add(new ToolbarItem("All", null, SelectAll, ToolbarItemOrder.Primary));
            ToolbarItems.Add(new ToolbarItem("None", null, SelectNone, ToolbarItemOrder.Primary));
            ToolbarItems.Add(new ToolbarItem("Done", null, Done, ToolbarItemOrder.Primary));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Drugs.Count == 0)
                viewModel.LoadDrugsCommand.Execute(null);

            WrappedItems = viewModel.Drugs.Select(item => new WrappedSelection<Drug>() { Item = item, IsSelected = false }).ToList();

            mainList.ItemsSource = WrappedItems;

        }

        async void Done()
        {
            MessagingCenter.Send(this, "AddReminder", reminder);
            await Navigation.PopToRootAsync();
        }

        void SelectAll()
        {
            foreach (var wi in WrappedItems)
            {
                wi.IsSelected = true;
            }
        }
        void SelectNone()
        {
            foreach (var wi in WrappedItems)
            {
                wi.IsSelected = false;
            }
        }

        public class WrappedSelection<T> : INotifyPropertyChanged
        {
            public T Item { get; set; }
            bool isSelected = false;
            public bool IsSelected
            {
                get
                {
                    return isSelected;
                }
                set
                {
                    if (isSelected != value)
                    {
                        isSelected = value;
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                    }
                }
            }
            public event PropertyChangedEventHandler PropertyChanged = delegate { };
        }

        public class PillsTemplate : ViewCell
        {
            public PillsTemplate() : base()
            {
                Grid grid = new Grid();
                grid.Padding = new Thickness(5);
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(50);
                ColumnDefinition switchCol = new ColumnDefinition();
                switchCol.Width = new GridLength(50);
                ColumnDefinition imgCol = new ColumnDefinition();
                imgCol.Width = new GridLength(1, GridUnitType.Star);
                ColumnDefinition detailsCol = new ColumnDefinition();
                detailsCol.Width = new GridLength(2, GridUnitType.Star);
                grid.RowDefinitions.Add(rowDefinition);
                grid.ColumnDefinitions.Add(imgCol);
                grid.ColumnDefinitions.Add(detailsCol);
                grid.ColumnDefinitions.Add(switchCol);

                Switch mainSwitch = new Switch();
                mainSwitch.SetBinding(Switch.IsToggledProperty, new Binding("IsSelected"));
                Grid.SetRow(mainSwitch, 0);
                Grid.SetColumn(mainSwitch, 2);

                Image img = new Image()
                {
                VerticalOptions = LayoutOptions.Center,
                Aspect = Aspect.AspectFit,
                HeightRequest = 50,
                WidthRequest = 50
                };
                img.SetBinding(Image.SourceProperty, new Binding("Item.ImageFile") { Converter = new ImageConverter() });
                Grid.SetRow(img, 0);
                Grid.SetColumn(img, 0);

                StackLayout stackLayout = new StackLayout();
                Label name = new Label()
                {
                    FontSize = 16,
                    Style = Styles.ListItemTextStyle,
                };
                name.SetBinding(Label.TextProperty, new Binding("Item.Name"));
                Label drugType = new Label()
                {
                    FontSize = 16,
                    Style = Styles.ListItemDetailTextStyle,
                };
                drugType.SetBinding(Label.TextProperty, new Binding("Item.DrugType") { Converter = new DrugTypeConverter() });

                stackLayout.Children.Add(name);
                stackLayout.Children.Add(drugType);

                Grid.SetRow(stackLayout, 0);
                Grid.SetColumn(stackLayout, 1);

                grid.Children.Add(mainSwitch);
                grid.Children.Add(img);
                grid.Children.Add(stackLayout);

                View = grid;
            }
        }


    }


}
