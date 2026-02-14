# Energify

A mobile app for calculating electricity costs of household appliances.

## Description

Energify lets you track and estimate electricity costs for individual devices. Based on power consumption, daily usage time, and price per kWh, the app calculates daily, monthly, and yearly energy costs.

## Features

- Add appliances with power (W) and daily usage time
- Calculate energy costs (daily, monthly, yearly)
- Compare appliances — identify the most and least expensive to run
- Summary of total costs across all appliances

## Tech Stack

- **.NET 9 / .NET MAUI** — cross-platform UI framework
- **CommunityToolkit.Mvvm** — MVVM pattern
- **Platforms:** Android, iOS

## Requirements

| Platform | Minimum version |
|----------|----------------|
| Android  | 5.0 (API 21)   |
| iOS      | 15.0           |

## Getting Started

```bash
dotnet build Energify.sln
dotnet run --project Energify/Energify.csproj -f net9.0-android
```

## License

Copyright (c) 2026 Damian Kusek

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.
