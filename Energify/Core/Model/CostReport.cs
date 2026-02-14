namespace ElectricityCost.Model;

public record CostReport(Energy DailyEnergy, Energy MonthlyEnergy, Energy YearlyEnergy, Money DailyCost, Money MonthlyCost, Money YearlyCost);