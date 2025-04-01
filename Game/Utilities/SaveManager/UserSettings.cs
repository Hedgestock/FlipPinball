using Godot;

namespace Wafflestock
{
    public class UserSettings
    {
        public static void Save(string path)
        {
            ConfigFile config = new();

            for (int bus = 0; bus < AudioServer.BusCount; bus++)
            {
                config.SetValue("Audio", AudioServer.GetBusName(bus) + "IsMuted", AudioServer.IsBusMute(bus));
                config.SetValue("Audio", AudioServer.GetBusName(bus) + "Volume", AudioServer.GetBusVolumeLinear(bus));
            }

            config.Save(path);
        }

        public static bool Load(string path)
        {
            ConfigFile config = new();

            Error err = config.Load(path);

            if (err != Error.Ok) return false;

            for (int bus = 0; bus < AudioServer.BusCount; bus++)
            {
                AudioServer.SetBusMute(bus, (bool)config.GetValue("Audio", AudioServer.GetBusName(bus) + "IsMuted", false));
                AudioServer.SetBusVolumeLinear(bus, (float)config.GetValue("Audio", AudioServer.GetBusName(bus) + "Volume", 1));
            }

            return true;
        }
    }
}