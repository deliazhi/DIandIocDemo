using System;
namespace DIdemo.ConstructionInjection
{
    public class OutboundTravel:ITravelProduct
    {
        public string GetLineList() => "出境游的线路列表";
    }
}
