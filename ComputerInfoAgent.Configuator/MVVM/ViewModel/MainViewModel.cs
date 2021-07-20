using ComputerInfoAgent.Configuator.Core;

namespace ComputerInfoAgent.Configuator.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {

        public RelyCommand HomeViewCommand { get; set; }
        public RelyCommand TestViewCommand { get; set; }

        public TestViewModel TestVM { get; set; }
        public HomeViewModel HomeVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }


        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            TestVM = new TestViewModel();
            CurrentView = HomeVM;

            HomeViewCommand = new RelyCommand(o =>
            {
                CurrentView = HomeVM;
            });

            TestViewCommand = new RelyCommand(o =>
            {
                CurrentView = TestVM;
            });
        }
    }
}
