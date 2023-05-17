using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Ligric.UI.ViewModels.Data
{
	public class ApiClientViewModel : ReactiveObject
	{
		[Reactive] public long? UserApiId { get; set; }
		[Reactive] public string? Name { get; set; }
		[Reactive] public int? Permissions { get; set; }
	}
}
