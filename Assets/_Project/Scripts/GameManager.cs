using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using _Project.Scripts._GlobalLogic;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract.Unit;
using _Project.Scripts.GameObjects.ActionSystems;
using _Project.Scripts.Registries;
using _Project.Scripts.Services;
using _Project.Scripts.UI.Windows;
using _Project.Scripts.UI.Windows.GameWindow;
using _Project.Scripts.UI.Windows.LoadingWindow;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts
{
    public class GameManager : IInitializable, IAsyncStartable, IDisposable
    {
        [Inject] protected SoundManager SoundManager;
        [Inject] protected AppData AppData;
        [Inject] protected SaveLoadLevelService SaveLoadLevelService;
        [Inject] protected ResetLevelService ResetService;
        [Inject] protected SceneCreator SceneCreator;
        [Inject] protected WindowsManager WindowsManager;
        [Inject] protected SaveRegistry SaveRegistry;
        [Inject] protected ApplicationEventsHandler ApplicationEventsHandler;
        
        public virtual void Initialize()
        {
            Application.targetFrameRate = 120;
            AppData.AppMode = AppMode.Game;
        }
        
        public virtual async UniTask StartAsync(CancellationToken cancellation = default)
        {
            // SoundManager.PlayMusicAsync(SoundKey.MenuMusic).Forget();
            // WindowsManager.ShowFastWindow<LoadingWindowPresenter>();
            // await StartLevel(AppData.User.CurrentLevel);
        }

        public virtual async UniTask ResetRound()
        {
            await ResetService.ResetLevel();
            await SceneCreator.InstantiateObjects(AppData.LevelData.ObjectsForRestoring);
            WindowsManager.ShowFastWindow<GameWindowPresenter>();
        }

        public virtual async UniTask RestartLevel()
        {
            SaveLoadLevelService.RemoveProgress(AppData.User.CurrentLevel);
            await StartLevel(AppData.User.CurrentLevel);
        }

        public virtual async UniTask StartLevel(int levelIndex)
        {
            Dispose();
            AppData.User.CurrentLevel = levelIndex;
            
            Time.timeScale = 0;
            await WindowsManager.ShowWindow<LoadingWindowPresenter>();
            WindowsManager.HideFastWindow<GameWindowPresenter>();
            
            await LoadLevel(levelIndex);

            var playerController = SaveRegistry.GetAllByType<UnitController>().First(x => x.UnitType == UnitType.Player);
            GlobalObjects.CameraController.CameraFollow.Init(GlobalObjects.CameraController.transform, playerController.transform);

            if(AppData.LevelData.IsFighting) AppData.LevelEvents.Initialize();
            // ApplicationEventsHandler.OnApplicationQuited += OnApplicationQuit;
            // ApplicationEventsHandler.OnApplicationPaused += OnApplicationPause;

            WindowsManager.ShowFastWindow<GameWindowPresenter>();
            await WindowsManager.HideWindow<LoadingWindowPresenter>();
            Time.timeScale = 1;
        }
        
        public virtual async UniTask LoadLevel(int levelIndex, bool isInitialize = true)
        {
            // need return bool and handle(exists file or not)
            await ResetService.ResetLevel();
            await SaveLoadLevelService.LoadLevel(levelIndex);
            await SceneCreator.InstantiateObjects(AppData.LevelData.SavableModels, isInitialize);
        }
        
        private void OnApplicationQuit()
        {
            SaveLoadLevelService?.SaveLevelProgress(AppData.User.CurrentLevel).GetAwaiter().GetResult();
        }
        
        private void OnApplicationPause(bool pause)
        {
            if (pause)
                SaveLoadLevelService?.SaveLevelProgress(AppData.User.CurrentLevel).GetAwaiter().GetResult();
        }
        
        public void Dispose()
        {
            ApplicationEventsHandler.OnApplicationQuited -= OnApplicationQuit;
            ApplicationEventsHandler.OnApplicationPaused -= OnApplicationPause;
        }
    }
}