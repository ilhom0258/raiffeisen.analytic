using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Raiffeisen.Analytic.Bot.Services;

public static class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) 
    {
        var botToken = Environment.GetEnvironmentVariable("BOT_TOKEN") ?? throw new ArgumentNullException("Not found BOT_TOKEN environment variable");
        
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService((_) => new TelegramBotHostedService(botToken));
            });
    }
}

// // Read configuration from environment variables
// var botToken = Environment.GetEnvironmentVariable("BOT_TOKEN") ?? throw new ArgumentNullException("Not found BOT_TOKEN environment variable");
//
// var botClient = new TelegramBotClient(botToken);
//
// using CancellationTokenSource cts = new ();
//
// // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
// ReceiverOptions receiverOptions = new ()
// {
//     AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
// };
//
//
// Dictionary<string, string> categoryMapping = new ()
//         {
//             { "Raiffeisen banka a.d. Beograd", "Banking" },
//             { "CORAL SRB", "Supermarket" },
//             { "MP607", "Supermarket" },
//             { "LIDL", "Supermarket" },
//             { "MP603", "Supermarket" },
//             { "CITY 35", "Supermarket" },
//             { "Mikromarket", "Supermarket" },
//             { "MAXI", "Supermarket" },
//             { "UNIVEREXPORT", "Supermarket" },
//             { "AROMA", "Supermarket" },
//             { "RAKIC-PROM 3", "Supermarket" },
//
//             { "Kafeterija", "Cafes" },
//             { "SBX BELGRADE Raiceva SRB BEOGRAD", "Cafes" },
//             { "CREMA GEL", "Cafes" },
//             { "JUICE BAR", "Cafes" },
//             { "HLEB I KIFLE", "Cafes" },
//             { "VIDA FOODS", "Cafes" },
//             { "SZTR PEREC", "Cafes" },
//             { "FOOD BAR", "Cafes" },
//             { "AMREST COFFEE", "Cafes" },
//             { "STREET PASTA", "Cafes" },
//             { "PR PIE", "Cafes" },
//             { "Miodrag Nikolic", "Cafes" },
//             { "ENCADA DOO", "Cafes" },
//
//             { "WOLT", "Food delivery" },
//
//             { "LP BROWSSVETA", "Pharmacy/Beauty" },
//             { "LILLY APOTEKA", "Pharmacy/Beauty" },
//             { "DM FILIJALA", "Pharmacy/Beauty" },
//
//
//             { "LA SORELLA SHOPPING MALL SRB NOVI SAD", "Clothing" },
//
//             { "RESERVED", "Clothing" },
//             { "STRADIVARIUS", "Clothing" },
//             { "SEPHORA", "Clothing" },
//             { "H&M", "Clothing" },
//             { "TRGOCENTAR", "Clothing" },
//             { "TEXTIL", "Clothing" },
//             { "EUROSHOP", "Clothing" },
//             { "VIENNA FASHION STORE", "Clothing" },
//             { "KOM691 SRB SRB BEOGRAD", "Clothing" },
//             { "TH 27 Jevrejska", "Clothing" },
//             { "VINTAGE STORY", "Clothing" },
//
//             { "Yandex Go", "Transportation" },
//             { "BG NAPLATA", "Transportation" },
//             { "Zeleznicka stanica", "Transportation" },
//             { "SRBIJA VOZ", "Transportation" },
//
//             { "CINEPLEXX", "Entertainment" },
//             { "Planet Bike", "Entertainment" },
//
//             { "YETTEL", "Communication" },
//
//             { "RAIFFEISEN BANK", "Banking(Cash)" },
//
//             { "Raiffeisen banka a,d", "Bank Commission" },
//
//             { "Poreska uprava Republike Srbije", "Tax" },
//
//             { "AU CVEJIC SRB", "Pharmacy" },
//             { "APOTEKA JANKOVIC", "Pharmacy" },
//             { "SPEC RADIOLOSKA ORDINA", "Pharmacy" },
//             { "AU LAURUS", "Pharmacy" },
//
//             { "GIGATRON", "Tech" },
//             { "Sava neživotno osiguranje", "Insurance" },
//
//             { "SBB DOO", "Internet" },
//             { "DEV", "Job" }
//         };
//
// void DrawPieChart(SKCanvas canvas, Dictionary<string, decimal> data, string title)
// {
//     // Define chart parameters
//     int chartWidth = canvas.DeviceClipBounds.Width;
//     int chartHeight = canvas.DeviceClipBounds.Height;
//     int centerX = chartWidth / 2;
//     int centerY = chartHeight / 2;
//     int radius = Math.Min(chartWidth, chartHeight) / 2 - 50;
//
//     // Calculate total value
//     decimal total = 0;
//     foreach (var pair in data)
//     {
//         total += pair.Value;
//     }
//
//     // Define colors for each category
//     SKColor[] colors = new SKColor[]
//     {
//         SKColor.Parse("#FF6384"), // Red
//         SKColor.Parse("#36A2EB"), // Blue
//         SKColor.Parse("#FFCE56"), // Yellow
//         SKColor.Parse("#4BC0C0"), // Teal
//         SKColor.Parse("#9966FF"), // Purple
//         SKColor.Parse("#FF9F40"), // Orange
//         SKColor.Parse("#8BC34A"), // Green
//         SKColor.Parse("#FF5722"), // Deep Orange
//         SKColor.Parse("#9C27B0"), // Deep Purple
//         SKColor.Parse("#795548")  // Brown
//     };
//
//     // Draw pie slices
//     float startAngle = 0;
//     int colorIndex = 0;
//     foreach (var pair in data)
//     {
//         // Calculate sweep angle for the current slice
//         float sweepAngle = (float)(360 * (pair.Value / total));
//
//         // Define paint for the slice
//         using (var paint = new SKPaint())
//         {
//             paint.Style = SKPaintStyle.Fill;
//             paint.Color = colors[colorIndex % colors.Length];
//
//             // Draw the slice
//             canvas.DrawArc(new SKRect(centerX - radius, centerY - radius, centerX + radius, centerY + radius),
//                 startAngle, sweepAngle, true, paint);
//         }
//
//         // Draw text label with category name and decimal value
//         float angle = startAngle + sweepAngle / 2;
//         float labelRadius = radius + 50;
//         float labelX = centerX + labelRadius * (float)Math.Cos(angle * Math.PI / 180);
//         float labelY = centerY + labelRadius * (float)Math.Sin(angle * Math.PI / 180);
//         string labelText = $"{pair.Key}: {pair.Value:C2}"; // Category name and decimal value
//
//         // Adjust font size based on label length
//         float fontSize = 16;
//         if (labelText.Length > 20) // Adjust this threshold as needed
//         {
//             fontSize = 12;
//         }
//
//         // Define text paint
//         using (var textPaint = new SKPaint())
//         {
//             textPaint.TextSize = fontSize;
//             textPaint.TextAlign = SKTextAlign.Center;
//
//             // Measure text width
//             float textWidth = textPaint.MeasureText(labelText);
//
//             // Draw text label
//             canvas.DrawText(labelText, labelX - textWidth / 2, labelY, textPaint);
//         }
//
//         // Update start angle for the next slice
//         startAngle += sweepAngle;
//         colorIndex++;
//     }
//
//     // Draw chart title
//     using (var titlePaint = new SKPaint())
//     {
//         titlePaint.TextSize = 24;
//         titlePaint.TextAlign = SKTextAlign.Center;
//         titlePaint.Color = SKColors.Black;
//         canvas.DrawText(title, centerX, centerY - radius - 50, titlePaint);
//     }
// }
//
//
// string GeneratePieChartAndSendMessage(Dictionary<string, decimal> data, string title)
// {
//     // Create a chart image
//     using (var chartImage = new SKBitmap(1000, 800))
//     using (var chartCanvas = new SKCanvas(chartImage))
//     {
//         chartCanvas.Clear(SKColors.White);
//
//         // Draw the pie chart
//         DrawPieChart(chartCanvas, data, title);
//
//         // Save the chart image to a file
//         string chartImagePath = $"{title}_chart.png";
//         using (var fs = new FileStream(chartImagePath, FileMode.Create))
//         {
//             chartImage.Encode(SKEncodedImageFormat.Png, 100).SaveTo(fs);
//         }
//
//         // Return the file path of the chart image
//         return chartImagePath;
//     }
// }
//
// botClient.StartReceiving(
//     updateHandler: HandleUpdateAsync,
//     pollingErrorHandler: HandlePollingErrorAsync,
//     receiverOptions: receiverOptions,
//     cancellationToken: cts.Token
// );
//
// var me = await botClient.GetMeAsync();
//
// Console.WriteLine($"Start listening for @{me.Username}");
// Console.ReadLine();
//
// // Send cancellation request to stop bot
// cts.Cancel();
//
//
//
// // Helper method to create a namespace manager
// static XmlNamespaceManager GetNamespaceManager(XmlDocument xmlDoc)
// {
//     XmlNamespaceManager nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
//     nsManager.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");
//     return nsManager;
// }
//
// async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
// {
//     if (update.Message is not { } message)
//         return;
//
//     var chatId = message.Chat.Id;
//
//
//     if (update.Message.Document != null && update.Message.Document.FileName.Contains(".xls"))
//     {
//         // Define dictionaries to store debit and credit sums by group
//         var debitSums = new Dictionary<string, decimal>();
//         var creditSums = new Dictionary<string, decimal>();
//         
//         decimal totalDebit = 0;
//         decimal totalCredit = 0;
//
//         var currency = string.Empty;
//         
//         try
//         {
//             var file = await botClient.GetFileAsync(update.Message.Document.FileId, cancellationToken);
//             using var reader = new MemoryStream();
//             await botClient.DownloadFileAsync(file.FilePath, reader, cancellationToken);
//             // Reset the memory stream position to read from the beginning
//             reader.Seek(0, SeekOrigin.Begin);
//             
//             var xmlDoc = new XmlDocument();
//             xmlDoc.Load(reader);
//
//             // Get all rows
//             using var rows = xmlDoc.SelectNodes("//ss:Row", GetNamespaceManager(xmlDoc));
//
//             var i = 0;
//             // Iterate over each row
//             if (rows != null)
//                 foreach (XmlNode row in rows)
//                 {
//                     i++;
//                     if (i <= 5)
//                     {
//                         continue;
//                     }
//
//                     // Get all cells in the row
//                     var cells = row.SelectNodes("./ss:Cell/ss:Data",
//                         GetNamespaceManager(xmlDoc));
//                     if (cells == null)
//                     {
//                         continue;
//                     } 
//                     
//                     if (cells.Count <= 0)
//                     {
//                         continue;
//                     }
//
//                     if (cells.Count == 4 && cells[0].InnerText.Contains("Prethodno stanje"))
//                     {
//                         continue;
//                     }
//
//                     // Extract relevant data from cells
//                     var date = cells[0].InnerText.Trim();
//                     var description = cells[1].InnerText.Trim();
//                     var transaction = cells[5].InnerText.Trim();
//                     
//                     var creditAmount = decimal.Parse(cells[11].InnerText.Trim(),
//                         CultureInfo.InvariantCulture);
//                     
//                     var debitAmount = decimal.Parse(cells[10].InnerText.Trim(),
//                         CultureInfo.InvariantCulture);
//
//                     // Group by description and accumulate amounts
//                     if (string.IsNullOrEmpty(description)) continue;
//                     
//                     if (creditAmount > 0)
//                     {
//                         var categoryName = categoryMapping.FirstOrDefault(c => description.ToUpper().Contains(c.Key.ToUpper())).Value ?? ""; 
//
//                         categoryName = categoryName == "" ? description : categoryName;
//                         
//                         // Credit transaction
//                         if (!creditSums.TryAdd(categoryName, creditAmount))
//                         {
//                             creditSums[categoryName] += creditAmount;
//                         }
//
//                         totalCredit += creditAmount;
//                     }
//                     else
//                     {
//                         var categoryName = categoryMapping.FirstOrDefault(c => description.ToUpper().Contains(c.Key.ToUpper())).Value ?? ""; 
//
//                         categoryName = categoryName == "" ? description : categoryName;
//                         // Debit transaction
//                         if (!debitSums.TryAdd(categoryName, debitAmount))
//                         {
//                             debitSums[categoryName] += debitAmount;
//                         }
//
//                         totalDebit += debitAmount;
//                     }
//                 }
//
//             // Display statistics
//             // Console.WriteLine("Debit Statistics:");
//             // foreach (var pair in debitSums)
//             // {
//             //     Console.WriteLine($"{pair.Key}: {pair.Value}");
//             // }
//
//             // Console.WriteLine("\nCredit Statistics:");
//             // foreach (var pair in creditSums)
//             // {
//             //     Console.WriteLine($"{pair.Key}: {pair.Value}");
//             // }
//
//             // Console.WriteLine($"Total Debit {totalDebit}");
//             // Console.WriteLine($"Total Credit {totalCredit}");
//             
//             
//     
//             // Generate chart images for debit and credit statistics
//             var debitChartImagePath = GeneratePieChartAndSendMessage(debitSums, "debit");
//             var creditChartImagePath = GeneratePieChartAndSendMessage(creditSums, "credit");
//
//             var debitCaption = debitSums.Select(d => $"{d.Key}: {d.Value:C2}").Aggregate((a, b) => $"{a}\n{b}");
//             debitCaption += "\nTotal: " + $"{totalDebit:C2}";
//             
//             var creditCaption = creditSums.Select(d => $"{d.Key}: {d.Value:C2}").Aggregate((a, b) => $"{a}\n{b}");
//             creditCaption += "\nTotal: " + $"{totalCredit:C2}";
//             
//             // Send the chart images to the bot
//             await using (var debitChartStream = new FileStream(debitChartImagePath, FileMode.Open))
//             await using (var creditChartStream = new FileStream(creditChartImagePath, FileMode.Open))
//             {
//                 var debitChartFile = new InputFileStream(debitChartStream, "debit_chart.png");
//                 await botClient.SendPhotoAsync(chatId, debitChartFile, update.Message.MessageThreadId, caption: debitCaption, cancellationToken: cancellationToken);
//
//                 var creditChartFile = new InputFileStream(creditChartStream, "credit_chart.png");
//                 await botClient.SendPhotoAsync(chatId, creditChartFile, update.Message.MessageThreadId, caption: creditCaption, cancellationToken: cancellationToken);
//             }
//
//             // Clean up: remove the generated chart image files
//             File.Delete(debitChartImagePath);
//             File.Delete(creditChartImagePath);
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine("Error: " + ex.Message);
//         }
//     }
//
//     // Echo received message text
//     // var sentMessage = await botClient.SendTextMessageAsync(
//     //     chatId: chatId,
//     //     text: "You said:\n" + messageText,
//     //     cancellationToken: cancellationToken);
// }
//
// Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
// {
//     var ErrorMessage = exception switch
//     {
//         ApiRequestException apiRequestException
//             => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
//         _ => exception.ToString()
//     };
//
//     Console.WriteLine(ErrorMessage);
//     return Task.CompletedTask;
// }