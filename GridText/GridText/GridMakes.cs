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

            DummyDataMake();
            Loaded += CheckBoxInit;
        }

        private static int _gridCount = 0;
        private static int _dataCount = 0;
        private static int _nowPage = 1;
        private const int MinPage = 1;
        private static double _maxPage = 0;
        private static readonly Dictionary<string, string> CheckedItm = new Dictionary<string, string>();
        private ObservableCollection<AutoGridConstruct> dummyData = new ObservableCollection<AutoGridConstruct>();
        private ReadOnlyObservableCollection<AutoGridConstruct> _autoGridDataGroup;

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
            //_autoGridDataGroup = (ReadOnlyReactiveCollection<AutoGridConstruct>)DataContext;
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

        /**  DummyDataMake  */
        private void DummyDataMake()
        {


            for (int i = 0; i < 40; i++)
            {
                dummyData.Add(DataFormat("T" + i, i.ToString(), 0));
            }

            _autoGridDataGroup = dummyData.ToReadOnlyReactiveCollection();
        }

        private AutoGridConstruct DataFormat(string dataId, string dataName, int dataStatus)
        {
            AutoGridConstruct data = new AutoGridConstruct()
            {
                DataID = dataId,
                DataName = dataName,
                DataStatus = dataStatus,
            };
            return data;
        }
    }
}