using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Misy_API.Main.Sign_in
{
    public partial class sign_in : Form
    {
        public sign_in()
        {
            InitializeComponent();

            misyApi.SetMisyApiToken("ee5fcf995151d3f39f7ca5165e0639e5199831fe058797041a3d23c1ab1c95bc");
        }

        Classes.System.MISYAPI misyApi = new Classes.System.MISYAPI();
        private void button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text) && !String.IsNullOrEmpty(textBox2.Text))
            {
                misyApi.MisyApiLogin(textBox1.Text, textBox2.Text);

                if (misyApi.GetMisyApiAuthStatusCode().ToString() == "200")
                {
                    dashboard dashboard = new dashboard();
                    dashboard.Show();
                    this.Hide();
                } else { MessageBox.Show(misyApi.GetMisyApiAuthStatusCode().ToString());  }
            } else { MessageBox.Show(Convert.ToString(misyApi.GetMisyApiAuthStatusCode));  }
        }
    }
}
