using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;
using STTTS.UI.Windows;

namespace STTTS.UI.ViewModels;

public class MenuBarViewModel : BaseViewModel
{
	public MenuBarViewModel()
	{
		OpenSettingsCommand = ReactiveCommand.Create(OpenSettings);
	}

	public IReadOnlyList<MenuItemViewModel> MenuItems { get; set; } = new List<MenuItemViewModel>();

	public ReactiveCommand<Unit, Unit> OpenSettingsCommand { get; }

	public void OpenSettings()
	{
		var window = new SettingsWindow()
		{
			DataContext = new Settings.SettingsWindowViewModel(),
		};
		window.Show();
	}
}
