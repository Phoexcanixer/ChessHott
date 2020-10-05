namespace Chess
{
    using Chess.Pieces;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    public class ChessManage : MonoBehaviour
    {
        public List<BasePieces> allPiecesFirstPlayers = new List<BasePieces>();
        public List<BasePieces> allPiecesSecondPlayers = new List<BasePieces>();

        public Action CallBackCheckKingCheckmate { get; set; }
        public ChessManage ExecuteStart()
        {
            BasePieces[] _allPieces = Resources.LoadAll<BasePieces>("Pieces");

            PawnInstanc(_allPieces.Where(item => item.ePieces.Equals(EPieces.Pawn)).First());
            RookInstanc(_allPieces.Where(item => item.ePieces.Equals(EPieces.Rook)).First());
            KnightInstanc(_allPieces.Where(item => item.ePieces.Equals(EPieces.Knight)).First());
            BishopInstance(_allPieces.Where(item => item.ePieces.Equals(EPieces.Bishop)).First());
            QueenInstance(_allPieces.Where(item => item.ePieces.Equals(EPieces.Queen)).First());
            KingInstance(_allPieces.Where(item => item.ePieces.Equals(EPieces.King)).First());

            ClearRidgid(1);
            return this;
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
        public void ClearNullList()
        {
            allPiecesFirstPlayers = allPiecesFirstPlayers.Where(item => item).ToList();
            allPiecesSecondPlayers = allPiecesSecondPlayers.Where(item => item).ToList();
        }
        void ClearRidgid(int times)
        {
            allPiecesFirstPlayers.ForEach(item => Destroy(item.GetComponent<Rigidbody>(), times));
            allPiecesSecondPlayers.ForEach(item => Destroy(item.GetComponent<Rigidbody>(), times));

            StartCoroutine(OffBoxInteraction(BoardManage.instance.eTurnPlayer, 1f));
        }
        void PawnInstanc(BasePieces pawn)
        {
            PawnSpawn(ESelectStartSide.FirstSide, 1, BoardManage.instance.firstPlayerMat, allPiecesFirstPlayers); // PawnLineFirstPlayer
            PawnSpawn(ESelectStartSide.SecondSide, 6, BoardManage.instance.secondPlayerMat, allPiecesSecondPlayers); // PawnLineSecondPlayer
            void PawnSpawn(ESelectStartSide eSelectStartColor, int slot, Material mat, List<BasePieces> basePieces)
            {
                for (int i = 0; i < BoardManage.instance.detailBoxes[slot].boxManages.Length; i++)
                {
                    BasePieces _pieces = Instantiate(pawn, BoardManage.instance.detailBoxes[slot].boxManages[i].transform);
                    _pieces.eSelectSide = eSelectStartColor;
                    _pieces.meshRdr.material = mat;
                    basePieces.Add(_pieces);
                }
            }
        }
        void RookInstanc(BasePieces rook)
        {
            SpawnPieceTwin(ESelectStartSide.FirstSide, rook, Vector2Int.zero, new Vector2Int(0, 7), BoardManage.instance.firstPlayerMat, allPiecesFirstPlayers); // RookLineFirstPlayer
            SpawnPieceTwin(ESelectStartSide.SecondSide, rook, new Vector2Int(7, 0), new Vector2Int(7, 7), BoardManage.instance.secondPlayerMat, allPiecesSecondPlayers); // RookLineSecondPlayer
        }
        void KnightInstanc(BasePieces knight)
        {
            SpawnPieceTwin(ESelectStartSide.FirstSide, knight, new Vector2Int(0, 1), new Vector2Int(0, 6), BoardManage.instance.firstPlayerMat, allPiecesFirstPlayers); // RookLineFirstPlayer
            SpawnPieceTwin(ESelectStartSide.SecondSide, knight, new Vector2Int(7, 1), new Vector2Int(7, 6), BoardManage.instance.secondPlayerMat, allPiecesSecondPlayers); // RookLineSecondPlayer
        }
        void BishopInstance(BasePieces bishop)
        {
            SpawnPieceTwin(ESelectStartSide.FirstSide, bishop, new Vector2Int(0, 2), new Vector2Int(0, 5), BoardManage.instance.firstPlayerMat, allPiecesFirstPlayers); // RookLineFirstPlayer
            SpawnPieceTwin(ESelectStartSide.SecondSide, bishop, new Vector2Int(7, 2), new Vector2Int(7, 5), BoardManage.instance.secondPlayerMat, allPiecesSecondPlayers); // RookLineSecondPlayer
        }
        void QueenInstance(BasePieces queen)
        {
            SpawnPieceSingle(ESelectStartSide.FirstSide, queen, new Vector2Int(0, 3), BoardManage.instance.firstPlayerMat, allPiecesFirstPlayers); // RookLineFirstPlayer
            SpawnPieceSingle(ESelectStartSide.SecondSide, queen, new Vector2Int(7, 3), BoardManage.instance.secondPlayerMat, allPiecesSecondPlayers); // RookLineSecondPlayer
        }
        void KingInstance(BasePieces king)
        {
            SpawnPieceSingle(ESelectStartSide.FirstSide, king, new Vector2Int(0, 4), BoardManage.instance.firstPlayerMat, allPiecesFirstPlayers); // RookLineFirstPlayer
            SpawnPieceSingle(ESelectStartSide.SecondSide, king, new Vector2Int(7, 4), BoardManage.instance.secondPlayerMat, allPiecesSecondPlayers); // RookLineSecondPlayer
        }
        void SpawnPieceSingle(ESelectStartSide eSelectStartColor, BasePieces piece, Vector2Int slot, Material mat, List<BasePieces> basePieces)
        {
            Spawn(BoardManage.instance.detailBoxes[slot.x].boxManages[slot.y].transform);
            void Spawn(Transform parentPiece)
            {
                BasePieces _pieces = Instantiate(piece, parentPiece);
                _pieces.eSelectSide = eSelectStartColor;
                _pieces.meshRdr.material = mat;
                basePieces.Add(_pieces);
            }
        }
        void SpawnPieceTwin(ESelectStartSide eSelectStartColor, BasePieces piece, Vector2Int slotFirst, Vector2Int slotLast, Material mat, List<BasePieces> basePieces)
        {
            Spawn(BoardManage.instance.detailBoxes[slotFirst.x].boxManages[slotFirst.y].transform);
            Spawn(BoardManage.instance.detailBoxes[slotLast.x].boxManages[slotLast.y].transform);
            void Spawn(Transform parentPiece)
            {
                BasePieces _pieces = Instantiate(piece, parentPiece);
                _pieces.eSelectSide = eSelectStartColor;
                _pieces.meshRdr.material = mat;
                basePieces.Add(_pieces);
            }
        }
    }
}
