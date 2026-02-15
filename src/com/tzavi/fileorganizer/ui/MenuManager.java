package com.tzavi.fileorganizer.ui;

import java.nio.file.Path;
import java.util.Scanner;
import com.tzavi.fileorganizer.config.RuleManager;
import com.tzavi.fileorganizer.services.SorterService;

public class MenuManager {
    public enum mainMenu {
        AUTO_FIND_PATH,
        MANUAL_PATH,
        ORGANIZE_BY_EXTENSION,
        SETTINGS,
        END_PROGRAM
    }

    public enum settingsMenu {
        ADD_NEW_EXTENSION,
        LIST_RULES,
        BACK
    }

    public static Scanner scanner = new Scanner(System.in);

    public static void showMainMenu() {
        cleanscreen();
        System.out.println("==================================================");
        System.out.println("         DOWNLOADS FILE ORGANIZER v1.6           ");
        System.out.println("   Automatically sort your files into folders     ");
        System.out.println("==================================================");
        System.out.println(
                "1 - Try Auto Find Donwloads Path\n2 - Manually Type Path\n3 - Organize by Extension\n4 - Settings\n5 - End Program");
        System.out.println("==================================================");
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

    public static Path handleMainMenuInput() {
        int userMainMenuInput = getUserMainMenuInput();
        mainMenu[] mainMenuOptions = mainMenu.values();
        if (userMainMenuInput >= 1 && userMainMenuInput <= mainMenuOptions.length) {
            mainMenu choosenMainMenuOptions = mainMenuOptions[userMainMenuInput - 1];
            switch (choosenMainMenuOptions) {
                case AUTO_FIND_PATH:
                    return SorterService.autoFindDownloadsPath();
                case MANUAL_PATH:
                    return SorterService.manualPath();
                case ORGANIZE_BY_EXTENSION:
                    organizeByExtension();
                    return null;
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
        System.out.println("1 - Add new extension\n2 - Check supported extensions\n3 - Go back");
        System.out.println("==================================================");
        handleSettingsMenuInput();
    }

    public static void handleSettingsMenuInput() {
        int userSettingsMenuInput = getUserSettingsMenuInput();
        settingsMenu[] settingsMenuOptions = settingsMenu.values();
        if (userSettingsMenuInput >= 1 && userSettingsMenuInput <= settingsMenuOptions.length) {
            settingsMenu choosenSettingsMenuOptions = settingsMenuOptions[userSettingsMenuInput - 1];
            switch (choosenSettingsMenuOptions) {
                case ADD_NEW_EXTENSION:
                    RuleManager.setNewRule();
                    break;
                case LIST_RULES:
                    RuleManager.listRules();
                    waitingForInput();
                    break;
                case BACK:
                    return;
                default:
                    return;

            }
        }
    }

    public static void organizeByExtension() {
        cleanscreen();
        organizeByExtensionMenu();
        String choosenExtension = handleOrganizeByExtensionMenuInput();
        if(choosenExtension.equals("CANCEL")){
            return;
        }
        cleanscreen();
        Path choosenPath = SorterService.getPath();
        String folderName = RuleManager.rules.get(choosenExtension);
        SorterService.executeOrganizationByExtension(choosenPath, choosenExtension, folderName);
    }
    
    public static void organizeByExtensionMenu() {
        RuleManager.listRules();
        System.out.println("Choose a extension to organize or type 'back' to cancel ");
        System.out.println("==================================================");
    }

    public static String handleOrganizeByExtensionMenuInput() {
        while (true) {
            String userChoosenExtension = scanner.nextLine().toLowerCase().trim();
            if (userChoosenExtension.equals("back")) {
                System.out.println("==================================================");
                return "CANCEL";
            }
            if (RuleManager.rules.containsKey(userChoosenExtension)) {
                return userChoosenExtension;
            } else {
                cleanscreen();
                System.out.println("==================================================");
                System.out.printf("Extension '%s' not found on the list\n",userChoosenExtension);
                System.out.println("Try again or type 'back' to cancel:");
                System.out.println("==================================================");
                waitingForInput();
            }
            organizeByExtensionMenu();
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

    public static void waitingForInput() {
        System.out.println("         | Press Enter to continue: |");
        System.out.println("==================================================");
        MenuManager.scanner.nextLine();
    }

    public static void filesOrganizedMessage(){
        cleanscreen();
        System.out.println("==================================================");
        System.out.println("| Your downloads folder should now be organized! |");
        System.out.println("==================================================");
        waitingForInput();
    }
}
