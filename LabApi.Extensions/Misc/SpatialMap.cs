using LabApi.Features.Wrappers;
using UnityEngine;

namespace LabApi.Extensions.Misc
{
    /// <summary>
    /// Provides enterprise-grade global spatial lookup queries, entity resolution matrices, 
    /// and high-performance native factory wrappers for active game world tracking objects.
    /// </summary>
    internal static class SpatialMap
    {
        /// <summary>
        /// Retrieves a pseudo-randomly selected <see cref="Elevator"/> instance from the active global facility map registry.
        /// </summary>
        /// <returns>A randomly evaluated structural <see cref="Elevator"/> module context from the global tracking pool.</returns>
        public static Elevator GetRandomElevator() => Map.GetRandomElevator();

        /// <summary>
        /// Resolves and maps a concrete 3D spatial coordinate vector onto its corresponding active <see cref="Room"/> architectural sector containment bounds.
        /// </summary>
        /// <param name="position">The raw 3D spatial <see cref="Vector3"/> coordinates queried within the active simulation grid workspace.</param>
        /// <returns>The concrete <see cref="Room"/> instance encompassing the targeted spatial coordinates; otherwise, <see langword="null"/> if the location maps to void zones.</returns>
        public static Room GetRoomAtPosition(Vector3 position) => Room.GetRoomAtPosition(position);

        /// <summary>
        /// Resolves a high-level wrapper instance of <see cref="Player"/> bound directly to a native internal engine <see cref="ReferenceHub"/> identifier token.
        /// </summary>
        /// <param name="referenceHub">The underlying native low-level <see cref="ReferenceHub"/> component instance tracking client state network links.</param>
        /// <returns>The fully synchronized, strongly-typed <see cref="Player"/> wrapper node linked to the target hub signature; otherwise, <see langword="null"/>.</returns>
        public static Player GetPlayer(ReferenceHub referenceHub) => Player.Get(referenceHub);

        /// <summary>
        /// Resolves a high-level wrapped <see cref="Ragdoll"/> entity structural instance directly from a native low-level Unity engine <see cref="PlayerRoles.Ragdolls.BasicRagdoll"/> component context.
        /// </summary>
        /// <param name="ragdoll">The underlying low-level native engine <see cref="PlayerRoles.Ragdolls.BasicRagdoll"/> asset component instance generated upon player elimination.</param>
        /// <returns>The API-wrapped <see cref="Ragdoll"/> proxy structure facilitating high-level property access; otherwise, <see langword="null"/>.</returns>
        public static Ragdoll GetRagdoll(PlayerRoles.Ragdolls.BasicRagdoll ragdoll) => Ragdoll.Get(ragdoll);
    }
}