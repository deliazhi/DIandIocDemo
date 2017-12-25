using System;
namespace DIdemo
{
    public class TC_Factory : IFactory
    {
        public ICruiseTravel createCruiseTravel()
        {
            return new TC_CruiseTravel();
        }

        public IDomesticTravel createDomesticTravel()
        {
            return new TC_DomesticTravel();
        }

        public IOutboundTravel createOutboundTravel()
        {
            return new TC_OutboundTravel();
        }
    }
}
