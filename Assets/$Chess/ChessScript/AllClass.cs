using Chess.Table;
using System;
using UnityEngine;

#region Class&Struct
[Serializable]
public struct DetailBox
{
    public BoxManage[] boxManages;
}
#endregion

#region Enum
public enum ESelectStartSide { FirstSide = -1, SecondSide = 1 }
public enum EPieces { Pawn, Rook, Knight, Bishop, Queen, King, PawnHott, Hott }
public enum ESelectGame { Chase, Checkers }
#endregion
