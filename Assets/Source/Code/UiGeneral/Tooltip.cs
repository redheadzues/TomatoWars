using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Source.Code.UiGeneral
{
    [RequireComponent(typeof(RayCastTarget))]
    public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text _description;
        [SerializeField] private Transform _tooltip;

        private void Awake() => 
            _tooltip.gameObject.SetActive(false);

        public void OnPointerEnter(PointerEventData eventData) => 
            _tooltip.gameObject.SetActive(true);

        public void OnPointerExit(PointerEventData eventData) => 
            _tooltip.gameObject.SetActive(false);
        
        public void SetDescription(string description) => 
            _description.text = description;
    }
}