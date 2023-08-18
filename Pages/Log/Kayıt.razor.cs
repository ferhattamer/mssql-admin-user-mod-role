using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.JSInterop;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Net.Http;
using Microsoft.Data.SqlClient;
using ex9.Data;


using System.Data;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Text.Json;

namespace ex9.Pages.Log
{
    public class LogUser
    {
        public string? Id { get; set; }
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        public string? KullaniciAdi { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }


    }
    public class KayıtBase : ComponentBase
    {
        static string connectionString = ("Server=.;Database=PdfApp;Encrypt=False;Integrated Security=SSPI;");

        static SqlConnection conn = new SqlConnection(connectionString);
        static string sql = "select Id,Ad,Soyad,KullaniciAdi,Password,Role from Login";
        static SqlDataAdapter daps = new SqlDataAdapter(sql, conn);
        SqlCommandBuilder cb = new SqlCommandBuilder(daps);
        DataSet dsps = new DataSet();
        public List<LogUser> kullanici = new List<LogUser>();
        public string? error { get; set; }
        public string? inputId { get; set; }
        public string? inputSoyad { get; set; }
        public string? inputRole { get; set; }
        public string? inputAd { get; set; }
        public string? inputKullaniciadi { get; set; }
        public string? inputPassword { get; set; }
        private string? kontrol = "";
        [Inject]
        public ProtectedLocalStorage? localstr { get; set; }
        public string? Kontrol
        {
            get
            {
                maMen();
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
        private async void maMen()
        {
            var result = await localstr.GetAsync<string>("myUser");
            mystr = result.Success ? result.Value : "";
            UserModel user=new UserModel();
            user=JsonSerializer.Deserialize<UserModel>(mystr);
            if (user.Role != "admin" )
                    Navigation.NavigateTo("/");
        }

        public async Task InsertUser()
        {
            dsps.Tables[0].Rows.Add(null, inputAd, inputSoyad, inputKullaniciadi, inputPassword, inputRole);
            daps.Update(dsps, "kullanici");
            dsps.Tables["kullanici"].Clear();
            //cb.Dispose();
            inputAd = "";
            inputSoyad = "";
            inputKullaniciadi = "";
            inputPassword = "";
            inputRole = "";
            await OnInitializedAsync();
        }
        public async Task GetUser()
        {
            
            for (int i = 0; i < dsps.Tables[0].Rows.Count; i++)
            {
                if (dsps.Tables[0].Rows[i]["Id"].ToString() == inputId)
                {
                    inputAd = dsps.Tables[0].Rows[i]["Ad"].ToString();
                    inputSoyad = dsps.Tables[0].Rows[i]["Soyad"].ToString();
                    inputKullaniciadi = dsps.Tables[0].Rows[i]["KullaniciAdi"].ToString();
                    inputPassword = dsps.Tables[0].Rows[i]["Password"].ToString();
                    inputRole = dsps.Tables[0].Rows[i]["Role"].ToString();

                }
            }
             dsps.Tables[0].Clear();
            await OnInitializedAsync();
        }
        public async Task UpdateUser()
        {
            DataRowCollection itemColumns = dsps.Tables[0].Rows;
            for (int i = 0; i < dsps.Tables[0].Rows.Count; i++)
            {
                if (dsps.Tables[0].Rows[i]["Id"].ToString() == inputId)
                {
                    dsps.Tables[0].Rows[i]["Ad"] = inputAd;
                    dsps.Tables[0].Rows[i]["Soyad"] = inputSoyad;
                    dsps.Tables[0].Rows[i]["KullaniciAdi"] = inputKullaniciadi;
                    dsps.Tables[0].Rows[i]["Password"] = inputPassword;
                    dsps.Tables[0].Rows[i]["Role"] = inputRole;

                }
            }
            daps.Update(dsps, "kullanici");
            // cb.Dispose();
            dsps.Tables[0].Clear();

            inputId = "";
            inputAd = "";
            inputSoyad = "";
            inputKullaniciadi = "";
            inputPassword = "";
            inputRole = "";
            await OnInitializedAsync();
        }
        public async Task DeleteUser()
        {
            foreach (DataRow row in dsps.Tables[0].Rows)
            {
                if (row["Id"].ToString() == inputId)

                    row.Delete();
            }
            daps.Update(dsps, "kullanici");
            //  cb.Dispose();
            dsps.Tables["kullanici"].Clear();

            inputId = "";
            inputAd = "";
            inputSoyad = "";
            inputKullaniciadi = "";
            inputPassword = "";
            inputRole = "";
            await OnInitializedAsync();

        }
        protected override async Task OnInitializedAsync()
        {
            kullanici.Clear();
            await selectproc();
        }
        public Task selectproc()
        {
            return Task.Run(() =>
            {
                try
                {
                    daps.Fill(dsps, "kullanici");
                    error += dsps.Tables[0].Rows.Count.ToString();
                    for (int i = 0; i < dsps.Tables[0].Rows.Count; i++)
                    {
                        LogUser xm = new LogUser();
                        xm.Id = dsps.Tables[0].Rows[i]["Id"].ToString();
                        xm.Ad = dsps.Tables[0].Rows[i]["Ad"].ToString();
                        xm.Soyad = dsps.Tables[0].Rows[i]["Soyad"].ToString();
                        xm.KullaniciAdi = dsps.Tables[0].Rows[i]["KullaniciAdi"].ToString();
                        xm.Password = dsps.Tables[0].Rows[i]["Password"].ToString();
                        xm.Role = dsps.Tables[0].Rows[i]["Role"].ToString();


                        kullanici.Add(xm);

                    }
                }
                catch (Exception ex)
                {
                    error = ex.ToString();
                }
            });
        }

    }
}