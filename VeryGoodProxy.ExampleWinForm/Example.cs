using System;
using System.Net.Http;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace VeryGoodProxy.ExampleWinForm
{
    public partial class Example : Form
    {
        public Example()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var client = new HttpClient();
            var response = client.PostAsync(@"https://echo.apps.verygood.systems/post" , new StringContent(textBoxRequest.Text));
            response.Wait();
            var content = response.Result.Content.ReadAsStringAsync();
            content.Wait();
            textBoxResposne.Text = JObject.Parse(content.Result).ToString(Newtonsoft.Json.Formatting.Indented);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
