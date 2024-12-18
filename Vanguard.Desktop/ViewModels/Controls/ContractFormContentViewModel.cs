using Vanguard.DataAccess.Models;

namespace Vanguard.Desktop.ViewModels.Controls;
public partial class ContractFormContentViewModel : ObservableObject
{
    [ObservableProperty]
    private string _contentTitle;

    [ObservableProperty]
    private bool _isLegalEntity;

    [ObservableProperty]
    private string _id;

    [ObservableProperty]
    private string _address;

    [ObservableProperty]
    private string _price;

    [ObservableProperty]
    private string _comment;

    public Contract? ToContract()
    {
        if (int.TryParse(Id, out var parsedId) && double.TryParse(Price, out var parsedPrice))
        {
            var c = new Contract();

            if (IsLegalEntity) c.OrganizationId = parsedId;
            else c.OwnerId = parsedId;

            c.IsLegalEntity = IsLegalEntity;
            c.SignDate = DateTime.UtcNow;
            c.Address = Address;
            c.Price = parsedPrice;
            c.Comment = Comment;

            return c;
        }

        return null;
    }

    public void FromContract(Contract c)
    {
        IsLegalEntity = c.IsLegalEntity;
        Id = (IsLegalEntity ? c.OrganizationId : c.OwnerId).ToString();
        Address = c.Address;
        Price = c.Price.ToString();
        Comment = c.Comment;
    }
}
