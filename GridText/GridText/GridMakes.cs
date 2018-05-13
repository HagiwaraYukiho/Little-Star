using System;
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
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:GridText"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:GridText;assembly=GridText"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:GridMakes/>
    ///
    /// </summary>

    public class GridMakes : UniformGrid
    {
        private UniformGrid _UniformGrid;
        public GridMakes()
        {
            this.Loaded += CheckBoxMake;
        }

        private static int GridCount = 13;
        private static int DataCount = 0;
        private static int PageState = 1;
        private static int PageMin = 1;
        private static int PageMax = 20;

        private static string[] buttonStrings =
            {"T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12", "T13", "T14"};

        private void CheckBoxMake(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < GridCount; i++)
            {
                CheckBox newCheckBox = new CheckBox { Content = i+"number" };
                newCheckBox.Checked += CheckBox_Checked;
                this.Children.Add(newCheckBox);
            }
            

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Children.Clear();
        }


    }
}
