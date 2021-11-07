using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RaiseCasa
{
	public class BridgeApi
	{
		private readonly HttpClient client;
		private readonly string ip;
		private readonly string username;

		public BridgeApi(string ip, string username)
		{
			this.ip = ip;
			this.username = username;
			client = new HttpClient();
		}

		public async Task<List<HueDevice>> GetAllDevices()
		{
			var response = await client.GetStringAsync($"http://{ip}/api/{username}/lights");
			var devices = new List<HueDevice>();
			var jObject = JObject.Parse(response);
			foreach (var entry in jObject)
			{
				devices.Add(JsonConvert.DeserializeObject<HueDevice>(entry.Value!.ToString()));
				int.TryParse(entry.Key, out var value);
				devices[^1].Id = value;
			}

			return devices;
		}

		public async Task<HueDevice> GetDevice(string name)
		{
			var devices = await GetAllDevices();
			return devices.FirstOrDefault(x => x.Name == name);
		}

		public async Task<string> Switch(string name, bool state)
		{
			var json = JsonConvert.SerializeObject(new State {on = state});
			var response = await HttpResponseMessage(name, json);
			return await response.Content.ReadAsStringAsync();
		}

		private async Task<HttpResponseMessage> HttpResponseMessage(string name, string json)
		{
			var data = new StringContent(json, Encoding.UTF8, "application/json");
			var url = $"http://{ip}/api/{username}/lights/{(await GetDevice(name)).Id}/state";
			var response = await client.PutAsync(url, data);
			return response;
		}

		public async Task<string> SetBrightness(string name, int brightness)
		{
			var json = JsonConvert.SerializeObject(new State
			{
				on = true,
				bri = brightness
			});
			var response = await HttpResponseMessage(name, json);
			return await response.Content.ReadAsStringAsync();
		}

		public async Task<string> SetWarmness(string name, int warmness)
		{
			var json = JsonConvert.SerializeObject(new State
			{
				on = true,
				ct = warmness
			});
			var response = await HttpResponseMessage(name, json);
			return await response.Content.ReadAsStringAsync();
		}

		public async Task<string> SetBrightnessAndWarmness(string name, int brightness, int warmness)
		{
			var json = JsonConvert.SerializeObject(new State
			{
				on = true,
				bri = brightness,
				ct = warmness
			});
			var response = await HttpResponseMessage(name, json);
			return await response.Content.ReadAsStringAsync();
		}


		public async Task<List<HueGroup>> GetAllGroups()
		{
			var response = await client.GetStringAsync($"http://{ip}/api/{username}/groups");
			var devices = new List<HueGroup>();
			var jObject = JObject.Parse(response);
			foreach (var entry in jObject)
			{
				devices.Add(JsonConvert.DeserializeObject<HueGroup>(entry.Value!.ToString()));
				int.TryParse(entry.Key, out var value);
				devices[^1].Id = value;
			}

			return devices;
		}

		public async Task<HueGroup> GetGroup(string name)
		{
			var groups = await GetAllGroups();
			return groups.FirstOrDefault(x => x.Name == name);
		}

		public async Task<string> SwitchGroup(string name, bool state)
		{
			var json = JsonConvert.SerializeObject(new State {on = state});
			var response = await HttpGroupResponseMessage(name, json);
			return await response.Content.ReadAsStringAsync();
		}

		private async Task<HttpResponseMessage> HttpGroupResponseMessage(string name, string json)
		{
			var data = new StringContent(json, Encoding.UTF8, "application/json");
			var url = $"http://{ip}/api/{username}/groups/{(await GetGroup(name)).Id}/action";
			var response = await client.PutAsync(url, data);
			return response;
		}

		public async Task<string> SetGroupBrightness(string name, int brightness)
		{
			var json = JsonConvert.SerializeObject(new State
			{
				on = true,
				bri = brightness
			});
			var response = await HttpGroupResponseMessage(name, json);
			return await response.Content.ReadAsStringAsync();
		}

		public async Task<string> SetGroupWarmness(string name, int warmness)
		{
			var json = JsonConvert.SerializeObject(new State
			{
				on = true,
				ct = warmness
			});
			var response = await HttpGroupResponseMessage(name, json);
			return await response.Content.ReadAsStringAsync();
		}

		public async Task<string> SetGroupBrightnessAndWarmness(string name, int brightness, int warmness)
		{
			var json = JsonConvert.SerializeObject(new State
			{
				on = true,
				bri = brightness,
				ct = warmness
			});
			var response = await HttpGroupResponseMessage(name, json);
			return await response.Content.ReadAsStringAsync();
		}
	}
}