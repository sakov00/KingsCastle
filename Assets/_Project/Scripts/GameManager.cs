using System;
using System.Linq;
using System.Threading;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Additional.EnemyRoads;
using _Project.Scripts.Registries;
using _Project.Scripts.Services;
using _Project.Scripts.UI.Windows;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts
{
    public class GameManager : IInitializable, IAsyncStartable, IDisposable
    {
        [Inject] private SettingsService _settingsService;
        [Inject] private AppData _appData;
        [Inject] private SaveLoadLevelService _saveLoadLevelService;
        [Inject] private ResetLevelService _resetService;
        [Inject] private SceneCreator _sceneCreator;
        [Inject] private WindowsManager _windowsManager;
        [Inject] private SaveRegistry _saveRegistry;
        [Inject] private ApplicationEventsHandler _applicationEventsHandler;
        
        public virtual void Initialize()
        {
            Application.targetFrameRate = 120;
        }
        
        public virtual async UniTask StartAsync(CancellationToken cancellation = default)
        {
            // SoundManager.PlayMusicAsync(SoundKey.MenuMusic).Forget();
            // WindowsManager.ShowFastWindow<LoadingWindow>();
            // await StartLevel(AppData.User.CurrentLevel);
        }

        public virtual async UniTask ResetRound()
        {
            await _resetService.ResetLevel();
            await _sceneCreator.InstantiateObjects(_appData.LevelData.ObjectsForRestoring);
            _windowsManager.ShowFastWindow<GameWindow>();
        }

        public virtual async UniTask RestartLevel()
        {
            _saveLoadLevelService.RemoveProgress(_appData.User.CurrentLevel);
            await StartLevel(_appData.User.CurrentLevel);
        }

        public virtual async UniTask StartLevel(int levelIndex)
        {
            Dispose();
            
            Time.timeScale = 0;
            
            await UniTask.Delay(2000, DelayType.UnscaledDeltaTime);
            // await LoadLevel(levelIndex);
            //
            // var playerController = SaveRegistry.GetAllByType<UnitController>().First(x => x.UnitType == UnitType.Player);
            // GlobalObjects.CameraController.CameraFollow.Init(GlobalObjects.CameraController.transform, playerController.transform);
            //
            // if(AppData.LevelData.IsFighting) AppData.LevelEvents.Initialize();
            // ApplicationEventsHandler.OnApplicationQuited += OnApplicationQuit;
            // ApplicationEventsHandler.OnApplicationPaused += OnApplicationPause;
            
            Time.timeScale = 1;
        }
        
        public virtual async UniTask LoadLevel(int levelIndex, bool isInitialize = true)
        {
            // need return bool and handle(exists file or not)
            await _resetService.ResetLevel();
            await _saveLoadLevelService.LoadLevel(levelIndex);
            await _sceneCreator.InstantiateObjects(_appData.LevelData.SavableModels, isInitialize);
        }
        
        private void NextRoundOnClick()
        {
            _appData.LevelData.IsFighting = true;
            _appData.LevelData.ObjectsForRestoring = _saveRegistry.GetAll()
                .Select(o => o.GetSavableModel().GetSaveData()).ToList();
            
            _saveRegistry.GetAllByType<EnemyRoadController>().ForEach(x => x.StartSpawn());
            _appData.LevelEvents.Initialize();
        }
        
        private async UniTaskVoid WinHandle()
        {
            Dispose();
            _appData.LevelData.IsFighting = false;
            await _windowsManager.ShowWindow<WinWindow>();
            _windowsManager.HideFastWindow<GameWindow>();
        }

        private async UniTaskVoid FailHandle()
        {
            Dispose();
            _appData.LevelData.IsFighting = false;
            await _windowsManager.ShowWindow<FailWindow>();
            _windowsManager.HideFastWindow<GameWindow>();
        }
        
        private void OnApplicationQuit()
        {
            _saveLoadLevelService?.SaveLevelProgress(_appData.User.CurrentLevel).GetAwaiter().GetResult();
        }
        
        private void OnApplicationPause(bool pause)
        {
            if (pause)
                _saveLoadLevelService?.SaveLevelProgress(_appData.User.CurrentLevel).GetAwaiter().GetResult();
        }
        
        public void Dispose()
        {
            _applicationEventsHandler.OnApplicationQuited -= OnApplicationQuit;
            _applicationEventsHandler.OnApplicationPaused -= OnApplicationPause;
        }
    }
}