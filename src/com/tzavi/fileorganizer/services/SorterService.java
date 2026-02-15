package com.tzavi.fileorganizer.services;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.StandardCopyOption;
import java.util.stream.Stream;

import com.tzavi.fileorganizer.config.RuleManager;
import com.tzavi.fileorganizer.ui.MenuManager;

public class SorterService {
    public static void moveFile(Path originalFile, String targetFolderName, Path targetPath) {
        Path target = targetPath.resolve(targetFolderName);
        try {
            Files.createDirectories(target);
            Path finalPath = target.resolve(originalFile.getFileName());
            Files.move(originalFile, finalPath, StandardCopyOption.REPLACE_EXISTING);
        } catch (IOException e) {
            System.out.println("Could not move file: " + originalFile.getFileName() + " (File might be in use)");
        }
    }

    public static void executeOrganization(Path folderPath) {
        if (folderPath == null || !Files.exists(folderPath)) {
            System.out.println("Invalid Path! Cannot organize.");
            return;
        }
        MenuManager.cleanscreen();
        final Path finalPath = folderPath;
        try (Stream<Path> stream = Files.list(folderPath)) {
            stream.forEach(file -> {
                if (Files.isRegularFile(file)) {
                    String fileName = file.getFileName().toString();
                    int dotIndex = fileName.lastIndexOf(".");
                    if (dotIndex > 0) {
                        String extension = fileName.substring(dotIndex + 1).toLowerCase();

                        if (RuleManager.rules.containsKey(extension)) {
                            String destinationFolder = RuleManager.rules.get(extension);
                            moveFile(file, destinationFolder, finalPath);
                        }
                    }
                }
            });
            MenuManager.filesOrganizedMessage();
        } catch (Exception e) {
            MenuManager.cleanscreen();
            System.out.println("Something went wrong when trying to organize files.");
        }
    }

    public static Path autoFindDownloadsPath() {
        Path userHome = Path.of(System.getProperty("user.home")).resolve("Downloads");
        return userHome;
    }

    public static Path manualPath() {
        MenuManager.cleanscreen();
        System.out.println("==================================================");
        System.out.println("Manually Type Path Below: ");
        System.out.println("==================================================");
        String userHomeInput = MenuManager.scanner.nextLine().trim();
        Path choosenPath = Path.of(userHomeInput).toAbsolutePath();
        if (userHomeInput.isEmpty() || !Files.exists(choosenPath)) {
            MenuManager.cleanscreen();
            System.out.println("==================================================");
            System.out.println("              Path not found");
            System.out.println("         Press ENTER to continue:");
            System.out.println("==================================================");
            MenuManager.scanner.nextLine();
            return null;
        } else {
            return choosenPath;
        }
    }

    public static Path getPath() {
        while (true) {
            try {
                MenuManager.cleanscreen();
                System.out.println("==================================================");
                System.out.println("                 Choose a path");
                System.out.println("==================================================");
                System.out.println("1 - Try Auto Find Donwloads Path\n2 - Manually Type Path");
                System.out.println("==================================================");
                String userInputString = MenuManager.scanner.nextLine();
                int userInputInt = Integer.parseInt(userInputString);
                if (userInputInt == 1) {
                    return autoFindDownloadsPath();
                } else if (userInputInt == 2) {
                    return manualPath();
                } else {
                    continue;
                }
            } catch (NumberFormatException e) {
                System.out.println("==================================================");
                System.out.println("Invalid option!\nPlease type a NUMBER");
                System.out.println("==================================================");
                MenuManager.waitingForInput();
            }
        }
    }

    public static void executeOrganizationByExtension(Path folderPath, String extension, String folderName) {
        if (folderPath == null || !Files.exists(folderPath)) {
            System.out.println("Invalid Path! Cannot organize.");
            return;
        }
        MenuManager.cleanscreen();
        try (Stream<Path> stream = Files.list(folderPath)) {
            stream.filter(file -> file.getFileName().toString().toLowerCase().endsWith("." + extension))
                    .forEach(file -> {
                        moveFile(file, folderName, folderPath);
                    });
            MenuManager.filesOrganizedMessage();
        } catch (Exception e) {
            MenuManager.cleanscreen();
            System.out.println("Something went wrong when trying to organize files.");
        }
    }
}
