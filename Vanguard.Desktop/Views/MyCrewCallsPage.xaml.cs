﻿namespace Vanguard.Desktop.Views;
public sealed partial class MyCrewCallsPage : Page
{
    public MyCrewCallsPageViewModel ViewModel { get; set; }

    public MyCrewCallsPage()
    {
        ViewModel = App.GetService<MyCrewCallsPageViewModel>();
        this.InitializeComponent();
    }
}