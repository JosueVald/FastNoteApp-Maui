using CommunityToolkit.Maui.Views;

namespace FastNoteApp.Views;

public partial class EditFolderPopup : Popup
{
	public EditFolderPopup()
	{
		InitializeComponent();
	}

    void OnRenameClicked(object sender, EventArgs e) => Close("rename");

    void OnDeleteClicked(object sender, EventArgs e) => Close("delete");

    void OnCancelClicked(object sender, EventArgs e) => Close("cancel");
}