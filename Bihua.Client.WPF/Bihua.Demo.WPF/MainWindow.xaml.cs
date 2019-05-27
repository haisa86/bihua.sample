using Bihua.Demo.WPF.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace Bihua.Demo.WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //需要先启动服务端项目：Bihua.WebApi
        string _apiUrl = "https://localhost:44319/api/bihuas/chinese";

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MainWindow), null);

        public ChineseChar ChineseChar
        {
            get { return (ChineseChar)GetValue(ChineseCharProperty); }
            private set { SetValue(ChineseCharProperty, value); }
        }

        public static readonly DependencyProperty ChineseCharProperty =
            DependencyProperty.Register("ChineseChar", typeof(ChineseChar), typeof(MainWindow), null);


        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await GetChineseChar();
        }

        private async Task GetChineseChar()
        {
            if (string.IsNullOrWhiteSpace(Text)) { ChineseChar = null; return; }

            using (var httpClient = new HttpClient())
            {
                using (var response = (await httpClient.GetAsync($"{_apiUrl }/{Text}")).EnsureSuccessStatusCode())
                {
                    ChineseChar = Newtonsoft.Json.JsonConvert.DeserializeObject<ChineseChar>(await response.Content.ReadAsStringAsync());
                }
            }
        }
    }
}
