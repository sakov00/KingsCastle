using _Project.Scripts;
using _Project.Scripts._VContainer;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Redactor.Scripts._VContainer
{
    public class RedactorLifetimeScope : GameLifetimeScope
    {
        protected override void RegisterGame(IContainerBuilder builder)
        {
            builder.Register<RedactorManager>(Lifetime.Singleton).AsSelf().As<IInitializable, IAsyncStartable, GameManager>();
        }
    }
}