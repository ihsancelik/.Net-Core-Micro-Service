using Miracle.Api.Models.Currency;
using Miracle.Api.Responses.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace Miracle.Api.Services.Helpers
{
    public class CurrencyService
    {
        private string baseUrl = "https://www.tcmb.gov.tr/kurlar/today.xml";
        public ListResponse<CurrencyModel> GetCurrency(List<string> codes)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(baseUrl);
            var currencyElements = xmlDoc.GetElementsByTagName("Currency");
            var currencyList = new List<CurrencyModel>();

            foreach (XmlElement element in currencyElements)
            {
                var currency = element.Attributes["CurrencyCode"].InnerText;
                if (codes.Contains(currency))
                {
                    var decimalStr = element.GetElementsByTagName("BanknoteSelling")[0].FirstChild.Value;
                    decimal.TryParse(decimalStr, NumberStyles.Currency, new CultureInfo("en-US"), out decimal value);

                    currencyList.Add(new CurrencyModel()
                    {
                        Currency = currency,
                        Value = value
                    });
                }
            }
            var result = new ListResponse<CurrencyModel>();
            result.SetData(currencyList);
            return result;
        }
    }
}