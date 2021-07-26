//+--------------------------------------------+
//| Author: XXXX                                |
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
            Game.LogTrivial("[YourPluginName] INFO: YourPluginName " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " has been initialised.");

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
                RegisterCallouts();
                Game.LogTrivial("[YourPluginName] Info: Initialized");
                Game.DisplayNotification("~y~YourPluginName~w~: \nInitialized");

                GameFiber.StartNew(delegate
                {
                    try
                    {

                    }
                    catch (Exception e)
                    {
                        Game.LogTrivial("[YourPluginName] ERROR: " + e.ToString());
                    }
                });
            }
            else
            {

            }

        }
        private static void RegisterCallouts()
        {
            //Functions.RegisterCallout(typeof(Callouts.YOURcalloutNAME));
        }
    }
}