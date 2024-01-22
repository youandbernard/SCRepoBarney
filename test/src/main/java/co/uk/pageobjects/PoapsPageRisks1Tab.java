package co.uk.pageobjects;

import org.openqa.selenium.By;

import co.uk.webelements.Button;
import co.uk.webelements.Element;
import co.uk.webelements.Tab;
import co.uk.webelements.TextBox;

public class PoapsPageRisks1Tab {
	private static Tab risks_1TabActive = new Tab("Risk_1 Tab",
			By.xpath("//a[@class='nav-link active']/span[text()='Risks-1']"));
	private static Element bloodPressureDropdown = new Element("Blood pressure dropdown",
			By.xpath("//select[@name='Risk_BloodPressure_Value']"));

	private static Element bloodPressureValue(String bloodpressure) {
		return new Element("Blood pressure dropdown: "+bloodpressure, By
				.xpath("//select[@name='Risk_BloodPressure_Value']/option[contains(text(),'" + bloodpressure + "')]"));
	}

	private static Element bmiDropdown = new Element("BMI dropdown", By.xpath("//select[@name='Risk_BMI_Value']"));

	private static Element bmiValue(String bmiValue) {
		return new Element("BMI Value",
				By.xpath("//select[@name='Risk_BMI_Value']/option[contains(text(),'" + bmiValue + "')]"));
	}

	private static TextBox gender = new TextBox("Gender", By.xpath("//input[@name='Risk_Gender_Value']"));
	private static TextBox age = new TextBox("Age", By.xpath("//input[@name='Risk_Age_Value']"));
	private static TextBox ethnicity = new TextBox("Ethnicity", By.xpath("//input[@name='Risk_Ethnicity_Value']"));
	private static Button smokerToggle = new Button("Smoker Toggle",
			By.xpath("//input[@name='isSmoker']/following::span[contains(@class,'slider')]"));
	private static Button nextButton = new Button("Next button",
			By.xpath("//tab[@class='tab-pane active']//button[contains(text(),'Next')]"));

	public static void clickBloodPressureDropdown() {
		bloodPressureDropdown.click();
	}
	
	public static void verifyBloodPressureValues(String bloodpressure) {
		bloodPressureValue(bloodpressure).verifyDisplayed();
	}

	public static void selectBloodPressure(String bloodPressure) {
		bloodPressureValue(bloodPressure).click();
	}
	
	public static void clickBmiDropdown() {
		bmiDropdown.click();
	}
	public static void selectBmi(String bmi) {
		bmiValue(bmi).click();
	}
	public static void verifyBmiValue(String bmi) {
		bmiValue(bmi).verifyDisplayed();
	}
	public static void verifyRisks1TabIsActive() {
		risks_1TabActive.verifyDisplayed();
	}

	public static void nextButton() {
		nextButton.click();
	}
	public static void clickSmokerStatus() {
		smokerToggle.click();
	}
}
