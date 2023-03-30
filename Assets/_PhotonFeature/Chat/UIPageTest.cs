using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JSGCode.Internship.Chat;

public class UIPageTest : MonoBehaviour
{
    #region Member
    [SerializeField] private Button enterA_Btn;
    [SerializeField] private Button enterB_Btn;
    [SerializeField] private Button exitBtn;
    #endregion

    #region Method : Mono
    private void Awake()
    {
        enterA_Btn.onClick.AddListener(() => ChatProvider.Instance.Connect("ARoom", "Test1"));
        enterB_Btn.onClick.AddListener(() => ChatProvider.Instance.Connect("BRoom", "Test1"));
        exitBtn.onClick.AddListener(ChatProvider.Instance.DisConnect);
    }
    #endregion
}
