namespace Chess.Pieces
{
    using Chess.Table;
    using UnityEngine;
    using UnityEngine.EventSystems;
    public class BasePieces : MonoBehaviour, IPointerClickHandler
    {
        public EPieces ePieces;
        public ESelectStartSide eSelectSide;
        [HideInInspector] public MeshRenderer meshRdr;
        [HideInInspector] public Collider thisCollider;
        public bool isClick, isFirstMove;
        void Awake()
        {
            meshRdr = GetComponentInChildren<MeshRenderer>();
            thisCollider = GetComponent<Collider>();
        }
        void Start()
        {
            if (ePieces.Equals(EPieces.King))
                gameObject.AddComponent<KingCheck>();
        }
        public virtual void Move()
        {
            isFirstMove = false;
            BoardManage.instance.presentTargetBox.ClearChild();
            transform.SetParent(BoardManage.instance.presentTargetBox.transform);
            transform.localPosition = Vector2.zero;

            if (BoardManage.instance.checkersManage)
            {
                if (BoardManage.instance.checkersManage.CheckHaveEatPiece())
                    BoardManage.instance.checkersManage.EatPiece(BoardManage.instance.presentTargetBox.slot, ePieces, eSelectSide, BoardManage.instance.presentTargetBox.slot);
                else
                {
                    if (BoardManage.instance.presentTargetBox.isPawnEnchant && ePieces.Equals(EPieces.PawnHott))
                        EnchantPawn.CallBackAutoEnchant?.Invoke();
                    else
                        ChangTurn();
                }
            }
            else
            {
                if (BoardManage.instance.presentTargetBox.isPawnEnchant && ePieces.Equals(EPieces.Pawn))
                    EnchantPawn.CallBackSelectEnchant?.Invoke(true);
                else
                    ChangTurn();
            }
        }
        void ChangTurn()
        {
            DefaulValue();
            BoardManage.instance.SwichTurn();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!isClick && BoardManage.instance.eTurnPlayer.Equals(eSelectSide))
            {
                BoxManage _box = GetComponentInParent<BoxManage>();
                if (BoardManage.instance.checkersManage)
                {
                    BoardManage.instance.checkersManage.ClearTemp();
                    BoardManage.instance.checkersManage.oldSlot = _box.slot;
                }
                isClick = !isClick;
                meshRdr.material = eSelectSide.Equals(ESelectStartSide.FirstSide) ? BoardManage.instance.selectionFirstMat : BoardManage.instance.selectionSecondMat;
                BoardManage.instance.subBoardMovePieces.CheckMovePieces(ePieces, eSelectSide, _box.slot);
                BoardManage.instance.presentPieces?.DefaulValue();
                BoardManage.instance.presentPieces = this;
            }
        }
        public void DefaulValue()
        {
            isClick = false;
            meshRdr.material = eSelectSide.Equals(ESelectStartSide.FirstSide) ? BoardManage.instance.firstPlayerMat : BoardManage.instance.secondPlayerMat;
        }
    }
}
