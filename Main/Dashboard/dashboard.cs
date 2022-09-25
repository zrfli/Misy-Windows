using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Collections;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Misy_API.Classes.System;

namespace Misy_API
{
    public partial class dashboard : Form
    {
        public dashboard()
        {
            InitializeComponent();

            misyApi.SetMisyApiToken("ee5fcf995151d3f39f7ca5165e0639e5199831fe058797041a3d23c1ab1c95bc");
        }

        Classes.System.MISYAPI misyApi = new Classes.System.MISYAPI();
        
        private int uid = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(textBox1.Text))
            {
                uid = Convert.ToInt32(textBox1.Text);

                misyApi.SetMisyApiUid(uid);
                misyApi.SetMisyApiType("fetch");

                switch (misyApi.GetMisyApiType())
                {
                    case "fetch":
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(misyApi.GetMisyApiSelfHostname());

                        // Add an Accept header for JSON format.
                        client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                        string parameters = misyApi.GetMisyQuestParameters();

                        // List data response.
                        HttpResponseMessage response = client.GetAsync(parameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
                        if (response.IsSuccessStatusCode)
                        {
                            // Parse the response body.
                            var dataObjects = response.Content.ReadAsStringAsync().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll

                            if (dataObjects != "user not found")
                            {
                                //User
                                JObject jObject = JObject.Parse(dataObjects);
                                JObject jobj = jObject;

                                string avatar = (string)jobj["avatar"];
                                string fullName = (string)jobj["first_name"] + " " + (string)jobj["last_name"];

                                label1.Text = fullName;
                                pictureBox1.ImageLocation = misyApi.GetMisyApiBaseHostname() + avatar;
                            }
                            else { MessageBox.Show(dataObjects); }

                        }
                        else
                        {
                            MessageBox.Show(response.Headers.ToString());
                        }

                        //Make any other calls using HttpClient here.
                        //Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
                        client.Dispose();
                        break;
                    case "udate":

                        break;

                    case "insert":

                        break;

                    case "delete":

                        break;
                    default:
                        MessageBox.Show("method not correct!");
                        break;
                }
            } else
            {
                MessageBox.Show("user id not accept!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox2.Text) && !String.IsNullOrEmpty(textBox3.Text))
            {
                misyApi.MisyApiRegister(textBox2.Text, textBox3.Text);

                if (misyApi.GetMisyApiAuthStatusCode().ToString() == "200")
                {
                    MessageBox.Show(misyApi.GetMisyApiAuthStatusCode().ToString());
                    MessageBox.Show(misyApi.GetMisyAuthMessage());
                }
                else { MessageBox.Show(misyApi.GetMisyApiAuthStatusCode().ToString()); }
            }
            else { MessageBox.Show(Convert.ToString(misyApi.GetMisyApiAuthStatusCode)); }
        }
    }
}