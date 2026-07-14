# Audio Manager API - Architecture API Registry

## 📦 Class: ActionChainExtensions

---

## 📦 Class: AssemblyExtensions

### 🔹 `FindEmbeddedAsset()`
**Description:** Finds an embedded resource inside the assembly. Looks for: primaryKey + extension, primaryKey with dots replaced by underscores, or any fallback token. Full manifest resource path if found; otherwise <c>null</c>. </returns>
```csharp
public static string FindEmbeddedAsset( this Assembly assembly, string primaryKey, string fileExtension = ".wav", params string[] alternativeTokens)
```

---

## 📦 Class: CassieExtensions

### 🔹 `SanitizeCassieString()`
**Description:** Removes CR/LF and trims whitespace for safe CASSIE usage.
```csharp
public static string SanitizeCassieString(this string rawMessage) => string.IsNullOrWhiteSpace(rawMessage) ? string.Empty : rawMessage.Replace("\r", "").Replace("\n", " ").Trim();
```

### 🔹 `DispatchGlitchyMessage()`
**Description:** Glitchifies and dispatches a message, returning playback duration.
```csharp
public static double DispatchGlitchyMessage(string message, float glitchChance, float jamChance)
```

### 🔹 `DispatchMessage()`
**Description:** Dispatches a standard CASSIE message and returns playback duration.
```csharp
public static double DispatchMessage(string message, CassiePlaybackModifiers modifiers = default)
```

### 🔹 `ProcessAndDispatchMessage()`
**Description:** Dispatches a formatted CASSIE message with optional subtitles and priority.
```csharp
public static void ProcessAndDispatchMessage(string message, string subtitles, bool clear, float priority, bool disableMessages = false, CassiePlaybackModifiers modifiers = default)
```

### 🔹 `DispatchMessage()`
**Description:** Dispatches all messages in the collection.
```csharp
public static void DispatchMessage(this IEnumerable<string> messages, CassiePlaybackModifiers modifiers = default) => messages.ForEach(m => DispatchMessage(m, modifiers));
```

### 🔹 `DispatchMessage()`
**Description:** Dispatches all provided messages (default modifiers).
```csharp
public static void DispatchMessage(params string[] messages) => ((IEnumerable<string>)messages).DispatchMessage(default);
```

### 🔹 `DispatchMessage()`
**Description:** Dispatches all provided messages with custom modifiers.
```csharp
public static void DispatchMessage(CassiePlaybackModifiers modifiers, params string[] messages) => ((IEnumerable<string>)messages).DispatchMessage(modifiers);
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
**Description:** Calculates total duration of messages with per-message pitch modifiers.
```csharp
public static double CalculateTotalMessagesDurations(IDictionary<string, float> messageSpeedDictionary)
```

### 🔹 `CalculateTotalMessagesDurations()`
**Description:** Calculates total duration of all messages in the collection.
```csharp
public static double CalculateTotalMessagesDurations(this IEnumerable<string> messages, CassiePlaybackModifiers modifiers = default)
```

### 🔹 `CalculateTotalMessagesDurations()`
**Description:** Calculates total duration of provided messages (default modifiers).
```csharp
public static double CalculateTotalMessagesDurations(params string[] messages) => ((IEnumerable<string>)messages).CalculateTotalMessagesDurations(default);
```

### 🔹 `CalculateTotalMessagesDurations()`
**Description:** Calculates total duration of provided messages with custom modifiers.
```csharp
public static double CalculateTotalMessagesDurations(CassiePlaybackModifiers modifiers, params string[] messages) => ((IEnumerable<string>)messages).CalculateTotalMessagesDurations(modifiers);
```

---

## 📦 Class: CollectionExtensions

### 🔹 `ForEach()`
**Description:** Runs the action for every item in the collection. Uses a zero‑allocation fast‑path for List<T>.
```csharp
public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
```

### 🔹 `ExecuteThrottled()`
**Description:** Executes the action if the cooldown for the key has elapsed. Updates the timestamp and returns true if executed.
```csharp
public static bool ExecuteThrottled<TKey>(this IDictionary<TKey, DateTime> cooldownMap, TKey key, TimeSpan window, Action throttleAction)
```

### 🔹 `PruneExpired()`
**Description:** Removes all entries whose timestamps are older than the comparison time.
```csharp
public static void PruneExpired<TKey>(this IDictionary<TKey, DateTime> dictionary, DateTime comparisonTime)
```

### 🔹 `IsCooldownActive()`
**Description:** Returns true if the key exists and its cooldown has not yet expired.
```csharp
public static bool IsCooldownActive<TKey>(this IDictionary<TKey, DateTime> cooldownMap, TKey key)
```

### 🔹 `TryAcquireLock()`
**Description:** Returns true if the cooldown has elapsed and commits a new expiration timestamp.
```csharp
public static bool TryAcquireLock<TKey>(this IDictionary<TKey, DateTime> cooldownMap, TKey key, TimeSpan lockWindow)
```

---

## 📦 Class: CommandExtensions

### 🔹 `ConfirmPermission()`
**Description:** Returns true if the sender has the required permission. Server console always passes.
```csharp
public static bool ConfirmPermission(this ICommandSender sender, PlayerPermissions permission, out string errorResponse)
```

### 🔹 `TryGetFloat()`
**Description:** Tries to read a float from the argument list.
```csharp
public static bool TryGetFloat(this ArraySegment<string> arguments, int index, out float value, float minValue = float.MinValue)
```

### 🔹 `TryGetInt()`
**Description:** Tries to read an int from the argument list.
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
**Description:** Returns true if the door is open.
```csharp
public static bool IsOpen(this Door door) => door != null && door.IsOpened;
```

### 🔹 `WhereNameIn()`
**Description:** Returns only doors whose DoorName matches any of the provided names.
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
**Description:** Toggles the door open/closed state.
```csharp
public static void Toggle(this Door door)
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
public static void OpenAndLock(this Door door, DoorLockReason reason, bool playSound = true)
```

### 🔹 `Open()`
**Description:** Opens all doors.
```csharp
public static void Open(this IEnumerable<Door> doors, bool bypassLocks = false) => doors.ForEach(d => d?.Open(bypassLocks));
```

### 🔹 `Open()`
**Description:** Opens all provided doors.
```csharp
public static void Open(bool bypassLocks, params Door[] doors) => ((IEnumerable<Door>)doors).Open(bypassLocks);
```

### 🔹 `Close()`
**Description:** Closes all doors.
```csharp
public static void Close(this IEnumerable<Door> doors, bool bypassLocks = false) => doors.ForEach(d => d?.Close(bypassLocks));
```

### 🔹 `Close()`
**Description:** Closes all provided doors.
```csharp
public static void Close(bool bypassLocks, params Door[] doors) => ((IEnumerable<Door>)doors).Close(bypassLocks);
```

### 🔹 `SetLockState()`
**Description:** Sets lock state for all doors.
```csharp
public static void SetLockState(this IEnumerable<Door> doors, DoorLockReason reason, bool locked = true) => doors.ForEach(d => d?.SetLockState(reason, locked));
```

### 🔹 `SetLockState()`
**Description:** Sets lock state for all provided doors.
```csharp
public static void SetLockState(DoorLockReason reason, bool locked, params Door[] doors) => ((IEnumerable<Door>)doors).SetLockState(reason, locked);
```

### 🔹 `SetOpenState()`
**Description:** Sets open/closed state for all doors.
```csharp
public static void SetOpenState(this IEnumerable<Door> doors, bool opened, bool bypassLocks = false) => doors.ForEach(d => d?.SetOpenState(opened, bypassLocks));
```

### 🔹 `SetOpenState()`
**Description:** Sets open/closed state for all provided doors.
```csharp
public static void SetOpenState(bool opened, bool bypassLocks, params Door[] doors) => ((IEnumerable<Door>)doors).SetOpenState(opened, bypassLocks);
```

### 🔹 `OpenAndLock()`
**Description:** Opens all doors and applies a lock reason.
```csharp
public static void OpenAndLock(this IEnumerable<Door> doors, DoorLockReason reason, bool playSound = true) => doors.ForEach(d => d?.OpenAndLock(reason, playSound));
```

### 🔹 `OpenAndLock()`
**Description:** Opens all provided doors and applies a lock reason.
```csharp
public static void OpenAndLock(DoorLockReason reason, bool playSound, params Door[] doors) => ((IEnumerable<Door>)doors).OpenAndLock(reason, playSound);
```

### 🔹 `GetElevator()`
**Description:** Returns the elevator associated with this door.
```csharp
public static Elevator GetElevator(this Door door)
```

### 🔹 `IsElevatorDoor()`
**Description:** Returns true if the door is an elevator door.
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

### 🔹 `EnableEffect()`
**Description:** Enables a status effect for all players in the collection.
```csharp
public static void EnableEffect(this IEnumerable<Player> players, FacilityEffectType effect, byte intensity = 1, float duration = 0f) => players.ForEach(p => p?.EnableEffect(effect, intensity, duration));
```

### 🔹 `EnableEffect()`
**Description:** Enables a status effect for all provided players (default intensity and duration).
```csharp
public static void EnableEffect(FacilityEffectType effect, params Player[] players) => ((IEnumerable<Player>)players).EnableEffect(effect);
```

### 🔹 `EnableEffect()`
**Description:** Enables a status effect for all provided players with custom intensity and duration.
```csharp
public static void EnableEffect(FacilityEffectType effect, byte intensity, float duration, params Player[] players) => ((IEnumerable<Player>)players).EnableEffect(effect, intensity, duration);
```

---

## 📦 Class: ElevatorExtensions

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
public static void OpenActiveDoors(this IEnumerable<Elevator> elevators, bool bypassLocks = false) => elevators.ForEach(e => e?.OpenActiveDoors(bypassLocks));
```

### 🔹 `OpenActiveDoors()`
**Description:** Opens active-floor doors for multiple elevators (params overload).
```csharp
public static void OpenActiveDoors(bool bypassLocks, params Elevator[] elevators) => ((IEnumerable<Elevator>)elevators).OpenActiveDoors(bypassLocks);
```

### 🔹 `CloseActiveDoors()`
**Description:** Closes active-floor doors for multiple elevators.
```csharp
public static void CloseActiveDoors(this IEnumerable<Elevator> elevators, bool bypassLocks = false) => elevators.ForEach(e => e?.CloseActiveDoors(bypassLocks));
```

### 🔹 `CloseActiveDoors()`
**Description:** Closes active-floor doors for multiple elevators (params overload).
```csharp
public static void CloseActiveDoors(bool bypassLocks, params Elevator[] elevators) => ((IEnumerable<Elevator>)elevators).CloseActiveDoors(bypassLocks);
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
**Description:** Turns off lights for multiple elevators.
```csharp
public static void TurnOffLights(this IEnumerable<Elevator> elevators, float duration)
```

### 🔹 `TurnOffLights()`
**Description:** Turns off lights for multiple elevators (params overload).
```csharp
public static void TurnOffLights(float duration, params Elevator[] elevators) => ((IEnumerable<Elevator>)elevators).TurnOffLights(duration);
```

### 🔹 `TurnOnLights()`
**Description:** Turns on lights for multiple elevators.
```csharp
public static void TurnOnLights(this IEnumerable<Elevator> elevators)
```

### 🔹 `TurnOnLights()`
**Description:** Turns on lights for multiple elevators (params overload).
```csharp
public static void TurnOnLights(params Elevator[] elevators) => ((IEnumerable<Elevator>)elevators).TurnOnLights();
```

---

## 📦 Class: EnumExtensions

### 🔹 `ToAudioKey()`
**Description:** Returns the enum value as a lowercase string.
```csharp
public static string ToAudioKey(this Enum value) => value?.ToString().ToLowerInvariant() ?? string.Empty;
```

### 🔹 `ParseOrDefault()`
**Description:** Parses the string into an enum value or returns the fallback.
```csharp
public static T ParseOrDefault<T>(this string value, T defaultValue = default, bool ignoreCase = true) where T : struct, Enum
```

### 🔹 `GetRandomValue()`
**Description:** Returns a random value from the enum.
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
**Description:** Returns true if the firearm has all specified attachments enabled.
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
**Description:** Registers multiple event handlers.
```csharp
public static void RegisterAll(this IEnumerable<CustomEventsHandler> handlers) => handlers.ForEach(h => CustomHandlersManager.RegisterEventsHandler(h));
```

### 🔹 `RegisterAll()`
**Description:** Registers multiple event handlers (params overload).
```csharp
public static void RegisterAll(params CustomEventsHandler[] handlers) => ((IEnumerable<CustomEventsHandler>)handlers).RegisterAll();
```

### 🔹 `UnregisterAll()`
**Description:** Unregisters multiple event handlers.
```csharp
public static void UnregisterAll(this IEnumerable<CustomEventsHandler> handlers) => handlers.ForEach(h => CustomHandlersManager.UnregisterEventsHandler(h));
```

### 🔹 `UnregisterAll()`
**Description:** Unregisters multiple event handlers (params overload).
```csharp
public static void UnregisterAll(params CustomEventsHandler[] handlers) => ((IEnumerable<CustomEventsHandler>)handlers).UnregisterAll();
```

---

## 📦 Class: IoExtensions

### 🔹 `IsReparsePoint()`
**Description:** Returns true if the path exists and is a reparse point (symlink, junction, etc.).
```csharp
public static bool IsReparsePoint(this string path)
```

### 🔹 `CopyFilesTo()`
**Description:** Copies files from one directory to another using a search pattern. Creates the destination directory if needed.
```csharp
public static void CopyFilesTo( this string sourceDirectory, string destinationDirectory, string searchPattern = "*.*", bool overwrite = true)
```

---

## 📦 Class: MapExtensions

### 🔹 `BreakAllFacilityDoors()`
**Description:** Breaks all breakable doors in the entire facility.
```csharp
public static void BreakAllFacilityDoors() => Room.List?.ForEach(r => r?.BreakAllDoors());
```

### 🔹 `SetAllLightsEnabled()`
**Description:** Enables or disables all lights in the facility.
```csharp
public static void SetAllLightsEnabled(bool enabled) => Room.List?.ForEach(room => room?.AllLightControllers?.ForEach(c => c.LightsEnabled = enabled));
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
public static float Clamp(this float value, float min, float max) => Mathf.Clamp(value, min, max);
```

### 🔹 `Clamp()`
**Description:** Clamps the value between the given minimum and maximum.
```csharp
public static int Clamp(this int value, int min, int max) => Mathf.Clamp(value, min, max);
```

### 🔹 `Clamp()`
**Description:** Clamps the value between the given minimum and maximum.
```csharp
public static double Clamp(this double value, double min, double max) => value < min ? min : (value > max ? max : value);
```

### 🔹 `LimitMin()`
**Description:** Ensures the value is not lower than the given minimum.
```csharp
public static float LimitMin(this float value, float minBaseline) => Mathf.Max(minBaseline, value);
```

### 🔹 `LimitMin()`
**Description:** Ensures the value is not lower than the given minimum.
```csharp
public static int LimitMin(this int value, int minBaseline) => Mathf.Max(minBaseline, value);
```

### 🔹 `LimitMin()`
**Description:** Ensures the value is not lower than the given minimum.
```csharp
public static double LimitMin(this double value, double minBaseline) => value < minBaseline ? minBaseline : value;
```

### 🔹 `LimitMax()`
**Description:** Ensures the value is not higher than the given maximum.
```csharp
public static float LimitMax(this float value, float maxBaseline) => Mathf.Min(maxBaseline, value);
```

### 🔹 `LimitMax()`
**Description:** Ensures the value is not higher than the given maximum.
```csharp
public static int LimitMax(this int value, int maxBaseline) => Mathf.Min(maxBaseline, value);
```

### 🔹 `LimitMax()`
**Description:** Ensures the value is not higher than the given maximum.
```csharp
public static double LimitMax(this double value, double maxBaseline) => value > maxBaseline ? maxBaseline : value;
```

### 🔹 `Abs()`
**Description:** Returns the absolute value.
```csharp
public static int Abs(this int value) => Math.Abs(value);
```

### 🔹 `Abs()`
**Description:** Returns the absolute value.
```csharp
public static float Abs(this float value) => Mathf.Abs(value);
```

### 🔹 `Abs()`
**Description:** Returns the absolute value.
```csharp
public static double Abs(this double value) => Math.Abs(value);
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
**Description:** Linearly interpolates between two values.
```csharp
public static float Lerp(this float from, float to, float t) => Mathf.Lerp(from, to, t);
```

### 🔹 `LerpUnclamped()`
**Description:** Linearly interpolates without clamping the alpha.
```csharp
public static float LerpUnclamped(this float from, float to, float t) => Mathf.LerpUnclamped(from, to, t);
```

### 🔹 `Floor()`
**Description:** Floors the value.
```csharp
public static float Floor(this float value) => Mathf.Floor(value);
```

### 🔹 `Floor()`
**Description:** Floors the value.
```csharp
public static double Floor(this double value) => Math.Floor(value);
```

### 🔹 `Ceil()`
**Description:** Ceils the value.
```csharp
public static float Ceil(this float value) => Mathf.Ceil(value);
```

### 🔹 `Ceil()`
**Description:** Ceils the value.
```csharp
public static double Ceil(this double value) => Math.Ceiling(value);
```

### 🔹 `DbToLinear()`
**Description:** Converts decibels to a linear 0–1 value.
```csharp
public static float DbToLinear(this float db) => db <= -96f ? 0f : Mathf.Pow(10f, db / 20f);
```

### 🔹 `LinearToDb()`
**Description:** Converts a linear 0–1 value to decibels.
```csharp
public static float LinearToDb(this float linear) => linear <= 0.00001f ? -96f : 20f * Mathf.Log10(linear);
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
public static int RoundToInt(this float value) => Mathf.RoundToInt(value);
```

### 🔹 `RoundToInt()`
**Description:** Rounds the value to the nearest integer.
```csharp
public static int RoundToInt(this double value) => (int)Math.Round(value);
```

---

## 📦 Class: PickupExtensions

### 🔹 `ApplyKineticBlast()`
**Description:** Applies a physics impulse to a pickup.
```csharp
public static void ApplyKineticBlast(this Pickup pickup, float linearVelocityMagnitude, float angularVelocityMagnitude)
```

### 🔹 `ApplyKineticBlast()`
**Description:** Applies a physics impulse to multiple pickups.
```csharp
public static void ApplyKineticBlast(this IEnumerable<Pickup> pickups, float linearVelocityMagnitude, float angularVelocityMagnitude) => pickups.ForEach(p => p?.ApplyKineticBlast(linearVelocityMagnitude, angularVelocityMagnitude));
```

### 🔹 `ApplyKineticBlast()`
**Description:** Applies a physics impulse to multiple pickups (params overload).
```csharp
public static void ApplyKineticBlast(float linearVelocityMagnitude, float angularVelocityMagnitude, params Pickup[] pickups) => ((IEnumerable<Pickup>)pickups).ApplyKineticBlast(linearVelocityMagnitude, angularVelocityMagnitude);
```

---

## 📦 Class: PlayerExtensions

### 🔹 `AttachTrackingObject()`
**Description:** Attaches a follower object to the player. The follower tracks the player's transform with an optional offset.
```csharp
public static void AttachTrackingObject(this Player player, GameObject followerObject, Vector3 offset = default)
```

### 🔹 `AttachTrackingObject()`
**Description:** Attaches a follower object to multiple players.
```csharp
public static void AttachTrackingObject(this IEnumerable<Player> players, GameObject followerObject, Vector3 offset = default) => players?.ForEach(p => p?.AttachTrackingObject(followerObject, offset));
```

### 🔹 `AttachTrackingObject()`
**Description:** Attaches a follower object to multiple players (params overload).
```csharp
public static void AttachTrackingObject(GameObject followerObject, Vector3 offset, params Player[] players) => ((IEnumerable<Player>)players).AttachTrackingObject(followerObject, offset);
```

### 🔹 `BroadcastHintToAll()`
**Description:** Sends a hint to all ready players.
```csharp
public static void BroadcastHintToAll(string hintContent, float duration = 5f)
```

### 🔹 `BroadcastHint()`
**Description:** Sends a hint to multiple players. Only ready players receive the hint.
```csharp
public static void BroadcastHint(this IEnumerable<Player> players, string hintContent, float duration = 5f) => players?.ForEach(p => { if (p?.IsReady == true) p.SendHint(hintContent, duration);
```

### 🔹 `BroadcastHint()`
**Description:** Sends a hint to multiple players (params overload).
```csharp
public static void BroadcastHint(string hintContent, float duration, params Player[] players) => ((IEnumerable<Player>)players).BroadcastHint(hintContent, duration);
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
**Description:** Sets the emission state of held light sources for multiple players.
```csharp
public static void SetHeldLightSourceState(this IEnumerable<Player> players, bool emit) => players?.ForEach(p => p?.SetHeldLightSourceState(emit));
```

### 🔹 `SetHeldLightSourceState()`
**Description:** Sets the emission state of held light sources (params overload).
```csharp
public static void SetHeldLightSourceState(bool emit, params Player[] players) => ((IEnumerable<Player>)players).SetHeldLightSourceState(emit);
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
**Description:** Returns true if the player is within the given radius of any provided positions. Uses squared magnitude for performance.
```csharp
public static bool IsWithinAnyRadius(this Player player, IEnumerable<Vector3> positions, float radiusSize)
```

### 🔹 `IsInRoom()`
**Description:** Returns true if the player is currently inside any of the given rooms.
```csharp
public static bool IsInRoom(this Player player, params RoomName[] roomNames)
```

### 🔹 `IsInDarkRoom()`
**Description:** Returns true if the player is in a room with lights turned off.
```csharp
public static bool IsInDarkRoom(this Player player)
```

### 🔹 `IsInTrueDarkness()`
**Description:** Returns true if the player is in total darkness (dark room or dark elevator cabin).
```csharp
public static bool IsInTrueDarkness(this Player player)
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
**Description:** Applies sensory shock to multiple players.
```csharp
public static void ApplySensoryShock( this IEnumerable<Player> players, float flashDuration = 0f, float blindDuration = 0f, float blurDuration = 0f, float deafenDuration = 0f) => players?.ForEach(p => p?.ApplySensoryShock(flashDuration, blindDuration, blurDuration, deafenDuration));
```

### 🔹 `ApplySensoryShock()`
**Description:** Applies sensory shock to multiple players (params overload).
```csharp
public static void ApplySensoryShock( float flashDuration, float blindDuration, float blurDuration, float deafenDuration, params Player[] players) => ((IEnumerable<Player>)players).ApplySensoryShock(flashDuration, blindDuration, blurDuration, deafenDuration);
```

### 🔹 `FlickerHeldLightSourceCoroutine()`
**Description:** Coroutine that flickers the player's held light source.
```csharp
public static IEnumerator<float> FlickerHeldLightSourceCoroutine( this Player player, int flickerCount, float delayPerFlicker, bool forceOff = false, Action<Player, bool> onTickFeedback = null)
```

### 🔹 `FlickerHeldLightSource()`
**Description:** Starts the flicker coroutine for multiple players.
```csharp
public static void FlickerHeldLightSource( this IEnumerable<Player> players, int flickerCount, float delayPerFlicker, bool forceOff = false, Action<Player, bool> onTickFeedback = null) => players?.ForEach(p => { if (p != null) Timing.RunCoroutine(p.FlickerHeldLightSourceCoroutine(flickerCount, delayPerFlicker, forceOff, onTickFeedback));
```

### 🔹 `FlickerHeldLightSource()`
**Description:** Starts the flicker coroutine for multiple players (params overload).
```csharp
public static void FlickerHeldLightSource( int flickerCount, float delayPerFlicker, bool forceOff = false, Action<Player, bool> onTickFeedback = null, params Player[] players) => ((IEnumerable<Player>)players).FlickerHeldLightSource(flickerCount, delayPerFlicker, forceOff, onTickFeedback);
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

### 🔹 `TryResolveFuzzy()`
**Description:** Attempts to resolve a player from a fuzzy identifier. Supports exact ID, UserId, exact nickname, substring match, and Levenshtein fallback.
```csharp
public static bool TryResolveFuzzy(this IEnumerable<Player> players, string identifier, out Player target, out string error)
```

---

## 📦 Class: PluginConfigExtensions

### 🔹 `LoadOrCreateSubConfig()`
**Description:** Loads a sub‑config file or creates a new one if missing or invalid. Runs optional validation and saves the result back to disk. <typeparam name="TMainConfig">Main plugin config type.</typeparam> <typeparam name="TSubConfig">Sub‑config type to load.</typeparam>
```csharp
public static TSubConfig LoadOrCreateSubConfig<TMainConfig, TSubConfig>( this Plugin<TMainConfig> plugin, string fileName, Action<TSubConfig> validationAction = null) where TMainConfig : LabApiConfig, new() where TSubConfig : class, new()
```

---

## 📦 Class: RoomExtensions

### 🔹 `GetNeighbors()`
**Description:** Returns all rooms directly connected to this room.
```csharp
public static IEnumerable<Room> GetNeighbors(this Room room)
```

### 🔹 `GetElevatorsConnectedToRoom()`
**Description:** Returns elevators connected to the given room.
```csharp
public static IEnumerable<Elevator> GetElevatorsConnectedToRoom(this Room room)
```

### 🔹 `WhereNotInPocket()`
**Description:** Returns all rooms except the Pocket Dimension.
```csharp
public static IEnumerable<Room> WhereNotInPocket(this IEnumerable<Room> rooms)
```

### 🔹 `IsCheckpoint()`
**Description:** Returns true if the room is a checkpoint.
```csharp
public static bool IsCheckpoint(this RoomName name) => name is RoomName.LczCheckpointA or RoomName.LczCheckpointB or RoomName.HczCheckpointA or RoomName.HczCheckpointB or RoomName.HczCheckpointToEntranceZone;
```

### 🔹 `IsScpRoom()`
**Description:** Returns true if the room is an SCP containment room.
```csharp
public static bool IsScpRoom(this RoomName name) => name is RoomName.Lcz173 or RoomName.Lcz330 or RoomName.Hcz049 or RoomName.Hcz079 or RoomName.Hcz096 or RoomName.Hcz106 or RoomName.Hcz939 or RoomName.Lcz914 or RoomName.HczTestroom;
```

### 🔹 `IsArmory()`
**Description:** Returns true if the room is an armory.
```csharp
public static bool IsArmory(this RoomName name) => name is RoomName.LczArmory or RoomName.HczArmory;
```

### 🔹 `IsFreeOfEngagedGenerators()`
**Description:** Returns true if the room has no engaged generators.
```csharp
public static bool IsFreeOfEngagedGenerators(this Room room)
```

### 🔹 `IsRoomAndNeighborsFreeOfEngagedGenerators()`
**Description:** Returns true if the room and all its neighbors have no engaged generators.
```csharp
public static bool IsRoomAndNeighborsFreeOfEngagedGenerators(this Room room)
```

### 🔹 `IsElevatorActiveInRoom()`
**Description:** Returns true if any elevator connected to the room is currently moving.
```csharp
public static bool IsElevatorActiveInRoom(this Room room)
```

### 🔹 `ExecuteActionOnRoomAndNeighbors()`
**Description:** Executes an action on the room and all its neighbors.
```csharp
public static void ExecuteActionOnRoomAndNeighbors(this Room room, Action<Room> action)
```

### 🔹 `HandleElevatorsForRoom()`
**Description:** Executes an action on all elevators connected to the room. s
```csharp
public static void HandleElevatorsForRoom(this Room room, float affectChance, Action<Elevator> action)
```

### 🔹 `BreakAllDoors()`
**Description:** Breaks all breakable doors in the room.
```csharp
public static void BreakAllDoors(this Room room)
```

### 🔹 `TurnOffLights()`
**Description:** Turns off lights in the room for a given duration.
```csharp
public static void TurnOffLights(this Room room, float duration) => room?.AllLightControllers?.ForEach(lc => lc.FlickerLights(duration));
```

### 🔹 `TurnOnLights()`
**Description:** Turns on lights in the room and optionally flickers them.
```csharp
public static void TurnOnLights(this Room room, float flickerDuration = 0f) => room?.AllLightControllers?.ForEach(c => c.FlickerLights(flickerDuration));
```

### 🔹 `TurnOffRoomAndNeighborLights()`
**Description:** Turns off lights in the room and its neighbors.
```csharp
public static void TurnOffRoomAndNeighborLights(this Room room, float duration) => room.ExecuteActionOnRoomAndNeighbors(r => r.TurnOffLights(duration));
```

### 🔹 `TurnOnRoomAndNeighborLights()`
**Description:** Turns on lights in the room and its neighbors.
```csharp
public static void TurnOnRoomAndNeighborLights(this Room room, float duration = 0f) => room.ExecuteActionOnRoomAndNeighbors(r => r.TurnOnLights(duration));
```

### 🔹 `SetLightsColor()`
**Description:** Sets the light color for the room.
```csharp
public static void SetLightsColor(this Room room, Color color)
```

### 🔹 `SetLightsColor()`
**Description:** Sets light color for multiple rooms.
```csharp
public static void SetLightsColor(this IEnumerable<Room> rooms, Color color) => rooms.ForEach(r => r?.SetLightsColor(color));
```

### 🔹 `SetLightsColor()`
**Description:** Sets light color for multiple rooms (params).
```csharp
public static void SetLightsColor(Color color, params Room[] rooms) => ((IEnumerable<Room>)rooms).SetLightsColor(color);
```

### 🔹 `TurnOffLights()`
**Description:** Turns off lights for multiple rooms.
```csharp
public static void TurnOffLights(this IEnumerable<Room> rooms, float duration) => rooms.ForEach(r => r?.TurnOffLights(duration));
```

### 🔹 `TurnOffLights()`
**Description:** Turns off lights for multiple rooms (params).
```csharp
public static void TurnOffLights(float duration, params Room[] rooms) => ((IEnumerable<Room>)rooms).TurnOffLights(duration);
```

### 🔹 `TurnOnLights()`
**Description:** Turns on lights for multiple rooms.
```csharp
public static void TurnOnLights(this IEnumerable<Room> rooms, float flickerDuration = 0f) => rooms.ForEach(r => r?.TurnOnLights(flickerDuration));
```

### 🔹 `TurnOnLights()`
**Description:** Turns on lights for multiple rooms (params).
```csharp
public static void TurnOnLights(float flickerDuration, params Room[] rooms) => ((IEnumerable<Room>)rooms).TurnOnLights(flickerDuration);
```

### 🔹 `BreakAllDoors()`
**Description:** Breaks all breakable doors in multiple rooms.
```csharp
public static void BreakAllDoors(this IEnumerable<Room> rooms) => rooms.ForEach(r => r?.BreakAllDoors());
```

### 🔹 `BreakAllDoors()`
**Description:** Breaks all breakable doors in multiple rooms (params).
```csharp
public static void BreakAllDoors(params Room[] rooms) => ((IEnumerable<Room>)rooms).BreakAllDoors();
```

### 🔹 `GetRoom()`
**Description:** Returns the room at the given world position.
```csharp
public static Room GetRoom(this Vector3 position) => Room.GetRoomAtPosition(position);
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

### 🔹 `FlickerLightsCoroutine()`
**Description:** Executes a flicker animation on the room lights.
```csharp
public static IEnumerator<float> FlickerLightsCoroutine(this Room room, Color color, float duration, float frequency)
```

---

## 📦 Class: StringExtensions

### 🔹 `NormalizeUserId()`
**Description:** Returns the user ID in lowercase, or an empty string if null.
```csharp
public static string NormalizeUserId(this string userId) => userId?.ToLowerInvariant() ?? string.Empty;
```

### 🔹 `ComputeLevenshteinDistance()`
**Description:** Returns the Levenshtein distance between two strings.
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
**Description:** Disables multiple Tesla gates for a given duration.
```csharp
public static void DisableFor(this IEnumerable<Tesla> teslas, float duration, bool forceTrigger = true) => teslas.ForEach(t => t?.DisableFor(duration, forceTrigger));
```

### 🔹 `DisableFor()`
**Description:** Disables multiple Tesla gates (params overload).
```csharp
public static void DisableFor(float duration, bool forceTrigger, params Tesla[] teslas) => ((IEnumerable<Tesla>)teslas).DisableFor(duration, forceTrigger);
```

---

## 📦 Class: TimingExtensions

### 🔹 `CallDelayedIf()`
**Description:** Calls <paramref name="action"/> after a delay if <paramref name="condition"/> returns true.
```csharp
public static CoroutineHandle CallDelayedIf(float delay, Func<bool> condition, Action action, string coroutineTag = null)
```

### 🔹 `Kill()`
**Description:** Kills all coroutines bound to this tag.
```csharp
public static void Kill(this string tag)
```

### 🔹 `Kill()`
**Description:** Kills this coroutine handle if it is running.
```csharp
public static void Kill(this CoroutineHandle handle)
```

### 🔹 `Kill()`
**Description:** Kills all coroutines bound to the given tags.
```csharp
public static void Kill(this IEnumerable<string> tags) => tags.ForEach(t => t?.Kill());
```

### 🔹 `Kill()`
**Description:** Kills all coroutines bound to the given tags (params overload).
```csharp
public static void Kill(params string[] tags) => ((IEnumerable<string>)tags).Kill();
```

### 🔹 `KillAll()`
**Description:** Kills all coroutines associated with the given handles.
```csharp
public static void KillAll(this IEnumerable<CoroutineHandle> handles) => handles.ForEach(h => h.Kill());
```

### 🔹 `KillAll()`
**Description:** Kills all coroutines associated with the given handles (params overload).
```csharp
public static void KillAll(params CoroutineHandle[] handles) => ((IEnumerable<CoroutineHandle>)handles).KillAll();
```

### 🔹 `KillAllAndClear()`
**Description:** Kills all coroutines in the list and clears it.
```csharp
public static void KillAllAndClear(this List<CoroutineHandle> handles)
```

---

## 📦 Class: VectorExtensions

### 🔹 `GetDistanceTo()`
**Description:** Returns the distance between two points.
```csharp
public static float GetDistanceTo(this Vector3 origin, Vector3 target) => Vector3.Distance(origin, target);
```

### 🔹 `GetDistanceSquaredTo()`
**Description:** Returns the squared distance between two points (no square root).
```csharp
public static float GetDistanceSquaredTo(this Vector3 origin, Vector3 target) => Vector3.SqrMagnitude(origin - target);
```

### 🔹 `GetUpwardReflectedVector()`
**Description:** Reflects the vector upward if it points too far downward.
```csharp
public static Vector3 GetUpwardReflectedVector(this Vector3 direction, float dotThreshold = 0.707f)
```

### 🔹 `GetRandomUpwardSphereVelocity()`
**Description:** Returns a random upward-facing direction scaled by the given magnitude.
```csharp
public static Vector3 GetRandomUpwardSphereVelocity(float magnitude = 1f)
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

### 🔹 `GetDoors()`
**Description:** Returns all doors located inside the zone.
```csharp
public static IEnumerable<Door> GetDoors(this FacilityZone zone)
```

### 🔹 `SetDoorsLockState()`
**Description:** Sets the lock state of all doors in the zone. Use <paramref name="locked"/> = false to unlock them.
```csharp
public static void SetDoorsLockState(this FacilityZone zone, DoorLockReason reason, bool locked = true) => zone.GetDoors().SetLockState(reason, locked);
```

### 🔹 `GetElevators()`
**Description:** Returns all elevators whose destination rooms belong to the given zone.
```csharp
public static IEnumerable<Elevator> GetElevators(this FacilityZone zone)
```

### 🔹 `TurnOffLights()`
**Description:** Turns off lights in the zone and its elevators.
```csharp
public static void TurnOffLights(this FacilityZone zone, float duration)
```

### 🔹 `TurnOnLights()`
**Description:** Turns on lights in the zone and its elevators.
```csharp
public static void TurnOnLights(this FacilityZone zone)
```

### 🔹 `TurnOffLights()`
**Description:** Turns off lights across multiple zones.
```csharp
public static void TurnOffLights(this IEnumerable<FacilityZone> zones, float duration) => zones.ForEach(z => z.TurnOffLights(duration));
```

### 🔹 `TurnOffLights()`
**Description:** Turns off lights across multiple zones (params overload).
```csharp
public static void TurnOffLights(float duration, params FacilityZone[] zones) => ((IEnumerable<FacilityZone>)zones).TurnOffLights(duration);
```

### 🔹 `TurnOnLights()`
**Description:** Turns on lights across multiple zones.
```csharp
public static void TurnOnLights(this IEnumerable<FacilityZone> zones) => zones.ForEach(z => z.TurnOnLights());
```

### 🔹 `TurnOnLights()`
**Description:** Turns on lights across multiple zones (params overload).
```csharp
public static void TurnOnLights(params FacilityZone[] zones) => ((IEnumerable<FacilityZone>)zones).TurnOnLights();
```

### 🔹 `FlickerLightsCoroutine()`
**Description:** Performs a flicker animation on zone lights.
```csharp
public static IEnumerator<float> FlickerLightsCoroutine(this FacilityZone zone, Color color, float duration, float frequency)
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

