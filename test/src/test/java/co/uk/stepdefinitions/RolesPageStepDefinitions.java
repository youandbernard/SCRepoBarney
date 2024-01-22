package co.uk.stepdefinitions;

import co.uk.pageobjects.RolesPage;
import io.cucumber.java.en.Then;

public class RolesPageStepDefinitions {

    @Then("^user is successfully navigated to roles page$")
    public void verifyRolesPage() {
        RolesPage.verifyRolesPage();
    }

}
