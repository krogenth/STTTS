using System;
using Avalonia.Controls;
using FluentAvalonia.Core;
using FluentAvalonia.UI.Controls;
using STTTS.UI.ViewModels.Settings;

namespace STTTS.UI.Windows;

public partial class SettingsWindow : Window
{
    public SettingsWindow(SettingsWindowViewModel viewModel)
	{
		DataContext = viewModel;
		viewModel.CloseWindow += Close;

		InitializeComponent();
		Load();
    }

	public SettingsWindow()
	{
		DataContext = new SettingsWindowViewModel();
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
				"RecognizerPage" => RecognizerPage,
				"VoskPage" => VoskPage,
				"WhisperPage" => WhisperPage,
				"VoicePage" => VoicePage,
				_ => throw new NotImplementedException(),
			};
		}
	}
}
