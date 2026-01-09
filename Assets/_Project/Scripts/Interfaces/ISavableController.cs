namespace _Project.Scripts.Interfaces
{
    public interface ISavableController : IInitializableAsync
    {
        public ISavableModel GetSavableModel();
        public void SetSavableModel(ISavableModel savableModel);
    }
}