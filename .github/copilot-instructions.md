# GitHub Copilot Instructions for "Relevant Stats In Description" Mod

## Mod Overview and Purpose

**Mod Name:** Relevant Stats In Description  
**Author:** Mlie  
**PackageId:** Mlie.RelevantStatsInDescription  

The "Relevant Stats In Description" mod aims to enhance the building selection process in RimWorld by displaying pertinent stats in the description window within the architect menu. By providing stats specific to certain types of buildings, it streamlines decision-making, such as choosing the best defensive wall or the appropriate power generator. Settings allow for customization of which stats appear and whether they display in a separate tooltip window.

## Key Features and Systems

- **Stat Display:** Depending on the building, this mod shows selected stats in the architect menu description window.
- **Customization:** Individual stats can be toggled on or off via settings. Users can also choose to view additional information in a separate tooltip.
- **Mod Support:** Supports display of Helixien Gas use/production for "Vanilla Furniture Expanded - Power" and Chemfuel use for "Rimefeller".
- **Multilingual Support:** Includes translations for German and Spanish, contributed by community members.

## Coding Patterns and Conventions

- **C# Naming Conventions:** Classes and methods follow PascalCase.
- **Commenting:** Code is liberally commented to describe the functionality of methods and important logic components.
- **Namespaces:** Proper namespace usage to organize classes and facilitate modularity.

## XML Integration

- **XML Usage:** Utilizes XML files primarily for settings management and localization. No XML defs are present, indicating no additional content such as new buildings or items are directly added through XML.
- **Localization:** XML files are used for handling translations and multi-language support.

## Harmony Patching

The mod uses Harmony to modify existing game methods and interfaces without altering the original game files, ensuring compatibility and easy updates:

- **Methods Patched:** 
  - `ArchitectCategoryTab_InfoRect`: Handles postfix operations to insert new content.
  - `Designator_Build_Desc`, `Designator_Place_DoExtraGuiControls`: Implements postfix and prefix patches to append or precede existing logic with additional stat display.
  - `ArchitectCategoryTab_DesignationTabOnGUI_Patch_DoInfoBox`: A combination of postfixes and prefixes, often setting up conditions or changes before and after the core logic runs.

## Suggestions for Copilot

- **Code Suggestions:** Assist with writing additional postfix or prefix methods to handle other potential stats as per user feedback and requests.
- **Settings Management:** Generate helper methods for managing the enabling/disabling of stats displays in the mod settings.
- **Localization:** Suggest or create XML entries for additional languages or updated translations.
- **Error Handling:** Propose try-catch blocks or validation checks surrounding potential areas of failure, particularly in user input parsing.
- **Documentation:** Guide in creating comprehensive method and class documentation using XML comments, which can further aid compatibility and future development.

## Contribution and Feedback

- If you have suggestions for other building stats that could be displayed, please leave feedback through the GitHub issues page or join the discussion on our Discord server.

This concludes the AI-powered assistance for enhancing and maintaining the "Relevant Stats In Description" mod.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).

