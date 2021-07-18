using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using congestion.calculator;
using congestion_tax_calculator_net_core_WebSite.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace congestion_tax_calculator_net_core_WebSite.Controllers
{
    public class TaxCalculatorController : Controller
    {
        private readonly CongestionTaxCalculator congestionTaxCalculator;
        readonly string HoursAndAmountsSettings;
        IOptions<Myconfig> configuration;
        //private readonly IOptions<Myconfig> config;
        public TaxCalculatorController(IOptions<Myconfig> configuration)
        {
            congestionTaxCalculator = new CongestionTaxCalculator();
            this.configuration = configuration;
        }
        /// <summary>
        /// API call
        /// </summary>
        /// <param name="vehicleType"> Get the type of vehicle</param>
        /// <param name="Dates">Get comma seperated dates</param>
        /// <returns></returns>

        [HttpGet("GetCalCulatedTax/{vehicleType}/Dates/{Dates}")]
        public IActionResult GetCalCulatedTax(string vehicleType, string Dates)
        {
            var allDates = Dates.Split(',');
            DateTime[] dateTimes = new DateTime[allDates.Length];
            Vehicle vehicle;
            DateTime dt=new DateTime();
            int i = 0;
            List<string> HourAmountList = configuration.Value.HoursAndAmounts;
            List<HourAmountDistribution> HourAmountDistributions = GetHourAmountDistribution(HourAmountList);

            foreach (var dateTime in allDates)
            {                
                bool parseError=DateTime.TryParseExact(dateTime,
                       "yyyy-MM-dd HH:mm:ss",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out dt);
                if (parseError)
                {
                    dateTimes[i++] = dt;
                }
                else
                {
                    return BadRequest("Bad date format");
                }
            }

            switch(vehicleType)
            {
                case "Motorbike":
                    vehicle=new Motorbike();
                    break;
                case "Tractor":
                    vehicle = new Tractor();
                    break;
                case "Emergency":
                    vehicle = new Emergency();
                    break;
                case "Diplomat":
                    vehicle = new Diplomat();
                    break;
                case "Foreign":
                    vehicle = new Foreign();
                    break;
                case "Military":
                    vehicle = new Military();
                    break;
                case "Car":
                default:
                    vehicle = new Car();
                    break;
            }
            congestionTaxCalculator.HourAmountDistributionList = HourAmountDistributions;

            int result= congestionTaxCalculator.GetTax(vehicle, dateTimes);

            return Ok(result);
        }

        private List<HourAmountDistribution> GetHourAmountDistribution(List<string> hourAmountList)
        {
            List<HourAmountDistribution> myHourAmountDistribution = new List<HourAmountDistribution>();
            
            foreach (var HourAmount in hourAmountList)
            {

                myHourAmountDistribution.Add(new HourAmountDistribution
                {
                    StartHour = Convert.ToInt32(HourAmount.Split('|')[1].Split(':')[0]),
                    EndHour = Convert.ToInt32(HourAmount.Split('|')[2].Split(':')[0]),
                    StartMin = Convert.ToInt32(HourAmount.Split('|')[1].Split(':')[1]),
                    EndMin = Convert.ToInt32(HourAmount.Split('|')[2].Split(':').Last()),
                    Amount = Convert.ToInt32(HourAmount.Split('|')[3])
                });
            }
            return myHourAmountDistribution;
        }
    }
}
