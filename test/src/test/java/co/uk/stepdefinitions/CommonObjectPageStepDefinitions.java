package co.uk.stepdefinitions;

import java.io.IOException;

import co.uk.core.DriverHandler;
import co.uk.pageobjects.CommonObjects;
import io.cucumber.java.en.And;
import io.cucumber.java.en.Then;
import io.cucumber.java.en.When;

public class CommonObjectPageStepDefinitions {

    @Then("^user is successfully login$")
    public void verifyHeaderUserImageIsDisplayed() throws IOException {
        DriverHandler.delay(5);
        CommonObjects.verifyHeaderUserImageIsDisplayed();
    }

    @Then("^user is not successfully login$")
    public void verifyHeaderUserImageIsNotDisplayed() {
        CommonObjects.verifyHeaderUserImageIsNotDisplayed();
    }

    @Then("^\"(.*)\" is displayed in the header$")
    public void verifyHospitalNameInHeader(String hospital) {
        CommonObjects.verifyHospitalNameInHeader(hospital);
    }

    @And("^hospital name is not displayed in the header$")
    public void verifyHospitalNameInHeaderIsNotDisplayed() {
        CommonObjects.verifyHospitalNameInHeaderIsNotDisplayed();
    }

    @When("user navigate to Reporting Settings page from menu navigation$")
    public static void clickLeftNavigationReportingSettingsMenu() {
        CommonObjects.clickLeftNavigationReportingSettingsMenu();
    }

    @When("user navigate to Device Video Settings page from menu navigation$")
    public static void clickLeftNavigationDeviceVideoSettingsMenu() {
        CommonObjects.clickLeftNavigationDeviceVideoSettingsMenu();
    }

    @When("user navigate to Survey Settings page from menu navigation$")
    public static void clickLeftNavigationSurveySettingsMenu() {
        CommonObjects.clickLeftNavigationSurveySettingsMenu();
    }

    @When("user navigate to Devices page from menu navigation$")
    public static void clickLeftNavigationDevicesMenu() {
        CommonObjects.clickLeftNavigationDevicesMenu();
    }

    @When("user navigate to Theaters page from menu navigation$")
    public static void clickLeftNavigationTheatersMenu() {
        CommonObjects.clickLeftNavigationTheatersMenu();
    }

    @When("user navigate to Configuration page from menu navigation$")
    public static void clickLeftNavigationConfigurationMenu() {
        CommonObjects.clickLeftNavigationConfigurationMenu();
    }

    @When("user navigate to Roles page from menu navigation$")
    public static void clickLeftNavigationRolesMenu() {
        CommonObjects.clickLeftNavigationRolesMenu();
    }

    @When("user navigate to Users page from menu navigation$")
    public static void clickLeftNavigationUsersMenu() {
        CommonObjects.clickLeftNavigationUsersMenu();
    }

    @When("^user navigate to POAPs page from menu navigation$")
    public static void clickLeftNavigationPOAPsMenu() {
        CommonObjects.clickLeftNavigationPOAPsMenu();
    }

    @When("user navigate to Surveys page from menu navigation$")
    public static void clickLeftNavigationSurveysMenu() {
        CommonObjects.clickLeftNavigationSurveysMenu();
    }

    @When("user navigate to Dashboard page from menu navigation$")
    public static void clickLeftNavigationDashboardMenu() {
        CommonObjects.clickLeftNavigationDashboardMenu();
    }

    @When("^user logout$")
    public void logout() {
        CommonObjects.logout();
    }

    @Then("^user has no available left navigation menu$")
    public void verifyNoMenuIsDisplayed() {
        CommonObjects.verifyLeftNavigationReportingSettingsMenuIsNotDisplayed();
        CommonObjects.verifyLeftNavigationDeviceVideoSettingsMenuIsNotDisplayed();
        CommonObjects.verifyLeftNavigationSurveySettingsMenuIsNotDisplayed();
        CommonObjects.verifyLeftNavigationDevicesMenuIsNotDisplayed();
        CommonObjects.verifyLeftNavigationTheatersMenuIsNotDisplayed();
        CommonObjects.verifyLeftNavigationConfigurationMenuIsNotDisplayed();
        CommonObjects.verifyLeftNavigationRolesMenuIsNotDisplayed();
        CommonObjects.verifyLeftNavigationUsersMenuIsNotDisplayed();
        CommonObjects.verifyLeftNavigationPOAPsMenuIsNotDisplayed();
        CommonObjects.verifyLeftNavigationSurveysMenuIsNotDisplayed();
        CommonObjects.verifyLeftNavigationDashboardMenuIsNotDisplayed();
    }

    @Then("^available left navigation menu for \"(.*)\" are displayed$")
    public void verifyNoMenuSectionByRole(String role) {
        if (role.equalsIgnoreCase("admin")) {
            CommonObjects.verifyLeftNavigationDashboardMenuIsDisplayed();
            CommonObjects.verifyLeftNavigationSurveysMenuIsDisplayed();
            CommonObjects.verifyLeftNavigationPOAPsMenuIsDisplayed();
            CommonObjects.verifyLeftNavigationUsersMenuIsDisplayed();
            CommonObjects.verifyLeftNavigationRolesMenuIsDisplayed();
            CommonObjects.verifyLeftNavigationConfigurationMenuIsDisplayed();
            CommonObjects.clickLeftNavigationConfigurationMenu();
            CommonObjects.verifyLeftNavigationReportingSettingsMenuIsDisplayed();
            CommonObjects.verifyLeftNavigationDeviceVideoSettingsMenuIsDisplayed();
            CommonObjects.verifyLeftNavigationSurveySettingsMenuIsDisplayed();
            CommonObjects.verifyLeftNavigationDevicesMenuIsDisplayed();
            CommonObjects.verifyLeftNavigationTheatersMenuIsDisplayed();
        } else if (role.equalsIgnoreCase("surgeon")) {
            CommonObjects.verifyLeftNavigationDashboardMenuIsDisplayed();
            CommonObjects.verifyLeftNavigationSurveysMenuIsNotDisplayed();
            CommonObjects.verifyLeftNavigationPOAPsMenuIsNotDisplayed();
            CommonObjects.verifyLeftNavigationUsersMenuIsNotDisplayed();
            CommonObjects.verifyLeftNavigationRolesMenuIsNotDisplayed();
            CommonObjects.verifyLeftNavigationConfigurationMenuIsNotDisplayed();
            CommonObjects.verifyLeftNavigationReportingSettingsMenuIsNotDisplayed();
            CommonObjects.verifyLeftNavigationDeviceVideoSettingsMenuIsNotDisplayed();
            CommonObjects.verifyLeftNavigationSurveySettingsMenuIsNotDisplayed();
            CommonObjects.verifyLeftNavigationDevicesMenuIsNotDisplayed();
            CommonObjects.verifyLeftNavigationTheatersMenuIsNotDisplayed();
        } else {
            CommonObjects.verifyLeftNavigationDashboardMenuIsDisplayed();
            CommonObjects.verifyLeftNavigationSurveysMenuIsNotDisplayed();
            CommonObjects.verifyLeftNavigationPOAPsMenuIsNotDisplayed();
            CommonObjects.verifyLeftNavigationUsersMenuIsNotDisplayed();
            CommonObjects.verifyLeftNavigationRolesMenuIsNotDisplayed();
            CommonObjects.verifyLeftNavigationConfigurationMenuIsNotDisplayed();
            CommonObjects.verifyLeftNavigationReportingSettingsMenuIsNotDisplayed();
            CommonObjects.verifyLeftNavigationDeviceVideoSettingsMenuIsNotDisplayed();
            CommonObjects.verifyLeftNavigationSurveySettingsMenuIsNotDisplayed();
            CommonObjects.verifyLeftNavigationDevicesMenuIsNotDisplayed();
            CommonObjects.verifyLeftNavigationTheatersMenuIsNotDisplayed();
        }
    }

    @Then("^error message '\"(.*)\"' is displayed$")
    public void verifyFieldValidationMessageIsDisplayedF(String message) {
        CommonObjects.verifyFieldValidationMessageIsDisplayed(message);
    }

}
