namespace FastNoteApp.Views;

using FastNoteApp.Models;
using FastNoteApp.Database;
using CommunityToolkit.Maui.Core.Primitives;

public partial class MainPage : ContentPage
{
    public static MainPage instance = null;

    public List<AppNote> noteList { get; set; }


    AppFolder selectedFolder;

    public MainPage()
	{
        instance = this;

        InitializeComponent();

        AppFolder folder = AppDatabase.Instance().GetFirstFolder();
        if (folder != null)
        {
            noteList = AppDatabase.Instance().GetNoteList(folder.id);
        }

        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        mainContent.ItemsSource = null;

        if (selectedFolder != null)
        {
            noteList = AppDatabase.Instance().GetNoteList(selectedFolder.id);
            this.Title = selectedFolder.name;
        }
        else
        {
            selectedFolder = AppDatabase.Instance().GetFirstFolder();
            if(selectedFolder != null)
            {
                noteList = AppDatabase.Instance().GetNoteList(selectedFolder.id);
                this.Title = selectedFolder.name;
            }
        }

        mainContent.ItemsSource = noteList;
    }

    public void Reset(AppFolder folder)
    {
        selectedFolder = folder;

        mainContent.ItemsSource = null;

        if (selectedFolder != null)
        {
            noteList = AppDatabase.Instance().GetNoteList(selectedFolder.id);
            this.Title = selectedFolder.name;
        }
        else
        {
            noteList = AppDatabase.Instance().GetNoteList(0);
        }

        mainContent.ItemsSource = noteList;
    }

    private void PlusButton_Clicked(object sender, EventArgs e)
    {
        if (selectedFolder == null) return;

        Navigation.PushAsync(new EditPage(selectedFolder.id));

        mainContent.SelectedItem = null;
    }

    public void mainContent_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count != 0)
        {
            // Get the note model
            var note = (AppNote)e.CurrentSelection[0];

            Navigation.PushAsync(new EditPage(note));

            // Unselect the UI
            mainContent.SelectedItem = null;
        }
    }

    public void Search_Clicked(object sender, EventArgs e)
    {
        //Navigation.PushAsync(new MenuPage());
    }
}

