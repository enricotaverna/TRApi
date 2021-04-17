using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartupAttribute(typeof(TRApi.Startup))]

namespace TRApi 
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app) 
        {
            ConfigureAuth(app); 
        }
    }
}
