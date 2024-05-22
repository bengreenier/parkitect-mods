using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Shared;
using UnityEngine;
using Logger = Shared.Logger;
using Object = UnityEngine.Object;

namespace Undo
{
    public class UndoMod : HarmonyMod<UndoMod>
    {
        public static Logger Logger = new Logger("com.bengreenier.undo");
        
        protected override int SupportedAppBuildId => LatestKnownVersion;
        protected override Version Version => new Version(1, 0, 0);
        protected override string Name => Constants.Title;

        protected override string Description =>
            "Adds support for undoing build operations, with configurable key bindings.";

        private GameObject undoGO;

        public override bool isMultiplayerModeCompatible()
        {
            return true;
        }

        public override bool isRequiredByAllPlayersInMultiplayerMode()
        {
            return false;
        }

        public override void onEnabled()
        {
            base.onEnabled();

            undoGO = new GameObject("undoGO");
            undoGO.AddComponent<UndoActivationBehaviour>();
            
        }

        public override void onDisabled()
        {
            base.onDisabled();

            if (undoGO != null)
            {
                Object.DestroyImmediate(undoGO);
            }
        }
        
         [HarmonyPatch]
        internal class BuilderImplementationUndoPatch
        {
            private static IEnumerable<MethodBase> TargetMethods() => new[]
            {
                // these are just all the BuilderImplementation<T> concrete types - see Harmony issues with generics.
                AccessTools.Method(
                    typeof(BlockBuilderImplementation), "onAfterBuild",
                    new[] { typeof(List<BuildableObject>) }),
                AccessTools.Method(
                    typeof(BoatRideEntranceExitBuilderImplementation), "onAfterBuild",
                    new[] { typeof(List<BuildableObject>) }),
                AccessTools.Method(
                    typeof(FlatRideEntranceExitBuilderImplementation), "onAfterBuild",
                    new[] { typeof(List<BuildableObject>) }),
                AccessTools.Method(
                    typeof(TrackedRideEntranceExitBuilderImplementation), "onAfterBuild",
                    new[] { typeof(List<BuildableObject>) }),
                AccessTools.Method(
                    typeof(BlueprintBuilderImplementation), "onAfterBuild", new[] { typeof(List<BuildableObject>) }),
                AccessTools.Method(
                    typeof(BoatRideBuilderImplementation), "onAfterBuild", new[] { typeof(List<BuildableObject>) }),
                AccessTools.Method(
                    typeof(ConnectedBlockBuilderImplementation), "onAfterBuild",
                    new[] { typeof(List<BuildableObject>) }),
                AccessTools.Method(
                    typeof(PathBuilderImplementation), "onAfterBuild", new[] { typeof(List<BuildableObject>) }),
                AccessTools.Method(
                    typeof(TransportSystemBuilderImplementation), "onAfterBuild",
                    new[] { typeof(List<BuildableObject>) }),
                AccessTools.Method(
                    typeof(DecoBuilderImplementation), "onAfterBuild", new[] { typeof(List<BuildableObject>) }),
                AccessTools.Method(
                    typeof(FenceBuilderImplementation), "onAfterBuild", new[] { typeof(List<BuildableObject>) }),
                AccessTools.Method(
                    typeof(FlatRideBuilderImplementation), "onAfterBuild", new[] { typeof(List<BuildableObject>) }),
                AccessTools.Method(
                    typeof(LayeredTextBuilderImplementation), "onAfterBuild", new[] { typeof(List<BuildableObject>) }),
                AccessTools.Method(
                    typeof(PathAttachmentBuilderImplementation), "onAfterBuild",
                    new[] { typeof(List<BuildableObject>) }),
                AccessTools.Method(
                    typeof(RideCameraBuilderImplementation), "onAfterBuild", new[] { typeof(List<BuildableObject>) }),
                AccessTools.Method(
                    typeof(SpawnBuilderImplementation), "onAfterBuild", new[] { typeof(List<BuildableObject>) }),
                AccessTools.Method(
                    typeof(SplashBattleTargetBuildCommand), "onAfterBuild", new[] { typeof(List<BuildableObject>) }),
                AccessTools.Method(
                    typeof(TrackAttachmentBuilderImplementation), "onAfterBuild",
                    new[] { typeof(List<BuildableObject>) }),
                AccessTools.Method(
                    typeof(TrackBuilderImplementation), "onAfterBuild", new[] { typeof(List<BuildableObject>) }),
                AccessTools.Method(
                    typeof(TrackedRideBuilderImplementation), "onAfterBuild", new[] { typeof(List<BuildableObject>) }),
                (MethodBase)AccessTools.Method(
                    typeof(UtilityBuildingBuilderImplementation), "onAfterBuild",
                    new[] { typeof(List<BuildableObject>) }),
            }.Where((methodBase, index) =>
            {
                // shouldn't need this, but keeping while debugging
                if (methodBase == null)
                {
                    UndoMod.Logger.Log($"Warning - null methodBase {index} skipped!");
                }

                return methodBase != null;
            });

            [HarmonyPostfix]
            public static void Postfix(
                AbstractBaseBuildCommand ___triggeredFromCommand,
                List<BuildableObject> builtObjectInstances)
            {
                Logger.Log("Running postfix");
                
                if (___triggeredFromCommand.isOwnCommand)
                {
                    UndoStateManagerSingleton.Instance.UndoStack.Push(builtObjectInstances);
                }
            }
        }
    }
}