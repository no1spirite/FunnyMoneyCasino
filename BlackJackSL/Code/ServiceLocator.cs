using System.Collections.ObjectModel;
using BlackJackSL.Model;
using BlackJackSL.ViewModels;
using BlackJackSL.Views;
using Ninject.Core;

namespace BlackJackSL.Code
{
    public class ServiceLocator
    {
        private static readonly IKernel Kernel;

        static ServiceLocator()
        {
            if (Kernel == null)
                Kernel = new StandardKernel(new Module());
        }

        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }

        public TableView TableView
        {
            get { return Kernel.Get<TableView>(); }
        }

        public TableViewModel TableViewModel
        {
            get { return Kernel.Get<TableViewModel>(); }
        }

        public ChatView ChatView
        {
            get { return Kernel.Get<ChatView>(); }
        }

        public ChatViewModel ChatViewModel
        {
            get { return Kernel.Get<ChatViewModel>(); }
        }

        public LoginView LoginView
        {
            get { return Kernel.Get<LoginView>(); }
        }

        public LoginViewModel LoginViewModel
        {
            get { return Kernel.Get<LoginViewModel>(); }
        }

        public PlayerCollectionView PlayerCollectionView
        {
            get { return Kernel.Get<PlayerCollectionView>(); }
        }

        public PlayerCollectionViewModel PlayerCollectionViewModel
        {
            get { return Kernel.Get<PlayerCollectionViewModel>(); }
        }

        public Shell MainWindow
        {
            get { return Kernel.Get<Shell>(); }
        }

        public DealerView DealerView
        {
            get { return Kernel.Get<DealerView>(); }
        }

        public DealerViewModel DealerViewModel
        {
            get { return Kernel.Get<DealerViewModel>(); }
        }

        public ClientComms ClientComms
        {
            get { return Kernel.Get<ClientComms>(); }
        }

        //public Deck Deck
        //{
        //    get { return Kernel.Get<Deck>(); }
        //}
    }
}