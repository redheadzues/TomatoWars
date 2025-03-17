using System.Collections.Generic;

namespace Source.Code.ModelsAndServices.Player
{
    public class Wallet
    {
        public Dictionary<Currency, int> Balances = new();
    }
}