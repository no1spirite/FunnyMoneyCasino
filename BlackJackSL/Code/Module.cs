using BlackJackSL.Model;
using BlackJackSL.ViewModels;
using BlackJackSL.Views;

namespace BlackJackSL.Code
{
    using Ninject.Modules;

    class Module : NinjectModule
    {
        public override void Load()
        {
            Bind<TableView>().ToSelf().InSingletonScope();
            Bind<TableViewModel>().ToSelf().InSingletonScope();

            Bind<ChatView>().ToSelf().InSingletonScope();
            Bind<ChatViewModel>().ToSelf().InSingletonScope();

            Bind<LoginView>().ToSelf().InSingletonScope();
            Bind<LoginViewModel>().ToSelf().InSingletonScope();

            Bind<PlayerCollectionView>().ToSelf().InSingletonScope();
            Bind<PlayerCollectionViewModel>().ToSelf().InSingletonScope();

            Bind<Shell>().ToSelf().InSingletonScope();

            Bind<DealerView>().ToSelf().InSingletonScope();
            Bind<DealerViewModel>().ToSelf().InSingletonScope();

            Bind<ClientComms>().ToSelf().InSingletonScope();

            //Bind<Deck>().ToSelf().Using<SingletonBehavior>();

        }
    }
}