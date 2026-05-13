# Combat Screen

The combat screen is where the player fights by typing. A challenge (a word
or a letter) is shown on screen, the player types it, and when they get it
right a new challenge appears.

This document is for teammates who need to **change** the combat screen —
add a new kind of challenge, swap how input is read, restyle the on-screen
text, or extend the loop with scoring and win conditions.

## TL;DR

Everything in this folder is wired together by [CombatCoordinator.cs](CombatCoordinator.cs).
It owns three references that you assign in the Unity inspector:

```
┌─────────────────────┐
│  CombatCoordinator  │  ← drives the loop in Update()
└──────────┬──────────┘
           │ references (set in inspector)
           ├──────────────► ChallengeProvider   "what should the player type next?"
           ├──────────────► WordDetector        "what has the player typed so far?"
           └──────────────► TypingDisplay       "show progress on screen"

                              WordDetector also references:
                              └─► TypingDetector  "raw keystrokes from somewhere"
```

The three abstract base classes (`ChallengeProvider`, `TypingDetector`,
`TypingDisplay`) are the **extension points**. To change behaviour, write a
new subclass and drop it on a GameObject in the scene — no changes to
`CombatCoordinator` needed.

## The loop, in plain English

Inside [CombatCoordinator.cs](CombatCoordinator.cs):

1. **Start** — ask the `ChallengeProvider` for a challenge string, hand it
   to the `TypingDisplay` so the player can see it.
2. **Every frame** — ask the `WordDetector` for what the player has typed,
   pass it to the `TypingDisplay` so it can colour letters as correct /
   incorrect / untyped.
3. **When typed string == goal** — fetch the next challenge, reset the
   display, tell the `WordDetector` to clear its buffer.

There is currently no scoring, no health, no win condition. Those 
hooks will need to be added to `CombatCoordinator` — see *Extending the
loop* below.

## The components

### `CombatCoordinator` ([CombatCoordinator.cs](CombatCoordinator.cs))
The only `MonoBehaviour` that contains gameplay logic. Lives on a single
GameObject in the scene. If you need to add scoring, win/lose, combos,
timers — this is where it goes.

### `ChallengeProvider` ([ChallengeProvider.cs](ChallengeProvider.cs)) — abstract
Returns the next string the player must type. One method:
`string getNextChallenge()`.

Concrete implementations:
- [RandomLetterChallengeProvider.cs](RandomLetterChallengeProvider.cs) —
  returns a random character from a configurable alphabet. Good for early
  game / testing.
- [WordListChallengeProvider.cs](WordListChallengeProvider.cs) — cycles
  through a list of words set in the inspector.

### `TypingDetector` ([TypingDetector.cs](TypingDetector.cs)) — abstract
Returns the raw characters typed *since the last call*, then forgets them.
One method: `string get_latest_keys()`. Each call should drain the buffer
so the next call only sees newer keystrokes.

Concrete implementations:
- [KeyboardTypingDetector.cs](KeyboardTypingDetector.cs) — listens to
  `Keyboard.current.onTextInput` from the new Input System.
- [MocTypingDetector.cs](MocTypingDetector.cs) — scripted/test input. Walks
  through a pre-set string one character at a time when `type_next_key()`
  is called from another script. Useful for tests and demos; not intended
  for shipped builds.

### `WordDetector` ([WordDetector.cs](WordDetector.cs)) — concrete
Sits between the `TypingDetector` and the `CombatCoordinator`. Maintains
the *cumulative* string the player has typed for the current challenge,
including backspace handling. `new_word()` clears it when the challenge
is solved.

This one is **not** abstract — backspace + buffering logic is shared, so
there is no reason to swap it out. Subclass only if you genuinely need
different word-assembly rules (e.g. ignoring spaces, normalising case).

### `TypingDisplay` ([TypingDisplay.cs](TypingDisplay.cs)) — abstract
Renders the challenge and the player's progress. Two methods:
- `initializeText(string text)` — called when a new challenge starts.
- `displayProgress(string text)` — called every frame with the player's
  current typed string.

Concrete implementations:
- [TMPTypingDisplay.cs](TMPTypingDisplay.cs) — colours each character of
  the goal string with one of three configurable colours (untyped /
  correctly typed / incorrectly typed) via TextMeshPro rich-text tags.

## Unity setup

Open [Assets/Scenes/CombatScreen.unity](../../Scenes/CombatScreen.unity).
The scene contains one coordinator GameObject with the four scripts
wired up. To change behaviour at runtime, swap which subclass component
is attached and re-assign the inspector field on `CombatCoordinator` /
`WordDetector`.

Quick checklist when wiring a new scene from scratch:
1. Add a GameObject, attach `CombatCoordinator`.
2. Attach **one** `ChallengeProvider` subclass (e.g. `WordListChallengeProvider`)
   and drag it into the `ChallengeProvider` slot.
3. Attach `WordDetector`, drag into the `wordDetector` slot.
4. Attach **one** `TypingDetector` subclass (e.g. `KeyboardTypingDetector`)
   and drag it into `WordDetector`'s `typingDetector` slot.
5. Attach **one** `TypingDisplay` subclass (e.g. `TMPTypingDisplay`), give
   it a `TextMeshPro` reference, drag it into `CombatCoordinator`'s
   `typingDisplay` slot.

## How to extend

### Add a new kind of challenge
Subclass `ChallengeProvider`, implement `getNextChallenge()`, attach to a
GameObject, drag into the coordinator's `ChallengeProvider` field.

```csharp
namespace CombatScreen
{
    class TimedDifficultyChallengeProvider : ChallengeProvider
    {
        [SerializeField] List<string> easy;
        [SerializeField] List<string> hard;

        public override string getNextChallenge()
        {
            var pool = Time.timeSinceLevelLoad > 30 ? hard : easy;
            return pool[Random.Range(0, pool.Count)];
        }
    }
}
```

### Add a new input source
Subclass `TypingDetector`, buffer keystrokes from wherever (gamepad,
network, AI), return-and-clear the buffer in `get_latest_keys()`.
Backspace is `'\b'` or `(char)127` — `WordDetector` handles it.

### Restyle the on-screen text
Subclass `TypingDisplay`. `initializeText` is called once per challenge,
`displayProgress` every frame with the typed-so-far string. Compare it
to the original (you'll need to cache the original yourself, like
`TMPTypingDisplay` does) and render however you like.

### Extending the loop (scoring, health, win)
All loop logic lives in `CombatCoordinator.Update()`. The current
solved-check is `if (typed == goal)` — that is the natural place to:
- award score / damage on a correct challenge,
- compare character-by-character to count mistakes,
- raise events for other systems (UI, audio, enemies) to subscribe to.

Prefer firing a `UnityEvent` or C# event from `CombatCoordinator` over
adding more `[SerializeField]` references — it keeps the coordinator
ignorant of who's listening.

## Known rough edges

- "Challenge" is misspelled throughout (should be "Challenge"). If you
  rename, do it as one commit with a global replace so meta-file GUIDs
  stay attached.
- Method naming is mixed (`getNextChallenge`, `get_latest_keys`,
  `new_word`, `initializeText`). C# convention is `PascalCase` for public
  methods — feel free to normalise when you touch a file.
- The class-level summary on `CombatCoordinator` mentions scoring and a
  win condition that are not implemented yet — that's the next step, not
  existing behaviour.
