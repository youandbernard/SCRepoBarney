package co.uk.pageobjects;

import org.openqa.selenium.By;

import co.uk.core.DriverHandler;
import co.uk.webelements.Button;
import co.uk.webelements.CheckBox;
import co.uk.webelements.Element;
import co.uk.webelements.TextBox;

public class SurveysPage {

	private static Element checkPatientID(String patinetID) {
		return new Element("Patient:" + patinetID, By.xpath("//td[contains(text(),'" + patinetID + "')]"));
	}

	private static Element headerSurveys = new Element("Survey Header", By.xpath("//h1[text()='Surveys']"));
	private static TextBox searchBox = new TextBox("Search", By.xpath("//label[contains(text(),'Search')]/input"));
	private static Button deleteButton(String patientId) {
		return new Button("Delete", By.xpath("(//td[text()='"+patientId+"']/following::td/button[contains(@class,'delete-survey')]/i[contains(@class,'trash')])[1]"));
	}

	private static Button viewButton(String patientId) {
		return new Button("View", By.xpath(
				"(//td[text()='"+patientId+"']/following::td/button[contains(@class,'view-survey')]/i[contains(@class,'fa-eye')])[1]"));
	}

	private static Element deleteMessage = new Element("Delete message",
			By.xpath("//div[@class='toast-message' and contains(text(),'Successfully deleted')]"));
	private static Element successMessage = new Element("Success message",
			By.xpath("//div[@class='toast-message' and contains(text(),'Saved successfully')]"));
	private static CheckBox completedSurvey = new CheckBox("CheckBox",
			By.xpath("//input[@id='checkboxDisplayforCompletedSurveyd']"));

	public static void checkCompletedSurvey() {
		if (!completedSurvey.isSelected()) {
			completedSurvey.click();
		}
	}

	public static void uncheckCompletedSurvey() {
		if (completedSurvey.isSelected()) {
			completedSurvey.click();
		}
	}

	public static void verifyDeleteMessage() {
		deleteMessage.verifyDisplayed();
	}

	public static void verifySuccessMessasge() {
		successMessage.verifyDisplayed();
	}

	public static void verifyPatientId(String patientID) {
		checkPatientID(patientID).verifyDisplayed();
	}

	public static void verifyPatientIdIsNotDisplayed(String patientID) {
		checkPatientID(patientID).verifyNotDisplayed();
	}

	public static void search(String search) {
		searchBox.setText(search);
		DriverHandler.delay(3);
	}

	public static void verifySurveyPage() {
		headerSurveys.verifyDisplayed();
	}

	public static void clickDeleteButton(String patientId) {
		deleteButton(patientId).click();
	}

	public static void clickViewButton(String patientId) {
		viewButton(patientId).click();
	}

	public static void verifyDeleteButtonIsNotDisplayed(String patientId) {
		deleteButton(patientId).verifyNotDisplayed();
	}

	public static class DeleteSurveryModal {
		private static Element modalDelete = new Element("Delete modal",
				By.xpath("//div[@class='swal-modal']/div[contains(text(),'Are you sure?')]"));
		private static Button yesButton = new Button("Yes",
				By.xpath("//button[contains(@class,'swal-button') and contains(text(),'Yes')]"));
		private static Button cancelButton = new Button("No",
				By.xpath("//button[contains(@class,'swal-button') and contains(text(),'Cancel')]"));

		public static void modalIsDisplayed() {
			modalDelete.verifyDisplayed();
		}

		public static void clickYes() {
			yesButton.click();
		}

		public static void clickCancel() {
			cancelButton.click();
		}
	}

}
