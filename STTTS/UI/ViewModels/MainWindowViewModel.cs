namespace STTTS.UI.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
	public MainModelViewModel MainModelVM { get; set; }

    public MainWindowViewModel()
	{
		MainModelVM = new MainModelViewModel();
	}
}
