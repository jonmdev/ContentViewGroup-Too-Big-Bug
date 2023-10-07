using System.Diagnostics;

namespace ContentViewGroup_Too_Big_Bug {
    public partial class App : Application {

        public event Action screenSizeChanged = null;
        public double screenWidth = 0;
        public double screenHeight = 0;
        ContentPage mainPage;
        public App() {

            InitializeComponent();

            //=========
            //LAYOUT
            //=========
            mainPage = new();
            mainPage.Background = Colors.GreenYellow;
            MainPage = mainPage;
            mainPage.SizeChanged += delegate {
                invokeScreenSizeChangeEvent();
            };

            VerticalStackLayout vert = new();
            mainPage.Content = vert;
            vert.HorizontalOptions = LayoutOptions.Center;

            Border border1 = new Border();
            border1.BackgroundColor = Colors.Azure;
            border1.Shadow = new Shadow() { Offset = new Point(0, 30), Radius = 40, Brush = Colors.Red }; //causes too large to fit in cache when border 1 is larger than the screen
            vert.Children.Add(border1);

            //==================
            //RESIZE FUNCTION
            //==================
            screenSizeChanged += delegate {

                vert.HeightRequest = screenHeight;
                vert.WidthRequest = screenWidth * 1;

                border1.HeightRequest = screenHeight;
                border1.WidthRequest = screenWidth * 1.1; //causes too large to fit in cacche error when >1x width with a shadow enabled

            };

            //===================
            //ANIMATION TIMER
            //===================
            var timer = Application.Current.Dispatcher.CreateTimer();
            DateTime dateTime = DateTime.Now;
            double deltaTime = 0;
            double time = 0;
            timer.Tick += delegate {
                deltaTime = (DateTime.Now - dateTime).TotalSeconds;
                time += deltaTime;
                //border1.TranslationX = Math.Sin(time) * 200;
                border1.TranslationY = Math.Sin(time * 1.2) * screenHeight * 0.1;
                dateTime = DateTime.Now;
            };
            timer.Start();

        }
        private void invokeScreenSizeChangeEvent() {
            if (mainPage.Width > 0 && mainPage.Height > 0) {
                screenWidth = mainPage.Width;
                screenHeight = mainPage.Height;
                screenSizeChanged?.Invoke();
                //Debug.WriteLine("main page size changed | width: " + screenWidth + " height: " + screenHeight);
            }
        }
    }
}