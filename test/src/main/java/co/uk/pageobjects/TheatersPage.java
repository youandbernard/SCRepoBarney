package co.uk.pageobjects;

import org.openqa.selenium.By;

import co.uk.webelements.Button;
import co.uk.webelements.Element;
import co.uk.webelements.TextBox;

public class TheatersPage {

    private static Element pageHeader = new Element("Home page header", By.xpath("//h1[text()='Theaters']"));
    private static Button buttonCreate = new Button("Create", By.xpath("//*[contains(text(),'Create')]"));
    private static TextBox textBoxSearch = new TextBox("Search", By.xpath("//input[@type='search']"));
    private static TextBox textBoxTheaterId = new TextBox("Theater ID", By.xpath("//input[@id='Theater_TheaterId']"));
    private static TextBox textBoxName = new TextBox("Name", By.xpath("//input[@id='Theater_Name']"));
    private static Button buttonCancel = new Button("Cancel", By.xpath("//*[contains(@class,'modal')]/button[contains(text(),'Cancel')]"));
    private static Button buttonSave = new Button("Save", By.xpath("//*[contains(@class,'modal')]/button[contains(text(),'Save')]"));
    private static Button buttonDeleteModalCancel = new Button("Cancel", By.xpath("//button[contains(@class,'cancel')]"));
    private static Button buttonDeleteModalYes = new Button("Yes", By.xpath("//button[contains(@class,'confirm')]"));
    private static Element messageSavedSuccessfully = new Element("Saved successfully message",
            By.xpath("//*[contains(text(),'Saved successfully')]"));
    private static Element messageSuccessfullyDeleted = new Element("Successfully deleted message",
            By.xpath("//*[contains(text(),'Successfully deleted')]"));
    private static Element modalCreateTheater = new Element("Create theater modal",
            By.xpath("//h4[@class='modal-title' and text()='Create new theater']"));
    private static Element modalEditTheater = new Element("Edit theater modal", By.xpath("//h4[@class='modal-title' and text()='Edit theater']"));

    private static Button buttonEditByIndex(int index) {
        return new Button("Edit", By.xpath("(//*[contains(@class,'pencil')])[" + index + "]"));
    }

    private static Button buttonDeleteByIndex(int index) {
        return new Button("Delete", By.xpath("(//*[contains(@class,'trash')])[" + index + "]"));
    }

    private static Element theater(String name) {
        return new Element("Theater : " + name, By.xpath("//td[text()='" + name + "']"));
    }

    private static Element valueTheaterIdByIndex(int index) {
        return new Element("Theater ID", By.xpath("//*[text()='Theater ID']//following::tr[" + index + "]/td[1]"));
    }

    private static Element valueNameByIndex(int index) {
        return new Element("Name", By.xpath("//*[text()='Theater ID']//following::tr[" + index + "]/td[2]"));
    }

    public static void enterName(String name) {
        textBoxName.setText(name);
    }
    
    public static void clearName() {
        textBoxName.clear();
    }

    public static void enterTheaterId(String id) {
        textBoxTheaterId.setText(id);
    }
    
    public static void clearTheaterId() {
        textBoxTheaterId.clear();
    }

    public static void verifyTheatersPage() {
        pageHeader.verifyDisplayed();
    }

    public static void clickCreateButton() {
        buttonCreate.click();
    }

    public static void enterSearch(String search) {
        textBoxSearch.setText(search);
    }

    public static void verifyTheaterIdByIndexContains(int index, String value) {
        valueTheaterIdByIndex(index).verifyTextContains(value);
    }

    public static void verifyNameByIndexContains(int index, String value) {
        valueNameByIndex(index).verifyTextContains(value);
    }

    public static void clickEditButton(int index) {
        buttonEditByIndex(index).click();
    }

    public static void clickDeleteButton(int index) {
        buttonDeleteByIndex(index).click();
    }

    public static void verifyTheaterIsDisplayed(String name) {
        theater(name).verifyDisplayed();
    }
    
    public static void verifyTheaterIsNotDisplayed(String name) {
        theater(name).verifyNotDisplayed();
    }

    public static void verifySaveButtonIsDisabled() {
        buttonSave.verifyDisabled();
    }

    public static void verifySaveButtonIsEnabled() {
        buttonSave.verifyEnabled();
    }

    public static void clickSaveButton() {
        buttonSave.click();
    }

    public static void clickCancelButton() {
        buttonCancel.click();
    }

    public static void clickDeleteModalCancelButton() {
        buttonDeleteModalCancel.click();
    }

    public static void clickDeleteModalYesButton() {
        buttonDeleteModalYes.click();
    }

    public static void verifySavedSuccessfullyMessageIsDisplayed() {
        messageSavedSuccessfully.verifyDisplayed();
    }

    public static void verifySavedSuccessfullyMessageIsNotDisplayed() {
        messageSavedSuccessfully.verifyNotDisplayed();
    }

    public static void verifySuccessfullyDeletedMessageIsDisplayed() {
        messageSuccessfullyDeleted.verifyDisplayed();
    }

    public static void verifySuccessfullyDeletedMessageIsNotDisplayed() {
        messageSuccessfullyDeleted.verifyNotDisplayed();
    }

    public static void verifyCreateTheaterModalIsDisplayed() {
        modalCreateTheater.verifyDisplayed();
    }

    public static void verifyCreateTheaterModalIsNotDisplayed() {
        modalCreateTheater.verifyNotDisplayed();
    }

    public static void verifyEditTheaterModalIsDisplayed() {
        modalEditTheater.verifyDisplayed();
    }

    public static void verifyEditTheaterModalIsNotDisplayed() {
        modalEditTheater.verifyNotDisplayed();
    }

}
