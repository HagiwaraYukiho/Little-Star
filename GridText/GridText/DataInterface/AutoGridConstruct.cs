using System;
using Prism.Mvvm;

namespace GridText.DataInterface
{
   public class AutoGridConstruct: BindableBase
   {
       private string _dataId; //データiD（ブラック時Grid構築時自動採番）
        private string _dataName;//データ名
       private int _dataStatus;//データ状態（０：オン、１：オフ）

        public string DataID
       {
           get => _dataId;
           set => SetProperty(ref _dataId, value);
       }

       public string DataName
       {
           get => _dataName;
           set => SetProperty(ref _dataName, value);
       }

       public int DataStatus
       {
           get => _dataStatus;
           set => SetProperty(ref _dataStatus, value);
        }
    }

}
