namespace BlackJackSL.Code
{
    using BlackJackSL.ViewModels;
    using BlackJackSL.Views;

    using Ninject.Modules;

    internal class Module : NinjectModule
    {
        public override void Load()
        {
            this.Bind<TableView>().ToSelf().InSingletonScope();
            this.Bind<TableViewModel>().ToSelf().InSingletonScope();

            this.Bind<ChatView>().ToSelf().InSingletonScope();
            this.Bind<ChatViewModel>().ToSelf().InSingletonScope();

            this.Bind<LoginView>().ToSelf().InSingletonScope();
            this.Bind<LoginViewModel>().ToSelf().InSingletonScope();

            this.Bind<PlayerCollectionView>().ToSelf().InSingletonScope();
            this.Bind<PlayerCollectionViewModel>().ToSelf().InSingletonScope();

            this.Bind<Shell>().ToSelf().InSingletonScope();

            this.Bind<DealerView>().ToSelf().InSingletonScope();
            this.Bind<DealerViewModel>().ToSelf().InSingletonScope();

            this.Bind<ClientComms>().ToSelf().InSingletonScope();

            //Bind<Deck>().ToSelf().Using<SingletonBehavior>();
        }
    }
}