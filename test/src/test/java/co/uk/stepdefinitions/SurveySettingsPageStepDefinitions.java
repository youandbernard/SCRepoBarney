package co.uk.stepdefinitions;

import co.uk.pageobjects.SurveySettingsPage;
import io.cucumber.java.en.Then;

public class SurveySettingsPageStepDefinitions {

    @Then("^user is successfully navigated to survey settings page$")
    public void verifySurveySettingsPage() {
        SurveySettingsPage.verifySurveySettingsPage();
    }

}
