package co.uk.stepdefinitions;

import java.io.IOException;
import java.util.List;
import java.util.Map;

import co.uk.core.DateUtilities;
import co.uk.core.DriverHandler;
import co.uk.pageobjects.SurveysPage;
import co.uk.pageobjects.SurveysPageEditSurvey;
import io.cucumber.datatable.DataTable;
import io.cucumber.java.en.Then;
import io.cucumber.java.en.When;

public class SurveysPageStepDefinitions {

	@When("^user search patient id \"(.*)\" in surveys page$")
	public void searchPatientId(String patientID) {
		SurveysPage.search(patientID);
	}

	@When("^user delete \"(.*)\" survey$")
	public void clickDelete(String patientId) {
		SurveysPage.clickDeleteButton(patientId);
	}

	@Then("^delete modal is displayed$")
	public void verifyDeleteModalIsDisplayed() {
		SurveysPage.DeleteSurveryModal.modalIsDisplayed();
	}

	@When("^user select yes to delete a survey$")
	public void yesToDeleteSurvey() {
		SurveysPage.DeleteSurveryModal.clickYes();
	}

	@When("^user select cancel to delete a survey$")
	public void cancelDeleteSurvey() throws IOException {
		SurveysPage.DeleteSurveryModal.clickCancel();
		DriverHandler.refreshPage();
	}

	@Then("^\"(.*)\" is displayed$")
	public void verifyNewPatientId(String patientID) {
		SurveysPage.verifyPatientId(patientID);

	}

	@Then("^patient id \"(.*)\" is displayed in surveys page$")
	public void verifyPatientId(String patientID) {
		SurveysPage.verifyPatientId(patientID);
		DriverHandler.delay(2);
	}

	@Then("^user is successfully navigated to Surveys page$")
	public void verifySurveysPage() {
		SurveysPage.verifySurveyPage();
		DriverHandler.delay(1);
	}

	@Then("^\"(.*)\" survey cancel deletion is successful$")
	public void verifyDeletionCancel(String patientID) {
		SurveysPage.verifyPatientId(patientID);
	}

	@Then("^survey deletion is successful$")
	public void verifyDeleteIsSuccessful() throws IOException {
		SurveysPage.verifyDeleteMessage();
		DriverHandler.delay(1);
		DriverHandler.refreshPage();

	}

	@Then("^\"(.*)\" should not see a delete button$")
	public void verifyDeleteButtonIsNotDisplayed(String patientId) {
		SurveysPage.verifyDeleteButtonIsNotDisplayed(patientId);
	}

	@When("^user view \"(.*)\" survey$")
	public void viewSurvey(String patientId) {
		SurveysPage.clickViewButton(patientId);
	}

	@Then("^user proceed to edit survey$")
	public void verifyEditSurveyIsDisplayed() {
		SurveysPageEditSurvey.verifyEditSurvey();
	}

	@When("^user set start time$")
	public void setStartTime() {
		SurveysPageEditSurvey.clickSetStartTime();
		DriverHandler.delay(5);
	}

	@Then("^user set start time is successful$")
	public void verifySetTime() {
		SurveysPageEditSurvey.verifySuccessfulMessage();
		SurveysPageEditSurvey.verifyPatientInformation("Time of Arrival in OR", DateUtilities.getTimeStamp());
		SurveysPageEditSurvey.verifySetStartTimeIsNotDisplayed();
		SurveysPageEditSurvey.verifyStopwatchIsEnable();
	}

	@When("^user start a procedure$")
	public void startProcedure() {
		SurveysPageEditSurvey.clickStopwatch();
	}

	@Then("^procedure modal timer is displayed$")
	public void verifyProcedureModalTimerIsDisplayed() {
		SurveysPageEditSurvey.modal.verifyModalHeaderIsDisplayed();
	}

	@Then("^large start button is displayed$")
	public void verifyStartButtonIsDisplayed() {
		SurveysPageEditSurvey.modal.verifyStartButtonIsDisplayed();
	}

	@Then("^timer time 00:00:00 is displayed$")
	public void verifyTimerTimeIsDisplayed() {
		SurveysPageEditSurvey.modal.verifyTimerIs0Min();
	}

	@When("^user start a timer of the procedure$")
	public void startTimerProcedure() {
		SurveysPageEditSurvey.modal.clickStart();
	}

	@Then("^user successfully start a timer of the procedure$")
	public void verifyStartTimerProcedure() {
		SurveysPageEditSurvey.modal.verifyStopButtonIsDisplayed();
		DriverHandler.delay(5);
		SurveysPageEditSurvey.modal.verifyTimerIsNot0Min();
	}

	@When("^user stop a timer of the procedure$")
	public void stopTimerProcedure() {
		SurveysPageEditSurvey.modal.clickStop();
	}

	@Then("^saved successful message is displayed$")
	public void verifySuccessfulMessageIsDisplayed() {
		SurveysPageEditSurvey.verifySuccessfulMessage();
	}

	@When("^user complete a survey$")
	public void completeSurvey() {
		SurveysPageEditSurvey.clickCompleteSurvey();
	}

	@Then("^successful message is displayed$")
	public void verifySuccessfulMessage() throws IOException {
		SurveysPageEditSurvey.verifySuccessfulMessage();
		DriverHandler.delay(3);
		DriverHandler.refreshPage();
	}

	@When("^user check display completed surveys$")
	public void checkCompletedSurveys() {

		SurveysPage.checkCompletedSurvey();
		DriverHandler.delay(5);
	}

	@Then("^patient id \"(.*)\" is not displayed in surveys page$")
	public void verifyPatientIdIsNotDisplayed(String patientID) {
		SurveysPage.verifyPatientIdIsNotDisplayed(patientID);
	}

	@When("^user enter \"(.*)\" surgeon notes$")
	public void enterSurgeonNotes(String note) {
		SurveysPageEditSurvey.enterSurgeonNurseNotes(note);
	}

	@When("^user save surgeon notes$")
	public void saveSurgeonNotes() {
		SurveysPageEditSurvey.clickSaveNotes();
	}

	@When("^user close edit survey$")
	public void closeEditSurvey() {
		SurveysPageEditSurvey.clickClose();
		DriverHandler.delay(3);
	}

	@Then("^surgeon notes is populated with \"(.*)\"$")
	public void verifySurgeonNotesIsPopulated(String note) {
		SurveysPageEditSurvey.verifySurgeonNurseNotes(note);
	}

	@Then("^user unsuccessful complete a survey$")
	public void verifyCannotCompleteSurvey() {
		SurveysPageEditSurvey.verifyCompleteSurveyButtonIsDisable();
	}

	@Then("^user patient details should be displayed$")
	public void verifyPatientDetails(DataTable patientDetails) {
		DriverHandler.delay(6);
		List<Map<String, String>> data = patientDetails.asMaps(String.class, String.class);
		String patientID = data.get(0).get("Patient ID");
		String patientDoBYear = data.get(0).get("Patient Date of Birth Year");
		String surgeryDateAndtime = data.get(0).get("Surgery Date/Time");
		String hospitalName = data.get(0).get("HospitalName");
		String theaterID = data.get(0).get("Theater ID");
		String specialty = data.get(0).get("Specialty");
		String procedure = data.get(0).get("Procedure");
		if (!patientID.equals("null")) {
			SurveysPageEditSurvey.verifyPatientInformation("Patient ID", patientID);
		}
		if (!patientDoBYear.equals("null")) {
			SurveysPageEditSurvey.verifyPatientInformation("Patient DoB Year", patientDoBYear);
		}
		if (!surgeryDateAndtime.equals("null")) {
			SurveysPageEditSurvey.verifyPatientInformation("Surgery Date/Time", surgeryDateAndtime);
		}
		if (!hospitalName.equals("null")) {
			SurveysPageEditSurvey.verifyPatientInformation("HospitalName", hospitalName);
		}
		if (!theaterID.equals("null")) {
			SurveysPageEditSurvey.verifyPatientInformation("Theater ID", theaterID);
		}
		if (!specialty.equals("null")) {
			SurveysPageEditSurvey.verifyPatientInformation("Specialty", specialty);
		}
		if (!procedure.equals("null")) {
			SurveysPageEditSurvey.verifyPatientInformation("Procedure", procedure);
		}
	}

	@When("^user completing all procedure$")
	public void completeAllProcedure() {
		while (SurveysPageEditSurvey.find0MinActualTime.isDisplayed()) {
			SurveysPageEditSurvey.clickStopwatch();
			SurveysPageEditSurvey.modal.verifyModalHeaderIsDisplayed();
			SurveysPageEditSurvey.modal.verifyStartButtonIsDisplayed();
			SurveysPageEditSurvey.modal.verifyTimerIs0Min();
			SurveysPageEditSurvey.modal.clickStart();
			SurveysPageEditSurvey.modal.verifyStopButtonIsDisplayed();
			DriverHandler.delay(10);
			SurveysPageEditSurvey.modal.clickStop();
			DriverHandler.delay(5);
			SurveysPageEditSurvey.verifySuccessfulMessage();
		}

	}

	@When("^user uncheck display completed surveys$")
	public void uncheckDisplayCompletedSurveys() {
		SurveysPage.uncheckCompletedSurvey();
	}

}
