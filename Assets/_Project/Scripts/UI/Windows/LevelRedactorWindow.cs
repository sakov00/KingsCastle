using _Redactor.Scripts;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Project.Scripts.UI.Windows
{
    public class LevelRedactorWindow : BaseWindow
    {
        [Inject] private RedactorManager _redactorManager;
        
        [Header("Dev")]
        [SerializeField] private Button _saveLevelButton;
        [SerializeField] private Button _loadLevelButton;
        [SerializeField] private Button _playLevelButton;
        [SerializeField] private TMP_InputField _selectLevelInputField;

        protected override void Awake()
        {
            base.Awake();
            
            _saveLevelButton.gameObject.SetActive(true);
            _loadLevelButton.gameObject.SetActive(true);
            _selectLevelInputField.gameObject.SetActive(true);
            
            _saveLevelButton.OnClickAsObservable()
                .Subscribe(_ => _redactorManager.SaveLevel(int.Parse(_selectLevelInputField.text))).AddTo(Disposables);
            _loadLevelButton.OnClickAsObservable()
                .Subscribe(_ => _redactorManager.LoadLevel(int.Parse(_selectLevelInputField.text), false).Forget()).AddTo(Disposables);
            _playLevelButton.OnClickAsObservable()
                .Subscribe(_ => _redactorManager.StartLevel(int.Parse(_selectLevelInputField.text)).Forget()).AddTo(Disposables);
        }
    }
}