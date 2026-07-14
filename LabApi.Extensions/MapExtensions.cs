using LabApi.Features.Wrappers;

namespace LabApi.Extensions
{
    /// <summary>
    /// Utility extensions for global facility operations: doors, lights and generators.
    /// </summary>
    public static class MapExtensions
    {
        #region Doors

        /// <summary>
        /// Breaks all breakable doors in the entire facility.
        /// </summary>
        public static void BreakAllFacilityDoors()
            => Room.List?.ForEach(r => r?.BreakAllDoors());

        #endregion

        #region Lights

        /// <summary>
        /// Enables or disables all lights in the facility.
        /// </summary>
        public static void SetAllLightsEnabled(bool enabled)
            => Room.List?.ForEach(room =>
                room?.AllLightControllers?.ForEach(c => c.LightsEnabled = enabled));

        #endregion

        #region Generators

        /// <summary>
        /// Returns the number of engaged generators.
        /// </summary>
        public static int GetEngagedGeneratorsCount()
        {
            int count = 0;

            foreach (var generator in Generator.List)
            {
                if (generator != null && generator.Engaged)
                    count++;
            }

            return count;
        }

        /// <summary>
        /// Returns true if all generators are engaged and the count meets the required minimum.
        /// </summary>
        public static bool AreAllGeneratorsEngaged(int minimumRequiredCount = 3)
        {
            var generators = Generator.List;
            if (generators is null)
                return false;

            int engaged = GetEngagedGeneratorsCount();
            int total = generators.Count;

            return engaged == total && engaged >= minimumRequiredCount;
        }

        #endregion
    }
}
