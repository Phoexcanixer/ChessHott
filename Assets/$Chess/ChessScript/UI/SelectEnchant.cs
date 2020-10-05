namespace Chess
{
    using Chess.Pieces;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;

    public class SelectEnchant : MonoBehaviour
    {
        public EPieces ePieces;
        public bool isFirstSelect;
        Toggle _thisTog;
        void Awake() => _thisTog = GetComponent<Toggle>();
        void Start()
        {
            _thisTog.onValueChanged.AddListener((result) =>
            {
                if (_thisTog.isOn && isFirstSelect)
                {
                    ESelectStartSide selectStartColor = BoardManage.instance.presentPieces.eSelectSide;
                    var _remove = selectStartColor.Equals(ESelectStartSide.FirstSide) ? BoardManage.instance.chessManage.allPiecesFirstPlayers.Remove(BoardManage.instance.presentPieces) : BoardManage.instance.chessManage.allPiecesSecondPlayers.Remove(BoardManage.instance.presentPieces);
                    BoardManage.instance.presentTargetBox.ClearChild();
                    BasePieces _piece = Instantiate(SelectPieces(), BoardManage.instance.presentTargetBox.transform);
                    Destroy(_piece.GetComponent<Rigidbody>());
                    _piece.eSelectSide = selectStartColor;
                    _piece.transform.localPosition = Vector3.zero;
                    BoardManage.instance.presentPieces = _piece;

                    if (selectStartColor.Equals(ESelectStartSide.FirstSide))
                    {
                        _piece.meshRdr.material = BoardManage.instance.firstPlayerMat;
                        BoardManage.instance.chessManage.allPiecesFirstPlayers.Add(_piece);
                    }
                    else
                    {
                        _piece.meshRdr.material = BoardManage.instance.secondPlayerMat;
                        BoardManage.instance.chessManage.allPiecesSecondPlayers.Add(_piece);
                    }
                    EnchantPawn.CallBackSelectEnchant?.Invoke(false);
                }
                isFirstSelect = result;
            });
        }
        BasePieces SelectPieces()
        {
            BasePieces[] _allPieces = Resources.LoadAll<BasePieces>("Pieces");
            switch (ePieces)
            {
                case EPieces.Rook: return _allPieces.Where(item => item.ePieces.Equals(EPieces.Rook)).First();
                case EPieces.Knight: return _allPieces.Where(item => item.ePieces.Equals(EPieces.Knight)).First();
                case EPieces.Bishop: return _allPieces.Where(item => item.ePieces.Equals(EPieces.Bishop)).First();
                default: return _allPieces.Where(item => item.ePieces.Equals(EPieces.Queen)).First();
            }
        }

    }
}
