using MaoMaoYuFlickKeyboard.Src.Model;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MaoMaoYuFlickKeyboard.Src.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        public ObservableCollection<FlickKeyPartModel> FlickKeyParts 
        { 
            get => _flickKeyParts;
            set => SetProperty(ref _flickKeyParts, value);
        }
        private ObservableCollection<FlickKeyPartModel> _flickKeyParts;

        public MainPageViewModel()
        {
            Init();
        }

        private void Init()
        {
            FlickKeyParts =
            [
                // 第一行
                new FlickKeyPartModel { BottomButtonInputText = "お", CenterButtonInputText = "あ", CenterButtonUIContent = "あ", LeftButtonInputText = "い", RightButtonInputText = "え", TopButtonInputText = "う", GridRow = 1, GridColumn = 1 },
                new FlickKeyPartModel { BottomButtonInputText = "こ", CenterButtonInputText = "か", CenterButtonUIContent = "か", LeftButtonInputText = "き", RightButtonInputText = "け", TopButtonInputText = "く", GridRow = 1, GridColumn = 2 },
                new FlickKeyPartModel { BottomButtonInputText = "そ", CenterButtonInputText = "さ", CenterButtonUIContent = "さ", LeftButtonInputText = "し", RightButtonInputText = "せ", TopButtonInputText = "す", GridRow = 1, GridColumn = 3 },
                
                // 第二行
                new FlickKeyPartModel { BottomButtonInputText = "と", CenterButtonInputText = "た", CenterButtonUIContent = "た", LeftButtonInputText = "ち", RightButtonInputText = "て", TopButtonInputText = "つ", GridRow = 2, GridColumn = 1 },
                new FlickKeyPartModel { BottomButtonInputText = "の", CenterButtonInputText = "な", CenterButtonUIContent = "な", LeftButtonInputText = "に", RightButtonInputText = "ね", TopButtonInputText = "ぬ", GridRow = 2, GridColumn = 2 },
                new FlickKeyPartModel { BottomButtonInputText = "ほ", CenterButtonInputText = "は", CenterButtonUIContent = "は", LeftButtonInputText = "ひ", RightButtonInputText = "へ", TopButtonInputText = "ふ", GridRow = 2, GridColumn = 3 },
                
                // 第三行
                new FlickKeyPartModel { BottomButtonInputText = "も", CenterButtonInputText = "ま", CenterButtonUIContent = "ま", LeftButtonInputText = "み", RightButtonInputText = "め", TopButtonInputText = "む", GridRow = 3, GridColumn = 1 },
                new FlickKeyPartModel { BottomButtonInputText = "よ", CenterButtonInputText = "や", CenterButtonUIContent = "や", LeftButtonInputText = "", RightButtonInputText = "", TopButtonInputText = "ゆ", GridRow = 3, GridColumn = 2 },
                new FlickKeyPartModel { BottomButtonInputText = "ろ", CenterButtonInputText = "ら", CenterButtonUIContent = "ら", LeftButtonInputText = "り", RightButtonInputText = "れ", TopButtonInputText = "る", GridRow = 3, GridColumn = 3 },
                
                // 第四行
                new FlickKeyPartModel { BottomButtonInputText = "を", CenterButtonInputText = "わ", CenterButtonUIContent = "わ", LeftButtonInputText = "", RightButtonInputText = "", TopButtonInputText = "", GridRow = 4, GridColumn = 1 },
                new FlickKeyPartModel { BottomButtonInputText = "を", CenterButtonInputText = "わ", CenterButtonUIContent = "わ", LeftButtonInputText = "", RightButtonInputText = "", TopButtonInputText = "", GridRow = 4, GridColumn = 2 },
                new FlickKeyPartModel { BottomButtonInputText = "ん", CenterButtonInputText = "", CenterButtonUIContent = "ん", LeftButtonInputText = "", RightButtonInputText = "", TopButtonInputText = "", GridRow = 4, GridColumn = 3 }
            ];
        }
    }
}
