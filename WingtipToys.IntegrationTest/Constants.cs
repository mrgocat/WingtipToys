using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WingtipToys.IntegrationTest
{
    public static class Constants
    {
        public static string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\WORK\ORDERDYNAMICS\CODINGEXERCISE\DATA\WINGTIPTOYS.MDF;Integrated Security=True";

        public static void LoadConnectionString()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            ConnectionString = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }
    }
}
