package co.uk.pageobjects;

import org.openqa.selenium.By;

import co.uk.webelements.Button;
import co.uk.webelements.Element;

public class ReportingSettingsPage {

    private static Element pageHeader = new Element("Home page header", By.xpath("//h1[text()='Reporting Settings']"));
    private static Button buttonSave = new Button("Save", By.xpath("//button[@type='submit']"));
    private static Element messageSavedSuccessfully = new Element("Saved successfully message",
            By.xpath("//*[contains(text(),'Saved successfully')]"));

    private static Element checkBoxHospital(String hospital) {
        return new Element(hospital, By.xpath("//label[contains(text(),'Test Hospital')]/preceding::input[1]"));
    }

    public static void verifySavedSuccessfullyMessageIsDisplayed() {
        messageSavedSuccessfully.verifyDisplayed();
    }

    public static void clickSaveButton() {
        buttonSave.click();
    }

    public static void checkHospital(String hospital) {
        if (!checkBoxHospital(hospital).isSelected()) {
            checkBoxHospital(hospital).click();
        }
    }

    public static void uncheckHospital(String hospital) {
        if (checkBoxHospital(hospital).isSelected()) {
            checkBoxHospital(hospital).click();
        }
    }

    public static void verifyReportingSettingsPage() {
        pageHeader.verifyDisplayed();
    }

}
