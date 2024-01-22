package co.uk.pageobjects;

import org.openqa.selenium.By;

import co.uk.webelements.Element;

public class HomePage {

    private static Element homePageHeader = new Element("Home page header", By.xpath("//h1[text()='Home page']"));
    private static Element pOAPBox = new Element("POAP box", By.xpath("//section//*[text()='POAP']"));
    private static Element surveysBox = new Element("Surveys box", By.xpath("//section//*[text()='Surveys']"));
    private static Element userManagementBox = new Element("User Management box", By.xpath("//section//*[text()='User Management']"));
    private static Element configurationBox = new Element("Configuration box", By.xpath("//section//*[text()='Configuration']"));
    private static Element reportsBox = new Element("Reports box", By.xpath("//section//*[text()='Reports']"));
    private static Element dropDownHospital = new Element("Hospital dropdown", By.xpath("//select[@id='Hospital']"));
    private static Element reportsPDF = new Element("Reports PDF", By.xpath("//pdf-viewer"));

    private static Element message(String message) {
        return new Element(message, By.xpath("//*[contains(text(),'" + message + "')]"));
    }

    private static Element nameHospital(String hospital) {
        return new Element(hospital, By.xpath("//option[text()='" + hospital + "']"));
    }

    public static void verifyReportsPdfIsDisplayed() {
        reportsPDF.verifyDisplayed();
    }

    public static void verifyHomePageMessage(String message) {
        message(message).verifyDisplayed();
    }

    public static void verifyReportsBoxIsNotDisplayed() {
        reportsBox.verifyNotDisplayed();
    }

    public static void verifyConfigurationBoxIsNotDisplayed() {
        configurationBox.verifyNotDisplayed();
    }

    public static void verifyUserManagementBoxIsNotDisplayed() {
        userManagementBox.verifyNotDisplayed();
    }

    public static void verifySurveysBoxIsNotDisplayed() {
        surveysBox.verifyNotDisplayed();
    }

    public static void verifyPOAPBoxIsNotDisplayed() {
        pOAPBox.verifyNotDisplayed();
    }

    public static void verifyReportsBoxIsDisplayed() {
        reportsBox.verifyDisplayed();
    }

    public static void verifyConfigurationBoxIsDisplayed() {
        configurationBox.verifyDisplayed();
    }

    public static void verifyUserManagementBoxIsDisplayed() {
        userManagementBox.verifyDisplayed();
    }

    public static void verifySurveysBoxIsDisplayed() {
        surveysBox.verifyDisplayed();
    }

    public static void verifyPOAPBoxIsDisplayed() {
        pOAPBox.verifyDisplayed();
    }

    public static void clickReportsBox() {
        reportsBox.click();
    }

    public static void clickConfigurationBox() {
        configurationBox.click();
    }

    public static void clickUserManagementBox() {
        userManagementBox.click();
    }

    public static void clickSurveysBox() {
        surveysBox.click();
    }

    public static void clickPOAPBox() {
        pOAPBox.click();
    }

    public static void verifyHomePage() {
        homePageHeader.verifyDisplayed();
    }

    public static void clickHospitalDropdown() {
        dropDownHospital.click();
    }

    public static void selectHospitalByName(String hospital) {
        nameHospital(hospital).click();
    }

    public static void verifyHospitalIsNotDisplayed(String hospital) {
        nameHospital(hospital).verifyNotDisplayed();
    }

}
