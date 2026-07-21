using Kader.DTOs.Payment;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kader.DTOs.Cart
{
    public class CombinedDto
    {
        [JsonProperty("payment")]
        public PaymentDto Payment { get; set; }

        [JsonProperty("checkout")]
        public SaveCheckoutDto Checkout { get; set; }
    }
}
