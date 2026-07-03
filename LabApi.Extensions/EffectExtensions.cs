using LabApi.Features.Wrappers;
using System;

namespace LabApi.Extensions
{
    /// <summary>
    /// Explicit unified tracking keys for all native SCP:Secret Laboratory status effect classifications.
    /// Eliminates the need for custom plugin-level mapping enums.
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
    /// Provides high-performance execution routing layers to dynamically activate status effects 
    /// via runtime enum tokens, bypassing strict compile-time generic constraints.
    /// </summary>
    internal static class EffectExtensions
    {
        /// <summary>
        /// Forcibly activates a native facility status effect onto the targeted player instance via its runtime enum identifier.
        /// </summary>
        /// <param name="player">The target <see cref="Player"/> entity undergoing sensory or physical modulation.</param>
        /// <param name="effect">The runtime <see cref="FacilityEffectType"/> token specifying the desired status effect.</param>
        /// <param name="intensity">The active scale intensity coefficient assigned to the status node.</param>
        /// <param name="duration">The total temporal execution lifespan in seconds allocated for the effect loop.</param>
        public static void EnableEffect(this Player player, FacilityEffectType effect, byte intensity = 1, float duration = 0f)
        {
            if (player?.GameObject is null) return;

            // Highly optimized C# 9.0 clean pattern-driven execution matrix
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
    }
}