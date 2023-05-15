using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Ligric.UI.ViewModels.Data
{
	public class ApiDataViewModel : ReactiveObject
	{
		[Reactive] public string? Name { get; set; }
		[Reactive] public string? PublicKey { get; set; }
		[Reactive] public string? PrivateKey { get; set; }
	}
}
