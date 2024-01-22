package co.uk.stepdefinitions;

import co.uk.pageobjects.ConfigurationPage;
import io.cucumber.java.en.Then;

public class ConfigurationPageStepDefinitions {

    @Then("^user is successfully navigated to configuration page$")
    public void verifyConfigurationPage() {
        ConfigurationPage.verifyConfigurationPage();
    }

}
