using CustomPlayerEffects;
using LabApi.Features.Enums;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace LabApi.Extensions
{
    /// <summary>
    /// Specifies the native SCP:Secret Laboratory status effect types.
    /// </summary>
    public enum FacilityEffectType
    {
        // Visual Impairments
        Blurred, Blindness, Flashed, InsufficientLighting,

        // Auditory Impairments
        Deafened, SoundtrackMute,

        // Mobility Modifiers
        Slowness, SilentWalk, Exhausted, Disabled, Ensnared, MovementBoost,

        // Trauma and Biological Statuses
        Bleeding, Poisoned, Burned, Corroding, CardiacArrest, Asphyxiated,

        // Neurological and Mental Alterations
        Concussed, Traumatized,

        // Anomalous and Special Item Overlays
        Invisible, Scp207, AntiScp207, Scp1853, SpawnProtected,
        RainbowTaste, BodyshotReduction, DamageReduction,
        Ghostly, SeveredHands, Stained, Vitality,
        Decontaminating, PocketCorroding
    }
    /// <summary>
    /// Ultra high-performance extension methods for status effects management.
    /// Features compile-time zero-allocation paths and boilerplate-free batch operations using state-passing lambdas.
    /// </summary>
    public static class EffectExtensions
    {
        // Flat array caches providing O(1) direct index execution paths (faster than Dictionary lookup).
        private static readonly Action<Player, byte, float>[] EnableDelegates;
        private static readonly Action<Player>[] DisableDelegates;
        private static readonly Func<Player, bool>[] HasDelegates;
        private static readonly Func<Player, byte>[] IntensityDelegates;

        /// <summary>
        /// Static constructor - executes once at startup to meta-compile all status effect generic calls.
        /// </summary>
        static EffectExtensions()
        {
            var values = (FacilityEffectType[])Enum.GetValues(typeof(FacilityEffectType));

            // Find the maximum enum integer value to dynamically size the lookup arrays.
            int maxVal = 0;
            for (int i = 0; i < values.Length; i++)
            {
                int val = (int)values[i];
                if (val > maxVal)
                    maxVal = val;
            }

            int arraySize = maxVal + 1;
            EnableDelegates = new Action<Player, byte, float>[arraySize];
            DisableDelegates = new Action<Player>[arraySize];
            HasDelegates = new Func<Player, bool>[arraySize];
            IntensityDelegates = new Func<Player, byte>[arraySize];

            // Access the game assembly containing native CustomPlayerEffects.
            var effectsAssembly = typeof(StatusEffectBase).Assembly;

            // Reuse parameters to avoid redundant expression rebuilding.
            var playerParam = Expression.Parameter(typeof(Player), "player");
            var intensityParam = Expression.Parameter(typeof(byte), "intensity");
            var durationParam = Expression.Parameter(typeof(float), "duration");

            for (int i = 0; i < values.Length; i++)
            {
                var effect = values[i];
                int index = (int)effect;
                string effectName = effect.ToString();

                // Dynamically resolve the corresponding CustomPlayerEffects class type by name.
                Type effectType = effectsAssembly.GetType($"CustomPlayerEffects.{effectName}");
                if (effectType == null)
                {
                    throw new TypeLoadException($"[LabApi.Extensions] Could not find native status effect class for: CustomPlayerEffects.{effectName}");
                }

                // 1. Compile EnableDelegate: (player, intensity, duration) => player.EnableEffect<T>(intensity, duration, false)
                var enableMethod = typeof(Player).GetMethod("EnableEffect", new[] { typeof(byte), typeof(float), typeof(bool) })
                                                 ?.MakeGenericMethod(effectType);
                if (enableMethod != null)
                {
                    var enableCall = Expression.Call(playerParam, enableMethod, intensityParam, durationParam, Expression.Constant(false));
                    EnableDelegates[index] = Expression.Lambda<Action<Player, byte, float>>(enableCall, playerParam, intensityParam, durationParam).Compile();
                }

                // 2. Compile DisableDelegate: (player) => player.DisableEffect<T>()
                var disableMethod = typeof(Player).GetMethod("DisableEffect", Type.EmptyTypes)
                                                  ?.MakeGenericMethod(effectType);
                if (disableMethod != null)
                {
                    var disableCall = Expression.Call(playerParam, disableMethod);
                    DisableDelegates[index] = Expression.Lambda<Action<Player>>(disableCall, playerParam).Compile();
                }

                // 3. Compile HasDelegate: (player) => player.HasEffect<T>()
                var hasMethod = typeof(Player).GetMethod("HasEffect", Type.EmptyTypes)
                                              ?.MakeGenericMethod(effectType);
                if (hasMethod != null)
                {
                    var hasCall = Expression.Call(playerParam, hasMethod);
                    HasDelegates[index] = Expression.Lambda<Func<Player, bool>>(hasCall, playerParam).Compile();
                }

                // 4. Compile IntensityDelegate: (player) => player.GetEffect<T>() != null ? player.GetEffect<T>().Intensity : 0
                var getEffectMethod = typeof(Player).GetMethod("GetEffect", Type.EmptyTypes)
                                                   ?.MakeGenericMethod(effectType);
                if (getEffectMethod != null)
                {
                    var effectVar = Expression.Variable(effectType, "effect");
                    var assign = Expression.Assign(effectVar, Expression.Call(playerParam, getEffectMethod));
                    var intensityProp = typeof(StatusEffectBase).GetProperty("Intensity");

                    var condition = Expression.Condition(
                        Expression.NotEqual(effectVar, Expression.Constant(null, effectType)),
                        Expression.Property(effectVar, intensityProp),
                        Expression.Constant((byte)0)
                    );

                    var block = Expression.Block(new[] { effectVar }, assign, condition);
                    IntensityDelegates[index] = Expression.Lambda<Func<Player, byte>>(block, playerParam).Compile();
                }
            }
        }

        #region Single Player - Enable Operations

        /// <summary>
        /// Enables a specific status effect on a single player.
        /// </summary>
        public static void EnableEffect(this Player player, FacilityEffectType effect, byte intensity = 1, float duration = 0f)
        {
            if (player == null || !player.IsReady)
                return;

            int index = (int)effect;
            if (index >= 0 && index < EnableDelegates.Length)
            {
                EnableDelegates[index]?.Invoke(player, intensity, duration);
            }
        }

        #endregion

        #region Single Player - Disable Operations

        /// <summary>
        /// Disables a specific status effect on a single player.
        /// </summary>
        public static void DisableEffect(this Player player, FacilityEffectType effect)
        {
            if (player == null || !player.IsReady)
                return;

            int index = (int)effect;
            if (index >= 0 && index < DisableDelegates.Length)
            {
                DisableDelegates[index]?.Invoke(player);
            }
        }

        /// <summary>
        /// Disables all active status effects on a player.
        /// </summary>
        public static void DisableAllEffects(this Player player)
        {
            if (player == null || !player.IsReady)
                return;

            player.DisableAllEffects();
        }

        #endregion

        #region Single Player - Queries

        /// <summary>
        /// Returns true if the player has the specified status effect active.
        /// </summary>
        public static bool HasEffect(this Player player, FacilityEffectType effect)
        {
            if (player == null || !player.IsReady)
                return false;

            int index = (int)effect;
            if (index >= 0 && index < HasDelegates.Length)
            {
                return HasDelegates[index]?.Invoke(player) ?? false;
            }

            return false;
        }

        /// <summary>
        /// Returns the current intensity level (0-255) of the specified status effect on the player.
        /// </summary>
        public static byte GetEffectIntensity(this Player player, FacilityEffectType effect)
        {
            if (player == null || !player.IsReady)
                return 0;

            int index = (int)effect;
            if (index >= 0 && index < IntensityDelegates.Length)
            {
                return IntensityDelegates[index]?.Invoke(player) ?? 0;
            }

            return 0;
        }

        #endregion

        #region Batch Operations (True Zero-Allocation Paths)

        /// <summary>
        /// Enables a status effect for all players in the collection with zero heap allocations.
        /// Uses ValueTuple state-passing and static lambdas to completely avoid GC display class allocation.
        /// </summary>
        public static void EnableEffect(this IEnumerable<Player> players, FacilityEffectType effect, byte intensity = 1, float duration = 0f)
        {
            if (players == null)
                return;

            players.ForEach(
                (effect, intensity, duration),
                static (player, state) => player?.EnableEffect(state.effect, state.intensity, state.duration)
            );
        }

        /// <summary>
        /// Enables a status effect for all provided players.
        /// </summary>
        public static void EnableEffect(FacilityEffectType effect, params Player[] players)
            => players.EnableEffect(effect);

        /// <summary>
        /// Enables a status effect for all provided players with custom intensity and duration.
        /// </summary>
        public static void EnableEffect(FacilityEffectType effect, byte intensity, float duration, params Player[] players)
            => players.EnableEffect(effect, intensity, duration);

        /// <summary>
        /// Disables a status effect for all players in the collection with zero heap allocations.
        /// Uses state-passing and static lambdas to completely avoid GC display class allocation.
        /// </summary>
        public static void DisableEffect(this IEnumerable<Player> players, FacilityEffectType effect)
        {
            if (players == null)
                return;

            players.ForEach(
                effect,
                static (player, eff) => player?.DisableEffect(eff)
            );
        }

        /// <summary>
        /// Disables a status effect for all provided players.
        /// </summary>
        public static void DisableEffect(FacilityEffectType effect, params Player[] players)
            => players.DisableEffect(effect);

        /// <summary>
        /// Disables all status effects for all players in the collection with zero heap allocations.
        /// </summary>
        public static void DisableAllEffects(this IEnumerable<Player> players)
        {
            if (players == null)
                return;

            players.ForEach(static player => player?.DisableAllEffects());
        }

        /// <summary>
        /// Disables all status effects for all provided players.
        /// </summary>
        public static void DisableAllEffects(params Player[] players)
            => players.DisableAllEffects();

        #endregion
    }
}