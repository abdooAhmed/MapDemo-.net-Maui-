using MapDemo.ViewModels;
using Map = Microsoft.Maui.Controls.Maps.Map;
namespace MapDemo.Views;

public partial class MapView : ContentPage
{
    MapViewModel vm;
	public MapView(MapViewModel vm)
	{
		InitializeComponent();
        this.vm = vm;
        BindingContext = vm;
    }

    protected override void OnDisappearing()
    {
        vm.DisposeCancellationTokenCommand.Execute(null);

        base.OnDisappearing();
    }
}