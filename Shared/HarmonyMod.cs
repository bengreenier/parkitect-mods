using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace Shared
{
    public abstract class HarmonyMod<TSelf> : ParkitectMod where TSelf : HarmonyMod<TSelf>
    {
        protected Harmony Patcher { get; private set; }
        
        public override void onEnabled()
        {
            base.onEnabled();

            this.Patcher = new Harmony(this.getIdentifier());

            var patchTypes = typeof(TSelf).GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic |
                                                       BindingFlags.Instance | BindingFlags.Static)
                .Where(ty => ty.IsClass && ty.GetCustomAttribute(typeof(HarmonyPatch)) != null);
            
            foreach (var patchType in patchTypes)
            {
                Debug.Log($"Beginning patch {patchType.Name}");
                var proc = this.Patcher.CreateClassProcessor(patchType);
                Debug.Log($"Applying patch {patchType.Name}");
                proc.Patch();
            }
        }

        public override void onDisabled()
        {
            base.onDisabled();

            var patchTypes = typeof(TSelf).GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic |
                                                       BindingFlags.Instance | BindingFlags.Static)
                .Where(ty => ty.IsClass && ty.GetCustomAttribute(typeof(HarmonyPatch)) != null);
            
            foreach (var patchType in patchTypes)
            {
                Debug.Log($"Beginning unpatch {patchType.Name}");
                var proc = this.Patcher.CreateClassProcessor(patchType);
                Debug.Log($"Removing patch {patchType.Name}");
                proc.Unpatch();
            }
        }
    }
}