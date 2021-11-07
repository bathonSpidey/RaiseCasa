using System.Threading.Tasks;
using NUnit.Framework;

namespace RaiseCasa.Tests
{
	public class BridgeApiTests
	{
		private BridgeApi casa;

		[SetUp]
		public void CreateApi()
		{
			casa = new BridgeApi("192.168.0.76", "nBdSwRzgzPMEixzmdFDFbxzUq869QTmDI3UXQPfP");
		}

		[Test]
		public async Task GetAllDevices()
		{
			var devices = await casa.GetAllDevices();
			Assert.That(devices.Count, Is.GreaterThanOrEqualTo(14));
		}

		[Test]
		public async Task GetLightInfo()
		{
			var device = await casa.GetDevice("SpideyRoom");
			Assert.That(device.Id, Is.EqualTo(2));
			Assert.That(device.State.on, Is.False);
		}

		[Test]
		public async Task SwitchOnLight()
		{
			Assert.That((await casa.Switch("SpideyRoom", true)).Contains("success"));
		}

		[Test]
		public async Task SettingBrightnessToMid()
		{
			Assert.That((await casa.SetBrightness("SpideyRoom", 127)).Contains("success"));
		}

		[Test]
		public async Task SettingWarmnessToCold()
		{
			Assert.That((await casa.SetWarmness("SpideyRoom", 153)).Contains("success"));
		}

		[Test]
		public async Task SettingWarmnessToWarmAndBrightnessHigh()
		{
			Assert.That((await casa.SetBrightnessAndWarmness("SpideyRoom", 255, 327)).Contains("success"));
		}

		[Test]
		public async Task GetAllGroups()
		{
			var groups = await casa.GetAllGroups();
			Assert.That(groups.Count, Is.GreaterThanOrEqualTo(8));
		}

		[Test]
		public async Task GetGroupInfo()
		{
			var device = await casa.GetGroup("Abir's Bedroom");
			Assert.That(device.Id, Is.EqualTo(1));
			Assert.That(device.Action.on, Is.True);
		}


		[Test]
		public async Task SwitchGroupOff()
		{
			Assert.That((await casa.SwitchGroup("Kitchen", false)).Contains("success"));
		}

		[Test]
		public async Task SettingGroupBrightnessToMid()
		{
			Assert.That((await casa.SetGroupBrightness("Abir's Bedroom", 127)).Contains("success"));
		}

		[Test]
		public async Task SettingGroupWarmnessToCold()
		{
			Assert.That((await casa.SetGroupWarmness("Abir's Bedroom", 153)).Contains("success"));
		}

		[Test]
		public async Task SettingGroupWarmnessToWarmAndBrightnessHigh()
		{
			Assert.That((await casa.SetGroupBrightnessAndWarmness("Abir's Bedroom", 255, 327)).Contains("success"));
		}
	}
}