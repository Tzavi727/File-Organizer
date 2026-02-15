package com.tzavi.fileorganizer.app;

import java.nio.file.Path;
import com.tzavi.fileorganizer.config.RuleManager;
import com.tzavi.fileorganizer.services.SorterService;
import com.tzavi.fileorganizer.ui.MenuManager;

public class FileOrganizer {

    public static void main(String[] args) throws Exception {
        RuleManager.setRules();
        while (true) {
            MenuManager.showMainMenu();
            Path selectedPath = null;
            selectedPath = MenuManager.handleMainMenuInput();
            if (selectedPath != null) {
                SorterService.executeOrganization(selectedPath);
            }
        }
    }
}
