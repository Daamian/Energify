using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectricityCost.Model;
using ElectricityCost.Service;
using Energify.Services;

namespace Energify.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ApplianceStorageService _storageService;

    [ObservableProperty]
    private decimal pricePerKwh = 0.65m;

    [ObservableProperty]
    private string currency = "PLN";

    [ObservableProperty]
    private string newApplianceName = string.Empty;

    [ObservableProperty]
    private string newAppliancePower = string.Empty;

    [ObservableProperty]
    private string newApplianceUsage = string.Empty;

    [ObservableProperty]
    private string totalMonthlyCost = "0.00 PLN";

    [ObservableProperty]
    private bool hasAppliances;

    public ObservableCollection<ApplianceViewModel> Appliances { get; } = new();

    public MainViewModel()
    {
        _storageService = new ApplianceStorageService();
        LoadData();
    }

    [RelayCommand]
    private void AddAppliance()
    {
        if (string.IsNullOrWhiteSpace(NewApplianceName) ||
            !double.TryParse(NewAppliancePower, out var power) ||
            !double.TryParse(NewApplianceUsage, out var usage))
        {
            return;
        }

        if (power <= 0 || usage < 0 || usage > 24)
        {
            return;
        }

        var appliance = new Appliance(NewApplianceName, new Power(power), new Duration(usage));
        var viewModel = ApplianceViewModel.FromAppliance(appliance);

        CalculateAndUpdateAppliance(viewModel);
        Appliances.Add(viewModel);

        // Wyczyść formularz
        NewApplianceName = string.Empty;
        NewAppliancePower = string.Empty;
        NewApplianceUsage = string.Empty;

        UpdateTotalCost();
        SaveData();
    }

    [RelayCommand]
    private void RemoveAppliance(ApplianceViewModel appliance)
    {
        Appliances.Remove(appliance);
        UpdateTotalCost();
        SaveData();
    }

    [RelayCommand]
    private void RecalculateAll()
    {
        foreach (var appliance in Appliances)
        {
            CalculateAndUpdateAppliance(appliance);
        }
        UpdateTotalCost();
    }

    [RelayCommand]
    private void ClearAll()
    {
        Appliances.Clear();
        PricePerKwh = 0.65m;
        Currency = "PLN";
        UpdateTotalCost();
        _storageService.ClearAll();
    }

    partial void OnPricePerKwhChanged(decimal value)
    {
        RecalculateAll();
        _storageService.SavePricePerKwh(value);
    }

    partial void OnCurrencyChanged(string value)
    {
        RecalculateAll();
        _storageService.SaveCurrency(value);
    }

    private void CalculateAndUpdateAppliance(ApplianceViewModel viewModel)
    {
        var appliance = viewModel.ToAppliance();
        var pricePerKWh = new Money(PricePerKwh, Currency);
        var report = CostCalculator.Calculate(appliance, pricePerKWh);

        viewModel.DailyCost = report.DailyCost.ToString();
        viewModel.MonthlyCost = report.MonthlyCost.ToString();
        viewModel.YearlyCost = report.YearlyCost.ToString();
        viewModel.DailyEnergy = report.DailyEnergy.ToString();
        viewModel.MonthlyEnergy = report.MonthlyEnergy.ToString();
        viewModel.YearlyEnergy = report.YearlyEnergy.ToString();
    }

    private void UpdateTotalCost()
    {
        HasAppliances = Appliances.Count > 0;

        if (!HasAppliances)
        {
            TotalMonthlyCost = $"0.00 {Currency}";
            return;
        }

        var appliances = Appliances.Select(a => a.ToAppliance()).ToList();
        var pricePerKWh = new Money(PricePerKwh, Currency);
        var result = CostCalculator.CalculateMultiple(appliances, pricePerKWh);

        TotalMonthlyCost = result.TotalMonthly.ToString();
    }

    private void LoadData()
    {
        PricePerKwh = _storageService.LoadPricePerKwh();
        Currency = _storageService.LoadCurrency();
        
        var savedAppliances = _storageService.LoadAppliances();

        if (savedAppliances.Count == 0)
        {
            return;
        }

        foreach (var appliance in savedAppliances)
        {
            var viewModel = ApplianceViewModel.FromAppliance(appliance);
            CalculateAndUpdateAppliance(viewModel);
            Appliances.Add(viewModel);
        }

        UpdateTotalCost();
    }

    private void SaveData()
    {
        var appliances = Appliances.Select(a => a.ToAppliance()).ToList();
        _storageService.SaveAppliances(appliances);
    }

    private void AddSampleData()
    {
        var samples = new List<(string Name, double Power, double Usage)>
        {
            ("Czajnik", 2000, 0.5),
            ("Lodówka", 150, 24),
            ("Telewizor", 100, 4),
            ("Komputer", 300, 6)
        };

        foreach (var (name, power, usage) in samples)
        {
            var appliance = new Appliance(name, new Power(power), new Duration(usage));
            var viewModel = ApplianceViewModel.FromAppliance(appliance);
            CalculateAndUpdateAppliance(viewModel);
            Appliances.Add(viewModel);
        }

        UpdateTotalCost();
    }
}
