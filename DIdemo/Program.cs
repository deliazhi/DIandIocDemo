using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using DIdemo.SetterInjection;
using DIdemo.ConstructionInjection;
using Autofac;

namespace DIdemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=========依赖注入的方式==========");
            IFactory factory = FactoryContainer.factory;
            IDomesticTravel domesticTravel = factory.createDomesticTravel();
            Console.WriteLine(domesticTravel.GetLineList()); 
            IOutboundTravel outboundTravel = factory.createOutboundTravel();
            Console.WriteLine(outboundTravel.GetLineList()); 
            ICruiseTravel cruiseTravel = factory.createCruiseTravel();
            Console.WriteLine(cruiseTravel.GetLineList()); 

            Console.WriteLine("=========属性注入==========");
            ITravelProd domestic = new SetterInjection.DomesticTravel();
            ITravelProd outbound = new SetterInjection.OutboundTravel();
            ITravelProd cruise = new SetterInjection.CruiseTravel();

            TravelProd travel = new TravelProd();
            travel.itravelproduct = domestic;
            travel.GetLineList();
            travel.itravelproduct = outbound;
            travel.GetLineList();
            travel.itravelproduct = cruise;
            travel.GetLineList();

            Console.WriteLine("=========构造注入==========");
            ITravelProduct construction_domestic = new ConstructionInjection.DomesticTravel();
            ITravelProduct construction_outbound = new ConstructionInjection.OutboundTravel();
            ITravelProduct construction_cruise = new ConstructionInjection.CruiseTravel();

            TravelProduct travel_domestic = new TravelProduct(construction_domestic);
            travel_domestic.GetLineList();
            TravelProduct travel_outbound = new TravelProduct(construction_outbound);
            travel_outbound.GetLineList();
            TravelProduct travel_cruise = new TravelProduct(construction_cruise);
            travel_cruise.GetLineList();

            Console.WriteLine("=========Autofac==========");
            var builder = new ContainerBuilder();
            builder.RegisterType<SetterInjection.DomesticTravel>();
            builder.RegisterType<SetterInjection.OutboundTravel>();
            builder.RegisterType<SetterInjection.CruiseTravel>();
            var container = builder.Build();
            var domesticContainer=container.Resolve<SetterInjection.DomesticTravel>();
            var outboundContainer = container.Resolve<SetterInjection.OutboundTravel>();
            var cruiseContainer = container.Resolve<SetterInjection.CruiseTravel>();
            Console.WriteLine(domesticContainer.GetLineList()); 
            Console.WriteLine(outboundContainer.GetLineList()); 
            Console.WriteLine(cruiseContainer.GetLineList()); 
        }
    }
}
