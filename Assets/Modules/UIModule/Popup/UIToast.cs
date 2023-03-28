namespace JSGCode.UI
{
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using System;
    using System.Threading;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(CanvasGroup))]
    public class UIToast : UIBase
    {
        #region Constant
        private readonly float defaultFadeInTime = 1f;
        private readonly float defaultFadeOutTime = 0.5f;
        #endregion

        #region Members
        [SerializeField] private CanvasGroup group;
        [SerializeField] private Image backgroundImg;
        [SerializeField] private TextMeshProUGUI message;

        private RectTransform rectTrans;
        private float duration;
        private CancellationTokenSource fadeCancelToken;
        private Action finishCallback;
        #endregion

        #region Method : UI Base
        public override void Init()
        {
            typeID = PopupType.Toast.ToString();
            Active(false);

            group = group ?? GetComponent<CanvasGroup>();
            message = message ?? GetComponentInChildren<TextMeshProUGUI>();
            backgroundImg = backgroundImg ?? GetComponentInChildren<Image>();
            rectTrans = GetComponent<RectTransform>();

        }

        public override void Release()
        {
        }

        public override void Active(bool isActive)
        {
            backgroundImg?.gameObject.SetActive(isActive);
            message?.gameObject.SetActive(isActive);
        }
        #endregion

        #region Method : Controll Toast
        public void ShowToast(float duration, string msg, UIToastPosition position, System.Action finishCallback = null)
        {
            this.duration = duration;
            this.finishCallback = finishCallback;
            message.text = msg;
            SetPosition(position);
            FadeIn().Forget();
        }

        private void SetPosition(UIToastPosition position)
        {
            switch (position)
            {
                case UIToastPosition.Upper:
                    rectTrans.anchorMax = new Vector2(rectTrans.anchorMax.x, 0.8f);
                    rectTrans.anchorMin = new Vector2(rectTrans.anchorMin.x, 0.8f);
                    break;
                case UIToastPosition.Center:
                    rectTrans.anchorMax = new Vector2(rectTrans.anchorMax.x, 0.5f);
                    rectTrans.anchorMin = new Vector2(rectTrans.anchorMin.x, 0.5f);
                    break;
                case UIToastPosition.Lower:
                    rectTrans.anchorMax = new Vector2(rectTrans.anchorMax.x, 0.2f);
                    rectTrans.anchorMin = new Vector2(rectTrans.anchorMin.x, 0.2f);
                    break;
            }

            rectTrans.anchoredPosition = Vector2.zero;
            rectTrans.sizeDelta = new Vector2(0, 150f);
        }
        #endregion

        #region Implementation
        public async UniTask FadeIn()
        {
            fadeCancelToken?.Cancel();
            fadeCancelToken = new CancellationTokenSource();

            Active(true);

            try
            {
                var fadeInTween = group.DOFade(1.0f, duration == 0 ? defaultFadeInTime : duration / 2.0f).SetEase(Ease.OutQuad);
                fadeInTween.onComplete += () => FadeOut().Forget();

                await fadeInTween.WithCancellation(fadeCancelToken.Token);
            }
            catch (OperationCanceledException ex)
            {
                Debug.LogWarning(ex.Message);
            }
        }

        public async UniTask FadeOut()
        {
            fadeCancelToken?.Cancel();
            fadeCancelToken = new CancellationTokenSource();

            try
            {
                var fadeOutween = group.DOFade(0.0f, duration == 0 ? defaultFadeOutTime : duration / 2.0f).SetEase(Ease.OutQuad);
                fadeOutween.onComplete += () =>
                {
                    group.alpha = 0.0f;
                    Active(false);

                    finishCallback?.Invoke();
                    finishCallback = null;
                };

                await fadeOutween.WithCancellation(fadeCancelToken.Token);
            }
            catch (OperationCanceledException ex)
            {
                Debug.LogWarning(ex.Message);
            }
        }
        #endregion
    }
}