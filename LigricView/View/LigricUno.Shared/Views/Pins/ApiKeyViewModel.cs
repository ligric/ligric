using LigricMvvmToolkit.BaseMvvm;

namespace LigricUno.Views.Pins
{
    public class ApiKeyViewModel : DispatchedBindableBase
    {
        private string _name, _publicKey, _privateKey;

        public string Name { get => _name; set => SetProperty(ref _name, value); }
        public string PublicKey { get => _publicKey; set => SetProperty(ref _publicKey, value); }
        public string PrivateKey { get => _privateKey; set => SetProperty(ref _privateKey, value); }
    }
}
