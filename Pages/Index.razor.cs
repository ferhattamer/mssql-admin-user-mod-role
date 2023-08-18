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
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json;
using ex9.Data;

namespace ex9.Pages
{
    public class User
    {
        public int Id { get; set; } = 1;
        public string? KullaniciAdi { get; set; }
        public string? Password { get; set; }
        public string? Role;

    }
    public class ControlBase : ComponentBase
    {

        static string connectionString = ("Server=.;Database=PdfApp;Encrypt=False;Integrated Security=SSPI;");

        static SqlConnection conn = new SqlConnection(connectionString);
        static string sql = "select Id,Ad,Soyad,KullaniciAdi,Password,Role from Login";
        static SqlDataAdapter daps = new SqlDataAdapter(sql, conn);
        SqlCommandBuilder cb = new SqlCommandBuilder(daps);
        DataSet dsps = new DataSet();
        public List<User> kullanici = new List<User>();
        public string? inputId { get; set; }
        public string? error { get; set; }
        //[Required(ErrorMessage = "Please enter your user Id")]
        //[StringLength(16,ErrorMessage ="The user ıd to long")]
        public string? inputKullaniciAdi { get; set; }
        public string? inputPassword { get; set; }
        [Inject]
        protected NavigationManager? Navigation { get; set; }
        [Inject]
        public ProtectedLocalStorage? localstr { get; set; }
        public string? Text { get; set; }

        public async Task SearchUser()
        {
            DataRowCollection itemColumns = dsps.Tables[0].Rows;
            for (int i = 0; i < dsps.Tables[0].Rows.Count; i++)
            {
                if (dsps.Tables[0].Rows[i]["KullaniciAdi"].ToString() == inputKullaniciAdi && dsps.Tables[0].Rows[i]["Password"].ToString() == inputPassword)
                {
                    UserModel user=new UserModel(){
                        UserName=dsps.Tables[0].Rows[i]["KullaniciAdi"].ToString(),Role=dsps.Tables[0].Rows[i]["Role"].ToString()};
                    /*Text=inputKullaniciAdi;
                    NavigationManeger.NavigateTo("/giris"+inputKullaniciAdi,inputPassword);*/
                    //locale veri at
                    await localstr.SetAsync("myUser", JsonSerializer.Serialize(user));//kullanci adi changed role

                    Navigation.NavigateTo("/Giris");

                }
                else
                {

                }
            }
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
                        User xm = new User();
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