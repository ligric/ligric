using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Ligric.UI.ViewModels.Data
{
	public class ApiDataViewModel : ReactiveObject
	{
		[Reactive] public string? Name { get; set; } = "My test api key";
		[Reactive] public string? PublicKey { get; set; } = "c58577a8b8d83617fb678838fa8e43c83e53384e88fef416c81658e51c6c48f3";
		[Reactive] public string? PrivateKey { get; set; } = "651096d67c3d1a080daf6d26a37ad545864d312b7a6b24d5f654d4f26a1a7ddc";
	}
}
