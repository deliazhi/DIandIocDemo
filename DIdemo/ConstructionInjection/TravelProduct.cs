using System;
namespace DIdemo.ConstructionInjection
{
    public class TravelProduct
    {
        ITravelProduct _itravelprod;
        public TravelProduct(ITravelProduct itravelprod){
            _itravelprod = itravelprod;
        }
        public void GetLineList()
        {
            Console.WriteLine(_itravelprod.GetLineList());
        }
    }
}
