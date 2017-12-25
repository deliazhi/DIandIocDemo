using System;
using Microsoft.Extensions.Configuration;
namespace DIdemo
{
    internal class FactoryContainer
    {
        public static IFactory factory { get; private set; }
        static FactoryContainer()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("factoryConfig.json");
            var configuration = builder.Build();
            string configValue = configuration["factory"];

            if(configValue=="TC"){
                factory = new TC_Factory();
            }
            else if(configValue=="XC"){
                factory = new XC_Factory();
            }
            else{
                throw new Exception("Factory Init Error");
            }
        }

    }
}
