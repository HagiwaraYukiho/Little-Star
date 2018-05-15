using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using GridText.DataInterface;
using Reactive.Bindings;

namespace GridText
{
    public class GridMakes : UniformGrid
    {
        private ReadOnlyObservableCollection<AutoGridConstruct> _autoGridDataGroup;
        public static readonly DependencyProperty MultiSelectProperty = DependencyProperty.Register(
            "MultiSelect", typeof(bool), typeof(GridMakes), new PropertyMetadata(false));

        public bool MultiSelect
        {
            get => (bool)GetValue(MultiSelectProperty);
            set => SetValue(MultiSelectProperty, value);
        }

        public static readonly DependencyProperty MaxSelectCountProperty = DependencyProperty.Register(
            "MaxSelectCount", typeof(int), typeof(GridMakes), new PropertyMetadata(2));

        public int MaxSelectCount
        {
            get => (int)GetValue(MaxSelectCountProperty);
            set => SetValue(MaxSelectCountProperty, value);
        }

        public static readonly DependencyProperty SelectedBoxpProperty = DependencyProperty.Register(
            "SelectedBox", typeof(string), typeof(GridMakes), new PropertyMetadata(string.Empty));

        public string SelectedBox
        {
            get => (string)GetValue(SelectedBoxpProperty);
            set => SetValue(SelectedBoxpProperty, value);
        }

        public static readonly DependencyProperty AddPageSourceProperty = DependencyProperty.Register(
            "AddPageSource", typeof(string), typeof(GridMakes), new PropertyMetadata(string.Empty));

        public string AddPageSource
        {
            get => (string)GetValue(AddPageSourceProperty);
            set => SetValue(AddPageSourceProperty, value);
        }

        public GridMakes()
        {
            Loaded += CheckBoxInit;
        }

        private static int _gridCount = 0;
        private static int _dataCount = 0;
        private static int _nowPage = 1;
        private const int MinPage = 1;
        private static double _maxPage = 0;
        private static readonly Dictionary<string, string> CheckedItm = new Dictionary<string, string>();

        private static readonly string[] ButtonStrings =
        {
            "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12", "T13", "T14", "T15", "T16",
            "T17", "T18", "T19", "T20", "T21", "T22", "T23", "T24", "T25", ",gh", ",gh", ",gh", ",ghdfg", ",gh", ",gh",
            ",gh", ",ghghj", ",gh", ",ghjh", ",gh", ",gjhh", ",gh", ",gh", ",gh", ",ghjhj", ",gh", ",gh", ",gh", ",8gh",
            ",gh", ",gh9", ",gh", ",gh", ",g0h", ",g-h", ",gh", ",gh", ",g2h", ",gh"
        };

        private void Page_Control()
        {
            var buttonNext = new CheckBox { Content = "次へ" };
            buttonNext.Checked += Button_Next;
            var buttonPervious = new CheckBox { Content = "前へ" };
            buttonPervious.Checked += Button_Pervious;
            var buttonCreate = new CheckBox { Content = "追加する" };
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
            _autoGridDataGroup = (ReadOnlyObservableCollection< AutoGridConstruct>)DataContext;
            CheckedItm.Clear();
            for (var i = 0; i < _gridCount; i++)
            {
                try
                {
                    if (_autoGridDataGroup[_dataCount + i].DataStatus==0)
                    {
                        var dataCheckBox = new CheckBox
                        {
                            Content = _autoGridDataGroup[_dataCount + i].DataName,
                            Name = _autoGridDataGroup[_dataCount + i].DataID,
                        };

                        dataCheckBox.Checked += Checked_Event;
                        dataCheckBox.Unchecked += Unchecked_Event;
                        //if (CheckedItm.ContainsKey(DataCheckBox.Name))
                        //    DataCheckBox.IsChecked = true;
                        Children.Add(dataCheckBox);
                    }
                    
                }
                catch (Exception)
                {
                    var emptyCheckBox = new CheckBox { Content = "ブランク" };
                    Children.Add(emptyCheckBox);
                }
            }
        }

        private void CheckBoxInit(object sender, RoutedEventArgs e)
        {
            if (Rows == 0 || Columns == 0)
            {
                Rows = 2;
                Columns = 2;
            }

            _gridCount = Rows * Columns - 2;

            _maxPage = (double)_autoGridDataGroup.Count / _gridCount;
            _maxPage = Math.Ceiling((_maxPage));
            Data_Make();
            Page_Control();
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
            var addButton = (CheckBox)sender;
            addButton.IsChecked = false;
            var newWindow = new NavigationWindow { Source = new Uri(AddPageSource, UriKind.Relative) };
            newWindow.Show();
        }

        private void Checked_Event(object sender, RoutedEventArgs e)
        {
            var checkedBox = (CheckBox)sender;

            foreach (CheckBox ckd in this.Children)
            {
                if (ckd.IsChecked == true)
                {
                    if (CheckedItm.ContainsKey(ckd.Name))
                        CheckedItm.Remove(ckd.Name);
                    CheckedItm.Add(ckd.Name, ckd.Content.ToString());
                }
            }

            if (CheckedItm.Count > MaxSelectCount)
            {
                if (MaxSelectCount == 1)
                {
                    CheckedItm.Clear();
                    foreach (CheckBox ckdc in this.Children)
                    {
                        if (ckdc.IsChecked == true)
                            ckdc.IsChecked = false;
                    }

                    checkedBox.IsChecked = true;
                }
                else
                {
                    MessageBox.Show("LimitCount Over!");
                    foreach (CheckBox ckdc in this.Children)
                    {
                        ckdc.IsChecked = false;
                    }

                    CheckedItm.Clear();
                }
            }

            SelectedBox = "";
            foreach (var dicStr in CheckedItm.Values)
            {
                SelectedBox += dicStr;
            }
        }

        private void Unchecked_Event(object sender, RoutedEventArgs e)
        {
            SelectedBox = "";

            foreach (CheckBox ckd in this.Children)
            {
                if (ckd.IsChecked == false)
                {
                    CheckedItm.Remove(ckd.Name);
                }
            }

            foreach (var value in CheckedItm.Values)
            {
                SelectedBox += value;
            }
        }
    }
}