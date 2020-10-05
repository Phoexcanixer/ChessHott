namespace Chess
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class SideWin : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _resultTMP = null;
        [SerializeField] Button _replay = null;
        [SerializeField] Canvas _thisCvs = null;
        public static Action<ESelectStartSide> CallBackSideWin { get; set; }
        void Start()
        {
            CallBackSideWin = (result) =>
            {
                _thisCvs.enabled = true;
                _resultTMP.text = $"{result} Win";
            };
            _replay.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
        }

    }
}
