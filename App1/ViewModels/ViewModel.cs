using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Media;
using static ItemsRepeaterDemo.Extensions.EnumerableExtensions;

namespace ItemsRepeaterDemo.ViewModels
{
    public class MainViewModel
    {
        static int index;

        public MainViewModel()
        {
            index = 0;
            TopItems = GenerateTopItems(500);
            FilteredItems = new ObservableCollection<TopItem>();

            MenuItems = new List<TopItemType>
            {
                TopItemType.Animals,
                TopItemType.Cars,
                TopItemType.Continents,
                TopItemType.Plants
            };

            SelectedMenuItem = TopItemType.Animals;
        }

        private List<TopItem> GenerateTopItems(int count)
        {
            List<TopItem> topItems = new List<TopItem>();

            for (int i = 0; i < count; i++)
            {
                TopItem topItem = new TopItem
                {
                    Type = RandomType(),
                    Name = RandomString(15),
                    SubItems = GenerateSubItems(100)
                };
                topItems.Add(topItem);
            }

            return topItems;
        }

        private ObservableCollection<SubItem> GenerateSubItems(int count)
        {
            ObservableCollection<SubItem> subItems = new ObservableCollection<SubItem>();

            for (int i = 0; i < count; i++)
            {
                SubItem subItem = new SubItem
                {
                    Name = RandomString(15, true),
                    Color = new SolidColorBrush(GenerateColor()),
                    ImageUrl = new Uri("https://picsum.photos/id/" + RandomNumber(1, 1050) + "/150/150?cachebuster=" + index++),
                    Index = index
                };
                subItems.Add(subItem);
            }

            return subItems;
        }
        private Random random = new Random();

        private Color GenerateColor()
        {
            byte[] b = new byte[4];
            random.NextBytes(b);
            return Color.FromArgb(b[0], b[1], b[2], b[3]);
        }

        public List<TopItem> TopItems { get; set; }
        public ObservableCollection<TopItem> FilteredItems { get; set; }

        public List<TopItemType> MenuItems { get; set; }

        private TopItemType _selectedMenuItem = TopItemType.Animals;
        public TopItemType SelectedMenuItem
        {
            get
            {
                return _selectedMenuItem;
            }
            set
            {
                _selectedMenuItem = value;
                OnSelectedMenuItemChanged();
            }
        }

        private void OnSelectedMenuItemChanged()
        {
            FilteredItems.RemoveRange(0, FilteredItems.Count);
            FilteredItems.Merge(TopItems.Where(topItem => topItem.Type == SelectedMenuItem));
        }

        private TopItemType RandomType()
        {
            Array values = Enum.GetValues(typeof(TopItemType));
            return (TopItemType)values.GetValue(random.Next(values.Length));
        }

        private int RandomNumber(int min, int max)
        {
            return random.Next(min, max);
        }
        private string RandomString(int size, bool lowerCase = false)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
    }

    public enum TopItemType
    {
        Animals = 1,
        Continents = 2,
        Plants = 3,
        Cars = 4
    }

    public class TopItem : ObservableObject, IMergeable<TopItem>
    {
        public TopItemType Type { get; set; }

        public string Name { get; set; }

        public ObservableCollection<SubItem> SubItems { get; set; }

        public bool Equals(TopItem other)
        {
            return false;
        }

        public bool Update(TopItem other)
        {
            return false;
        }
    }

    public class SubItem
    {
        public SolidColorBrush Color { get; set; }

        public string Name { get; set; }

        public Uri ImageUrl { get; set; }

        public int Index { get; set; }
    }
}
