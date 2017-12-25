using System;
namespace DIdemo
{
    public interface IFactory
    {
        IDomesticTravel createDomesticTravel();
        IOutboundTravel createOutboundTravel();
        ICruiseTravel createCruiseTravel();
    }
}
