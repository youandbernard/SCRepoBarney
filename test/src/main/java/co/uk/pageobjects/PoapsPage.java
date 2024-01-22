package co.uk.pageobjects;

import org.openqa.selenium.By;

import co.uk.core.DriverHandler;
import co.uk.webelements.Button;
import co.uk.webelements.Element;
import co.uk.webelements.TextBox;

public class PoapsPage {
    private static Element headerPOAPs = new Element("Poaps Header", By.xpath("//h1[text()='Pre Operative Assessments']"));
    private static Button createButton = new Button("Create", By.xpath("//a[contains(text(),'Create')]"));
    private static TextBox searchBox = new TextBox("SearchBox", By.xpath("//input[@type='search']"));

    private static Element searchresult(String result) {
        return new Element("Search " + result, By.xpath("//td[contains(text(),'" + result + "')]"));
    }

    private static Button editPoapButton(String patientId) {
        return new Button("Edit", By.xpath("(//td[text()='" + patientId + "']/following::td/button/i[contains(@class,'pencil')])[1]"));
    }

    private static Button deletePoapButton(String patientId) {
        return new Button("Edit", By.xpath("(//td[text()='" + patientId + "']/following::td/button/i[contains(@class,'trash')])[1]"));
    }

    private static Element deleteMesssage = new Element("Delete message",
            By.xpath("//div[@class='toast-message' and text()='Successfully deleted']"));
    private static Element successMessage = new Element("Success messsage",
            By.xpath("//div[@class='toast-message' and text()='Saved successfully']"));

    public static void clickEditButton(String patientId) {
        editPoapButton(patientId).click();
    }

    public static void clickCreateButton() {
        createButton.click();
    }

    public static void verifyDeleteMessage() {
        deleteMesssage.verifyDisplayed();
    }

    public static void verifySuccessMessage() {
        successMessage.verifyDisplayed();
    }

    public static void enterSearch(String searchtext) {
        searchBox.setText(searchtext);
        DriverHandler.delay(3);
    }

    public static void verifySearchResultIsDisplayed(String result) {
        searchresult(result).verifyDisplayed();
    }

    public static void verifySearchResultIsNotDisplayed(String result) {
        searchresult(result).verifyNotDisplayed();
    }

    public static void verifyPOAPsPage() {
        headerPOAPs.verifyDisplayed();
    }

    public static void clickDeleteButton(String patientID) {
        deletePoapButton(patientID).click();
    }

    public static class DeletePoapModal {
        private static Element modalDelete = new Element("Delete modal",
                By.xpath("//div[@class='swal-modal']/div[contains(text(),'Are you sure?')]"));
        private static Button yesButton = new Button("Yes", By.xpath("//button[contains(@class,'swal-button') and contains(text(),'Yes')]"));
        private static Button noButton = new Button("No", By.xpath("//button[contains(@class,'swal-button') and contains(text(),'No')]"));

        public static void modalIsDisplayed() {
            modalDelete.verifyDisplayed();
        }

        public static void clickYes() {
            yesButton.click();
        }

        public static void clickNo() {
            noButton.click();
        }
    }
}
