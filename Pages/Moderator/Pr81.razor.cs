using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Net.Http;
using Microsoft.Data.SqlClient;
using System.Data;
using ex9.Data;
using System.Text.Json;

namespace ex9.Pages.Moderator
{
    public class YorumModel
    {
        public string? Id1 { get; set; }
        public string? yorumcu { get; set; }
        public string? kAdi { get; set; }
        public string? kYorum { get; set; }


    }
    public class PersonModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Url { get; set; }
        public string? Yazar { get; set; }


    }
    public class Pr81Base : ComponentBase
    {
         static string connectionString1 = ("Server=.;Database=master;Encrypt=False;Integrated Security=SSPI;");
         
        static SqlConnection conn1 = new SqlConnection(connectionString1);
        static string sql1 = "select Id,yorumcu,kAdi,kYorum from Yorum";
        static SqlDataAdapter daps1 = new SqlDataAdapter(sql1, conn1);
          SqlCommandBuilder cb1 = new SqlCommandBuilder(daps1);
        DataSet dsps1 = new DataSet();
        public List<YorumModel> yorum = new List<YorumModel>();
        public string? inputId1 { get; set; }
        public string? inputkAdi { get; set; }
        public string? inputkYorum { get; set; }
        private string? kullanici = "";
        [Inject]
        public ProtectedLocalStorage? localstr1 { get; set; }
        public string? Kullanici
        {
            get
            {
                KullaniciAdi();
                return kullanici;
            }
            set
            {
                kullanici = value;
            }
        }
        public string? mystr1 { get; set; }
        UserModel user = new UserModel();

        private async void KullaniciAdi()
        {
            var result = await localstr.GetAsync<string>("myUser");
            mystr = result.Success ? result.Value : "";
            UserModel user = new UserModel();
            user = JsonSerializer.Deserialize<UserModel>(mystr);
            kullanici=user.UserName;

        }
        public string Text { get; set; } = "Linke Tıklayarak Kitabı İndirebilirsin.";
        static string connectionString = ("Server=.;Database=PdfApp;Encrypt=False;Integrated Security=SSPI;");
        static SqlConnection conn = new SqlConnection(connectionString);
          SqlCommandBuilder cb = new SqlCommandBuilder(daps);
        static string sql = "select Id,Name,Url,Yazar from Pdf";
        static SqlDataAdapter daps = new SqlDataAdapter(sql, conn);
        DataSet dsps = new DataSet();
        public List<PersonModel> people = new List<PersonModel>();
        public string? error { get; set; }
        public string? error2 { get; set; }
        public string? inputId { get; set; }
        public string? inputName { get; set; }
        public string? inputUrl { get; set; }
        public string? inputYazar { get; set; }
        public string? inputYorumsahibi{get;set;}

        private string? kontrol = "";
        [Inject]
        public ProtectedLocalStorage? localstr { get; set; }
        public string? Kontrol
        {
            get
            {
                AdminModerator();
                return kontrol;
            }
            set
            {
                kontrol = value;
            }
        }
        public string? mystr { get; set; }
        [Inject]
        protected NavigationManager? Navigation { get; set; }

        private async void AdminModerator()
        {
            var result = await localstr.GetAsync<string>("myUser");
            mystr = result.Success ? result.Value : "";
            UserModel user = new UserModel();
            user = JsonSerializer.Deserialize<UserModel>(mystr);
            if (user.Role != "admin")
                if (user.Role != "moderator")
                    Navigation.NavigateTo("/");
        }

        public async Task InsertData()
        {
            dsps.Tables[0].Rows.Add(null, inputName, inputUrl, inputYazar);
            daps.Update(dsps, "people");
            dsps.Tables["people"].Clear();
            //cb.Dispose();
            inputName = "";
            inputUrl = "";
            inputYazar = "";
            await OnInitializedAsync();
        }
        public async Task GetData()
        {
            DataRowCollection itemColumns = dsps.Tables[0].Rows;
            for (int i = 0; i < dsps.Tables[0].Rows.Count; i++)
            {
                if (dsps.Tables[0].Rows[i]["Id"].ToString() == inputId)
                {
                    inputName = dsps.Tables[0].Rows[i]["Name"].ToString();
                    inputUrl = dsps.Tables[0].Rows[i]["Url"].ToString();
                    inputYazar = dsps.Tables[0].Rows[i]["Yazar"].ToString();
                }
            }
            dsps.Tables[0].Clear();
            await OnInitializedAsync();
        }
        public async Task GetKitap()
        {
            DataRowCollection itemColumns = dsps.Tables[0].Rows;
            for (int i = 0; i < dsps.Tables[0].Rows.Count; i++)
            {
                if (dsps.Tables[0].Rows[i]["Id"].ToString() == inputId1)
                {
                    inputkAdi = dsps.Tables[0].Rows[i]["Name"].ToString();
                }
            }
            dsps.Tables[0].Clear();
            dsps1.Tables[0].Clear();
            await OnInitializedAsync();
        }
        public async Task InsertYorum()
        {
            kullanici=user.UserName;

            dsps1.Tables[0].Rows.Add(null,inputYorumsahibi, inputkAdi, inputkYorum);
            daps1.Update(dsps1, "yorum");
            dsps1.Tables["yorum"].Clear();
            //cb.Dispose();
            inputkAdi = "";
            inputkYorum = "";
            inputId1 = "";
            await OnInitializedAsync1();
        }

        public async Task UpdateData()
        {
            DataRowCollection itemColumns = dsps.Tables[0].Rows;
            for (int i = 0; i < dsps.Tables[0].Rows.Count; i++)
            {
                if (dsps.Tables[0].Rows[i]["Id"].ToString() == inputId)
                {
                    dsps.Tables[0].Rows[i]["Name"] = inputName;
                    dsps.Tables[0].Rows[i]["Url"] = inputUrl;
                    dsps.Tables[0].Rows[i]["Yazar"] = inputYazar;
                }
            }
            daps.Update(dsps, "people");
            // cb.Dispose();
            dsps.Tables[0].Clear();

            inputId = "";
            inputName = "";
            inputUrl = "";
            inputYazar = "";
            await OnInitializedAsync();
        }
        public async Task DeleteData()
        {
            foreach (DataRow row in dsps.Tables[0].Rows)
            {
                if (row["Id"].ToString() == inputId)

                    row.Delete();
            }
            daps.Update(dsps, "people");
            //  cb.Dispose();
            dsps.Tables["people"].Clear();

            inputId = "";
            inputName = "";
            await OnInitializedAsync();

        }
        protected override async Task OnInitializedAsync()
        {
            inputYorumsahibi=kullanici;
            yorum.Clear();
            people.Clear();
            await selectproc();
            await selectproc1();
        }
         public  async Task OnInitializedAsync1()
        {
            inputYorumsahibi=kullanici;
            yorum.Clear();
            await selectproc1();
        }
        public Task selectproc()
        {
            return Task.Run(() =>
            {
                try
                {
                    daps.Fill(dsps, "people");
                    error += dsps.Tables[0].Rows.Count.ToString();
                    for (int i = 0; i < dsps.Tables[0].Rows.Count; i++)
                    {
                        PersonModel pm = new PersonModel();
                        pm.Id= dsps.Tables[0].Rows[i]["Id"].ToString();
                        pm.Name = dsps.Tables[0].Rows[i]["Name"].ToString();
                        pm.Url = dsps.Tables[0].Rows[i]["Url"].ToString();
                        pm.Yazar = dsps.Tables[0].Rows[i]["Yazar"].ToString();

                        people.Add(pm);

                    }
                }
                catch (Exception ex)
                {
                    error = ex.ToString();
                }
            });
        }
        public Task selectproc1()
        {
            return Task.Run(() =>
            {
                try
                {
                    daps1.Fill(dsps1, "yorum");
                    error2 += dsps1.Tables[0].Rows.Count.ToString();

                    for (int i = 0; i < dsps1.Tables[0].Rows.Count; i++)
                    {
                        YorumModel xm = new YorumModel();
                         xm.Id1 = dsps1.Tables[0].Rows[i]["Id"].ToString();
                        xm.yorumcu = dsps1.Tables[0].Rows[i]["yorumcu"].ToString();
                        xm.kAdi = dsps1.Tables[0].Rows[i]["kAdi"].ToString();
                        xm.kYorum = dsps1.Tables[0].Rows[i]["kYorum"].ToString();

                        yorum.Add(xm);

                    }
                }
                catch (Exception ex)
                {
                    error2 = ex.ToString();
                }
            });
        }
    }
}