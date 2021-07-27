using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace StopRightThere
{
    class Initialization
    {
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
                try { Main.key1 = (Keys)kc.ConvertFromString(Reader.ReadString("Keybindings", "PulloverKey", "L")); successfullySetup[0] = true; } catch { Game.LogTrivial($"[StopRightThere] [initialization] WARNING: Couldn't read PulloverKey"); }
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

                Main.key1 = Keys.L;
                Main.key2 = Keys.LControlKey;
            }

            if (!settingsOK)
            {
                if (successfullySetup[0] == false) Main.key1 = Keys.L; ;
                if (successfullySetup[1] == false) Main.key2 = Keys.LControlKey;
            }


            Game.LogTrivial("[StopRightThere] [initialization] INFO: Setting: PulloverKey = " + Main.key1);
            Game.LogTrivial("[StopRightThere] [initialization] INFO: Setting: ModifierKey = " + Main.key2);

            return settingsOK;
        }

    }
}
