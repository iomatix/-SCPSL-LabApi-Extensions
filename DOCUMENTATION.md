# Company.PluginProject - Architecture API Registry

## 📦 Class: `Config`

### 🔹 `Validate()`
**Description:** Validates core plugin settings.
```csharp
public void Validate()
```

---

## 📦 Class: `ExiledCompatibilityLayer`

Advanced cross-platform compatibility engine that bridges legacy EXILED configurations with the LabAPI architecture using native OS reparse points or runtime mirroring fail-safes.

### 🔹 `ExecuteFallback()`
**Description:** Executes the synchronization fallback to bridge directories and load missing configurations.
```csharp
public static void ExecuteFallback(Plugin plugin)
```

---

## 📦 Class: `ExiledCompatibilityLayer`

Advanced cross-platform compatibility engine that bridges legacy EXILED configurations with the LabAPI architecture using native OS reparse points or runtime mirroring fail-safes.

### 🔹 `ExecuteFallback()`
**Description:** Executes the synchronization fallback to bridge directories and load missing configurations.
```csharp
public static void ExecuteFallback(Plugin plugin)
```

---

## 📦 Struct: `ChainNode<T>`

Represents a single step in an action chain: either a callback or a delay.

---

## 📦 Class: `ActionChain<T>`

Builds and runs a sequence of actions with optional delays.

### 🔹 `Then()`
**Description:** Adds an action to run immediately.
```csharp
public ActionChain<T> Then(Action<T> action)
```

### 🔹 `Wait()`
**Description:** Adds a delay before the next action.
```csharp
public ActionChain<T> Wait(float seconds)
```

### 🔹 `Run()`
**Description:** Starts executing the chain as a coroutine.
```csharp
public CoroutineHandle Run(string coroutineTag = null)
```
**Parameters:**
- `coroutineTag`: Optional tag for managing the coroutine.

---

## 📦 Class: `ActionChainExtensions`

Creates a new action chain for the target.

---

## 📦 Class: `AssemblyExtensions`

Helpers for working with embedded resources inside assemblies.

### 🔹 `FindEmbeddedAsset()`
**Description:** Finds an embedded resource inside the assembly. Looks for: primaryKey + extension, primaryKey with dots replaced by underscores, or any fallback token.
```csharp
public static string FindEmbeddedAsset( this Assembly assembly, string primaryKey, string fileExtension = ".wav", params string[] alternativeTokens)
```
**Parameters:**
- `assembly`: Assembly that contains the embedded resources.
- `primaryKey`: Main name to search for (without extension).
- `fileExtension`: File extension (e.g. ".wav" or "wav").
- `alternativeTokens`: Optional fallback names to try if the primary key fails.
**Returns:** Full manifest resource path if found; otherwise, `null`.

---

## 📦 Class: `CassieExtensions`

Highly optimized helper extensions for working with the CASSIE announcement system.

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

## 📦 Class: `CollectionExtensions`

Highly optimized extension methods for working with collections, arrays, and cooldown/throttling maps.

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
**Description:** Executes the action if the cooldown (based on last execution timestamp) has elapsed. Updates the timestamp to current time and returns true if executed.

> **Remarks:** This method uses the "Throttle Model" (stores the last execution time). Do not mix this with absolute lock methods like `IsCooldownActive`.
```csharp
public static bool ExecuteThrottled<TKey>( this IDictionary<TKey, DateTime> cooldownMap, TKey key, TimeSpan window, Action throttleAction)
```

### 🔹 `IsThrottleActive()`
**Description:** Checks if a throttle-based cooldown is currently active for the specified key.

> **Remarks:** Use this method specifically when your map is managed via `ExecuteThrottled`.
```csharp
public static bool IsThrottleActive<TKey>( this IDictionary<TKey, DateTime> cooldownMap, TKey key, TimeSpan window)
```

### 🔹 `IsCooldownActive()`
**Description:** Checks if a lock-based cooldown is active (where the stored value represents the absolute expiry time).

> **Remarks:** Use this method specifically when your map is managed via `TryAcquireLock`.
```csharp
public static bool IsCooldownActive<TKey>(this IDictionary<TKey, DateTime> cooldownMap, TKey key)
```

### 🔹 `TryAcquireLock()`
**Description:** Attempts to acquire a lock for the specified key. If the lock has elapsed or does not exist, commits a new absolute expiration timestamp (current time + lockWindow) and returns true.

> **Remarks:** This method uses the "Lock Model" (stores the absolute expiration time).
```csharp
public static bool TryAcquireLock<TKey>( this IDictionary<TKey, DateTime> cooldownMap, TKey key, TimeSpan lockWindow)
```

### 🔹 `PruneExpired()`
**Description:** Removes all expired entries from the dictionary with zero GC heap allocations. Works seamlessly with both Throttle and Lock models.
```csharp
public static void PruneExpired<TKey>(this IDictionary<TKey, DateTime> dictionary, DateTime comparisonTime)
```

---

## 📦 Class: `CommandExtensions`

Advanced and highly optimized helper extensions for command permission verification, target resolution, and safe multi-format argument parsing.

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

## 📦 Class: `DictionaryExtensions`

Performance-focused dictionary extensions backporting modern .NET features to .NET Framework 4.8.

### 🔹 `Deconstruct()`
**Description:** Backports KeyValuePair deconstruction to .NET Framework 4.8, enabling C# 9.0 pattern matching and tuple-like iteration.
```csharp
public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> kvp, out TKey key, out TValue value)
```

### 🔹 `GetOrAdd()`
**Description:** Gets a value from the dictionary if it exists; otherwise, adds a new value using the provided factory and returns it.
```csharp
public static TValue GetOrAdd<TKey, TValue>( this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueFactory)
```

### 🔹 `GetOrAdd()`
**Description:** Gets a value from the dictionary if it exists; otherwise, adds a new value using a state-passing factory to completely avoid GC closure (lambda display class) allocations.

> **Remarks:** Highly recommended for performance-critical systems inside loops or event handlers.
```csharp
public static TValue GetOrAdd<TKey, TValue, TState>( this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TState, TValue> valueFactory, TState state)
```

### 🔹 `TryRemove()`
**Description:** Backports the modern TryRemove / Remove-with-out functionality to .NET Framework 4.8 dictionaries. Safely attempts to remove a key and outputs the removed value.
```csharp
public static bool TryRemove<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, out TValue value)
```
**Returns:** `true` if the element is successfully found and removed; otherwise, `false`.

---

## 📦 Class: `DoorExtensions`

Highly optimized extensions for working with doors and elevators. Features JIT-cached state-passing batch operations and fast-path Unity component caching.

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
**Description:** Returns only doors whose DoorName matches any of the provided names.

> **Remarks:** This is a query operation and will allocate an iterator state-machine under the hood.
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

## 📦 Enum: `FacilityEffectType`

Specifies the native SCP:Secret Laboratory status effect types.

---

## 📦 Class: `EffectExtensions`

Ultra high-performance extension methods for status effects management. Features compile-time zero-allocation paths and boilerplate-free batch operations using state-passing lambdas.

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

## 📦 Class: `ElevatorExtensions`

Utility extensions for elevator door control and basic cabin state checks. Reuses core Door extensions to maintain a single source of truth and leverage O(1) instance caching.

### 🔹 `GetZone()`
**Description:** Instantly resolves the target destination `FacilityZone` of this elevator sequence.
```csharp
public static FacilityZone GetZone(this Elevator elevator)
```
**Parameters:**
- `elevator`: The active elevator wrapper instance.
**Returns:** The destination `FacilityZone`; otherwise, `FacilityZone.None` if unresolved.

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

## 📦 Class: `ElevatorLightingExtensions`

Utility extensions for controlling elevator lighting states. Features highly optimized zero-allocation state-passing batch operations.

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

## 📦 Class: `EnumExtensions`

Highly optimized extension methods for working with enums. Features compile-time zero-allocation paths and JIT-cached lowercase string representations.

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

## 📦 Class: `FirearmExtensions`

Highly optimized utility extensions for checking firearm attachments. Reuses state-passing query extensions to maintain a zero-allocation, short-circuiting execution path.

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

## 📦 Class: `HandlerExtensions`

Highly optimized utility extensions for registering and unregistering LabAPI event handlers. Features JIT-cached static delegates and zero heap allocations during batch operations.

### 🔹 `Register()`
**Description:** Registers a single event handler with zero heap allocations.
```csharp
public static void Register(this CustomEventsHandler handler)
```

### 🔹 `Unregister()`
**Description:** Unregisters a single event handler with zero heap allocations.
```csharp
public static void Unregister(this CustomEventsHandler handler)
```

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

## 📦 Class: `IoExtensions`

Highly optimized utility extensions for filesystem checks and file operations. Employs lazy enumeration and state-passing loops to guarantee minimal heap allocations.

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

## 📦 Class: `MapExtensions`

Highly optimized utility extensions for global facility operations: doors, lights, and generators. Employs early-exit (short-circuiting) logic and nested state-passing loops to guarantee 0 heap allocations.

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

## 📦 Class: `MathExtensions`

Thread-safe, high-performance math and probability extensions for float, int, and double values. Bypasses UnityEngine.Mathf to guarantee compatibility with background worker threads and maximize JIT inlining.

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

## 📦 Class: `PickupExtensions`

Highly optimized utility extensions for applying physics impulses to world pickups. Employs state-passing loops and strict Unity lifetime checks to guarantee zero heap allocations.

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

## 📦 Class: `PlayerExtensions`

Highly optimized utility extensions for working with `Player`. Features compile-time zero-allocation paths and highly optimized double-dispatch fuzzy resolution.

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

## 📦 Class: `PlayerLightingExtensions`

Highly optimized modular extensions for player lighting states, darkness tracking, and weapon flashlight controls. Features compile-time zero-allocation hot paths, unique item serial verification, and network-safe coroutine cadences.

### 🔹 `PlayerFlickerTagPrefix`
**Description:** Base prefix applied to player-specific light flicker coroutine tracking tags.
```csharp
public const string PlayerFlickerTagPrefix = "LabApi.Extensions-playerFlickerLights";
```

### 🔹 `MinimumFlickerDelay`
**Description:** Absolute minimum safe delay in seconds enforced between successive light flicker pulses to prevent Mirror packet flooding.
```csharp
public const float MinimumFlickerDelay = 0.05f;
```

### 🔹 `HasActiveLightSource()`
**Description:** Determines whether the player's currently held item is an active, emitting light source (e.g., powered flashlight or enabled firearm attachment).
```csharp
public static bool HasActiveLightSource(this Player player)
```
**Parameters:**
- `player`: The target player wrapper instance.
**Returns:** `true` if the held item is emitting light; otherwise, `false`.

### 🔹 `GetHeldLightSourceState()`
**Description:** Retrieves the emission state of the player's currently equipped light source.
```csharp
public static bool GetHeldLightSourceState(this Player player)
```
**Parameters:**
- `player`: The target player wrapper instance.
**Returns:** `true` if the held light emitter is active; otherwise, `false`.

### 🔹 `SetHeldLightSourceState()`
**Description:** Sets the emission state of the player's currently equipped light source (standard flashlight or firearm attachment).
```csharp
public static void SetHeldLightSourceState(this Player player, bool emit)
```
**Parameters:**
- `player`: The target player wrapper instance.
- `emit`: The desired emission state.

### 🔹 `SetHeldLightSourceState()`
**Description:** Sets the emission state of held light sources across multiple players with zero closure heap allocations.
```csharp
public static void SetHeldLightSourceState(this IEnumerable<Player> players, bool emit)
```
**Parameters:**
- `players`: The collection of target players.
- `emit`: The desired emission state.

### 🔹 `SetHeldLightSourceState()`
**Description:** Sets the emission state of held light sources across multiple players supplied via parameter array.
```csharp
public static void SetHeldLightSourceState(bool emit, params Player[] players) => players.SetHeldLightSourceState(emit);
```
**Parameters:**
- `emit`: The desired emission state.
- `players`: The parameter array of target players.

### 🔹 `IsInDarkRoom()`
**Description:** Determines whether the player is currently inside a room where electrical lighting is completely disabled. Fully optimized list loops bypassing struct-boxing.
```csharp
public static bool IsInDarkRoom(this Player player)
```
**Parameters:**
- `player`: The target player wrapper instance.
**Returns:** `true` if the current room has disabled light controllers; otherwise, `false`.

### 🔹 `IsInTrueDarkness()`
**Description:** Determines whether the player is enveloped in total environmental darkness (either inside an unlit room or an unlit elevator cabin).
```csharp
public static bool IsInTrueDarkness(this Player player)
```
**Parameters:**
- `player`: The target player wrapper instance.
**Returns:** `true` if the environmental enclosure is pitch dark; otherwise, `false`.

### 🔹 `FlickerHeldLightSourceCoroutine()`
**Description:** Executes an isolated flicker coroutine on the player's held light source. Tracks the exact physical item instance via its unique serial number to prevent state contamination on weapon swapping.
```csharp
public static IEnumerator<float> FlickerHeldLightSourceCoroutine( this Player player, int flickerCount, float delayPerFlicker, bool forceOff = false, Action<Player, bool> onTickFeedback = null)
```
**Parameters:**
- `player`: The target player instance.
- `flickerCount`: The total number of state toggle intervals.
- `delayPerFlicker`: The delay between state toggles in seconds. Automatically clamped to `MinimumFlickerDelay`.
- `forceOff`: If set to `true`, the light will remain forcibly turned off upon coroutine completion.
- `onTickFeedback`: Optional callback dispatched on each tick, passing the player and whether the final force-off state has been reached.
**Returns:** A coroutine enumerator controlling the flicker progression.

### 🔹 `FlickerHeldLightSource()`
**Description:** Initiates a synchronized flicker sequence on held light sources for multiple players with zero heap allocations. Generates deterministic per-player tracking tags to eliminate coroutine race conditions.
```csharp
public static void FlickerHeldLightSource( this IEnumerable<Player> players, int flickerCount, float delayPerFlicker, bool forceOff = false, Action<Player, bool> onTickFeedback = null)
```
**Parameters:**
- `players`: The collection of target players.
- `flickerCount`: The number of flicker state changes.
- `delayPerFlicker`: Delay in seconds between state toggles.
- `forceOff`: If `true`, the light source is left disabled after completion.
- `onTickFeedback`: Optional per-tick callback invoked with the player and terminal status.

### 🔹 `FlickerHeldLightSource()`
**Description:** Initiates a flicker sequence on held light sources for multiple players supplied via parameter array.
```csharp
public static void FlickerHeldLightSource( int flickerCount, float delayPerFlicker, bool forceOff = false, Action<Player, bool> onTickFeedback = null, params Player[] players) => players.FlickerHeldLightSource(flickerCount, delayPerFlicker, forceOff, onTickFeedback);
```
**Parameters:**
- `flickerCount`: The number of flicker state changes.
- `delayPerFlicker`: Delay in seconds between state toggles.
- `forceOff`: If `true`, the light source is left disabled after completion.
- `onTickFeedback`: Optional per-tick callback invoked with the player and terminal status.
- `players`: The parameter array of target players.

---

## 📦 Class: `PluginConfigExtensions`

Highly optimized utility extensions for loading and validating plugin sub‑config files safely.

### 🔹 `LoadOrCreateSubConfig()`
**Description:** Loads a sub‑config file or creates a new one if missing or invalid. Runs optional validation and saves the result back to disk.
```csharp
public static TSubConfig LoadOrCreateSubConfig<TMainConfig, TSubConfig>( this Plugin<TMainConfig> plugin, string fileName, Action<TSubConfig> validationAction = null) where TMainConfig : LabApiConfig, new() where TSubConfig : class, new()
```
**Type Parameters:**
- `TMainConfig`: Main plugin config type.
- `TSubConfig`: Sub‑config type to load.
**Parameters:**
- `plugin`: Plugin instance.
- `fileName`: File name on disk (e.g. "settings.yml").
- `validationAction`: Optional validation callback.
**Returns:** Loaded or newly created sub‑config instance.

---

## 📦 Class: `RagdollExtensions`

Utility extensions for working with Ragdoll entities.

### 🔹 `GetRagdoll()`
**Description:** Resolves the API-wrapped `Ragdoll` proxy from this native low-level Unity `BasicRagdoll`.
```csharp
public static Ragdoll GetRagdoll(this BasicRagdoll ragdoll)
```

---

## 📦 Class: `ReferenceHubExtensions`

Utility extensions for working with native `ReferenceHub` components.

### 🔹 `GetPlayer()`
**Description:** Resolves the strongly-typed LabAPI `Player` wrapper from this native `ReferenceHub` safely.
```csharp
public static Player GetPlayer(this ReferenceHub referenceHub)
```
**Parameters:**
- `referenceHub`: The native ReferenceHub component instance.
**Returns:** The API-wrapped `Player` proxy; otherwise, `null` if the hub is invalid or destroyed.

---

## 📦 Class: `RoomExtensions`

Highly optimized utility extensions for working with `Room` and `RoomName`. Includes neighbor lookups, generator checks, door operations, and spatial helpers with zero heap allocations.

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

## 📦 Struct: `RoomLightSnapshot`

Represents an immutable, zero-allocation snapshot capturing a room's physical power status and active color override. Used by automated orchestration systems to preserve ambient lighting states across transient facility disruptions.

### 🔹 `OverrideColor`
**Description:** Gets the active ambient override light color at the time the snapshot was taken. Evaluates to `Color.clear` if default room profile lighting was active.
```csharp
public Color OverrideColor { get; }
```

### 🔹 `IsPowered`
**Description:** Gets a value indicating whether the electrical grid feeding the room light controller was active and powered. Evaluates to `false` if lights were disabled by a blackout, EMP, or containment surge.
```csharp
public bool IsPowered { get; }
```

### 🔹 `RoomLightSnapshot()`
**Description:** Initializes a new instance of the `RoomLightSnapshot` struct.
```csharp
public RoomLightSnapshot(Color overrideColor, bool isPowered)
```
**Parameters:**
- `overrideColor`: The captured override color spectrum.
- `isPowered`: Whether the room light infrastructure was actively powered.

### 🔹 `operator`
**Description:** Checks two `RoomLightSnapshot` structures for equality.
```csharp
public static bool operator ==(RoomLightSnapshot left, RoomLightSnapshot right) => left.Equals(right);
```

### 🔹 `UnknownMember`
**Description:** Checks two `RoomLightSnapshot` structures for inequality.
```csharp
public static bool operator !=(RoomLightSnapshot left, RoomLightSnapshot right) => !left.Equals(right);
```

---

## 📦 Class: `RoomLightingExtensions`

Highly optimized extensions for controlling room lighting states, executing server-friendly flicker animations, and inspecting electrical grid telemetry without allocations.

### 🔹 `MinimumFrequency`
**Description:** Minimum allowable frequency in Hertz to avoid zero-division and infinite delay states.
```csharp
public const float MinimumFrequency = 0.1f;
```

### 🔹 `MaximumSafeFrequency`
**Description:** Maximum safe frequency in Hertz. Constrains network SyncVar / RPC dispatch rates to prevent Mirror buffer saturation.
```csharp
public const float MaximumSafeFrequency = 1.2f;
```

### 🔹 `DefaultFlickerTag`
**Description:** Default coroutine tag assigned to room light flicker operations.
```csharp
public const string DefaultFlickerTag = "LabApi.Extensions-flickerLights";
```

### 🔹 `IsLightingPowered()`
**Description:** Verifies whether the lighting infrastructure in the specified room currently has active electrical power.
```csharp
public static bool IsLightingPowered(this Room room)
```
**Parameters:**
- `room`: The target room wrapper instance.
**Returns:** `true` if at least one light controller in the room reports active power feed (`LightsController.LightsEnabled` is `true`); otherwise, `false` (indicating an active blackout from anomalies such as SCP-575, SCP-079, or warhead surges).

### 🔹 `GetOverrideLightsColor()`
**Description:** Retrieves the current ambient override light color assigned to the specified room.
```csharp
public static Color GetOverrideLightsColor(this Room room)
```
**Parameters:**
- `room`: The target room wrapper instance.
**Returns:** The currently active `Color` override, or `Color.clear` if no custom spectrum is applied or no controllers exist.

### 🔹 `CaptureLightSnapshot()`
**Description:** Captures an atomic, zero-heap-allocation snapshot of the room's current electrical and color override state.
```csharp
public static RoomLightSnapshot CaptureLightSnapshot(this Room room)
```
**Parameters:**
- `room`: The target room instance to sample.
**Returns:** A stack-allocated `RoomLightSnapshot` structure representing current lighting parameters.

### 🔹 `RestoreLightSnapshot()`
**Description:** Restores a room's ambient light override color to match a previously sampled `RoomLightSnapshot`. Note: This method purposely preserves existing blackout conditions and does NOT force lights on if the grid remains unpowered.
```csharp
public static void RestoreLightSnapshot(this Room room, RoomLightSnapshot snapshot)
```
**Parameters:**
- `room`: The target room instance to restore.
- `snapshot`: The source snapshot containing the target override color profile.

### 🔹 `RestoreLightSnapshots()`
**Description:** Restores ambient light override colors across a collection of paired rooms and snapshots with zero delegate allocations.
```csharp
public static void RestoreLightSnapshots(this IEnumerable<KeyValuePair<Room, RoomLightSnapshot>> snapshots)
```
**Parameters:**
- `snapshots`: A collection of key-value pairs linking rooms to their respective snapshots.

### 🔹 `TurnOffLights()`
**Description:** Turns off lights in the specified room for a set duration using the native engine flicker pipeline.
```csharp
public static void TurnOffLights(this Room room, float duration)
```
**Parameters:**
- `room`: The target room wrapper instance.
- `duration`: The total duration in seconds for which the lights remain off.

### 🔹 `TurnOnLights()`
**Description:** Turns on lights in the specified room and optionally initiates an introductory native flicker effect.
```csharp
public static void TurnOnLights(this Room room, float flickerDuration = 0f)
```
**Parameters:**
- `room`: The target room wrapper instance.
- `flickerDuration`: The duration in seconds of the introductory native flicker effect. Defaults to 0.

### 🔹 `TurnOffRoomAndNeighborLights()`
**Description:** Turns off lights in the specified room and all directly adjacent neighboring rooms.
```csharp
public static void TurnOffRoomAndNeighborLights(this Room room, float duration) => room.ExecuteActionOnRoomAndNeighbors(duration, static (r, dur) => r.TurnOffLights(dur));
```
**Parameters:**
- `room`: The origin room instance.
- `duration`: The duration in seconds during which lighting remains disabled.

### 🔹 `TurnOnRoomAndNeighborLights()`
**Description:** Turns on lights in the specified room and all directly adjacent neighboring rooms.
```csharp
public static void TurnOnRoomAndNeighborLights(this Room room, float duration = 0f) => room.ExecuteActionOnRoomAndNeighbors(duration, static (r, dur) => r.TurnOnLights(dur));
```
**Parameters:**
- `room`: The origin room instance.
- `duration`: The duration in seconds for the optional startup flicker sequence.

### 🔹 `SetLightsColor()`
**Description:** Sets the ambient override light color for the specified room.
```csharp
public static void SetLightsColor(this Room room, Color color)
```
**Parameters:**
- `room`: The target room instance.
- `color`: The target color spectrum applied to the room lighting controllers.

### 🔹 `SetLightsColor()`
**Description:** Applies an override light color across a collection of rooms with zero closure allocations.
```csharp
public static void SetLightsColor(this IEnumerable<Room> rooms, Color color)
```
**Parameters:**
- `rooms`: The collection of target rooms.
- `color`: The color spectrum to apply.

### 🔹 `SetLightsColor()`
**Description:** Applies an override light color across multiple rooms supplied via parameter array.
```csharp
public static void SetLightsColor(Color color, params Room[] rooms) => rooms.SetLightsColor(color);
```
**Parameters:**
- `color`: The color spectrum to apply.
- `rooms`: The array of rooms to modify.

### 🔹 `TurnOffLights()`
**Description:** Turns off lights across a collection of rooms for a specified duration with zero heap allocations.
```csharp
public static void TurnOffLights(this IEnumerable<Room> rooms, float duration)
```
**Parameters:**
- `rooms`: The collection of target rooms.
- `duration`: The duration in seconds for which lights remain disabled.

### 🔹 `TurnOffLights()`
**Description:** Turns off lights across multiple rooms supplied via parameter array.
```csharp
public static void TurnOffLights(float duration, params Room[] rooms) => rooms.TurnOffLights(duration);
```
**Parameters:**
- `duration`: The duration in seconds for which lights remain disabled.
- `rooms`: The array of rooms to modify.

### 🔹 `TurnOnLights()`
**Description:** Turns on lights across a collection of rooms with zero heap allocations.
```csharp
public static void TurnOnLights(this IEnumerable<Room> rooms, float flickerDuration = 0f)
```
**Parameters:**
- `rooms`: The collection of target rooms.
- `flickerDuration`: Optional startup flicker duration in seconds.

### 🔹 `TurnOnLights()`
**Description:** Turns on lights across multiple rooms supplied via parameter array.
```csharp
public static void TurnOnLights(float flickerDuration, params Room[] rooms) => rooms.TurnOnLights(flickerDuration);
```
**Parameters:**
- `flickerDuration`: Optional startup flicker duration in seconds.
- `rooms`: The array of rooms to modify.

### 🔹 `FlickerLightsCoroutine()`
**Description:** Executes a synchronized native pulse flicker animation across an individual room. Utilizes the native engine light controller pipelines and clamps dispatch frequency to preserve server TPS.
```csharp
public static IEnumerator<float> FlickerLightsCoroutine(this Room room, Color color, float duration, float frequency)
```
**Parameters:**
- `room`: The target room to animate.
- `color`: The ambient override color applied during the flicker sequence.
- `duration`: The total lifetime of the flicker sequence in seconds.
- `frequency`: The desired cadence frequency in Hertz. Automatically clamped between `MinimumFrequency` and `MaximumSafeFrequency`.
**Returns:** A coroutine enumerator managing the temporal pulse progression.

### 🔹 `FlickerLightsBatchCoroutine()`
**Description:** Executes a unified, single-coroutine batch pulse flicker animation across multiple rooms. Enforces native engine light triggers at the specified cadence while shielding the Mirror networking pipeline from packet saturation.
```csharp
public static IEnumerator<float> FlickerLightsBatchCoroutine(this IEnumerable<Room> rooms, Color color, float duration, float frequency)
```
**Parameters:**
- `rooms`: The collection of rooms to animate.
- `color`: The ambient override color active throughout the sequence.
- `duration`: Total animation lifetime in seconds.
- `frequency`: The rhythm frequency in Hertz. Clamped internally to protect network buffers.
**Returns:** A unified coroutine enumerator controlling all target room lights simultaneously.

### 🔹 `FlickerLights()`
**Description:** Initiates a synchronized, low-overhead native flicker animation across multiple rooms using a single centralized coroutine.
```csharp
public static void FlickerLights( this IEnumerable<Room> rooms, Color color, float duration, float frequency, string coroutineTag = DefaultFlickerTag)
```
**Parameters:**
- `rooms`: The collection of target rooms undergoing visual containment disruption.
- `color`: The override color displayed during active flicker intervals.
- `duration`: The operational window of the flicker effect in seconds.
- `frequency`: The pulse cadence in Hertz. Clamped to safe thresholds to guarantee packet stability.
- `coroutineTag`: MEC tracking tag used to manage or cancel active coroutine pipelines.

### 🔹 `FlickerLights()`
**Description:** Initiates a synchronized native flicker animation across multiple rooms supplied via parameter array.
```csharp
public static void FlickerLights( Color color, float duration, float frequency, string coroutineTag = DefaultFlickerTag, params Room[] rooms) => rooms.FlickerLights(color, duration, frequency, coroutineTag);
```
**Parameters:**
- `color`: The override color displayed during active flicker intervals.
- `duration`: The operational window of the flicker effect in seconds.
- `frequency`: The pulse cadence in Hertz.
- `coroutineTag`: MEC tracking tag used to manage or cancel active coroutine pipelines.
- `rooms`: The parameter array of target rooms.

---

## 📦 Class: `StringExtensions`

Highly optimized utility extensions for string manipulation, normalization, and distance metrics. Employs array pooling and allocation-free fast-paths to completely protect the Garbage Collector during fuzzy target lookups.

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

## 📦 Class: `TeslaExtensions`

Highly optimized utility extensions for controlling Tesla gates. Features zero-allocation state-passing batch operations and unified null-safety.

### 🔹 `DisableFor()`
**Description:** Disables a Tesla gate for a given duration. Use `forceTrigger` to trigger a discharge.
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

## 📦 Class: `TimingExtensions`

Highly optimized utility extensions for working with MEC coroutines. Features zero-allocation batch operations and custom lightweight, closure-free delayed runners.

### 🔹 `CallDelayedIf()`
**Description:** Calls `action` after a delay if `condition` returns true.
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

## 📦 Class: `VectorExtensions`

Thread-safe, high-performance utility extensions for Unity Vector3. Bypasses native engine boundary transitions and UnityEngine.Random to maximize speed and thread compatibility.

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

## 📦 Class: `ZoneExtensions`

Highly optimized utility extensions for working with `FacilityZone`: doors, elevator locks and zone registry. Employs direct state-passing loops to bypass iterator allocations entirely.

### 🔹 `All`
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
**Description:** Sets the lock state of all doors in the zone with zero heap allocations. Use `locked` = false to unlock them.
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

## 📦 Class: `ZoneLightingExtensions`

Highly optimized utility extensions for controlling lighting states and executing synchronized animations in `FacilityZone`. Leverages underlying room pipeline extensions and native engine controllers with zero closure garbage.

### 🔹 `TurnOffLights()`
**Description:** Turns off lights across the specified zone and its connected elevator shafts with zero heap allocations.
```csharp
public static void TurnOffLights(this FacilityZone zone, float duration)
```
**Parameters:**
- `zone`: The target facility zone.
- `duration`: The duration in seconds during which lights remain deactivated.

### 🔹 `TurnOnLights()`
**Description:** Restores lights across the specified zone and its connected elevator shafts with zero heap allocations.
```csharp
public static void TurnOnLights(this FacilityZone zone)
```
**Parameters:**
- `zone`: The target facility zone.

### 🔹 `TurnOffLights()`
**Description:** Turns off lights across multiple facility zones with zero heap allocations.
```csharp
public static void TurnOffLights(this IEnumerable<FacilityZone> zones, float duration)
```
**Parameters:**
- `zones`: The collection of target zones.
- `duration`: The duration in seconds for which lights remain deactivated.

### 🔹 `TurnOffLights()`
**Description:** Turns off lights across multiple facility zones supplied via parameter array.
```csharp
public static void TurnOffLights(float duration, params FacilityZone[] zones)
```
**Parameters:**
- `duration`: The duration in seconds for which lights remain deactivated.
- `zones`: The array of target zones.

### 🔹 `TurnOnLights()`
**Description:** Restores lights across multiple facility zones with zero heap allocations.
```csharp
public static void TurnOnLights(this IEnumerable<FacilityZone> zones)
```
**Parameters:**
- `zones`: The collection of target zones.

### 🔹 `TurnOnLights()`
**Description:** Restores lights across multiple facility zones supplied via parameter array.
```csharp
public static void TurnOnLights(params FacilityZone[] zones)
```
**Parameters:**
- `zones`: The array of target zones.

### 🔹 `FlickerLightsCoroutine()`
**Description:** Executes a synchronized native pulse flicker animation across all rooms within a specified zone. Reuses `RoomLightingExtensions.FlickerLightsBatchCoroutine` to enforce DRY and guarantee network safety.
```csharp
public static IEnumerator<float> FlickerLightsCoroutine(this FacilityZone zone, Color color, float duration, float frequency)
```
**Parameters:**
- `zone`: The target facility zone.
- `color`: The override color applied during the animation window.
- `duration`: Total animation lifetime in seconds.
- `frequency`: Cadence frequency in Hertz. Clamped internally to prevent Mirror network buffer overflow.
**Returns:** A coroutine enumerator controlling the zone-wide pulse progression.

### 🔹 `FlickerLights()`
**Description:** Initiates a synchronized, low-overhead native flicker animation across multiple facility zones using a single centralized coroutine.
```csharp
public static void FlickerLights( this IEnumerable<FacilityZone> zones, Color color, float duration, float frequency, string coroutineTag = RoomLightingExtensions.DefaultFlickerTag)
```
**Parameters:**
- `zones`: The collection of target zones undergoing lockdown flicker disruption.
- `color`: The override color applied during the animation sequence.
- `duration`: The duration in seconds for the active animation window.
- `frequency`: The cadence frequency in Hertz. Automatically clamped to network-safe bounds.
- `coroutineTag`: MEC tracking tag used to cancel or manage active animation coroutines.

### 🔹 `FlickerLights()`
**Description:** Initiates a synchronized native flicker animation across multiple facility zones supplied via parameter array.
```csharp
public static void FlickerLights( Color color, float duration, float frequency, string coroutineTag = RoomLightingExtensions.DefaultFlickerTag, params FacilityZone[] zones) => zones.FlickerLights(color, duration, frequency, coroutineTag);
```
**Parameters:**
- `color`: The override color applied during the animation sequence.
- `duration`: The duration in seconds for the active animation window.
- `frequency`: The cadence frequency in Hertz.
- `coroutineTag`: MEC tracking tag used to cancel or manage active animation coroutines.
- `zones`: The parameter array of target zones.

---

## 📦 Class: `ExiledCompatibilityLayer`

Advanced cross-platform compatibility engine that bridges legacy EXILED configurations with the LabAPI architecture using native OS directory junctions or runtime mirroring fallbacks.

### 🔹 `ExecuteFallback()`
**Description:** Executes the synchronization fallback to bridge directories and load missing configurations safely.
```csharp
public static void ExecuteFallback(Plugin plugin)
```

---

## 📦 Class: `RuntimeTransformTracker`

A lightweight Unity MonoBehaviour that smoothly locks its position (and optionally rotation) to a target Transform with a specified offset.

### 🔹 `Setup()`
**Description:** Configures the tracking targets, offset parameters, and custom validity checks safely.
```csharp
public void Setup( Transform target, Vector3 positionOffset = default, Func<bool> validityPredicate = null, // FIX: Restored to 3rd position to guarantee 100% legacy call compatibility! bool trackRotation = false, Quaternion? rotationOffset = null) // FIX: Used nullable type to avoid invalid default(Quaternion) (0,0,0,0) bug.
```
**Parameters:**
- `target`: The target transform to follow.
- `positionOffset`: The position offset relative to the target.
- `validityPredicate`: Optional callback check. If it returns false, the tracker auto-destructs.
- `trackRotation`: If set to true, the tracker will also match the target's rotation.
- `rotationOffset`: The rotation offset relative to the target (defaults to identity).

---

## 📦 Class: `EnvironmentEngine`

Legacy environment manipulation helpers.

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

## 📦 Class: `EscapeEngine`

Utility engine designed to orchestrate and execute custom player escape scenarios safely.

### 🔹 `RunScenarioRoutine()`
**Description:** Runs a non-blocking asynchronous evacuation routine driven by the provided scenario parameters.
```csharp
public static IEnumerator<float> RunScenarioRoutine(EscapeScenario scenario)
```
**Parameters:**
- `scenario`: The escape scenario configuration rules driving this process.
**Returns:** An enumerator tracking the active timeline states.

---

## 📦 Class: `EscapeScenario`

Configuration template representing player escape conditions, teleportations, and visual progression scenarios.

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

## 📦 Enum: `InterpretationStatus`

Outlines the structural resolution status of a fuzzy text interpretation sequence.

---

## 📦 Struct: `InterpretationResult<T>`

Immutable result container holding the execution metrics of a string interpretation sequence. Implemented as a readonly struct to prevent heap allocations during parsing.

### 🔹 `Status`
**Description:** Gets the final resolution status of the execution pipeline.
```csharp
public InterpretationStatus Status { get; }
```

### 🔹 `Value`
**Description:** Gets the verified strongly-typed enum value. Evaluates to default if status is not DefinitiveMatch.
```csharp
public T Value { get; }
```

### 🔹 `Candidates`
**Description:** Gets the list of close structural candidates discovered if the command invocation was ambiguous.
```csharp
public string[] Candidates { get; }
```

---

## 📦 Class: `StringInterpretationExtensions`

High-performance parsing engine delivering fuzzy string resolution, acronym mapping, and typo-tolerance layers. Uses static generic caching to completely eliminate runtime reflection overhead.

### 🔹 `InterpretEnum()`
**Description:** Interprets a raw string input and resolves it against an enum layout using multi-stage heuristic cascades.
```csharp
public static InterpretationResult<T> InterpretEnum<T>(this string input) where T : struct, Enum
```

---

## 📦 Class: `SafeRandom`

Thread-safe utility for generating pseudo-random numbers. Uses ThreadStatic isolation to eliminate concurrency locks across asynchronous coroutines and threads.

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

## 📦 Class: `SpatialMap`

Legacy spatial lookup and entity resolution queries. Kept as a compatibility bridge.

### 🔹 `GetRandomElevator()`
**Description:** Retrieves a randomly selected `Elevator` instance.
```csharp
public static Elevator GetRandomElevator() => Map.GetRandomElevator();
```

### 🔹 `GetRoomAtPosition()`
**Description:** Resolves a 3D coordinate vector to its corresponding `Room` containment bounds.
```csharp
public static Room GetRoomAtPosition(Vector3 position) => position.GetRoom();
```

### 🔹 `GetPlayer()`
**Description:** Resolves a wrapped `Player` instance from a native `ReferenceHub`.
```csharp
public static Player GetPlayer(ReferenceHub referenceHub) => referenceHub.GetPlayer();
```

### 🔹 `GetRagdoll()`
**Description:** Resolves a wrapped `Ragdoll` instance from a native `BasicRagdoll`.
```csharp
public static Ragdoll GetRagdoll(BasicRagdoll ragdoll) => ragdoll.GetRagdoll();
```

---

## 📦 Class: `iLogger`

Centralized logging utility providing fast, structured console outputs with automatic full call-path resolution.

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

## 📦 Struct: `NestingNode<TContext, TLogic>`

A generic structural node designed to bind a logic layer to its parent context.

### 🔹 `Logic`
**Description:** Gets the encapsulated runtime logic instance.
```csharp
public TLogic Logic { get; }
```

### 🔹 `NestingNode()`
**Description:** Initializes a new instance of the `NestingNode` struct safely.
```csharp
public NestingNode(TContext context, Func<TContext, TLogic> logicFactory)
```
**Parameters:**
- `context`: The parent framework context instance to bind.
- `logicFactory`: The factory delegate used to instantiate the logic layer, injecting the context.

---

## 📦 Class: `PluginBuilder`

Static companion factory providing implicit type-inference for the fluent plugin builder.

### 🔹 `Create()`
**Description:** Initiates a fluent builder instance utilizing implicit compiler type inference.
```csharp
public static PluginBuilder<TConfig> Create<TConfig>(Plugin<TConfig> plugin) where TConfig : LabApiConfig, new()
```
**Type Parameters:**
- `TConfig`: The primary configuration type inferred by the compiler.
**Parameters:**
- `plugin`: The live plugin instance.
**Returns:** A zero-allocation fluent `PluginBuilder` struct.

---

## 📦 Struct: `PluginBuilder<TConfig>`

A zero-allocation, lightweight fluent builder designed to streamline sub-configuration loading and module initialization. Implemented as a readonly struct to completely eliminate heap garbage during plugin bootstrap sequences.

### 🔹 `PluginBuilder()`
**Description:** Initializes a new instance of the `PluginBuilder` struct.
```csharp
public PluginBuilder(Plugin<TConfig> plugin)
```
**Parameters:**
- `plugin`: The live plugin instance.

### 🔹 `BindSubConfig()`
**Description:** Dynamically loads, validates, and binds a sub-configuration file to a plugin field or property.
```csharp
public PluginBuilder<TConfig> BindSubConfig<TSubConfig>( string fileName, Action<TSubConfig> bindAction, Action<TSubConfig> validationAction = null) where TSubConfig : class, new()
```
**Type Parameters:**
- `TSubConfig`: The target sub-configuration class type being loaded.
**Parameters:**
- `fileName`: The specific file name on disk (e.g. "settings.yml").
- `bindAction`: The assignment callback mapping the loaded configuration to a plugin property.
- `validationAction`: An optional validation callback executed to enforce custom rules.
**Returns:** The current struct context to support fluent method chaining with zero heap allocations.

### 🔹 `InitializeModule()`
**Description:** Executes an initialization routine or boots up a subsystem component as part of the fluent pipeline.
```csharp
public PluginBuilder<TConfig> InitializeModule(Action initAction)
```
**Parameters:**
- `initAction`: The callback containing the initialization steps.
**Returns:** The current struct context to support fluent method chaining with zero heap allocations.

---

## 📦 Class: `RoundController`

Centralized controller to register, track, and safely execute custom round scenarios while overriding native round ending metrics.

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

## 📦 Class: `RoundScenario`

Abstract template for defining custom server-side round ending sequences, gamemode transitions, or map events.

### 🔹 `OnCleanupRequested`
**Description:** Gets or sets an optional custom lifecycle cleanup hook executed automatically when the scenario completes.
```csharp
public Action OnCleanupRequested { get; set; }
```

### 🔹 `Execute()`
**Description:** Executes the concrete internal scenario logic. Must be overridden by implementation nodes.
```csharp
public abstract void Execute();
```

---
