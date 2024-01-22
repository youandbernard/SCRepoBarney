package co.uk.pageobjects;

import org.openqa.selenium.By;

import co.uk.webelements.Button;
import co.uk.webelements.CheckBox;
import co.uk.webelements.Element;
import co.uk.webelements.Tab;
import co.uk.webelements.TextBox;

public class PoapsPageProcedureTab {
	private static Tab risk2Tab = new Tab("Risks-2 tab", By.xpath("//a[@role='tab']/span[text()='Risks-2']"));
	private static Tab procedureTabActive = new Tab("Procedure Tab active",
			By.xpath("//a[@class='nav-link active']/span[text()='Procedure']"));
	private static Button addButton = new Button("Add",
			By.xpath("//button[contains(@class,'btn') and contains(text(),'Add')]"));
	private static Element specialtyDropdown = new Element("Speciality dropdown",
			By.xpath("//select[@name='BodyStructure']"));
	private static Element totalTimePredictedTime = new Element("Total Time: Predicted time",
			By.xpath("//td/b[contains(text(),'Total Procedure Time')]/following::td[1]"));

	private static Element specialtyOption(String specialty) {
		return new Element("Select " + specialty + " speciality",
				By.xpath("//option[contains(text(),'" + specialty + "')]"));
	}

	private static Element procedureMethodType = new Element("Procedure Method Typetextbox",
			By.xpath("//input[@id='procedureMethodTypeName']"));

	private static Button procedureMethodType_Option(String procedureMethodType) {
		return new Button("Procedure Method Type: " + procedureMethodType,
				By.xpath("//button/span[contains(text(),'" + procedureMethodType + "')]"));
	}

	private static Element procedureMethodType_Selected(String procedureMethodType) {
		return new Element(procedureMethodType + " Procedure Method type",
				By.xpath("//input[@id='procedureMethodTypeName']/following::div[contains(text(),'" + procedureMethodType
						+ "')]"));
	}

	private static Button saveButton = new Button("Save", By.xpath("//button[contains(text(),'Save')]"));

	private static Element find0MinPredictedTime = new Element("Verify Predicted time",
			By.xpath("(//p-celleditor[contains(text(),'0 min')])[1]"));

	private static TextBox predictedTime_Input = new TextBox("Predicted Time input",
			By.xpath("//input[contains(@class,'edit-procedure-input')]"));

	private static Button editButton = new Button("Edit button",
			By.xpath("(//p-celleditor[contains(text(),'0 min')]/following::*[contains(@class,'pencil')])[1]"));

	private static Button checkButton = new Button("Check", By.xpath("//td/button/i[contains(@class,'fa-check')]"));

	private static Element riskName(String riskName) {
		return new Element("Risk Name: " + riskName,
				By.xpath("//tbody[@class='ui-table-tbody']/tr/td[contains(text(),'RISK - " + riskName + "')]"));
	}

	private static Button deleteButton(String procedureName) {
		return new Button(procedureName + " delete", By.xpath("//td[contains(text(),'" + procedureName
				+ "')]/following-sibling::td[3]//i[contains(@class,'trash')]"));
	}

	private static Element subProcedure(String procedureName) {
		return new Element(procedureName,
				By.xpath("//tbody[@class='ui-table-tbody']/tr/td[contains(text(),'" + procedureName + "')]"));
	}

	private static Element successfulToastMessage = new Element("Saved Message",
			By.xpath("//div[@class='toast-message' and contains(text(),'Saved successfully')]"));

	public static void verifyTotalTimeForPredicted(String total) {
		totalTimePredictedTime.verifyTextContains(total);
	}

	public static void verifyProcedureTabisActive() {
		procedureTabActive.verifyDisplayed();

	}

	public static void verifyProcedureNameIsDisplayed(String procedureName) {
		subProcedure(procedureName).verifyDisplayed();
	}

	public static void verifyProcedureNameIsNotDisplayed(String procedureName) {
		subProcedure(procedureName).verifyNotDisplayed();
	}

	public static void verifyRiskNameIsDisplayed(String riskName) {
		riskName(riskName).verifyDisplayed();
	}

	public static void verifyRiskNameIsNotDisplayed(String riskName) {
		riskName(riskName).verifyNotDisplayed();
	}

	public static void verifySpecialty(String specialty) {
		specialtyOption(specialty).verifyDisplayed();
	}

	public static void clickSpecialtyDropdown() {
		specialtyDropdown.click();
	}

	public static void selectSpecialty(String specialty) {
		specialtyOption(specialty).click();
	}

	public static void clickProcedureMethodTypeDropdown() {
		procedureMethodType.click();
	}

	public static void selectProcedureMethodType(String procedureMethodType) {
		procedureMethodType_Option(procedureMethodType).click();
	}

	public static void verifyProcedureMethodTypeIsSelected(String procedureMethodType) {
		procedureMethodType_Selected(procedureMethodType).verifyDisplayed();
	}

	public static void clickRisk2Tab() {
		risk2Tab.click();
	}

	public static void clickSavePoap() {
		saveButton.click();
	}

	public static void verifySavedMessage() {
		successfulToastMessage.verifyDisplayed();
	}

	public static void clickAddProcedure() {
		addButton.click();
	}

	public static void clickDeleteProcedure(String procedureName) {
		deleteButton(procedureName).click();
	}

	public static void enterPredictedTime_For_ZeroMin(String predictedTime) {

		while (find0MinPredictedTime.isDisplayed()) {
			editButton.click();
			predictedTime_Input.setText(predictedTime);
			checkButton.click();
		}

	}

	public static class AddProcedureModal {
		private static CheckBox selectAll = new CheckBox("Select all",
				By.xpath("//input[@name='isCheckedSelectAll']/following::div[@role='checkbox']"));
		private static Element modalProcedures = new Element("Modal Procedures",
				By.xpath("//h4[text()='Procedures' and @class='modal-title']"));
		private static Button saveButton = new Button("Save",
				By.xpath("//div[contains(@class,'modal-footer')]/button[contains(text(),'Save')]"));
		private static Button cancelButton = new Button("Cancel",
				By.xpath("//div[contains(@class,'modal-footer')]/button[contains(text(),'Cancel')]"));
		private static TextBox searchBox = new TextBox("Search",
				By.xpath("//app-procedure-search//input[contains(@class,'ui-tree-filter')]"));

		private static CheckBox procedure(String procedure) {
			return new CheckBox(procedure, By.xpath("//div[contains(@aria-label,'" + procedure + "')]"));
		}

		public static void clickSelectAll() {
			selectAll.click();

		}

		public static void clickSave() {
			saveButton.click();
		}

		public static void clickCancel() {
			cancelButton.click();
		}

		public static void verifyModalProcedures() {
			modalProcedures.verifyDisplayed();
		}

		public static void selectProcedure(String procedure) {
			procedure(procedure).click();
		}

		public static void enterSearchBox(String procedure) {
			searchBox.setTextAndEnter(procedure);
		}

		public static void verifyProcedure(String procedure) {
			procedure(procedure).verifyDisplayed();
		}

	}

}
