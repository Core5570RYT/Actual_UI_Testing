using System;
using System.IO;
using System.Windows.Forms;

// A simple File Explorer program i made using C#
// (Spoiler alert: i do not know how to use C#)
// -= Rehan P.S - X PPLG B =-
class Program
{
    static Form curWindow = new Form();
    static TabControl tabControl1 = new TabControl();
    static ListView listView = new ListView();
    static ImageList imageList = new ImageList();

    static TabPage filePage = GetTabPage("File");
    static TabPage toolsPage = GetTabPage("Tools");
    static TabPage aboutPage = GetTabPage("About");

    static Button goBackButton = new Button();

    static Stack<string> directoryHistory = new Stack<string>();

    static void Main(string[] args)
    {
        tabControl1.Dock = DockStyle.Fill;

        toolsPage.Controls.Add(GetLabelText("tooolsss :]\ni might add more to this, or maybe not, who knows lol"));
        Label temp = GetLabelText("A simple File Explorer program i made using C#\n(Spoiler alert: i do not know how to use C#)\n-= Rehan P.S - X PPLG B =-\n\n Thank you for using this app :D");
        temp.AutoSize = true;
        temp.TextAlign = ContentAlignment.MiddleCenter;
        aboutPage.Controls.Add(temp);

        temp.Anchor = AnchorStyles.None;
        temp.Location = new Point((aboutPage.Width - temp.Width) / 2, (aboutPage.Height - temp.Height) / 2);

        goBackButton.Text = "Back";
        goBackButton.Dock = DockStyle.Top;
        goBackButton.Click += (sender, e) => ReturnDirectory();

        listView.Dock = DockStyle.Fill;
        listView.View = View.LargeIcon;
        listView.SelectedIndexChanged += ListView_OnUpdateSelected;
        listView.DoubleClick += ListView_OnFileSelected;
        MakeListViewItems("C:\\");

        TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
        tableLayoutPanel.Dock = DockStyle.Fill;
        tableLayoutPanel.RowCount = 2;
        tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanel.Controls.Add(goBackButton, 0, 0);
        tableLayoutPanel.Controls.Add(listView, 0, 1);

        filePage.Controls.Add(tableLayoutPanel);

        tabControl1.TabPages.Add(filePage);
        tabControl1.TabPages.Add(toolsPage);
        tabControl1.TabPages.Add(aboutPage);

        curWindow.Controls.Add(tabControl1);
        curWindow.Text = "Basic Explorer - v0.0.1";
        curWindow.Size = new System.Drawing.Size(700, 400);
        curWindow.StartPosition = FormStartPosition.CenterScreen;

        ChangeFontToConsolas(curWindow);
        LoadIcons();
        listView.LargeImageList = imageList;

        Application.EnableVisualStyles();
        if (Environment.OSVersion.Version.Major >= 6) SetProcessDPIAware();
        Application.Run(curWindow);
    }

    static Label GetLabelText(String text)
    {
        Label label = new Label() { Text = text };
        label.AutoSize = true;
        return label;
    }

    static TabPage GetTabPage(String text)
    {
        TabPage tabPage = new TabPage(text);
        return tabPage;
    }

    static void MakeListViewItems(string directoryPath)
    {
        try
        {
            listView.Items.Clear();
            string[] directories = Directory.GetDirectories(directoryPath);
            foreach (string directory in directories)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                listView.Items.Add(directoryInfo.Name, 0);
            }

            string[] files = Directory.GetFiles(directoryPath);
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                listView.Items.Add(fileInfo.Name, 1);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message);
        }
        directoryHistory.Push(directoryPath);
    }

    static void ListView_OnUpdateSelected(object sender, EventArgs e)
    {
        string selectedDirectory = Path.Combine(directoryHistory.Peek(), listView.SelectedItems[0].Text);
        MakeListViewItems(selectedDirectory);
    }

    static void ListView_OnFileSelected(object sender, EventArgs e)
    {
        string selectedFile = Path.Combine(directoryHistory.Peek(), listView.SelectedItems[0].Text);
        if (File.Exists(selectedFile))
        {
            System.Diagnostics.Process.Start(selectedFile);
        }
    }

    static void ReturnDirectory()
    {
        if (directoryHistory.Count > 1)
        {
            directoryHistory.Pop();
            string previousDirectory = directoryHistory.Peek();
            MakeListViewItems(previousDirectory);
        }
    }

    static void ChangeFontToConsolas(Control control)
    {
        foreach (Control childControl in control.Controls)
        {
            ChangeFontToConsolas(childControl);
        }
    }

    static void LoadIcons()
    {
        try
        {
            Icon folderIcon = SystemIcons.GetStockIcon(StockIconId.Folder);
            Icon fileIcon = SystemIcons.WinLogo;

            Bitmap resizedFolderIcon = new Bitmap(folderIcon.ToBitmap(), new Size(64, 64));
            Bitmap resizedFileIcon = new Bitmap(fileIcon.ToBitmap(), new Size(64, 64));

            imageList.Images.Add("folder", resizedFolderIcon);
            imageList.Images.Add("file", resizedFileIcon);
            imageList.ImageSize = new Size(64, 64);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error loading icons: " + ex.Message);
        }
    }

    // stole this from my own source code :)
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    static extern bool SetProcessDPIAware();
}
