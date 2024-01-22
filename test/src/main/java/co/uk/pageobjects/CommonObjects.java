package co.uk.pageobjects;

import org.openqa.selenium.By;

import co.uk.webelements.Element;
import co.uk.webelements.Link;

public class CommonObjects {

    private final static Element headerUserImage = new Element("Header User Image", By.xpath("//header-user-menu//img[@alt='User Image']"));
    private final static Element updatePasswordOption = new Element("Update password option", By.xpath("//*[contains(text(),'Update Password')]"));
    private final static Element logoutOption = new Element("Logout option", By.xpath("//*[contains(text(),'Logout')]"));
    private final static Element headerHospitalName = new Element("Hospital name", By.xpath("//h3[contains(@class,'hospital-name')]"));
    private final static Element headerBurgerIcon = new Element("Burger icon", By.xpath("//i[contains(@class,'fa-bars')]"));
    private final static Element leftNavigationBrandLogo = new Element("Left navigation brand logo",
            By.xpath("//img[contains(@class,'brand-image')]"));
    private final static Element leftNavigationBrandName = new Element("Left navigation brand name",
            By.xpath("//span[contains(@class,'brand-text')]"));
    private final static Link leftNavigationBrandLink = new Link("Left navigation brand", By.xpath("//a[@class='brand-link']"));
    private final static Element leftNavigationUserImage = new Element("Left navigation user Image",
            By.xpath("//sidebar-user-panel//img[@alt='User Image']"));
    private final static Element leftNavigationDashboardMenu = new Element("Left navigation Dashboard menu",
            By.xpath("//nav//p[contains(text(),'Dashboard')]"));
    private final static Element leftNavigationSurveysMenu = new Element("Left navigation Surveys menu",
            By.xpath("//nav//p[contains(text(),'Surveys')]"));
    private final static Element leftNavigationPOAPsMenu = new Element("Left navigation POAPs menu", By.xpath("//nav//p[contains(text(),'POAPs')]"));
    private final static Element leftNavigationUsersMenu = new Element("Left navigation Users menu", By.xpath("//nav//p[contains(text(),'Users')]"));
    private final static Element leftNavigationRolesMenu = new Element("Left navigation Roles menu", By.xpath("//nav//p[contains(text(),'Roles')]"));
    private final static Element leftNavigationConfigurationMenu = new Element("Left navigation Configuration menu",
            By.xpath("//nav//p[contains(text(),'Configuration')]"));
    private final static Element leftNavigationTheatersMenu = new Element("Left navigation Theaters menu",
            By.xpath("//nav//p[contains(text(),'Theaters')]"));
    private final static Element leftNavigationDevicesMenu = new Element("Left navigation Devices menu",
            By.xpath("//nav//p[contains(text(),'Devices')]"));
    private final static Element leftNavigationSurveySettingsMenu = new Element("Left navigation Survey Settings menu",
            By.xpath("//nav//p[contains(text(),'Survey Settings')]"));
    private final static Element leftNavigationDeviceVideoSettingsMenu = new Element("Left navigation Device Video Settings menu",
            By.xpath("//nav//p[contains(text(),'Device Video Settings')]"));
    private final static Element leftNavigationReportingSettingsMenu = new Element("Left navigation Reporting Settings menu",
            By.xpath("//nav//p[contains(text(),'Reporting Settings')]"));

    private static Element messageFieldValidation(String message) {
        return new Element("Message: " + message, By.xpath("//*[contains(text(),'" + message + "')]"));
    }

    public static void verifyLeftNavigationReportingSettingsMenuIsNotDisplayed() {
        leftNavigationReportingSettingsMenu.verifyNotDisplayed();
    }

    public static void verifyLeftNavigationDeviceVideoSettingsMenuIsNotDisplayed() {
        leftNavigationDeviceVideoSettingsMenu.verifyNotDisplayed();
    }

    public static void verifyLeftNavigationSurveySettingsMenuIsNotDisplayed() {
        leftNavigationSurveySettingsMenu.verifyNotDisplayed();
    }

    public static void verifyLeftNavigationDevicesMenuIsNotDisplayed() {
        leftNavigationDevicesMenu.verifyNotDisplayed();
    }

    public static void verifyLeftNavigationTheatersMenuIsNotDisplayed() {
        leftNavigationTheatersMenu.verifyNotDisplayed();
    }

    public static void verifyLeftNavigationConfigurationMenuIsNotDisplayed() {
        leftNavigationConfigurationMenu.verifyNotDisplayed();
    }

    public static void verifyLeftNavigationRolesMenuIsNotDisplayed() {
        leftNavigationRolesMenu.verifyNotDisplayed();
    }

    public static void verifyLeftNavigationUsersMenuIsNotDisplayed() {
        leftNavigationUsersMenu.verifyNotDisplayed();
    }

    public static void verifyLeftNavigationPOAPsMenuIsNotDisplayed() {
        leftNavigationPOAPsMenu.verifyNotDisplayed();
    }

    public static void verifyLeftNavigationSurveysMenuIsNotDisplayed() {
        leftNavigationSurveysMenu.verifyNotDisplayed();
    }

    public static void verifyLeftNavigationDashboardMenuIsNotDisplayed() {
        leftNavigationDashboardMenu.verifyNotDisplayed();
    }

    public static void verifyLeftNavigationReportingSettingsMenuIsDisplayed() {
        leftNavigationReportingSettingsMenu.verifyDisplayed();
    }

    public static void verifyLeftNavigationDeviceVideoSettingsMenuIsDisplayed() {
        leftNavigationDeviceVideoSettingsMenu.verifyDisplayed();
    }

    public static void verifyLeftNavigationSurveySettingsMenuIsDisplayed() {
        leftNavigationSurveySettingsMenu.verifyDisplayed();
    }

    public static void verifyLeftNavigationDevicesMenuIsDisplayed() {
        leftNavigationDevicesMenu.verifyDisplayed();
    }

    public static void verifyLeftNavigationTheatersMenuIsDisplayed() {
        leftNavigationTheatersMenu.verifyDisplayed();
    }

    public static void verifyLeftNavigationConfigurationMenuIsDisplayed() {
        leftNavigationConfigurationMenu.verifyDisplayed();
    }

    public static void verifyLeftNavigationRolesMenuIsDisplayed() {
        leftNavigationRolesMenu.verifyDisplayed();
    }

    public static void verifyLeftNavigationUsersMenuIsDisplayed() {
        leftNavigationUsersMenu.verifyDisplayed();
    }

    public static void verifyLeftNavigationPOAPsMenuIsDisplayed() {
        leftNavigationPOAPsMenu.verifyDisplayed();
    }

    public static void verifyLeftNavigationSurveysMenuIsDisplayed() {
        leftNavigationSurveysMenu.verifyDisplayed();
    }

    public static void verifyLeftNavigationDashboardMenuIsDisplayed() {
        leftNavigationDashboardMenu.verifyDisplayed();
    }

    public static void clickLeftNavigationReportingSettingsMenu() {
        if (!leftNavigationReportingSettingsMenu.isDisplayed()) {
            clickLeftNavigationConfigurationMenu();
        }
        leftNavigationReportingSettingsMenu.click();
    }

    public static void clickLeftNavigationDeviceVideoSettingsMenu() {
        if (!leftNavigationDeviceVideoSettingsMenu.isDisplayed()) {
            clickLeftNavigationConfigurationMenu();
        }
        leftNavigationDeviceVideoSettingsMenu.click();
    }

    public static void clickLeftNavigationSurveySettingsMenu() {
        if (!leftNavigationSurveySettingsMenu.isDisplayed()) {
            clickLeftNavigationConfigurationMenu();
        }
        leftNavigationSurveySettingsMenu.click();
    }

    public static void clickLeftNavigationDevicesMenu() {
        if (!leftNavigationDevicesMenu.isDisplayed()) {
            clickLeftNavigationConfigurationMenu();
        }
        leftNavigationDevicesMenu.click();
    }

    public static void clickLeftNavigationTheatersMenu() {
        if (!leftNavigationTheatersMenu.isDisplayed()) {
            clickLeftNavigationConfigurationMenu();
        }
        leftNavigationTheatersMenu.click();
    }

    public static void clickLeftNavigationConfigurationMenu() {
        leftNavigationConfigurationMenu.click();
    }

    public static void clickLeftNavigationRolesMenu() {
        leftNavigationRolesMenu.click();
    }

    public static void clickLeftNavigationUsersMenu() {
        leftNavigationUsersMenu.click();
    }

    public static void clickLeftNavigationPOAPsMenu() {
        leftNavigationPOAPsMenu.click();
    }

    public static void clickLeftNavigationSurveysMenu() {
        leftNavigationSurveysMenu.click();
    }

    public static void clickLeftNavigationDashboardMenu() {
        leftNavigationDashboardMenu.click();
    }

    public static void updatePassword() {
        clickHeaderUserImage();
        updatePasswordOption.click();
    }

    public static void logout() {
        clickHeaderUserImage();
        logoutOption.click();
    }

    public static void verifyLeftNavigationBrandLogoIsDisplayed() {
        leftNavigationBrandLogo.verifyDisplayed();
    }

    public static void verifyLeftNavigationBrandName(String name) {
        leftNavigationBrandName.verifyTextContains(name);
    }

    public static void verifyLeftNavigationUserImageIsDisplayed() {
        leftNavigationUserImage.verifyDisplayed();
    }

    public static void clickLeftNavigationBrandLink() {
        leftNavigationBrandLink.click();
    }

    public static void verifyHeaderUserImageIsDisplayed() {
        headerUserImage.verifyDisplayed();
    }

    public static void verifyHeaderUserImageIsNotDisplayed() {
        headerUserImage.verifyNotDisplayed();
    }

    public static void clickHeaderUserImage() {
        headerUserImage.click();
    }

    public static void verifyHospitalNameInHeader(String name) {
        headerHospitalName.verifyTextContains(name);
    }

    public static void verifyHospitalNameInHeaderIsNotDisplayed() {
        headerHospitalName.verifyNotDisplayed();
    }

    public static void clickHeaderBurgerIcon() {
        headerBurgerIcon.click();
    }

    public static void verifyFieldValidationMessageIsDisplayed(String message) {
        messageFieldValidation(message).verifyDisplayed();
    }

}
