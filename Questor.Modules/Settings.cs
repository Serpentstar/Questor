﻿// ------------------------------------------------------------------------------
//   <copyright from='2010' to='2015' company='THEHACKERWITHIN.COM'> 
//     Copyright (c) TheHackerWithin.COM. All Rights Reserved.
// 
//     Please look in the accompanying license.htm file for the license that 
//     applies to this source code. (a copy can also be found at: 
//     http://www.thehackerwithin.com/license.htm)
//   </copyright>
// -------------------------------------------------------------------------------
namespace Questor.Modules
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Xml.Linq;

    public class Settings
    {
        /// <summary>
        ///   Singleton implementation
        /// </summary>
        public static Settings Instance = new Settings();

        private string _characterName;
        private DateTime _lastModifiedDate;

        public Settings()
        {
            Ammo = new List<Ammo>();
            Blacklist = new List<string>();
				FactionBlacklist = new List<string>();
        }

        public bool DebugStates { get; set; }
        public bool DebugPerformance { get; set; }

        public bool AutoStart { get; set; }
		  public bool waitDecline { get; set; }
        public int RandomDelay { get; set; }
		  public float minStandings { get; set; }

        public bool EnableStorylines { get; set; }

        public string CombatShipName { get; set; }
        public string SalvageShipName { get; set; }

        public string LootHangar { get; set; }
        public string AmmoHangar { get; set; }

        public bool CreateSalvageBookmarks { get; set; }
        public string BookmarkPrefix { get; set; }
        public int MinimumWreckCount { get; set; }
        public bool AfterMissionSalvaging { get; set; }
        public bool UnloadLootAtStation { get; set; }

        public string AgentName { get; set; }

        public string MissionsPath { get; set; }

        public int MaximumHighValueTargets { get; set; }
        public int MaximumLowValueTargets { get; set; }

        public List<Ammo> Ammo { get; private set; }

        public int MinimumAmmoCharges { get; set; }

        public int WeaponGroupId { get; set; }

        public int ReserveCargoCapacity { get; set; }

        public int MaximumWreckTargets { get; set; }

        public bool SpeedTank { get; set; }
        public int OrbitDistance { get; set; }

        public int ActivateRepairModules { get; set; }
        public int DeactivateRepairModules { get; set; }

        public int MinimumShieldPct { get; set; }
        public int MinimumArmorPct { get; set; }
        public int MinimumCapacitorPct { get; set; }
        public int SafeShieldPct { get; set; }
        public int SafeArmorPct { get; set; }
        public int SafeCapacitorPct { get; set; }

        public bool LootEverything { get; set; }

        public bool UseDrones { get; set; }
        public int DroneTypeId { get; set; }
        public int DroneControlRange { get; set; }
        public int DroneMinimumShieldPct { get; set; }
        public int DroneMinimumArmorPct { get; set; }
        public int DroneMinimumCapacitorPct { get; set; }
        public int DroneRecallShieldPct { get; set; }
        public int DroneRecallArmorPct { get; set; }
        public int DroneRecallCapacitorPct { get; set; }
        public int LongRangeDroneRecallShieldPct { get; set; }
        public int LongRangeDroneRecallArmorPct { get; set; }
        public int LongRangeDroneRecallCapacitorPct { get; set; }

        public List<string> Blacklist { get; private set; }
        public List<string> FactionBlacklist { get; private set; }

        public int? WindowXPosition { get; set; }
        public int? WindowYPosition { get; set; }
        public event EventHandler<EventArgs> SettingsLoaded;

        public void LoadSettings()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var settingsPath = Path.Combine(path, Cache.Instance.FilterPath(_characterName) + ".xml");

            var reloadSettings = _characterName != Cache.Instance.DirectEve.Me.Name;
            if (File.Exists(settingsPath))
                reloadSettings = _lastModifiedDate != File.GetLastWriteTime(settingsPath);

            if (!reloadSettings)
                return;

            _characterName = Cache.Instance.DirectEve.Me.Name;
            _lastModifiedDate = File.GetLastWriteTime(settingsPath);

            if (!File.Exists(settingsPath))
            {
                // Clear settings
                AgentName = string.Empty;

                AutoStart = false;
					 waitDecline = false;
                RandomDelay = 0;
					 minStandings = 10;

                WindowXPosition = null;
                WindowYPosition = null;

                LootHangar = string.Empty;
                AmmoHangar = string.Empty;

                MissionsPath = Path.Combine(path, "Missions");

                MaximumHighValueTargets = 0;
                MaximumLowValueTargets = 0;

                Ammo.Clear();

                MinimumAmmoCharges = 0;

                WeaponGroupId = 0;

                ReserveCargoCapacity = 0;

                MaximumWreckTargets = 0;

                SpeedTank = false;
                OrbitDistance = 0;

                ActivateRepairModules = 0;
                DeactivateRepairModules = 0;

                MinimumShieldPct = 0;
                MinimumArmorPct = 0;
                MinimumCapacitorPct = 0;
                SafeShieldPct = 0;
                SafeArmorPct = 0;
                SafeCapacitorPct = 0;

                UseDrones = false;
                DroneTypeId = 0;
                DroneControlRange = 0;
                DroneMinimumShieldPct = 0;
                DroneMinimumArmorPct = 0;
                DroneMinimumCapacitorPct = 0;
                DroneRecallCapacitorPct = 0;
                LongRangeDroneRecallCapacitorPct = 0;

                Blacklist.Clear();
					 FactionBlacklist.Clear();
                return;
            }

            var xml = XDocument.Load(settingsPath).Root;

            DebugStates = (bool?) xml.Element("debugStates") ?? false;
            DebugPerformance = (bool?) xml.Element("debugPerformance") ?? false;

            AutoStart = (bool?) xml.Element("autoStart") ?? false;
            waitDecline = (bool?) xml.Element("waitDecline") ?? false;
            RandomDelay = (int?) xml.Element("randomDelay") ?? 0;
			   minStandings = (float?) xml.Element("minStandings") ?? 10;

            EnableStorylines = (bool?) xml.Element("enableStorylines") ?? false;

            WindowXPosition = (int?) xml.Element("windowXPosition");
            WindowYPosition = (int?) xml.Element("windowYPosition");

            CombatShipName = (string) xml.Element("combatShipName");
            SalvageShipName = (string) xml.Element("salvageShipName");

            LootHangar = (string) xml.Element("lootHangar");
            AmmoHangar = (string) xml.Element("ammoHangar");

            CreateSalvageBookmarks = (bool?) xml.Element("createSalvageBookmarks") ?? false;
            BookmarkPrefix = (string) xml.Element("bookmarkPrefix") ?? "Salvage:";
            MinimumWreckCount = (int?) xml.Element("minimumWreckCount") ?? 1;
            AfterMissionSalvaging = (bool?) xml.Element("afterMissionSalvaging") ?? false;
            UnloadLootAtStation = (bool?) xml.Element("unloadLootAtStation") ?? false;

            AgentName = (string) xml.Element("agentName");

            var missionsPath = (string) xml.Element("missionsPath");
            MissionsPath = !string.IsNullOrEmpty(missionsPath) ? Path.Combine(path, missionsPath) : Path.Combine(path, "Missions");

            MaximumHighValueTargets = (int?) xml.Element("maximumHighValueTargets") ?? 2;
            MaximumLowValueTargets = (int?) xml.Element("maximumLowValueTargets") ?? 2;

            Ammo.Clear();
            var ammoTypes = xml.Element("ammoTypes");
            if (ammoTypes != null)
                foreach (var ammo in ammoTypes.Elements("ammoType"))
                    Ammo.Add(new Ammo(ammo));

            MinimumAmmoCharges = (int?) xml.Element("minimumAmmoCharges") ?? 0;

            WeaponGroupId = (int?) xml.Element("weaponGroupId") ?? 0;

            ReserveCargoCapacity = (int?) xml.Element("reserveCargoCapacity") ?? 0;

            MaximumWreckTargets = (int?) xml.Element("maximumWreckTargets") ?? 0;

            SpeedTank = (bool?) xml.Element("speedTank") ?? false;
            OrbitDistance = (int?) xml.Element("orbitDistance") ?? 0;

            ActivateRepairModules = (int?) xml.Element("activateRepairModules") ?? 65;
            DeactivateRepairModules = (int?) xml.Element("deactivateRepairModules") ?? 95;

            MinimumShieldPct = (int?) xml.Element("minimumShieldPct") ?? 100;
            MinimumArmorPct = (int?) xml.Element("minimumArmorPct") ?? 100;
            MinimumCapacitorPct = (int?) xml.Element("minimumCapacitorPct") ?? 50;
            SafeShieldPct = (int?)xml.Element("safeShieldPct") ?? 0;
            SafeArmorPct = (int?)xml.Element("safeArmorPct") ?? 0;
            SafeCapacitorPct = (int?)xml.Element("safeCapacitorPct") ?? 0;

            LootEverything = (bool?) xml.Element("lootEverything") ?? true;

            UseDrones = (bool?) xml.Element("useDrones") ?? true;
            DroneTypeId = (int?) xml.Element("droneTypeId") ?? 0;
            DroneControlRange = (int?) xml.Element("droneControlRange") ?? 0;
            DroneMinimumShieldPct = (int?) xml.Element("droneMinimumShieldPct") ?? 50;
            DroneMinimumArmorPct = (int?) xml.Element("droneMinimumArmorPct") ?? 50;
            DroneMinimumCapacitorPct = (int?) xml.Element("droneMinimumCapacitorPct") ?? 0;
            DroneRecallShieldPct = (int?) xml.Element("droneRecallShieldPct") ?? 0;
            DroneRecallArmorPct = (int?) xml.Element("droneRecallArmorPct") ?? 0;
            DroneRecallCapacitorPct = (int?) xml.Element("droneRecallCapacitorPct") ?? 0;
            LongRangeDroneRecallShieldPct = (int?) xml.Element("longRangeDroneRecallShieldPct") ?? 0;
            LongRangeDroneRecallArmorPct = (int?) xml.Element("longRangeDroneRecallArmorPct") ?? 0;
            LongRangeDroneRecallCapacitorPct = (int?) xml.Element("longRangeDroneRecallCapacitorPct") ?? 0;

            Blacklist.Clear();
            var blacklist = xml.Element("blacklist");
            if (blacklist != null)
                foreach (var mission in blacklist.Elements("mission"))
                    Blacklist.Add((string) mission);

            FactionBlacklist.Clear();
            var factionblacklist = xml.Element("factionblacklist");
            if (factionblacklist != null)
                foreach (var faction in factionblacklist.Elements("faction"))
                    FactionBlacklist.Add((string) faction);

            if (SettingsLoaded != null)
                SettingsLoaded(this, new EventArgs());
        }
    }
}