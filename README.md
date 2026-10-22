# CodePatternReplacer

A lightweight Windows desktop utility for finding and replacing text across files in a selected directory.

## Features

- Search for text in files with configurable extensions.
- Scan nested directories optionally.
- Preview matching files and count occurrences before applying changes.
- Replace matching text in files.
- Create backup copies before modifying files.
- Display scan results, modifications, and errors in an activity log.

## Technology Stack

- C#
- .NET 10
- Windows Forms

## Getting Started

### Prerequisites

- Windows
- .NET 10 SDK

### Run the application

Clone the repository and open a terminal in the project directory:

```powershell
dotnet restore
dotnet run
```

## How to Use

1. Select the target directory.
2. Enter the text to search for.
3. Enter the replacement text.
4. Specify the file extensions, such as `.cs`, `.cshtml`, `.js`, or `.config`.
5. Choose whether to include subdirectories and create backups.
6. Preview the matching files.
7. Apply replacements after reviewing the results.

## Safety Notes

- Test the application on sample files before using it on important projects.
- Keep backups of important source code.
- Review the preview results before applying replacements.
- Text replacement is literal and does not understand programming language syntax. Replacing code can introduce compilation errors or change application behavior.

## Current Limitations

- The application currently uses a simple Windows Forms interface.
- Preview results are displayed in the activity log rather than a side-by-side diff view.
- Automated tests and advanced encoding preservation are not yet implemented.

## Purpose

This project demonstrates practical C# desktop development, file-system operations, text processing, and basic safeguards for bulk file modifications.
