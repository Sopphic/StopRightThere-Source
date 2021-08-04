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
        internal static void StopRightThere_Run()
        {
            GameFiber.StartNew(delegate
            {
                while (Main.OnDutyIndicator)
                {
                    try
                    {
                        if (Game.IsKeyDownRightNow(Main.key1) && Game.IsKeyDownRightNow(Main.key2) && Game.LocalPlayer.Character.IsInAnyPoliceVehicle && !Game.LocalPlayer.Character.IsPassenger) // both keys are pressed and plaayer is driving a police vehicle
                        {
                            Vector3 posAheadVeh = Game.LocalPlayer.Character.CurrentVehicle.GetOffsetPosition(new Vector3(7f,0f,0f)); // curent position of player, and therefore vehicle
                            try
                            {
                                Vehicle[] nearbyVehs = Game.LocalPlayer.Character.GetNearbyVehicles(6);
                                float targetDist = 99.0f; // distance to target vehicle
                                Vehicle targetVehicle = nearbyVehs[nearbyVehs.Length-1];    //so we take tha vehicle that is the farest vehicle. so it will get definetly changed by algorithm.
                                foreach (Vehicle veh in nearbyVehs.Where(v => (v.Driver && v.Driver != Game.LocalPlayer.Character && !v.IsPoliceVehicle)))
                                {
                                    float tempDist = posAheadVeh.DistanceTo(veh.Position); // distance to current vehicle in list
                                    if (tempDist < targetDist)
                                    {
                                        targetDist = tempDist; // new minimum to compare to
                                        targetVehicle = veh; // set the new "close" vehicle as traffic stop target
                                    }
                                }
                                if (targetDist > 15.0f)
                                {
                                    Game.DisplayNotification("No nearby vehicle to pull over!");
                                }
                                else
                                {
                                    Game.LogTrivial("SRT: Attempting to start traffic stop");
                                    Functions.StartPulloverOnParkedVehicle(targetVehicle, true, false);
                                    Game.DisplayNotification(targetVehicle.Model.ToString() + "with plate" + targetVehicle.LicensePlate + "pulled over.");
                                    targetVehicle.AttachBlip();
                                    GameFiber.Sleep(1000);
                                }
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

        internal static void StopRightThere_Stop()
        {
            
        }
    }
}
