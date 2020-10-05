namespace Checkers
{
    using Chess;
    using Chess.Pieces;
    using Chess.Table;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class CheckersManage : MonoBehaviour
    {
        public List<BasePieces> allPiecesFirstPlayers = new List<BasePieces>();
        public List<BasePieces> allPiecesSecondPlayers = new List<BasePieces>();

        public BasePieces tempEatUpLeft { get; set; }
        public BasePieces tempEatUpRight { get; set; }
        public BasePieces tempEatDownLeft { get; set; }
        public BasePieces tempEatDownRight { get; set; }
        public Vector2Int oldSlot { get; set; }
        public bool isEatPiece;
        public CheckersManage ExecuteStart()
        {
            PawnHottInstant(Resources.LoadAll<BasePieces>("Pieces").Where(item => item.ePieces.Equals(EPieces.PawnHott)).First());
            ClearRidgid(1);
            return this;
        }
        public void ClearNullList()
        {
            allPiecesFirstPlayers = allPiecesFirstPlayers.Where(item => item).ToList();
            allPiecesSecondPlayers = allPiecesSecondPlayers.Where(item => item).ToList();
        }
        void PawnHottInstant(BasePieces pawnHott)
        {
            PawnHottSpawn(ESelectStartSide.FirstSide, new Vector2Int(0, 1), BoardManage.instance.firstPlayerMat, allPiecesFirstPlayers); // PawnLineFirstPlayer
            PawnHottSpawn(ESelectStartSide.SecondSide, new Vector2Int(6, 7), BoardManage.instance.secondPlayerMat, allPiecesSecondPlayers); // PawnLineSecondPlayer
            void PawnHottSpawn(ESelectStartSide eSelectStartColor, Vector2Int slot, Material mat, List<BasePieces> basePieces)
            {
                for (int i = 0; i < BoardManage.instance.detailBoxes[slot.x].boxManages.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        BasePieces _pieces = Instantiate(pawnHott, BoardManage.instance.detailBoxes[slot.x].boxManages[i].transform);
                        _pieces.eSelectSide = eSelectStartColor;
                        _pieces.meshRdr.material = mat;
                        basePieces.Add(_pieces);
                    }
                }

                for (int i = 0; i < BoardManage.instance.detailBoxes[slot.y].boxManages.Length; i++)
                {
                    if (i % 2 != 0)
                    {
                        BasePieces _pieces = Instantiate(pawnHott, BoardManage.instance.detailBoxes[slot.y].boxManages[i].transform);
                        _pieces.eSelectSide = eSelectStartColor;
                        _pieces.meshRdr.material = mat;
                        basePieces.Add(_pieces);
                    }
                }
            }
        }
        public void OffBoxInteraction(ESelectStartSide eSelectStartSide)
        {
            if (eSelectStartSide.Equals(ESelectStartSide.FirstSide))
            {
                allPiecesFirstPlayers.ForEach(item => { if (item) item.thisCollider.enabled = true; });
                allPiecesSecondPlayers.ForEach(item => { if (item) item.thisCollider.enabled = false; });
            }
            else
            {
                allPiecesFirstPlayers.ForEach(item => { if (item) item.thisCollider.enabled = false; });
                allPiecesSecondPlayers.ForEach(item => { if (item) item.thisCollider.enabled = true; });
            }
        }
        IEnumerator OffBoxInteraction(ESelectStartSide eSelectStartSide, float times = 0)
        {
            yield return new WaitForSeconds(times);
            if (eSelectStartSide.Equals(ESelectStartSide.FirstSide))
            {
                allPiecesFirstPlayers.ForEach(item => { if (item) item.thisCollider.enabled = true; });
                allPiecesSecondPlayers.ForEach(item => { if (item) item.thisCollider.enabled = false; });
            }
            else
            {
                allPiecesFirstPlayers.ForEach(item => { if (item) item.thisCollider.enabled = false; });
                allPiecesSecondPlayers.ForEach(item => { if (item) item.thisCollider.enabled = true; });
            }
        }
        void ClearRidgid(int times)
        {
            allPiecesFirstPlayers.ForEach(item => Destroy(item.GetComponent<Rigidbody>(), times));
            allPiecesSecondPlayers.ForEach(item => Destroy(item.GetComponent<Rigidbody>(), times));

            StartCoroutine(OffBoxInteraction(BoardManage.instance.eTurnPlayer, 1f));
        }
        public bool CheckHaveEatPiece() => tempEatUpLeft || tempEatUpRight || tempEatDownLeft || tempEatDownRight;
        public void EatPiece(Vector2Int presentSlot, EPieces ePieces, ESelectStartSide eSelectSide, Vector2Int slot)
        {
            if (presentSlot.x > oldSlot.x) // Up
            {
                if (presentSlot.y > oldSlot.y)
                {
                    if (tempEatUpRight && presentSlot.y > tempEatUpRight.transform.parent.GetComponent<BoxManage>().slot.y)
                    {
                        Debug.Log($"UR");
                        Destroy(tempEatUpRight.gameObject);
                        isEatPiece = true;
                    }
                    else
                        Debug.Log($"OUR");
                }
                else
                {
                    if (tempEatUpLeft && presentSlot.y < tempEatUpLeft.transform.parent.GetComponent<BoxManage>().slot.y)
                    {
                        Debug.Log($"UL");
                        Destroy(tempEatUpLeft.gameObject);
                        isEatPiece = true;
                    }
                    else
                        Debug.Log($"OUL");
                }
            }
            else // Down
            {
                if (presentSlot.y > oldSlot.y)
                {
                    if (tempEatDownRight && presentSlot.y > tempEatDownRight.transform.parent.GetComponent<BoxManage>().slot.y)
                    {
                        Debug.Log($"DR");
                        Destroy(tempEatDownRight.gameObject);
                        isEatPiece = true;
                    }
                    else
                        Debug.Log($"ODR");
                }
                else
                {
                    if (tempEatDownLeft && presentSlot.y < tempEatDownLeft.transform.parent.GetComponent<BoxManage>().slot.y)
                    {
                        Debug.Log($"DL");
                        Destroy(tempEatDownLeft.gameObject);
                        isEatPiece = true;
                    }
                    else
                        Debug.Log($"ODL");
                }
            }

            StartCoroutine(IEEatPiece(ePieces, eSelectSide, slot));
        }
        IEnumerator IEEatPiece(EPieces ePieces, ESelectStartSide eSelectSide, Vector2Int slot)
        {
            ClearTemp();
            ClearNullList();
            yield return new WaitForFixedUpdate();

            if (allPiecesFirstPlayers.Count(item => item) <= 0)
                SideWin.CallBackSideWin?.Invoke(ESelectStartSide.SecondSide);
            else if (allPiecesSecondPlayers.Count(item => item) <= 0)
                SideWin.CallBackSideWin?.Invoke(ESelectStartSide.FirstSide);
            else
            {
                if (isEatPiece)
                {
                    oldSlot = BoardManage.instance.presentTargetBox.slot;
                    yield return new WaitForFixedUpdate();
                    yield return StartCoroutine(BoardManage.instance.subBoardMovePieces.CheckChainingEat(ePieces, eSelectSide, slot));
                }

                if (BoardManage.instance.presentTargetBox.isPawnEnchant && ePieces.Equals(EPieces.PawnHott))
                    EnchantPawn.CallBackAutoEnchant?.Invoke();
                else if (BoardManage.instance.checkersManage.CheckHaveEatPiece())
                    BoardManage.instance.checkersManage.OffBoxInteraction(BoardManage.instance.eTurnPlayer);
                else
                {
                    BoardManage.instance.presentPieces?.DefaulValue();
                    BoardManage.instance.SwichTurn();
                }
            }
        }
        public void ClearTemp()
        {
            tempEatUpLeft = null;
            tempEatUpRight = null;
            tempEatDownLeft = null;
            tempEatDownRight = null;
        }

    }
}
