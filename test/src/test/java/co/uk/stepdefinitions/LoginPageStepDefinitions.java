package co.uk.stepdefinitions;

import java.io.IOException;
import java.util.List;
import java.util.Map;

import org.json.JSONException;

import co.uk.dataobjects.TestDataObjects;
import co.uk.pageobjects.LoginPage;
import io.cucumber.datatable.DataTable;
import io.cucumber.java.en.Then;
import io.cucumber.java.en.When;

public class LoginPageStepDefinitions {

    @When("^user login as \"(.*)\"$")
    public void login(String userRole) throws JSONException, InterruptedException, IOException {
        LoginPage.enterUsername(TestDataObjects.getUsername(userRole));
        LoginPage.enterPassword(TestDataObjects.getPassword(userRole));
        LoginPage.clickLoginButton();
    }

    @When("^user enter username \"(.*)\" and password \"(.*)\"$")
    public void login(String username, String password) throws InterruptedException, IOException {
        LoginPage.enterUsername(username);
        LoginPage.enterPassword(password);
        LoginPage.clickLoginButton();
    }

    @When("^user login with credentials$")
    public void login(DataTable credentials) {
        List<Map<String, String>> data = credentials.asMaps(String.class, String.class);
        String username = data.get(0).get("Username");
        String password = data.get(0).get("Password");
        if (!username.equals("null")) {
            LoginPage.enterUsername(username);
        } else {
            LoginPage.clearUsername();
        }
        if (!password.equals("null")) {
            LoginPage.enterPassword(password);
        } else {
            LoginPage.clearPassword();
        }
        LoginPage.clickLoginButton();
    }

    @When("^user enter username \"(.*)\" and the new password$")
    public void loginWithNewPassword(String username) throws InterruptedException, IOException {
        LoginPage.enterUsername(username);
        LoginPage.enterPassword(UsersPageStepDefinitions.getNewPassword());
        LoginPage.clickLoginButton();
    }

    @Then("^user is in login page$")
    public static void verifyLoginPage() {
        LoginPage.verifyLoginPage();
    }

    @Then("^login failed message is displayed for inactive user \"(.*)\"$")
    public static void verifyLoginFailedMessageIsDisplayedForInactiveUser(String user) {
        LoginPage.verifyLoginFailedMessageIsDisplayedForInactiveUser(user);
    }

    @Then("^login failed message is displayed for invalid credentials$")
    public static void verifyLoginFailedMessageIsDisplayedForInvalidCredentials() {
        LoginPage.verifyLoginFailedMessageIsDisplayedForInvalidCredentials();
    }

}
