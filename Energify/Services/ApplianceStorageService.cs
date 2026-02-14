using System.Text.Json;
using Energify.Core.Model;

namespace Energify.Services;

public class ApplianceStorageService
{
    private const string AppliancesKey = "saved_appliances";
    private const string PricePerKwhKey = "price_per_kwh";
    private const string CurrencyKey = "currency";

    public void SaveAppliances(IEnumerable<Appliance> appliances)
    {
        try
        {
            var appliancesList = appliances.ToList();
            var json = JsonSerializer.Serialize(appliancesList);
            Preferences.Set(AppliancesKey, json);
        }
        catch (Exception ex)
        {
            // Log error - w produkcji można użyć loggera
            System.Diagnostics.Debug.WriteLine($"Error saving appliances: {ex.Message}");
        }
    }

    public List<Appliance> LoadAppliances()
    {
        try
        {
            var json = Preferences.Get(AppliancesKey, string.Empty);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<Appliance>();
            }

            var appliances = JsonSerializer.Deserialize<List<Appliance>>(json);
            return appliances ?? new List<Appliance>();
        }
        catch (Exception ex)
        {
            // Log error
            System.Diagnostics.Debug.WriteLine($"Error loading appliances: {ex.Message}");
            return new List<Appliance>();
        }
    }

    public void SavePricePerKwh(decimal price)
    {
        try
        {
            Preferences.Set(PricePerKwhKey, (double)price);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error saving price: {ex.Message}");
        }
    }

    public decimal LoadPricePerKwh(decimal defaultValue = 0.65m)
    {
        try
        {
            var price = Preferences.Get(PricePerKwhKey, (double)defaultValue);
            return (decimal)price;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading price: {ex.Message}");
            return defaultValue;
        }
    }

    public void SaveCurrency(string currency)
    {
        try
        {
            Preferences.Set(CurrencyKey, currency);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error saving currency: {ex.Message}");
        }
    }

    public string LoadCurrency(string defaultValue = "PLN")
    {
        try
        {
            return Preferences.Get(CurrencyKey, defaultValue);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading currency: {ex.Message}");
            return defaultValue;
        }
    }

    public void ClearAll()
    {
        try
        {
            Preferences.Remove(AppliancesKey);
            Preferences.Remove(PricePerKwhKey);
            Preferences.Remove(CurrencyKey);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error clearing data: {ex.Message}");
        }
    }
}
