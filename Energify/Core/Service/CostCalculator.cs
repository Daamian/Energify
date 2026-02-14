using Energify.Core.Model;

namespace Energify.Core.Service;

public static class CostCalculator
{
    public static CostReport Calculate(Appliance appliance, Money pricePerKWh)
    {
        var dailyEnergy = appliance.Power * appliance.DailyUsage;

        var monthlyEnergy = dailyEnergy * 30;
        var yearlyEnergy = dailyEnergy * 365;

        var dailyCost = dailyEnergy * pricePerKWh;
        var monthlyCost = monthlyEnergy * pricePerKWh;
        var yearlyCost = yearlyEnergy * pricePerKWh;

        return new CostReport(dailyEnergy, monthlyEnergy, yearlyEnergy, dailyCost, monthlyCost, yearlyCost);
    }

    public static (Money Daily, Money Monthly, Money Yearly) CalculateSummary(Appliance appliance , Money pricePerKWh)
    {
        var report = Calculate(appliance, pricePerKWh);
        return (report.DailyCost, report.MonthlyCost, report.YearlyCost);
    }

    public static (Money TotalMonthly, Appliance MostExpensive, Appliance Cheapest) CalculateMultiple(IEnumerable<Appliance> appliances, Money pricePerKWh)
    {
        var applianceList = appliances.ToList();

        if (applianceList.Count == 0)
            throw new ArgumentException("Appliance collection is empty.", nameof(appliances));

        Money totalMonthly = new(0, pricePerKWh.Currency);
        Appliance mostExpensive = applianceList[0];
        Appliance cheapest = applianceList[0];
        Money highestCost = Calculate(applianceList[0], pricePerKWh).MonthlyCost;
        Money lowestCost = highestCost;

        foreach (var appliance in applianceList)
        {
            var report = Calculate(appliance, pricePerKWh);
            totalMonthly += report.MonthlyCost;

            if (report.MonthlyCost > highestCost)
            {
                highestCost = report.MonthlyCost;
                mostExpensive = appliance;
            }

            if (report.MonthlyCost < lowestCost)
            {
                lowestCost = report.MonthlyCost;
                cheapest = appliance;
            }
        }

        return (totalMonthly, mostExpensive, cheapest);
    }
}