# Audio Manager API - Architecture API Registry

## 📦 Class: AssemblyExtensions

### 🔹 `FindEmbeddedAsset()`
**Description:** Dynamically resolves an embedded manifest resource pathway matching primary identifiers or structural fallback names.
```csharp
public static string FindEmbeddedAsset(this Assembly assembly, string primaryKey, string fileExtension = ".wav", params string[] alternativeTokens)
```

---

## 📦 Class: CassieExtensions

### 🔹 `CassieClear()`
**Description:** Flushes the current real-time vocal audio broadcast queue entirely, instantly suspending any ongoing audio playback.
```csharp
public static void CassieClear() => Announcer.Clear();
```

### 🔹 `SanitizeCassieString()`
**Description:** Systematically scrubs raw text fields, stripping hidden carriage returns and formatting errors while safely preserving empty strings for intentional text muting configurations.
```csharp
public static string SanitizeCassieString(this string rawMessage)
```

### 🔹 `Cassie_GlitchyMessage()`
**Description:** Dispatches an intentionally modulated, corrupted vocal sequence across the global audio matrix, computing its final playback duration tracking envelope.
```csharp
public static double Cassie_GlitchyMessage(string message, float glitchChance, float jamChance, string glitchifierOutput)
```

### 🔹 `Cassie_Message()`
**Description:** Dispatches a clean, high-clarity vocal notification sequence using optimized high-pass pitch metrics across global facility channels.
```csharp
public static double Cassie_Message(string message)
```

### 🔹 `ProcessAndDispatchMessage()`
**Description:** Executes defensive sanitization, structural sequence formatting, and real-time streaming transmission of CASSIE phrases under concrete queue priority matrices, optionally overriding active subtitle layers.
```csharp
public static void ProcessAndDispatchMessage(string message, string customSubtitles, bool shouldClear, string pitchModifier, float priority, bool disableMessages = false)
```

### 🔹 `ToCassieCountdown()`
**Description:** Transforms a primitive integer value token into a structured CASSIE vocal countdown phonetic sequence format matching the specific operational context tracking threshold boundaries.
```csharp
public static string ToCassieCountdown(this int notifyTime, string alertContext = "Seconds until Event Detonation")
```

### 🔹 `CalculateCassieMessageDuration()`
**Description:** Simulates the exact runtime execution footprint duration matching a specific message phrase sequence under a localized speed modification configuration envelope.
```csharp
public static double CalculateCassieMessageDuration(string message, double speed = 0.95)
```

### 🔹 `CalculateTotalMessagesDurations()`
**Description:** Iteratively aggregates and computes the total unified summation timeline bounds for a collection layout dictionary mapping asynchronous text phrases to explicit local frequency speed bounds.
```csharp
public static double CalculateTotalMessagesDurations(IDictionary<string, float> messageSpeedDictionary)
```

### 🔹 `CalculateTotalMessagesDurations()`
**Description:** Aggregates linear string data collections and determines their absolute runtime playback footprints utilizing a uniform fixed structural fallback speed index.
```csharp
public static double CalculateTotalMessagesDurations(IEnumerable<string> messages, float defaultSpeed = 1f)
```

### 🔹 `CalculateTotalMessagesDurations()`
**Description:** High-performance params collection inline overloading layer designed to process inline string listings and capture cumulative timeline tracks without manual collection allocations.
```csharp
public static double CalculateTotalMessagesDurations(float defaultSpeed = 1f, params string[] messages)
```

---

## 📦 Class: CollectionExtensions

### 🔹 `ExecuteThrottled()`
**Description:** Evaluates a temporal cooldown window for a specific key profile. If the tracking node is ready, executes the payload and automatically commits the new timestamp. <typeparam name="TKey">The structural identity lookup key tracking type.</typeparam>
```csharp
public static bool ExecuteThrottled<TKey>(this IDictionary<TKey, DateTime> cooldownMap, TKey key, TimeSpan window, Action throttleAction)
```

### 🔹 `PruneExpired()`
**Description:** Systematically purges all historical mapping entries whose tracking timestamps have safely elapsed past a specific temporal comparison index. Mitigates collection modification exceptions by isolating target records dynamically. <typeparam name="TKey">The structural identity lookup key tracking type.</typeparam>
```csharp
public static void PruneExpired<TKey>(this System.Collections.Generic.IDictionary<TKey, System.DateTime> dictionary, System.DateTime comparisonTime)
```

### 🔹 `IsCooldownActive()`
**Description:** Evaluates defensively whether a specific key profile is currently locked within an active temporal cooldown window. <typeparam name="TKey">The structural identity lookup key tracking type.</typeparam>
```csharp
public static bool IsCooldownActive<TKey>(this System.Collections.Generic.IDictionary<TKey, System.DateTime> cooldownMap, TKey key)
```

### 🔹 `TryAcquireLock()`
**Description:** Atomically evaluates a temporal gate check for a specific key target. If the lock window has elapsed, automatically commits a new future expiration milestone and grants authorization. <typeparam name="TKey">The structural identity lookup key tracking type.</typeparam>
```csharp
public static bool TryAcquireLock<TKey>(this System.Collections.Generic.IDictionary<TKey, System.DateTime> cooldownMap, TKey key, System.TimeSpan lockWindow)
```

---

## 📦 Class: DictionaryExtensions

### 🔹 `Deconstruct()`
**Description:** Deconstructs a key-value pair into separate architectural components (C# 9.0 Pattern Matcher support).
```csharp
public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> kvp, out TKey key, out TValue value)
```

---

## 📦 Class: DoorExtensions

### 🔹 `IsOpen()`
**Description:** Evaluates the underlying structural state of the specified door entity to determine if it is currently unsealed.
```csharp
public static bool IsOpen(this Door door) => door != null && door.IsOpened;
```

### 🔹 `Toggle()`
**Description:** Inverts the operational passage status of the targeted door instance, executing a swift state mutation between open and closed topologies defensively.
```csharp
public static void Toggle(this Door door)
```

### 🔹 `WhereNameIn()`
**Description:** Filters a sequence of doors to return only those matching any of the specified <see cref="DoorName"/> layout tokens.
```csharp
public static IEnumerable<Door> WhereNameIn(this IEnumerable<Door> doors, params DoorName[] names)
```

### 🔹 `Open()`
**Description:** Fluently unseals an individual door instance, driving its structural passage topology state to open.
```csharp
public static void Open(this Door door, bool bypassLocks = false) => door.SetOpenState(opened: true, bypassLocks);
```

### 🔹 `Close()`
**Description:** Fluently seals an individual door instance, driving its structural passage topology state to closed.
```csharp
public static void Close(this Door door, bool bypassLocks = false) => door.SetOpenState(opened: false, bypassLocks);
```

### 🔹 `SetLockState()`
**Description:** Updates the administrative server-side lock state of an individual door instance under a specific constraint reason.
```csharp
public static void SetLockState(this Door door, DoorLockReason reason, bool locked = true)
```

### 🔹 `SetOpenState()`
**Description:** Mutates the passage activation status (Open or Closed topology) of an individual door instance.
```csharp
public static void SetOpenState(this Door door, bool opened, bool bypassLocks = false)
```

### 🔹 `OpenAndLock()`
**Description:** Forcibly unseals an individual door and applies an administrative server-side lock state under a specific structural reason constraint.
```csharp
public static void OpenAndLock(this Door door, DoorLockReason reason, bool playSound = true)
```

### 🔹 `Open()`
**Description:** Attempts to mass unseal an aggregated collection sequence of doors cleanly.
```csharp
public static void Open(this IEnumerable<Door> doors, bool bypassLocks = false) => doors.SetOpenState(opened: true, bypassLocks);
```

### 🔹 `Close()`
**Description:** Attempts to mass seal an aggregated collection sequence of doors cleanly.
```csharp
public static void Close(this IEnumerable<Door> doors, bool bypassLocks = false) => doors.SetOpenState(opened: false, bypassLocks);
```

### 🔹 `SetLockState()`
**Description:** Forcibly updates the administrative server-side lock state across an aggregated collection sequence of doors.
```csharp
public static void SetLockState(this IEnumerable<Door> doors, DoorLockReason reason, bool locked = true)
```

### 🔹 `SetOpenState()`
**Description:** Attempts to mass mutate the passage activation status (Open or Closed topology) across an aggregated collection sequence of doors.
```csharp
public static void SetOpenState(this IEnumerable<Door> doors, bool opened, bool bypassLocks = false)
```

### 🔹 `OpenAndLock()`
**Description:** Forcibly unseals a collection layout of doors and applies an administrative server-side lock state under a specific structural reason constraint.
```csharp
public static void OpenAndLock(this IEnumerable<Door> doors, DoorLockReason reason, bool playSound = true)
```

### 🔹 `IsElevatorDoor()`
**Description:** Performs hierarchical component reflection and token verification on the native GameObject metadata to isolate whether the asset behaves structurally as an elevator cabin bulkhead.
```csharp
public static bool IsElevatorDoor(this Door door)
```

### 🔹 `IsGate()`
**Description:** Scans the underlying engine transform string identifiers to evaluate if the targeted runtime door asset is classified as a heavy checkpoint airlock Gate structure.
```csharp
public static bool IsGate(this Door door)
```

---

## 📦 Class: EffectExtensions

### 🔹 `EnableEffect()`
**Description:** Forcibly activates a native facility status effect onto the targeted player instance via its runtime enum identifier.
```csharp
public static void EnableEffect(this Player player, FacilityEffectType effect, byte intensity = 1, float duration = 0f)
```

---

## 📦 Class: ElevatorExtensions

### 🔹 `TurnOffLights()`
**Description:** Forcibly suppresses all physical and software-controlled light emission vectors inside the elevator cabin for a specific duration.
```csharp
public static void TurnOffLights(this Elevator elevator, float duration)
```

### 🔹 `TurnOnLights()`
**Description:** Restores standard electrical power and re-activates all internal illumination emitters inside the elevator cabin.
```csharp
public static void TurnOnLights(this Elevator elevator)
```

### 🔹 `FlickerElevatorLightsCoroutine()`
**Description:** Fluently executes a synchronized asynchronous lighting flicker animation loop over an individual <see cref="Elevator"/> cabin space.
```csharp
public static IEnumerator<float> FlickerElevatorLightsCoroutine(this Elevator elevator, float duration, float frequency)
```

### 🔹 `AreLightsOff()`
**Description:** Verifies whether the targeted elevator cabin is currently locked in an active blackout state loop.
```csharp
public static bool AreLightsOff(this Elevator elevator)
```

### 🔹 `OpenActiveDoors()`
**Description:** Fluently opens ONLY the elevator doors located on the currently active floor level, preventing cross-floor safety exploits.
```csharp
public static void OpenActiveDoors(this Elevator elevator, bool bypassLocks = false)
```

### 🔹 `CloseActiveDoors()`
**Description:** Fluently closes ONLY the elevator doors located on the currently active floor level.
```csharp
public static void CloseActiveDoors(this Elevator elevator, bool bypassLocks = false)
```

### 🔹 `GetElevatorsInZone()`
**Description:** Retrieves a filtered sequence of active elevator modules whose current destination grids map directly to a target facility zone boundary.
```csharp
public static IEnumerable<Elevator> GetElevatorsInZone(FacilityZone zone)
```

### 🔹 `GetElevatorsConnectedToRoom()`
**Description:** Isolates and filters the global elevator tracking arrays to return only the specific units structurally bridging into the target room.
```csharp
public static IEnumerable<Elevator> GetElevatorsConnectedToRoom(this Room room)
```

### 🔹 `IsActiveInRoom()`
**Description:** Evaluates whether any elevator infrastructure bound to the specified room is actively executing a mechanical movement sequence.
```csharp
public static bool IsActiveInRoom(this Room room)
```

### 🔹 `IsInExecutiveElevator()`
**Description:** Evaluates if an active player's spatial coordinates currently overlap an operational elevator cabin mapped to executive or facility transitional sectors.
```csharp
public static bool IsInExecutiveElevator(this Player player)
```

### 🔹 `LockElevatorsInZone()`
**Description:** Enforces absolute structural lockdowns on all elevator bulkhead vectors tracking within the requested facility zone.
```csharp
public static void LockElevatorsInZone(FacilityZone zone)
```

### 🔹 `UnlockElevatorsInZone()`
**Description:** Restores normal passage access and lifts all operational bulkhead locking restrictions across elevator units within the specified zone.
```csharp
public static void UnlockElevatorsInZone(FacilityZone zone)
```

### 🔹 `HandleElevatorsForRoom()`
**Description:** Processes a localized, probability-driven evaluation sweep across all elevators bound to a room, safely routing matching units into an execution action graph.
```csharp
public static void HandleElevatorsForRoom(this Room room, float affectChance, float duration, Action<Elevator> elevatorAction)
```

---

## 📦 Class: EnumExtensions

### 🔹 `ToAudioKey()`
**Description:** Automatically transforms an execution enum field token into a standardized lowercase string key identifier. Eliminates redundant switch-case evaluation structures across independent sub-system audio pipeline configurations.
```csharp
public static string ToAudioKey(this Enum value)
```

### 🔹 `ParseOrDefault()`
**Description:** Defensively converts a raw string literal target into its verified strongly-typed structural enum representation. Safely insulates execution pipelines against human typographical initialization anomalies located within deployment configuration matrices. <typeparam name="T">The concrete structural value constraint matching a standard system <see cref="Enum"/> layout topology.</typeparam>
```csharp
public static T ParseOrDefault<T>(this string value, T defaultValue = default, bool ignoreCase = true) where T : struct, Enum
```

### 🔹 `GetRandomValue()`
**Description:** Aggregates enumeration boundaries and retrieves a structurally random field token from the targeted enum reflection array space. Seamlessly targets the thread-isolated, safe data abstraction framework to guarantee performance scaling. <typeparam name="T">The targeting assembly constraint layout matching a core active system enumeration signature.</typeparam>
```csharp
public static T GetRandomValue<T>() where T : struct, Enum
```

---

## 📦 Class: FirearmExtensions

### 🔹 `HasAttachment()`
**Description:** Evaluates defensively whether the specified firearm instance contains an active attachment matching a designated blueprint identity token.
```csharp
public static bool HasAttachment(this FirearmItem firearm, AttachmentName attachmentName)
```

---

## 📦 Class: HandlerExtensions

### 🔹 `RegisterAll()`
**Description:** Systematically registers an aggregated inline array sequence of event handlers onto the central framework routing engine.
```csharp
public static void RegisterAll(params CustomEventsHandler[] handlers)
```

### 🔹 `UnregisterAll()`
**Description:** Systematically evicts and unregisters an aggregated inline array sequence of event handlers from the central framework routing engine.
```csharp
public static void UnregisterAll(params CustomEventsHandler[] handlers)
```

---

## 📦 Class: IoExtensions

### 🔹 `IsReparsePoint()`
**Description:** Evaluates defensively whether the targeted path configuration behaves structurally as an OS reparse point (Virtual Junction, Symlink, or Hardlink partition).
```csharp
public static bool IsReparsePoint(this string path)
```

### 🔹 `CopyFilesTo()`
**Description:** Iterates over the source directory to discover and mirror file assets into a destination folder boundary utilizing a concrete structural search query pattern filter.
```csharp
public static void CopyFilesTo(this string sourceDirectory, string destinationDirectory, string searchPattern = "*.*", bool overwrite = true)
```

---

## 📦 Class: MapExtensions

### 🔹 `BreakAllFacilityDoors()`
**Description:** Performs a comprehensive, high-performance iteration sweep across all active rooms to forcibly shatter and destroy every structural <see cref="BreakableDoor"/> instance that is not currently broken.
```csharp
public static void BreakAllFacilityDoors()
```

### 🔹 `SetAllLightsEnabled()`
**Description:** Global shortcut matrix to instantly toggle the active illumination status field of every light controller component deployed across the entire facility map topology.
```csharp
public static void SetAllLightsEnabled(bool enabled)
```

### 🔹 `GetEngagedGeneratorsCount()`
**Description:** Computes the exact real-time operational volume load metrics of facility power generators currently sitting in a fully engaged state.
```csharp
public static int GetEngagedGeneratorsCount()
```

### 🔹 `AreAllGeneratorsEngaged()`
**Description:** Evaluates with zero heap allocations whether all currently deployed power generator sub-units are fully engaged, verifying metrics against a minimum structural compliance count.
```csharp
public static bool AreAllGeneratorsEngaged(int minimumRequiredCount = 3)
```

---

## 📦 Class: MathExtensions

### 🔹 `Clamp()`
**Description:** Fluently clamps a single-precision floating-point value within specified structural minimum and maximum bounds.
```csharp
public static float Clamp(this float value, float min, float max) => Mathf.Clamp(value, min, max);
```

### 🔹 `Clamp()`
**Description:** Fluently clamps a 32-bit signed integer value within specified structural minimum and maximum bounds.
```csharp
public static int Clamp(this int value, int min, int max) => Mathf.Clamp(value, min, max);
```

### 🔹 `Clamp()`
**Description:** Fluently clamps a double-precision floating-point value within specified structural minimum and maximum bounds.
```csharp
public static double Clamp(this double value, double min, double max) => value < min ? min : (value > max ? max : value);
```

### 🔹 `LimitMin()`
**Description:** Enforces a strict minimum baseline constraint on a single-precision floating-point value.
```csharp
public static float LimitMin(this float value, float minBaseline) => Mathf.Max(minBaseline, value);
```

### 🔹 `LimitMin()`
**Description:** Enforces a strict minimum baseline constraint on a 32-bit signed integer value.
```csharp
public static int LimitMin(this int value, int minBaseline) => Mathf.Max(minBaseline, value);
```

### 🔹 `LimitMin()`
**Description:** Enforces a strict minimum baseline constraint on a double-precision floating-point value.
```csharp
public static double LimitMin(this double value, double minBaseline) => value < minBaseline ? minBaseline : value;
```

### 🔹 `IsNanOrInfinity()`
**Description:** Verifies whether the specified single-precision floating-point value evaluates to NaN or Infinity.
```csharp
public static bool IsNanOrInfinity(this float value) => float.IsNaN(value) || float.IsInfinity(value);
```

### 🔹 `IsNanOrInfinity()`
**Description:** Verifies whether the specified double-precision floating-point value evaluates to NaN or Infinity.
```csharp
public static bool IsNanOrInfinity(this double value) => double.IsNaN(value) || double.IsInfinity(value);
```

### 🔹 `IsNanOrInfinity()`
**Description:** Baseline safety fallback for integer primitives. Always returns false as integral types cannot represent NaN or Infinity states.
```csharp
public static bool IsNanOrInfinity(this int value) => false;
```

---

## 📦 Class: PickupExtensions

### 🔹 `ApplyKineticBlast()`
**Description:** Forcibly applies kinetic physical propulsion forces onto a single world-space pickup asset. Single Point of Truth for individual pickup physics manipulation.
```csharp
public static void ApplyKineticBlast(this Pickup pickup, float linearVelocityMagnitude, float angularVelocityMagnitude)
```

### 🔹 `ApplyKineticBlast()`
**Description:** Iterates over an aggregated collection layout of spawned pickups and forcibly applies batch kinetic physical propulsion forces. Seamlessly delegates item execution to maintain absolute single responsibility and zero duplication.
```csharp
public static void ApplyKineticBlast(this IEnumerable<Pickup> pickups, float linearVelocityMagnitude, float angularVelocityMagnitude)
```

---

## 📦 Class: PlayerExtensions

### 🔹 `AttachTrackingObject()`
**Description:** Seamlessly attaches any external tracking GameObject source to a live Player target utilizing native sub-frame anchor locking components.
```csharp
public static void AttachTrackingObject(this Player player, GameObject followerObject, Vector3 offset = default)
```

### 🔹 `BroadcastHintToAll()`
**Description:** Dispatches a localized streaming Hint display message layer across all currently verified, ready human and active simulation subjects simultaneously.
```csharp
public static void BroadcastHintToAll(string hintContent, float duration = 5f)
```

### 🔹 `BroadcastHint()`
**Description:** Dispatches a visual streaming Hint display message across a specific filtered collection of player instances. Defensively filters out null entities or subjects that are not fully initialized and round-ready.
```csharp
public static void BroadcastHint(this System.Collections.Generic.IEnumerable<Player> players, string hintContent, float duration = 5f)
```

### 🔹 `GetHumeShieldValue()`
**Description:** Safe, high-performance extraction wrapper designed to query the underlying structural statistical tracking systems and yield the current active value coefficient of a subject's Hume Shield asset pool.
```csharp
public static float GetHumeShieldValue(this Player player)
```

### 🔹 `IsLivingHuman()`
**Description:** Evaluates with high-performance metrics whether the targeted player instance represents a fully initialized, active human subject who is currently alive in the current round lifecycle.
```csharp
public static bool IsLivingHuman(this Player player)
```

### 🔹 `GetDistanceTo()`
**Description:** Computes the precise linear Euclidean distance between the target player's current spatial coordinates and a specified 3D position vector.
```csharp
public static float GetDistanceTo(this Player player, Vector3 position)
```

### 🔹 `IsWithinDistance()`
**Description:** Evaluates defensively whether the specified player is positioned within a concrete linear maximum distance radius from a target spatial coordinate vector.
```csharp
public static bool IsWithinDistance(this Player player, Vector3 position, float maxDistance)
```

### 🔹 `HasActiveLightSource()`
**Description:** Evaluates whether the targeted player instance is currently emitting mobile photon fields via active standalone flashlights or tactical firearm attachments.
```csharp
public static bool HasActiveLightSource(this Player player)
```

### 🔹 `IsInRoom()`
**Description:** Evaluates with absolute zero heap allocation whether the player's active room context matches any of the specified structural room layout signatures.
```csharp
public static bool IsInRoom(this Player player, params RoomName[] roomNames)
```

### 🔹 `IsInDarkRoom()`
**Description:** Evaluates whether the localized room lighting grid envelope encompassing a specific <see cref="Player"/> instance has had its active illumination disabled.
```csharp
public static bool IsInDarkRoom(this Player player)
```

### 🔹 `IsWithinRadius()`
**Description:** Performs a high-performance distance query utilizing underlying Unity vector squaring math (<c>sqrMagnitude</c>). Completely circumvents expensive processor square root calculations (<c>Math.Sqrt</c>) to validate circular perimeter limits.
```csharp
public static bool IsWithinRadius(this Player player, Vector3 targetPosition, float radiusSize)
```

### 🔹 `IsWithinAnyRadius()`
**Description:** Evaluates defensively whether the player's spatial coordinates fall within a concrete radial proximity constraint intersecting any item position stored inside an aggregated sequence matrix tracking layout coordinates.
```csharp
public static bool IsWithinAnyRadius(this Player player, IEnumerable<Vector3> positions, float radiusSize)
```

### 🔹 `WhereAlive()`
**Description:** Filters an enumerable collection layout stream of players utilizing high-performance lazy execution to yield exclusively active, ready human subjects who are currently alive in the current round lifecycle.
```csharp
public static IEnumerable<Player> WhereAlive(this IEnumerable<Player> players)
```

### 🔹 `WhereHuman()`
**Description:** Filters an enumerable collection layout stream of players utilizing high-performance lazy execution to yield exclusively player subjects belonging to human factions.
```csharp
public static System.Collections.Generic.IEnumerable<Player> WhereHuman(this System.Collections.Generic.IEnumerable<Player> players)
```

### 🔹 `WhereNotInPocket()`
**Description:** Filters an enumerable collection layout stream of players to insulate the pipeline against subjects currently trapped inside the low-level execution bounds of SCP-106's Pocket Dimension.
```csharp
public static System.Collections.Generic.IEnumerable<Player> WhereNotInPocket(this System.Collections.Generic.IEnumerable<Player> players)
```

### 🔹 `ApplySensoryShock()`
**Description:** Deploys an aggregated tactical batch of adverse visual and auditory status effect matrices simultaneously onto a single target entity. Safely skips internal duration initialization routines for any specific effect parameter mapped to zero or negative temporal scales.
```csharp
public static void ApplySensoryShock(this Player player, float flashDuration = 0.0f, float blindDuration = 0.0f, float blurDuration = 0.0f, float deafenDuration = 0.0f)
```

### 🔹 `GetRoom()`
**Description:** Explicitly resolves the live structural <see cref="Room"/> sector currently encompassing the player's real-time position vector by forwarding the query directly onto the underlying map topology grid.
```csharp
public static Room GetRoom(this Player player)
```

### 🔹 `GetDistanceTo()`
**Description:** Computes the precise linear Euclidean distance between the player's current spatial coordinates and the central transform anchor position of the targeted room entity.
```csharp
public static float GetDistanceTo(this Player player, Room room)
```

### 🔹 `IsWithinRadius()`
**Description:** Performs a high-performance cross-entity proximity validation sweep between the player and a target room center utilizing underlying Unity vector squaring math (<c>sqrMagnitude</c>) to avoid high overhead calculation paths.
```csharp
public static bool IsWithinRadius(this Player player, Room room, float radiusSize)
```

### 🔹 `IsEligibleForEscape()`
**Description:** Validates defensively whether a player entity satisfies generic escape criteria based on life state, anomalous faction restrictions, and optimized radial proximity to an extraction zone vector.
```csharp
public static bool IsEligibleForEscape(this Player player, Vector3 escapeZone, float escapeZoneSize)
```

### 🔹 `IsInShelter()`
**Description:** Evaluates with high-performance metrics whether a player is safely positioned inside a valid protection shelter infrastructure. Dynamically verifies structural room identifiers first before cascading into cached multi-point radial proximity checks.
```csharp
public static bool IsInShelter(this Player player, float shelterZoneSize, IEnumerable<Vector3> cachedShelterLocations, params RoomName[] additionalRooms)
```

---

## 📦 Class: PluginConfigExtensions

### 🔹 `LoadOrCreateSubConfig()`
**Description:** Atomically loads a decentralized sub-configuration file from the filesystem. Automatically deploys a clean fallback instance, executes validation delegates, and flushes changes to disk if missing or corrupt. <typeparam name="TMainConfig">The core configuration type binding the primary plugin layout framework inheriting from <see cref="LabApiConfig"/>.</typeparam> <typeparam name="TSubConfig">The target modular sub-configuration class layer being initialized.</typeparam>
```csharp
public static TSubConfig LoadOrCreateSubConfig<TMainConfig, TSubConfig>( this Plugin<TMainConfig> plugin, string fileName, Action<TSubConfig> validationAction = null) where TMainConfig : LabApiConfig, new() where TSubConfig : class, new()
```

---

## 📦 Class: RoomExtensions

### 🔹 `GetNeighbors()`
**Description:** Resolves and collects all valid physically adjacent neighboring <see cref="Room"/> nodes connected directly to this room instance.
```csharp
public static IEnumerable<Room> GetNeighbors(this Room room)
```

### 🔹 `WhereNotInPocket()`
**Description:** Filters an enumerable collection stream of rooms to insulate the pipeline against sectors representing the unstable spatial bounds of SCP-106's Pocket Dimension.
```csharp
public static IEnumerable<Room> WhereNotInPocket(this IEnumerable<Room> rooms)
```

### 🔹 `IsCheckpoint()`
**Description:** Evaluates if a specific structural <see cref="RoomName"/> token corresponds directly to a tactical zone checkpoint airlock node.
```csharp
public static bool IsCheckpoint(this RoomName roomName) => roomName is RoomName.LczCheckpointA or RoomName.LczCheckpointB or RoomName.HczCheckpointA or RoomName.HczCheckpointB or RoomName.HczCheckpointToEntranceZone;
```

### 🔹 `IsScpRoom()`
**Description:** Determines whether the designated <see cref="RoomName"/> spatial layout topology represents a secure containment sector for anomalous entities.
```csharp
public static bool IsScpRoom(this RoomName roomName) => roomName is RoomName.Lcz173 or RoomName.Lcz330 or RoomName.Hcz049 or RoomName.Hcz079 or RoomName.Hcz096 or RoomName.Hcz106 or RoomName.Hcz939 or RoomName.Lcz914 or RoomName.HczTestroom;
```

### 🔹 `IsArmory()`
**Description:** Checks if the designated <see cref="RoomName"/> structural context is classified as a high-security tactical weapons or munitions armory depot.
```csharp
public static bool IsArmory(this RoomName roomName) => roomName is RoomName.LczArmory or RoomName.HczArmory;
```

### 🔹 `IsFreeOfEngagedGenerators()`
**Description:** Verifies defensively if the target <see cref="Room"/> spatial zone is completely free of any power generators that are currently in an active, fully engaged state.
```csharp
public static bool IsFreeOfEngagedGenerators(this Room room)
```

### 🔹 `IsRoomAndNeighborsFreeOfEngagedGenerators()`
**Description:** Performs an aggregated spatial validation sweep across the target <see cref="Room"/> and all physically connected adjacent neighbor zones to confirm absolute grid isolation from any active, fully engaged power generators.
```csharp
public static bool IsRoomAndNeighborsFreeOfEngagedGenerators(this Room room)
```

### 🔹 `TurnOffLights()`
**Description:** Forcibly suppresses the active illumination controllers across the specified room topology for a precise timeframe. Insulation Upgrade: Mechanical elevator door override routines removed to preserve Single Responsibility Principle (SRP).
```csharp
public static void TurnOffLights(this Room room, float duration)
```

### 🔹 `TurnOnLights()`
**Description:** Fluent API DRY Refactor: Restores electrical power to the room's lighting grid controllers and optionally triggers a brief flicker sequence.
```csharp
public static void TurnOnLights(this Room room, float flickerDuration = 0f)
```

### 🔹 `TurnOffRoomAndNeighborLights()`
**Description:** Forcibly suppresses illumination across this room and all physically connected adjacent neighboring rooms simultaneously for a designated duration track.
```csharp
public static void TurnOffRoomAndNeighborLights(this Room room, float duration, bool forced = false)
```

### 🔹 `TurnOnRoomAndNeighborLights()`
**Description:** Restores active electrical power and forces an optional brief flickering update sequence across this room and all adjacent neighbors.
```csharp
public static void TurnOnRoomAndNeighborLights(this Room room, float duration = 0f)
```

### 🔹 `SetLightsColor()`
**Description:** Fluently overrides the active rendering illumination color spectrum channel variables for a specific room.
```csharp
public static void SetLightsColor(this Room room, Color color)
```

### 🔹 `SetLightsColor()`
**Description:** Systematically executes a batch color spectrum override sweep across an aggregated collection sequence of rooms.
```csharp
public static void SetLightsColor(this IEnumerable<Room> rooms, Color color)
```

### 🔹 `ExecuteActionOnRoomAndNeighbors()`
**Description:** Executes a specified procedural action delegate graph across a localized room anchor point and seamlessly propagates the delegate pattern execution out into all adjacent physical room nodes safely.
```csharp
public static void ExecuteActionOnRoomAndNeighbors(this Room room, Action<Room> action)
```

### 🔹 `BreakAllDoors()`
**Description:** Iterates over all door sub-components bound to the target room context to safely locate and fracture any breakable barriers (<see cref="BreakableDoor"/>) that remain intact.
```csharp
public static void BreakAllDoors(this Room room)
```

### 🔹 `GetRoom()`
**Description:** Extension method on <see cref="Vector3"/> to seamlessly resolve and fetch the live <see cref="Room"/> instance encompassing the targeted coordinates layer directly from the underlying map topology grid.
```csharp
public static Room GetRoom(this Vector3 position)
```

### 🔹 `GetDistanceTo()`
**Description:** Computes the precise linear Euclidean distance between the structural transform center position of the room asset and a targeted raw 3D position vector coordinate.
```csharp
public static float GetDistanceTo(this Room room, Vector3 position)
```

### 🔹 `IsWithinRadius()`
**Description:** Performs a high-performance proximity validation query tracking from the room's transform center point utilizing underlying Unity vector squaring math (<c>sqrMagnitude</c>) to avoid high overhead calculation paths.
```csharp
public static bool IsWithinRadius(this Room room, Vector3 position, float radiusSize)
```

### 🔹 `FlickerLightsCoroutine()`
**Description:** Fluently executes a synchronized asynchronous lighting flicker animation loop over an individual <see cref="Room"/> instance.
```csharp
public static IEnumerator<float> FlickerLightsCoroutine(this Room room, Color color, float duration, float frequency)
```

### 🔹 `FlickerBulkLightsCoroutine()`
**Description:** Fluently executes a global or batch collection wide lighting flicker animation loop sequence spanning multiple rooms simultaneously.
```csharp
public static IEnumerator<float> FlickerBulkLightsCoroutine(this IEnumerable<Room> rooms, Color color, float duration, float frequency)
```

---

## 📦 Class: StringExtensions

### 🔹 `NormalizeUserId()`
**Description:** Enforces standard lowercase invariant formatting on a raw network identifier token, mitigating platform-specific auth casing anomalies and dictionary key mismatches.
```csharp
public static string NormalizeUserId(this string userId)
```

---

## 📦 Class: TeslaExtensions

### 🔹 `DisableFor()`
**Description:** Forcibly puts a specific Tesla gate into an inactive operational cooldown state for a designated time window.
```csharp
public static void DisableFor(this Tesla tesla, float duration, bool forceTrigger = true)
```

### 🔹 `SetInactiveTimeForAll()`
**Description:** Iterates over a collection layout of Tesla gates to reset their active cooldown timers or force a temporary tactical shutdown loop.
```csharp
public static void SetInactiveTimeForAll(this IEnumerable<Tesla> teslas, float duration, bool forceTrigger = true)
```

### 🔹 `Disable()`
**Description:** High-level semantic shortcut to instantly deactivate an entire collection profile of Tesla gates with an optional chronological duration envelope. Seamlessly delegates execution to the underlying structural state engine to maintain absolute single responsibility.
```csharp
public static void Disable(this IEnumerable<Tesla> teslas, float duration = 5.0f, bool forceTrigger = false)
```

---

## 📦 Class: TimingExtensions

### 🔹 `CallDelayedIf()`
**Description:** Executes a specified action after a structural delay window, but only if a designated state validation condition evaluates to true at the exact moment of execution.
```csharp
public static CoroutineHandle CallDelayedIf(float delay, Func<bool> condition, Action action, string coroutineTag = null)
```

### 🔹 `KillCoroutine()`
**Description:** Forcibly terminates the active coroutine tracking instance bound to this specific string token identifier safely.
```csharp
public static void KillCoroutine(this string tag)
```

### 🔹 `Kill()`
**Description:** Defensively verifies whether the individual coroutine handle is actively running and forcibly terminates its lifetime thread.
```csharp
public static void Kill(this CoroutineHandle handle)
```

### 🔹 `KillCoroutines()`
**Description:** Systematically terminates all active running coroutine execution tracks mapped to an aggregated collection layout of string tokens.
```csharp
public static void KillCoroutines(this IEnumerable<string> tags)
```

### 🔹 `Kill()`
**Description:** Systematically iterates over a collection stream of coroutine handles to terminate each active executing thread safely.
```csharp
public static void Kill(this IEnumerable<CoroutineHandle> handles)
```

### 🔹 `KillAndClear()`
**Description:** Systematically terminates all running coroutine handles stored within the list layout utilizing zero heap allocation indexing loops, and flushes the cache.
```csharp
public static void KillAndClear(this List<CoroutineHandle> coroutines)
```

---

## 📦 Class: VectorExtensions

### 🔹 `GetDistanceTo()`
**Description:** Computes the exact linear Euclidean distance between the source vector and a targeted coordinate position.
```csharp
public static float GetDistanceTo(this Vector3 origin, Vector3 target)
```

### 🔹 `GetDistanceSquaredTo()`
**Description:** Computes the rapid squared distance magnitude between two spatial points, bypassing expensive processor square-root calculation chains.
```csharp
public static float GetDistanceSquaredTo(this Vector3 origin, Vector3 target)
```

### 🔹 `GetUpwardReflectedVector()`
**Description:** Evaluates if the source direction vector points too steeply downwards against a threshold and forcibly reflects it upward utilizing normal plane mirroring algorithms.
```csharp
public static UnityEngine.Vector3 GetUpwardReflectedVector(this UnityEngine.Vector3 direction, float dotThreshold = 0.707f)
```

### 🔹 `GetRandomUpwardSphereVelocity()`
**Description:** Generates a random mathematical directional unit vector across a spherical layout, completely insulated against downward trajectory drops into floor topologies.
```csharp
public static UnityEngine.Vector3 GetRandomUpwardSphereVelocity(float magnitude = 1f)
```

### 🔹 `Sanitize()`
**Description:** Defensively audits all float components of a 3D vector against structural corruption anomalies (NaN or Infinity parameters).
```csharp
public static UnityEngine.Vector3 Sanitize(this UnityEngine.Vector3 vector, UnityEngine.Vector3 fallback = default)
```

---

## 📦 Class: ZoneExtensions

### 🔹 `UnknownMember()`
**Description:** Cached array containing all valid facility zone tokens to eliminate runtime enum allocation overhead.
```csharp
public static readonly FacilityZone[] All = (FacilityZone[])Enum.GetValues(typeof(FacilityZone));
```

### 🔹 `GetDoors()`
**Description:** Resolves and aggregates all <see cref="Door"/> instances deployed within the specified facility zone boundary.
```csharp
public static IEnumerable<Door> GetDoors(this FacilityZone zone)
```

### 🔹 `OpenDoors()`
**Description:** Forcibly unseals all doors located within the targeted facility zone boundary.
```csharp
public static void OpenDoors(this FacilityZone zone, bool bypassLocks = false)
```

### 🔹 `CloseDoors()`
**Description:** Forcibly seals all doors located within the targeted facility zone boundary.
```csharp
public static void CloseDoors(this FacilityZone zone, bool bypassLocks = false)
```

### 🔹 `SetDoorsLockState()`
**Description:** Updates the administrative lock state for all doors across an entire facility zone under a specific lock reason.
```csharp
public static void SetDoorsLockState(this FacilityZone zone, DoorLockReason reason, bool locked = true)
```

### 🔹 `TurnOffLights()`
**Description:** Forcibly suppresses all layout light controllers and connected elevator cabins across an entire facility zone for a designated duration.
```csharp
public static void TurnOffLights(this FacilityZone zone, float duration)
```

### 🔹 `TurnOnLights()`
**Description:** Instantly restores electrical power to all light controllers and connected elevator cabins across a specific facility zone.
```csharp
public static void TurnOnLights(this FacilityZone zone)
```

### 🔹 `TurnOffLights()`
**Description:** Forcibly suppresses illumination across a collection sequence of facility zones simultaneously.
```csharp
public static void TurnOffLights(this IEnumerable<FacilityZone> zones, float duration)
```

### 🔹 `TurnOnLights()`
**Description:** Instantly restores operational grid power parameters across an entire collection sequence of facility zones.
```csharp
public static void TurnOnLights(this IEnumerable<FacilityZone> zones)
```

### 🔹 `FlickerLightsCoroutine()`
**Description:** Fluently executes a batch synchronized visual lighting flicker animation sequence across a concrete <see cref="FacilityZone"/>.
```csharp
public static IEnumerator<float> FlickerLightsCoroutine(this FacilityZone targetZone, Color color, float duration, float frequency)
```

---

## 📦 Class: EnvironmentEngine

### 🔹 `StartEmergencyStrobe()`
**Description:** Launches a non-blocking background asynchronous coroutine loop that flashes the global facility lighting grid between a targeted alert color configuration and a total blackout pulse state.
```csharp
public static void StartEmergencyStrobe(float totalDuration, float pulseInterval, Color alertColor, string masterTag = "Emergency-Strobe")
```

### 🔹 `StartZoneFlicker()`
**Description:** Launches a non-blocking background asynchronous coroutine loop that flickers the illumination grid of a specific facility zone at a given frequency baseline before reverting to pristine spectrum maps.
```csharp
public static void StartZoneFlicker(MapGeneration.FacilityZone zone, float duration, float frequency, UnityEngine.Color color, string coroutineTag = "Zone-Flicker")
```

---

## 📦 Class: EscapeEngine

### 🔹 `RunScenarioRoutine()`
**Description:** Runs a comprehensive, non-blocking asynchronous evacuation routine driven entirely by the provided scenario parameters matrix.
```csharp
public static IEnumerator<float> RunScenarioRoutine(EscapeScenario scenario)
```

---

## 📦 Class: EscapeScenario

### 🔹 `Name`
**Description:** Gets or sets the unique structural identity or display label assigned to this execution scenario.
```csharp
public string Name { get; set; } = "Default Evacuation";
```

### 🔹 `EscapeZone`
**Description:** Gets or sets the global world-space coordinate vector defining the center anchor of the escape verification perimeter.
```csharp
public Vector3 EscapeZone { get; set; } = Vector3.zero;
```

### 🔹 `EscapeRadius`
**Description:** Gets or sets the maximum linear radial boundary threshold constraint in meters allowed to pass proximity verification.
```csharp
public float EscapeRadius { get; set; } = 5f;
```

### 🔹 `TeleportPosition`
**Description:** Gets or sets the target destination coordinate vector where eligible entities are transferred post-validation.
```csharp
public Vector3 TeleportPosition { get; set; } = Vector3.zero;
```

### 🔹 `InitialHint`
**Description:** Gets or sets the localized notification Hint text sequence broadcast to the entire server pool when the timeline initiates.
```csharp
public string InitialHint { get; set; }
```

### 🔹 `InitialHintDuration`
**Description:** Gets or sets the visual display lifetime duration in seconds allocated for the initial broadcast interface layer.
```csharp
public float InitialHintDuration { get; set; } = 6f;
```

### 🔹 `SuccessHint`
**Description:** Gets or sets the direct text notification string transmitted exclusively to an individual entity upon successful scenario verification.
```csharp
public string SuccessHint { get; set; }
```

### 🔹 `InitialDelay`
**Description:** Gets or sets the temporal operational standby delay in seconds executed immediately after sequence activation.
```csharp
public float InitialDelay { get; set; } = 0f;
```

### 🔹 `ProcessingDelay`
**Description:** Gets or sets the secondary simulation processing delay in seconds spent during structural gameplay transition steps.
```csharp
public float ProcessingDelay { get; set; } = 0f;
```

### 🔹 `FinalDelay`
**Description:** Gets or sets the brief terminal resolution delay in seconds applied directly before role transformation commits.
```csharp
public float FinalDelay { get; set; } = 0.5f;
```

### 🔹 `FlashDuration`
**Description:** Gets or sets the duration coefficient in seconds for the post-escape screen flash trauma overlay.
```csharp
public float FlashDuration { get; set; } = 1.75f;
```

### 🔹 `BlindDuration`
**Description:** Gets or sets the duration coefficient in seconds for the post-escape structural blindness status block.
```csharp
public float BlindDuration { get; set; } = 0f;
```

### 🔹 `BlurDuration`
**Description:** Gets or sets the duration coefficient in seconds for the post-escape visual lens blur distortion layer.
```csharp
public float BlurDuration { get; set; } = 0f;
```

### 🔹 `DeafenDuration`
**Description:** Gets or sets the duration coefficient in seconds for the post-escape environment auditory dampening filter.
```csharp
public float DeafenDuration { get; set; } = 3.75f;
```

### 🔹 `OnSequenceStarted`
**Description:** Occurs instantly when the master execution pipeline initializes the scenario tracking tree.
```csharp
public Action OnSequenceStarted { get; set; }
```

### 🔹 `OnSequenceProcessing`
**Description:** Occurs immediately after the initial standby delay threshold expires, separating arrival steps from processing sweeps.
```csharp
public Action OnSequenceProcessing { get; set; }
```

### 🔹 `OnPlayerValidated`
**Description:** Occurs dynamically whenever an individual player entity successfully passes position, alignment, and structural life-state audits. Ideal for custom external data caching, point allocations, or inventory tracking mutations.
```csharp
public Action<Player> OnPlayerValidated { get; set; }
```

### 🔹 `OnSequenceFinalized`
**Description:** Occurs upon complete finalization of the sequence loop iteration sweep.
```csharp
public Action OnSequenceFinalized { get; set; }
```

---

## 📦 Class: StringInterpretationExtensions

### 🔹 `InterpretEnum()`
**Description:** Fluently interprets a raw string input and resolves it against an enum layout using multi-stage heuristic cascades. <typeparam name="T">The target value mapping constraint conforming to standard system enums.</typeparam>
```csharp
public static InterpretationResult<T> InterpretEnum<T>(this string input) where T : struct, Enum
```

---

## 📦 Class: SafeRandom

### 🔹 `Next()`
**Description:** Generates a thread-safe pseudo-random signed 32-bit integer within a concrete range boundary.
```csharp
public static int Next(int min, int max) => ThreadRandom.Next(min, max);
```

### 🔹 `NextDouble()`
**Description:** Generates a thread-safe pseudo-random double-precision floating-point scalar token tracking between 0.0 and 1.0.
```csharp
public static double NextDouble() => ThreadRandom.NextDouble();
```

### 🔹 `Range()`
**Description:** Computes a single-precision random floating-point scalar value mapped across a localized linear scaling envelope bridging the specified minimum and maximum mathematical limits.
```csharp
public static float Range(float min, float max) => (float)(NextDouble() * (max - min) + min);
```

### 🔹 `RollSuccess()`
**Description:** Performs a thread-safe probability evaluation roll against a specified percentage value (0.0 to 100.0).
```csharp
public static bool RollSuccess(this float chancePercentage)
```

---

## 📦 Class: SpatialMap

### 🔹 `GetRandomElevator()`
**Description:** Retrieves a pseudo-randomly selected <see cref="Elevator"/> instance from the active global facility map registry.
```csharp
public static Elevator GetRandomElevator() => Map.GetRandomElevator();
```

### 🔹 `GetRoomAtPosition()`
**Description:** Resolves and maps a concrete 3D spatial coordinate vector onto its corresponding active <see cref="Room"/> architectural sector containment bounds.
```csharp
public static Room GetRoomAtPosition(Vector3 position) => Room.GetRoomAtPosition(position);
```

### 🔹 `GetPlayer()`
**Description:** Resolves a high-level wrapper instance of <see cref="Player"/> bound directly to a native internal engine <see cref="ReferenceHub"/> identifier token.
```csharp
public static Player GetPlayer(ReferenceHub referenceHub) => Player.Get(referenceHub);
```

### 🔹 `GetRagdoll()`
**Description:** Resolves a high-level wrapped <see cref="Ragdoll"/> entity structural instance directly from a native low-level Unity engine <see cref="PlayerRoles.Ragdolls.BasicRagdoll"/> component context.
```csharp
public static Ragdoll GetRagdoll(PlayerRoles.Ragdolls.BasicRagdoll ragdoll) => Ragdoll.Get(ragdoll);
```

---

## 📦 Class: iLogger

### 🔹 `Info()`
**Description:** Dispatches a standardized informational telemetry entry directly onto the global console stream interface.
```csharp
public static void Info(string message) => Logger.Info(message);
```

### 🔹 `Info()`
**Description:** Dispatches an informational trace message bound into a discrete, bracketed subsystem contextual source tag.
```csharp
public static void Info(string source, string message) => Logger.Info($"[{source}] {message}");
```

### 🔹 `Warn()`
**Description:** Captures a non-fatal anomaly alert execution sequence or automated threshold correction trace to indicate soft system variances.
```csharp
public static void Warn(string message) => Logger.Warn(message);
```

### 🔹 `Warn()`
**Description:** Captures a non-fatal anomaly alert execution sequence bound within a distinct tracking domain header tag.
```csharp
public static void Warn(string source, string message) => Logger.Warn($"[{source}] {message}");
```

### 🔹 `Error()`
**Description:** Records a critical runtime failure path or exceptional pipeline interruption that severely compromises system integrity.
```csharp
public static void Error(string message) => Logger.Error(message);
```

### 🔹 `Error()`
**Description:** Records a critical runtime failure trace wrapped inside a contextual tracking system exception identifier tag.
```csharp
public static void Error(string source, string message) => Logger.Error($"[{source}] {message}");
```

### 🔹 `Debug()`
**Description:** Routes a low-level diagnostic trace message onto the console output pipe conditionally based on a production runtime state evaluation block. Safely insulates live server allocations against verbose data pollution during standard execution deployments.
```csharp
public static void Debug(string message, bool isDebugEnabled)
```

### 🔹 `Debug()`
**Description:** Routes a contextual low-level trace sequence wrapped inside an explicit subsystem domain tag if live diagnostics evaluate to true.
```csharp
public static void Debug(string source, string message, bool isDebugEnabled)
```

### 🔹 `LocalTrace()`
**Description:** Executes an aggressive, highly detailed debugging pipeline trace. Active exclusively during local local IDE sandbox compilation phases. Completely omitted and stripped out by the compiler in release builds to guarantee absolute zero deployment allocation footprints.
```csharp
public static void LocalTrace(string source, string message)
```

---

## 📦 Class: RoundController

### 🔹 `IsRoundEndLocked`
**Description:** Gets a value indicating whether the native server automatic round evaluation loop is currently locked out.
```csharp
public bool IsRoundEndLocked => _autoRoundEndLocked;
```

### 🔹 `EndingScenarios`
**Description:** Gets an enumerable matrix stream tracking all currently registered architectural scenario nodes.
```csharp
public IEnumerable<RoundScenario> EndingScenarios => _registeredScenarios;
```

### 🔹 `RegisterScenario()`
**Description:** Commits a concrete scenario node structure into the internal tracking state registry.
```csharp
public void RegisterScenario(RoundScenario scenario)
```

### 🔹 `SetAutoRoundEndLock()`
**Description:** Locks or unlocks native automatic round ending routines utilizing underlying LabAPI framework properties.
```csharp
public void SetAutoRoundEndLock(bool locked)
```

### 🔹 `ExecuteScenario()`
**Description:** Enforces an administrative lockout on standard round termination and executes the targeted scenario logic stream immediately.
```csharp
public void ExecuteScenario(RoundScenario scenario)
```

### 🔹 `EndRoundGracefully()`
**Description:** Gracefully terminates the round lifecycle sequence after a deferred temporal delay window, forcing execution metrics.
```csharp
public void EndRoundGracefully(float delay = 0f, string coroutineTag = "CustomRoundEnd")
```

### 🔹 `GetScenario()`
**Description:** Queries the registered scenario registry matrix to resolve and yield a specific strongly-typed scenario implementation.
```csharp
public T GetScenario<T>() where T : RoundScenario
```

---

