using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstagramWeb.Common
{
    public static class ExtensionMethods
    {
    }
    public  class StringFromInt
    {
        public enum FormatStyle
        {
            Kilo = 1000,
            Mega = 1000000
        }
        public int Number;
        public FormatStyle Format
        {
            get
            {
                switch (LimitValueBeforeConversion)
                {
                    case 1000: return FormatStyle.Kilo;
                    case 1000000: return FormatStyle.Mega;
                    default:
                        throw new NotImplementedException("You must implement the code for this kind of value");
                }
            }
            set
            {
                if (value == FormatStyle.Kilo)
                {
                    LimitValueBeforeConversion = 1000;
                }
                else if (value == FormatStyle.Mega)
                {
                    LimitValueBeforeConversion = 1000000;
                }
            }
        }
        public int LimitValueBeforeConversion { get; set; }
        public static implicit operator int(StringFromInt s)
        {
            return s.Number;
        }
        public static implicit operator StringFromInt(int number)
        {
            StringFromInt s = new StringFromInt(number);
            return s;
        }

        #region Constructors
        public StringFromInt(int number, FormatStyle format)
        {
            this.Number = number;
            Format = format;
        }
        public StringFromInt(int number)
            : this(number, FormatStyle.Kilo)
        {
            if (number >= 1000000)
            {
                this.Format = FormatStyle.Mega;
            }
        }
        #endregion

        public override string ToString()
        {
            if (Number >= LimitValueBeforeConversion)
            {
                string formatString = "0.#k";
                switch (Format)
                {
                    case FormatStyle.Kilo:
                        formatString = "0.#k";
                        break;
                    case FormatStyle.Mega:
                        formatString = "0.#m";
                        break;
                    default:
                        throw new NotImplementedException("You must implement the code for this kind of value");
                }
                return ((double)Number / LimitValueBeforeConversion).ToString(formatString);
            }
            else
            {
                return Number.ToString();
            }
        }

    }

}