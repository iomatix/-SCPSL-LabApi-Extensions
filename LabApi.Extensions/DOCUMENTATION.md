# Audio Manager API - Architecture API Registry

## 📦 Class: ActionChainExtensions

### 🔹 `CreateChain()`
**Description:** Initiates a clean, type-safe, fluent action orchestration chain bound to this target instance context.
```csharp
public static ActionChain<T> CreateChain<T>(this T target) where T : class
```

---

## 📦 Class: AssemblyExtensions

### 🔹 `FindEmbeddedAsset()`
**Description:** Dynamically resolves an embedded manifest resource pathway matching primary identifiers or structural fallback names.
```csharp
public static string FindEmbeddedAsset(this Assembly assembly, string primaryKey, string fileExtension = ".wav", params string[] alternativeTokens)
```

---

## 📦 Class: CassieExtensions

### 🔹 `SanitizeCassieString()`
**Description:** Sanitizes a raw CASSIE string by removing carriage returns, replacing newlines with spaces, and trimming whitespace.
```csharp
public static string SanitizeCassieString(this string rawMessage) => string.IsNullOrWhiteSpace(rawMessage) ? string.Empty : rawMessage.Replace("\r", "").Replace("\n", " ").Trim();
```

### 🔹 `DispatchGlitchyMessage()`
**Description:** Glitchifies the specified message internally and dispatches the announcement, returning its playback duration.
```csharp
public static double DispatchGlitchyMessage(string message, float glitchChance, float jamChance)
```

### 🔹 `DispatchMessage()`
**Description:** Dispatches a standard CASSHE announcement and returns its estimated playback duration using the provided modifiers.
```csharp
public static double DispatchMessage(string message, CassiePlaybackModifiers modifiers = default)
```

### 🔹 `ProcessAndDispatchMessage()`
**Description:** Sanitizes, formats, and dispatches a CASSIE announcement with custom subtitles and queue priority.
```csharp
public static void ProcessAndDispatchMessage(string message, string customSubtitles, bool shouldClear, float priority, bool disableMessages = false, CassiePlaybackModifiers modifiers = default)
```

### 🔹 `DispatchMessage()`
**Description:** Dispatches a collection of messages sequentially using the specified playback modifiers.
```csharp
public static void DispatchMessage(IEnumerable<string> messages, CassiePlaybackModifiers modifiers = default)
```

### 🔹 `DispatchMessage()`
**Description:** Dispatches an inline array of messages sequentially using default playback modifiers.
```csharp
public static void DispatchMessage(params string[] messages) => DispatchMessage(messages, default);
```

### 🔹 `DispatchMessage()`
**Description:** Dispatches an inline array of messages sequentially using the specified playback modifiers.
```csharp
public static void DispatchMessage(CassiePlaybackModifiers modifiers, params string[] messages)
```

### 🔹 `ToCassieCountdown()`
**Description:** Converts a remaining time integer into a formatted CASSIE countdown announcement string.
```csharp
public static string ToCassieCountdown(this int notifyTime, string alertContext = "seconds until event detonation")
```

### 🔹 `CalculateCassieMessageDuration()`
**Description:** Calculates the playback duration of a single CASSIE message using the provided modifiers.
```csharp
public static double CalculateCassieMessageDuration(string message, CassiePlaybackModifiers modifiers = default)
```

### 🔹 `CalculateTotalMessagesDurations()`
**Description:** Calculates the total duration of a dictionary of messages, mapping each float value to a modifier pitch.
```csharp
public static double CalculateTotalMessagesDurations(IDictionary<string, float> messageSpeedDictionary)
```

### 🔹 `CalculateTotalMessagesDurations()`
**Description:** Calculates the cumulative playback duration for a collection of messages using the specified modifiers.
```csharp
public static double CalculateTotalMessagesDurations(IEnumerable<string> messages, CassiePlaybackModifiers modifiers = default)
```

### 🔹 `CalculateTotalMessagesDurations()`
**Description:** Calculates the cumulative playback duration for an inline array of messages using default modifiers.
```csharp
public static double CalculateTotalMessagesDurations(params string[] messages) => CalculateTotalMessagesDurations(messages, default);
```

### 🔹 `CalculateTotalMessagesDurations()`
**Description:** Calculates the cumulative playback duration for an inline array of messages using the specified modifiers.
```csharp
public static double CalculateTotalMessagesDurations(CassiePlaybackModifiers modifiers, params string[] messages)
```

---

## 📦 Class: CollectionExtensions

### 🔹 `ExecuteThrottled()`
**Description:** Evaluates a temporal cooldown window for a specific key profile. If the tracking node is ready, executes the payload and automatically commits the new timestamp. <typeparam name="TKey">The structural identity lookup key tracking type.</typeparam>
```csharp
public static bool ExecuteThrottled<TKey>(this IDictionary<TKey, DateTime> cooldownMap, TKey key, TimeSpan window, Action throttleAction)
```

### 🔹 `PruneExpired()`
**Description:** Systematically purges all historical mapping entries whose tracking timestamps have safely elapsed past a specific temporal comparison index. Mitigates collection modification exceptions by isolating target records dynamically via zero-allocation memory pooling. <typeparam name="TKey">The structural identity lookup key tracking type.</typeparam>
```csharp
public static void PruneExpired<TKey>(this IDictionary<TKey, DateTime> dictionary, DateTime comparisonTime)
```

### 🔹 `IsCooldownActive()`
**Description:** Evaluates defensively whether a specific key profile is currently locked within an active temporal cooldown window. <typeparam name="TKey">The structural identity lookup key tracking type.</typeparam>
```csharp
public static bool IsCooldownActive<TKey>(this IDictionary<TKey, DateTime> cooldownMap, TKey key)
```

### 🔹 `TryAcquireLock()`
**Description:** Atomically evaluates a temporal gate check for a specific key target. If the lock window has elapsed, automatically commits a new future expiration milestone and grants authorization. <typeparam name="TKey">The structural identity lookup key tracking type.</typeparam>
```csharp
public static bool TryAcquireLock<TKey>(this IDictionary<TKey, DateTime> cooldownMap, TKey key, TimeSpan lockWindow)
```

---

## 📦 Class: CommandExtensions

### 🔹 `ConfirmPermission()`
**Description:** Defensively verifies if the command sender possesses the required administrative permission tracking profile. Automatically grants authorization if the command originated from the native Server Console.
```csharp
public static bool ConfirmPermission(this ICommandSender sender, PlayerPermissions permission, out string errorResponse)
```

### 🔹 `TryGetFloat()`
**Description:** Safely attempts to extract and parse a single-precision floating-point scalar variable from the raw arguments segment layout.
```csharp
public static bool TryGetFloat(this ArraySegment<string> arguments, int index, out float value, float minValue = float.MinValue)
```

### 🔹 `TryGetInt()`
**Description:** Safely attempts to extract and parse a signed 32-bit integer data variable from the raw arguments segment layout.
```csharp
public static bool TryGetInt(this ArraySegment<string> arguments, int index, out int value, int minValue = int.MinValue)
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
**Description:** Filters a sequence of doors to return only those matching any of the specified <see cref="DoorName"/> tokens. Leverages lazy streaming pipelines to guarantee absolute zero heap allocation metrics.
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
**Description:** Mutates the passage activation status (Open or Closed topology) of an individual door instance. This method acts as the single source of truth for all door state mutations.
```csharp
public static void SetOpenState(this Door door, bool opened, bool bypassLocks = false)
```

### 🔹 `OpenAndLock()`
**Description:** Forcibly unseals an individual door and applies an administrative server-side lock state under a specific structural reason constraint.
```csharp
public static void OpenAndLock(this Door door, DoorLockReason reason, bool playSound = true)
```

### 🔹 `Open()`
**Description:** Attempts to mass unseal a collection of doors cleanly.
```csharp
public static void Open(this IEnumerable<Door> doors, bool bypassLocks = false)
```

### 🔹 `Open()`
**Description:** Attempts to mass unseal an inline array of doors cleanly.
```csharp
public static void Open(bool bypassLocks, params Door[] doors)
```

### 🔹 `Close()`
**Description:** Attempts to mass seal a collection of doors cleanly.
```csharp
public static void Close(this IEnumerable<Door> doors, bool bypassLocks = false)
```

### 🔹 `Close()`
**Description:** Attempts to mass seal an inline array of doors cleanly.
```csharp
public static void Close(bool bypassLocks, params Door[] doors)
```

### 🔹 `SetLockState()`
**Description:** Forcibly updates the administrative server-side lock state across a collection of doors.
```csharp
public static void SetLockState(this IEnumerable<Door> doors, DoorLockReason reason, bool locked = true)
```

### 🔹 `SetLockState()`
**Description:** Forcibly updates the administrative server-side lock state across an inline array of doors.
```csharp
public static void SetLockState(DoorLockReason reason, bool locked, params Door[] doors)
```

### 🔹 `SetOpenState()`
**Description:** Attempts to mass mutate the passage activation status across a collection of doors.
```csharp
public static void SetOpenState(this IEnumerable<Door> doors, bool opened, bool bypassLocks = false)
```

### 🔹 `SetOpenState()`
**Description:** Attempts to mass mutate the passage activation status across an inline array of doors.
```csharp
public static void SetOpenState(bool opened, bool bypassLocks, params Door[] doors)
```

### 🔹 `OpenAndLock()`
**Description:** Forcibly unseals a collection of doors and applies a server-side lock state.
```csharp
public static void OpenAndLock(this IEnumerable<Door> doors, DoorLockReason reason, bool playSound = true)
```

### 🔹 `OpenAndLock()`
**Description:** Forcibly unseals an inline array of doors and applies a server-side lock state.
```csharp
public static void OpenAndLock(DoorLockReason reason, bool playSound, params Door[] doors)
```

### 🔹 `IsElevatorDoor()`
**Description:** Performs hierarchical component reflection and token verification on the native GameObject metadata to isolate whether the asset behaves structurally as an elevator cabin bulkhead.
```csharp
public static bool IsElevatorDoor(this Door door)
```

### 🔹 `CheckAndRestoreElevatorDoorState()`
**Description:** Evaluates and corrects the open state of an elevator door after a lock release event. If the elevator is physically present at the deck and unlocked, the doors are automatically restored to their open state.
```csharp
public static void CheckAndRestoreElevatorDoorState(this Door door)
```

### 🔹 `IsGate()`
**Description:** Scans the underlying engine transform string identifiers to evaluate if the targeted runtime door asset is classified as a heavy checkpoint airlock Gate structure.
```csharp
public static bool IsGate(this Door door)
```

---

## 📦 Class: EffectExtensions

### 🔹 `EnableEffect()`
**Description:** Enables a specific status effect on a single player.
```csharp
public static void EnableEffect(this Player player, FacilityEffectType effect, byte intensity = 1, float duration = 0f)
```

### 🔹 `EnableEffect()`
**Description:** Enables a specific status effect on a collection of players.
```csharp
public static void EnableEffect(this IEnumerable<Player> players, FacilityEffectType effect, byte intensity = 1, float duration = 0f)
```

### 🔹 `EnableEffect()`
**Description:** Enables a specific status effect on an inline array of players using default intensity and duration.
```csharp
public static void EnableEffect(FacilityEffectType effect, params Player[] players) => EnableEffect(players, effect);
```

### 🔹 `EnableEffect()`
**Description:** Enables a specific status effect on an inline array of players with custom intensity and duration.
```csharp
public static void EnableEffect(FacilityEffectType effect, byte intensity, float duration, params Player[] players)
```

---

## 📦 Class: ElevatorExtensions

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

### 🔹 `OpenActiveDoors()`
**Description:** Opens active floor doors for a collection of elevators.
```csharp
public static void OpenActiveDoors(this IEnumerable<Elevator> elevators, bool bypassLocks = false)
```

### 🔹 `OpenActiveDoors()`
**Description:** Opens active floor doors for an inline array of elevators.
```csharp
public static void OpenActiveDoors(bool bypassLocks, params Elevator[] elevators)
```

### 🔹 `CloseActiveDoors()`
**Description:** Closes active floor doors for a collection of elevators.
```csharp
public static void CloseActiveDoors(this IEnumerable<Elevator> elevators, bool bypassLocks = false)
```

### 🔹 `CloseActiveDoors()`
**Description:** Closes active floor doors for an inline array of elevators.
```csharp
public static void CloseActiveDoors(bool bypassLocks, params Elevator[] elevators)
```

### 🔹 `TurnOffLights()`
**Description:** Turns off lights for a collection of elevators for the specified duration.
```csharp
public static void TurnOffLights(this IEnumerable<Elevator> elevators, float duration)
```

### 🔹 `TurnOffLights()`
**Description:** Turns off lights for an inline array of elevators for the specified duration.
```csharp
public static void TurnOffLights(float duration, params Elevator[] elevators)
```

### 🔹 `TurnOnLights()`
**Description:** Turns on lights for a collection of elevators.
```csharp
public static void TurnOnLights(this IEnumerable<Elevator> elevators)
```

### 🔹 `TurnOnLights()`
**Description:** Turns on lights for an inline array of elevators.
```csharp
public static void TurnOnLights(params Elevator[] elevators)
```

### 🔹 `GetElevator()`
**Description:** Resolves the high-level <see cref="Elevator"/> wrapper associated with this specific door instance. Supports both wrapper casting and direct native component resolution fallbacks.
```csharp
public static Elevator GetElevator(this Door door)
```

### 🔹 `GetElevators()`
**Description:** Gets all active elevators associated with the specified facility zone destination.
```csharp
public static IEnumerable<Elevator> GetElevators(this FacilityZone zone)
```

### 🔹 `GetElevatorsInZone()`
**Description:** Retrieves a filtered sequence of active elevator modules whose current destination grids map directly to a target facility zone boundary.
```csharp
public static IEnumerable<Elevator> GetElevatorsInZone(FacilityZone zone) => zone.GetElevators();
```

### 🔹 `GetElevatorsConnectedToRoom()`
**Description:** Isolates and filters the global elevator tracking arrays to return only the specific units structurally bridging into the target room.
```csharp
public static IEnumerable<Elevator> GetElevatorsConnectedToRoom(this Room room)
```

### 🔹 `IsElevatorAtDoorLevel()`
**Description:** Verifies whether the associated elevator cabin is currently physically aligned with this specific door's vertical deck level. Used comprehensively across door interaction layers to prevent void access exploits.
```csharp
public static bool IsElevatorAtDoorLevel(this Door door)
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

### 🔹 `LockElevators()`
**Description:** Enforces absolute structural lockdowns on all elevator bulkhead vectors tracking within the requested facility zone.
```csharp
public static void LockElevators(this FacilityZone zone)
```

### 🔹 `LockElevatorsInZone()`
**Description:** Enforces absolute structural lockdowns on all elevator bulkhead vectors tracking within the requested facility zone. Retained for backward compatibility.
```csharp
public static void LockElevatorsInZone(FacilityZone zone) => zone.LockElevators();
```

### 🔹 `UnlockElevators()`
**Description:** Restores normal passage access and lifts all operational bulkhead locking restrictions across elevator units within the specified zone.
```csharp
public static void UnlockElevators(this FacilityZone zone)
```

### 🔹 `UnlockElevatorsInZone()`
**Description:** Restores normal passage access and lifts all operational bulkhead locking restrictions across elevator units within the specified zone. Retained for backward compatibility.
```csharp
public static void UnlockElevatorsInZone(FacilityZone zone) => zone.UnlockElevators();
```

### 🔹 `HandleElevatorsForRoom()`
**Description:** Processes a localized, probability-driven evaluation sweep across all elevators bound to a room, safely routing matching units into an execution action graph.
```csharp
public static void HandleElevatorsForRoom(this Room room, float affectChance, float duration, Action<Elevator> elevatorAction)
```

---

## 📦 Class: ElevatorLightingExtensions

### 🔹 `TurnOffLights()`
**Description:** Delays or suppresses elevator cabin lighting for a specified duration.
```csharp
public static void TurnOffLights(this Elevator elevator, float duration)
```

### 🔹 `TurnOnLights()`
**Description:** Restores elevator cabin lighting.
```csharp
public static void TurnOnLights(this Elevator elevator)
```

### 🔹 `FlickerElevatorLightsCoroutine()`
**Description:** Runs a visual flicker animation loop for the elevator lighting.
```csharp
public static IEnumerator<float> FlickerElevatorLightsCoroutine(this Elevator elevator, float duration, float frequency)
```

### 🔹 `AreLightsOff()`
**Description:** Checks if the elevator lights are currently disabled.
```csharp
public static bool AreLightsOff(this Elevator elevator) => false;
```

### 🔹 `TurnOffLights()`
**Description:** Turns off lights for a collection of elevators for the specified duration.
```csharp
public static void TurnOffLights(this IEnumerable<Elevator> elevators, float duration)
```

### 🔹 `TurnOffLights()`
**Description:** Turns off lights for an inline array of elevators for the specified duration.
```csharp
public static void TurnOffLights(float duration, params Elevator[] elevators)
```

### 🔹 `TurnOnLights()`
**Description:** Turns on lights for a collection of elevators.
```csharp
public static void TurnOnLights(this IEnumerable<Elevator> elevators)
```

### 🔹 `TurnOnLights()`
**Description:** Turns on lights for an inline array of elevators.
```csharp
public static void TurnOnLights(params Elevator[] elevators)
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
**Description:** Checks if the firearm has the specified attachment enabled.
```csharp
public static bool HasAttachment(this FirearmItem firearm, AttachmentName attachmentName)
```

### 🔹 `HasAttachments()`
**Description:** Checks if the firearm has all specified attachments enabled.
```csharp
public static bool HasAttachments(this FirearmItem firearm, IEnumerable<AttachmentName> attachmentNames)
```

### 🔹 `HasAttachments()`
**Description:** Checks if the firearm has all inline array attachments enabled.
```csharp
public static bool HasAttachments(this FirearmItem firearm, params AttachmentName[] attachmentNames)
```

### 🔹 `HasAttachment()`
**Description:** Checks if all firearms in the collection have the specified attachment enabled.
```csharp
public static bool HasAttachment(this IEnumerable<FirearmItem> firearms, AttachmentName attachmentName)
```

### 🔹 `HasAttachment()`
**Description:** Checks if all inline array firearms have the specified attachment enabled.
```csharp
public static bool HasAttachment(AttachmentName attachmentName, params FirearmItem[] firearms)
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
**Description:** Clamps a float value between a minimum and maximum range.
```csharp
public static float Clamp(this float value, float min, float max) => Mathf.Clamp(value, min, max);
```

### 🔹 `Clamp()`
**Description:** Clamps an int value between a minimum and maximum range.
```csharp
public static int Clamp(this int value, int min, int max) => Mathf.Clamp(value, min, max);
```

### 🔹 `Clamp()`
**Description:** Clamps a double value between a minimum and maximum range.
```csharp
public static double Clamp(this double value, double min, double max) => value < min ? min : (value > max ? max : value);
```

### 🔹 `LimitMin()`
**Description:** Ensures a float value does not fall below the specified minimum baseline.
```csharp
public static float LimitMin(this float value, float minBaseline) => Mathf.Max(minBaseline, value);
```

### 🔹 `LimitMin()`
**Description:** Ensures an int value does not fall below the specified minimum baseline.
```csharp
public static int LimitMin(this int value, int minBaseline) => Mathf.Max(minBaseline, value);
```

### 🔹 `LimitMin()`
**Description:** Ensures a double value does not fall below the specified minimum baseline.
```csharp
public static double LimitMin(this double value, double minBaseline) => value < minBaseline ? minBaseline : value;
```

### 🔹 `LimitMax()`
**Description:** Ensures a float value does not exceed the specified maximum baseline.
```csharp
public static float LimitMax(this float value, float maxBaseline) => Mathf.Min(maxBaseline, value);
```

### 🔹 `LimitMax()`
**Description:** Ensures an int value does not exceed the specified maximum baseline.
```csharp
public static int LimitMax(this int value, int maxBaseline) => Mathf.Min(maxBaseline, value);
```

### 🔹 `LimitMax()`
**Description:** Ensures a double value does not exceed the specified maximum baseline.
```csharp
public static double LimitMax(this double value, double maxBaseline) => value > maxBaseline ? maxBaseline : value;
```

### 🔹 `Abs()`
**Description:** Returns the absolute value of an int.
```csharp
public static int Abs(this int value) => Math.Abs(value);
```

### 🔹 `Abs()`
**Description:** Returns the absolute value of a float.
```csharp
public static float Abs(this float value) => Mathf.Abs(value);
```

### 🔹 `Abs()`
**Description:** Returns the absolute value of a double.
```csharp
public static double Abs(this double value) => Math.Abs(value);
```

### 🔹 `Sign()`
**Description:** Returns the sign of an int value (-1, 0, or 1).
```csharp
public static int Sign(this int value) => value == 0 ? 0 : (value > 0 ? 1 : -1);
```

### 🔹 `Sign()`
**Description:** Returns the sign of a float value (-1.0, 0.0, or 1.0).
```csharp
public static float Sign(this float value) => value == 0f ? 0f : (value > 0f ? 1f : -1f);
```

### 🔹 `Sign()`
**Description:** Returns the sign of a double value (-1.0, 0.0, or 1.0).
```csharp
public static double Sign(this double value) => value == 0.0 ? 0.0 : (value > 0.0 ? 1.0 : -1.0);
```

### 🔹 `Lerp()`
**Description:** Linearly interpolates between two float values based on the specified alpha.
```csharp
public static float Lerp(this float from, float to, float t) => Mathf.Lerp(from, to, t);
```

### 🔹 `LerpUnclamped()`
**Description:** Linearly interpolates between two float values, allowing out-of-bounds alpha values.
```csharp
public static float LerpUnclamped(this float from, float to, float t) => Mathf.LerpUnclamped(from, to, t);
```

### 🔹 `Floor()`
**Description:** Returns the largest integer smaller than or equal to the float value.
```csharp
public static float Floor(this float value) => Mathf.Floor(value);
```

### 🔹 `Floor()`
**Description:** Returns the largest integer smaller than or equal to the double value.
```csharp
public static double Floor(this double value) => Math.Floor(value);
```

### 🔹 `Ceil()`
**Description:** Returns the smallest integer greater than or equal to the float value.
```csharp
public static float Ceil(this float value) => Mathf.Ceil(value);
```

### 🔹 `Ceil()`
**Description:** Returns the smallest integer greater than or equal to the double value.
```csharp
public static double Ceil(this double value) => Math.Ceiling(value);
```

### 🔹 `DbToLinear()`
**Description:** Converts a decibel amplitude value (-96dB to 0dB) to a linear coefficient scale (0.0 to 1.0).
```csharp
public static float DbToLinear(this float db) => db <= -96f ? 0f : Mathf.Pow(10f, db / 20f);
```

### 🔹 `LinearToDb()`
**Description:** Converts a linear coefficient scalar (0.0 to 1.0) to a logarithmic decibel scale.
```csharp
public static float LinearToDb(this float linear) => linear <= 0.00001f ? -96f : 20f * Mathf.Log10(linear);
```

### 🔹 `IsNanOrInfinity()`
**Description:** Checks if a float value is NaN or Infinity.
```csharp
public static bool IsNanOrInfinity(this float value) => float.IsNaN(value) || float.IsInfinity(value);
```

### 🔹 `IsNanOrInfinity()`
**Description:** Checks if a double value is NaN or Infinity.
```csharp
public static bool IsNanOrInfinity(this double value) => double.IsNaN(value) || double.IsInfinity(value);
```

### 🔹 `IsNanOrInfinity()`
**Description:** Always returns false, as integral types cannot represent NaN or Infinity.
```csharp
public static bool IsNanOrInfinity(this int value) => false;
```

### 🔹 `RoundToInt()`
**Description:** Rounds a float value to the nearest integer.
```csharp
public static int RoundToInt(this float value) => Mathf.RoundToInt(value);
```

### 🔹 `RoundToInt()`
**Description:** Rounds a double value to the nearest integer.
```csharp
public static int RoundToInt(this double value) => (int)Math.Round(value);
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

### 🔹 `ApplyKineticBlast()`
**Description:** Iterates over an inline array of spawned pickups and forcibly applies batch kinetic physical propulsion forces.
```csharp
public static void ApplyKineticBlast(float linearVelocityMagnitude, float angularVelocityMagnitude, params Pickup[] pickups)
```

---

## 📦 Class: PlayerExtensions

### 🔹 `AttachTrackingObject()`
**Description:** Attaches a tracking GameObject to a single player using a transform tracker component.
```csharp
public static void AttachTrackingObject(this Player player, GameObject followerObject, Vector3 offset = default)
```

### 🔹 `AttachTrackingObject()`
**Description:** Attaches a tracking GameObject to a collection of players.
```csharp
public static void AttachTrackingObject(this IEnumerable<Player> players, GameObject followerObject, Vector3 offset = default)
```

### 🔹 `AttachTrackingObject()`
**Description:** Attaches a tracking GameObject to an inline array of players.
```csharp
public static void AttachTrackingObject(GameObject followerObject, Vector3 offset, params Player[] players)
```

### 🔹 `BroadcastHintToAll()`
**Description:** Sends a hint message to all fully initialized and ready players globally.
```csharp
public static void BroadcastHintToAll(string hintContent, float duration = 5f)
```

### 🔹 `BroadcastHint()`
**Description:** Sends a hint message to a collection of players, filtering out null or unready entities.
```csharp
public static void BroadcastHint(this IEnumerable<Player> players, string hintContent, float duration = 5f)
```

### 🔹 `BroadcastHint()`
**Description:** Sends a hint message to an inline array of players, filtering out null or unready entities.
```csharp
public static void BroadcastHint(string hintContent, float duration, params Player[] players)
```

### 🔹 `GetHumeShieldValue()`
**Description:** Gets the current Hume Shield value of a player, returning 0 if the stat module is missing.
```csharp
public static float GetHumeShieldValue(this Player player)
```

### 🔹 `IsLivingHuman()`
**Description:** Determines if the player is an active, ready, and living human faction member.
```csharp
public static bool IsLivingHuman(this Player player)
```

### 🔹 `GetDistanceTo()`
**Description:** Calculates the Euclidean distance between a player and a specified position vector.
```csharp
public static float GetDistanceTo(this Player player, Vector3 position)
```

### 🔹 `IsWithinDistance()`
**Description:** Checks if a player is within a maximum distance threshold from a position vector.
```csharp
public static bool IsWithinDistance(this Player player, Vector3 position, float maxDistance)
```

### 🔹 `GetHeldLightSourceState()`
**Description:** Checks if the player's currently held light source item or weapon flashlight attachment is active.
```csharp
public static bool GetHeldLightSourceState(this Player player)
```

### 🔹 `SetHeldLightSourceState()`
**Description:** Toggles the activation state of the player's held light source item or weapon flashlight attachment.
```csharp
public static void SetHeldLightSourceState(this Player player, bool emit)
```

### 🔹 `SetHeldLightSourceState()`
**Description:** Toggles the activation state of a collection of players' held light sources.
```csharp
public static void SetHeldLightSourceState(this IEnumerable<Player> players, bool emit)
```

### 🔹 `SetHeldLightSourceState()`
**Description:** Toggles the activation state of an inline array of players' held light sources.
```csharp
public static void SetHeldLightSourceState(bool emit, params Player[] players)
```

### 🔹 `FlickerHeldLightSourceCoroutine()`
**Description:** Runs a coroutine loop that flickers the player's held light source.
```csharp
public static IEnumerator<float> FlickerHeldLightSourceCoroutine(this Player player, int flickerCount, float delayPerFlicker, bool forceOff = false, Action<Player, bool> onTickFeedback = null)
```

### 🔹 `FlickerHeldLightSource()`
**Description:** Triggers the flicker coroutine loop for a collection of players.
```csharp
public static void FlickerHeldLightSource(this IEnumerable<Player> players, int flickerCount, float delayPerFlicker, bool forceOff = false, Action<Player, bool> onTickFeedback = null)
```

### 🔹 `FlickerHeldLightSource()`
**Description:** Triggers the flicker coroutine loop for an inline array of players.
```csharp
public static void FlickerHeldLightSource(int flickerCount, float delayPerFlicker, bool forceOff = false, Action<Player, bool> onTickFeedback = null, params Player[] players)
```

### 🔹 `HasActiveLightSource()`
**Description:** Verifies if the player is actively emitting light via flashlights or firearm attachments.
```csharp
public static bool HasActiveLightSource(this Player player)
```

### 🔹 `IsInRoom()`
**Description:** Checks if the player is in any of the specified rooms using a zero-allocation evaluation loop.
```csharp
public static bool IsInRoom(this Player player, params RoomName[] roomNames)
```

### 🔹 `IsInTrueDarkness()`
**Description:** Verifies if the player is in total darkness, accounting for both standard facility rooms and elevator cabins.
```csharp
public static bool IsInTrueDarkness(this Player player)
```

### 🔹 `IsInDarkRoom()`
**Description:** Checks if the standard room lighting controllers are disabled in the player's current location.
```csharp
public static bool IsInDarkRoom(this Player player)
```

### 🔹 `IsWithinRadius()`
**Description:** Validates circular proximity to a target vector using square magnitude to avoid performance overhead.
```csharp
public static bool IsWithinRadius(this Player player, Vector3 targetPosition, float radiusSize)
```

### 🔹 `IsWithinAnyRadius()`
**Description:** Checks if the player falls within a radius threshold of any provided target positions.
```csharp
public static bool IsWithinAnyRadius(this Player player, IEnumerable<Vector3> positions, float radiusSize)
```

### 🔹 `WhereAlive()`
**Description:** Filters a player collection to return only ready and living instances.
```csharp
public static IEnumerable<Player> WhereAlive(this IEnumerable<Player> players)
```

### 🔹 `WhereHuman()`
**Description:** Filters a player collection to return only human faction members.
```csharp
public static IEnumerable<Player> WhereHuman(this IEnumerable<Player> players)
```

### 🔹 `WhereNotInPocket()`
**Description:** Filters a player collection to return players who are not in the Pocket Dimension.
```csharp
public static IEnumerable<Player> WhereNotInPocket(this IEnumerable<Player> players)
```

### 🔹 `ApplySensoryShock()`
**Description:** Applies a combination of visual and auditory status impairments to a single player.
```csharp
public static void ApplySensoryShock(this Player player, float flashDuration = 0.0f, float blindDuration = 0.0f, float blurDuration = 0.0f, float deafenDuration = 0.0f)
```

### 🔹 `ApplySensoryShock()`
**Description:** Applies a combination of visual and auditory status impairments to a collection of players.
```csharp
public static void ApplySensoryShock(this IEnumerable<Player> players, float flashDuration = 0.0f, float blindDuration = 0.0f, float blurDuration = 0.0f, float deafenDuration = 0.0f)
```

### 🔹 `ApplySensoryShock()`
**Description:** Applies a combination of visual and auditory status impairments to an inline array of players.
```csharp
public static void ApplySensoryShock(float flashDuration, float blindDuration, float blurDuration, float deafenDuration, params Player[] players)
```

### 🔹 `GetRoom()`
**Description:** Resolves the facility room matching the player's current spatial coordinates.
```csharp
public static Room GetRoom(this Player player)
```

### 🔹 `GetDistanceTo()`
**Description:** Calculates the distance between a player and a room center anchor.
```csharp
public static float GetDistanceTo(this Player player, Room room)
```

### 🔹 `IsWithinRadius()`
**Description:** Checks if the player is within a radius of the targeted room center.
```csharp
public static bool IsWithinRadius(this Player player, Room room, float radiusSize)
```

### 🔹 `IsEligibleForEscape()`
**Description:** Determines if a player fulfills basic criteria to trigger an escape sequence.
```csharp
public static bool IsEligibleForEscape(this Player player, Vector3 escapeZone, float escapeZoneSize)
```

### 🔹 `IsInShelter()`
**Description:** Checks if a player is safely inside a designated shelter structure or location coordinate.
```csharp
public static bool IsInShelter(this Player player, float shelterZoneSize, IEnumerable<Vector3> cachedShelterLocations, params RoomName[] additionalRooms)
```

### 🔹 `TryResolveFuzzy()`
**Description:** Resolves a single player from a fuzzy string identifier using exact matching, substring containment, and Levenshtein distance checks. Completely avoids heavy allocations on large player lists during evaluation cycles.
```csharp
public static bool TryResolveFuzzy(this IEnumerable<Player> players, string identifier, out Player target, out string errorResponse)
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

### 🔹 `TurnOffLights()`
**Description:** Forcibly suppresses the active illumination controllers across the specified room topology for a precise timeframe.
```csharp
public static void TurnOffLights(this Room room, float duration)
```

### 🔹 `TurnOnLights()`
**Description:** Restores electrical power to the room's lighting grid controllers and optionally triggers a brief flicker sequence.
```csharp
public static void TurnOnLights(this Room room, float flickerDuration = 0f)
```

### 🔹 `TurnOffRoomAndNeighborLights()`
**Description:** Forcibly suppresses illumination across this room and all physically connected adjacent neighboring rooms simultaneously. Fully DRY implementation utilizing centralized action propagation cascades.
```csharp
public static void TurnOffRoomAndNeighborLights(this Room room, float duration, bool forced = false)
```

### 🔹 `TurnOnRoomAndNeighborLights()`
**Description:** Restores active electrical power and forces an optional brief flickering update sequence across this room and all adjacent neighbors. Fully DRY implementation utilizing centralized action propagation cascades.
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

### 🔹 `SetLightsColor()`
**Description:** Systematically executes a batch color spectrum override sweep across an inline array of rooms.
```csharp
public static void SetLightsColor(Color color, params Room[] rooms)
```

### 🔹 `TurnOffLights()`
**Description:** Systematically suppresses illumination across an aggregated collection sequence of rooms.
```csharp
public static void TurnOffLights(this IEnumerable<Room> rooms, float duration)
```

### 🔹 `TurnOffLights()`
**Description:** Systematically suppresses illumination across an inline array of rooms.
```csharp
public static void TurnOffLights(float duration, params Room[] rooms)
```

### 🔹 `TurnOnLights()`
**Description:** Systematically restores electrical power and optionally triggers a brief flicker sequence across a collection of rooms.
```csharp
public static void TurnOnLights(this IEnumerable<Room> rooms, float flickerDuration = 0f)
```

### 🔹 `TurnOnLights()`
**Description:** Systematically restores electrical power and optionally triggers a brief flicker sequence across an inline array of rooms.
```csharp
public static void TurnOnLights(float flickerDuration, params Room[] rooms)
```

### 🔹 `BreakAllDoors()`
**Description:** Iterates over all door sub-components bound to the target room collection to safely locate and fracture any breakable barriers.
```csharp
public static void BreakAllDoors(this IEnumerable<Room> rooms)
```

### 🔹 `BreakAllDoors()`
**Description:** Iterates over all door sub-components bound to the target room inline array to safely locate and fracture any breakable barriers.
```csharp
public static void BreakAllDoors(params Room[] rooms)
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

### 🔹 `ComputeLevenshteinDistance()`
**Description:** Computes the Levenshtein Distance between two string primitives with zero heap allocations.
```csharp
public static int ComputeLevenshteinDistance(this string source, string target)
```

---

## 📦 Class: TeslaExtensions

### 🔹 `DisableFor()`
**Description:** Disables a single Tesla gate for a specified duration with an optional discharge trigger.
```csharp
public static void DisableFor(this Tesla tesla, float duration, bool forceTrigger = true)
```

### 🔹 `DisableFor()`
**Description:** Disables a collection of Tesla gates for a specified duration with an optional discharge trigger.
```csharp
public static void DisableFor(this IEnumerable<Tesla> teslas, float duration, bool forceTrigger = true)
```

### 🔹 `DisableFor()`
**Description:** Disables an inline array of Tesla gates for a specified duration with an optional discharge trigger.
```csharp
public static void DisableFor(float duration, bool forceTrigger, params Tesla[] teslas)
```

### 🔹 `SetInactiveTimeForAll()`
**Description:** Sets the inactive cooldown time for a collection of Tesla gates.
```csharp
public static void SetInactiveTimeForAll(this IEnumerable<Tesla> teslas, float duration, bool forceTrigger = true) => teslas.DisableFor(duration, forceTrigger);
```

### 🔹 `Disable()`
**Description:** Deactivates a collection of Tesla gates using a default shorthand configuration.
```csharp
public static void Disable(this IEnumerable<Tesla> teslas, float duration = 5.0f, bool forceTrigger = false) => teslas.DisableFor(duration, forceTrigger);
```

---

## 📦 Class: TimingExtensions

### 🔹 `CallDelayedIf()`
**Description:** Executes an action after a delay if the specified condition evaluates to true.
```csharp
public static CoroutineHandle CallDelayedIf(float delay, Func<bool> condition, Action action, string coroutineTag = null)
```

### 🔹 `KillCoroutine()`
**Description:** Kills all coroutines bound to the specified string tag.
```csharp
public static void KillCoroutine(this string tag)
```

### 🔹 `Kill()`
**Description:** Kills the coroutine mapped to the specified handle if it is running.
```csharp
public static void Kill(this CoroutineHandle handle)
```

### 🔹 `KillCoroutines()`
**Description:** Kills all coroutines bound to the collection of string tags.
```csharp
public static void KillCoroutines(this IEnumerable<string> tags)
```

### 🔹 `KillCoroutines()`
**Description:** Kills all coroutines bound to the inline array of string tags.
```csharp
public static void KillCoroutines(params string[] tags)
```

### 🔹 `Kill()`
**Description:** Kills all coroutines associated with the collection of handles.
```csharp
public static void Kill(this IEnumerable<CoroutineHandle> handles)
```

### 🔹 `Kill()`
**Description:** Kills all coroutines associated with the inline array of handles.
```csharp
public static void Kill(params CoroutineHandle[] handles)
```

### 🔹 `KillAndClear()`
**Description:** Kills all coroutines in the list using a zero-allocation loop and clears the list.
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
**Description:** Audits all float components of a 3D vector against structural corruption anomalies like NaN or Infinity.
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

## 📦 Class: ExiledCompatibilityLayer

### 🔹 `ExecuteFallback()`
**Description:** Executes the synchronization fallback to bridge directories and load missing configurations.
```csharp
public static void ExecuteFallback(Plugin plugin)
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

## 📦 Class: PluginBuilder

### 🔹 `Create()`
**Description:** Initiates a fluent orchestration builder instance utilizing implicit compiler type inference. <typeparam name="TConfig">The primary configuration type inferred by the compiler context.</typeparam>
```csharp
public static PluginBuilder<TConfig> Create<TConfig>(Plugin<TConfig> plugin) where TConfig : LabApiConfig, new()
```

### 🔹 `new()`
**Description:** A high-performance fluent orchestration builder designed to streamline sub-configuration deployment and module initialization sequences. <typeparam name="TConfig">The primary framework configuration type conforming to <see cref="LabApiConfig"/>.</typeparam>
```csharp
public sealed class PluginBuilder<TConfig> where TConfig : LabApiConfig, new()
```

### 🔹 `PluginBuilder()`
**Description:** Initializes a new instance of the <see cref="PluginBuilder{TConfig}"/> class.
```csharp
public PluginBuilder(Plugin<TConfig> plugin)
```

### 🔹 `BindSubConfig()`
**Description:** Dynamically loads, validates, and binds a decentralized sub-configuration file to the plugin ecosystem. <typeparam name="TSubConfig">The target modular sub-configuration class type being loaded.</typeparam>
```csharp
public PluginBuilder<TConfig> BindSubConfig<TSubConfig>( string fileName, Action<TSubConfig> bindAction, Action<TSubConfig> validationAction = null) where TSubConfig : class, new()
```

### 🔹 `InitializeModule()`
**Description:** Executes an initialization routine or boots up a subsystem component as part of the fluent pipeline.
```csharp
public PluginBuilder<TConfig> InitializeModule(Action initAction)
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

