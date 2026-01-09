using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Interfaces
{
    public interface IInitializableAsync
    {
        UniTask InitializeAsync();
    }
}