using CommunityToolkit.Mvvm.ComponentModel;
using ElectricityCost.Model;

namespace Energify.ViewModels;

public partial class ApplianceViewModel : ObservableObject
{
    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private double powerWatts;

    [ObservableProperty]
    private double dailyUsageHours;

    [ObservableProperty]
    private string dailyCost = string.Empty;

    [ObservableProperty]
    private string monthlyCost = string.Empty;

    [ObservableProperty]
    private string yearlyCost = string.Empty;

    [ObservableProperty]
    private string dailyEnergy = string.Empty;

    [ObservableProperty]
    private string monthlyEnergy = string.Empty;

    [ObservableProperty]
    private string yearlyEnergy = string.Empty;

    public Appliance ToAppliance()
    {
        return new Appliance(Name, new Power(PowerWatts), new Duration(DailyUsageHours));
    }

    public static ApplianceViewModel FromAppliance(Appliance appliance)
    {
        return new ApplianceViewModel
        {
            Name = appliance.Name,
            PowerWatts = appliance.Power.Watts,
            DailyUsageHours = appliance.DailyUsage.HoursPerDay
        };
    }
}
