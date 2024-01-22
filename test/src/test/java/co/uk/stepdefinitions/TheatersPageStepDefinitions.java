package co.uk.stepdefinitions;

import java.io.IOException;
import java.util.List;
import java.util.Map;

import co.uk.core.DriverHandler;
import co.uk.pageobjects.TheatersPage;
import io.cucumber.datatable.DataTable;
import io.cucumber.java.en.And;
import io.cucumber.java.en.Then;
import io.cucumber.java.en.When;

public class TheatersPageStepDefinitions {

    @Then("^user is successfully navigated to theaters page$")
    public void verifyTheatersPage() {
        TheatersPage.verifyTheatersPage();
    }

    @When("^user create a new theater$")
    public void clickCreateButton() {
        TheatersPage.clickCreateButton();
    }

    @Then("^create theater modal is displayed$")
    public void verifyCreateTheaterModalIsDisplayed() {
        TheatersPage.verifyCreateTheaterModalIsDisplayed();
    }

    @Then("^create theater modal is not displayed$")
    public void verifyCreateTheaterModalIsNotDisplayed() {
        TheatersPage.verifyCreateTheaterModalIsNotDisplayed();
    }

    @Then("^edit theater modal is displayed$")
    public void verifyEditTheaterModalIsDisplayed() {
        TheatersPage.verifyEditTheaterModalIsDisplayed();
    }

    @Then("^edit theater modal is not displayed$")
    public void verifyEditTheaterModalIsNotDisplayed() {
        TheatersPage.verifyEditTheaterModalIsNotDisplayed();
    }

    @Then("^create theater save button is disabled$")
    public void verifySaveButtonIsDisabled() {
        TheatersPage.verifySaveButtonIsDisabled();
    }

    @Then("^create theater save button is enabled$")
    public void verifySaveButtonIsEnabled() {
        TheatersPage.verifySaveButtonIsEnabled();
    }

    @When("^user enter theater details$")
    public void enterTheaterDetails(DataTable theaterDetails) {
        List<Map<String, String>> data = theaterDetails.asMaps(String.class, String.class);
        String theaterId = data.get(0).get("Theater ID");
        String name = data.get(0).get("Name");
        if (!theaterId.equals("null")) {
            TheatersPage.enterTheaterId(theaterId);
        } else {
            TheatersPage.clearTheaterId();
        }
        if (!name.equals("null")) {
            TheatersPage.enterName(name);
        } else {
            TheatersPage.clearName();
        }
    }

    @When("^user edit theater details$")
    public void editTheaterDetails(DataTable theaterDetails) {
        List<Map<String, String>> data = theaterDetails.asMaps(String.class, String.class);
        String theaterId = data.get(0).get("Theater ID");
        String name = data.get(0).get("Name");
        if (!theaterId.equals("null")) {
            TheatersPage.enterTheaterId(theaterId);
        } else {
            TheatersPage.clearTheaterId();
        }
        if (!name.equals("null")) {
            TheatersPage.enterName(name);
        } else {
            TheatersPage.clearName();
        }
    }

    @And("^user save the new theater$")
    public void saveTheaterCreation() {
        TheatersPage.clickSaveButton();
    }

    @And("^user cancel the theater creation$")
    public void cancelTheaterCreation() {
        TheatersPage.clickCancelButton();
    }

    @And("^user save the new theater details$")
    public void saveTheaterDetails() {
        TheatersPage.clickSaveButton();
    }

    @And("^user cancel the theater editing$")
    public void cancelTheaterEditing() {
        TheatersPage.clickCancelButton();
    }

    @Then("^theater details are saved successfully$")
    public void verifySavedSuccessfullyMessageIsDisplayed() throws IOException {
        TheatersPage.verifySavedSuccessfullyMessageIsDisplayed();
        DriverHandler.delay(1);
        DriverHandler.refreshPage();
    }

    @Then("^theater creation is not successful$")
    public void verifySavedSuccessfullyMessageIsNotDisplayed() throws IOException {
        TheatersPage.verifySavedSuccessfullyMessageIsNotDisplayed();
    }

    @Then("^theater deletion is successful$")
    public void verifySuccessfulDeletionMessageIsDisplayed() throws IOException {
        TheatersPage.verifySuccessfullyDeletedMessageIsDisplayed();
        DriverHandler.refreshPage();
    }

    @Then("^theater deletion is not successful$")
    public void verifySuccessfulDeletionMessageIsNotDisplayed() throws IOException {
        TheatersPage.verifySuccessfullyDeletedMessageIsNotDisplayed();
        DriverHandler.refreshPage();
    }

    @When("^user search theater \"(.*)\"$")
    public void searchTheater(String searchText) {
        TheatersPage.enterSearch(searchText);
        DriverHandler.delay(5);
    }

    @Then("^theater \"(.*)\" is displayed$")
    public void verifyTheaternameIsDisplayed(String theater) {
        TheatersPage.verifyTheaterIsDisplayed(theater);
    }

    @Then("^theater \"(.*)\" is not displayed$")
    public void verifyTheaterIsNotDisplayed(String theater) {
        TheatersPage.verifyTheaterIsNotDisplayed(theater);
    }

    @When("^user edit the theater search result$")
    public void editTheater() {
        TheatersPage.clickEditButton(1);
    }

    @When("^user delete the theater search result$")
    public void deleteTheater() {
        TheatersPage.clickDeleteButton(1);
    }

    @And("^user confirm the theater deletion$")
    public void clickDeleteModalYesButton() {
        TheatersPage.clickDeleteModalYesButton();
    }

    @And("^user cancel the theater deletion$")
    public void clickDeleteModalCancelButton() {
        TheatersPage.clickDeleteModalCancelButton();
    }

}
