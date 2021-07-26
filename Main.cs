//+--------------------------------------------+
//| Author: Sopphic                                |
//+--------------------------------------------+

using LSPD_First_Response.Mod.API;
using Rage;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StopRightThere
{

    public class Main : Plugin
    {
        static Keys key1 = new Keys();
        static Keys key2 = new Keys();
        private static InitializationFile Reader { get; set; }
        //In this method, we load up the .ini file so other methods can use it.
        private static void InitialiseFile()
        {
            Game.LogTrivial("[StopRightThere] [initialization] INFO: Attempting to load StopRightThere.ini");
            Reader = new InitializationFile("Plugins/LSPDFR/StopRightThere.ini");
            Reader.Create();
            Game.LogTrivial("[StopRightThere] [initialization] INFO: ini file loaded successfully");
        }
        internal static bool getSettings()
        {
            InitialiseFile();

            KeysConverter kc = new KeysConverter();
            List<Keys> keyList = new List<Keys>();
            int settingsCounter = 2;      //Indicates how many bools are needed for for the settings in order to work.
            List<bool> successfullySetup = new List<bool>(); for (int i = 0; i < settingsCounter; i++) { successfullySetup.Add(false); }
            bool settingsOK;
            try
            {
                Game.LogTrivial("[StopRightThere] [initialization] INFO: Attempt to read settings");
                try { Main.key1 = (Keys)kc.ConvertFromString(Reader.ReadString("Keybindings", "PulloverKey", "P")); successfullySetup[0] = true; } catch { Game.LogTrivial($"[StopRightThere] [initialization] WARNING: Couldn't read PulloverKey"); }
                try { Main.key2 = (Keys)kc.ConvertFromString(Reader.ReadString("Keybindings", "ModifierKey", "LControlKey")); successfullySetup[1] = true; } catch { Game.LogTrivial($"[StopRightThere] [initialization] WARNING: Couldn't read ModifierKey"); }
                bool allOK = true;
                foreach (bool ok in successfullySetup) { if (ok == false) allOK = false; }

                if (allOK)
                {
                    Game.LogTrivial($"[StopRightThere] [initialization] INFO: Successfully read settings");
                    settingsOK = true;
                }
                else
                {
                    Game.LogTrivial($"[StopRightThere] [initialization] WARNING: Reading settings ERROR");
                    settingsOK = false;
                }

            }
            catch (System.Threading.ThreadAbortException) { settingsOK = false; }
            catch (Exception e)
            {
                Game.LogTrivial($"[StopRightThere] [initialization] WARNING: Fatal error reading StopRightThere settings from StopRightThere.ini, make sure your preferred settings are valid. Applying all default settings!");
                Game.LogTrivial($"[StopRightThere] [initialization] WARNING: error MSG{e}");
                settingsOK = false;

                Main.key1 = Keys.P;
                Main.key2 = Keys.LControlKey;
            }

            if (!settingsOK)
            {
                if (successfullySetup[0] == false) Main.key1 = Keys.P; ;
                if (successfullySetup[1] == false) Main.key2 = Keys.LControlKey;
            }


            Game.LogTrivial("[StopRightThere] [initialization] INFO: Setting: PulloverKey = " + Main.key1);
            Game.LogTrivial("[StopRightThere] [initialization] INFO: Setting: ModifierKey = " + Main.key2);

            return settingsOK;
        }

        //Initialization of the plugin.
        public override void Initialize()
        {
            //This is saying that when our OnDuty status is changed, it calls for the code to call private static void OnOnDutyStateChangedHandler near the bottom of this page.
            Functions.OnOnDutyStateChanged += OnOnDutyStateChangedHandler;

            //Game.LogTrivial's are used to help you identify problems when debugging a crash with your plugin, so you know exactly what's going on when.

            //This will show in the RagePluginHook.log as "Vehicle Search Plugin 2.0.0.0 has been initialised." 
            Game.LogTrivial("[StopRightThere] INFO: StopRightThere " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " has been initialised.");
            getSettings();
            //This one will show in RagePluginHook.log as "Go on duty to fully load Example Plugin."
            //Game.LogTrivial("Go on duty to fully load Example Plugin.");
        }
        //This is a simple message saying that Example Plugin has been cleanup.
        public override void Finally()
        {

        }
        //This is called by Functions.OnOnDutyStateChanged as stated above, but only when bool OnDuty is true.
        private static void OnOnDutyStateChangedHandler(bool OnDuty)
        {

            //We have to make sure they are actually on duty so the code can do its work, so we use an "if" statement.
            if (OnDuty)
            {

                Game.LogTrivial("[StopRightThere] Info: Initialized");
                Game.DisplayNotification("~y~StopRightThere~w~: \nInitialized");

                GameFiber.StartNew(delegate
                {
                    while (OnDuty) {
                        try
                        {
                            KeyboardState keyboard = new KeyboardState();
                            if (keyboard.IsDown(key1) && keyboard.IsDown(key2) && Game.LocalPlayer.Character.IsInAnyPoliceVehicle && !Game.LocalPlayer.Character.IsPassenger) // both keys are pressed and plaayer is driving a police vehicle
                            {
                                Vector3 posAheadVeh = Game.LocalPlayer.Character.FrontPosition; // curent position of player, and therefore vehicle
                                try
                                {
                                    Vehicle[] nearbyVehs = Game.LocalPlayer.Character.GetNearbyVehicles(6);
                                    float targetDist = new float(); // distance to target vehicle
                                    Vehicle targetVehicle = new Vehicle();
                                    foreach (Vehicle veh in nearbyVehs)
                                    { 
                                        float tempDist = posAheadVeh.DistanceTo(veh.Position); // distance to current vehicle in list
                                        if (tempDist < targetDist)
                                        {
                                            targetDist = tempDist; // new minimum to compare to
                                            targetVehicle = veh; // set the new "close" vehicle as traffic stop target
                                        }
                                    }
                                    if (targetDist > 8.0f)
                                    {
                                        Game.DisplayNotification("No nearby vehicle to pull over!");
                                        break;
                                    }
                                        Functions.StartPulloverOnParkedVehicle(targetVehicle, true, false); // TODO change to front of vehicle
                                }
                                catch (Exception e)
                                {
                                    Game.LogTrivial("[StopRightThere] ERROR: " + e.ToString());
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Game.LogTrivial("[StopRightThere] ERROR: " + e.ToString());
                        }
                        GameFiber.Sleep(100);
                    }
                });
            }
            else
            {

            }

        }
    }
}