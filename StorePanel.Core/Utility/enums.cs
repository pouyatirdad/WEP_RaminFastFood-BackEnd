using System;
using System.Collections.Generic;
using System.Text;

namespace StorePanel.Core.Utility
{
    public enum DiscountType
    {
        Percentage = 1,
        Amount = 2
    }
    public enum GeoDivisionType
    {
        Country = 0,
        State = 1,
        City = 2,
    }
    public enum StaticContents
    {
        Phone = 1002,
        Map = 1007,
        Address = 1001,
        Email = 1003,
        Youtube = 1008,
        Instagram = 1009,
        Twitter = 1011,
        Pinterest = 1012,
        Facebook = 1010,
        BlogImage = 1013,
        ContactInfo = 1014
    }
}
