using System;
using System.Linq;
using BTCPayServer.Client.Models;
using NBitcoin;
using Newtonsoft.Json;

namespace BTCPayServer.Payments.Bitcoin
{
    public class BitcoinLikeOnChainPaymentMethod : IPaymentMethodDetails
    {
        public PaymentType GetPaymentType() => PaymentTypes.BTCLike;

        public string GetPaymentDestination()
        {
            return DepositAddress;
        }

        public decimal GetNextNetworkFee()
        {
            return NextNetworkFee.ToDecimal(MoneyUnit.BTC);
        }

        public decimal GetFeeRate()
        {
            return FeeRate.SatoshiPerByte;
        }

        public void SetPaymentDetails(IPaymentMethodDetails newPaymentMethodDetails)
        {
            DepositAddress = newPaymentMethodDetails.GetPaymentDestination();
            Index = (newPaymentMethodDetails as BitcoinLikeOnChainPaymentMethod)?.Index;
        }
        public NetworkFeeMode NetworkFeeMode { get; set; }

        FeeRate _NetworkFeeRate;
        [JsonConverter(typeof(NBitcoin.JsonConverters.FeeRateJsonConverter))]
        public FeeRate NetworkFeeRate
        {
            get
            {
                // Some old invoices don't have this field set, so we fallback on FeeRate
                return _NetworkFeeRate ?? FeeRate;
            }
            set
            {
                _NetworkFeeRate = value;
            }
        }
        public bool PayjoinEnabled { get; set; }
        // Those properties are JsonIgnore because their data is inside CryptoData class for legacy reason
        [JsonIgnore]
        public FeeRate FeeRate { get; set; }
        [JsonIgnore]
        public Money NextNetworkFee { get; set; }
        [JsonIgnore]
        public String DepositAddress { get; set; }
        public uint? Index { get; set; }

        public BitcoinAddress GetDepositAddress(Network network)
        {
            return string.IsNullOrEmpty(DepositAddress) ? null : BitcoinAddress.Create(DepositAddress, network);
        }
        ///////////////////////////////////////////////////////////////////////////////////////
    }
}
