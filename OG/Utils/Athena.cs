using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace og.Utils
{
    public class Athena
    {
        [JsonProperty("profileChanges")]
        public ProfileChanges[] ProfileChanges { get; set; }
    }

    public class ProfileChanges
    {
        [JsonProperty("profile")]
        public Profile Profile { get; set; }
    }

    public class Profile
    {
        [JsonProperty("items")]
        public Items Items { get; set; }
    }

    public class Items
    {
        [JsonProperty("lawin-loadout")]
        public LawinLoadOut LawinLoadOut { get; set; }
    }

    public class LawinLoadOut
    {
        [JsonProperty("attributes")]
        public Attributes Attributes { get; set; }
    }

    public class Attributes
    {
        [JsonProperty("locker_slots_data")]
        public LockerSlots LockerSlotsData { get; set; }
    }

    public class LockerSlots
    {
        [JsonProperty("slots")]
        public Slots Slots { get; set; }
    }

    public class Slots
    {
        [JsonProperty("Character")]
        public Character Character { get; set; }
    }

    public class Character
    {
        [JsonProperty("items")]
        public string[] Items { get; set; }
    }
}
