namespace Chess.Table
{
    using System.Linq;
    using UnityEngine;

    public class SetPositionBoxHorizontal : MonoBehaviour
    {
        public ESelectStartSide eSelectStartColor;
        public Material[] allMats;

        #region SetHorizontalLayOut
        public float distancBox = 1.1f;
        [ContextMenu("SetPosition")]
        void SetPos()
        {
            Material _first = null, _second = null;
            SetMat(ref _first, ref _second);
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = i % 2 == 0 ? _first : _second;
                transform.GetChild(i).transform.localPosition = new Vector3(i * distancBox, 0, 0);
            }
        }
        #endregion
        void Start() { }
        void SetMat(ref Material first, ref Material second)
        {
            switch (eSelectStartColor)
            {
                case ESelectStartSide.FirstSide:
                    first = allMats.First();
                    second = allMats.Last();
                    break;
                default:
                    first = allMats.Last();
                    second = allMats.First();
                    break;
            }
        }
    }
}
