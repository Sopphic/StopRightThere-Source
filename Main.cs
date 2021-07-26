//+--------------------------------------------+
//| Author: Sopphic                                |
//+--------------------------------------------+

using LSPD_First_Response.Mod.API;
using Rage;
using System;


namespace StopRightThere
{

    public class Main : Plugin
    {
        //Initialization of the plugin.
        public override void Initialize()
        {
            //This is saying that when our OnDuty status is changed, it calls for the code to call private static void OnOnDutyStateChangedHandler near the bottom of this page.
            Functions.OnOnDutyStateChanged += OnOnDutyStateChangedHandler;

            //Game.LogTrivial's are used to help you identify problems when debugging a crash with your plugin, so you know exactly what's going on when.

            //This will show in the RagePluginHook.log as "Vehicle Search Plugin 2.0.0.0 has been initialised." 
            Game.LogTrivial("[StopRightThere] INFO: StopRightThere " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " has been initialised.");

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
                            System.Windows.Forms.Keys modifier = System.Windows.Forms.Keys.LControlKey;
                            System.Windows.Forms.Keys key = System.Windows.Forms.Keys.P;
                            if (keyboard.IsDown(key) && keyboard.IsDown(modifier) && Game.LocalPlayer.Character.IsInAnyPoliceVehicle && !Game.LocalPlayer.Character.IsPassenger) // both keys are pressed and plaayer is driving a police vehicle
                            {
                                Vector3 posAheadVeh = Game.LocalPlayer.Character.FrontPosition; // curent position of player, and therefore vehicle
                                Vector3 tempPosition = new Vector3();
                                try
                                {
                                    Functions.StartPulloverOnParkedVehicle(Game.LocalPlayer.Character.GetNearbyVehicles(1)[0], true, false); // TODO change to front of vehicle
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
                    }
                });
            }
            else
            {

            }

        }
    }
}