using _Project.Scripts._VContainer;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.UI.Windows.BaseWindow
{
    public abstract class BaseWindowView : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup _canvasGroup;
        
        protected CompositeDisposable Disposables;
        protected bool _isShowed;

        protected Sequence _tweenShow;
        protected Sequence _tweenHide;
        
        public virtual void Initialize()
        {
            InjectManager.Inject(this);
            Disposables = new CompositeDisposable();
        }
        
        public virtual Tween Show()
        {
            if(_isShowed == true) return _tweenShow;
            _isShowed = true;
            
            _tweenShow = DOTween.Sequence();
            _tweenShow.AppendCallback(() => gameObject.SetActive(true));
            _tweenShow.Append(_canvasGroup.DOFade(1f, 0.5f).From(0));
            _tweenShow.SetUpdate(true);
            _tweenShow.OnComplete(() => _tweenShow = null);
            return _tweenShow;
        }

        public virtual Tween Hide()
        {
            if(_isShowed == false) return _tweenHide;
            _isShowed = false;
            
            _tweenHide = DOTween.Sequence();
            _tweenHide.Append(_canvasGroup.DOFade(0f, 0.5f).From(1));
            _tweenHide.AppendCallback(() => gameObject.SetActive(false));
            _tweenHide.SetUpdate(true);
            _tweenHide.OnComplete(() => _tweenHide = null);
            return _tweenHide;
        }
        
        public virtual void ShowFast()
        {
            _tweenShow?.Complete();
            _isShowed = true;
            gameObject.SetActive(true);
            _canvasGroup.alpha = 1f;
        }

        public virtual void HideFast()
        {
            _tweenHide?.Complete();
            _isShowed = false;
            gameObject.SetActive(false);
            _canvasGroup.alpha = 0;
        }
        
        public virtual void Dispose()
        {
            Disposables?.Dispose();
        }
    }
}