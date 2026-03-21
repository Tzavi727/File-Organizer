# File Organizer

A robust C# program that organizes your files into folders according to their respective types. This is the official port and evolution of my original Java tool.

<p align="middle">
  <img src="https://i.imgur.com/C3CVl6N.png" width="410" align="middle"/>
  <img src="https://i.imgur.com/VEZNVQO.png" width="410" align="middle"/>
  <img src="https://i.imgur.com/4fqSSjp.png" width="410" align="middle"/>
</p>

---

## Features

*   **Auto Find Path**
    *   Automatically locates your system's downloads folder.
*   **Manual Directory Support**
    *   Target any specific folder on your computer for organization.
*   **Auto Create Folders**
    *   Automatically creates destination folders or verifies existing ones, preventing duplicates.
*   **Extension Sorting**
    *   It also auto sorts files by its extension (e.g., .png -> images folder) using LINQ queries.
*   **Modular Architecture**
    *   Cleanly separated into UI, Service, Config, and Settings layers for a professional-grade structure.
*   **Modern GUI Dashboard**
    *   A clean, industrial interface inspired by professional engineering tools
*   **Real-Time File Scanning**
    *   Automatically scans and lists files in the selected directory as you type or browse.

---

## Technologies Used

*   **Language:** C# & Avalonia UI (XAML)
*   **Framework:** .NET
*   **Development Environment:** Visual Studio 2022 / VS Code
*   **Key C# Concepts Applied:**
    *   **LINQ & Lambdas:** For data filtering and processing.
    *   **Dictionary (Generic):** Used as the "brain" for extension mapping.
    *   **System.IO:** High-level interaction with the Windows File System.
    *   **JSON Serialization:** For persistent user data storage.
    *   **Error Handling:** Robust validation with `int.TryParse` and `try-catch` blocks.

---

## How to Run

### Option 1: Just get the latest release (Recommended)

1.  Go to the [Latest Release](https://github.com/Tzavi727/File-Organizer/releases) page.
2.  Download the `FileOrganizer.zip` file.
3.  Extract it anywhere and run **`FileOrganizer.exe`**.

---

### Option 2: Run from the source

1.  **Prerequisites:** You need to have the **.NET SDK** installed on your machine (Version 8.0 or higher recommended).
2.  **Clone the repository:**
    ```bash
    git clone https://github.com/Tzavi727/File-Organizer.git
    ```
3.  **Navigate to the project folder:**
    ```bash
    cd src/FileOrganizer
    ```
4.  **Run the application:**
    ```bash
    dotnet run
    ```
*(Note: Running directly from an IDE like Visual Studio 2022 is the recommended way for the best experience.)*