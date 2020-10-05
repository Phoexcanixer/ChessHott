using Checkers;
using Chess;
using Chess.Pieces;
using Chess.Table;
using Common.Singleton;
using UnityEngine;

public class BoardManage : ExSingleton<BoardManage>
{
    public ESelectGame eSelectGame;
    public ESelectStartSide eTurnPlayer;
    public Material guideMat, firstPlayerMat, secondPlayerMat, selectionFirstMat, selectionSecondMat;
    public DetailBox[] detailBoxes; // All Box

    #region Property
    public BasePieces presentPieces { get; set; } // PresentSelectPiece
    public BoxManage presentTargetBox { get; set; } // PresentSelectSlot

    public ChessManage chessManage { get; private set; }
    public CheckersManage checkersManage { get; private set; }
    #endregion

    #region Sub
    public SubBoardMovePieces subBoardMovePieces = new SubBoardMovePieces();
    #endregion

    [ContextMenu("SetSlotBoxes")]
    void SetSlotBoxes()
    {
        for (int i = 0; i < detailBoxes.Length; i++)
        {
            for (int j = 0; j < detailBoxes[i].boxManages.Length; j++)
            {
                detailBoxes[i].boxManages[j].slot = new Vector2Int(i, j);
                detailBoxes[i].boxManages[j].name = $"Box[{i}][{j}]";
            }
        }
    }
    void Start()
    {
        switch (eSelectGame)
        {
            case ESelectGame.Chase: chessManage = gameObject.AddComponent<ChessManage>().ExecuteStart(); break;
            default: checkersManage = gameObject.AddComponent<CheckersManage>().ExecuteStart(); break;
        }
    }
    public void SwichTurn()
    {
        subBoardMovePieces.ClearBorad();
        eTurnPlayer = ~(eTurnPlayer - 1);
        if (chessManage)
        {
            chessManage.CallBackCheckKingCheckmate?.Invoke();
            chessManage.OffBoxInteraction(eTurnPlayer);
        }
        else
            checkersManage.OffBoxInteraction(eTurnPlayer);
    }
    public void ClearTurn()
    {
        presentPieces = null;
        presentTargetBox = null;
    }
}
