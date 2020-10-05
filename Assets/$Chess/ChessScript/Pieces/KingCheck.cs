namespace Chess.Pieces
{
    using UnityEngine;
    public class KingCheck : MonoBehaviour
    {
        ESelectStartSide _selectStartColor;
        void Start()
        {
            _selectStartColor = GetComponent<BasePieces>().eSelectSide;
            BoardManage.instance.chessManage.CallBackCheckKingCheckmate += CheckPiecesEatKing;
        }
        void CheckPiecesEatKing()
        {
            Debug.Log($"{_selectStartColor} -- Check");
            BoardManage.instance.subBoardMovePieces.CheckMovePieces(BoardManage.instance.presentPieces.ePieces, BoardManage.instance.presentPieces.eSelectSide, BoardManage.instance.presentTargetBox.slot, true);
        }
        void OnDestroy()
        {
            Debug.Log($"{ ~(_selectStartColor - 1)} --- Win");
            SideWin.CallBackSideWin?.Invoke(~(_selectStartColor - 1));
        }
    }
}
