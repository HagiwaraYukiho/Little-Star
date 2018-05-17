using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Collections.ObjectModel;
using GridText.DataInterface;
using Reactive.Bindings;

namespace GridText
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public ReadOnlyObservableCollection<AutoGridConstruct> DummyDataManage { get; set; }
        readonly ObservableCollection<AutoGridConstruct> dummyData = new ObservableCollection<AutoGridConstruct>();
        public MainWindow()
        {
           
            InitializeComponent();
            DummyDataMake();

        }

        private void DummyDataMake()
        {

            
            for (int i = 0; i < 40; i++)
            {
                dummyData.Add(DataFormat("T"+i, i.ToString(), 0));
            }

           DummyDataManage = dummyData.ToReadOnlyReactiveCollection();
            GridMakes.AutoGridData = DummyDataManage;
            //this.GridMakes.DataContext= DummyDataManage;
        }

        private AutoGridConstruct DataFormat(string dataId, string dataName, int dataStatus)
        {
            AutoGridConstruct data = new AutoGridConstruct()
            {
                DataId = dataId,
                DataName = dataName,
                DataStatus = dataStatus,
            };
            return data;
        }
    }
}
