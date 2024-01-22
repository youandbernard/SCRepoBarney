package co.uk.pageobjects;

import org.openqa.selenium.By;
import co.uk.webelements.Button;
import co.uk.webelements.Tab;
import co.uk.webelements.TextBox;

public class PoapsPagePatientPreparationTab {

	private static Tab patientPreparationTabActive = new Tab("Patient Preparation Active",
			By.xpath("//a[@class='nav-link active']/span[text()='Patient Preparation']"));
	private static TextBox whoCheckList_PredictedTime = new TextBox("WHO Checklist:Predicted Time",
			By.xpath("//input[@id='Risk_WHOCheckList_MeanTime']"));
	private static TextBox patientPositioning_PredictedTime = new TextBox("Patient Positioning: Predicted time",
			By.xpath("//input[@id='Risk_PatientPositioning_MeanTime']"));
	private static TextBox applicationOfSurgicalDrapes_PredictedTime = new TextBox(
			"Application of surgical drapes: Predicted Time",
			By.xpath("//label[@for='Risk_Draping_MeanTime']/following::input[@id='Risk_Draping_MeanTime'][1]"));
	private static TextBox cleaningAndSterilizationOfSkin_PredictedTime = new TextBox(
			"Cleaning and sterilization of skin: Predicted Time",
			By.xpath("//label[@for='Risk_Scrub_MeanTime']/following::input[@id='Risk_Draping_MeanTime']"));
	private static TextBox markingSkinSitePriorProcedure_PredictedTime = new TextBox(
			"Marking skin site prior to procedure: Predicted Time",
			By.xpath("//input[@id='Risk_Identification_MeanTime']"));
	private static TextBox totalTime = new TextBox("Total time", By.xpath(
			"//label[contains(text(),'Total Time')]/following::input[@id='Risk_Identification_StandardDeviation']"));
	private static Button nextButton = new Button("Next button",
			By.xpath("//tab[@class='tab-pane active']//button[contains(text(),'Next')]"));

	public static void enterPredictedTimeForWhoCheckList(String predictedTime) {
		whoCheckList_PredictedTime.setText(predictedTime);
	}

	public static void enterPredictedTimeForPatientPositioning(String predictedTime) {
		patientPositioning_PredictedTime.setText(predictedTime);
	}

	public static void enterPredictedTimeForApplicationOfSurgicalDrapes(String predictedTime) {
		applicationOfSurgicalDrapes_PredictedTime.setText(predictedTime);
	}

	public static void enterPredictedTimeForCleaningAndSterilizationOfSkin(String predictedTime) {
		cleaningAndSterilizationOfSkin_PredictedTime.setText(predictedTime);
	}

	public static void enterPredictedTimeForMarkingSkinSitePriorProcedure(String predictedTime) {
		markingSkinSitePriorProcedure_PredictedTime.setText(predictedTime);
	}

	public static void verifyPatientPreparationTabisActive() {
		patientPreparationTabActive.verifyDisplayed();
	}

	public static void verifyTotalTime(String total) {
		totalTime.verifyAttributeContains("value", total);
	}

	public static void clickNextButton() {
		nextButton.click();
	}
}
