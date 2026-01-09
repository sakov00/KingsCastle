using _Project.Scripts.UI.Windows.BaseWindow;
using UnityEngine;

namespace _Project.Scripts.UI.Windows.LoadingWindow
{
    public class LoadingWindowPresenter : BaseWindowPresenter
    {
        [SerializeField] private BaseWindowModel _model;
        [SerializeField] private LoadingWindowView _view;
        
        public override BaseWindowModel Model => _model;
        public override BaseWindowView View => _view;
    }
}