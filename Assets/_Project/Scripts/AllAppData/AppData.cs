using _Project.Scripts._VContainer;
using _Project.Scripts.Enums;
using VContainer.Unity;

namespace _Project.Scripts.AllAppData
{
    public class AppData : IInitializable
    {
        public User User { get; private set; }
        public LevelData LevelData { get; set; }
        public LevelEvents LevelEvents { get; private set; }

        public void Initialize()
        {
            User = new User();
            LevelData = new LevelData();
            LevelEvents = new LevelEvents();
            InjectManager.Inject(LevelEvents);
        }
    }
}