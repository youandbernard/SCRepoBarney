package co.uk.stepdefinitions;

import java.io.IOException;

import co.uk.core.DriverHandler;
import co.uk.pageobjects.ReportingSettingsPage;
import io.cucumber.java.en.And;
import io.cucumber.java.en.Then;
import io.cucumber.java.en.When;

public class ReportingSettingsPageStepDefinitions {

    @Then("^user is successfully navigated to reporting settings page$")
    public void verifyReportingSettingsPage() {
        ReportingSettingsPage.verifyReportingSettingsPage();
    }
    
    @When("^user check hospital \"(.*)\"$")
    public void checkHospital(String hospital) {
        ReportingSettingsPage.checkHospital(hospital);
    }
    
    @When("^user uncheck hospital \"(.*)\"$")
    public void uncheckHospital(String hospital) {
        ReportingSettingsPage.uncheckHospital(hospital);
    }
    
    @Then("^reporting settings are saved successfully$")
    public void verifySavedSuccessfullyMessageIsDisplayed() throws IOException {
        ReportingSettingsPage.verifySavedSuccessfullyMessageIsDisplayed();
        DriverHandler.delay(1);
        DriverHandler.refreshPage();
    }
    
    @And("^user save the reporting settings$")
    public void saveReportingSettings() {
        ReportingSettingsPage.clickSaveButton();
    }

}
