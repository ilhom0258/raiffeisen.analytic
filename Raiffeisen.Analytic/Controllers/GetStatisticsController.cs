using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Xml;
using Microsoft.AspNetCore.Mvc;

namespace Raiffeisen.Analytic.Controllers;

public class GetStatisticsController : Controller
{
    // GET
    public IActionResult Index()
    {
        Dictionary<string, string> categoryMapping = new Dictionary<string, string>()
        {
            { "Raiffeisen banka a.d. Beograd", "Banking" },
            { "CORAL SRB", "Supermarket" },
            { "MP607", "Supermarket" },
            { "LIDL", "Supermarket" },
            { "MP603", "Supermarket" },
            { "CITY 35", "Supermarket" },
            { "Mikromarket", "Supermarket" },
            { "MAXI", "Supermarket" },
            { "UNIVEREXPORT", "Supermarket" },
            { "AROMA", "Supermarket" },
            { "RAKIC-PROM 3", "Supermarket" },

            { "Kafeterija", "Cafes" },
            { "SBX BELGRADE Raiceva SRB BEOGRAD", "Cafes" },
            { "CREMA GEL", "Cafes" },
            { "JUICE BAR", "Cafes" },
            { "HLEB I KIFLE", "Cafes" },
            { "VIDA FOODS", "Cafes" },
            { "SZTR PEREC", "Cafes" },
            { "FOOD BAR", "Cafes" },
            { "AMREST COFFEE", "Cafes" },
            { "STREET PASTA", "Cafes" },
            { "PR PIE", "Cafes" },
            { "Miodrag Nikolic", "Cafes" },
            { "ENCADA DOO", "Cafes" },

            { "WOLT", "Food delivery" },

            { "LP BROWSSVETA", "Pharmacy/Beauty" },
            { "LILLY APOTEKA", "Pharmacy/Beauty" },
            { "DM FILIJALA", "Pharmacy/Beauty" },


            { "LA SORELLA SHOPPING MALL SRB NOVI SAD", "Clothing" },

            { "RESERVED", "Clothing" },
            { "STRADIVARIUS", "Clothing" },
            { "SEPHORA", "Clothing" },
            { "H&M", "Clothing" },
            { "TRGOCENTAR", "Clothing" },
            { "TEXTIL", "Clothing" },
            { "EUROSHOP", "Clothing" },
            { "VIENNA FASHION STORE", "Clothing" },
            { "KOM691 SRB SRB BEOGRAD", "Clothing" },
            { "TH 27 Jevrejska", "Clothing" },
            { "VINTAGE STORY", "Clothing" },

            { "Yandex Go", "Transportation" },
            { "BG NAPLATA", "Transportation" },
            { "Zeleznicka stanica", "Transportation" },
            { "SRBIJA VOZ", "Transportation" },

            { "CINEPLEXX", "Entertainment" },
            { "Planet Bike", "Entertainment" },

            { "YETTEL", "Communication" },

            { "RAIFFEISEN BANK", "Cash" },

            { "Raiffeisen banka a,d", "Commission" },

            { "Poreska uprava Republike Srbije", "Tax" },

            { "AU CVEJIC SRB", "Pharmacy" },
            { "APOTEKA JANKOVIC", "Pharmacy" },
            { "SPEC RADIOLOSKA ORDINA", "Pharmacy" },
            { "AU LAURUS", "Pharmacy" },

            { "GIGATRON", "Tech" },
            { "Sava neživotno osiguranje", "Insurance" },

            { "SBB DOO", "Internet" },
            { "DEV", "Job" }
        };


        // URL of the HTTP request
        string url =
            "https://rol.raiffeisenbank.rs/CorporateV4/Protected/Services/DataServiceCorporate.svc/GetExcelAccountTurnover";

        // Create the HTTP request
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/json"; // Assuming JSON content type, change as needed

        // JSON body for the POST request
        string jsonBody =
            "{\n  \"accountNumber\": \"265205031000293089\",\n  \"productCoreID\": \"501\",\n  \"filterParam\": {\n    \"AccountNumber\": \"265205031000293089\",\n    \"ItemType\": \"\",\n    \"FromDate\": \"01.05.2024\",\n    \"ToDate\": \"12.05.2024\",\n    \"FromAmount\": \"\",\n    \"ToAmount\": \"\",\n    \"Name\": \"\",\n    \"CurrencyCodeNumeric\": \"941\",\n    \"ItemCount\": \"\"\n  },\n  \"gridName\": null\n}"; // Example JSON body, replace with your actual JSON

        // Convert the JSON string to a byte array
        byte[] postData = Encoding.UTF8.GetBytes(jsonBody);

        // Set the content length of the POST data
        request.ContentLength = postData.Length;



        // Add cookies to the request
        request.Headers.Add(HttpRequestHeader.Cookie,
            "jts-rw={\"u\":\"8484170147346133876524\"}; jts-ga-fe-gid=228347206.1701473461; jts-ga-fe-cid=1891761062.1701473461; rzbcorv4_dd=Desktop; rzbcorv4_Desktop.theme=RaiffeisenSME; saveChoice=; _pk_id.31.f6be=17aec297167f8bb0.1715469906.; rzbcorv4_culture=en-US; HOEBPWEB_imp_sessID=ffffffffaf1691b545525d5f4f58455e445a4a423660; rzbcorv4_HolosToken=1CF6079331FBFBD770D861FCA20B695B53BB1C73B755D79195498849473BA80A1AADDD9A7CF9E204CB4F434D50200FBBB589D50CEF09A1700188EB3F8A5B293F49990C68F2F673B2095D60389D2914457344243AF1EDE561482E2AE76CB3176F02B8DABE; rzbcorv4_analytics=f4520689-479c-4b60-b0b6-dcb039833c66; rzbcorv4_analytics_activity=1715548002533.44; _pk_ses.31.f6be=1");

        try
        {
            // Write the POST data to the request stream
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(postData, 0, postData.Length);
            }

            // Get the HTTP response
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Read the response data
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    // Read the JSON response
                    string jsonResponse = reader.ReadToEnd();

                    // Parse the JSON response to extract the base64 string
                    using (JsonDocument jsonDocument = JsonDocument.Parse(jsonResponse))
                    {
                        JsonElement root = jsonDocument.RootElement;
                        if (root.ValueKind == JsonValueKind.Array)
                        {
                            // Get the first element of the array (assuming it contains the base64 string)
                            JsonElement base64Element = root[0];
                            if (base64Element.ValueKind == JsonValueKind.String)
                            {
                                // Extract the base64 string
                                string base64String = base64Element.GetString();

                                // Decode the base64 string into a byte array
                                byte[] bytes = Convert.FromBase64String(base64String);

                                // Convert the byte array into a string (assuming it's XML)
                                string xmlString = Encoding.UTF8.GetString(bytes);

                                // Load the XML string into an XML document
                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(xmlString);

                                // Define dictionaries to store debit and credit sums by group
                                Dictionary<string, decimal> debitSums = new Dictionary<string, decimal>();
                                Dictionary<string, decimal> creditSums = new Dictionary<string, decimal>();
                                decimal totalDebit = 0;
                                decimal totalCredit = 0;

                                // Get all rows
                                XmlNodeList rows = xmlDoc.SelectNodes("//ss:Row", GetNamespaceManager(xmlDoc));

                                var i = 0;
                                // Iterate over each row
                                foreach (XmlNode row in rows)
                                {
                                    i++;
                                    if (i <= 5)
                                    {
                                        continue;
                                    }

                                    // Get all cells in the row
                                    XmlNodeList cells = row.SelectNodes("./ss:Cell/ss:Data",
                                        GetNamespaceManager(xmlDoc));

                                    if (cells.Count <= 0)
                                    {
                                        continue;
                                    }

                                    if (cells.Count == 4 && cells[0].InnerText.Contains("Prethodno stanje"))
                                    {
                                        continue;
                                    }

                                    // Extract relevant data from cells
                                    string date = cells[0].InnerText.Trim();
                                    string description = cells[1].InnerText.Trim();
                                    string transaction = cells[5].InnerText.Trim();
                                    decimal creditAmount = decimal.Parse(cells[11].InnerText.Trim(),
                                        CultureInfo.InvariantCulture);
                                    decimal amount = decimal.Parse(cells[10].InnerText.Trim(),
                                        CultureInfo.InvariantCulture);

                                    // Group by description and accumulate amounts
                                    if (!string.IsNullOrEmpty(description))
                                    {
                                        if (creditAmount > 0)
                                        {
                                            var categoryName = "";
                                            foreach (var category in categoryMapping)
                                            {
                                                if (description.ToUpper().Contains(category.Key.ToUpper()))
                                                {
                                                    categoryName = category.Value;
                                                    break;
                                                }
                                            }

                                            categoryName = categoryName == "" ? description : categoryName;
                                            // Credit transaction
                                            if (creditSums.ContainsKey(categoryName))
                                            {
                                                creditSums[categoryName] += creditAmount;
                                            }
                                            else
                                            {
                                                creditSums[categoryName] = creditAmount;
                                            }

                                            totalCredit += creditAmount;
                                        }
                                        else
                                        {
                                            var categoryName = "";
                                            foreach (var category in categoryMapping)
                                            {
                                                if (description.ToUpper().Contains(category.Key.ToUpper()))
                                                {
                                                    categoryName = category.Value;
                                                    break;
                                                }
                                            }

                                            categoryName = categoryName == "" ? description : categoryName;
                                            // Debit transaction
                                            if (debitSums.ContainsKey(categoryName))
                                            {
                                                debitSums[categoryName] += amount;
                                            }
                                            else
                                            {
                                                debitSums[categoryName] = amount;
                                            }

                                            totalDebit += amount;
                                        }
                                    }
                                }

                                // Display statistics
                                Console.WriteLine("Debit Statistics:");
                                foreach (var pair in debitSums)
                                {
                                    Console.WriteLine($"{pair.Key}: {pair.Value}");
                                }

                                Console.WriteLine("\nCredit Statistics:");
                                foreach (var pair in creditSums)
                                {
                                    Console.WriteLine($"{pair.Key}: {pair.Value}");
                                }

                                Console.WriteLine($"Total Debit {totalDebit}");
                                Console.WriteLine($"Total Credit {totalCredit}");

                            }
                            else
                            {
                                Console.WriteLine("Error: Base64 string not found in JSON response.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error: JSON response is not an array.");
                        }
                    }
                }
            }

            // Close the HTTP response
            response.Close();
        }
        catch (WebException ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

        return Ok();
    }
    
    
    // Helper method to create a namespace manager
    static XmlNamespaceManager GetNamespaceManager(XmlDocument xmlDoc)
    {
        XmlNamespaceManager nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
        nsManager.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");
        return nsManager;
    }
}