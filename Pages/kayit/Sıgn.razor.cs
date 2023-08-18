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

namespace ex9.Pages.kayit
{
    public class Sıgnbase
    {
        public string? Id { get; set; }
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        public string? KullaniciAdi { get; set; }
        public string? Password { get; set; }


    }
    public class SıgnBase : ComponentBase
    {
        static string connectionString = ("Server=.;Database=PdfApp;Encrypt=False;Integrated Security=SSPI;");

        static SqlConnection conn = new SqlConnection(connectionString);
        static string sql = "select Id,Ad,Soyad,KullaniciAdi,Password,Role from Login";
        static SqlDataAdapter daps = new SqlDataAdapter(sql, conn);
        SqlCommandBuilder cb = new SqlCommandBuilder(daps);
        DataSet dsps = new DataSet();
        public List<Sıgnbase> people = new List<Sıgnbase>();
        public string? error { get; set; }
        public string? inputSoyad { get; set; }
        public string? inputAd { get; set; }
        public string? inputKullaniciAdi { get; set; }
        public string? inputPassword { get; set; }



        public async Task InsertAccount()
        {

            if (inputAd != null && inputSoyad != null && inputKullaniciAdi != null && inputPassword != null)
            {

                dsps.Tables[0].Rows.Add(null, inputAd, inputSoyad, inputKullaniciAdi, inputPassword, "user");
                daps.Update(dsps, "people");
                dsps.Tables["people"].Clear();
                //cb.Dispose();
                inputAd = "";
                inputSoyad = "";
                inputKullaniciAdi = "";
                inputPassword = "";
                
            }
            else
            {

            }
            await OnInitializedAsync();

        }

        protected override async Task OnInitializedAsync()
        {
            people.Clear();
            await selectproc();
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
                        Sıgnbase pm = new Sıgnbase();
                        pm.Id = dsps.Tables[0].Rows[i]["Id"].ToString();
                        pm.Ad = dsps.Tables[0].Rows[i]["Ad"].ToString();
                        pm.Soyad = dsps.Tables[0].Rows[i]["Soyad"].ToString();
                        pm.KullaniciAdi = dsps.Tables[0].Rows[i]["KullaniciAdi"].ToString();
                        pm.Password = dsps.Tables[0].Rows[i]["Password"].ToString();

                        people.Add(pm);

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