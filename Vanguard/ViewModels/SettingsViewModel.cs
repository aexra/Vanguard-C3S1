using Microsoft.UI.Xaml.Media.Animation;

namespace Vanguard.ViewModels;
public partial class SettingsViewModel : ObservableObject
{
    public IJsonNavigationViewService JsonNavigationViewService;
    public SettingsViewModel(IJsonNavigationViewService jsonNavigationViewService)
    {
        JsonNavigationViewService = jsonNavigationViewService;
    }

    [RelayCommand]
    private void GoToSettingPage(object sender)
    {
        var item = sender as SettingsCard;
        if (item.Tag != null)
        {
            Type pageType = Application.Current.GetType().Assembly.GetType($"Vanguard.Views.{item.Tag}");

            System.Diagnostics.Debug.WriteLine(pageType == null);

            if (pageType != null)
            {
                SlideNavigationTransitionInfo entranceNavigation = new SlideNavigationTransitionInfo();
                entranceNavigation.Effect = SlideNavigationTransitionEffect.FromRight;
                JsonNavigationViewService.NavigateTo(pageType, item.Header, false, entranceNavigation);
            }
        }
    }
}
