using System;
using System.Collections.Generic;
using System.Text;

namespace DbService.Objects
{
    public class StorePosition
    {
        public int Id { get; set; }
        public string PositionName { get; set; }
        public int PositionType { get; set; }
        public int PositionValue { get; set; }
        public double PositionPrice { get; set; }
        public double PriceCurrency { get; set; }
    }
}
