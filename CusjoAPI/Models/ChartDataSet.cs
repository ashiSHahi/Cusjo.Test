using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CusjoAPI.Models
{
    public class ChartDataSet
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public float Light { get; set; }
        public float CO2 { get; set; }
        public float HumidityRatio { get; set; }
        public float Occupancy { get; set; }
    }
}
