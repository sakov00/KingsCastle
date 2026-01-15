using _Project.Scripts.UI.Windows.BaseWindow;
using _Project.Scripts.UI.Windows.LoadingWindow;
using UnityEngine;

namespace _Project.Scripts.UI.Windows.SettingsWindow
{
    public class UpgradeWindowPresenter : BaseWindowPresenter
    {
        [SerializeField] private BaseWindowModel _model;
        [SerializeField] private UpgradeWindowView _view;
        
        public override BaseWindowModel Model => _model;
        public override BaseWindowView View => _view;
    }
}