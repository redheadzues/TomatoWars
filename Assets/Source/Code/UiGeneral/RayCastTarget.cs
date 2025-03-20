using UnityEngine;
using UnityEngine.UI;

namespace Source.Code.UiGeneral
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class RayCastTarget : Graphic
    {
        public override void SetMaterialDirty() { return; }
        public override void SetVerticesDirty() { return; }  
        

    }
}