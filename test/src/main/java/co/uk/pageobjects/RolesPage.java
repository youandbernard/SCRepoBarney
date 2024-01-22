package co.uk.pageobjects;

import org.openqa.selenium.By;

import co.uk.webelements.Button;
import co.uk.webelements.Element;

public class RolesPage {

	private static Element pageHeader = new Element("Home page header", By.xpath("//h1[text()='Roles']"));
	private static Button buttonCreate = new Button("Create button",
			By.xpath("//section[contains(@class,'page-header')]//*[contains(text(),'Create')]"));

	public static void clickCreateButton() {
		buttonCreate.click();
	}

	public static void verifyRolesPage() {
	    pageHeader.verifyDisplayed();
	}

}
