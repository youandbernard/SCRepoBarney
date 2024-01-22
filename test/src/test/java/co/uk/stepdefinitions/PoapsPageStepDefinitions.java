package co.uk.stepdefinitions;

import java.io.IOException;
import java.util.List;
import java.util.Map;

import co.uk.core.DriverHandler;
import co.uk.pageobjects.PoapsPage;
import co.uk.pageobjects.PoapsPageMedicalTeamTab;
import co.uk.pageobjects.PoapsPagePatientPreparationTab;
import co.uk.pageobjects.PoapsPagePatientTab;
import co.uk.pageobjects.PoapsPageProcedureTab;
import co.uk.pageobjects.PoapsPageRisks1Tab;
import co.uk.pageobjects.PoapsPageRisks2Tab;
import io.cucumber.datatable.DataTable;
import io.cucumber.java.en.Then;
import io.cucumber.java.en.When;

public class PoapsPageStepDefinitions {
    @When("^user create a new POAP$")
    public void createPoap() {
        PoapsPage.clickCreateButton();
    }

    @When("^user proceed to medical team tab$")
    public void clickNextTOmedicalTab() {
        PoapsPagePatientTab.clickNextButton();
    }

    @When("^user proceed to risks-1 tab$")
    public void clickNextToRisk1Tab() {
        PoapsPageMedicalTeamTab.clickNextButton();
    }

    @When("^user proceed to risks-2 tab$")
    public void clickNextToRisk2Tab() {
        PoapsPageRisks1Tab.nextButton();
    }

    @When("^user back to risks-2 tab$")
    public void clickBackToRisk2Tab() {
        PoapsPageProcedureTab.clickRisk2Tab();
    }

    @When("^user proceed to patient preparation tab$")
    public void clickNextToPatientPreparationTab() {
        PoapsPageRisks2Tab.nextButton();
    }

    @When("^user proceed to procedure tab$")
    public void clickNextToProcedureTab() {
        PoapsPagePatientPreparationTab.clickNextButton();
    }

    @When("^user select \"(.*)\" specialty$")
    public void selectSpecialty(String specialty) {
        PoapsPageMedicalTeamTab.clickSpecialtyDropdown();
        PoapsPageMedicalTeamTab.selectaSpecialty(specialty);
    }

    @When("^user select blood pressure \"(.*)\"$")
    public void selectBloodPressure(String bloodpressure) {
        PoapsPageRisks1Tab.clickBloodPressureDropdown();
        PoapsPageRisks1Tab.verifyBloodPressureValues("Elevated");
        PoapsPageRisks1Tab.verifyBloodPressureValues("Normal");
        PoapsPageRisks1Tab.verifyBloodPressureValues("Hypertension Stage 1");
        PoapsPageRisks1Tab.verifyBloodPressureValues("Hypertension Stage 2");
        PoapsPageRisks1Tab.verifyBloodPressureValues("Hypertensive Crisis");
        PoapsPageRisks1Tab.selectBloodPressure(bloodpressure);

    }

    @When("^user select BMI \"(.*)\"$")
    public void selectBMI(String bmiValue) {
        PoapsPageRisks1Tab.clickBmiDropdown();

        PoapsPageRisks1Tab.verifyBmiValue("< 18.5");
        PoapsPageRisks1Tab.verifyBmiValue("18.5 - 25");
        PoapsPageRisks1Tab.verifyBmiValue("25 - 30");
        PoapsPageRisks1Tab.verifyBmiValue("30-35");
        PoapsPageRisks1Tab.verifyBmiValue("35-40");
        PoapsPageRisks1Tab.verifyBmiValue("> 40");
        PoapsPageRisks1Tab.selectBmi(bmiValue);
    }

    @When("^user check Smoker status$")
    public void changeSmokerStatus() {
        PoapsPageRisks1Tab.clickSmokerStatus();
    }

    @When("^user expand parent risk \"(.*)\"$")
    public void expandParentRisk(String parent) {
        PoapsPageRisks2Tab.expandAndCollapseArrow(parent);
    }

    @When("^user select \"(.*)\" risk from \"(.*)\"$")
    public void selectSubRisk(String subRisk, String parentRiskExpand) {
        PoapsPageRisks2Tab.expandAndCollapseArrow(parentRiskExpand);
        PoapsPageRisks2Tab.selectSubRisk(subRisk);
    }

    @When("^user select COVID-19 Classification risk parent group$")
    public void selectParentRisk() {
        PoapsPageRisks2Tab.selectParentRisk("COVID-19 Classification");
    }

    @When("^user deselect \"(.*)\" parent risk group$")
    public void deselectParentRisk(String parentRisk) {
        PoapsPageRisks2Tab.selectParentRisk(parentRisk);
        PoapsPageRisks2Tab.selectParentRisk(parentRisk);
    }

    @When("^user enter \"(.*)\" risk$")
    public void searchForRisk(String searchForRisk) {
        PoapsPageRisks2Tab.searchRisk(searchForRisk);
    }

    @When("^user select specialty$")
    public void selectSpecialty() {
        PoapsPageProcedureTab.clickSpecialtyDropdown();
    }

    @When("^user save the poap$")
    public void userSavePoap() {
        PoapsPageProcedureTab.clickSavePoap();
    }

    @When("^user search patient id \"(.*)\" in POAPs$")
    public void searchNewPatientId(String patientId) throws IOException {
        PoapsPage.enterSearch(patientId);
    }

    @When("^\"(.*)\" procedure is displayed")
    public void procedure(String procedureName) {
        PoapsPageProcedureTab.verifyProcedureNameIsDisplayed(procedureName);
    }

    @When("^user delete \"(.*)\" procedure$")
    public void deleteProcedure(String procedureName) {
        PoapsPageProcedureTab.clickDeleteProcedure(procedureName);
    }

    @When("^user delete \"(.*)\" POAP$")
    public void deletePoap(String patientID) {
        PoapsPage.clickDeleteButton(patientID);
    }

    @When("^user select yes to delete POAP$")
    public void yesToDeletePoap() throws IOException {
        PoapsPage.DeletePoapModal.clickYes();
        DriverHandler.delay(3);

    }

    @When("^user enter patient details$")
    public void enterPatientInformation(DataTable patientDetails) {
        List<Map<String, String>> data = patientDetails.asMaps(String.class, String.class);
        String patientId = data.get(0).get("Patient ID");
        String dateOfBirth = data.get(0).get("Date of birth");
        String gender = data.get(0).get("Gender");
        String ethnicity = data.get(0).get("Ethnicity");
        if (!patientId.equals("null")) {
            PoapsPagePatientTab.clickPatientId();
            DriverHandler.delay(1);
            PoapsPagePatientTab.enterPatientID(patientId);
            DriverHandler.delay(3);
            PoapsPagePatientTab.selectPatientId(patientId);
        }
        if (!dateOfBirth.equals("null")) {
            PoapsPagePatientTab.enterPatientDoB(dateOfBirth);
        }
        if (!gender.equals("null")) {
            PoapsPagePatientTab.clickPatientGenderDropdown();
            PoapsPagePatientTab.selectPatientGender(gender);
        }
        if (!ethnicity.equals("null")) {
            PoapsPagePatientTab.clickEthnicityDropdown();
            PoapsPagePatientTab.selectEthnicity(ethnicity);
        }
        PoapsPagePatientTab.clickAssessmentDateTime();
        PoapsPagePatientTab.clickSaveButton();
        PoapsPagePatientTab.clickSurgeryDateTime();
        PoapsPagePatientTab.clickSaveButton();
    }

    @When("^user enter medical team details$")
    public void enterMedicalTeamInformation(DataTable medicalTeamDetails) {
        List<Map<String, String>> data = medicalTeamDetails.asMaps(String.class, String.class);
        String specialty = data.get(0).get("Specialty");
        String surgeonName = data.get(0).get("Surgeon Name");
        String anesthetist = data.get(0).get("Anesthetist Name");
        String theater = data.get(0).get("Theater");
        if (!specialty.equals("null")) {
            PoapsPageMedicalTeamTab.selectaSpecialty(specialty);
        }
        if (!surgeonName.equals("null")) {
            PoapsPageMedicalTeamTab.selectSurgeon(surgeonName);
        }
        if (!anesthetist.equals("null")) {
            PoapsPageMedicalTeamTab.selectAnesthetist(anesthetist);
        }
        if (!theater.equals("null")) {
            PoapsPageMedicalTeamTab.selectTheater(theater);
        }
    }

    @When("^user enter procedure details$")
    public void enterProcedureInformation(DataTable procedureDetails) {
        List<Map<String, String>> data = procedureDetails.asMaps(String.class, String.class);
        String specialty = data.get(0).get("Specialty");
        String procedureMethodType = data.get(0).get("Procedure method type");
        String predictedTime = data.get(0).get("Predicted time");
        String procedure = data.get(0).get("Procedure");
        if (!specialty.equals("null")) {
            PoapsPageProcedureTab.clickSpecialtyDropdown();
            PoapsPageProcedureTab.selectSpecialty(specialty);
        }
        if (!procedureMethodType.equals("null")) {
            PoapsPageProcedureTab.clickProcedureMethodTypeDropdown();
            PoapsPageProcedureTab.selectProcedureMethodType(procedureMethodType);
        }
        if (!predictedTime.equals("null")) {
            PoapsPageProcedureTab.enterPredictedTime_For_ZeroMin(predictedTime);
        }
        if (!procedure.equals("null") && !procedure.equals("Select all")) {
            PoapsPageProcedureTab.clickAddProcedure();
            PoapsPageProcedureTab.AddProcedureModal.verifyModalProcedures();
            DriverHandler.delay(5);
            PoapsPageProcedureTab.AddProcedureModal.enterSearchBox(procedure);
            PoapsPageProcedureTab.AddProcedureModal.selectProcedure(procedure);
            PoapsPageProcedureTab.AddProcedureModal.clickSave();
            PoapsPageProcedureTab.enterPredictedTime_For_ZeroMin(predictedTime);
        }
        if (procedure.equals("Select all")) {
            PoapsPageProcedureTab.clickAddProcedure();
            PoapsPageProcedureTab.AddProcedureModal.verifyModalProcedures();
            DriverHandler.delay(5);
            PoapsPageProcedureTab.AddProcedureModal.clickSelectAll();
            PoapsPageProcedureTab.AddProcedureModal.clickSave();
        }

    }

    @When("^user enter predicted time$")
    public void enterPredictedtimeOnPreparationTab(DataTable PredictedtimeOnPreparationTab) {
        List<Map<String, String>> data = PredictedtimeOnPreparationTab.asMaps(String.class, String.class);
        String whoCheckList = data.get(0).get("WHO Surgical Safety Check List");
        String patientPositioning = data.get(0).get("Patient Positioning");
        String applicationOfSurgicalDrapes = data.get(0).get("Application of surgical drapes");
        String cleaningAndSterilizationOfSkin = data.get(0).get("Cleaning and sterilisation of skin");
        String markingSkinSitePriorProcedure = data.get(0).get("Marking skin site prior to procedure");
        if (!whoCheckList.equals("null")) {
            PoapsPagePatientPreparationTab.enterPredictedTimeForWhoCheckList(whoCheckList);
        }
        if (!patientPositioning.equals("null")) {
            PoapsPagePatientPreparationTab.enterPredictedTimeForPatientPositioning(patientPositioning);
        }
        if (!applicationOfSurgicalDrapes.equals("null")) {
            PoapsPagePatientPreparationTab.enterPredictedTimeForApplicationOfSurgicalDrapes(applicationOfSurgicalDrapes);
        }
        if (!cleaningAndSterilizationOfSkin.equals("null")) {
            PoapsPagePatientPreparationTab.enterPredictedTimeForCleaningAndSterilizationOfSkin(cleaningAndSterilizationOfSkin);
        }
        if (!markingSkinSitePriorProcedure.equals("null")) {
            PoapsPagePatientPreparationTab.enterPredictedTimeForMarkingSkinSitePriorProcedure(markingSkinSitePriorProcedure);
        }
    }

    @Then("^the user should be in POAP form$")
    public void verifyCreateForm() {
        PoapsPagePatientTab.verifyCreateAssessmentPage();
        DriverHandler.delay(5);
    }

    @Then("^medical team tab is active$")
    public void verifyMedicalTeamTab() {
        PoapsPageMedicalTeamTab.verifyMedicalTeamTabIsActive();
    }

    @Then("^risks-1 tab is active$")
    public void verifyRisks_1Tab() {
        PoapsPageRisks1Tab.verifyRisks1TabIsActive();
    }

    @Then("^risks-2 tab is active$")
    public void verifyRisks_2Tab() {
        PoapsPageRisks2Tab.verifyRisk_2TabisActive();
    }

    @Then("^patient preparation tab is active$")
    public void verifyPatientPreparationTab() {
        PoapsPagePatientPreparationTab.verifyPatientPreparationTabisActive();
    }

    @Then("^procedure tab is active$")
    public void verifyProcedureTab() {
        PoapsPageProcedureTab.verifyProcedureTabisActive();
    }

    @Then("^user is successfully navigated to POAP page$")
    public void verifyPoapsPage() {
        PoapsPage.verifyPOAPsPage();
        DriverHandler.delay(2);
    }

    @Then("^verify parent risk group$")
    public void verifyParentRisk() {
        PoapsPageRisks2Tab.verifyParentRisk("PREVIOUS INCISIONS [Surgery] 363565009");
        PoapsPageRisks2Tab.verifyParentRisk("DEFORMITIES 298390003");
        PoapsPageRisks2Tab.verifyParentRisk("ASA Class");
        PoapsPageRisks2Tab.verifyParentRisk("COVID-19 Classification");
    }

    @Then("^verify sub risk for previous incisions$")
    public void verifySubRiskForPreviousIncisions() {
        PoapsPageRisks2Tab.verifySubRisk("Head and Neck");
        PoapsPageRisks2Tab.verifySubRisk("Thorax");
        PoapsPageRisks2Tab.verifySubRisk("Abdomen");
        PoapsPageRisks2Tab.verifySubRisk("Upper Extremity");
        PoapsPageRisks2Tab.verifySubRisk("Lower Extremity");

    }

    @Then("^verify sub risk for deformities$")
    public void verifySubRiskForDeormities() {
        PoapsPageRisks2Tab.verifySubRisk("Neck deformity (difficulty of intubation)");
        PoapsPageRisks2Tab.verifySubRisk("Burn injury (difficulty of intubation)");
    }

    @Then("^verify sub risk for ASA class")
    public void verifySubRiskForASAclass() {
        PoapsPageRisks2Tab.verifySubRisk("ASA1: Normal Healthy");
        PoapsPageRisks2Tab.verifySubRisk("ASA2: Patient with mild systemic disease");
        PoapsPageRisks2Tab.verifySubRisk("ASA3: Patient with severe systemic disease");
        PoapsPageRisks2Tab.verifySubRisk("ASA4: Patient with severe systemic disease that is a threat to constant life");
        PoapsPageRisks2Tab.verifySubRisk("ASA5: Moribund patient who is not expected to survive without the operation");
    }

    @Then("^verify sub risk for covid-19 classification")
    public void verifySubRiskForCovid19() {
        PoapsPageRisks2Tab.verifySubRisk("Class 1: Previous history of COVID-19");
        PoapsPageRisks2Tab.verifySubRisk("Class 2: No history of COVID-19");
    }

    @Then("^user \"(.*)\" risk is displayed$")
    public void verifySelectedRiskOnProcedureTab(String riskName) {
        PoapsPageProcedureTab.verifyRiskNameIsDisplayed(riskName);
    }

    @Then("^user successful deselect \"(.*)\" risk$")
    public void verifyDeseleteRiskOnProcedureTab(String riskName) {
        PoapsPageProcedureTab.verifyRiskNameIsNotDisplayed(riskName);
    }

    @Then("^user select specialty is successful$")
    public void verifySurgeonDropdown() {
        PoapsPageMedicalTeamTab.clickSurgeonDropdown();
        PoapsPageMedicalTeamTab.verifySurgeonDropdownMenu();
    }

    @Then("^surgeon years of experience should be displayed$")
    public void verifySurgeonExperiencePopulated() {
        PoapsPageMedicalTeamTab.verifySurgeonExperience();
    }

    @Then("^\"(.*)\" risk is filtered in the list")
    public void verifyFileterdRiskList(String subRisk) {
        PoapsPageRisks2Tab.verifySubRisk(subRisk);
    }

    @Then("^user see all specialties for colorectal surgery$")
    public void verifySpecialtiesForColerectalSurgery() {
        PoapsPageProcedureTab.verifySpecialty("Right colon structure");
        PoapsPageProcedureTab.verifySpecialty("Sigmoid colon structure");
        PoapsPageProcedureTab.verifySpecialty("Laparoscopic cholecystectomy");
    }

    @Then("^user see all specialties for orthopaedic surgery$")
    public void verifySpecialtiesForOrthopaedic() {
        PoapsPageProcedureTab.verifySpecialty("Open Reduction and Internal Fixation");
        PoapsPageProcedureTab.verifySpecialty("Arthroscopy");
        PoapsPageProcedureTab.verifySpecialty("Removal of internal fixators");
        PoapsPageProcedureTab.verifySpecialty("Carpal tunnel release");
        PoapsPageProcedureTab.verifySpecialty("Intermedullary nailing");
        PoapsPageProcedureTab.verifySpecialty("Wound revision");
        PoapsPageProcedureTab.verifySpecialty("Soft tissue Exploration of the hand");
        PoapsPageProcedureTab.verifySpecialty("Cervicocapital prosthesis");
        PoapsPageProcedureTab.verifySpecialty("ACL reconstruction");
        PoapsPageProcedureTab.verifySpecialty("Resection and reconstruction of the clavicle");
        PoapsPageProcedureTab.verifySpecialty("Bicondylar knee prosthesis");
        PoapsPageProcedureTab.verifySpecialty("Total Endoprostheses Placement");
        PoapsPageProcedureTab.verifySpecialty("Decompression and adhesiolysis of peripheral nerve");
        PoapsPageProcedureTab.verifySpecialty("Correctional osteotomy of the Tibia");
        PoapsPageProcedureTab.verifySpecialty("Achilles tendon reconstruction");
        PoapsPageProcedureTab.verifySpecialty("External fixators");
        PoapsPageProcedureTab.verifySpecialty("Haematoma evacuation");
        PoapsPageProcedureTab.verifySpecialty("Exostosis Removal");
    }

    @Then("^poap creation is successful$")
    public void verifySuccessfulMessaeg(String patientID) {
        PoapsPageProcedureTab.verifySavedMessage();
        DriverHandler.delay(1);
        // DriverHandler.refreshPage();
    }

    @Then("user selected whole COVID-19 Classification risk parent is successful")
    public void verifyCovid19Risk() {
        PoapsPageProcedureTab.verifyRiskNameIsDisplayed("Class 1: Previous history of COVID-19");
        PoapsPageProcedureTab.verifyRiskNameIsDisplayed("Class 2: No history of COVID-19");
    }

    @Then("^predicted total time should be equal to \"(.*)\"$")
    public void verifyTotalTime(String total) {
        PoapsPagePatientPreparationTab.verifyTotalTime(total);
    }

    @Then("^\"(.*)\" is deleted$")
    public void verifyDeletedProcedure(String procedureName) {
        PoapsPageProcedureTab.verifyProcedureNameIsNotDisplayed(procedureName);
    }

    @Then("^success message is displayed$")
    public void verifySuccessMessage() throws IOException {
        PoapsPage.verifySuccessMessage();
        DriverHandler.delay(1);
        DriverHandler.refreshPage();
    }

    @Then("^the delete modal displayed$")
    public void verifyDeleteModalIsDisplayed() {
        PoapsPage.DeletePoapModal.modalIsDisplayed();
    }

    @Then("^POAP deletion is successful$")
    public void verifyDeletePoapMessage() throws IOException {
        PoapsPage.verifyDeleteMessage();
        DriverHandler.delay(1);
        DriverHandler.refreshPage();
    }

    @Then("^patient id \"(.*)\" is displayed$")
    public void verifyNewCreatedPoap(String patientID) {
        PoapsPage.verifySearchResultIsDisplayed(patientID);
    }

    @When("^user edit \"(.*)\" POAP$")
    public void editPoap(String patientID) {
        PoapsPage.clickEditButton(patientID);
    }

    @Then("^patient id \"(.*)\" is not displayed$")
    public void verifyDeletedPoap(String patientID) {
        DriverHandler.delay(3);
        PoapsPage.verifySearchResultIsNotDisplayed(patientID);
    }

    @When("^user search patient id \"(.*)\"$")
    public void searchPatientId(String patientID) {
        PoapsPage.enterSearch(patientID);
        DriverHandler.delay(2);
    }

    @Then("^user able to search patient id \"(.*)\"$")
    public void verifySearchByPatientId(String patientID) {
        PoapsPage.verifySearchResultIsDisplayed(patientID);
        DriverHandler.delay(2);
    }

    @When("^user search surgeon name \"(.*)\"$")
    public void searchSurgeonName(String surgeonName) {
        PoapsPage.enterSearch(surgeonName);
        DriverHandler.delay(2);
    }

    @Then("^user able to search surgeon name \"(.*)\"$")
    public void verifySearchBySurgeonName(String surgeonName) {
        PoapsPage.verifySearchResultIsDisplayed(surgeonName);
        DriverHandler.delay(2);
    }

    @When("^user search anesthetist name \"(.*)\"$")
    public void searchAnesthetisName(String anesthetistName) {
        PoapsPage.enterSearch(anesthetistName);
        DriverHandler.delay(2);
    }

    @Then("^user able to search anesthetist name \"(.*)\"$")
    public void verifySearchByAnesthetistName(String anesthetist) {
        PoapsPage.verifySearchResultIsDisplayed(anesthetist);
        DriverHandler.delay(2);
    }

    @When("^user search specialty \"(.*)\"$")
    public void searchSpecialty(String specialty) {
        PoapsPage.enterSearch(specialty);
        DriverHandler.delay(2);
    }

    @Then("^user able to search specialty \"(.*)\"$")
    public void verifySearchBySpecialty(String specialty) {
        PoapsPage.verifySearchResultIsDisplayed(specialty);
        DriverHandler.delay(2);
    }

    @Then("^total procedure time must be equal to \"(.*)\" for the predicted time$")
    public void verifyTotalProcedureTimeForPredictedTime(String total) {
        PoapsPageProcedureTab.verifyTotalTimeForPredicted(total);
    }

    @Then("^patient date of birth is populated with \"(.*)\"$")
    public void verifyPatientDoBiSPopulated(String dob) {
        PoapsPagePatientTab.verifyPatientDob(dob);
    }

    @Then("^\"(.*)\" Gender is selected$")
    public void verifyGenderIsSelected(String gender) {
        PoapsPagePatientTab.verifyPatientGender(gender);
    }

    @When("^user click procedure tab$")
    public void clickProcedureTab() {
        PoapsPageMedicalTeamTab.clickProcedureTab();
    }
}
