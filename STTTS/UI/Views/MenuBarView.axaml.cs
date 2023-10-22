using Avalonia.Controls;
using STTTS.UI.ViewModels;
namespace STTTS.UI.Views;

public partial class MenuBarView : UserControl
{
    public MenuBarView()
    {
        InitializeComponent();
		var vm = new MenuBarViewModel();
		vm.MenuItems = new[]
		{
			new MenuItemViewModel
			{
				Header = "_File",
				Command = null,
				CommandParameter = null,
				Items = null,
			},
			new MenuItemViewModel
			{
				Header = "Options",
				Command = vm.OpenSettingsCommand,
				CommandParameter = null,
				Items = new[]
				{
					new MenuItemViewModel { Header = "_Settings" },
				},
			},
		};
		DataContext = vm;
    }
}
