namespace Chess.Table
{
    using Chess.Pieces;
    using UnityEngine;
    using UnityEngine.EventSystems;
    public class BoxManage : MonoBehaviour, IPointerClickHandler
    {
        public Vector2Int slot;
        public bool isPawnEnchant;
        MeshRenderer _meshRender;
        MeshCollider _meshCollider;
        Material _thisMat;
        void Awake()
        {
            _meshRender = GetComponent<MeshRenderer>();
            _meshCollider = GetComponent<MeshCollider>();
            _thisMat = _meshRender.material;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            BoardManage.instance.presentTargetBox = this;
            BoardManage.instance.presentPieces.Move();
        }
        public void ShowPathMove(bool isShow) => _meshCollider.enabled = isShow;
        public void ShowGuide(bool isShow) => _meshRender.material = isShow ? BoardManage.instance.guideMat : _thisMat;
        public BasePieces GetPieces() => GetComponentInChildren<BasePieces>();
        public void ClearChild()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            (BoardManage.instance.eSelectGame.Equals(ESelectGame.Chase) ? (System.Action)BoardManage.instance.chessManage.ClearNullList : BoardManage.instance.checkersManage.ClearNullList)();
        }
    }
}
