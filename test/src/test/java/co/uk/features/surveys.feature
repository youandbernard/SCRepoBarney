Feature: surveys

  @UI @TestRails(13955)
  Scenario: C13955 - Verify the survey forms linked to the selected hospital
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Surveys page from menu navigation
    Then user is successfully navigated to Surveys page
    When user search patient id "Test2021A" in surveys page
    Then patient id "Test2021A" is displayed in surveys page
    When user navigate to Dashboard page from menu navigation
    Then user is successfully navigated to home page
    When user select "THE PARK HOSPITAL" from hospital dropdown
    Then "THE PARK HOSPITAL" is displayed in the header
    When user navigate to Surveys page from menu navigation
    Then user is successfully navigated to Surveys page
    When user search patient id "Test2021C" in surveys page
    Then patient id "Test2021C" is displayed in surveys page

  @UI @TestRails(13958)
  Scenario: C13958 - Verify cancel deleting a survey as admin
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Surveys page from menu navigation
    Then user is successfully navigated to Surveys page
    When user search patient id "Test2021A" in surveys page
    Then patient id "Test2021A" is displayed in surveys page
    When user delete "Test2021A" survey
    Then delete modal is displayed
    When user select cancel to delete a survey
    And user search patient id "Test2021A" in surveys page
    Then "Test2021A" survey cancel deletion is successful

  @UI @TestRails(13946) @Smoke
  Scenario: C13946 - Verify successful deletion of a survey as admin
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to POAPs page from menu navigation
    Then user is successfully navigated to POAP page
    When user create a new POAP
    Then the user should be in POAP form
    When user enter patient details
      | Patient ID       | Date of birth | Gender | Ethnicity | Assessment Date | Assessment Time | Surgery Date | Surgery Time |
      | Automation-00003 |          2000 | Male   | English   | null            | null            | null         | null         |
    And user proceed to medical team tab
    Then medical team tab is active
    When user enter medical team details
      | Specialty          | Surgeon Name       | Anesthetist Name | Theater      |
      | Colorectal Surgery | Automation Surgeon | Jon Dun          | 99999 / Test |
    And user proceed to risks-1 tab
    Then risks-1 tab is active
    When user proceed to risks-2 tab
    Then risks-2 tab is active
    When user select "Head and Neck" risk from "PREVIOUS INCISIONS"
    And user proceed to patient preparation tab
    Then patient preparation tab is active
    When user proceed to procedure tab
    Then procedure tab is active
    When user enter procedure details
      | Specialty             | Procedure method type | Predicted time | Procedure |
      | Right colon structure | Open Wound Surgery    |             22 | null      |
    And user save the poap
    Then success message is displayed
    And user search patient id "Automation-00003" in POAPs
    Then patient id "Automation-00003" is displayed
    When user navigate to Surveys page from menu navigation
    Then user is successfully navigated to Surveys page
    When user search patient id "Automation-00003" in surveys page
    Then patient id "Automation-00003" is displayed in surveys page
    When user delete "Automation-00003" survey
    Then delete modal is displayed
    When user select yes to delete a survey
    Then survey deletion is successful
    When user search patient id "Automation-00003" in surveys page
    Then patient id "Automation-00003" is not displayed in surveys page

  @UI @TestRails(13953)
  Scenario: C13953 - Verify unsuccessful deletion of a survey if not admin
    Given user is in casemix login page
    When user login as "surgeon"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Surveys page from menu navigation
    Then user is successfully navigated to Surveys page
    And "Test2021A" should not see a delete button

  @UI @TestRails(13959)
  Scenario: C13959 - Verify successful adding surgeon/nurse notes
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to POAPs page from menu navigation
    Then user is successfully navigated to POAP page
    When user create a new POAP
    Then the user should be in POAP form
    When user enter patient details
      | Patient ID       | Date of birth | Gender | Ethnicity | Assessment Date | Assessment Time | Surgery Date | Surgery Time |
      | Automation-00004 |          2000 | Male   | English   | null            | null            | null         | null         |
    And user proceed to medical team tab
    Then medical team tab is active
    When user enter medical team details
      | Specialty          | Surgeon Name       | Anesthetist Name | Theater      |
      | Colorectal Surgery | Automation Surgeon | Jon Dun          | 99999 / Test |
    And user proceed to risks-1 tab
    Then risks-1 tab is active
    When user proceed to risks-2 tab
    Then risks-2 tab is active
    When user select "Head and Neck" risk from "PREVIOUS INCISIONS"
    And user proceed to patient preparation tab
    Then patient preparation tab is active
    When user proceed to procedure tab
    Then procedure tab is active
    When user enter procedure details
      | Specialty             | Procedure method type | Predicted time | Procedure |
      | Right colon structure | Open Wound Surgery    |             22 | null      |
    And user save the poap
    Then success message is displayed
    And user search patient id "Automation-00004" in POAPs
    Then patient id "Automation-00004" is displayed
    When user navigate to Surveys page from menu navigation
    Then user is successfully navigated to Surveys page
    When user search patient id "Automation-00004" in surveys page
    Then patient id "Automation-00004" is displayed in surveys page
    When user view "Automation-00004" survey
    Then user proceed to edit survey
    When user enter "Test" surgeon notes
    And user save surgeon notes
    When user close edit survey
    Then user is successfully navigated to Surveys page
    When user search patient id "Automation-00004" in surveys page
    Then patient id "Automation-00004" is displayed in surveys page
    When user view "Automation-00004" survey
    Then user proceed to edit survey
    And surgeon notes is populated with "Test"

  @UI @TestRails(13945)
  Scenario: C13945 - Verify successful showing completed surveys
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Surveys page from menu navigation
    Then user is successfully navigated to Surveys page
    When user uncheck display completed surveys
    And user search patient id "Test-Complete-0000" in surveys page
    Then patient id "Test-Complete-0000" is not displayed in surveys page
    When user check display completed surveys
    And user search patient id "Test-Complete-0000" in surveys page
    Then patient id "Test-Complete-0000" is displayed in surveys page

  @UI @TestRails(13956)
  Scenario: C13956 - Verify searching a survey
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Surveys page from menu navigation
    Then user is successfully navigated to Surveys page
    When user uncheck display completed surveys
    And user search patient id "Test2021A" in surveys page
    Then patient id "Test2021A" is displayed in surveys page
    When user check display completed surveys
    And user search patient id "Test-Complete-0000" in surveys page
    Then patient id "Test-Complete-0000" is displayed in surveys page
