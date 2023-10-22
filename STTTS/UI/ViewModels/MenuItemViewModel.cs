using System.Collections.Generic;
using System.Windows.Input;

namespace STTTS.UI.ViewModels;

public class MenuItemViewModel
{
	public string Header { get; set; } = string.Empty;
	public ICommand? Command { get; set; }
	public object? CommandParameter { get; set; }
	public IList<MenuItemViewModel>? Items { get; set; }
}
