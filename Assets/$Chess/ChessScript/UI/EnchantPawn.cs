namespace Chess
{
    using Chess.Pieces;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class EnchantPawn : MonoBehaviour
    {
        public static Action<bool> CallBackSelectEnchant { get; set; }
        public static Action CallBackAutoEnchant { get; set; }
        public Toggle _baseTog;
        Canvas _thisCvs;
        void Awake()
        {
            _thisCvs = GetComponent<Canvas>();
            CallBackSelectEnchant = EnchantPawnSelect;
            CallBackAutoEnchant = EnchantAuto;
        }
        void EnchantAuto()
        {
            Debug.Log($"ENchance");
            BoardManage.instance.presentPieces.ePieces = EPieces.Hott;
            Instantiate(BoardManage.instance.presentPieces.meshRdr.gameObject, BoardManage.instance.presentPieces.transform);
            BoardManage.instance.presentPieces.meshRdr.transform.localPosition = new Vector2(0, 2);

            BoardManage.instance.presentPieces?.DefaulValue();
            BoardManage.instance.SwichTurn();
        }
        void EnchantPawnSelect(bool isShow)
        {
            _thisCvs.enabled = isShow;

            if (!isShow)
            {
                _baseTog.isOn = true;
                BoardManage.instance.presentPieces?.DefaulValue();
                BoardManage.instance.SwichTurn();
            }
        }
    }
}
