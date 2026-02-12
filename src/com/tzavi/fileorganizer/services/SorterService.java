package com.tzavi.fileorganizer.services;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.StandardCopyOption;
import java.util.stream.Stream;

import com.tzavi.fileorganizer.config.RuleManager;
import com.tzavi.fileorganizer.ui.MenuManager;

public class SorterService {
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
        } catch (Exception e) {
            MenuManager.cleanscreen();
            System.out.println("Something went wrong when trying to list files.");
        }
        System.out.println("Your downloads folder should now be organized!");
        MenuManager.scanner.nextLine();
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
        String userHomeInput = MenuManager.scanner.nextLine();
        return Path.of(userHomeInput).toAbsolutePath();
    }
}
