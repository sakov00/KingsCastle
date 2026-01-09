using DG.Tweening;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.UI.Windows.BaseWindow
{
    public abstract class BaseWindowView : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup _canvasGroup;
        
        protected CompositeDisposable Disposables;
        
        public virtual void Initialize()
        {
            Disposables = new CompositeDisposable();
        }
        
        public virtual Tween Show()
        {
            var sequence = DOTween.Sequence();
            sequence.AppendCallback(() => gameObject.SetActive(true));
            sequence.Append(_canvasGroup.DOFade(1f, 0.5f).From(0));
            sequence.SetUpdate(true);
            return sequence;
        }

        public virtual Tween Hide()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(0f, 0.5f).From(1));
            sequence.AppendCallback(() => gameObject.SetActive(false));
            sequence.SetUpdate(true);
            return sequence;
        }
        
        public virtual void ShowFast()
        {
            gameObject.SetActive(true);
            _canvasGroup.alpha = 1f;
        }

        public virtual void HideFast()
        {
            gameObject.SetActive(false);
            _canvasGroup.alpha = 0;
        }
        
        public virtual void Dispose()
        {
            Disposables?.Dispose();
        }
    }
}