package co.uk.pageobjects;

import org.openqa.selenium.By;

import co.uk.webelements.Element;

public class SurveySettingsPage {

    private static Element pageHeader = new Element("Home page header", By.xpath("//h1[text()='Survey Settings']"));

    public static void verifySurveySettingsPage() {
        pageHeader.verifyDisplayed();
    }

}
