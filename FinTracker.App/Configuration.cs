﻿using MudBlazor;

namespace FinTracker.App
{
    public static class Configuration
    {
        public const string HttpClientName = "FinTracker";
        public static string BackendUrl { get; set; } = "https://localhost:44380";
        public static string StripePublicKey { get; set; } = "";

        public static MudTheme Theme = new()
        {
            Typography = new Typography
            {
                Default = new Default
                {
                    FontFamily = ["Sora", "sans-seriff"]
                }
            },
            PaletteLight = new PaletteLight
            {
                Primary = new MudBlazor.Utilities.MudColor("#1EFA2D"),
                PrimaryContrastText = new MudBlazor.Utilities.MudColor("#000000"),
                Secondary = Colors.LightGreen.Darken3,
                Background = Colors.Gray.Lighten4,
                AppbarBackground = new MudBlazor.Utilities.MudColor("#1EFA2D"),
                AppbarText = Colors.Shades.Black,
                TextPrimary = Colors.Shades.Black,
                DrawerText = Colors.Shades.White,
                DrawerBackground = Colors.Green.Lighten4
            },
            PaletteDark = new PaletteDark
            {
                Primary = Colors.LightGreen.Accent3,
                Secondary = Colors.LightGreen.Darken3,
                AppbarBackground = Colors.LightGreen.Accent3,
                AppbarText = Colors.Shades.Black,
                PrimaryContrastText = new MudBlazor.Utilities.MudColor("#000000")
            }
        };
    }
}
