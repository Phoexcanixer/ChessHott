namespace Chess.Table
{
    using Chess.Pieces;
    using Common.ArrayList;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    public class SubBoardMovePieces
    {
        Vector2Int _slot;
        List<Vector2Int> _allSlot;
        ESelectStartSide _side;
        public void ClearBorad()
        {
            for (int i = 0; i < BoardManage.instance.detailBoxes.Length; i++)
            {
                for (int j = 0; j < BoardManage.instance.detailBoxes[i].boxManages.Length; j++)
                {
                    BoardManage.instance.detailBoxes[i].boxManages[j].ShowPathMove(false);
                    BoardManage.instance.detailBoxes[i].boxManages[j].ShowGuide(false);
                }
            }

        }
        public void CheckMovePieces(EPieces ePieces, ESelectStartSide eSelectSide, Vector2Int slot, bool isCheckmate = false)
        {
            _allSlot = new List<Vector2Int>();
            _slot = slot;
            _side = eSelectSide;

            switch (ePieces)
            {
                case EPieces.Pawn: PawnMove(); break;
                case EPieces.Rook: RookMove(); break;
                case EPieces.Knight: KnightMove(); break;
                case EPieces.Bishop: BishopMove(); break;
                case EPieces.Queen: QueenMove(); break;
                case EPieces.King: KingMove(); break;
                case EPieces.PawnHott: PawnHottMove(); break;
                case EPieces.Hott: HottMove(); break;
                default: break;
            }

            _allSlot = _allSlot.Distinct().OrderBy(item => item.x).ThenBy(item => item.y).ToList();
            (isCheckmate ? (System.Action)Checkmate : CheckInteractBoard)();
        }
        public IEnumerator CheckChainingEat(EPieces ePieces, ESelectStartSide eSelectSide, Vector2Int slot)
        {
            BoardManage.instance.checkersManage.isEatPiece = false;
            _allSlot = new List<Vector2Int>();
            _slot = slot;
            _side = eSelectSide;

            switch (ePieces)
            {
                case EPieces.PawnHott: PawnHottMove(true); break;
                case EPieces.Hott: HottMove(true); break;
                default: break;
            }

            if (BoardManage.instance.checkersManage.CheckHaveEatPiece())
            {
                _allSlot = _allSlot.Distinct().OrderBy(item => item.x).ThenBy(item => item.y).ToList();
                CheckInteractBoard();
            }
            yield return new WaitForFixedUpdate();
        }
        void CheckInteractBoard()
        {
            for (int i = 0; i < BoardManage.instance.detailBoxes.Length; i++)
            {
                for (int j = 0; j < BoardManage.instance.detailBoxes[i].boxManages.Length; j++)
                {
                    BoardManage.instance.detailBoxes[i].boxManages[j].ShowPathMove(false);
                    BoardManage.instance.detailBoxes[i].boxManages[j].ShowGuide(false);
                    CheckMove(BoardManage.instance.detailBoxes[i].boxManages[j].slot);
                }
            }
            void CheckMove(Vector2Int slot)
            {
                if (_allSlot.Count <= 0)
                    return;

                Vector2Int _tempSlot = new Vector2Int();
                _allSlot.ForEach(item =>
                {
                    if (item == slot)
                    {
                        BoardManage.instance.detailBoxes[item.x].boxManages[item.y].ShowPathMove(true);
                        BoardManage.instance.detailBoxes[item.x].boxManages[item.y].ShowGuide(true);
                        _tempSlot = item;
                    }
                });
                _allSlot.Remove(_tempSlot);
            }
        }
        void Checkmate()
        {
            // TODO: Checkmate
            Debug.Log($"Check");
        }
        void CheckSide(Vector2Int slot)
        {
            BasePieces _piece = BoardManage.instance.detailBoxes[slot.x].boxManages[slot.y].GetPieces();
            if (!_piece)
            {
                _allSlot.Add(new Vector2Int(slot.x, slot.y));
            }
            else
            {
                if (!_piece.eSelectSide.Equals(_side))
                    _allSlot.Add(new Vector2Int(slot.x, slot.y));
            }
        }
        void PawnMove()
        {
            PawnMoveSlot(_side.Equals(ESelectStartSide.FirstSide) ? _slot.x + 1 : _slot.x - 1);
            if (BoardManage.instance.detailBoxes[_slot.x].boxManages[_slot.y].GetPieces().isFirstMove)
                PawnMoveSlotFirst(_side.Equals(ESelectStartSide.FirstSide) ? 2 : -2);

            void PawnMoveSlot(int slotX)
            {
                int _slotPlayer = slotX;
                if (BoardManage.instance.detailBoxes.CheckOutOfRange(_slotPlayer))
                {
                    // Up
                    if (BoardManage.instance.detailBoxes[_slotPlayer].boxManages[_slot.y].transform.childCount <= 0)
                    {
                        _allSlot.Add(new Vector2Int(_slotPlayer, _slot.y));
                    }
                    // Left
                    if (BoardManage.instance.detailBoxes[_slotPlayer].boxManages.CheckOutOfRange(_slot.y - 1))
                    {
                        var _piece = BoardManage.instance.detailBoxes[_slotPlayer].boxManages[_slot.y - 1].GetPieces();
                        if (BoardManage.instance.detailBoxes[_slotPlayer].boxManages[_slot.y - 1].transform.childCount > 0 && _piece != null && !_piece.eSelectSide.Equals(_side))
                        {
                            _allSlot.Add(new Vector2Int(_slotPlayer, _slot.y - 1));
                        }
                    }
                    // Right
                    if (BoardManage.instance.detailBoxes[_slotPlayer].boxManages.CheckOutOfRange(_slot.y + 1))
                    {
                        var _piece = BoardManage.instance.detailBoxes[_slotPlayer].boxManages[_slot.y + 1].GetPieces();
                        if (BoardManage.instance.detailBoxes[_slotPlayer].boxManages[_slot.y + 1].transform.childCount > 0 && _piece != null && !_piece.eSelectSide.Equals(_side))
                        {
                            _allSlot.Add(new Vector2Int(_slotPlayer, _slot.y + 1));
                        }
                    }
                }
            }
            void PawnMoveSlotFirst(int slotX)
            {
                int _slotPlayer = _slot.x + slotX;
                if (BoardManage.instance.detailBoxes[slotX > 0 ? _slotPlayer - 1 : _slotPlayer + 1].boxManages[_slot.y].transform.childCount <= 0)
                {
                    if (BoardManage.instance.detailBoxes[_slotPlayer].boxManages[_slot.y].transform.childCount <= 0)
                    {
                        _allSlot.Add(new Vector2Int(_slotPlayer, _slot.y));
                    }
                }
            }
        }
        void RookMove()
        {
            int _slotX = 1;
            int _slotY = 1;

            //Check UP
            while (BoardManage.instance.detailBoxes.CheckOutOfRange(_slot.x + _slotX))
            {
                int _slotUp = _slot.x + _slotX;
                BasePieces _piece = BoardManage.instance.detailBoxes[_slotUp].boxManages[_slot.y].GetPieces();
                if (!_piece)
                {
                    _allSlot.Add(new Vector2Int(_slotUp, _slot.y));
                    _slotX++;
                }
                else
                {
                    if (!_piece.eSelectSide.Equals(_side))
                        _allSlot.Add(new Vector2Int(_slotUp, _slot.y));

                    break;
                }
            }
            //Check Down
            _slotX = 1;
            while (BoardManage.instance.detailBoxes.CheckOutOfRange(_slot.x - _slotX))
            {
                int _slotDown = _slot.x - _slotX;
                BasePieces _piece = BoardManage.instance.detailBoxes[_slotDown].boxManages[_slot.y].GetPieces();
                if (!_piece)
                {
                    _allSlot.Add(new Vector2Int(_slotDown, _slot.y));
                    _slotX++;
                }
                else
                {
                    if (!_piece.eSelectSide.Equals(_side))
                        _allSlot.Add(new Vector2Int(_slotDown, _slot.y));

                    break;
                }
            }
            //Check Right
            while (BoardManage.instance.detailBoxes[_slot.x].boxManages.CheckOutOfRange(_slot.y + _slotY))
            {
                int _slotRight = _slot.y + _slotY;
                BasePieces _piece = BoardManage.instance.detailBoxes[_slot.x].boxManages[_slotRight].GetPieces();
                if (!_piece)
                {
                    _allSlot.Add(new Vector2Int(_slot.x, _slotRight));
                    _slotY++;
                }
                else
                {
                    if (!_piece.eSelectSide.Equals(_side))
                        _allSlot.Add(new Vector2Int(_slot.x, _slotRight));

                    break;
                }
            }
            //Check Left
            _slotY = 1;
            while (BoardManage.instance.detailBoxes[_slot.x].boxManages.CheckOutOfRange(_slot.y - _slotY))
            {
                int _slotRight = _slot.y - _slotY;
                BasePieces _piece = BoardManage.instance.detailBoxes[_slot.x].boxManages[_slotRight].GetPieces();
                if (!_piece)
                {
                    _allSlot.Add(new Vector2Int(_slot.x, _slotRight));
                    _slotY++;
                }
                else
                {
                    if (!_piece.eSelectSide.Equals(_side))
                        _allSlot.Add(new Vector2Int(_slot.x, _slotRight));

                    break;
                }
            }
        }
        void KnightMove()
        {
            int _slotX = _slot.x + 2;
            int _slotY = _slot.y - 1;
            // UP
            if (BoardManage.instance.detailBoxes.CheckOutOfRange(_slotX))
            {
                // Left
                if (BoardManage.instance.detailBoxes[_slotX].boxManages.CheckOutOfRange(_slotY))
                {
                    CheckSide(new Vector2Int(_slotX, _slotY));
                }
                _slotY = _slot.y + 1;
                // Right
                if (BoardManage.instance.detailBoxes[_slotX].boxManages.CheckOutOfRange(_slotY))
                {
                    CheckSide(new Vector2Int(_slotX, _slotY));
                }
            }
            _slotX = _slot.x - 2;
            // Down
            if (BoardManage.instance.detailBoxes.CheckOutOfRange(_slotX))
            {
                _slotY = _slot.y - 1;
                // Left
                if (BoardManage.instance.detailBoxes[_slotX].boxManages.CheckOutOfRange(_slotY))
                {
                    CheckSide(new Vector2Int(_slotX, _slotY));
                }
                _slotY = _slot.y + 1;
                // Right
                if (BoardManage.instance.detailBoxes[_slotX].boxManages.CheckOutOfRange(_slotY))
                {
                    CheckSide(new Vector2Int(_slotX, _slotY));
                }
            }
            _slotY = _slot.y + 2;
            // Right
            if (BoardManage.instance.detailBoxes[_slot.x].boxManages.CheckOutOfRange(_slotY))
            {
                _slotX = _slot.x - 1;
                // Left
                if (BoardManage.instance.detailBoxes.CheckOutOfRange(_slotX))
                {
                    CheckSide(new Vector2Int(_slotX, _slotY));
                }
                _slotX = _slot.x + 1;
                // Right
                if (BoardManage.instance.detailBoxes.CheckOutOfRange(_slotX))
                {
                    CheckSide(new Vector2Int(_slotX, _slotY));
                }
            }
            // Left
            _slotY = _slot.y - 2;
            if (BoardManage.instance.detailBoxes[_slot.x].boxManages.CheckOutOfRange(_slotY))
            {
                _slotX = _slot.x - 1;
                // Left
                if (BoardManage.instance.detailBoxes.CheckOutOfRange(_slotX))
                {
                    CheckSide(new Vector2Int(_slotX, _slotY));
                }
                _slotX = _slot.x + 1;
                // Right
                if (BoardManage.instance.detailBoxes.CheckOutOfRange(_slotX))
                {
                    CheckSide(new Vector2Int(_slotX, _slotY));
                }
            }
        }
        void BishopMove()
        {
            int _slotX = 1;
            int _slotY = 1;
            // UpLeft
            while (BoardManage.instance.detailBoxes.CheckOutOfRange(_slot.x + _slotX))
            {
                int _slotUpX = _slot.x + _slotX;
                int _slotUpY = _slot.y - _slotY;

                if (BoardManage.instance.detailBoxes[_slotUpX].boxManages.CheckOutOfRange(_slotUpY))
                {
                    BasePieces _piece = BoardManage.instance.detailBoxes[_slotUpX].boxManages[_slotUpY].GetPieces();
                    if (!_piece)
                    {
                        _allSlot.Add(new Vector2Int(_slotUpX, _slotUpY));
                        _slotX++;
                        _slotY++;
                    }
                    else
                    {
                        if (!_piece.eSelectSide.Equals(_side))
                            _allSlot.Add(new Vector2Int(_slotUpX, _slotUpY));

                        break;
                    }
                }
                else
                    break;
            }

            _slotX = 1;
            _slotY = 1;
            // UpRight
            while (BoardManage.instance.detailBoxes.CheckOutOfRange(_slot.x + _slotX))
            {
                int _slotUpX = _slot.x + _slotX;
                int _slotUpY = _slot.y + _slotY;

                if (BoardManage.instance.detailBoxes[_slotUpX].boxManages.CheckOutOfRange(_slotUpY))
                {
                    BasePieces _piece = BoardManage.instance.detailBoxes[_slotUpX].boxManages[_slotUpY].GetPieces();
                    if (!_piece)
                    {
                        _allSlot.Add(new Vector2Int(_slotUpX, _slotUpY));
                        _slotX++;
                        _slotY++;
                    }
                    else
                    {
                        if (!_piece.eSelectSide.Equals(_side))
                            _allSlot.Add(new Vector2Int(_slotUpX, _slotUpY));

                        break;
                    }
                }
                else
                    break;
            }

            _slotX = 1;
            _slotY = 1;
            // DownLeft
            while (BoardManage.instance.detailBoxes.CheckOutOfRange(_slot.x - _slotX))
            {
                int _slotUpX = _slot.x - _slotX;
                int _slotUpY = _slot.y - _slotY;

                if (BoardManage.instance.detailBoxes[_slotUpX].boxManages.CheckOutOfRange(_slotUpY))
                {
                    BasePieces _piece = BoardManage.instance.detailBoxes[_slotUpX].boxManages[_slotUpY].GetPieces();
                    if (!_piece)
                    {
                        _allSlot.Add(new Vector2Int(_slotUpX, _slotUpY));
                        _slotX++;
                        _slotY++;
                    }
                    else
                    {
                        if (!_piece.eSelectSide.Equals(_side))
                            _allSlot.Add(new Vector2Int(_slotUpX, _slotUpY));

                        break;
                    }
                }
                else
                    break;
            }
            _slotX = 1;
            _slotY = 1;
            // DownRight
            while (BoardManage.instance.detailBoxes.CheckOutOfRange(_slot.x - _slotX))
            {
                int _slotUpX = _slot.x - _slotX;
                int _slotUpY = _slot.y + _slotY;

                if (BoardManage.instance.detailBoxes[_slotUpX].boxManages.CheckOutOfRange(_slotUpY))
                {
                    BasePieces _piece = BoardManage.instance.detailBoxes[_slotUpX].boxManages[_slotUpY].GetPieces();
                    if (!_piece)
                    {
                        _allSlot.Add(new Vector2Int(_slotUpX, _slotUpY));
                        _slotX++;
                        _slotY++;
                    }
                    else
                    {
                        if (!_piece.eSelectSide.Equals(_side))
                            _allSlot.Add(new Vector2Int(_slotUpX, _slotUpY));

                        break;
                    }
                }
                else
                    break;
            }
        }
        void QueenMove()
        {
            RookMove();
            BishopMove();
        }
        void KingMove()
        {
            int _slotX = _slot.x + 1;
            int _slotY;
            // Up
            if (BoardManage.instance.detailBoxes.CheckOutOfRange(_slotX))
            {
                _slotY = _slot.y - 1;
                // Left
                if (BoardManage.instance.detailBoxes[_slotX].boxManages.CheckOutOfRange(_slotY))
                {
                    CheckSide(new Vector2Int(_slotX, _slotY));
                }
                _slotY = _slot.y;
                // Middle
                if (BoardManage.instance.detailBoxes[_slotX].boxManages.CheckOutOfRange(_slotY))
                {
                    CheckSide(new Vector2Int(_slotX, _slotY));
                }
                _slotY = _slot.y + 1;
                // Right
                if (BoardManage.instance.detailBoxes[_slotX].boxManages.CheckOutOfRange(_slotY))
                {
                    CheckSide(new Vector2Int(_slotX, _slotY));
                }
            }

            _slotX = _slot.x - 1;
            _slotY = _slot.y;
            // Down
            if (BoardManage.instance.detailBoxes.CheckOutOfRange(_slotX))
            {
                _slotY = _slot.y - 1;
                // Left
                if (BoardManage.instance.detailBoxes[_slotX].boxManages.CheckOutOfRange(_slotY))
                {
                    CheckSide(new Vector2Int(_slotX, _slotY));
                }
                _slotY = _slot.y;
                // Middle
                if (BoardManage.instance.detailBoxes[_slotX].boxManages.CheckOutOfRange(_slotY))
                {
                    CheckSide(new Vector2Int(_slotX, _slotY));
                }
                _slotY = _slot.y + 1;
                // Right
                if (BoardManage.instance.detailBoxes[_slotX].boxManages.CheckOutOfRange(_slotY))
                {
                    CheckSide(new Vector2Int(_slotX, _slotY));
                }

            }

            _slotX = _slot.x;
            _slotY = _slot.y - 1;
            // Left
            if (BoardManage.instance.detailBoxes[_slotX].boxManages.CheckOutOfRange(_slotY))
            {
                CheckSide(new Vector2Int(_slotX, _slotY));
            }
            _slotX = _slot.x;
            _slotY = _slot.y + 1;
            // Right
            if (BoardManage.instance.detailBoxes[_slotX].boxManages.CheckOutOfRange(_slotY))
            {
                CheckSide(new Vector2Int(_slotX, _slotY));
            }

        }

        void PawnHottMove(bool isChanningEat = false)
        {
            PawnMoveSlot(_side.Equals(ESelectStartSide.FirstSide) ? _slot.x + 1 : _slot.x - 1);
            void PawnMoveSlot(int slotX)
            {
                if (BoardManage.instance.detailBoxes.CheckOutOfRange(slotX))
                {
                    CheckMove(-1); // Left
                    CheckMove(1); // Right
                }

                void CheckMove(int slotY)
                {
                    int slotYs = _slot.y + slotY;
                    if (BoardManage.instance.detailBoxes[slotX].boxManages.CheckOutOfRange(slotYs))
                    {
                        var _piece = BoardManage.instance.detailBoxes[slotX].boxManages[slotYs].GetPieces();
                        if (!isChanningEat && BoardManage.instance.detailBoxes[slotX].boxManages[slotYs].transform.childCount <= 0)
                        {
                            _allSlot.Add(new Vector2Int(slotX, slotYs));
                        }
                        else if (BoardManage.instance.detailBoxes[slotX].boxManages[slotYs].transform.childCount > 0 && !_piece.eSelectSide.Equals(_side))
                        {
                            int _slotEatX = _side.Equals(ESelectStartSide.FirstSide) ? slotX + 1 : slotX - 1;
                            int _slotEatY = slotY > 0 ? slotYs + 1 : slotYs - 1;
                            if (BoardManage.instance.detailBoxes.CheckOutOfRange(_slotEatX))
                            {
                                if (BoardManage.instance.detailBoxes[_slotEatX].boxManages.CheckOutOfRange(_slotEatY))
                                {
                                    if (BoardManage.instance.detailBoxes[_slotEatX].boxManages[_slotEatY].transform.childCount <= 0)
                                    {
                                        CheckTempPiece(BoardManage.instance.detailBoxes[slotX].boxManages[slotYs].GetPieces(), slotY);
                                        _allSlot.Add(new Vector2Int(_slotEatX, _slotEatY));
                                    }
                                }
                            }
                        }
                    }
                }
                void CheckTempPiece(BasePieces basePieces, int sideY)
                {
                    if (_side.Equals(ESelectStartSide.FirstSide))
                    {
                        if (sideY < 0)
                            BoardManage.instance.checkersManage.tempEatUpLeft = basePieces;
                        else
                            BoardManage.instance.checkersManage.tempEatUpRight = basePieces;
                    }
                    else
                    {
                        if (sideY < 0)
                            BoardManage.instance.checkersManage.tempEatDownLeft = basePieces;
                        else
                            BoardManage.instance.checkersManage.tempEatDownRight = basePieces;
                    }
                }
            }
        }
        void HottMove(bool isChanningEat = false)
        {
            int _slotX = 1;
            int _slotY = 1;
            // UpLeft
            while (BoardManage.instance.detailBoxes.CheckOutOfRange(_slot.x + _slotX))
            {
                int _slotUpX = _slot.x + _slotX;
                int _slotUpY = _slot.y - _slotY;

                if (BoardManage.instance.detailBoxes[_slotUpX].boxManages.CheckOutOfRange(_slotUpY))
                {
                    BasePieces _piece = BoardManage.instance.detailBoxes[_slotUpX].boxManages[_slotUpY].GetPieces();
                    if (!_piece)
                    {
                        if (!isChanningEat)
                            _allSlot.Add(new Vector2Int(_slotUpX, _slotUpY));
                        _slotX++;
                        _slotY++;
                    }
                    else
                    {
                        int _lenghtX = _slot.x + (_slotX + 1);
                        int _lenghtY = _slot.y - (_slotY + 1);
                        if (!_piece.eSelectSide.Equals(_side) && BoardManage.instance.detailBoxes.CheckOutOfRange(_lenghtX))
                        {
                            if (BoardManage.instance.detailBoxes[_lenghtX].boxManages.CheckOutOfRange(_lenghtY))
                            {
                                if (BoardManage.instance.detailBoxes[_lenghtX].boxManages[_lenghtY].transform.childCount <= 0)
                                {
                                    BoardManage.instance.checkersManage.tempEatUpLeft = _piece;
                                    _allSlot.Add(new Vector2Int(_lenghtX, _lenghtY));
                                }
                            }
                        }

                        break;
                    }
                }
                else
                    break;
            }

            _slotX = 1;
            _slotY = 1;
            // UpRight
            while (BoardManage.instance.detailBoxes.CheckOutOfRange(_slot.x + _slotX))
            {
                int _slotUpX = _slot.x + _slotX;
                int _slotUpY = _slot.y + _slotY;

                if (BoardManage.instance.detailBoxes[_slotUpX].boxManages.CheckOutOfRange(_slotUpY))
                {
                    BasePieces _piece = BoardManage.instance.detailBoxes[_slotUpX].boxManages[_slotUpY].GetPieces();
                    if (!_piece)
                    {
                        if (!isChanningEat)
                            _allSlot.Add(new Vector2Int(_slotUpX, _slotUpY));
                        _slotX++;
                        _slotY++;
                    }
                    else
                    {
                        int _lenghtX = _slot.x + (_slotX + 1);
                        int _lenghtY = _slot.y + (_slotY + 1);
                        if (!_piece.eSelectSide.Equals(_side) && BoardManage.instance.detailBoxes.CheckOutOfRange(_lenghtX))
                        {
                            if (BoardManage.instance.detailBoxes[_lenghtX].boxManages.CheckOutOfRange(_lenghtY))
                            {
                                if (BoardManage.instance.detailBoxes[_lenghtX].boxManages[_lenghtY].transform.childCount <= 0)
                                {
                                    BoardManage.instance.checkersManage.tempEatUpRight = _piece;
                                    _allSlot.Add(new Vector2Int(_lenghtX, _lenghtY));
                                }
                            }
                        }

                        break;
                    }
                }
                else
                    break;
            }

            _slotX = 1;
            _slotY = 1;
            // DownLeft
            while (BoardManage.instance.detailBoxes.CheckOutOfRange(_slot.x - _slotX))
            {
                int _slotUpX = _slot.x - _slotX;
                int _slotUpY = _slot.y - _slotY;

                if (BoardManage.instance.detailBoxes[_slotUpX].boxManages.CheckOutOfRange(_slotUpY))
                {
                    BasePieces _piece = BoardManage.instance.detailBoxes[_slotUpX].boxManages[_slotUpY].GetPieces();
                    if (!_piece)
                    {
                        if (!isChanningEat)
                            _allSlot.Add(new Vector2Int(_slotUpX, _slotUpY));

                        _slotX++;
                        _slotY++;
                    }
                    else
                    {
                        int _lenghtX = _slot.x - (_slotX + 1);
                        int _lenghtY = _slot.y - (_slotY + 1);
                        if (!_piece.eSelectSide.Equals(_side) && BoardManage.instance.detailBoxes.CheckOutOfRange(_lenghtX))
                        {
                            if (BoardManage.instance.detailBoxes[_lenghtX].boxManages.CheckOutOfRange(_lenghtY))
                            {
                                if (BoardManage.instance.detailBoxes[_lenghtX].boxManages[_lenghtY].transform.childCount <= 0)
                                {
                                    BoardManage.instance.checkersManage.tempEatDownLeft = _piece;
                                    _allSlot.Add(new Vector2Int(_lenghtX, _lenghtY));
                                }
                            }
                        }

                        break;
                    }
                }
                else
                    break;
            }
            _slotX = 1;
            _slotY = 1;
            // DownRight
            while (BoardManage.instance.detailBoxes.CheckOutOfRange(_slot.x - _slotX))
            {
                int _slotUpX = _slot.x - _slotX;
                int _slotUpY = _slot.y + _slotY;

                if (BoardManage.instance.detailBoxes[_slotUpX].boxManages.CheckOutOfRange(_slotUpY))
                {
                    BasePieces _piece = BoardManage.instance.detailBoxes[_slotUpX].boxManages[_slotUpY].GetPieces();
                    if (!_piece)
                    {
                        if (!isChanningEat)
                            _allSlot.Add(new Vector2Int(_slotUpX, _slotUpY));
                        _slotX++;
                        _slotY++;
                    }
                    else
                    {
                        int _lenghtX = _slot.x - (_slotX + 1);
                        int _lenghtY = _slot.y + (_slotY + 1);
                        if (!_piece.eSelectSide.Equals(_side) && BoardManage.instance.detailBoxes.CheckOutOfRange(_lenghtX))
                        {
                            if (BoardManage.instance.detailBoxes[_lenghtX].boxManages.CheckOutOfRange(_lenghtY))
                            {
                                if (BoardManage.instance.detailBoxes[_lenghtX].boxManages[_lenghtY].transform.childCount <= 0)
                                {
                                    BoardManage.instance.checkersManage.tempEatDownRight = _piece;
                                    _allSlot.Add(new Vector2Int(_lenghtX, _lenghtY));
                                }
                            }
                        }

                        break;
                    }
                }
                else
                    break;
            }

        }
    }
}
