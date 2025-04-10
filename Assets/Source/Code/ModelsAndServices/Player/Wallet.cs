using System.Collections.Generic;
using Source.Code.IdleNumbers;

namespace Source.Code.ModelsAndServices.Player
{
    public class Wallet
    {
        public Dictionary<Currency, IdleNumber> Balances = new();
    }
}