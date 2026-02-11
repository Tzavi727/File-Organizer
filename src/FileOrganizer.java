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

    public static void main(String[] args) throws Exception {
        setRules();
        Path userHome = null;
        while (true) {
            cleanscreen();
            System.out.println("==================================================");
            System.out.println("         DOWNLOADS FILE ORGANIZER v1.0           ");
            System.out.println("   Automatically sort your files into folders     ");
            System.out.println("==================================================");
            System.out.println(
                    "1 - Try Auto Find Donwloads Path\n2 - Manually Type Downloads Path\n3 - Manually Type Another Folder Path");
            System.out.println("==================================================");
            int userInput = getUserInput();
            if (userInput == 1) {
                userHome = Path.of(System.getProperty("user.home"));
                userHome = userHome.resolve("Downloads");
            } else if (userInput == 2 || userInput == 3) {
                cleanscreen();
                System.out.println("==================================================");
                System.out.println("Manually Type Path Below: ");
                System.out.println("==================================================");
                String userHomeInput = scanner.nextLine();
                userHome = Path.of(userHomeInput).toAbsolutePath();
            }
            if (userHome != null && Files.exists(userHome)) {
                cleanscreen();
                final Path finalPath = userHome;
                try (Stream<Path> stream = Files.list(userHome)) {
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
                break;
            } else {
                cleanscreen();
                System.out.printf("Path '%s' not found.\n", userHome);
                System.out.println("Press ENTER to try again...");
                scanner.nextLine();
                userHome = null;
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

    public static int getUserInput() {
        while (true) {
            try {
                String input = scanner.nextLine();
                int choice = Integer.parseInt(input);
                if (choice == 1 || choice == 2 || choice == 3) {
                    return choice;
                } else {
                    System.out.println("Invalid option! Please type a NUMBER between 1 and 3:");
                }
            } catch (NumberFormatException e) {
                System.out.println("Error: Please type a NUMBER between 1 and 3:");
            }
        }
    }
}
