using Source.Code.IdleNumbers;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Code.CurrencyDisplayWindow
{
    public class CurrencyDisplayView : MonoBehaviour
    {
        [SerializeField] private Text _goldText;
        [SerializeField] private Text _gemText;

        public void SetCurrencyValue(CurrencyTypeId typeId, IdleNumber value)
        {
            var valueText = value.ToString();
            
            switch (typeId)
            {
                case CurrencyTypeId.Gold:
                    _goldText.text = valueText;
                    break;
                case CurrencyTypeId.Gem:
                    _gemText.text = valueText;
                    break;
            }
        }
    }
}