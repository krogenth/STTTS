using System;
using Avalonia.Controls;
using FluentAvalonia.Core;
using FluentAvalonia.UI.Controls;

namespace STTTS.UI.Windows;

public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
		Load();
    }

	private void Load()
	{
		Pages.Children.Clear();
		NavPanel.SelectionChanged += NavPanelOnSelectionChanged;
		NavPanel.SelectedItem = NavPanel.MenuItems.ElementAt(0);
	}

	private void NavPanelOnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
	{
		if (e.SelectedItem is NavigationViewItem navigationViewItem && navigationViewItem.Tag is not null)
		{
			NavPanel.Content = navigationViewItem.Tag.ToString() switch
			{
				"AudioPage" => AudioPage,
				"VoskPage" => VoskPage,
				"VoicePage" => VoicePage,
				_ => throw new NotImplementedException(),
			};
		}
	}
}
