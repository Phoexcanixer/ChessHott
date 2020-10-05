namespace Chess.Table
{
    using System.Linq;
    using UnityEngine;

    public class SetPositionBoxVertical : MonoBehaviour
    {
        #region SetVerticalLayOut
        public float distancBox = 1.1f;
        [ContextMenu("SetPosition")]
        void SetPos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).transform.localPosition = new Vector3(0, 0, i * distancBox);
            }
        }
        #endregion
        void Start() { }
    }
}
