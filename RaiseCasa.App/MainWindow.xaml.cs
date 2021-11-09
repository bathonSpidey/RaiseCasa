using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace RaiseCasa.App
{
	/// <summary>
	///   Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly BridgeApi casa;
		private List<HueDevice> devices;

		public MainWindow()
		{
			casa = new BridgeApi("192.168.0.76", "nBdSwRzgzPMEixzmdFDFbxzUq869QTmDI3UXQPfP");
			InitializeComponent();
			DataContext = this;
			SetInitialStates();
		}

		private void SetInitialStates()
		{
			devices = Task.Run(() => casa.GetAllDevices()).Result;
			var spideyState = "";
			var currentBrightness = 127;
			var currentWarmness = 327;
			var kitchenState = "";
			var bathroomState = "";
			foreach (var device in devices)
			{
				if (device.Name.Contains("Spidey"))
				{
					spideyState = device.State.on.ToString();
					currentBrightness = device.State.bri;
					currentWarmness = device.State.ct;
				}
				else if (device.Name.Contains("Kitchen"))
				{
					kitchenState = device.State.on.ToString();
				}
				else if (device.Name.Contains("Bathroom"))
					bathroomState= device.State.on.ToString();
			}

			SpideyRoomState.Content = spideyState;
			Kitchen.Content = kitchenState;
			MainBathroom.Content = bathroomState;
			Brightness.Value = currentBrightness;
			Warmness.Value = currentWarmness;
			SpideyRoomState.Foreground = SpideyRoomState.Content.ToString() == "True"
				? Brushes.Green
				: Brushes.DarkRed;
			Kitchen.Foreground = Kitchen.Content.ToString() == "True"
				? Brushes.Green
				: Brushes.DarkRed;
			MainBathroom.Foreground = MainBathroom.Content.ToString() == "True"
				? Brushes.Green
				: Brushes.DarkRed;

		}

		private void SpideyRoomButtonClick(object sender, RoutedEventArgs e)
		{
			var light = Task.Run(() => casa.GetDevice("SpideyRoom")).Result;
			var state = !light.State.on;
			var _ = Task.Run(() => casa.SwitchGroup("Abir's Bedroom", state)).Result;
			SpideyRoomState.Content = state.ToString();
			SpideyRoomState.Foreground = SpideyRoomState.Content.ToString() == "True"
				? Brushes.Green
				: Brushes.DarkRed;
		}

		private async void ChangeSpideyRoomBrightness(object sender,
			MouseButtonEventArgs mouseButtonEventArgs)
		{
			var light = await casa.GetDevice("SpideyRoom");
			var hue = light.State.ct;
			await casa.SetGroupBrightnessAndWarmness("Abir's Bedroom", (int) Brightness.Value, hue);
			;
		}

		private async void ChangeSpideyRoomWarmness(object sender, MouseButtonEventArgs e)
		{
			var light = await casa.GetDevice("SpideyRoom");
			var brightness = light.State.bri;
			await casa.SetGroupBrightnessAndWarmness("Abir's Bedroom", brightness, (int)Warmness.Value);
		}

		private void KitchenButtonClick(object sender, RoutedEventArgs e)
		{
			var light = Task.Run(() => casa.GetDevice("Kitchen 1")).Result;
			var state = !light.State.on;
			var _ = Task.Run(() => casa.SwitchGroup("Kitchen", state)).Result;
			Kitchen.Content = state.ToString();
			Kitchen.Foreground = Kitchen.Content.ToString() == "True"
				? Brushes.Green
				: Brushes.DarkRed;
		}

		private void MainBathroomClick(object sender, RoutedEventArgs e)
		{
			var light = Task.Run(() => casa.GetDevice("Main Bathroom 1")).Result;
			var state = !light.State.on;
			var _ = Task.Run(() => casa.SwitchGroup("Main Bathroom", state)).Result;
			MainBathroom.Content = state.ToString();
			MainBathroom.Foreground = MainBathroom.Content.ToString() == "True"
				? Brushes.Green
				: Brushes.DarkRed;
		}
	}
}