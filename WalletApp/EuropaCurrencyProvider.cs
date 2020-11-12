using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;
using Wallet.Data;

namespace WalletApp
{
    public class EuropaCurrencyProvider : CurrencyApiProvider
    {
        readonly string url = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";

        public override Dictionary<string, float> GetCurrenciesRates()
        {
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            HttpResponseMessage response = httpClient.GetAsync(url).Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                XDocument xdoc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);
                var rez = xdoc.Descendants()
                    .Where(x => x.Name.LocalName == "Cube" && x.Attribute("currency") != null)
                    .ToDictionary(
                        x => x.Attribute("currency").Value.ToUpper(),
                        x => float.Parse(x.Attribute("rate").Value.Replace(".", ",")));

                rez.Add("EUR", 1);
                return rez;
            }
            else
            {
                return null;
            }
        }
    }
}
