using WTLib.Mvvm;

namespace ThemeWindow.Models
{
    public class Account : ObservableObject
    {
        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        private string _id = null;

        public string Code
        {
            get => _code;
            set => SetProperty(ref _code, value);
        }
        private string _code = null;
    }
}
