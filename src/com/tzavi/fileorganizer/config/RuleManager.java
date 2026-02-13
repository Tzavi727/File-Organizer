package com.tzavi.fileorganizer.config;

import java.util.HashMap;
import java.util.Map;

import com.tzavi.fileorganizer.ui.MenuManager;

public class RuleManager {
    public static Map<String, String> rules = new HashMap<>();

    public static void setRules() {
        rules.put("zip", "compressed");
        rules.put("exe", "executables");
        rules.put("jpg", "images");
        rules.put("pdf", "documents");
        rules.put("mp4", "videos");
        rules.put("jpeg", "images");
        rules.put("png", "images");
    }

    public static void setNewRule() {
        MenuManager.cleanscreen();
        System.out.println("==================================================");
        System.out.println("          Add new extension and folder");
        System.out.println("For the extension dont type the '.' write only the name");
        System.out.println("             (e.g., exe or zip)");
        System.out.println("==================================================");
        System.out.println("Extension: ");
        System.out.println("==================================================");
        String extension = MenuManager.scanner.nextLine().trim().toLowerCase();
        System.out.println("==================================================");
        System.out.println("Folder name: ");
        System.out.println("==================================================");
        String folderName = MenuManager.scanner.nextLine();
        System.out.println("==================================================");
        System.out.printf("| Added: | Extension: '%s' | To | Folder: '%s' |\n", extension, folderName);
        System.out.println("==================================================");
        rules.put(extension, folderName);
        System.out.println("Press Enter to continue: ");
        System.out.println("==================================================");
        MenuManager.scanner.nextLine();
    }

    public static void listRules() {
        MenuManager.cleanscreen();
        System.out.println("==================================================");
        System.out.println("         Listing supported extensions...");
        System.out.println("==================================================");
        rules.forEach((extension, folder) -> {
            System.out.printf("| Extension: '%s' | -> | Folder: '%s' |\n", extension, folder);
            System.out.println("==================================================");
        });
        System.out.println("           Press Enter to continue: ");
        System.out.println("==================================================");
        MenuManager.scanner.nextLine();
    }
}
