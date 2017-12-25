using System;
namespace DIdemo.SetterInjection
{
    public class TravelProd
    {
        ITravelProd _itravelproduct;
        public ITravelProd itravelproduct
        {
            set => this._itravelproduct = value;
            get => this._itravelproduct;
        }
        public void GetLineList()
        {
            Console.WriteLine(_itravelproduct.GetLineList());
        }
    }
}
