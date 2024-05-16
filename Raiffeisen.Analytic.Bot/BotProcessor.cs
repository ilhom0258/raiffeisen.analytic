using System.Globalization;
using System.Xml;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using SkiaSharp;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;

namespace Raiffeisen.Analytic.Bot;

public class BotProcessor
{
    private readonly TelegramBotClient _botClient;
    private readonly Dictionary<string, string> _categoryMapping;

    public BotProcessor(string botToken)
    {
        _botClient = new TelegramBotClient(botToken);
        _categoryMapping = InitializeCategoryMapping();
    }

    public async Task StartBot(CancellationToken cancellationToken)
    {
        _botClient.StartReceiving(
            HandleUpdateAsync,
            HandlePollingErrorAsync,
            cancellationToken: cancellationToken
        );

        var me = await _botClient.GetMeAsync();
        Console.WriteLine($"Start listening for @{me.Username}");
        Console.ReadLine();
    }

    private static Dictionary<string, string> InitializeCategoryMapping()
    {
        return new Dictionary<string, string>
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

            { "RAIFFEISEN BANK", "Banking(Cash)" },

            { "Raiffeisen banka a,d", "Bank Commission" },

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
    }

    private static void DrawPieChart(SKCanvas canvas, Dictionary<string, decimal> data, string title)
    {
        // Define chart parameters
        int chartWidth = canvas.DeviceClipBounds.Width;
        int chartHeight = canvas.DeviceClipBounds.Height;
        int centerX = chartWidth / 2;
        int centerY = chartHeight / 2;
        int radius = Math.Min(chartWidth, chartHeight) / 2 - 50;

        // Calculate total value
        decimal total = 0;
        foreach (var pair in data)
        {
            total += pair.Value;
        }

        // Define colors for each category
        SKColor[] colors = new SKColor[]
        {
            SKColor.Parse("#FF6384"), // Red
            SKColor.Parse("#36A2EB"), // Blue
            SKColor.Parse("#FFCE56"), // Yellow
            SKColor.Parse("#4BC0C0"), // Teal
            SKColor.Parse("#9966FF"), // Purple
            SKColor.Parse("#FF9F40"), // Orange
            SKColor.Parse("#8BC34A"), // Green
            SKColor.Parse("#FF5722"), // Deep Orange
            SKColor.Parse("#9C27B0"), // Deep Purple
            SKColor.Parse("#795548") // Brown
        };

        // Draw pie slices
        float startAngle = 0;
        int colorIndex = 0;
        foreach (var pair in data)
        {
            // Calculate sweep angle for the current slice
            float sweepAngle = (float)(360 * (pair.Value / total));

            // Define paint for the slice
            using (var paint = new SKPaint())
            {
                paint.Style = SKPaintStyle.Fill;
                paint.Color = colors[colorIndex % colors.Length];

                // Draw the slice
                canvas.DrawArc(new SKRect(centerX - radius, centerY - radius, centerX + radius, centerY + radius),
                    startAngle, sweepAngle, true, paint);
            }

            // Draw text label with category name and decimal value
            float angle = startAngle + sweepAngle / 2;
            float labelRadius = radius + 50;
            float labelX = centerX + labelRadius * (float)Math.Cos(angle * Math.PI / 180);
            float labelY = centerY + labelRadius * (float)Math.Sin(angle * Math.PI / 180);
            string labelText = $"{pair.Key}: {pair.Value:C2}"; // Category name and decimal value

            // Adjust font size based on label length
            float fontSize = 16;
            if (labelText.Length > 20) // Adjust this threshold as needed
            {
                fontSize = 12;
            }

            // Define text paint
            using (var textPaint = new SKPaint())
            {
                textPaint.TextSize = fontSize;
                textPaint.TextAlign = SKTextAlign.Center;

                // Measure text width
                float textWidth = textPaint.MeasureText(labelText);

                // Draw text label
                canvas.DrawText(labelText, labelX - textWidth / 2, labelY, textPaint);
            }

            // Update start angle for the next slice
            startAngle += sweepAngle;
            colorIndex++;
        }

        // Draw chart title
        using (var titlePaint = new SKPaint())
        {
            titlePaint.TextSize = 24;
            titlePaint.TextAlign = SKTextAlign.Center;
            titlePaint.Color = SKColors.Black;
            canvas.DrawText(title, centerX, centerY - radius - 50, titlePaint);
        }
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update.Message is not { Document: { } document, Chat: { Id: var chatId } })
            return;

        if (!document.FileName.Contains(".xls"))
            return;

        try
        {
            var (debitSums, creditSums, totalDebit, totalCredit) =
                await ProcessExcelFileAsync(document, cancellationToken);

            var maxCategoryLength = debitSums.Select(item => item.Key.Length).Concat(creditSums.Select(item => item.Key.Length)).Max();

            // Generate and send pie chart images
            await SendPieChartAsync(debitSums, "Debit", chatId, totalDebit, maxCategoryLength, cancellationToken);
            await SendPieChartAsync(creditSums, "Credit", chatId, totalCredit, maxCategoryLength, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing Excel file: {ex.Message}");
        }
    }

    private async Task<(Dictionary<string, decimal>, Dictionary<string, decimal>, decimal, decimal)>
        ProcessExcelFileAsync(
            Document document,
            CancellationToken cancellationToken)
    {
        var debitSums = new Dictionary<string, decimal>();
        var creditSums = new Dictionary<string, decimal>();
        decimal totalDebit = 0;
        decimal totalCredit = 0;

        using var reader = new MemoryStream();
        var file = await _botClient.GetFileAsync(document.FileId, cancellationToken);
        await _botClient.DownloadFileAsync(file.FilePath, reader, cancellationToken);
        reader.Seek(0, SeekOrigin.Begin);

        var xmlDoc = new XmlDocument();
        xmlDoc.Load(reader);

        // Get all rows
        using var rows = xmlDoc.SelectNodes("//ss:Row", GetNamespaceManager(xmlDoc));

        var i = 0;
        // Iterate over each row
        if (rows != null)
            foreach (XmlNode row in rows)
            {
                i++;
                if (i <= 5)
                {
                    continue;
                }

                // Get all cells in the row
                var cells = row.SelectNodes("./ss:Cell/ss:Data",
                    GetNamespaceManager(xmlDoc));
                if (cells == null)
                {
                    continue;
                }

                if (cells.Count <= 0)
                {
                    continue;
                }

                if (cells.Count == 4 && cells[0].InnerText.Contains("Prethodno stanje"))
                {
                    continue;
                }

                // Extract relevant data from cells
                var date = cells[0].InnerText.Trim();
                var description = cells[1].InnerText.Trim();
                var transaction = cells[5].InnerText.Trim();

                var creditAmount = decimal.Parse(cells[11].InnerText.Trim(),
                    CultureInfo.InvariantCulture);

                var debitAmount = decimal.Parse(cells[10].InnerText.Trim(),
                    CultureInfo.InvariantCulture);

                // Group by description and accumulate amounts
                if (string.IsNullOrEmpty(description)) continue;

                if (creditAmount > 0)
                {
                    var categoryName = _categoryMapping
                        .FirstOrDefault(c => description.ToUpper().Contains(c.Key.ToUpper())).Value ?? "";

                    categoryName = categoryName == "" ? description : categoryName;

                    // Credit transaction
                    if (!creditSums.TryAdd(categoryName, creditAmount))
                    {
                        creditSums[categoryName] += creditAmount;
                    }

                    totalCredit += creditAmount;
                }
                else
                {
                    var categoryName = _categoryMapping
                        .FirstOrDefault(c => description.ToUpper().Contains(c.Key.ToUpper())).Value ?? "";

                    categoryName = categoryName == "" ? description : categoryName;
                    // Debit transaction
                    if (!debitSums.TryAdd(categoryName, debitAmount))
                    {
                        debitSums[categoryName] += debitAmount;
                    }

                    totalDebit += debitAmount;
                }
            }

        return (debitSums, creditSums, totalDebit, totalCredit);
    }
    
    private async Task SendPieChartAsync(
        Dictionary<string, decimal> data,
        string title,
        long chatId,
        decimal total,
        int maxCategoryLength,
        CancellationToken cancellationToken)
    {
        var chartImagePath = GeneratePieChartAndSaveImage(data, title);
        
        var caption = FormatCaption(data, total, maxCategoryLength);

        await using var chartStream = new FileStream(chartImagePath, FileMode.Open);
        var chartFile = new InputFileStream(chartStream, $"{title}_chart.png");

        await _botClient.SendPhotoAsync(
            chatId,
            chartFile,
            caption: caption,
            parseMode: ParseMode.MarkdownV2,
            cancellationToken: cancellationToken
        );

        File.Delete(chartImagePath);
    }

    private string GeneratePieChartAndSaveImage(Dictionary<string, decimal> data, string title)
    {
        // Create a chart image
        using var chartImage = new SKBitmap(1000, 800);
        using var chartCanvas = new SKCanvas(chartImage);
        chartCanvas.Clear(SKColors.White);

        // Draw the pie chart
        DrawPieChart(chartCanvas, data, title);

        // Save the chart image to a file
        var chartImagePath = $"{title}_chart.png";
        using var fs = new FileStream(chartImagePath, FileMode.Create);
        chartImage.Encode(SKEncodedImageFormat.Png, 100).SaveTo(fs);

        // Return the file path of the chart image
        return chartImagePath;
    }

    private static string FormatCaption(Dictionary<string, decimal> data, decimal total, int maxCategoryLength)
    {
        var caption = "```" +
                      "\nCategory".PadRight(maxCategoryLength + 5) + "Amount\n";
        
        caption += "\n" + data.Select(pair => $"{pair.Key}:".PadRight(maxCategoryLength + 5)+$"{pair.Value:N2}").Aggregate((a, b) => $"{a}\n{b}");
        caption += $"\n\nTotal:".PadRight(maxCategoryLength + 5)+$"{total:N2}```";
        return caption;
    }

    private static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }

    static XmlNamespaceManager GetNamespaceManager(XmlDocument xmlDoc)
    {
        XmlNamespaceManager nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
        nsManager.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");
        return nsManager;
    }
}