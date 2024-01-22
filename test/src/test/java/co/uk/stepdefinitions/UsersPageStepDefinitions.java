package co.uk.stepdefinitions;

import java.io.IOException;
import java.util.List;
import java.util.Map;

import org.json.JSONException;

import co.uk.core.DriverHandler;
import co.uk.dataobjects.TestDataObjects;
import co.uk.pageobjects.UsersPage;
import io.cucumber.datatable.DataTable;
import io.cucumber.java.en.And;
import io.cucumber.java.en.Then;
import io.cucumber.java.en.When;

public class UsersPageStepDefinitions {

    public static ThreadLocal<String> newPassword = new ThreadLocal<String>();

    public static String getNewPassword() {
        return newPassword.get();
    }

    @Then("^user is successfully navigated to user page$")
    public void verifyUsersPageIsDisplayed() {
        UsersPage.verifyUsersPage();
    }

    @When("^user create a new user$")
    public void clickCreateButton() {
        UsersPage.clickCreateButton();
    }

    @Then("^create user modal is displayed$")
    public void verifyCreateUserModalIsDisplayed() {
        UsersPage.Modal.verifyCreateUserModalIsDisplayed();
    }

    @Then("^create user modal is not displayed$")
    public void verifyCreateUserModalIsNotDisplayed() {
        UsersPage.Modal.verifyCreateUserModalIsNotDisplayed();
    }

    @Then("^edit user modal is displayed$")
    public void verifyEditUserModalIsDisplayed() {
        UsersPage.Modal.verifyEditUserModalIsDisplayed();
    }

    @Then("^reset password modal is displayed$")
    public void verifyResetPasswordModalIsDisplayed() {
        UsersPage.Modal.verifyResetPasswordModalIsDisplayed();
    }

    @When("^user enter admin password$")
    public void enterAdminPassword() throws JSONException, InterruptedException, IOException {
        UsersPage.Modal.enterAdminPassword(TestDataObjects.getPassword("admin"));
    }

    @Then("^reset password modal is not displayed$")
    public void verifyResetPasswordModalIsNotDisplayed() {
        UsersPage.Modal.verifyResetPasswordModalIsNotDisplayed();
    }

    @Then("^create user save button is disabled$")
    public void verifySaveButtonIsDisabled() {
        UsersPage.Modal.verifySaveButtonIsDisabled();
    }

    @Then("^create user save button is enabled$")
    public void verifySaveButtonIsEnabled() {
        UsersPage.Modal.verifySaveButtonIsEnabled();
    }

    @When("^user enter user details$")
    public void enterUserDetails(DataTable userDetails) {
        List<Map<String, String>> data = userDetails.asMaps(String.class, String.class);
        String name = data.get(0).get("Name");
        String surname = data.get(0).get("Surname");
        String username = data.get(0).get("Username");
        String password = data.get(0).get("Password");
        String confirmPassword = data.get(0).get("ConfirmPassword");
        String emailAddress = data.get(0).get("EmailAddress");
        if (!name.equals("null")) {
            UsersPage.Modal.enterName(name);
        } else {
            UsersPage.Modal.clearName();
        }
        if (!surname.equals("null")) {
            UsersPage.Modal.enterSurname(surname);
        } else {
            UsersPage.Modal.clearSurname();
        }
        if (!username.equals("null")) {
            UsersPage.Modal.enterUsername(username);
        } else {
            UsersPage.Modal.clearUsername();
        }
        if (!password.equals("null")) {
            UsersPage.Modal.enterPassword(password);
        } else {
            UsersPage.Modal.clearPassword();
        }
        if (!confirmPassword.equals("null")) {
            UsersPage.Modal.enterConfirmPassword(confirmPassword);
        } else {
            UsersPage.Modal.clearConfirmPassword();
        }
        if (!emailAddress.equals("null")) {
            UsersPage.Modal.enterEmailAddress(emailAddress);
        } else {
            UsersPage.Modal.clearEmailAddress();
        }
        if (data.get(0).get("IsActive").equals("No")) {
            UsersPage.Modal.clickIsActiveCheckbox();
        }
    }

    @When("^user edit user details$")
    public void editUserDetails(DataTable userDetails) {
        List<Map<String, String>> data = userDetails.asMaps(String.class, String.class);
        String name = data.get(0).get("Name");
        String surname = data.get(0).get("Surname");
        String username = data.get(0).get("Username");
        String emailAddress = data.get(0).get("EmailAddress");
        if (!name.equals("null")) {
            UsersPage.Modal.enterName(name);
        } else {
            UsersPage.Modal.clearName();
        }
        if (!surname.equals("null")) {
            UsersPage.Modal.enterSurname(surname);
        } else {
            UsersPage.Modal.clearSurname();
        }
        if (!username.equals("null")) {
            UsersPage.Modal.enterUsername(username);
        } else {
            UsersPage.Modal.clearUsername();
        }
        if (!emailAddress.equals("null")) {
            UsersPage.Modal.enterEmailAddress(emailAddress);
        } else {
            UsersPage.Modal.clearEmailAddress();
        }
        if (data.get(0).get("IsActive").equals("No")) {
            UsersPage.Modal.clickIsActiveCheckbox();
        }
    }

    @And("^user proceed to User roles tab$")
    public void clickUserRolesTab() {
        UsersPage.Modal.clickUserRolesTab();
    }

    @And("^user select role \"(.*)\"$")
    public void clickUserRolesTab(String userRole) {
        UsersPage.Modal.clickUserRoleCheckbox(userRole);
    }

    @And("^user select experience \"(.*)\"$")
    public void clickExperienceCheckbox(String experience) {
        UsersPage.Modal.clickExperienceCheckbox(experience);
    }

    @And("^user proceed to User Hospital tab$")
    public void clickUserHospitalsTab() {
        UsersPage.Modal.clickUserHospitalsTab();
    }

    @And("^user select hospital \"(.*)\"$")
    public void clickUserHospitalCheckbox(String userHospital) {
        UsersPage.Modal.clickUserHospitalCheckbox(userHospital);
    }

    @And("^user proceed to Specialties tab$")
    public void clickSpecialtiesTab() {
        UsersPage.Modal.clickSpecialtiesTab();
    }

    @And("^user select a specialty \"(.*)\"$")
    public void clickSpecialtiesTab(String specialty) {
        UsersPage.Modal.tickSpecialtiesRadioButton(specialty);
    }

    @And("^user save the new user profile$")
    public void saveUserCreation() {
        UsersPage.Modal.clickSaveButton();
    }

    @And("^user cancel the new user creation$")
    public void cancelUserCreation() {
        UsersPage.Modal.clickCancelButton();
    }

    @And("^user save the new password$")
    public void saveNewPassword() {
        newPassword.set(UsersPage.Modal.getNewPassword());
        UsersPage.Modal.clickSaveButton();
    }

    @And("^user cancel the reset password$")
    public void cancelResetPassword() {
        UsersPage.Modal.clickCancelButton();
    }

    @Then("^user details are saved successfully$")
    public void verifySavedSuccessfullyMessageIsDisplayed() throws IOException {
        UsersPage.verifySavedSuccessfullyMessageIsDisplayed();
        DriverHandler.delay(1);
        DriverHandler.refreshPage();
    }

    @Then("^password reset is successful$")
    public void verifyPasswordResetMessageIsDisplayed() throws IOException {
        UsersPage.verifyPasswordResetMessageIsDisplayed();
        DriverHandler.delay(1);
        DriverHandler.refreshPage();
    }

    @Then("^user creation is not successful$")
    public void verifySavedSuccessfullyMessageIsNotDisplayed() throws IOException {
        UsersPage.verifySavedSuccessfullyMessageIsNotDisplayed();
    }

    @Then("^user deletion is successful$")
    public void verifySuccessfulDeletionMessageIsDisplayed() throws IOException {
        UsersPage.verifySuccessfullyDeletedMessageIsDisplayed();
        DriverHandler.refreshPage();
    }

    @Then("^user deletion is not successful$")
    public void verifySuccessfulDeletionMessageIsNotDisplayed() throws IOException {
        UsersPage.verifySuccessfullyDeletedMessageIsNotDisplayed();
        DriverHandler.refreshPage();
    }

    @And("^error message for existing email '\"(.*)\"' is displayed$")
    public void verifyExistingEmailErrorMessageIsDisplayed(String email) throws IOException {
        UsersPage.Modal.verifyExistingEmailErrorMessageIsDisplayed(email);
        DriverHandler.refreshPage();
    }

    @And("^error message for existing username '\"(.*)\"' is displayed$")
    public void verifyExistingUsernameErrorMessageIsDisplayed(String username) throws IOException {
        UsersPage.Modal.verifyExistingUsernameErrorMessageIsDisplayed(username);
        DriverHandler.refreshPage();
    }

    @When("^user search user \"(.*)\" with IsActive status \"(.*)\"$")
    public void searchUser(String searchText, String status) {
        UsersPage.enterSearchText(searchText);
        UsersPage.clickSearchDropDown();
        UsersPage.tickIsActiveRadioButton(status);
        UsersPage.clickSearchButton();
    }

    @Then("^user \"(.*)\" is displayed$")
    public void verifyUsernameIsDisplayed(String username) {
        UsersPage.verifyUsernameIsDisplayed(username);
    }

    @Then("^user \"(.*)\" is not displayed$")
    public void verifyUserIsNotDisplayed(String userDetail) {
        UsersPage.verifyUserIsNotDisplayed(userDetail);
    }

    @When("^user edit the user search result$")
    public void editUser() {
        UsersPage.clickEditButton(1);
    }

    @When("^user delete the user search result$")
    public void deleteUser() {
        UsersPage.clickDeleteButton(1);
    }

    @When("^user delete the user if existing$")
    public void deleteExistingUser() throws IOException {
        DriverHandler.delay(3);
        if (UsersPage.isDeleteButtonExist(1)) {
            UsersPage.clickDeleteButton(1);
            clickDeleteModalYesButton();
            verifySuccessfulDeletionMessageIsDisplayed();
        }
    }

    @When("^user reset the password of user search result$")
    public void resetPasswordOfUser() {
        UsersPage.clickResetPasswordButton(1);
    }

    @And("^user confirm the user deletion$")
    public void clickDeleteModalYesButton() {
        UsersPage.Modal.clickDeleteModalYesButton();
    }

    @And("^user cancel the user deletion$")
    public void clickDeleteModalCancelButton() {
        UsersPage.Modal.clickDeleteModalCancelButton();
    }

}
