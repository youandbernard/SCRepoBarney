package co.uk.pageobjects;

import org.openqa.selenium.By;

import co.uk.webelements.Element;

public class DevicesPage {

    private static Element pageHeader = new Element("Home page header", By.xpath("//h1[text()='Devices']"));

    public static void verifyDevicesPage() {
        pageHeader.verifyDisplayed();
    }

}
