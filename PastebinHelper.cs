using System;
using System.Collections.Generic;
using System.Linq;
using PastebinAPI;

namespace VSPastebinExtension
{
    public static class PastebinHelper
    {
        public static String DefaultCaption => "Pastebin Extension";

        public static Dictionary<Expiration, String> ExpirationToStringDictionary = new Dictionary<Expiration, String>
        {
            {Expiration.Never, "Never"},
            {Expiration.TenMinutes, "10 minutes"},
            {Expiration.OneHour, "1 hour"},
            {Expiration.OneDay, "1 day"},
            {Expiration.OneWeek, "1 week"},
            {Expiration.TwoWeeks, "2 weeks"},
            {Expiration.OneMonth, "1 month"}
        };

        public static Dictionary<String, Expiration> StringToExpirationDictionary = new Dictionary<String, Expiration>
        {
            {"Never", Expiration.Never},
            {"10 minutes", Expiration.TenMinutes},
            {"1 hour", Expiration.OneHour},
            {"1 day", Expiration.OneDay},
            {"1 week", Expiration.OneWeek},
            {"2 weeks", Expiration.TwoWeeks},
            {"1 month", Expiration.OneMonth}
        };

        public static void ApplyDevKey()
        {
            Pastebin.DevKey = "4c8e82e3e03bef45028b783ecc14ace5";
        }

        public static String IdentifyLanguage(String language)
        {
            if(String.IsNullOrEmpty(language))
                return Language.None.ToString();
            return (Language
                        .All
                        .FirstOrDefault(pastebinLanguage => String.Compare(pastebinLanguage.ToString(), language, true) == 0) ??
                    Language.None)
                .ToString();
        }
    }
}
