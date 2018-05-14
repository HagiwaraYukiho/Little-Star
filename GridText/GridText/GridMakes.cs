using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GridText
{
    public class GridMakes : UniformGrid
    {
        public static readonly DependencyProperty MultiSelectProperty = DependencyProperty.Register(
            "MultiSelect", typeof(Boolean), typeof(GridMakes), new PropertyMetadata(false));

        public Boolean MultiSelect
        {
            get { return (Boolean) GetValue(MultiSelectProperty); }
            set { SetValue(MultiSelectProperty, value); }
        }

        public static readonly DependencyProperty MaxSelectCountProperty = DependencyProperty.Register(
            "MaxSelectCount", typeof(int), typeof(GridMakes), new PropertyMetadata(2));

        public int MaxSelectCount
        {
            get { return (int) GetValue(MaxSelectCountProperty); }
            set { SetValue(MaxSelectCountProperty, value); }
        }

        public static readonly DependencyProperty SelectedBoxpProperty = DependencyProperty.Register(
            "SelectedBox", typeof(string), typeof(GridMakes), new PropertyMetadata(""));

        public string SelectedBox
        {
            get { return (string) GetValue(SelectedBoxpProperty); }
            set { SetValue(SelectedBoxpProperty, value); }
        }

        public GridMakes()
        {
            this.Loaded += CheckBoxInit;
        }

        private static int GridCount = 0;
        private static int DataCount = 0;
        private static int NowPage = 1;
        private static int MinPage = 1;
        private static double MaxPage = 0;
        private static Dictionary<string, string> CheckedItm = new Dictionary<string, string>();

        private static string[] buttonStrings =
        {
            "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12", "T13", "T14", "T15", "T16",
            "T17", "T18", "T19", "T20", "T21", "T22", "T23", "T24", "T25", ",gh", ",gh", ",gh", ",ghdfg", ",gh", ",gh",
            ",gh", ",ghghj", ",gh", ",ghjh", ",gh", ",gjhh", ",gh", ",gh", ",gh", ",ghjhj", ",gh", ",gh", ",gh", ",8gh",
            ",gh", ",gh9", ",gh", ",gh", ",g0h", ",g-h", ",gh", ",gh", ",g2h", ",gh"
        };

        private void CheckBoxInit(object sender, RoutedEventArgs e)
        {
            GridCount = Rows * Columns - 2;
            MaxPage = (double) buttonStrings.Length / GridCount;
            MaxPage = Math.Ceiling((MaxPage));
            Data_Make();
            Page_Control();
        }

        private void Button_Next(object sender, RoutedEventArgs e)
        {
            NowPage++;
            if (NowPage != 1)
                DataCount = (NowPage - 1) * GridCount;
            this.Children.Clear();
            Data_Make();
            Page_Control();
        }

        private void Button_Pervious(object sender, RoutedEventArgs e)
        {
            NowPage--;
            DataCount = (NowPage - 1) * GridCount;

            this.Children.Clear();
            Data_Make();
            Page_Control();
        }

        private void Page_Control()
        {
            CheckBox ButtonNext = new CheckBox {Content = "次へ"};
            ButtonNext.Checked += Button_Next;
            CheckBox ButtonPervious = new CheckBox {Content = "前へ"};
            ButtonPervious.Checked += Button_Pervious;
            CheckBox ButtonCreate = new CheckBox {Content = "追加する"};
            ButtonCreate.Checked += Button_Next;
            if (NowPage == MinPage)
            {
                Children.Add(ButtonCreate);
                Children.Add(ButtonNext);
            }
            else if (NowPage > MinPage && NowPage < MaxPage)
            {
                Children.Add(ButtonPervious);
                Children.Add(ButtonNext);
            }
            else
            {
                Children.Add(ButtonPervious);
                Children.Add(ButtonCreate);
            }
        }

        private void Data_Make()
        {
            CheckedItm.Clear();
            for (var i = 0; i < GridCount; i++)
            {
                try
                {
                    var DataCheckBox = new CheckBox {Content = buttonStrings[DataCount + i]};
                    DataCheckBox.Name = "N" + NowPage + i.ToString();

                    DataCheckBox.Checked += Select_Event;
                    DataCheckBox.Unchecked += Unchecked_Event;
                    //if (CheckedItm.ContainsKey(DataCheckBox.Name))
                    //    DataCheckBox.IsChecked = true;
                    this.Children.Add(DataCheckBox);
                }
                catch (Exception e)
                {
                    var EmptyCheckBox = new CheckBox {Content = "Empty"};
                    this.Children.Add(EmptyCheckBox);
                }
            }
        }


        private void Select_Event(object sender, RoutedEventArgs e)
        {
            SelectedBox = "";
            CheckBox checkedBox = (CheckBox) sender;


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
                    foreach (CheckBox ckdc in this.Children)
                    {
                        ckdc.IsChecked = false;
                        CheckedItm.Remove(ckdc.Name);
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

            foreach (string value in CheckedItm.Values)
            {
                SelectedBox += value;
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

            foreach (string value in CheckedItm.Values)
            {
                SelectedBox += value;
            }
        }
    }
}