using System;
namespace DIdemo
{
    public class XC_Factory : IFactory
    {
        public IDomesticTravel createDomesticTravel()
        {
            return new XC_DomesticTravel();
        }

        public IOutboundTravel createOutboundTravel()
        {
            return new XC_OutboundTravel();
        }

        public ICruiseTravel createCruiseTravel()
        {
            return new XC_CruiseTravel();
        }
    }
}
