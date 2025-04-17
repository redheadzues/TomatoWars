using System;
using System.Collections.Generic;
using Source.Code.IdleNumbers;

namespace Source.Code.ModelsAndServices.Player
{
    public class Wallet
    {
        public Dictionary<CurrencyTypeId, IdleNumber> Balances = new();

        public Wallet()
        {
            foreach (CurrencyTypeId typeId in Enum.GetValues(typeof(CurrencyTypeId)))
            {
                Balances.TryAdd(typeId, 0);
            }
        }
    }
}