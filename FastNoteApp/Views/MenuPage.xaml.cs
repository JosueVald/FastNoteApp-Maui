namespace FastNoteApp.Views;

using FastNoteApp.Database;
using FastNoteApp.Models;

using CommunityToolkit.Maui.Views;

public partial class MenuPage : ContentPage
{
    public static MenuPage instance = null;

    public List<AppFolder> folderList { get; set; } // = new List<AppFolder>();

    AppFolder selectedfolder = null;
    int controlMenuCount = 0;

    public MenuPage()
	{
        instance = this;

        InitializeComponent();

        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        Reset();
    }


    public void Reset()
    {
        if (folderList != null) folderList.Clear();

        menuContent.ItemsSource = null;

        folderList = AppDatabase.Instance().GetFolderList();
        folderList.Add(new AppFolder("New Folder", "folder_plus1.png", ""));
        folderList.Add(new AppFolder("Edit Folder", "folder_account_outline.png", ""));
        //folderList.Add(new Folder("Theme", "theme.png"));

        controlMenuCount = 2;

        menuContent.ItemsSource = folderList;

        if (selectedfolder is null) selectedfolder = folderList[0];
    }

    public async void menuItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count != 0)
        {
            // Get the note model
            var folder = (AppFolder)e.CurrentSelection[0];

            if(folder.name == "New Folder")
            {
                string newFolderName = await DisplayPromptAsync("Fast Folder", "", "Ok", "Cancel");
                if(newFolderName != "" && newFolderName != null)
                {
                    menuContent.ItemsSource = null;

                    AppFolder newFolder = new AppFolder(newFolderName, "ic_folder_open_black.png");
                    AppDatabase.Instance().InsertFolder (newFolder);

                    //folderList.Insert(folderList.Count - controlMenuCount, newFolder);

                    Reset();

                    MainPage.instance.Reset(newFolder);
                }
                else
                {
                    menuContent.SelectedItem = null;
                }

                var flyoutPage = (FlyoutPage)Application.Current.MainPage;
                flyoutPage.IsPresented = false;

                return;
            }
            else if (folder.name == "Edit Folder")
            {
                if(selectedfolder is null)
                {
                    if(folderList.Count > controlMenuCount)
                    {
                        selectedfolder = folderList[0];
                    }
                    else
                    {
                        return;
                    }
                }

                menuContent.SelectedItem = selectedfolder;

                var popup = new EditFolderPopup();

                var result = await this.ShowPopupAsync(popup);
                if (result is string stringResult)
                {
                    if (stringResult == "rename")
                    {
                        string rename = await DisplayPromptAsync("Rename Folder", "", "Ok", "Cancel", selectedfolder.name);
                        if (rename != null && rename != "" && rename != selectedfolder.name)
                        {
                            selectedfolder.name = rename;

                            AppDatabase.Instance().UpdateFolder(selectedfolder);

                            menuContent.ItemsSource = null;
                            menuContent.ItemsSource = folderList;

                            MainPage.instance.Title = rename;
                        }
                    }
                    else if (stringResult == "delete")
                    {
                        AppDatabase.Instance().DeleteFolder(selectedfolder);

                        folderList.Remove(selectedfolder);
                        if (folderList.Count <= controlMenuCount)
                        {
                            AppDatabase.Instance().InsertFolder(new AppFolder("My Note", "ic_folder_special_black.png"));
                        }
                        
                        menuContent.ItemsSource = null;
                        menuContent.ItemsSource = folderList;
                        selectedfolder = folderList[0];

                        MainPage.instance.Reset(selectedfolder);
                    }
                    else
                    {
                        var flyoutPage = (FlyoutPage)Application.Current.MainPage;
                        flyoutPage.IsPresented = true;
                    }
                }

                menuContent.SelectedItem = null;
                return;
            }
            else
            {
                selectedfolder = folder;
                MainPage.instance.Reset(folder);

                var flyoutPage = (FlyoutPage)Application.Current.MainPage;
                flyoutPage.IsPresented = false;
            }
        }
    }
}