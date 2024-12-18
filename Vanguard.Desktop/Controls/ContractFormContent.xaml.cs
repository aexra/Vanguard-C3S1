using Vanguard.Desktop.ViewModels.Controls;

namespace Vanguard.Desktop.Controls;
public sealed partial class ContractFormContent : UserControl
{
    public ContractFormContentViewModel ViewModel { get; set; }

    public ContractFormContent()
    {
        ViewModel = App.GetService<ContractFormContentViewModel>();
        this.InitializeComponent();
    }
}
