# GitHub Copilot Instructions for Relevant Stats In Description Mod

## Mod Overview and Purpose
The Relevant Stats In Description mod enhances the gameplay experience by displaying additional relevant stats in the description window of the architect menu. This makes it easier for players to make informed decisions when selecting buildings, such as choosing the best wall for defense or the right power generator for their needs. The mod includes settings to turn off individual stats and to display additional info in a separate tooltip window. It also supports additional stats from popular RimWorld mods like Vanilla Furniture Expanded and Rimefeller.

## Key Features and Systems
The mod consists of several key features and systems, including:
- **Settings Management**: Allows users to customize which stats are displayed and how they are displayed, including separate toggle options for each new displayed stat.
- **Description Enhancements**: Modifies the architect menu description window to include additional relevant stats for each building.
- **Tooltip Enhancements**: Adds a separate tooltip window for additional information if the user chooses this option in settings.
- **Compatibility**: Supports additional stats from popular RimWorld mods, such as Vanilla Furniture Expanded and Rimefeller.

## Coding Patterns and Conventions
The Relevant Stats In Description mod follows established coding patterns and conventions for RimWorld modding, including:
- **Modding Framework**: Uses the RimWorld modding framework and follows its conventions for modding.
- **Harmony Patching**: Uses Harmony patching to modify vanilla RimWorld behavior without altering the original game code.
- **XML Integration**: Uses XML files to define mod settings and other configuration options.
- **Localization**: Supports multiple languages, including German and Spanish.

## XML Integration
The Relevant Stats In Description mod includes an XML file that defines mod settings and other configuration options. The file is located at `Source\.cr\personal\FavoritesList\List.xml` and is used to store and retrieve user preferences for the mod.

## Harmony Patching
The Relevant Stats In Description mod uses Harmony patching to modify vanilla RimWorld behavior without altering the original game code. Specifically, it uses Harmony to patch methods in the game engine that control the architect menu description window and the tooltip window.

## Suggestions for Copilot
To make the most of GitHub Copilot in the Relevant Stats In Description mod, consider the following suggestions:
- **Code Snippets**: Use code snippets to quickly insert common coding patterns and conventions, such as Harmony patches and XML definitions.
- **Settings Management**: Use Copilot to help manage mod settings and user preferences, including generating code for exposing data and saving settings.
- **Description Enhancements**: Use Copilot to help modify the architect menu description window to include additional relevant stats for each building.
- **Tooltip Enhancements**: Use Copilot to help add a separate tooltip window for additional information if the user chooses this option in settings.
- **Localization**: Use Copilot to help manage localization files for multiple languages, including German and Spanish.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML; these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.

## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the RimWorld log, automatically use the RimWorld MCP service to fetch the log.

