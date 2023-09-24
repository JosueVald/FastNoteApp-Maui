namespace FastNoteApp.Views;

using FastNoteApp.Models;
using FastNoteApp.Database;

public partial class EditPage : ContentPage
{
    int selectedFolderID = 0;
    AppNote selectedNote = null;

    public EditPage(int folderID)
	{
		InitializeComponent();

        selectedFolderID = folderID;

        BindingContext = this;
    }
    public EditPage(AppNote note)
    {
        InitializeComponent();

        selectedNote = note;
        selectedFolderID = note.folderID;

        BindingContext = this;

        textEditor.Text = note.description;
    }

    protected override bool OnBackButtonPressed()
    {
        if (textEditor.Text == null || textEditor.Text == "") return false;

        if(selectedNote == null)
        {
            AppNote note = new AppNote();
            {
                note.folderID = selectedFolderID;
                note.description = textEditor.Text;
                note.dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            AppDatabase.Instance().InsertNote(note);
            MenuPage.instance.Reset();
        }
        else
        {
            if(selectedNote.description != textEditor.Text)
            {
                selectedNote.description = textEditor.Text;
                selectedNote.dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                AppDatabase.Instance().UpdateNote(selectedNote);
            }
        }

        Navigation.PopAsync();

        return true;
    }

    public void Delete_Clicked(object sender, EventArgs e)
    {
        if(selectedNote != null)
        {
            AppDatabase.Instance().DeleteNote(selectedNote);
            MenuPage.instance.Reset();

            Navigation.PopAsync();
        }
    }
}