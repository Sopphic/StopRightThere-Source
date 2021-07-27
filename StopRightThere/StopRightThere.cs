using Rage;
using LSPD_First_Response.Mod.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StopRightThere
{
    static class StopRightThere
    {
        static internal void StopRightThere_Run()
        {
            GameFiber.StartNew(delegate
            {
                while (Main.OnDutyIndicator)
                {
                    try
                    {
                        KeyboardState keyboard = new KeyboardState();
                        if (keyboard.IsDown(Main.key1) && keyboard.IsDown(Main.key2) && Game.LocalPlayer.Character.IsInAnyPoliceVehicle && !Game.LocalPlayer.Character.IsPassenger) // both keys are pressed and plaayer is driving a police vehicle
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

        static internal void StopRightThere_Stop()
        {
            
        }
    }
}
