# File Organizer

A C# program that organizes your files into folders according to their respective types. This is the official port and evolution of my original Java tool.

<p align="middle">
  <img src="https://i.imgur.com/AeZnJHr.jpeg" width="410" align="middle"/>
  <img src="https://i.imgur.com/3Llu21L.jpeg" width="410" align="middle"/>
  <img src="https://i.imgur.com/nWc03bc.jpeg"" width="410" align="middle"/>
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
*   **Undo System**
    *   It also has an undo option allowing you to safely go back in session undoing your last session.
*   **Discord Integration**
    *   Live Rich Presence tracking your organization session.
*   **Real-Time File Scanning**
    *   Automatically scans and lists files in the selected directory as you type or browse.
*   **Session Logs:**
    *   Persistent JSON tracking of every file movement.
*   **Import and Export Rules**
    *   Import or export your rules or export someone else rules.
*   **Themes**
    *   File Organizer also has Themes such as dark and light themes. 
*   **Shortcuts**
    *   The program also works normally with shortcuts such as ctrl+z to undo or ctrl+s to save your rules. 
   
---

## Safety & Reliability

The **File Organizer** provides multiple tools to ensure your files remain safe and easy to track:

*   **Undo System**
    *   After each organization session, you always have the option to undo your last action. The File Organizer uses a **Stack** structure to handle your session history, allowing you to safely revert every move made in the current session.
*   **Files History**
    *   The program keeps a detailed history of every organization. You can find the log file manually in the program's folder or open it directly through the UI: **File -> Show History**.
*   **Persistent Log (JSON)**
    *   As mentioned, the program logs every session. The JSON log file contains key data such as: 
      
       `| Unique Session ID | Organization Timestamp | File's original path | File's new path |`.

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