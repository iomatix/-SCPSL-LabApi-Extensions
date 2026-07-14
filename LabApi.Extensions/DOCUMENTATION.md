# Audio Manager API - Architecture API Registry

## 📦 Class: ActionChainExtensions

---

## 📦 Class: AssemblyExtensions

### 🔹 `FindEmbeddedAsset()`
**Description:** Finds an embedded resource inside the assembly. Looks for: primaryKey + extension, primaryKey with dots replaced by underscores, or any fallback token. Full manifest resource path if found; otherwise, <c>null</c>. </returns>
```csharp
public static string FindEmbeddedAsset( this Assembly assembly, string primaryKey, string fileExtension = ".wav", params string[] alternativeTokens)
```

---

## 📦 Class: CassieExtensions

### 🔹 `CassieClear()`
**Description:** Clears the CASSIE announcement queue.
```csharp
public static void CassieClear() => Announcer.Clear();
```

### 🔹 `SanitizeCassieString()`
**Description:** Removes CR/LF and trims whitespace for safe CASSIE usage with zero redundant allocations.
```csharp
public static string SanitizeCassieString(this string rawMessage)
```

### 🔹 `DispatchGlitchyMessage()`
**Description:** Glitchifies and dispatches a message, returning the calculated playback duration.
```csharp
public static double DispatchGlitchyMessage(string message, float glitchChance, float jamChance)
```

### 🔹 `DispatchMessage()`
**Description:** Dispatches a standard CASSIE message and returns playback duration.
```csharp
public static double DispatchMessage(string message)
```

### 🔹 `ProcessAndDispatchMessage()`
**Description:** Dispatches a formatted CASSIE message with optional subtitles and priority.
```csharp
public static void ProcessAndDispatchMessage( string message, string subtitles, bool clear, float priority, bool disableMessages = false)
```

### 🔹 `DispatchMessage()`
**Description:** Dispatches all messages in the collection with zero-allocation path optimization (no lambda closures).
```csharp
public static void DispatchMessage(this IEnumerable<string> messages)
```

### 🔹 `DispatchMessage()`
**Description:** Dispatches all provided messages.
```csharp
public static void DispatchMessage(params string[] messages) => messages.DispatchMessage();
```

### 🔹 `ToCassieCountdown()`
**Description:** Converts an integer into a formatted CASSIE countdown string.
```csharp
public static string ToCassieCountdown(this int notifyTime, string context = "seconds until event detonation")
```

### 🔹 `CalculateCassieMessageDuration()`
**Description:** Calculates playback duration of a single message.
```csharp
public static double CalculateCassieMessageDuration(string message, CassiePlaybackModifiers modifiers = default)
```

### 🔹 `CalculateTotalMessagesDurations()`
**Description:** Calculates total duration of messages with per-message pitch modifiers, optimized to avoid GC heap allocations.
```csharp
public static double CalculateTotalMessagesDurations(IDictionary<string, float> messageSpeedDictionary)
```

### 🔹 `CalculateTotalMessagesDurations()`
**Description:** Calculates total duration of all messages in the collection, optimized to avoid GC heap allocations.
```csharp
public static double CalculateTotalMessagesDurations(this IEnumerable<string> messages, CassiePlaybackModifiers modifiers = default)
```

### 🔹 `CalculateTotalMessagesDurations()`
**Description:** Calculates total duration of provided messages (default modifiers).
```csharp
public static double CalculateTotalMessagesDurations(params string[] messages) => messages.CalculateTotalMessagesDurations(default);
```

### 🔹 `CalculateTotalMessagesDurations()`
**Description:** Calculates total duration of provided messages with custom modifiers.
```csharp
public static double CalculateTotalMessagesDurations(CassiePlaybackModifiers modifiers, params string[] messages) => messages.CalculateTotalMessagesDurations(modifiers);
```

---

## 📦 Class: CollectionExtensions

### 🔹 `ForEach()`
**Description:** Executes the specified action for every item in the collection. Contains optimized zero-allocation fast-paths for Arrays and Lists.
```csharp
public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
```

### 🔹 `ForEach()`
**Description:** Executes the specified action for every item in the collection with zero runtime allocations, passing a custom state to completely prevent GC closure (lambda display class) allocations.
```csharp
public static void ForEach<T, TState>(this IEnumerable<T> source, TState state, Action<T, TState> action)
```

### 🔹 `All()`
**Description:** Returns true if all items in the collection match the specified state-passing predicate. Features optimized zero-allocation fast-paths with immediate early-exit on mismatch.
```csharp
public static bool All<T, TState>(this IEnumerable<T> source, TState state, Func<T, TState, bool> predicate)
```

### 🔹 `ExecuteThrottled()`
**Description:** Executes the action if the cooldown (based on last execution timestamp) has elapsed. Updates the timestamp to current time and returns true if executed. <remarks> This method uses the "Throttle Model" (stores the last execution time). Do not mix this with absolute lock methods like <see cref="IsCooldownActive{TKey}"/>. </remarks>
```csharp
public static bool ExecuteThrottled<TKey>( this IDictionary<TKey, DateTime> cooldownMap, TKey key, TimeSpan window, Action throttleAction)
```

### 🔹 `IsThrottleActive()`
**Description:** Checks if a throttle-based cooldown is currently active for the specified key. <remarks> Use this method specifically when your map is managed via <see cref="ExecuteThrottled{TKey}"/>. </remarks>
```csharp
public static bool IsThrottleActive<TKey>( this IDictionary<TKey, DateTime> cooldownMap, TKey key, TimeSpan window)
```

### 🔹 `IsCooldownActive()`
**Description:** Checks if a lock-based cooldown is active (where the stored value represents the absolute expiry time). <remarks> Use this method specifically when your map is managed via <see cref="TryAcquireLock{TKey}"/>. </remarks>
```csharp
public static bool IsCooldownActive<TKey>(this IDictionary<TKey, DateTime> cooldownMap, TKey key)
```

### 🔹 `TryAcquireLock()`
**Description:** Attempts to acquire a lock for the specified key. If the lock has elapsed or does not exist, commits a new absolute expiration timestamp (current time + lockWindow) and returns true. <remarks> This method uses the "Lock Model" (stores the absolute expiration time). </remarks>
```csharp
public static bool TryAcquireLock<TKey>( this IDictionary<TKey, DateTime> cooldownMap, TKey key, TimeSpan lockWindow)
```

### 🔹 `PruneExpired()`
**Description:** Removes all expired entries from the dictionary with zero GC heap allocations. Works seamlessly with both Throttle and Lock models.
```csharp
public static void PruneExpired<TKey>(this IDictionary<TKey, DateTime> dictionary, DateTime comparisonTime)
```

---

## 📦 Class: CommandExtensions

### 🔹 `ConfirmPermission()`
**Description:** Returns true if the sender has the required permission. Server console and non-player senders always bypass this check.
```csharp
public static bool ConfirmPermission(this ICommandSender sender, PlayerPermissions permission, out string errorResponse)
```

### 🔹 `TryGetFloat()`
**Description:** Tries to read a float from the argument list. Uses invariant culture to prevent system-specific parsing bugs.
```csharp
public static bool TryGetFloat(this ArraySegment<string> arguments, int index, out float value, float minValue = float.MinValue)
```

### 🔹 `TryGetInt()`
**Description:** Tries to read an int from the argument list.
```csharp
public static bool TryGetInt(this ArraySegment<string> arguments, int index, out int value, int minValue = int.MinValue)
```

### 🔹 `TryGetBool()`
**Description:** Tries to read a boolean from the argument list. Supports: true/false, 1/0, yes/no, on/off.
```csharp
public static bool TryGetBool(this ArraySegment<string> arguments, int index, out bool value)
```

### 🔹 `TryGetPlayer()`
**Description:** Resolves a single player from the arguments at the specified index by ID or Name.
```csharp
public static bool TryGetPlayer(this ArraySegment<string> arguments, int index, out Player player)
```

### 🔹 `TryGetEnum()`
**Description:** Tries to parse a generic Enum value from the arguments.
```csharp
public static bool TryGetEnum<TEnum>(this ArraySegment<string> arguments, int index, out TEnum value) where TEnum : struct, Enum
```

### 🔹 `GetRemainingText()`
**Description:** Combines all remaining arguments starting from the specified index into a single spaced string. Uses the pooled StringBuilder to completely avoid generic GC allocations during generation.
```csharp
public static string GetRemainingText(this ArraySegment<string> arguments, int startIndex)
```

---

## 📦 Class: DictionaryExtensions

### 🔹 `Deconstruct()`
**Description:** Backports KeyValuePair deconstruction to .NET Framework 4.8, enabling C# 9.0 pattern matching and tuple-like iteration. <example> foreach (var (key, value) in myDictionary) { ... } </example>
```csharp
public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> kvp, out TKey key, out TValue value)
```

### 🔹 `GetOrAdd()`
**Description:** Gets a value from the dictionary if it exists; otherwise, adds a new value using the provided factory and returns it.
```csharp
public static TValue GetOrAdd<TKey, TValue>( this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueFactory)
```

### 🔹 `GetOrAdd()`
**Description:** Gets a value from the dictionary if it exists; otherwise, adds a new value using a state-passing factory to completely avoid GC closure (lambda display class) allocations. <remarks> Highly recommended for performance-critical systems inside loops or event handlers. </remarks>
```csharp
public static TValue GetOrAdd<TKey, TValue, TState>( this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TState, TValue> valueFactory, TState state)
```

### 🔹 `TryRemove()`
**Description:** Backports the modern TryRemove / Remove-with-out functionality to .NET Framework 4.8 dictionaries. Safely attempts to remove a key and outputs the removed value.
```csharp
public static bool TryRemove<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, out TValue value)
```

---

## 📦 Class: DoorExtensions

### 🔹 `ClearCache()`
**Description:** Explicitly clears the internal door and elevator caches. Must be called on round restart to prevent critical memory leaks of stale Unity objects.
```csharp
public static void ClearCache()
```

### 🔹 `IsOpen()`
**Description:** Returns true if the door is open.
```csharp
public static bool IsOpen(this Door door) => door != null && door.IsOpened;
```

### 🔹 `WhereNameIn()`
**Description:** Returns only doors whose DoorName matches any of the provided names. <remarks> This is a query operation and will allocate an iterator state-machine under the hood. </remarks>
```csharp
public static IEnumerable<Door> WhereNameIn(this IEnumerable<Door> doors, params DoorName[] names)
```

### 🔹 `Open()`
**Description:** Opens the door.
```csharp
public static void Open(this Door door, bool bypassLocks = false) => door.SetOpenState(true, bypassLocks);
```

### 🔹 `Close()`
**Description:** Closes the door.
```csharp
public static void Close(this Door door, bool bypassLocks = false) => door.SetOpenState(false, bypassLocks);
```

### 🔹 `Toggle()`
**Description:** Toggles the door open/closed state. Respects locks by default.
```csharp
public static void Toggle(this Door door, bool bypassLocks = false)
```

### 🔹 `SetLockState()`
**Description:** Sets or removes a lock reason on the door.
```csharp
public static void SetLockState(this Door door, DoorLockReason reason, bool locked = true)
```

### 🔹 `SetOpenState()`
**Description:** Sets the open/closed state of the door. This is the single source of truth for door state changes.
```csharp
public static void SetOpenState(this Door door, bool opened, bool bypassLocks = false)
```

### 🔹 `OpenAndLock()`
**Description:** Opens the door (if safe) and immediately applies a lock reason.
```csharp
public static void OpenAndLock(this Door door, DoorLockReason reason = DoorLockReason.AdminCommand, bool playSound = true)
```

### 🔹 `Open()`
**Description:** Opens all doors in the collection with zero heap allocations.
```csharp
public static void Open(this IEnumerable<Door> doors, bool bypassLocks = false)
```

### 🔹 `Open()`
**Description:** Opens all provided doors.
```csharp
public static void Open(bool bypassLocks, params Door[] doors) => doors.Open(bypassLocks);
```

### 🔹 `Close()`
**Description:** Closes all doors in the collection with zero heap allocations.
```csharp
public static void Close(this IEnumerable<Door> doors, bool bypassLocks = false)
```

### 🔹 `Close()`
**Description:** Closes all provided doors.
```csharp
public static void Close(bool bypassLocks, params Door[] doors) => doors.Close(bypassLocks);
```

### 🔹 `SetLockState()`
**Description:** Sets lock state for all doors in the collection with zero heap allocations.
```csharp
public static void SetLockState(this IEnumerable<Door> doors, DoorLockReason reason = DoorLockReason.AdminCommand, bool locked = true)
```

### 🔹 `SetLockState()`
**Description:** Sets lock state for all provided doors.
```csharp
public static void SetLockState(DoorLockReason reason, bool locked, params Door[] doors) => doors.SetLockState(reason, locked);
```

### 🔹 `SetOpenState()`
**Description:** Sets open/closed state for all doors in the collection with zero heap allocations.
```csharp
public static void SetOpenState(this IEnumerable<Door> doors, bool opened, bool bypassLocks = false)
```

### 🔹 `SetOpenState()`
**Description:** Sets open/closed state for all provided doors.
```csharp
public static void SetOpenState(bool opened, bool bypassLocks, params Door[] doors) => doors.SetOpenState(opened, bypassLocks);
```

### 🔹 `OpenAndLock()`
**Description:** Opens all doors and applies a lock reason with zero heap allocations.
```csharp
public static void OpenAndLock(this IEnumerable<Door> doors, DoorLockReason reason, bool playSound = true)
```

### 🔹 `OpenAndLock()`
**Description:** Opens all provided doors and applies a lock reason.
```csharp
public static void OpenAndLock(DoorLockReason reason, bool playSound, params Door[] doors) => doors.OpenAndLock(reason, playSound);
```

### 🔹 `GetElevator()`
**Description:** Returns the elevator associated with this door. Optimizes search paths and caches results.
```csharp
public static Elevator GetElevator(this Door door)
```

### 🔹 `IsElevatorDoor()`
**Description:** Returns true if the door is an elevator door. Caches the result to bypass native Unity calls.
```csharp
public static bool IsElevatorDoor(this Door door)
```

### 🔹 `IsElevatorAtDoorLevel()`
**Description:** Returns true if the elevator cabin is aligned with the door's floor level.
```csharp
public static bool IsElevatorAtDoorLevel(this Door door)
```

### 🔹 `CheckAndRestoreElevatorDoorState()`
**Description:** Restores elevator door state after lock removal.
```csharp
public static void CheckAndRestoreElevatorDoorState(this Door door)
```

### 🔹 `IsGate()`
**Description:** Returns true if the door is a heavy gate.
```csharp
public static bool IsGate(this Door door) => door is Gate;
```

---

## 📦 Class: EffectExtensions

### 🔹 `EnableEffect()`
**Description:** Enables a specific status effect on a single player.
```csharp
public static void EnableEffect(this Player player, FacilityEffectType effect, byte intensity = 1, float duration = 0f)
```

### 🔹 `DisableEffect()`
**Description:** Disables a specific status effect on a single player.
```csharp
public static void DisableEffect(this Player player, FacilityEffectType effect)
```

### 🔹 `DisableAllEffects()`
**Description:** Disables all active status effects on a player.
```csharp
public static void DisableAllEffects(this Player player)
```

### 🔹 `HasEffect()`
**Description:** Returns true if the player has the specified status effect active.
```csharp
public static bool HasEffect(this Player player, FacilityEffectType effect)
```

### 🔹 `GetEffectIntensity()`
**Description:** Returns the current intensity level (0-255) of the specified status effect on the player.
```csharp
public static byte GetEffectIntensity(this Player player, FacilityEffectType effect)
```

### 🔹 `EnableEffect()`
**Description:** Enables a status effect for all players in the collection with zero heap allocations. Uses ValueTuple state-passing and static lambdas to completely avoid GC display class allocation.
```csharp
public static void EnableEffect(this IEnumerable<Player> players, FacilityEffectType effect, byte intensity = 1, float duration = 0f)
```

### 🔹 `EnableEffect()`
**Description:** Enables a status effect for all provided players.
```csharp
public static void EnableEffect(FacilityEffectType effect, params Player[] players) => players.EnableEffect(effect);
```

### 🔹 `EnableEffect()`
**Description:** Enables a status effect for all provided players with custom intensity and duration.
```csharp
public static void EnableEffect(FacilityEffectType effect, byte intensity, float duration, params Player[] players) => players.EnableEffect(effect, intensity, duration);
```

### 🔹 `DisableEffect()`
**Description:** Disables a status effect for all players in the collection with zero heap allocations. Uses state-passing and static lambdas to completely avoid GC display class allocation.
```csharp
public static void DisableEffect(this IEnumerable<Player> players, FacilityEffectType effect)
```

### 🔹 `DisableEffect()`
**Description:** Disables a status effect for all provided players.
```csharp
public static void DisableEffect(FacilityEffectType effect, params Player[] players) => players.DisableEffect(effect);
```

### 🔹 `DisableAllEffects()`
**Description:** Disables all status effects for all players in the collection with zero heap allocations.
```csharp
public static void DisableAllEffects(this IEnumerable<Player> players)
```

### 🔹 `DisableAllEffects()`
**Description:** Disables all status effects for all provided players.
```csharp
public static void DisableAllEffects(params Player[] players) => players.DisableAllEffects();
```

---

## 📦 Class: ElevatorExtensions

### 🔹 `GetZone()`
**Description:** Instantly resolves the target destination <see cref="FacilityZone"/> of this elevator sequence.
```csharp
public static FacilityZone GetZone(this Elevator elevator)
```

### 🔹 `OpenActiveDoors()`
**Description:** Opens only the doors located on the elevator's current floor.
```csharp
public static void OpenActiveDoors(this Elevator elevator, bool bypassLocks = false)
```

### 🔹 `CloseActiveDoors()`
**Description:** Closes only the doors located on the elevator's current floor.
```csharp
public static void CloseActiveDoors(this Elevator elevator, bool bypassLocks = false)
```

### 🔹 `OpenActiveDoors()`
**Description:** Opens active-floor doors for multiple elevators.
```csharp
public static void OpenActiveDoors(this IEnumerable<Elevator> elevators, bool bypassLocks = false)
```

### 🔹 `OpenActiveDoors()`
**Description:** Opens active-floor doors for multiple elevators (params overload).
```csharp
public static void OpenActiveDoors(bool bypassLocks, params Elevator[] elevators) => elevators.OpenActiveDoors(bypassLocks);
```

### 🔹 `CloseActiveDoors()`
**Description:** Closes active-floor doors for multiple elevators.
```csharp
public static void CloseActiveDoors(this IEnumerable<Elevator> elevators, bool bypassLocks = false)
```

### 🔹 `CloseActiveDoors()`
**Description:** Closes active-floor doors for multiple elevators (params overload).
```csharp
public static void CloseActiveDoors(bool bypassLocks, params Elevator[] elevators) => elevators.CloseActiveDoors(bypassLocks);
```

---

## 📦 Class: ElevatorLightingExtensions

### 🔹 `TurnOffLights()`
**Description:** Turns off lights in the elevator cabin for the given duration.
```csharp
public static void TurnOffLights(this Elevator elevator, float duration)
```

### 🔹 `TurnOnLights()`
**Description:** Turns on lights in the elevator cabin.
```csharp
public static void TurnOnLights(this Elevator elevator)
```

### 🔹 `AreLightsOff()`
**Description:** Returns true if the elevator cabin lights are currently disabled.
```csharp
public static bool AreLightsOff(this Elevator elevator)
```

### 🔹 `FlickerElevatorLightsCoroutine()`
**Description:** Runs a flicker animation coroutine for elevator lighting.
```csharp
public static IEnumerator<float> FlickerElevatorLightsCoroutine(this Elevator elevator, float duration, float frequency)
```

### 🔹 `TurnOffLights()`
**Description:** Turns off lights for multiple elevators with zero heap allocations.
```csharp
public static void TurnOffLights(this IEnumerable<Elevator> elevators, float duration)
```

### 🔹 `TurnOffLights()`
**Description:** Turns off lights for multiple elevators (params overload).
```csharp
public static void TurnOffLights(float duration, params Elevator[] elevators) => elevators.TurnOffLights(duration);
```

### 🔹 `TurnOnLights()`
**Description:** Turns on lights for multiple elevators with zero heap allocations.
```csharp
public static void TurnOnLights(this IEnumerable<Elevator> elevators)
```

### 🔹 `TurnOnLights()`
**Description:** Turns on lights for multiple elevators (params overload).
```csharp
public static void TurnOnLights(params Elevator[] elevators) => elevators.TurnOnLights();
```

---

## 📦 Class: EnumExtensions

### 🔹 `ToAudioKey()`
**Description:** Returns the enum value as a lowercase string without heap allocations or boxing. Reuses JIT-cached string instances.
```csharp
public static string ToAudioKey<T>(this T value) where T : struct, Enum
```

### 🔹 `ParseOrDefault()`
**Description:** Parses the string into an enum value or returns the fallback.
```csharp
public static T ParseOrDefault<T>(this string value, T defaultValue = default, bool ignoreCase = true) where T : struct, Enum
```

### 🔹 `GetRandomValue()`
**Description:** Returns a random value from the enum with zero allocations.
```csharp
public static T GetRandomValue<T>() where T : struct, Enum
```

---

## 📦 Class: FirearmExtensions

### 🔹 `HasAttachment()`
**Description:** Returns true if the firearm has the specified attachment enabled.
```csharp
public static bool HasAttachment(this FirearmItem firearm, AttachmentName attachmentName)
```

### 🔹 `HasAttachments()`
**Description:** Returns true if the firearm has all specified attachments enabled. Uses state-passing All helper to guarantee 0 allocations and instant early-exit.
```csharp
public static bool HasAttachments(this FirearmItem firearm, IEnumerable<AttachmentName> attachmentNames)
```

### 🔹 `HasAttachments()`
**Description:** Returns true if the firearm has all specified attachments enabled (params overload).
```csharp
public static bool HasAttachments(this FirearmItem firearm, params AttachmentName[] attachmentNames) => firearm.HasAttachments((IEnumerable<AttachmentName>)attachmentNames);
```

### 🔹 `HasAttachment()`
**Description:** Returns true if all firearms in the collection have the specified attachment enabled.
```csharp
public static bool HasAttachment(this IEnumerable<FirearmItem> firearms, AttachmentName attachmentName)
```

### 🔹 `HasAttachment()`
**Description:** Returns true if all firearms in the params array have the specified attachment enabled.
```csharp
public static bool HasAttachment(AttachmentName attachmentName, params FirearmItem[] firearms) => ((IEnumerable<FirearmItem>)firearms).HasAttachment(attachmentName);
```

---

## 📦 Class: HandlerExtensions

### 🔹 `RegisterAll()`
**Description:** Registers multiple event handlers with zero heap allocations.
```csharp
public static void RegisterAll(this IEnumerable<CustomEventsHandler> handlers)
```

### 🔹 `RegisterAll()`
**Description:** Registers multiple event handlers (params overload).
```csharp
public static void RegisterAll(params CustomEventsHandler[] handlers)
```

### 🔹 `UnregisterAll()`
**Description:** Unregisters multiple event handlers with zero heap allocations.
```csharp
public static void UnregisterAll(this IEnumerable<CustomEventsHandler> handlers)
```

### 🔹 `UnregisterAll()`
**Description:** Unregisters multiple event handlers (params overload).
```csharp
public static void UnregisterAll(params CustomEventsHandler[] handlers)
```

---

## 📦 Class: IoExtensions

### 🔹 `IsReparsePoint()`
**Description:** Returns true if the path exists and is a reparse point (symlink, junction, etc.). Bypasses redundant filesystem existance checks to minimize disk IO roundtrips.
```csharp
public static bool IsReparsePoint(this string path)
```

### 🔹 `CopyFilesTo()`
**Description:** Copies files from one directory to another using a search pattern. Features lazy file enumeration and state-passing loops to prevent massive string array allocations.
```csharp
public static void CopyFilesTo( this string sourceDirectory, string destinationDirectory, string searchPattern = "*.*", bool overwrite = true)
```

---

## 📦 Class: MapExtensions

### 🔹 `BreakAllFacilityDoors()`
**Description:** Breaks all breakable doors in the entire facility with zero heap allocations.
```csharp
public static void BreakAllFacilityDoors()
```

### 🔹 `SetAllLightsEnabled()`
**Description:** Enables or disables all lights in the facility with zero heap allocations.
```csharp
public static void SetAllLightsEnabled(bool enabled)
```

### 🔹 `StartEmergencyStrobe()`
**Description:** Launches a highly optimized global facility-wide emergency strobe light sequence using a single lightweight coroutine loop.
```csharp
public static CoroutineHandle StartEmergencyStrobe( float totalDuration, float pulseInterval, Color alertColor, string coroutineTag = "LabApi.MapExtensions-emergencyStrobe")
```

### 🔹 `GetEngagedGeneratorsCount()`
**Description:** Returns the number of engaged generators.
```csharp
public static int GetEngagedGeneratorsCount()
```

### 🔹 `AreAllGeneratorsEngaged()`
**Description:** Returns true if all generators are engaged and the count meets the required minimum.
```csharp
public static bool AreAllGeneratorsEngaged(int minimumRequiredCount = 3)
```

---

## 📦 Class: MathExtensions

### 🔹 `Clamp()`
**Description:** Clamps the value between the given minimum and maximum.
```csharp
public static float Clamp(this float value, float min, float max) => value < min ? min : (value > max ? max : value);
```

### 🔹 `Clamp()`
**Description:** Clamps the value between the given minimum and maximum.
```csharp
public static int Clamp(this int value, int min, int max) => value < min ? min : (value > max ? max : value);
```

### 🔹 `Clamp()`
**Description:** Clamps the value between the given minimum and maximum.
```csharp
public static double Clamp(this double value, double min, double max) => value < min ? min : (value > max ? max : value);
```

### 🔹 `LimitMin()`
**Description:** Ensures the value is not lower than the given minimumBaseline.
```csharp
public static float LimitMin(this float value, float minBaseline) => value < minBaseline ? minBaseline : value;
```

### 🔹 `LimitMin()`
**Description:** Ensures the value is not lower than the given minimumBaseline.
```csharp
public static int LimitMin(this int value, int minBaseline) => value < minBaseline ? minBaseline : value;
```

### 🔹 `LimitMin()`
**Description:** Ensures the value is not lower than the given minimumBaseline.
```csharp
public static double LimitMin(this double value, double minBaseline) => value < minBaseline ? minBaseline : value;
```

### 🔹 `LimitMax()`
**Description:** Ensures the value is not higher than the given maximumBaseline.
```csharp
public static float LimitMax(this float value, float maxBaseline) => value > maxBaseline ? maxBaseline : value;
```

### 🔹 `LimitMax()`
**Description:** Ensures the value is not higher than the given maximumBaseline.
```csharp
public static int LimitMax(this int value, int maxBaseline) => value > maxBaseline ? maxBaseline : value;
```

### 🔹 `LimitMax()`
**Description:** Ensures the value is not higher than the given maximumBaseline.
```csharp
public static double LimitMax(this double value, double maxBaseline) => value > maxBaseline ? maxBaseline : value;
```

### 🔹 `Abs()`
**Description:** Returns the absolute value.
```csharp
public static int Abs(this int value) => value < 0 ? -value : value;
```

### 🔹 `Abs()`
**Description:** Returns the absolute value.
```csharp
public static float Abs(this float value) => value < 0f ? -value : value;
```

### 🔹 `Abs()`
**Description:** Returns the absolute value.
```csharp
public static double Abs(this double value) => value < 0.0 ? -value : value;
```

### 🔹 `Sign()`
**Description:** Returns -1, 0 or 1 depending on the value.
```csharp
public static int Sign(this int value) => value == 0 ? 0 : (value > 0 ? 1 : -1);
```

### 🔹 `Sign()`
**Description:** Returns -1, 0 or 1 depending on the value.
```csharp
public static float Sign(this float value) => value == 0f ? 0f : (value > 0f ? 1f : -1f);
```

### 🔹 `Sign()`
**Description:** Returns -1, 0 or 1 depending on the value.
```csharp
public static double Sign(this double value) => value == 0.0 ? 0.0 : (value > 0.0 ? 1.0 : -1.0);
```

### 🔹 `Lerp()`
**Description:** Linearly interpolates between two values, clamping t between 0 and 1.
```csharp
public static float Lerp(this float from, float to, float t)
```

### 🔹 `LerpUnclamped()`
**Description:** Linearly interpolates without clamping t.
```csharp
public static float LerpUnclamped(this float from, float to, float t) => from + (to - from) * t;
```

### 🔹 `Floor()`
**Description:** Floors the value.
```csharp
public static float Floor(this float value) => (float)Math.Floor(value);
```

### 🔹 `Floor()`
**Description:** Floors the value.
```csharp
public static double Floor(this double value) => Math.Floor(value);
```

### 🔹 `Ceil()`
**Description:** Ceils the value.
```csharp
public static float Ceil(this float value) => (float)Math.Ceiling(value);
```

### 🔹 `Ceil()`
**Description:** Ceils the value.
```csharp
public static double Ceil(this double value) => Math.Ceiling(value);
```

### 🔹 `DbToLinear()`
**Description:** Converts decibels to a linear 0–1 value.
```csharp
public static float DbToLinear(this float db) => db <= -96f ? 0f : (float)Math.Pow(10.0, db / 20.0);
```

### 🔹 `LinearToDb()`
**Description:** Converts a linear 0–1 value to decibels.
```csharp
public static float LinearToDb(this float linear) => linear <= 0.00001f ? -96f : 20f * (float)Math.Log10(linear);
```

### 🔹 `IsNanOrInfinity()`
**Description:** Returns true if the value is NaN or Infinity.
```csharp
public static bool IsNanOrInfinity(this float value) => float.IsNaN(value) || float.IsInfinity(value);
```

### 🔹 `IsNanOrInfinity()`
**Description:** Returns true if the value is NaN or Infinity.
```csharp
public static bool IsNanOrInfinity(this double value) => double.IsNaN(value) || double.IsInfinity(value);
```

### 🔹 `IsNanOrInfinity()`
**Description:** Always false for integers.
```csharp
public static bool IsNanOrInfinity(this int value) => false;
```

### 🔹 `RoundToInt()`
**Description:** Rounds the value to the nearest integer.
```csharp
public static int RoundToInt(this float value) => (int)Math.Round(value);
```

### 🔹 `RoundToInt()`
**Description:** Rounds the value to the nearest integer.
```csharp
public static int RoundToInt(this double value) => (int)Math.Round(value);
```

### 🔹 `RollChance()`
**Description:** Rolls an integer percentage-based chance (0% to 100%). Returns true if the roll succeeds.
```csharp
public static bool RollChance(this int percentageChance)
```

### 🔹 `RollChance()`
**Description:** Rolls a float percentage-based chance (0.0% to 100.0%). Returns true if the roll succeeds.
```csharp
public static bool RollChance(this float percentageChance)
```

### 🔹 `RollChance()`
**Description:** Rolls a double percentage-based chance (0.0% to 100.0%). Returns true if the roll succeeds.
```csharp
public static bool RollChance(this double percentageChance)
```

### 🔹 `RollChanceNormalized()`
**Description:** Rolls an integer normalized-based chance (0 or 1). Returns true if the roll succeeds (>= 1).
```csharp
public static bool RollChanceNormalized(this int normalizedChance)
```

### 🔹 `RollChanceNormalized()`
**Description:** Rolls a float normalized-based chance (0.0 to 1.0). Returns true if the roll succeeds.
```csharp
public static bool RollChanceNormalized(this float normalizedChance)
```

### 🔹 `RollChanceNormalized()`
**Description:** Rolls a double normalized-based chance (0.0 to 1.0). Returns true if the roll succeeds.
```csharp
public static bool RollChanceNormalized(this double normalizedChance)
```

### 🔹 `RandomTo()`
**Description:** Returns a random float between this minimum and the specified maximum.
```csharp
public static float RandomTo(this float min, float max) => SafeRandom.Range(min, max);
```

### 🔹 `RandomTo()`
**Description:** Returns a random integer between this minimum (inclusive) and the specified maximum (exclusive).
```csharp
public static int RandomTo(this int min, int max) => SafeRandom.Next(min, max);
```

---

## 📦 Class: PickupExtensions

### 🔹 `ApplyKineticBlast()`
**Description:** Applies a physics impulse to a pickup safely.
```csharp
public static void ApplyKineticBlast(this Pickup pickup, float linearVelocityMagnitude, float angularVelocityMagnitude)
```

### 🔹 `ApplyKineticBlast()`
**Description:** Applies a physics impulse to multiple pickups with 0 heap allocations.
```csharp
public static void ApplyKineticBlast(this IEnumerable<Pickup> pickups, float linearVelocityMagnitude, float angularVelocityMagnitude)
```

### 🔹 `ApplyKineticBlast()`
**Description:** Applies a physics impulse to multiple pickups (params overload).
```csharp
public static void ApplyKineticBlast(float linearVelocityMagnitude, float angularVelocityMagnitude, params Pickup[] pickups)
```

---

## 📦 Class: PlayerExtensions

### 🔹 `AttachTrackingObject()`
**Description:** Attaches a follower object to the player. The follower tracks the player's transform with an optional offset.
```csharp
public static void AttachTrackingObject(this Player player, GameObject followerObject, Vector3 offset = default)
```

### 🔹 `AttachTrackingObject()`
**Description:** Attaches a follower object to multiple players with zero heap allocations.
```csharp
public static void AttachTrackingObject(this IEnumerable<Player> players, GameObject followerObject, Vector3 offset = default)
```

### 🔹 `AttachTrackingObject()`
**Description:** Attaches a follower object to multiple players (params overload).
```csharp
public static void AttachTrackingObject(GameObject followerObject, Vector3 offset, params Player[] players) => players.AttachTrackingObject(followerObject, offset);
```

### 🔹 `BroadcastHintToAll()`
**Description:** Sends a hint to all ready players.
```csharp
public static void BroadcastHintToAll(string hintContent, float duration = 5f)
```

### 🔹 `BroadcastHint()`
**Description:** Sends a hint to multiple players with zero heap allocations. Only ready players receive the hint.
```csharp
public static void BroadcastHint(this IEnumerable<Player> players, string hintContent, float duration = 5f)
```

### 🔹 `BroadcastHint()`
**Description:** Sends a hint to multiple players (params overload).
```csharp
public static void BroadcastHint(string hintContent, float duration, params Player[] players) => players.BroadcastHint(hintContent, duration);
```

### 🔹 `GetHumeShieldValue()`
**Description:** Returns the player's current Hume Shield value. Returns 0 if the stat module is missing.
```csharp
public static float GetHumeShieldValue(this Player player)
```

### 🔹 `IsLivingHuman()`
**Description:** Returns true if the player is ready, alive and belongs to a human faction.
```csharp
public static bool IsLivingHuman(this Player player)
```

### 🔹 `GetDistanceTo()`
**Description:** Returns the distance between the player and a world position.
```csharp
public static float GetDistanceTo(this Player player, Vector3 position)
```

### 🔹 `IsWithinDistance()`
**Description:** Returns true if the player is within the given distance from a world position. Uses squared magnitude for performance.
```csharp
public static bool IsWithinDistance(this Player player, Vector3 position, float maxDistance)
```

### 🔹 `IsWithinRadius()`
**Description:** Returns true if the player is within the given radius from a world position. Uses squared magnitude for performance.
```csharp
public static bool IsWithinRadius(this Player player, Vector3 targetPosition, float radiusSize)
```

### 🔹 `IsWithinRadius()`
**Description:** Returns true if the player is within the given radius from the room center.
```csharp
public static bool IsWithinRadius(this Player player, Room room, float radiusSize)
```

### 🔹 `IsWithinAnyRadius()`
**Description:** Returns true if the player is within the given radius of any provided positions. Uses squared magnitude for performance with optimized fast-paths.
```csharp
public static bool IsWithinAnyRadius(this Player player, IEnumerable<Vector3> positions, float radiusSize)
```

### 🔹 `IsInRoom()`
**Description:** Returns true if the player is currently inside any of the given rooms.
```csharp
public static bool IsInRoom(this Player player, params RoomName[] roomNames)
```

### 🔹 `GetRoom()`
**Description:** Returns the room the player is currently standing in.
```csharp
public static Room GetRoom(this Player player)
```

### 🔹 `GetDistanceTo()`
**Description:** Returns the distance between the player and the room center.
```csharp
public static float GetDistanceTo(this Player player, Room room)
```

### 🔹 `ApplySensoryShock()`
**Description:** Applies visual and auditory impairments to the player.
```csharp
public static void ApplySensoryShock( this Player player, float flashDuration = 0f, float blindDuration = 0f, float blurDuration = 0f, float deafenDuration = 0f)
```

### 🔹 `ApplySensoryShock()`
**Description:** Applies sensory shock to multiple players with zero heap allocations. Uses ValueTuple state-passing to completely prevent closure garbage generation.
```csharp
public static void ApplySensoryShock( this IEnumerable<Player> players, float flashDuration = 0f, float blindDuration = 0f, float blurDuration = 0f, float deafenDuration = 0f)
```

### 🔹 `ApplySensoryShock()`
**Description:** Applies sensory shock to multiple players (params overload).
```csharp
public static void ApplySensoryShock( float flashDuration, float blindDuration, float blurDuration, float deafenDuration, params Player[] players) => players.ApplySensoryShock(flashDuration, blindDuration, blurDuration, deafenDuration);
```

### 🔹 `WhereAlive()`
**Description:** Returns only players who are ready and alive.
```csharp
public static IEnumerable<Player> WhereAlive(this IEnumerable<Player> players)
```

### 🔹 `WhereHuman()`
**Description:** Returns only players who belong to a human faction.
```csharp
public static IEnumerable<Player> WhereHuman(this IEnumerable<Player> players)
```

### 🔹 `WhereNotInPocket()`
**Description:** Returns only players who are not inside the Pocket Dimension.
```csharp
public static IEnumerable<Player> WhereNotInPocket(this IEnumerable<Player> players)
```

### 🔹 `IsEligibleForEscape()`
**Description:** Returns true if the player meets basic conditions to escape.
```csharp
public static bool IsEligibleForEscape(this Player player, IEnumerable<Vector3> escapeZones, float escapeZoneSize)
```

### 🔹 `IsInShelter()`
**Description:** Returns true if the player is inside a shelter room or within any shelter radius.
```csharp
public static bool IsInShelter(this Player player, float shelterZoneSize, IEnumerable<Vector3> shelterLocations, params RoomName[] additionalRooms)
```

### 🔹 `IsInExecutiveElevator()`
**Description:** Returns true if the player is inside an elevator cabin.
```csharp
public static bool IsInExecutiveElevator(this Player player)
```

### 🔹 `TryGetNearbyElevatorCabin()`
**Description:** Returns true if there is an elevator cabin near the player and outputs it. Shared with lighting extensions for modularity.
```csharp
public static bool TryGetNearbyElevatorCabin(this Player player, out Elevator cabin)
```

### 🔹 `TryResolveFuzzy()`
**Description:** Attempts to resolve a player from a fuzzy identifier. Supports exact ID, UserId, exact nickname, substring match, and Levenshtein fallback.
```csharp
public static bool TryResolveFuzzy(this IEnumerable<Player> players, string identifier, out Player target, out string error)
```

---

## 📦 Class: PlayerLightingExtensions

### 🔹 `HasActiveLightSource()`
**Description:** Returns true if the player's held item emits light (flashlight or light item).
```csharp
public static bool HasActiveLightSource(this Player player)
```

### 🔹 `GetHeldLightSourceState()`
**Description:** Returns true if the player's held light source is currently emitting.
```csharp
public static bool GetHeldLightSourceState(this Player player)
```

### 🔹 `SetHeldLightSourceState()`
**Description:** Sets the emission state of the player's held light source.
```csharp
public static void SetHeldLightSourceState(this Player player, bool emit)
```

### 🔹 `SetHeldLightSourceState()`
**Description:** Sets the emission state of held light sources for multiple players with zero heap allocations.
```csharp
public static void SetHeldLightSourceState(this IEnumerable<Player> players, bool emit)
```

### 🔹 `SetHeldLightSourceState()`
**Description:** Sets the emission state of held light sources (params overload).
```csharp
public static void SetHeldLightSourceState(bool emit, params Player[] players) => players.SetHeldLightSourceState(emit);
```

### 🔹 `IsInDarkRoom()`
**Description:** Returns true if the player is in a room with lights turned off. Fully optimized list loops bypassing struct-boxing.
```csharp
public static bool IsInDarkRoom(this Player player)
```

### 🔹 `IsInTrueDarkness()`
**Description:** Returns true if the player is in total darkness (dark room or dark elevator cabin). Reuses modular Player spatial extensions to minimize code duplication.
```csharp
public static bool IsInTrueDarkness(this Player player)
```

### 🔹 `FlickerHeldLightSourceCoroutine()`
**Description:** Coroutine that flickers the player's held light source.
```csharp
public static IEnumerator<float> FlickerHeldLightSourceCoroutine( this Player player, int flickerCount, float delayPerFlicker, bool forceOff = false, Action<Player, bool> onTickFeedback = null)
```

### 🔹 `FlickerHeldLightSource()`
**Description:** Starts the flicker coroutine for multiple players with zero heap allocations. Uses ValueTuple state-passing to completely prevent closure garbage generation.
```csharp
public static void FlickerHeldLightSource( this IEnumerable<Player> players, int flickerCount, float delayPerFlicker, bool forceOff = false, Action<Player, bool> onTickFeedback = null, string coroutineTag = "LabApi.Extensions-playerFlickerLights")
```

### 🔹 `FlickerHeldLightSource()`
**Description:** Starts the flicker coroutine for multiple players (params overload).
```csharp
public static void FlickerHeldLightSource( int flickerCount, float delayPerFlicker, bool forceOff = false, Action<Player, bool> onTickFeedback = null, string coroutineTag = "LabApi.Extensions-playerFlickerLights", params Player[] players) => players.FlickerHeldLightSource(flickerCount, delayPerFlicker, forceOff, onTickFeedback, coroutineTag);
```

---

## 📦 Class: PluginConfigExtensions

### 🔹 `LoadOrCreateSubConfig()`
**Description:** Loads a sub‑config file or creates a new one if missing or invalid. Runs optional validation and saves the result back to disk. <typeparam name="TMainConfig">Main plugin config type.</typeparam> <typeparam name="TSubConfig">Sub‑config type to load.</typeparam>
```csharp
public static TSubConfig LoadOrCreateSubConfig<TMainConfig, TSubConfig>( this Plugin<TMainConfig> plugin, string fileName, Action<TSubConfig> validationAction = null) where TMainConfig : LabApiConfig, new() where TSubConfig : class, new()
```

---

## 📦 Class: RagdollExtensions

### 🔹 `GetRagdoll()`
**Description:** Resolves the API-wrapped <see cref="Ragdoll"/> proxy from this native low-level Unity <see cref="BasicRagdoll"/>.
```csharp
public static Ragdoll GetRagdoll(this BasicRagdoll ragdoll)
```

---

## 📦 Class: ReferenceHubExtensions

### 🔹 `GetPlayer()`
**Description:** Resolves the strongly-typed LabAPI <see cref="Player"/> wrapper from this native <see cref="ReferenceHub"/> safely.
```csharp
public static Player GetPlayer(this ReferenceHub referenceHub)
```

---

## 📦 Class: RoomExtensions

### 🔹 `GetNeighbors()`
**Description:** Returns all rooms directly connected to this room.
```csharp
public static IEnumerable<Room> GetNeighbors(this Room room)
```

### 🔹 `GetElevatorsConnectedToRoom()`
**Description:** Returns elevators connected to the given room. Safe from recursion loops.
```csharp
public static IEnumerable<Elevator> GetElevatorsConnectedToRoom(this Room room)
```

### 🔹 `WhereNotInPocket()`
**Description:** Returns all rooms except the Pocket Dimension with optimized collection fast-paths.
```csharp
public static IEnumerable<Room> WhereNotInPocket(this IEnumerable<Room> rooms)
```

### 🔹 `IsCheckpoint()`
**Description:** Returns true if the room is a checkpoint. Compiled to highly optimized JIT jump tables.
```csharp
public static bool IsCheckpoint(this RoomName name) => name is RoomName.LczCheckpointA or RoomName.LczCheckpointB or RoomName.HczCheckpointA or RoomName.HczCheckpointB or RoomName.HczCheckpointToEntranceZone;
```

### 🔹 `IsScpRoom()`
**Description:** Returns true if the room is an SCP containment room. Compiled to highly optimized JIT jump tables.
```csharp
public static bool IsScpRoom(this RoomName name) => name is RoomName.Lcz173 or RoomName.Lcz330 or RoomName.Hcz049 or RoomName.Hcz079 or RoomName.Hcz096 or RoomName.Hcz106 or RoomName.Hcz939 or RoomName.Lcz914 or RoomName.HczTestroom;
```

### 🔹 `IsArmory()`
**Description:** Returns true if the room is an armory. Compiled to highly optimized JIT jump tables.
```csharp
public static bool IsArmory(this RoomName name) => name is RoomName.LczArmory or RoomName.HczArmory;
```

### 🔹 `IsFreeOfEngagedGenerators()`
**Description:** Returns true if the room has no engaged generators.
```csharp
public static bool IsFreeOfEngagedGenerators(this Room room)
```

### 🔹 `IsRoomAndNeighborsFreeOfEngagedGenerators()`
**Description:** Returns true if the room and all its neighbors have no engaged generators. Bypasses neighbor iterator allocations entirely.
```csharp
public static bool IsRoomAndNeighborsFreeOfEngagedGenerators(this Room room)
```

### 🔹 `IsElevatorActiveInRoom()`
**Description:** Returns true if any elevator connected to the room is currently moving.
```csharp
public static bool IsElevatorActiveInRoom(this Room room)
```

### 🔹 `ExecuteActionOnRoomAndNeighbors()`
**Description:** Executes an action on the room and all its neighbors with 0 iterator allocations.
```csharp
public static void ExecuteActionOnRoomAndNeighbors(this Room room, Action<Room> action)
```

### 🔹 `ExecuteActionOnRoomAndNeighbors()`
**Description:** Executes an action on the room and all its neighbors passing a custom state with 0 allocations.
```csharp
public static void ExecuteActionOnRoomAndNeighbors<TState>(this Room room, TState state, Action<Room, TState> action)
```

### 🔹 `HandleElevatorsForRoom()`
**Description:** Executes an action on all elevators connected to the room. Bypasses state-machine iterator allocations.
```csharp
public static void HandleElevatorsForRoom(this Room room, float affectChance, Action<Elevator> action)
```

### 🔹 `BreakAllDoors()`
**Description:** Breaks all breakable doors in the room with 0 allocations.
```csharp
public static void BreakAllDoors(this Room room)
```

### 🔹 `BreakAllDoors()`
**Description:** Breaks all breakable doors in multiple rooms with 0 allocations.
```csharp
public static void BreakAllDoors(this IEnumerable<Room> rooms)
```

### 🔹 `BreakAllDoors()`
**Description:** Breaks all breakable doors in multiple rooms (params overload).
```csharp
public static void BreakAllDoors(params Room[] rooms) => rooms.BreakAllDoors();
```

### 🔹 `GetDistanceTo()`
**Description:** Returns the distance from the room center to the given position.
```csharp
public static float GetDistanceTo(this Room room, Vector3 position)
```

### 🔹 `IsWithinRadius()`
**Description:** Returns true if the position is within the given radius from the room center.
```csharp
public static bool IsWithinRadius(this Room room, Vector3 position, float radius)
```

---

## 📦 Class: RoomLightingExtensions

### 🔹 `TurnOffLights()`
**Description:** Turns off lights in the room for a given duration.
```csharp
public static void TurnOffLights(this Room room, float duration)
```

### 🔹 `TurnOnLights()`
**Description:** Turns on lights in the room and optionally flickers them.
```csharp
public static void TurnOnLights(this Room room, float flickerDuration = 0f)
```

### 🔹 `TurnOffRoomAndNeighborLights()`
**Description:** Turns off lights in the room and its neighbors. Reuses bezalokacyjne propagation helper.
```csharp
public static void TurnOffRoomAndNeighborLights(this Room room, float duration) => room.ExecuteActionOnRoomAndNeighbors(duration, static (r, dur) => r.TurnOffLights(dur));
```

### 🔹 `TurnOnRoomAndNeighborLights()`
**Description:** Turns on lights in the room and its neighbors. Reuses bezalokacyjne propagation helper.
```csharp
public static void TurnOnRoomAndNeighborLights(this Room room, float duration = 0f) => room.ExecuteActionOnRoomAndNeighbors(duration, static (r, dur) => r.TurnOnLights(dur));
```

### 🔹 `SetLightsColor()`
**Description:** Sets the light color for the room.
```csharp
public static void SetLightsColor(this Room room, Color color)
```

### 🔹 `SetLightsColor()`
**Description:** Sets light color for multiple rooms with 0 allocations.
```csharp
public static void SetLightsColor(this IEnumerable<Room> rooms, Color color)
```

### 🔹 `SetLightsColor()`
**Description:** Sets light color for multiple rooms (params overload).
```csharp
public static void SetLightsColor(Color color, params Room[] rooms) => rooms.SetLightsColor(color);
```

### 🔹 `TurnOffLights()`
**Description:** Turns off lights for multiple rooms with 0 allocations.
```csharp
public static void TurnOffLights(this IEnumerable<Room> rooms, float duration)
```

### 🔹 `TurnOffLights()`
**Description:** Turns off lights for multiple rooms (params overload).
```csharp
public static void TurnOffLights(float duration, params Room[] rooms) => rooms.TurnOffLights(duration);
```

### 🔹 `TurnOnLights()`
**Description:** Turns on lights for multiple rooms with 0 allocations.
```csharp
public static void TurnOnLights(this IEnumerable<Room> rooms, float flickerDuration = 0f)
```

### 🔹 `TurnOnLights()`
**Description:** Turns on lights for multiple rooms (params overload).
```csharp
public static void TurnOnLights(float flickerDuration, params Room[] rooms) => rooms.TurnOnLights(flickerDuration);
```

### 🔹 `FlickerLightsCoroutine()`
**Description:** Executes a flicker animation on the room lights with 0 array allocation in standard lists/arrays.
```csharp
public static IEnumerator<float> FlickerLightsCoroutine(this Room room, Color color, float duration, float frequency)
```

### 🔹 `FlickerLights()`
**Description:** Starts a flicker animation on multiple rooms with 0 allocations.
```csharp
public static void FlickerLights( this IEnumerable<Room> rooms, Color color, float duration, float frequency, string coroutineTag = "LabApi.Extensions-flickerLights")
```

### 🔹 `FlickerLights()`
**Description:** Starts a flicker animation on multiple rooms (params overload).
```csharp
public static void FlickerLights( Color color, float duration, float frequency, string coroutineTag = "LabApi.Extensions-flickerLights", params Room[] rooms) => rooms.FlickerLights(color, duration, frequency, coroutineTag);
```

---

## 📦 Class: StringExtensions

### 🔹 `NormalizeUserId()`
**Description:** Returns the user ID in lowercase, or an empty string if null. Bypasses string allocations completely if the ID is already lowercase (99% of cases).
```csharp
public static string NormalizeUserId(this string userId)
```

### 🔹 `ComputeLevenshteinDistance()`
**Description:** Returns the Levenshtein distance between two strings with absolutely zero heap allocations. Reuses pooled array buffers from ArrayPool.
```csharp
public static int ComputeLevenshteinDistance(this string source, string target)
```

---

## 📦 Class: TeslaExtensions

### 🔹 `DisableFor()`
**Description:** Disables a Tesla gate for a given duration. Use <paramref name="forceTrigger"/> to trigger a discharge.
```csharp
public static void DisableFor(this Tesla tesla, float duration, bool forceTrigger = true)
```

### 🔹 `DisableFor()`
**Description:** Disables multiple Tesla gates for a given duration with zero heap allocations.
```csharp
public static void DisableFor(this IEnumerable<Tesla> teslas, float duration, bool forceTrigger = true)
```

### 🔹 `DisableFor()`
**Description:** Disables multiple Tesla gates (params overload).
```csharp
public static void DisableFor(float duration, bool forceTrigger, params Tesla[] teslas)
```

---

## 📦 Class: TimingExtensions

### 🔹 `CallDelayedIf()`
**Description:** Calls <paramref name="action"/> after a delay if <paramref name="condition"/> returns true.
```csharp
public static CoroutineHandle CallDelayedIf(float delay, Func<bool> condition, Action action, string coroutineTag = "LabApi.Extensions-callDelayedIf")
```

### 🔹 `Kill()`
**Description:** Kills all coroutines bound to this tag with zero allocations.
```csharp
public static void Kill(this string tag)
```

### 🔹 `Kill()`
**Description:** Kills this coroutine handle if it is running.
```csharp
public static void Kill(this CoroutineHandle handle)
```

### 🔹 `Kill()`
**Description:** Kills all coroutines bound to the given tags with zero heap allocations.
```csharp
public static void Kill(this IEnumerable<string> tags)
```

### 🔹 `Kill()`
**Description:** Kills all coroutines bound to the given tags (params overload).
```csharp
public static void Kill(params string[] tags) => ((IEnumerable<string>)tags).Kill();
```

### 🔹 `KillAll()`
**Description:** Kills all coroutines associated with the given handles with zero heap allocations.
```csharp
public static void KillAll(this IEnumerable<CoroutineHandle> handles)
```

### 🔹 `KillAll()`
**Description:** Kills all coroutines associated with the given handles (params overload).
```csharp
public static void KillAll(params CoroutineHandle[] handles) => ((IEnumerable<CoroutineHandle>)handles).KillAll();
```

### 🔹 `KillAllAndClear()`
**Description:** Kills all coroutines in the list and clears it safely.
```csharp
public static void KillAllAndClear(this List<CoroutineHandle> handles)
```

---

## 📦 Class: VectorExtensions

### 🔹 `GetDistanceTo()`
**Description:** Returns the distance between two points.
```csharp
public static float GetDistanceTo(this Vector3 origin, Vector3 target)
```

### 🔹 `GetDistanceSquaredTo()`
**Description:** Returns the squared distance between two points with zero square root overhead.
```csharp
public static float GetDistanceSquaredTo(this Vector3 origin, Vector3 target)
```

### 🔹 `IsWithinRange()`
**Description:** Returns true if the target is within the specified range from the origin. Highly optimized utilizing squared distances to avoid expensive square root operations.
```csharp
public static bool IsWithinRange(this Vector3 origin, Vector3 target, float range)
```

### 🔹 `FlatDistanceTo()`
**Description:** Returns the distance between two points ignoring the Y axis (great for same-floor evaluations).
```csharp
public static float FlatDistanceTo(this Vector3 origin, Vector3 target)
```

### 🔹 `FlatDistanceSquaredTo()`
**Description:** Returns the squared distance between two points ignoring the Y axis with zero square root overhead.
```csharp
public static float FlatDistanceSquaredTo(this Vector3 origin, Vector3 target)
```

### 🔹 `DirectionTo()`
**Description:** Safely returns a normalized direction vector pointing from origin to target. Features built-in zero-division safety.
```csharp
public static Vector3 DirectionTo(this Vector3 origin, Vector3 target)
```

### 🔹 `WithX()`
**Description:** Returns a new Vector3 with a modified X coordinate.
```csharp
public static Vector3 WithX(this Vector3 vector, float x) => new Vector3(x, vector.y, vector.z);
```

### 🔹 `WithY()`
**Description:** Returns a new Vector3 with a modified Y coordinate.
```csharp
public static Vector3 WithY(this Vector3 vector, float y) => new Vector3(vector.x, y, vector.z);
```

### 🔹 `WithZ()`
**Description:** Returns a new Vector3 with a modified Z coordinate.
```csharp
public static Vector3 WithZ(this Vector3 vector, float z) => new Vector3(vector.x, vector.y, z);
```

### 🔹 `GetUpwardReflectedVector()`
**Description:** Reflects the vector upward if it points too far downward. Optimally negated in-place without invoking heavy engine reflection methods.
```csharp
public static Vector3 GetUpwardReflectedVector(this Vector3 direction, float dotThreshold = 0.707f)
```

### 🔹 `GetRandomUpwardSphereVelocity()`
**Description:** Returns a random upward-facing direction scaled by the given magnitude. Thread-safe and powered by SafeRandom.
```csharp
public static Vector3 GetRandomUpwardSphereVelocity(float magnitude = 1f)
```

### 🔹 `GetRoom()`
**Description:** Resolves and returns the room at this specific 3D spatial coordinate vector.
```csharp
public static Room GetRoom(this Vector3 position) => Room.GetRoomAtPosition(position);
```

### 🔹 `Sanitize()`
**Description:** Returns the vector unless it contains NaN or Infinity, in which case the fallback is returned.
```csharp
public static Vector3 Sanitize(this Vector3 vector, Vector3 fallback = default)
```

---

## 📦 Class: ZoneExtensions

### 🔹 `UnknownMember()`
**Description:** Cached array of all facility zones (zero enum allocation).
```csharp
public static readonly FacilityZone[] All = (FacilityZone[])Enum.GetValues(typeof(FacilityZone));
```

### 🔹 `IsElevatorInZone()`
**Description:** Helper to check if an elevator belongs to a specific zone without generating heap garbage. Shared internally with ZoneLightingExtensions to maintain DRY.
```csharp
internal static bool IsElevatorInZone(Elevator elevator, FacilityZone zone)
```

### 🔹 `GetDoors()`
**Description:** Returns all doors located inside the zone.
```csharp
public static IEnumerable<Door> GetDoors(this FacilityZone zone)
```

### 🔹 `OpenDoors()`
**Description:** Opens all doors in the zone with zero heap allocations.
```csharp
public static void OpenDoors(this FacilityZone zone, bool bypassLocks = false)
```

### 🔹 `CloseDoors()`
**Description:** Closes all doors in the zone with zero heap allocations.
```csharp
public static void CloseDoors(this FacilityZone zone, bool bypassLocks = false)
```

### 🔹 `SetDoorsLockState()`
**Description:** Sets the lock state of all doors in the zone with zero heap allocations. Use <paramref name="locked"/> = false to unlock them.
```csharp
public static void SetDoorsLockState(this FacilityZone zone, DoorLockReason reason, bool locked = true)
```

### 🔹 `LockElevators()`
**Description:** Locks all elevators in the zone with zero heap allocations.
```csharp
public static void LockElevators(this FacilityZone zone)
```

### 🔹 `UnlockElevators()`
**Description:** Unlocks all elevators in the zone with zero heap allocations.
```csharp
public static void UnlockElevators(this FacilityZone zone)
```

### 🔹 `GetElevators()`
**Description:** Returns all elevators whose destination rooms belong to the given zone.
```csharp
public static IEnumerable<Elevator> GetElevators(this FacilityZone zone)
```

### 🔹 `GetElevatorsInZone()`
**Description:** Returns all elevators whose destination rooms belong to the given zone.
```csharp
public static IEnumerable<Elevator> GetElevatorsInZone(FacilityZone zone) => zone.GetElevators();
```

---

## 📦 Class: ZoneLightingExtensions

### 🔹 `TurnOffLights()`
**Description:** Turns off lights in the zone and its elevators with zero heap allocations.
```csharp
public static void TurnOffLights(this FacilityZone zone, float duration)
```

### 🔹 `TurnOnLights()`
**Description:** Turns on lights in the zone and its elevators with zero heap allocations.
```csharp
public static void TurnOnLights(this FacilityZone zone)
```

### 🔹 `TurnOffLights()`
**Description:** Turns off lights across multiple zones with zero heap allocations.
```csharp
public static void TurnOffLights(this IEnumerable<FacilityZone> zones, float duration)
```

### 🔹 `TurnOffLights()`
**Description:** Turns off lights across multiple zones (params overload).
```csharp
public static void TurnOffLights(float duration, params FacilityZone[] zones)
```

### 🔹 `TurnOnLights()`
**Description:** Turns on lights across multiple zones with zero heap allocations.
```csharp
public static void TurnOnLights(this IEnumerable<FacilityZone> zones)
```

### 🔹 `TurnOnLights()`
**Description:** Turns on lights across multiple zones (params overload).
```csharp
public static void TurnOnLights(params FacilityZone[] zones)
```

### 🔹 `FlickerLightsCoroutine()`
**Description:** Performs a flicker animation on zone lights.
```csharp
public static IEnumerator<float> FlickerLightsCoroutine(this FacilityZone zone, Color color, float duration, float frequency)
```

### 🔹 `FlickerLights()`
**Description:** Starts a flicker animation on multiple zones with zero heap allocations.
```csharp
public static void FlickerLights( this IEnumerable<FacilityZone> zones, Color color, float duration, float frequency, string coroutineTag = "LabApi.Extensions-flickerLights")
```

### 🔹 `FlickerLights()`
**Description:** Starts a flicker animation on multiple zones (params overload).
```csharp
public static void FlickerLights( Color color, float duration, float frequency, string @coroutineTag = "LabApi.Extensions-flickerLights", params FacilityZone[] zones) => zones.FlickerLights(color, duration, frequency, coroutineTag);
```

---

## 📦 Class: ExiledCompatibilityLayer

### 🔹 `ExecuteFallback()`
**Description:** Executes the synchronization fallback to bridge directories and load missing configurations safely.
```csharp
public static void ExecuteFallback(Plugin plugin)
```

---

## 📦 Class: EnvironmentEngine

### 🔹 `StartEmergencyStrobe()`
**Description:** Launches a background strobe loop.
```csharp
public static void StartEmergencyStrobe( float totalDuration, float pulseInterval, Color alertColor, string masterTag = "LabApi.Extensions.Environment-strobeLights")
```

### 🔹 `StartZoneFlicker()`
**Description:** Launches a background asynchronous coroutine loop that flickers the illumination grid of a specific zone.
```csharp
public static void StartZoneFlicker( MapGeneration.FacilityZone zone, float duration, float frequency, Color color, string coroutineTag = "LabApi.Extensions.Environment-flickerLights")
```

---

## 📦 Class: EscapeEngine

### 🔹 `RunScenarioRoutine()`
**Description:** Runs a non-blocking asynchronous evacuation routine driven by the provided scenario parameters.
```csharp
public static IEnumerator<float> RunScenarioRoutine(EscapeScenario scenario)
```

---

## 📦 Class: EscapeScenario

### 🔹 `Name`
**Description:** Gets or sets the name or display label of this scenario.
```csharp
public string Name { get; set; } = "Default Evacuation";
```

### 🔹 `EscapeZone`
**Description:** Gets or sets the world position representing the center of the escape area.
```csharp
public Vector3 EscapeZone { get; set; } = Vector3.zero;
```

### 🔹 `EscapeRadius`
**Description:** Gets or sets the escape radius in meters.
```csharp
public float EscapeRadius { get; set; } = 5f;
```

### 🔹 `TeleportPosition`
**Description:** Gets or sets the teleport target position after escaping.
```csharp
public Vector3 TeleportPosition { get; set; } = Vector3.zero;
```

### 🔹 `InitialHint`
**Description:** Gets or sets the message broadcast to all players when the scenario starts.
```csharp
public string InitialHint { get; set; }
```

### 🔹 `InitialHintDuration`
**Description:** Gets or sets the duration of the initial broadcast in seconds.
```csharp
public float InitialHintDuration { get; set; } = 6f;
```

### 🔹 `SuccessHint`
**Description:** Gets or sets the message shown to the escaping player upon successful escape.
```csharp
public string SuccessHint { get; set; }
```

### 🔹 `InitialDelay`
**Description:** Gets or sets the delay in seconds before the escape process starts.
```csharp
public float InitialDelay { get; set; } = 0f;
```

### 🔹 `ProcessingDelay`
**Description:** Gets or sets the delay in seconds before checking for escaping players.
```csharp
public float ProcessingDelay { get; set; } = 0f;
```

### 🔹 `FinalDelay`
**Description:** Gets or sets the delay in seconds before changing the player's role to Spectator.
```csharp
public float FinalDelay { get; set; } = 0.5f;
```

### 🔹 `FlashDuration`
**Description:** Gets or sets the screen flash duration in seconds.
```csharp
public float FlashDuration { get; set; } = 1.75f;
```

### 🔹 `BlindDuration`
**Description:** Gets or sets the blindness duration in seconds.
```csharp
public float BlindDuration { get; set; } = 0f;
```

### 🔹 `BlurDuration`
**Description:** Gets or sets the blur duration in seconds.
```csharp
public float BlurDuration { get; set; } = 0f;
```

### 🔹 `DeafenDuration`
**Description:** Gets or sets the deafen duration in seconds.
```csharp
public float DeafenDuration { get; set; } = 3.75f;
```

### 🔹 `OnSequenceStarted`
**Description:** Occurs when the escape scenario starts.
```csharp
public Action OnSequenceStarted { get; set; }
```

### 🔹 `OnSequenceProcessing`
**Description:** Occurs when the standby delay expires.
```csharp
public Action OnSequenceProcessing { get; set; }
```

### 🔹 `OnPlayerValidated`
**Description:** Occurs when a player successfully passes the escape criteria.
```csharp
public Action<Player> OnPlayerValidated { get; set; }
```

### 🔹 `OnSequenceFinalized`
**Description:** Occurs when the escape scenario is completed.
```csharp
public Action OnSequenceFinalized { get; set; }
```

---

## 📦 Class: StringInterpretationExtensions

### 🔹 `InterpretEnum()`
**Description:** Interprets a raw string input and resolves it against an enum layout using multi-stage heuristic cascades.
```csharp
public static InterpretationResult<T> InterpretEnum<T>(this string input) where T : struct, Enum
```

---

## 📦 Class: SafeRandom

### 🔹 `Next()`
**Description:** Generates a thread-safe random integer between min (inclusive) and max (exclusive).
```csharp
public static int Next(int min, int max) => ThreadRandom.Next(min, max);
```

### 🔹 `NextDouble()`
**Description:** Generates a thread-safe random double between 0.0 and 1.0.
```csharp
public static double NextDouble() => ThreadRandom.NextDouble();
```

### 🔹 `Range()`
**Description:** Generates a thread-safe random float between min (inclusive) and max (inclusive).
```csharp
public static float Range(float min, float max) => (float)(ThreadRandom.NextDouble() * (max - min) + min);
```

---

## 📦 Class: SpatialMap

### 🔹 `GetRandomElevator()`
**Description:** Retrieves a randomly selected <see cref="Elevator"/> instance.
```csharp
public static Elevator GetRandomElevator() => Map.GetRandomElevator();
```

### 🔹 `GetRoomAtPosition()`
**Description:** Resolves a 3D coordinate vector to its corresponding <see cref="Room"/> containment bounds.
```csharp
public static Room GetRoomAtPosition(Vector3 position) => position.GetRoom();
```

### 🔹 `GetPlayer()`
**Description:** Resolves a wrapped <see cref="Player"/> instance from a native <see cref="ReferenceHub"/>.
```csharp
public static Player GetPlayer(ReferenceHub referenceHub) => referenceHub.GetPlayer();
```

### 🔹 `GetRagdoll()`
**Description:** Resolves a wrapped <see cref="Ragdoll"/> instance from a native <see cref="BasicRagdoll"/>.
```csharp
public static Ragdoll GetRagdoll(BasicRagdoll ragdoll) => ragdoll.GetRagdoll();
```

---

## 📦 Class: iLogger

### 🔹 `Info()`
**Description:** Logs an informational message, automatically resolving the full dotted call path.
```csharp
public static void Info(string message) => Logger.Info(string.Concat("[", GetDefaultSource(), "] ", message));
```

### 🔹 `Info()`
**Description:** Logs an informational message with a specified custom tag.
```csharp
public static void Info(string source, string message) => Logger.Info(string.Concat("[", source, "] ", message));
```

### 🔹 `Warn()`
**Description:** Logs a warning message, automatically resolving the full dotted call path.
```csharp
public static void Warn(string message) => Logger.Warn(string.Concat("[", GetDefaultSource(), "] ", message));
```

### 🔹 `Warn()`
**Description:** Logs a warning message with a specified custom tag.
```csharp
public static void Warn(string source, string message) => Logger.Warn(string.Concat("[", source, "] ", message));
```

### 🔹 `Error()`
**Description:** Logs an error message, automatically resolving the full dotted call path.
```csharp
public static void Error(string message) => Logger.Error(string.Concat("[", GetDefaultSource(), "] ", message));
```

### 🔹 `Error()`
**Description:** Logs an error message with a specified custom tag.
```csharp
public static void Error(string source, string message) => Logger.Error(string.Concat("[", source, "] ", message));
```

### 🔹 `Debug()`
**Description:** Logs a debug message if debugging is enabled, automatically resolving the full dotted call path.
```csharp
public static void Debug(string message, bool isDebugEnabled)
```

### 🔹 `Debug()`
**Description:** Logs a debug message with a custom tag if debugging is enabled.
```csharp
public static void Debug(string source, string message, bool isDebugEnabled)
```

### 🔹 `Debug()`
**Description:** Logs a debug message using deferred evaluation, automatically resolving the full dotted call path.
```csharp
public static void Debug(Func<string> messageFactory, bool isDebugEnabled)
```

### 🔹 `Debug()`
**Description:** Logs a debug message with a custom tag using deferred evaluation.
```csharp
public static void Debug(string source, Func<string> messageFactory, bool isDebugEnabled)
```

### 🔹 `LocalTrace()`
**Description:** Logs a detailed local trace message. This call is completely stripped out by the compiler in Release builds.
```csharp
public static void LocalTrace(string source, string message)
```

---

## 📦 Class: PluginBuilder

### 🔹 `Create()`
**Description:** Initiates a fluent builder instance utilizing implicit compiler type inference. <typeparam name="TConfig">The primary configuration type inferred by the compiler.</typeparam>
```csharp
public static PluginBuilder<TConfig> Create<TConfig>(Plugin<TConfig> plugin) where TConfig : LabApiConfig, new()
```

### 🔹 `new()`
**Description:** A zero-allocation, lightweight fluent builder designed to streamline sub-configuration loading and module initialization. Implemented as a readonly struct to completely eliminate heap garbage during plugin bootstrap sequences. <typeparam name="TConfig">The primary configuration type conforming to <see cref="LabApiConfig"/>.</typeparam>
```csharp
public readonly struct PluginBuilder<TConfig> where TConfig : LabApiConfig, new()
```

### 🔹 `PluginBuilder()`
**Description:** Initializes a new instance of the <see cref="PluginBuilder{TConfig}"/> struct.
```csharp
public PluginBuilder(Plugin<TConfig> plugin)
```

### 🔹 `BindSubConfig()`
**Description:** Dynamically loads, validates, and binds a sub-configuration file to a plugin field or property. <typeparam name="TSubConfig">The target sub-configuration class type being loaded.</typeparam>
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
**Description:** Gets all currently registered round scenarios.
```csharp
public IEnumerable<RoundScenario> EndingScenarios => _registeredScenarios;
```

### 🔹 `RegisterScenario()`
**Description:** Registers a custom round scenario into the internal registry safely.
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
**Description:** Gracefully terminates the round lifecycle sequence after a deferred temporal delay window with zero heap allocations.
```csharp
public void EndRoundGracefully(float delay = 0f, string coroutineTag = "LabApi.RoundManagement-customRoundEnd")
```

### 🔹 `GetScenario()`
**Description:** Queries the registered scenario registry to resolve a specific strongly-typed scenario with zero heap allocations.
```csharp
public T GetScenario<T>() where T : RoundScenario
```

---

