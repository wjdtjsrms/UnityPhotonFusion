using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JSGCode.Internship.Chat;

public class UIPageTest : MonoBehaviour
{
    #region Member
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button startChatBtn;
    #endregion

    #region Method : Mono
    private void Awake()
    {
        startChatBtn.onClick.AddListener(() => ChatProvider.Instance.Connect());
    }
    #endregion
}
