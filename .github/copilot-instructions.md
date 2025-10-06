# GitHub Copilot Instructions for "Relevant Stats In Description" Modding Project

## Mod Overview and Purpose

The "Relevant Stats In Description" mod enhances the RimWorld gameplay experience by displaying selected stats for buildings in the architect-menu's description window. This feature assists players in choosing the best wall for defense or the most suitable power generator based on current needs. The mod is configurable, allowing users to disable certain stats or move them to a separate tooltip window.

## Key Features and Systems

- Display of selected building stats in the architect-menu.
- Configurable settings to hide specific stats or show them in a separate tooltip.
- Support for additional mods like 'Vanilla Furniture Expanded - Power' and 'Rimefeller,' displaying specific resource usage (Helixien Gas, Chemfuel).

## Coding Patterns and Conventions

- **Class Naming Conventions**: 
  - Classes are named using PascalCase, reflecting their functionality e.g., `RelevantStatsInDescription`.
  - Static utility classes are used for logical separation of functionalities: `ArchitectCategoryTab_DoInfoBox`.

- **Method Naming Conventions**: 
  - Method names are also written in PascalCase and should clearly describe their purpose.

- **Access Modifiers**:
  - Utilize `public` and `internal` to control accessibility where necessary. Classes and methods designed for mod utilization are public while internal use constructs are marked as `internal`.

- **Organization**:
  - Organize classes based on their functionality and responsibilities within the mod, resulting in clarity and manageability.

## XML Integration

- XML is not prominently used within this project, but it is essential for defining settings and configurations if required.
- Consider making use of XML for localization or additional settings management if extending the mod's functionality.

## Harmony Patching

- **Purpose**: Harmony is used to modify the behavior of the base game functions without altering the original code. It's integral for implementing changes in how the description window displays information.
  
- **Execution**:
  - Ensure Harmony patches are carefully targeted using appropriate prefixes or postfixes to avoid unexpected behavior.
  - Use Harmony to inject additional GUI elements for displaying stats.

## Suggestions for Copilot

1. **Improving Code Suggestions**:
   - Use comments to describe the purpose of each class and method. This guides Copilot in providing context-aware suggestions.
   
2. **Extending Compatibility**:
   - Use Copilot to assist in writing compatibility patches for other popular RimWorld mods by recommending stats that could be displayed.
   
3. **Enhancing Settings Management**:
   - Utilize Copilot to suggest code patterns for creating advanced configuration settings, such as dynamic toggling of displayed stats.
   
4. **Translation and Localization**:
   - Utilize Copilot for expanding language support by automated translation suggestions and organizing XML files for newer languages.
   
5. **Debugging and Testing**:
   - Encourage Copilot to provide suggestions for unit and integration tests to ensure robust and error-free operation of the mod across different scenarios.

Contributions to the mod are welcome. Refer to our Discord community or comment on our GitHub repository for suggestions and feedback.
