using System;
using System.Linq;

namespace Gritsenko.Universal.Common
{
    public class AmountDeclensions
    {
        public string Nominative { get; set; }
        public string Genitive { get; set; }
        public string Plural { get; set; }

        public string Decline(double number)
        {
            var result = Plural;

            var milesStr = Math.Round(number).ToString("N0");
            var l = milesStr.Last();

            if (l == '1') result = Nominative;

            if ((l == '2' ||
                 l == '3' ||
                 l == '4')
                && !(milesStr.EndsWith("11") || milesStr.EndsWith("12") || milesStr.EndsWith("13") || milesStr.EndsWith("14"))
                ) result = Genitive;
            return result;
        }

    }
}