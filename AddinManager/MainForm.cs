using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraEditors;

namespace AddinManager
{
    public partial class MainForm : XtraForm
    {
        IWizardViewModel wizardViewModel;
        public MainForm()
        {
            InitializeComponent();
            wizardViewModel = new ViewModels.WizardViewModel(
                new IWizardPageViewModel[]{
                    new ViewModels.StartPageViewModel(),
                    new ViewModels.OptionsPageViewModel(),
                    new ViewModels.InstallPageViewModel(),
                    new ViewModels.FinishPageViewModel() 
                },
                windowsUIView1, this);

            windowsUIView1.AddDocument(new ucStartPage() { Text = "Start" });
            windowsUIView1.AddDocument(new ucOptionsPage() { Text = "Options" });
            windowsUIView1.AddDocument(new ucInstallPage() { Text = "Install" });
            windowsUIView1.AddDocument(new ucFinishPage() { Text = "Finish" });

            foreach (Document document in windowsUIView1.Documents)
                pageGroup.Items.Add(document);
        }
        void windowsUIView1_NavigationBarsShowing(object sender, NavigationBarsCancelEventArgs e)
        {
            e.Cancel = true;
        }
        void windowsUIView1_NavigatedTo(object sender, NavigationEventArgs e)
        {
            e.Parameter = wizardViewModel;
        }
        void windowsUIView1_QueryDocumentActions(object sender, QueryDocumentActionsEventArgs e)
        {
            e.DocumentActions.Add(new DocumentAction(
                (document) => wizardViewModel.CanPrev(),
                (document) => wizardViewModel.Prev()) { Caption = "Back", Image = imageList1.Images[0] });
            e.DocumentActions.Add(new DocumentAction(
                (document) => wizardViewModel.CanNext(),
                (document) => wizardViewModel.Next()) { Caption = "Next", Image = imageList1.Images[1] });
            e.DocumentActions.Add(new DocumentAction(
                (document) => wizardViewModel.CanClose(),
                (document) => wizardViewModel.Close(true)) { Caption = "Exit", Image = imageList1.Images[2] });
        }
    }
}