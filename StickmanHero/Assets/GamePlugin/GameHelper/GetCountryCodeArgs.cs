using System;

namespace mygame.plugin.Android
{
    // Event that occurs when an ad fails to load.
    public class GetCountryCodeArgs : EventArgs
    {
        public string CountryCode { get; set; }
    }
}
