using FastNoteApp.Database;

namespace FastNoteApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        AppDatabase.Instance().Init();

        MainPage = new AppFlyout();

        //MainPage = new AppShell();
    }
}
