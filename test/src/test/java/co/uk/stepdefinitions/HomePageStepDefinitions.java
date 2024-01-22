package co.uk.stepdefinitions;

import java.io.IOException;

import co.uk.core.DriverHandler;
import co.uk.pageobjects.HomePage;
import io.cucumber.java.en.Then;
import io.cucumber.java.en.When;

public class HomePageStepDefinitions {

    @Then("^user is successfully navigated to home page$")
    public void verifyHomePage() {
        HomePage.verifyHomePage();
    }

    @When("^user select \"(.*)\" from hospital dropdown$")
    public void selectHospitalByName(String hospital) {
        HomePage.clickHospitalDropdown();
        HomePage.selectHospitalByName(hospital);
    }

    @When("^user click the hospital dropdown$")
    public void clickHospitalDropdown() throws IOException {
        HomePage.clickHospitalDropdown();
    }

    @Then("^user is not able to see not assigned hospital \"(.*)\"$")
    public void verifyHospitalIsNotDisplayed(String hospital) throws IOException {
        HomePage.verifyHospitalIsNotDisplayed(hospital);
        DriverHandler.refreshPage();
    }

    @Then("^user has no available menu section$")
    public void verifyNoMenuSectionIsDisplayed() {
        HomePage.verifyReportsBoxIsNotDisplayed();
        HomePage.verifyConfigurationBoxIsNotDisplayed();
        HomePage.verifyUserManagementBoxIsNotDisplayed();
        HomePage.verifySurveysBoxIsNotDisplayed();
        HomePage.verifyPOAPBoxIsNotDisplayed();
    }

    @Then("^available menu sections for \"(.*)\" are displayed$")
    public void verifyNoMenuSectionByRole(String role) {
        if (role.equalsIgnoreCase("admin")) {
            HomePage.verifyReportsBoxIsDisplayed();
            HomePage.verifyConfigurationBoxIsDisplayed();
            HomePage.verifyUserManagementBoxIsDisplayed();
            HomePage.verifySurveysBoxIsDisplayed();
            HomePage.verifyPOAPBoxIsDisplayed();
        } else if (role.equalsIgnoreCase("surgeon")) {
            HomePage.verifyReportsBoxIsNotDisplayed();
            HomePage.verifyConfigurationBoxIsNotDisplayed();
            HomePage.verifyUserManagementBoxIsNotDisplayed();
            HomePage.verifySurveysBoxIsDisplayed();
            HomePage.verifyPOAPBoxIsDisplayed();
        } else {
            HomePage.verifyReportsBoxIsNotDisplayed();
            HomePage.verifyConfigurationBoxIsNotDisplayed();
            HomePage.verifyUserManagementBoxIsNotDisplayed();
            HomePage.verifySurveysBoxIsDisplayed();
            HomePage.verifyPOAPBoxIsDisplayed();
        }
    }

    @Then("^reports menu sections is displayed$")
    public void verifyReportsBoxIsDisplayed() {
        HomePage.verifyReportsBoxIsDisplayed();
    }

    @Then("^reports menu sections is not displayed$")
    public void verifyReportsBoxIsNotDisplayed() {
        HomePage.verifyReportsBoxIsNotDisplayed();
    }

    @When("^user open reports from menu sections$")
    public void clickReportsBox() {
        HomePage.clickReportsBox();
    }

    @Then("^reports pdf is displayed$")
    public void verifyReportsPdfIsDisplayed() {
        HomePage.verifyReportsPdfIsDisplayed();
    }

    @Then("^message: \"(.*)\" is displayed$")
    public void verifyHomePageMessage(String message) {
        HomePage.verifyHomePageMessage(message);
    }

}
