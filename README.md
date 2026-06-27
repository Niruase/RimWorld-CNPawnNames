# No Name In Game - Chinese Name Replacement Mod

## Overview
This RimWorld mod intercepts English name generation when playing in Chinese and replaces them with randomly generated Western-style names (first name + last name) from separate male/female pools.

## Features
- ✅ Replaces English names with Chinese transliterations of Western-style names
- ✅ Separate first name pools for male and female pawns
- ✅ Shared last name pool
- ✅ No settings required - always active
- ✅ Preserves non-English names unchanged (e.g., already in Chinese characters)
- ✅ Nicknames always blank for cleaner names

## How It Works

### The Problem with Name Generation in RimWorld

RimWorld has **multiple code paths** for generating pawn names:

1. **`TryGetRandomUnusedSolidName`** - Used for randomly generated pawns (colonists, visitors, etc.)
2. **`GeneratePawnName`** - Used for backer pawns and "Name in Game" purchasers (the ones with fixed identities)

Our initial implementation only patched `TryGetRandomUnusedSolidName`, which meant backer names were NOT being replaced. This is why testing showed the mod wasn't working for name-in-game characters.

### The Solution

We now patch **both methods**:
- [PawnBioAndNameGenerator_TryGetRandomUnusedSolidName.cs](file://c:\Users\thewe\_Documents\VS%20Code\NoNameInGame\Source\NoNameInGame\PawnBioAndNameGenerator_TryGetRandomUnusedSolidName.cs) - Random pawns
- [PawnBioAndNameGenerator_GeneratePawnName.cs](file://c:\Users\thewe\_Documents\VS%20Code\NoNameInGame\Source\NoNameInGame\PawnBioAndNameGenerator_GeneratePawnName.cs) - Backer/Name-in-Game pawns

Both patches delegate to [NameReplacementUtility.ProcessNameReplacement()](file://c:\Users\thewe\_Documents\VS%20Code\NoNameInGame\Source\NoNameInGame\NameReplacementUtility.cs#L147-L169) which:
1. Checks if the name contains Latin/ASCII characters (English)
2. If YES → Replaces with Chinese transliterated Western name based on gender
3. If NO → Leaves it unchanged (already in Chinese or other language)

## Technical Implementation

### Architecture (TDD/BDD Approach)

The mod follows Test-Driven Development principles with clear separation of concerns:

#### 1. **NameReplacementUtility.cs** (Core Logic - Testable)
Pure utility class containing all business logic, designed to be easily testable:

- `IsChineseLanguage()` - Detects if current language is Chinese
- `IsEnglishName(string name)` - Determines if a name contains Latin characters
- `GenerateWesternName(Gender gender)` - Generates random Western name based on gender
- `ProcessNameReplacement(NameTriple originalName, Gender gender)` - Main orchestration method

#### 2. **PawnBioAndNameGenerator_TryGetRandomUnusedSolidName.cs** (Harmony Patch)
Thin Harmony patch that delegates to the utility class:
- Uses Postfix pattern to intercept name generation
- Minimal logic in patch itself (delegates to utility)

#### 3. **HarmonyPatcher.cs** (Initialization)
Standard Harmony initialization on mod startup.

### BDD Test Scenarios

The implementation covers these behavior scenarios:

**Scenario 1: Male Pawn with English Name**
```
GIVEN a male pawn is being generated
AND the generated name contains Latin characters (English)
WHEN TryGetRandomUnusedSolidName is called
THEN the name should be replaced with Chinese transliterated Western male first name + Chinese last name
```

**Scenario 2: Female Pawn with English Name**
```
GIVEN a female pawn is being generated
AND the generated name contains Latin characters (English)
WHEN TryGetRandomUnusedSolidName is called
THEN the name should be replaced with Chinese transliterated Western female first name + Chinese last name
```

**Scenario 3: Non-English Names**
```
GIVEN the generated name does NOT contain Latin characters (e.g., already in Chinese)
WHEN the patch processes the result
THEN the name should NOT be modified (pass through unchanged)
```

**Scenario 4: Null Name Handling**
```
GIVEN the original name result is null
WHEN the patch processes the result
THEN it should remain null (no modification)
```

### Name Pools

#### Male First Names (50 Chinese transliterations)
詹姆斯, 约翰, 罗伯特, 迈克尔, 威廉, 大卫, 理查德, 约瑟夫, 托马斯, 查尔斯, 克里斯托弗, 丹尼尔, 马修, 安东尼, 马克, 唐纳德, 史蒂文, 保罗, 安德鲁, 乔舒亚, 肯尼斯, 凯文, 布莱恩, 乔治, 蒂莫西, 罗纳德, 爱德华, 杰森, 杰弗里, 瑞安, 雅各布, 加里, 尼古拉斯, 埃里克, 乔纳森, 斯蒂芬, 拉里, 贾斯汀, 斯科特, 布兰登, 本杰明, 塞缪尔, 雷蒙德, 格雷戈里, 弗兰克, 亚历山大, 帕特里克, 杰克, 丹尼斯, 杰瑞

#### Female First Names (50 Chinese transliterations)
玛丽, 帕特里夏, 詹妮弗, 琳达, 伊丽莎白, 芭芭拉, 苏珊, 杰西卡, 莎拉, 凯伦, 南希, 丽莎, 贝蒂, 玛格丽特, 桑德拉, 阿什利, 金伯利, 艾米丽, 唐娜, 米歇尔, 卡罗尔, 阿曼达, 梅丽莎, 黛博拉, 斯蒂芬妮, 多萝西, 丽贝卡, 莎朗, 劳拉, 辛西娅, 凯瑟琳, 艾米, 安吉拉, 雪莉, 安娜, 布伦达, 帕梅拉, 艾玛, 妮可, 海伦, 萨曼莎, 凯瑟琳, 克里斯汀, 黛布拉, 瑞秋, 卡罗琳, 珍妮特, 凯瑟琳, 玛丽亚, 希瑟

#### Last Names (50 Chinese transliterations)
史密斯, 约翰逊, 威廉姆斯, 布朗, 琼斯, 加西亚, 米勒, 戴维斯, 罗德里格斯, 马丁内斯, 埃尔南德斯, 洛佩兹, 冈萨雷斯, 威尔逊, 安德森, 托马斯, 泰勒, 摩尔, 杰克逊, 马丁, 李, 佩雷斯, 汤普森, 怀特, 哈里斯, 桑切斯, 克拉克, 拉米雷斯, 刘易斯, 罗宾逊, 沃克, 扬, 艾伦, 金, 赖特, 斯科特, 托雷斯, 阮, 希尔, 弗洛雷斯, 格林, 亚当斯, 纳尔逊, 贝克, 霍尔, 里维拉, 坎贝尔, 米切尔, 卡特, 罗伯茨

### Nickname Generation
- Nicknames are always empty (blank) as per requirements

## Code Quality

### Design Principles
1. **Single Responsibility**: Each class has one clear purpose
2. **Separation of Concerns**: Business logic separated from Harmony patches
3. **Testability**: Core logic in pure static methods, easy to unit test
4. **Defensive Programming**: Null checks, try-catch blocks, logging
5. **Logging**: Clear log messages when names are replaced for debugging

### Error Handling
- All name generation wrapped in try-catch
- Errors logged with clear context
- Graceful degradation (returns null on failure rather than crashing)

## Testing Strategy

### Manual Testing Checklist
- [ ] Generate multiple male pawns - verify they get Chinese transliterated Western male names
- [ ] Generate multiple female pawns - verify they get Chinese transliterated Western female names
- [ ] Verify nicknames are always blank
- [ ] Check game logs for replacement messages showing `[NoNameInGame]`
- [ ] Verify no errors in log output
- [ ] Test with different factions to ensure compatibility

### Potential Unit Tests (if testing framework available)
```csharp
// Example test cases that could be implemented
[Test]
void IsChineseLanguage_ReturnsTrue_ForChineseSimplified()
[Test]
void IsChineseLanguage_ReturnsFalse_ForEnglish()
[Test]
void IsEnglishName_ReturnsTrue_ForLatinCharacters()
[Test]
void IsEnglishName_ReturnsFalse_ForChineseCharacters()
[Test]
void GenerateWesternName_Male_ReturnsMaleFirstName()
[Test]
void GenerateWesternName_Female_ReturnsFemaleFirstName()
[Test]
void ProcessNameReplacement_PassThrough_WhenNotChinese()
```

## Compatibility
- Requires Harmony mod
- Compatible with other naming mods (should layer properly)
- Does not affect factions with custom naming schemes (Tribals, Empire)
- Works with all RimWorld versions that support Harmony

## Installation
1. Install Harmony mod (required dependency)
2. Place this mod folder in RimWorld/Mods directory
3. Enable in mod configuration
4. Load order: After Harmony

## Troubleshooting
- Check `Player.log` for `[NoNameInGame]` messages
- Verify Harmony is loaded before this mod
- Ensure you're actually playing in Chinese language to see effects