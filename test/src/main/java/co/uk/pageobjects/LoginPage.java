package co.uk.pageobjects;

import org.openqa.selenium.By;

import co.uk.webelements.Button;
import co.uk.webelements.Element;
import co.uk.webelements.TextBox;

public class LoginPage {

    private static Button buttonLogin = new Button("Login", By.xpath("//button[@type='submit']"));
    private static TextBox textBoxPassWord = new TextBox("Password", By.xpath("//input[@name='password']"));
    private static TextBox textBoxUsername = new TextBox("Username", By.xpath("//input[@name='userNameOrEmailAddress']"));
    private static Element invalidCredentialsLoginFailedMessage = new Element("Invalid user name or password message",
            By.xpath("//*[text()='Invalid user name or password']"));

    private static Element notActiveLoginFailedMessage(String user) {
        return new Element("'User " + user + " is not active and can not log in' message",
                By.xpath("//div[text()='User " + user + " is not active and can not log in.']"));
    }

    public static void verifyLoginFailedMessageIsDisplayedForInactiveUser(String user) {
        notActiveLoginFailedMessage(user).verifyDisplayed();
    }

    public static void verifyLoginFailedMessageIsDisplayedForInvalidCredentials() {
        invalidCredentialsLoginFailedMessage.verifyDisplayed();
    }

    public static void clickLoginButton() {
        buttonLogin.click();
    }

    public static void enterUsername(String username) {
        textBoxUsername.setText(username);
    }

    public static void clearUsername() {
        textBoxUsername.clear();
    }

    public static void enterPassword(String password) {
        textBoxPassWord.setPassword(password);
    }

    public static void clearPassword() {
        textBoxPassWord.clear();
    }

    public static void verifyLoginPage() {
        textBoxUsername.verifyDisplayed();
    }

}
