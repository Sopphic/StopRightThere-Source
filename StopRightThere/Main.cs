//+--------------------------------------------+
//| Author: Sopphic                            |
//+--------------------------------------------+

using Rage;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StopRightThere
{

    public class Main : Plugin
    {
        internal static Keys key1 = new Keys();
        internal static Keys key2 = new Keys();
        internal static bool OnDutyIndicator = false; 

        //Initialization of the plugin.
        public override void Initialize()
        {
            //This is saying that when our OnDuty status is changed, it calls for the code to call private static void OnOnDutyStateChangedHandler near the bottom of this page.
            Functions.OnOnDutyStateChanged += OnOnDutyStateChangedHandler;

            //Game.LogTrivial's are used to help you identify problems when debugging a crash with your plugin, so you know exactly what's going on when.

            //This will show in the RagePluginHook.log as "Vehicle Search Plugin 2.0.0.0 has been initialised." 
            Game.LogTrivial("[StopRightThere] INFO: StopRightThere " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " has been initialised.");
            Initialization.getSettings();
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
            OnDutyIndicator = OnDuty;
            //We have to make sure they are actually on duty so the code can do its work, so we use an "if" statement.
            if (OnDuty)
            {
                Game.LogTrivial("[StopRightThere] Info: Initialized");
                Game.DisplayNotification("~y~StopRightThere~w~: \nInitialized");
                StopRightThere.StopRightThere_Run();
            }
            else
            {
                StopRightThere.StopRightThere_Stop();
                GameFiber.Yield(); //so the vars from this plugin stays in the stack as long as other fibers are still working with variables
            }

        }
    }
}