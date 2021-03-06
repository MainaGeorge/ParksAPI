﻿namespace NationalParksProject
{
    public static class AppConstants
    {
        public static string BaseUrl { get; set; } = "https://localhost:44388/";
        public static string NationalParkApiPath { get; set; } = $"{BaseUrl}api/v1/nationalparks";
        public static string TrailsApiPath { get; set; } = $"{BaseUrl}api/v1/trails";

        public static string RegistrationPath { get; set; } = $"{BaseUrl}api/v1/users/register";
        public static string LoginPath { get; set; } = $"{BaseUrl}api/v1/users/authenticate";
    }
}
