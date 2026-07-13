using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;

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
    /// Provides extension methods for applying status effects to players.
    /// </summary>
    public static class EffectExtensions
    {
        #region Single Player Operations
        /// <summary>
        /// Enables a specific status effect on a single player.
        /// </summary>
        public static void EnableEffect(this Player player, FacilityEffectType effect, byte intensity = 1, float duration = 0f)
        {
            if (player?.GameObject is null) return;

            switch (effect)
            {
                case FacilityEffectType.Blurred: player.EnableEffect<CustomPlayerEffects.Blurred>(intensity, duration); break;
                case FacilityEffectType.Blindness: player.EnableEffect<CustomPlayerEffects.Blindness>(intensity, duration); break;
                case FacilityEffectType.Flashed: player.EnableEffect<CustomPlayerEffects.Flashed>(intensity, duration); break;
                case FacilityEffectType.Deafened: player.EnableEffect<CustomPlayerEffects.Deafened>(intensity, duration); break;
                case FacilityEffectType.Slowness: player.EnableEffect<CustomPlayerEffects.Slowness>(intensity, duration); break;
                case FacilityEffectType.SilentWalk: player.EnableEffect<CustomPlayerEffects.SilentWalk>(intensity, duration); break;
                case FacilityEffectType.Exhausted: player.EnableEffect<CustomPlayerEffects.Exhausted>(intensity, duration); break;
                case FacilityEffectType.Disabled: player.EnableEffect<CustomPlayerEffects.Disabled>(intensity, duration); break;
                case FacilityEffectType.Bleeding: player.EnableEffect<CustomPlayerEffects.Bleeding>(intensity, duration); break;
                case FacilityEffectType.Poisoned: player.EnableEffect<CustomPlayerEffects.Poisoned>(intensity, duration); break;
                case FacilityEffectType.Burned: player.EnableEffect<CustomPlayerEffects.Burned>(intensity, duration); break;
                case FacilityEffectType.Corroding: player.EnableEffect<CustomPlayerEffects.Corroding>(intensity, duration); break;
                case FacilityEffectType.Concussed: player.EnableEffect<CustomPlayerEffects.Concussed>(intensity, duration); break;
                case FacilityEffectType.Traumatized: player.EnableEffect<CustomPlayerEffects.Traumatized>(intensity, duration); break;
                case FacilityEffectType.Invisible: player.EnableEffect<CustomPlayerEffects.Invisible>(intensity, duration); break;
                case FacilityEffectType.Scp207: player.EnableEffect<CustomPlayerEffects.Scp207>(intensity, duration); break;
                case FacilityEffectType.AntiScp207: player.EnableEffect<CustomPlayerEffects.AntiScp207>(intensity, duration); break;
                case FacilityEffectType.MovementBoost: player.EnableEffect<CustomPlayerEffects.MovementBoost>(intensity, duration); break;
                case FacilityEffectType.DamageReduction: player.EnableEffect<CustomPlayerEffects.DamageReduction>(intensity, duration); break;
                case FacilityEffectType.RainbowTaste: player.EnableEffect<CustomPlayerEffects.RainbowTaste>(intensity, duration); break;
                case FacilityEffectType.BodyshotReduction: player.EnableEffect<CustomPlayerEffects.BodyshotReduction>(intensity, duration); break;
                case FacilityEffectType.Scp1853: player.EnableEffect<CustomPlayerEffects.Scp1853>(intensity, duration); break;
                case FacilityEffectType.CardiacArrest: player.EnableEffect<CustomPlayerEffects.CardiacArrest>(intensity, duration); break;
                case FacilityEffectType.InsufficientLighting: player.EnableEffect<CustomPlayerEffects.InsufficientLighting>(intensity, duration); break;
                case FacilityEffectType.SoundtrackMute: player.EnableEffect<CustomPlayerEffects.SoundtrackMute>(intensity, duration); break;
                case FacilityEffectType.SpawnProtected: player.EnableEffect<CustomPlayerEffects.SpawnProtected>(intensity, duration); break;
                case FacilityEffectType.Ensnared: player.EnableEffect<CustomPlayerEffects.Ensnared>(intensity, duration); break;
                case FacilityEffectType.Ghostly: player.EnableEffect<CustomPlayerEffects.Ghostly>(intensity, duration); break;
                case FacilityEffectType.SeveredHands: player.EnableEffect<CustomPlayerEffects.SeveredHands>(intensity, duration); break;
                case FacilityEffectType.Stained: player.EnableEffect<CustomPlayerEffects.Stained>(intensity, duration); break;
                case FacilityEffectType.Vitality: player.EnableEffect<CustomPlayerEffects.Vitality>(intensity, duration); break;
                case FacilityEffectType.Asphyxiated: player.EnableEffect<CustomPlayerEffects.Asphyxiated>(intensity, duration); break;
                case FacilityEffectType.Decontaminating: player.EnableEffect<CustomPlayerEffects.Decontaminating>(intensity, duration); break;
                case FacilityEffectType.PocketCorroding: player.EnableEffect<CustomPlayerEffects.PocketCorroding>(intensity, duration); break;
                default: throw new ArgumentException($"[LabApi.Extensions] Unrecognized facility status effect mapping: {effect}");
            }
        }
        #endregion

        #region Batch & Params Operations (Added for API Consistency)
        /// <summary>
        /// Enables a specific status effect on a collection of players.
        /// </summary>
        public static void EnableEffect(this IEnumerable<Player> players, FacilityEffectType effect, byte intensity = 1, float duration = 0f)
        {
            if (players is null) return;

            if (players is List<Player> concreteList)
            {
                int count = concreteList.Count;
                for (int i = 0; i < count; i++)
                {
                    concreteList[i].EnableEffect(effect, intensity, duration);
                }
                return;
            }

            foreach (Player player in players)
            {
                player.EnableEffect(effect, intensity, duration);
            }
        }

        /// <summary>
        /// Enables a specific status effect on an inline array of players using default intensity and duration.
        /// </summary>
        public static void EnableEffect(FacilityEffectType effect, params Player[] players)
            => EnableEffect(players, effect);

        /// <summary>
        /// Enables a specific status effect on an inline array of players with custom intensity and duration.
        /// </summary>
        public static void EnableEffect(FacilityEffectType effect, byte intensity, float duration, params Player[] players)
        {
            if (players is null) return;

            int count = players.Length;
            for (int i = 0; i < count; i++)
            {
                players[i].EnableEffect(effect, intensity, duration);
            }
        }
        #endregion
    }
}