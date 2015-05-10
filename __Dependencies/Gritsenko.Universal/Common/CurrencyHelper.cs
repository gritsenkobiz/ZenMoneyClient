using System;
using System.Collections.Generic;

namespace Gritsenko.Universal.Common
{
    public static class CurrencyHelper
    {
        public static string GetCurrencySymbol(string code)
        {
            if (Currencies.ContainsKey(code))
            {
                return Currencies[code];
            }
            else
            {
                return code;
            }
        }

        public static Dictionary<string, string> Currencies = new Dictionary<string, string>()
        {
            {"AED", "د.إ.‏"},
            {"AFN", "؋ "},
            {"ALL", "Lek"},
            {"AMD", "դր."},
            {"ARS", "$"},
            {"AUD", "$"},
            {"AZN", "man."},
            {"BAM", "KM"},
            {"BDT", "৳"},
            {"BGN", "лв."},
            {"BHD", "د.ب.‏ "},
            {"BND", "$"},
            {"BOB", "$b"},
            {"BRL", "R$"},
            {"BYR", "р."},
            {"BZD", "BZ$"},
            {"CAD", "$"},
            {"CHF", "fr."},
            {"CLP", "$"},
            {"CNY", "¥"},
            {"COP", "$"},
            {"CRC", "₡"},
            {"CSD", "Din."},
            {"CZK", "Kč"},
            {"DKK", "kr."},
            {"DOP", "RD$"},
            {"DZD", "DZD"},
            {"EEK", "kr"},
            {"EGP", "ج.م.‏ "},
            {"ETB", "ETB"},
            {"EUR", "€"},
            {"GBP", "£"},
            {"GEL", "Lari"},
            {"GTQ", "Q"},
            {"HKD", "HK$"},
            {"HNL", "L."},
            {"HRK", "kn"},
            {"HUF", "Ft"},
            {"IDR", "Rp"},
            {"ILS", "₪"},
            {"INR", "रु"},
            {"IQD", "د.ع.‏ "},
            {"IRR", "ريال "},
            {"ISK", "kr."},
            {"JMD", "J$"},
            {"JOD", "د.ا.‏ "},
            {"JPY", "¥"},
            {"KES", "S"},
            {"KGS", "сом"},
            {"KHR", "៛"},
            {"KRW", "₩"},
            {"KWD", "د.ك.‏ "},
            {"KZT", "Т"},
            {"LAK", "₭"},
            {"LBP", "ل.ل.‏ "},
            {"LKR", "රු."},
            {"LTL", "Lt"},
            {"LVL", "Ls"},
            {"LYD", "د.ل.‏ "},
            {"MAD", "د.م.‏ "},
            {"MKD", "ден."},
            {"MNT", "₮"},
            {"MOP", "MOP"},
            {"MVR", "ރ."},
            {"MXN", "$"},
            {"MYR", "RM"},
            {"NIO", "N"},
            {"NOK", "kr"},
            {"NPR", "रु"},
            {"NZD", "$"},
            {"OMR", "ر.ع.‏ "},
            {"PAB", "B/."},
            {"PEN", "S/."},
            {"PHP", "PhP"},
            {"PKR", "Rs"},
            {"PLN", "zł"},
            {"PYG", "Gs"},
            {"QAR", "ر.ق.‏ "},
            {"RON", "lei"},
            {"RSD", "Din."},
            {"RUB", "ђ"}, //"р."}, 
            {"RUR", "руб."}, //"р."}, 
            {"RWF", "RWF"},
            {"SAR", "ر.س.‏ "},
            {"SEK", "kr"},
            {"SGD", "$"},
            {"SYP", "ل.س.‏ "},
            {"THB", "฿"},
            {"TJS", "т.р."},
            {"TMT", "m."},
            {"TND", "د.ت.‏ "},
            {"TRY", "TL"},
            {"TTD", "TT$"},
            {"TWD", "NT$"},
            {"UAH", "₴"},
            {"USD", "$"},
            {"UYU", "$U"},
            {"UZS", "so'm"},
            {"VEF", "Bs. F."},
            {"VND", "₫"},
            {"XOF", "XOF"},
            {"YER", "ر.ي.‏ "},
            {"ZAR", "R"},
            {"ZWL", "Z$"}
        };

        /**********************************************************************************/

        private static Dictionary<string, string> _formats = new Dictionary<string, string>()
        {
            {"RUB", "{0} {1}"},
            {"USD", "{1}{0}"},
            {"EUR", "{1}{0}"},
        };

        /**********************************************************************************/

        private static AmountDeclensions _rubleDeclensions = new AmountDeclensions()
        {
            Nominative = "рубль",
            Genitive = "рубля",
            Plural = "рублей"
        };
        private static AmountDeclensions _amountDeclensions = new AmountDeclensions()
        {
            Nominative = "рокетрубль",
            Genitive = "рокетрубля",
            Plural = "рокетрублей"
        };

        public static void OverrideMilesStrings(string nominative, string genitive, string plural)
        {
            _amountDeclensions = new AmountDeclensions()
            {
                Nominative = nominative,
                Genitive = genitive,
                Plural = plural
            };
        }

        /**********************************************************************************/

        public static string FormatMilesString(double miles, bool showDecimal = false, bool ceilInsteadRound = false)
        {
            try
            {
                return String.Format("{0} {1}", FormatWithDelimeters(miles, showDecimal, ceilInsteadRound), _amountDeclensions.Decline(miles));
            }
            catch (System.Exception ex)
            {
                var exMessage = "Exception in (FeedPageViewModel.cs)\\[FeedPageViewModel.FormatMilesString] " + ex.Message;
                throw  new Exception(exMessage, ex);
            }
            return miles.ToString();
        }

        /**********************************************************************************/
        public static string FormatRubleString(double rubles, bool showDecimal = false, bool ceilInsteadRound = false)
        {
            try
            {
                return String.Format("{0} {1}", FormatWithDelimeters(rubles, showDecimal, ceilInsteadRound), _rubleDeclensions.Decline(rubles));
            }
            catch (System.Exception ex)
            {
                var exMessage = "Exception in (FeedPageViewModel.cs)\\[FeedPageViewModel.FormatMilesString] " + ex.Message;
                throw  new Exception(exMessage, ex);
            }
            return rubles.ToString();
        }

        /**********************************************************************************/

        public static string FormatMoneyString(double amount, string currencyCode, bool showDecimal = false, bool ceilInsteadRound = false)
        {
            var format = "";
            if (!_formats.TryGetValue(currencyCode, out format))
            {
                format = "{0} {1}";
            }

            return String.Format(format, FormatWithDelimeters(amount, showDecimal, ceilInsteadRound), GetCurrencySymbol(currencyCode));
        }

        /**********************************************************************************/

        public static string FormatWithDelimeters(double number, bool decimalPart, bool ceilInsteadRound = false)
        {
            var dec = decimalPart ? ".00" : "";

            var formatStr = "###0" + dec;

            if (number > 10000)
            {
                formatStr = "### ### ### ##0" + dec;
            }

            if (!decimalPart)
            {
                number = (int) (ceilInsteadRound ? number : Math.Round(number));
            }

            var amountStr = Math.Abs(number).ToString(formatStr).Trim();
            if (number < 0)
            {
                amountStr = "-" + amountStr;
            }
            return amountStr;
        }
    }
}