package co.uk.pageobjects;

import org.openqa.selenium.By;

import co.uk.webelements.Button;
import co.uk.webelements.Element;
import co.uk.webelements.Tab;
import co.uk.webelements.TextBox;

public class PoapsPagePatientTab {
	private static Tab patientTab = new Tab("Patient tab", By.xpath("//a[@role='tab']/span[text()='Patient']"));
	private static Tab medicalteamTab = new Tab("Medical team tab",
			By.xpath("//a[@role='tab']/span[text()='Medical Team']"));
	private static Tab risk1Tab = new Tab("Risks-1 tab", By.xpath("//a[@role='tab']/span[text()='Risks-1']"));
	private static Tab risk2Tab = new Tab("Risks-2 tab", By.xpath("//a[@role='tab']/span[text()='Risks-2']"));
	private static Tab patientPreparationTab = new Tab("Patient preparation tab",
			By.xpath("//a[@role='tab']/span[text()='Patient Preparation']"));
	private static Tab procedureTab = new Tab("Procedure tab", By.xpath("//a[@role='tab']/span[text()='Procedure']"));
	private static Element headerCreatePoaps = new Element("Header Create assessment",
			By.xpath("//h1[text()='Create Assessment']"));
	private static TextBox patientId = new TextBox("Patient id", By.xpath("//input[@name='patientId']"));
	private static Element clickPatientId = new Element("Click Patient id Textbox",
			By.xpath("//input[@name='patientId']"));
	private static Element patientIdDropdown(String patientId) {
		return new Element("Select Patient id dropdown",
				By.xpath("//button[contains(@class, 'dropdown-item')]/span/strong[text()='" + patientId + "']"));
	}
	private static TextBox patientDob = new TextBox("Patient DoB", By.xpath("//input[@name='PatientDobYear']"));
	private static Element patientgenderDropdown = new Element("Patient gender dropdown",
			By.xpath("//select[@name='PatientGender']"));

	private static Element patientgender(String gender) {
		return new Element("Gender", By.xpath("//option[contains(text(),'" + gender + "')]"));
	}

	private static Element ethnicityDropdown = new Element("Ethnicity dropdown",
			By.xpath("//select[@name='EthnicityID']"));

	private static Element ethincity(String ethnicity) {
		return new Element("Select ethnicity", By.xpath("//option[contains(text(),'" + ethnicity + "')]"));
	}

	private static Element assessmentDateTime = new Element("Assessment Date/Time",
			By.xpath("//input[@name='AssessmentDate']/following::button[1]"));
	private static Element surgeryDateTime = new Element("Surgery Date/Time",
			By.xpath("//input[@name='SurgeryDate']/following::button[1]"));
	private static Button nextButton = new Button("Next button",
			By.xpath("//tab[@class='tab-pane active']//button[contains(text(),'Next')]"));

	public static void verifyCreateAssessmentPage() {
		headerCreatePoaps.verifyDisplayed();
	}
	

	public static void enterPatientID(String patientid) {
		patientId.setText(patientid);
	}

	public static void clickPatientId() {
		clickPatientId.click();
	}

	public static void selectPatientId(String patientid) {
		if (patientIdDropdown(patientid).isDisplayed()) {
			patientIdDropdown(patientid).click();
		}
	}

	public static void enterPatientDoB(String patientDoB) {
		patientDob.setText(patientDoB);
	}

	public static void verifyPatientDob(String dateOfBirth) {
		patientDob.verifyAttributeEquals("value", dateOfBirth);
	}
	public static void verifyPatientGender(String gender) {
		patientgender(gender).verifySelected();
	}

	public static void clickAssessmentDateTime() {
		assessmentDateTime.click();
	}

	public static void verifyAssessmentDateTime() {
		if (assessmentDateTime.getText() != null) {
			assessmentDateTime.verifyDisplayed();
		}
	}

	public static void clickSurgeryDateTime() {
		surgeryDateTime.click();
	}

	public static void verifySurgeryDateTime() {
		if (surgeryDateTime.getText() != null) {
			surgeryDateTime.verifyDisplayed();
		}
	}

	public static void clickPatientGenderDropdown() {
		patientgenderDropdown.click();
	}

	public static void selectPatientGender(String gender) {
		patientgender(gender).click();
	}

	public static void verifyPatientId() {
		if (patientId.getText() != null) {

		} else {
			patientId.verifyEmpty();
		}
	}

	public static void clickEthnicityDropdown() {
		ethnicityDropdown.click();
	}

	public static void selectEthnicity(String ethnicity) {
		ethincity(ethnicity).click();
	}

	public static void verifyEthnicity(String ethnicity) {
		ethincity(ethnicity).isSelected();
	}

	public static void clickNextButton() {
		nextButton.click();
	}

	private static Element modal = new Element("Change Date and time modal", By.xpath("//div[@class='modal-content']"));
	private static Button saveButton = new Button("Save button", By.xpath("//button[@type='submit']"));
	private static TextBox hour = new TextBox("Hour", By.xpath("//input[@placeholder='HH']"));
	private static TextBox minute = new TextBox("Minute", By.xpath("//input[@placeholder='MM']"));
	private static Button amPM = new Button("AM / PM", By.xpath("//button[@class='btn btn-default text-center']"));
	private static Button date = new Button("Date", By.xpath("//input[@name='Date']"));
	private static Button cancelButton = new Button("Cancel button", By.xpath("//button[contains(text(),'Cancel')]"));

	public static void verifyDateTimeModal() {
		modal.verifyDisplayed();
	}

	public static void clickSaveButton() {
		saveButton.click();
	}
}
