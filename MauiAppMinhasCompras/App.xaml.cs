using MauiAppMinhasCompras.Helpers;

namespace MauiAppMinhasCompras
{
    public partial class App : Application
    {
        static SQLiteDatabaseHelper _db; // É estatico pois pertence a classe, e nao uma instancia especifica

        public static SQLiteDatabaseHelper Db
        {
            get
            {
                if (_db == null) // verifica se a instancia de _db ainda nao foi criada. se nao, inicia o if
                {
                    string path = Path.Combine( //gera o caminho do arquivo do banco de dados.
                        Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData), // retorna o caminho para a pasta de dados locais da aplicacao
                        "banco_sqlite_compras.db3"); // nome do arquivo do banco de dados sqlite. o arquivo é criado ou aberto nesse local

                    _db = new SQLiteDatabaseHelper(path); // cria uma nova instancia da classe sqlitedatabasehelper,
                                                          // passando o caminho do banco de dados. essa instancia sera armazenada na variavel _db.
                                                          // Se a instância já tiver sido criada anteriormente, a verificação if (_db == null) não entrará
                                                          // no bloco de código e o código simplesmente retornará a instância existente de _db.
                }
                return _db;
            }
        }
        public App()
        {
            InitializeComponent();

            //MainPage = new AppShell();
            MainPage = new NavigationPage(new Views.ListaProduto());
        }
        protected override Window CreateWindow(IActivationState? activationState) // redimensionar a tela
        {
            var window = base.CreateWindow(activationState);

            window.Width = 700;
            window.Height = 750;

            return window;
        }
    }
}
