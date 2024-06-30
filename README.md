# C# Web Auth Handler

Bu C# projesi, bir WEB API'ye bağlanarak kullanıcı kimlik doğrulaması ve yetkilendirme yapabilen bir örneği içerir.

## Auth Kullanım

Projenin `sign_in` metodu içinde, `MISYAPI` sınıfı kullanılarak bir API token'ı ayarlanır ve kullanıcı girişi sağlanır.

```csharp
public sign_in()
{
    InitializeComponent();

    // MisyApi token'ı ayarla
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
```

### Dashboard Formu

Dashboard formunda, kullanıcı bilgilerini çekme ve kullanıcı kaydı yapma işlemleri yapılmaktadır.

#### Kullanıcı Bilgilerini Çekme

Kullanıcı ID'sini girdikten sonra "Fetch Data" butonuna tıklanarak API üzerinden kullanıcı bilgileri çekilir.

```csharp
private void button1_Click(object sender, EventArgs e)
{
    if (!String.IsNullOrEmpty(textBox1.Text))
    {
        // Kullanıcı ID'sini al
        uid = Convert.ToInt32(textBox1.Text);

        // MISYAPI sınıfı ile API konfigürasyonu yap
        misyApi.SetMisyApiUid(uid);
        misyApi.SetMisyApiType("fetch");

        switch (misyApi.GetMisyApiType())
        {
            case "fetch":
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(misyApi.GetMisyApiSelfHostname());

                // JSON formatında istek gönder
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                string parameters = misyApi.GetMisyQuestParameters();

                // Veri isteği yap
                HttpResponseMessage response = client.GetAsync(parameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
                if (response.IsSuccessStatusCode)
                {
                    // Yanıtı işle
                    var dataObjects = response.Content.ReadAsStringAsync().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll

                    if (dataObjects != "user not found")
                    {
                        // Kullanıcı bilgilerini işle
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

                // HttpClient kapat
                client.Dispose();
                break;
            default:
                MessageBox.Show("method not correct!");
                break;
        }
    }
    else
    {
        MessageBox.Show("user id not accept!");
    }
}
