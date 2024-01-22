package co.uk.pageobjects;

import org.openqa.selenium.By;

import co.uk.webelements.Element;

public class ConfigurationPage {

    private static Element pageHeader = new Element("Home page header", By.xpath("//h1[text()='Configuration']"));

    public static void verifyConfigurationPage() {
        pageHeader.verifyDisplayed();
    }

}
