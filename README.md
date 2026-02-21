# File Organizer

A robust C# program that organizes your files into folders according to their respective types. This is the official port and evolution of my original Java tool.

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

---

## Technologies Used

*   **Language:** C#
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

1.  **Prerequisites:** You need to have the **.NET SDK** installed on your machine (Version 8.0 or higher recommended).
2.  **Clone the repository:**
    ```bash
    git clone https://github.com/Tzavi727/File-Organizer.git
    ```
3.  **Navigate to the project folder:**
    ```bash
    cd File-Organizer/FileOrganizer
    ```
4.  **Run the application:**
    ```bash
    dotnet run
    ```
*(Note: Running directly from an IDE like Visual Studio 2022 is the recommended way for the best experience.)*