package co.uk.pageobjects;

import org.openqa.selenium.By;

import co.uk.webelements.Button;
import co.uk.webelements.Element;
import co.uk.webelements.Tab;
import co.uk.webelements.TextBox;

public class PoapsPageMedicalTeamTab {
	private static Tab patientTab = new Tab("Patient tab", By.xpath("//a[@role='tab']/span[text()='Patient']"));
	private static Tab medicalteamTab = new Tab("Medical team tab",
			By.xpath("//a[@role='tab']/span[text()='Medical Team']"));
	private static Tab risk1Tab = new Tab("Risks-1 tab", By.xpath("//a[@role='tab']/span[text()='Risks-1']"));
	private static Tab risk2Tab = new Tab("Risks-2 tab", By.xpath("//a[@role='tab']/span[text()='Risks-2']"));
	private static Tab patientPreparationTab = new Tab("Patient preparation tab",
			By.xpath("//a[@role='tab']/span[text()='Patient Preparation']"));
	private static Tab procedureTab = new Tab("Procedure tab", By.xpath("//a[@role='tab']/span[text()='Procedure']"));
	private static Tab medicalTeamTabActive = new Tab("Medical Team Tab",
			By.xpath("//a[contains(@class,'nav-link active')]/span[contains(text(),'Medical Team')]"));
	private static Element specialtyDropdown = new Element("Specialty dropdown",
			By.xpath("//select[@name='Specialty']"));

	private static Element selectSpecialty(String specialty) {
		return new Element("Select "+specialty+" specialty", By.xpath("//option[contains(text(),'" + specialty + "')]"));
	}

	private static Element surgeonNameTextBox = new Element("Surgeon name textBox",
			By.xpath("//input[@name='SurgeonName']"));
	private static Element surgeonNameDropdownContainer = new Element("Surgeon name dropdown",
			By.xpath("//input[@id='SurgeonName']/following-sibling::typeahead-container"));

	private static Button select_SurgeonName(String surgeonName) {
		return new Button("Select Surgeon name: "+surgeonName,
				By.xpath("//button[contains(@class,'dropdown-item')]/span[contains(text(),'" + surgeonName + "')]"));
	}

	private static TextBox surgeonExperience = new TextBox("Surgeon experience",
			By.xpath("//input[@name='SugeonExperience']"));
	private static Element anesthetistDropdown = new Element("Anesthetist name",
			By.xpath("//input[@name='AnesthetistName']"));

	private static Element select_AnesthetistName(String anesthetistName) {
		return new Element("Select anaesthetist name: "+anesthetistName, By
				.xpath("//button[contains(@class,'dropdown-item')]/span[contains(text(),'" + anesthetistName + "')]"));
	}

	private static Element theaterDropdown = new Element("Theater ID", By.xpath("//input[@name='TheaterId']"));

	private static Element theaterOption(String theater) {
		return new Element("Theater ID: "+theater,
				By.xpath("//button[contains(@class,'dropdown-item')]/span[contains(text(),'" + theater + "')]"));
	}

	private static Button nextButton = new Button("Next button",
			By.xpath("//tab[@class='tab-pane active']//button[contains(text(),'Next')]"));

	public static void verifySurgeonExperience() {
		surgeonExperience.verifyTextContains("years");
	}

	public static void verifyMedicalTeamTabIsActive() {
		medicalTeamTabActive.verifyDisplayed();
	}

	public static void verifySurgeonDropdownMenu() {
		surgeonNameDropdownContainer.verifyDisplayed();
	}

	public static void clickSpecialtyDropdown() {
		specialtyDropdown.click();
	}

	public static void selectaSpecialty(String specialty) {
		selectSpecialty(specialty).click();
	}

	public static void clickPatientTab() {
		patientTab.click();
	}

	public static void clickMedicalTab() {
		medicalteamTab.click();
	}

	public static void clickRisk1Tab() {
		risk1Tab.click();
	}

	public static void clickRisk2Tab() {
		risk2Tab.click();
	}

	public static void clickPreparationTab() {
		patientPreparationTab.click();
	}

	public static void clickProcedureTab() {
		procedureTab.click();
	}

	public static void clickSurgeonDropdown() {
		surgeonNameTextBox.click();
	}

	public static void selectSurgeon(String surgeonName) {
		surgeonNameTextBox.click();
		select_SurgeonName(surgeonName).click();
	}

	public static void selectAnesthetist(String anesthetistName) {
		anesthetistDropdown.click();
		select_AnesthetistName(anesthetistName).click();
	}

	public static void selectTheater(String theater) {
		theaterDropdown.click();
		theaterOption(theater).click();
	}


	public static void clickNextButton() {
		nextButton.click();
	}

}
