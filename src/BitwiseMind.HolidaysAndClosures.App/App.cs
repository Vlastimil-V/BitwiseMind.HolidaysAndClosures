using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;

namespace BitwiseMind.Globalization;

public class App([FromKeyedServices(Countries.Czechia)] IClosedDaysManager closedDaysManager, ILogger<App> logger)
{
    public async Task RunAsync()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Serilog.Debugging.SelfLog.Enable(Console.Error);

        logger.LogInformation("Application is starting...");

        // Generate 20 random dates between 1990 and 2024
        var dates = new List<DateOnly>();
        var random = new Random();
        for (int i = 0; i < 20; i++)
        {
            int year = random.Next(1900, 2025);
            int month = random.Next(1, 13);
            int day = random.Next(1, DateTime.DaysInMonth(year, month) + 1);
            dates.Add(new DateOnly(year, month, day));
        }

        // Combine random dates and bank holidays
        closedDaysManager.Holidays.ForEach(holiday => dates.Add(holiday.OccurrenceDetails.Start));
        dates.TrimExcess();

        // Iterate over all dates
        foreach (var date in dates)
        {
            logger.LogInformation("Is '{Date}' a holiday day in {Country}? {Answer}", date, Countries.Czechia, closedDaysManager.IsHoliday(date));
            logger.LogInformation("Is '{Date}' a closure day in {Country}? {Answer}", date, Countries.Czechia, closedDaysManager.IsClosed(date));
            
            var holidayName = closedDaysManager.Holidays.FirstOrDefault(holiday => holiday.OccurrenceDetails.Start == date)?.Name;
            if (!string.IsNullOrWhiteSpace(holidayName))
            {
                logger.LogInformation($">> {holidayName}{Environment.NewLine}");
            }
        }

        logger.LogInformation("Application has finished.");
        await Task.CompletedTask;
    }
}