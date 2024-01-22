package co.uk.pageobjects;

import org.openqa.selenium.By;

import co.uk.webelements.Element;

public class DeviceVideoSettingsPage {

    private static Element pageHeader = new Element("Home page header", By.xpath("//h1[text()='Device Video Settings']"));
    private static Element video = new Element("Video", By.xpath("//video/source[@type='video/mp4' and contains(@src,'.mp4')]"));

    public static void verifyVideoIsDisplayed() {
        video.verifyDisplayed();
    }

    public static void verifyDeviceVideoSettingsPage() {
        pageHeader.verifyDisplayed();
    }

}
