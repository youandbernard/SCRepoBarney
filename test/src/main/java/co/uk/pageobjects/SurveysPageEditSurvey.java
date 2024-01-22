package co.uk.pageobjects;

import org.openqa.selenium.By;
import co.uk.webelements.Button;
import co.uk.webelements.Element;
import co.uk.webelements.TextBox;

public class SurveysPageEditSurvey {
	private static Element editSurveyHeader = new Element("Edit survey header", By.xpath("//h1[text()='Edit Survey']"));
	private static Button setStartTime = new Button("Set start time",
			By.xpath("//a[contains(@class,'set-start-time') and contains(text(),'Set Start Time')]"));
	private static Button saveNotes = new Button("Save notes",
			By.xpath("//button[contains(@class,'btn') and contains(text(),'Save Notes')]"));
	private static Button closeButton = new Button("Close", By.xpath("//button[contains(text(),'Close')]"));
	private static Button completeSurvey = new Button("Complete survey",
			By.xpath("//button[contains(text(),'Complete Survey')]"));
	private static Button stopWatch = new Button("Stopwatch",
			By.xpath("(//button[contains(@class,'btn')]/i[contains(@class,'stopwatch')])[1]"));
	private static Button copyButton = new Button("Copy",
			By.xpath("(//button[contains(@class,'btn')]/i[contains(@class,'copy')])[1]"));
	private static TextBox surgeonNurseNotes = new TextBox("Surgeon/Nurse Notes",
			By.xpath("//textarea[@id='ObserverNotes']"));
	private static Element successMessage = new Element("Successful message",
			By.xpath("//div[@class='toast-message' and contains(text(),'Saved successfully')]"));
	public static Element find0MinActualTime = new Element("Find 0 Min Actual Time",
			By.xpath("(//tbody/tr/td[5][contains(text(),'0 min')])[1]"));

	private static Element patientInformation(String information, String Data) {
		return new Element(information,
				By.xpath("//td[contains(text(),'" + information + "')]/following-sibling::td[contains(text(),'" + Data + "')]"));
	}

	public static void verifyDuplicateButtonIsDisplayed() {
		copyButton.verifyDisplayed();
	}

	public static void verifyCompleteSurveyButtonIsDisable() {
		completeSurvey.verifyDisabled();
	}

	public static void verifySuccessfulMessage() {
		successMessage.verifyDisplayed();
	}

	public static void enterSurgeonNurseNotes(String note) {
		surgeonNurseNotes.setText(note);
	}

	public static void verifySurgeonNurseNotes(String note) {
		surgeonNurseNotes.verifyTextEquals(note);
	}

	public static void clickSaveNotes() {
		saveNotes.click();
	}

	public static void verifyStopwatchIsEnable() {
		stopWatch.isEnabled();
	}

	public static void clickStopwatch() {
		stopWatch.click();
	}

	public static void clickCopy() {
		copyButton.click();
	}

	public static void clickCompleteSurvey() {
		completeSurvey.click();
	}

	public static void clickClose() {
		closeButton.click();
	}

	public static void verifyEditSurvey() {
		editSurveyHeader.verifyDisplayed();
	}

	public static void clickSetStartTime() {
		setStartTime.click();
	}

	public static void verifySetStartTimeIsNotDisplayed() {
		setStartTime.verifyNotDisplayed();
	}

	public static void verifyPatientInformation(String information, String patientData) {
		patientInformation(information, patientData).verifyDisplayed();
	}

	public static class modal {
		private static Element modalHeader = new Element("Modal header", By.xpath("//div[@class='modal-header']/h4"));
		private static Button startButton = new Button("Start",
				By.xpath("//button[contains(@class,'btn') and contains(text(),'Start')]"));
		private static Button stopButton = new Button("Stop",
				By.xpath("//button[contains(@class,'btn') and contains(text(),'Stop')]"));
		private static Element timer = new Element("Timer",
				By.xpath("//button/following-sibling::h1[contains(text(),'00:00:00')]"));

		public static void verifyModalHeaderIsDisplayed() {
			modalHeader.verifyDisplayed();
		}

		public static void clickStart() {
			startButton.click();
		}

		public static void verifyStartButtonIsDisplayed() {
			startButton.verifyDisplayed();
		}

		public static void verifyStopButtonIsDisplayed() {
			stopButton.verifyDisplayed();
		}

		public static void clickStop() {
			stopButton.click();
		}

		public static void verifyTimerIs0Min() {
			timer.verifyDisplayed();
		}

		public static void verifyTimerIsNot0Min() {
			timer.verifyNotDisplayed();
		}
	}

}
