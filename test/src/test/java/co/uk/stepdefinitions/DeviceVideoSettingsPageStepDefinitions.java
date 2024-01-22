package co.uk.stepdefinitions;

import co.uk.pageobjects.DeviceVideoSettingsPage;
import io.cucumber.java.en.Then;

public class DeviceVideoSettingsPageStepDefinitions {

    @Then("^user is successfully navigated to device video settings page$")
    public void verifyDeviceVideoSettingsPage() {
        DeviceVideoSettingsPage.verifyDeviceVideoSettingsPage();
    }
    
    @Then("^video is displayed$")
    public void verifyVideoIsDisplayed() {
        DeviceVideoSettingsPage.verifyVideoIsDisplayed();
    }

}
