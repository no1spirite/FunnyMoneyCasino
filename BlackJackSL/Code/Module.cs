using BlackJackSL.Model;
using BlackJackSL.ViewModels;
using BlackJackSL.Views;
using Ninject.Core;
using Ninject.Core.Behavior;

namespace BlackJackSL.Code
{
    class Module : StandardModule
    {
        public override void Load()
        {
            Bind<TableView>().ToSelf().Using<SingletonBehavior>();
            Bind<TableViewModel>().ToSelf().Using<SingletonBehavior>();

            Bind<ChatView>().ToSelf().Using<SingletonBehavior>();
            Bind<ChatViewModel>().ToSelf().Using<SingletonBehavior>();

            Bind<LoginView>().ToSelf().Using<SingletonBehavior>();
            Bind<LoginViewModel>().ToSelf().Using<SingletonBehavior>();

            Bind<PlayerCollectionView>().ToSelf().Using<SingletonBehavior>();
            Bind<PlayerCollectionViewModel>().ToSelf().Using<SingletonBehavior>();

            Bind<Shell>().ToSelf().Using<SingletonBehavior>();

            Bind<DealerView>().ToSelf().Using<SingletonBehavior>();
            Bind<DealerViewModel>().ToSelf().Using<SingletonBehavior>();

            Bind<ClientComms>().ToSelf().Using<SingletonBehavior>();

            //Bind<Deck>().ToSelf().Using<SingletonBehavior>();

        }
    }
}