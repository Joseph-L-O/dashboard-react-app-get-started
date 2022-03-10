using DevExpress.AspNetCore;
using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.Json;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Native;
using DevExpress.DataAccess.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Web.UI;
using System.Collections.Generic; 

using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;

using System.Text.Json.Serialization;

namespace AspNetCoreDashboardBackend {
    public class Startup {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment) {
            Configuration = configuration;
            FileProvider = hostingEnvironment.ContentRootFileProvider;
        }

        public IConfiguration Configuration { get; }
        public IFileProvider FileProvider { get; }

        public void ConfigureServices(IServiceCollection services) {
            services
                // Configures CORS policies.                
                .AddCors(options => {
                    options.AddPolicy("CorsPolicy", builder => {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyMethod();
                        builder.WithHeaders("Content-Type");
                    });
                })
                // Adds the DevExpress middleware.
                .AddDevExpressControls()
                // Adds controllers.
                .AddControllers();
            // Configures the dashboard backend.
            services.AddScoped<DashboardConfigurator>((IServiceProvider serviceProvider) => {
                DashboardConfigurator configurator = new DashboardConfigurator();                
                configurator.SetDashboardStorage(new DashboardFileStorage(FileProvider.GetFileInfo("App_Data/Dashboards").PhysicalPath));
                configurator.SetDataSourceStorage(CreateDataSourceStorage());
                configurator.ConfigureDataConnection += Configurator_ConfigureDataConnection;
                configurator.SetConnectionStringsProvider(new MyDataSourceWizardConnectionStringsProvider());
                return configurator;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            // Registers the DevExpress middleware.            
            app.UseDevExpressControls();
            // Registers routing.
            app.UseRouting();
            // Registers CORS policies.
            app.UseCors("CorsPolicy");
            app.UseEndpoints(endpoints => {
                // Maps the dashboard route.
                EndpointRouteBuilderExtension.MapDashboardRoute(endpoints, "api/dashboard", "DefaultDashboard");
                // Requires CORS policies.
                endpoints.MapControllers().RequireCors("CorsPolicy");
            });
        }

        public DataSourceInMemoryStorage CreateDataSourceStorage() {
            DataSourceInMemoryStorage dataSourceStorage = new DataSourceInMemoryStorage();
            DashboardJsonDataSource jsonDataSourceSupport = new DashboardJsonDataSource("Support");
            jsonDataSourceSupport.ConnectionName = "jsonSupport";
            jsonDataSourceSupport.RootElement = "Employee";
            dataSourceStorage.RegisterDataSource("jsonDataSourceSupport", jsonDataSourceSupport.SaveToXml());
            DashboardJsonDataSource jsonDataSourceCategories = new DashboardJsonDataSource("Categories");
            jsonDataSourceCategories.ConnectionName = "jsonCategories";
            jsonDataSourceCategories.RootElement = "Customers";
            dataSourceStorage.RegisterDataSource("jsonDataSourceCategories", jsonDataSourceCategories.SaveToXml());
            return dataSourceStorage;
        }

        private void Configurator_ConfigureDataConnection(object sender, ConfigureDataConnectionWebEventArgs e) {
            if (e.ConnectionName == "jsonSupport") {
                Uri fileUri = new Uri(FileProvider.GetFileInfo("App_Data/Support.json").PhysicalPath, UriKind.RelativeOrAbsolute);
                JsonSourceConnectionParameters jsonParams = new JsonSourceConnectionParameters();
                jsonParams.JsonSource = new UriJsonSource(fileUri);
                e.ConnectionParameters = jsonParams;
            }
            if (e.ConnectionName == "jsonCategories") {
                Uri fileUri = new Uri(FileProvider.GetFileInfo("App_Data/Categories.json").PhysicalPath, UriKind.RelativeOrAbsolute);
                JsonSourceConnectionParameters jsonParams = new JsonSourceConnectionParameters();
                jsonParams.JsonSource = new UriJsonSource(fileUri);
                e.ConnectionParameters = jsonParams;
            }
        }
    }
    public class Item
    {
        public object ConnectionStrings;
        public object Logging;
    }
    
    public class MyDataSourceWizardConnectionStringsProvider : IDataSourceWizardConnectionStringsProvider {
        public IFileProvider FileProvider { get; }

        
        public Dictionary<string, string> GetConnectionDescriptions() {
            dynamic items = null;
            using (StreamReader r = new StreamReader("appsettings.Development.json"))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<dynamic>(json);
            }
            Dictionary<string, string> connections = new Dictionary<string, string>();
            var requisicaoWeb = WebRequest.CreateHttp(items["ConnectionStrings"]["JSON Connection URL"].ToString());
            requisicaoWeb.Method = "GET";
            requisicaoWeb.UserAgent = "RequisicaoWebDemo";
            var resposta = requisicaoWeb.GetResponse();
            var streamDados = resposta.GetResponseStream();
            StreamReader reader = new StreamReader(streamDados);
            object objResponse  = reader.ReadToEnd();
            var data = JsonConvert.DeserializeObject<Component[]>(objResponse.ToString());
            // dynamic[] yourArray = array.Cast();
            foreach (var i in data)
            {
                connections.Add(i.uriconnection, i.uriconnection);
            }
            //   Customize the loaded connections list.
            return connections;
        }

        public DataConnectionParametersBase GetDataConnectionParameters(string name) {
            // Return custom connection parameters for the custom connection.
            dynamic items = null;
            using (StreamReader r = new StreamReader("appsettings.json"))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<dynamic>(json);
            }
            var requisicaoWeb = WebRequest.CreateHttp("http://"+items["ConnectionStrings"]["JSON Connection URL"].ToString());
            requisicaoWeb.Method = "GET";
            requisicaoWeb.UserAgent = "RequisicaoWebDemo";
            var resposta = requisicaoWeb.GetResponse();
            var streamDados = resposta.GetResponseStream();
            StreamReader reader = new StreamReader(streamDados);
            object objResponse  = reader.ReadToEnd();
            var data = JsonConvert.DeserializeObject<Component[]>(objResponse.ToString());
            var finded = Array.Find(data, p => p.uriconnection == name);
            System.Console.WriteLine();
            if(finded.uriconnection == name){
                    return new JsonSourceConnectionParameters() {
                        JsonSource = new UriJsonSource(new Uri(Array.Find(data,p => p.uriconnection == name).uri))
                    };
                }
                
            throw new System.Exception("The connection string is undefined.");
            }
        }
    public class Component
    {
        public string uriconnection { get; set; }
        public string uri { get; set; }
    }
    
}