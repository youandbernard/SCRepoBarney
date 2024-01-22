package co.uk.stepdefinitions;

import co.uk.pageobjects.DevicesPage;
import io.cucumber.java.en.Then;

public class DevicesPageStepDefinitions {

    @Then("^user is successfully navigated to devices page$")
    public void verifyDevicesPage() {
        DevicesPage.verifyDevicesPage();
    }

}
