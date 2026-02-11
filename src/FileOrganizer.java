import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.StandardCopyOption;
import java.util.HashMap;
import java.util.Map;
import java.util.Scanner;
import java.util.stream.Stream;

public class FileOrganizer {
    public static Scanner scanner = new Scanner(System.in);
    public static Map<String, String> rules = new HashMap<>();

    public enum mainMenu {
        AUTO_FIND_PATH,
        MANUAL_PATH,
        SETTINGS,
        END_PROGRAM
    }

    public enum settingsMenu {
        ADD_NEW_EXTENSION
    }

    public static void main(String[] args) throws Exception {
        setRules();
        while (true) {
            showMainMenu();
            int userMainMenuInput = getUserMainMenuInput();
            Path selectedPath = null;
            selectedPath = handleMainMenuInput(userMainMenuInput);
            if (selectedPath != null) {
                executeOrganization(selectedPath);
            }
        }
    }

    public static void setRules() {
        rules.put("zip", "compressed");
        rules.put("exe", "executables");
        rules.put("jpg", "images");
        rules.put("pdf", "documents");
        rules.put("mp4", "videos");
        rules.put("jpeg", "images");
        rules.put("png", "images");

    }

    public static void moveFile(Path originalFile, String folderDestinationName, Path downloadsPath) {
        Path target = downloadsPath.resolve(folderDestinationName);
        try {
            Files.createDirectories(target);
            Path finalPath = target.resolve(originalFile.getFileName());
            Files.move(originalFile, finalPath, StandardCopyOption.REPLACE_EXISTING);
        } catch (IOException e) {
            e.printStackTrace();
        }

    }

    public static void cleanscreen() {
        try {
            if (System.getProperty("os.name").contains("Windows")) {
                new ProcessBuilder("cmd", "/c", "cls").inheritIO().start().waitFor();
            } else {
                System.out.println("\033[H\033[2J");
                System.out.flush();
            }
        } catch (Exception e) {
            for (int i = 0; i < 50; i++)
                System.out.println();
        }
    }

   public static int getUserMainMenuInput() {
         mainMenu[] mainMenuOptions = mainMenu.values();
        while (true) {
            try {
                String input = scanner.nextLine();
                int choice = Integer.parseInt(input);
                if (choice >= 1 && choice <= mainMenuOptions.length) {
                    return choice;
                } else {
                    System.out.printf("Invalid option! Please type a NUMBER between 1 and %d:\n",
                            mainMenuOptions.length);
                }
            } catch (NumberFormatException e) {
                System.out.printf("Error: Please type a NUMBER between 1 and %d:\n", mainMenuOptions.length);
            }
        }
    }

    public static void setNewRule() {
        cleanscreen();
        System.out.println("==================================================");
        System.out.println("          Add new extension and folder");
        System.out.println("For the extension dont type the '.' write only the name");
        System.out.println("             (e.g., exe or zip)");
        System.out.println("==================================================");
        System.out.println("Extension: ");
        System.out.println("==================================================");
        String extension = scanner.nextLine().trim().toLowerCase();
        System.out.println("==================================================");
        System.out.println("Folder name: ");
        System.out.println("==================================================");
        String folderName = scanner.nextLine();
        System.out.println("==================================================");
        System.out.printf("| Added: | Extension: '%s' | To | Folder: '%s' |\n", extension, folderName);
        System.out.println("==================================================");
        rules.put(extension, folderName);
        System.out.println("Press Enter to continue: ");
        System.out.println("==================================================");
        scanner.nextLine();
    }

    public static Path handleMainMenuInput(int userMainMenuInput) {
        mainMenu[] mainMenuOptions = mainMenu.values();
        if (userMainMenuInput >= 1 && userMainMenuInput <= mainMenuOptions.length) {
            mainMenu choosenMainMenuOptions = mainMenuOptions[userMainMenuInput - 1];
            switch (choosenMainMenuOptions) {
                case AUTO_FIND_PATH:
                    return autoFindDownloadsPath();
                case MANUAL_PATH:
                    return manualPath();
                case SETTINGS:
                    settingsMenu();
                    return null;
                case END_PROGRAM:
                    cleanscreen();
                    System.out.println("Exiting File Organizer...");
                    System.exit(0);
                    break;
                default:
                    return null;
            }
        }
        return null;
    }

    public static void settingsMenu() {
        cleanscreen();
        System.out.println("==================================================");
        System.out.println("            Setting/Customizations");
        System.out.println("==================================================");
        System.out.println("1 - Add new extension");
        System.out.println("==================================================");
        int userInput = getUserSettingsMenuInput();
        switch (userInput) {
            case 1:
                setNewRule();
                break;
            default:
                break;
        }
    }

    public static int getUserSettingsMenuInput() {
         settingsMenu[] settingsMenuOptions = settingsMenu.values();
        while (true) {
            try {
                String input = scanner.nextLine();
                int choice = Integer.parseInt(input);
                if (choice >= 1 && choice <= settingsMenuOptions.length) {
                    return choice;
                } else {
                    System.out.printf("Invalid option! Please type a NUMBER between 1 and %d:\n",
                            settingsMenuOptions.length);
                }
            } catch (NumberFormatException e) {
                System.out.printf("Error: Please type a NUMBER between 1 and %d:\n", settingsMenuOptions.length);
            }
        }
    }

    public static Path autoFindDownloadsPath() {
        Path userHome = Path.of(System.getProperty("user.home")).resolve("Downloads");
        return userHome;
    }

    public static Path manualPath() {
        cleanscreen();
        System.out.println("==================================================");
        System.out.println("Manually Type Path Below: ");
        System.out.println("==================================================");
        String userHomeInput = scanner.nextLine();
        return Path.of(userHomeInput).toAbsolutePath();
    }

    public static void executeOrganization(Path folderPath) {
        if (folderPath == null || !Files.exists(folderPath)) {
            System.out.println("Invalid Path! Cannot organize.");
            return;
        }
        cleanscreen();
        final Path finalPath = folderPath;
        try (Stream<Path> stream = Files.list(folderPath)) {
            stream.forEach(file -> {
                if (Files.isRegularFile(file)) {
                    String fileName = file.getFileName().toString();
                    int dotIndex = fileName.lastIndexOf(".");
                    if (dotIndex > 0) {
                        String extension = fileName.substring(dotIndex + 1).toLowerCase();

                        if (rules.containsKey(extension)) {
                            String destinationFolder = rules.get(extension);
                            moveFile(file, destinationFolder, finalPath);
                        }
                    }
                }
            });
        } catch (Exception e) {
            cleanscreen();
            System.out.println("Something went wrong when trying to list files.");
        }
        System.out.println("Your downloads folder should now be organized!");
        scanner.nextLine();
    }

    public static void showMainMenu() {
        cleanscreen();
        System.out.println("==================================================");
        System.out.println("         DOWNLOADS FILE ORGANIZER v1.0           ");
        System.out.println("   Automatically sort your files into folders     ");
        System.out.println("==================================================");
        System.out.println(
                "1 - Try Auto Find Donwloads Path\n2 - Manually Type Path\n3 - Settings\n4 - End Program");
        System.out.println("==================================================");
    }
}
