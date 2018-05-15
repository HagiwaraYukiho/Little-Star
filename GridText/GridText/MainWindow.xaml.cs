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

        public ReadOnlyObservableCollection<AutoGridConstruct> DummyDataManage;

        public ReadOnlyObservableCollection<AutoGridConstruct> _DummyDataManage
        {
            get => DummyDataManage;
            set => DummyDataManage = value;
        }
        public MainWindow()
        {
           
            InitializeComponent();
            DummyDataMake();

        }

        private void DummyDataMake()
        {
            ObservableCollection<AutoGridConstruct> dummyData = new ObservableCollection<AutoGridConstruct>();
            
            for (int i = 0; i < 40; i++)
            {
                dummyData.Add(DataFormat("T"+i, i.ToString(), 0));
            }

            DummyDataManage = dummyData.ToReadOnlyReactiveCollection();
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
