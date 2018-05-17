using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using GridText.DataInterface;

namespace GridText
{
    public class GridMakes : UniformGrid
    {
        private const int MinPage = 1;
        private const string Empty = "";

        public static readonly DependencyProperty MaxSelectCountProperty = DependencyProperty.Register(
            "MaxSelectCount", typeof(int), typeof(GridMakes), new PropertyMetadata(2));

        public static readonly DependencyProperty SelectedBoxpProperty = DependencyProperty.Register(
            "SelectedText", typeof(string), typeof(GridMakes), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty AddPageSourceProperty = DependencyProperty.Register(
            "AddPageSource", typeof(string), typeof(GridMakes), new PropertyMetadata(string.Empty));

        //public static readonly DependencyProperty GridDataProperty = DependencyProperty.Register(
        //    "GridData", typeof(ReadOnlyObservableCollection<object>), typeof(GridMakes), new PropertyMetadata(string.Empty));
        private static int _gridCount;
        private static int _dataCount;
        private static int _nowPage = 1;
        private static double _maxPage;
        private static readonly Dictionary<string, string> CheckedItem = new Dictionary<string, string>();
        private static readonly Dictionary<string, int> MapDictionary = new Dictionary<string, int>();
        private ReadOnlyObservableCollection<AutoGridConstruct> _autoGridDataGroup;
        public GridMakes()
        {
            Loaded += CheckBoxInit;
        }

        public ReadOnlyObservableCollection<AutoGridConstruct> AutoGridData { get; set; }
        public List<AutoGridConstruct> SelectedItem=new List<AutoGridConstruct>();

        public int MaxSelectCount
        {
            get => (int) GetValue(MaxSelectCountProperty);
            set => SetValue(MaxSelectCountProperty, value);
        }

        public string SelectedText
        {
            get => (string) GetValue(SelectedBoxpProperty);
            set => SetValue(SelectedBoxpProperty, value);
        }

        public string AddPageSource
        {
            get => (string) GetValue(AddPageSourceProperty);
            set => SetValue(AddPageSourceProperty, value);
        }

//        public string GridData
//        {
//            get => (string)GetValue(GridDataProperty);
//            set => SetValue(AddPageSourceProperty, value);
//        }

        private void Page_Control()
        {
            var buttonNext = new CheckBox {Content = "次へ"};
            buttonNext.Checked += Button_Next;
            var buttonPervious = new CheckBox {Content = "前へ"};
            buttonPervious.Checked += Button_Pervious;
            var buttonCreate = new CheckBox {Content = "追加する"};
            buttonCreate.Checked += Button_Add;

            if (_nowPage == MinPage)
            {
                Children.Add(buttonCreate);
                Children.Add(buttonNext);
            }
            else if (_nowPage > MinPage && _nowPage < _maxPage)
            {
                Children.Add(buttonPervious);
                Children.Add(buttonNext);
            }
            else
            {
                Children.Add(buttonPervious);
                Children.Add(buttonCreate);
            }
        }

        private void Data_Make()
        {
            CheckedItem.Clear();
            SelectedItem.Clear();
            for (var i = 0; i < _gridCount; i++)
                if (_autoGridDataGroup.Count > _dataCount + i)
                {
                    if (_autoGridDataGroup[_dataCount + i].DataStatus == 0)
                    {
                        var dataCheckBox = new CheckBox
                        {
                            Content = _autoGridDataGroup[_dataCount + i].DataName,
                            Name = _autoGridDataGroup[_dataCount + i].DataId
                        };
                        if (MapDictionary.ContainsKey(Name + i))
                            MapDictionary.Remove(Name + i);
                        MapDictionary.Add(dataCheckBox.Name, _dataCount + i);
                        dataCheckBox.Checked += Checked_Event;
                        dataCheckBox.Unchecked += Unchecked_Event;
                        //if (CheckedItm.ContainsKey(DataCheckBox.Name))
                        //    DataCheckBox.IsChecked = true;
                        Children.Add(dataCheckBox);
                    }
                }
                else
                {
                    var emptyCheckBox = new CheckBox {Content = "追加"};
                    emptyCheckBox.Checked += Button_Add;
                    Children.Add(emptyCheckBox);
                }
        }

        private void CheckBoxInit(object sender, RoutedEventArgs e)
        {
            _autoGridDataGroup = AutoGridData;
            if (Rows == 0 || Columns == 0)
            {
                Rows = 2;
                Columns = 2;
            }

            _gridCount = Rows * Columns - 2;

            _maxPage = (double) _autoGridDataGroup.Count / _gridCount;
            _maxPage = Math.Ceiling(_maxPage);
            Data_Make();
            Page_Control();
            SelectedText = Empty;
        }

        private void Button_Next(object sender, RoutedEventArgs e)
        {
            _nowPage++;
            if (_nowPage != 1)
                _dataCount = (_nowPage - 1) * _gridCount;
            Children.Clear();
            Data_Make();
            Page_Control();
        }

        private void Button_Pervious(object sender, RoutedEventArgs e)
        {
            _nowPage--;
            _dataCount = (_nowPage - 1) * _gridCount;
            Children.Clear();
            Data_Make();
            Page_Control();
        }

        private void Button_Add(object sender, RoutedEventArgs e)
        {
            var addButton = (CheckBox) sender;
            var newWindow = new NavigationWindow {Source = new Uri(AddPageSource, UriKind.Relative)};
            newWindow.Show();
            addButton.IsChecked = false;
        }

        private void Checked_Event(object sender, RoutedEventArgs e)
        {
            var checkedBox = (CheckBox) sender;

            foreach (CheckBox ckd in Children)
                if (ckd.IsChecked == true)
                {
                    if (CheckedItem.ContainsKey(ckd.Name))
                        CheckedItem.Remove(ckd.Name);
                    CheckedItem.Add(ckd.Name, ckd.Content.ToString());
                }

            if (CheckedItem.Count > MaxSelectCount)
            {
                if (MaxSelectCount == 1)
                {
                    CheckedItem.Clear();
                    foreach (CheckBox ckdc in Children)
                        if (ckdc.IsChecked == true)
                            ckdc.IsChecked = false;

                    checkedBox.IsChecked = true;
                }
                else
                {
                    MessageBox.Show("LimitCount Over!");
                    foreach (CheckBox ckdc in Children) ckdc.IsChecked = false;

                    CheckedItem.Clear();
                }
            }

            SelectedText = Empty;
            SelectedItem.Clear();
            foreach (var value in CheckedItem.Values)SelectedText += value;
            
            foreach (var key in CheckedItem.Keys)SelectedItem.Add(_autoGridDataGroup[MapDictionary[key]]);
        }

        private void Unchecked_Event(object sender, RoutedEventArgs e)
        {
            SelectedText = Empty;
            SelectedItem.Clear();
            foreach (CheckBox ckd in Children)
                if (ckd.IsChecked == false)
                    CheckedItem.Remove(ckd.Name);

            foreach (var value in CheckedItem.Values) SelectedText += value;
            foreach (var key in CheckedItem.Keys) SelectedItem.Add(_autoGridDataGroup[MapDictionary[key]]);
        }
    }
}