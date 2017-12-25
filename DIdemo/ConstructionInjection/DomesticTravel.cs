using System;
namespace DIdemo.ConstructionInjection
{
    public class DomesticTravel : ITravelProduct
    {
        public string GetLineList() => "国内游的线路列表";
    }
}
