Feature: POAPs

  @UI @TestRails(13931)
  Scenario: C13931 - Verify successful creation of a POAP form
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to POAPs page from menu navigation
    Then user is successfully navigated to POAP page
    When user create a new POAP
    Then the user should be in POAP form

  @UI @TestRails(13932)
  Scenario: C13932 - Verify the creation of a POAP with an existing patient ID
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
      | Patient ID     | Date of birth | Gender | Ethnicity | Assessment Date | Assessment Time | Surgery Date | Surgery Time |
      | Automation9999 | null          | null   | English   | null            | null            | null         | null         |
    Then patient date of birth is populated with "1999"
    And "Male" Gender is selected
    When user proceed to medical team tab
    Then medical team tab is active

  @UI @TestRails(13933)
  Scenario: C13933 - Verify the creation of a new patient ID that is not in the database
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
      | Automation-00001 |          2000 | Male   | English   | null            | null            | null         | null         |
    And user proceed to medical team tab
    Then medical team tab is active

  @UI @TestRails(13936)
  Scenario: C13936 - Verify successful selecting a surgeon specialty
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
      | Automation-00001 |          2000 | Male   | English   | null            | null            | null         | null         |
    And user proceed to medical team tab
    Then medical team tab is active
    When user select "Colorectal Surgery" specialty
    Then user select specialty is successful

  @UI @TestRails(13934)
  Scenario: C13934 - Verify successful selecting a surgeon name and anesthetist name
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
      | Automation-00001 |          2000 | Male   | English   | null            | null            | null         | null         |
    And user proceed to medical team tab
    Then medical team tab is active
    When user enter medical team details
      | Specialty          | Surgeon Name       | Anesthetist Name | Theater |
      | Colorectal Surgery | Automation Surgeon | Jon Dun          | null    |

  @UI @TestRails(13935)
  Scenario: C13935 - Verify successful adding a theater
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
      | Automation-00001 |          2000 | Male   | English   | null            | null            | null         | null         |
    And user proceed to medical team tab
    Then medical team tab is active
    When user enter medical team details
      | Specialty          | Surgeon Name       | Anesthetist Name | Theater      |
      | Colorectal Surgery | Automation Surgeon | Jon Dun          | 99999 / Test |
    And user proceed to risks-1 tab
    Then risks-1 tab is active

  @UI @TestRails(13941)
  Scenario: C13941 - Verify successful change of a blood pressure
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
      | Automation-00001 |          2000 | Male   | English   | null            | null            | null         | null         |
    And user proceed to medical team tab
    Then medical team tab is active
    When user enter medical team details
      | Specialty          | Surgeon Name       | Anesthetist Name | Theater      |
      | Colorectal Surgery | Automation Surgeon | Jon Dun          | 99999 / Test |
    And user proceed to risks-1 tab
    Then risks-1 tab is active
    When user select blood pressure "Hypertension Stage 2"
    And user proceed to risks-2 tab
    Then risks-2 tab is active

  @UI @TestRails(13942)
  Scenario: C13942 - Verify successful change of BMI
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
      | Automation-00001 |          2000 | Male   | English   | null            | null            | null         | null         |
    And user proceed to medical team tab
    Then medical team tab is active
    When user enter medical team details
      | Specialty          | Surgeon Name       | Anesthetist Name | Theater      |
      | Colorectal Surgery | Automation Surgeon | Jon Dun          | 99999 / Test |
    And user proceed to risks-1 tab
    Then risks-1 tab is active
    When user select BMI "25 - 30"
    And user proceed to risks-2 tab
    Then risks-2 tab is active

  @UI @TestRails(13943)
  Scenario: C13943 - Verify successful change of smoker status
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
      | Automation-00001 |          2000 | Male   | English   | null            | null            | null         | null         |
    And user proceed to medical team tab
    Then medical team tab is active
    When user enter medical team details
      | Specialty          | Surgeon Name       | Anesthetist Name | Theater      |
      | Colorectal Surgery | Automation Surgeon | Jon Dun          | 99999 / Test |
    And user proceed to risks-1 tab
    Then risks-1 tab is active
    When user check Smoker status
    And user proceed to risks-2 tab
    Then risks-2 tab is active
    And user proceed to patient preparation tab
    Then patient preparation tab is active
    When user proceed to procedure tab
    Then procedure tab is active
    And user "Smoker" risk is displayed

  @UI @TestRails(13960)
  Scenario: C13960 - Verify the new added risk group
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
      | Automation-00001 |          2000 | Male   | English   | null            | null            | null         | null         |
    And user proceed to medical team tab
    Then medical team tab is active
    When user enter medical team details
      | Specialty          | Surgeon Name       | Anesthetist Name | Theater      |
      | Colorectal Surgery | Automation Surgeon | Jon Dun          | 99999 / Test |
    And user proceed to risks-1 tab
    Then risks-1 tab is active
    When user proceed to risks-2 tab
    Then risks-2 tab is active
    And verify parent risk group
    When user expand parent risk "PREVIOUS INCISIONS"
    Then verify sub risk for previous incisions
    When user expand parent risk "DEFORMITIES"
    Then verify sub risk for deformities
    When user expand parent risk "ASA Class"
    And verify sub risk for ASA class
    When user expand parent risk "COVID-19 Classification"
    Then verify sub risk for covid-19 classification

  @UI @TestRails(13961)
  Scenario: C13961 - Verify successful selecting a risk
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
      | Automation-00001 |          2000 | Male   | English   | null            | null            | null         | null         |
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
    And user "Head and Neck" risk is displayed

  @UI @TestRails(13962)
  Scenario: C13962 - Verify successful selecting a whole risk group
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
      | Automation-00001 |          2000 | Male   | English   | null            | null            | null         | null         |
    And user proceed to medical team tab
    Then medical team tab is active
    When user enter medical team details
      | Specialty          | Surgeon Name       | Anesthetist Name | Theater      |
      | Colorectal Surgery | Automation Surgeon | Jon Dun          | 99999 / Test |
    And user proceed to risks-1 tab
    Then risks-1 tab is active
    When user proceed to risks-2 tab
    Then risks-2 tab is active
    When user select COVID-19 Classification risk parent group
    And user proceed to patient preparation tab
    Then patient preparation tab is active
    When user proceed to procedure tab
    Then procedure tab is active
    And user selected whole COVID-19 Classification risk parent is successful

  @UI @TestRails(13963)
  Scenario: C13963 - Verify successful deselecting risk
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
      | Automation-00001 |          2000 | Male   | English   | null            | null            | null         | null         |
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
    And user "Head and Neck" risk is displayed
    When user back to risks-2 tab
    When user deselect "PREVIOUS INCISIONS" parent risk group
    And user proceed to patient preparation tab
    And user proceed to procedure tab
    Then user successful deselect "Head and Neck" risk

  @UI @TestRails(13964)
  Scenario: C13964 - Verify successful searching a specific risk
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
      | Automation-00001 |          2000 | Male   | English   | null            | null            | null         | null         |
    And user proceed to medical team tab
    Then medical team tab is active
    When user enter medical team details
      | Specialty          | Surgeon Name       | Anesthetist Name | Theater      |
      | Colorectal Surgery | Automation Surgeon | Jon Dun          | 99999 / Test |
    And user proceed to risks-1 tab
    Then risks-1 tab is active
    When user proceed to risks-2 tab
    Then risks-2 tab is active
    When user enter "Head and Neck" risk
    Then "Head and Neck" risk is filtered in the list

  @UI @TestRails(13944)
  Scenario: C13944 - Verify successful changing a time
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
      | Automation-00001 |          2000 | Male   | English   | null            | null            | null         | null         |
    And user proceed to medical team tab
    Then medical team tab is active
    When user enter medical team details
      | Specialty          | Surgeon Name       | Anesthetist Name | Theater      |
      | Colorectal Surgery | Automation Surgeon | Jon Dun          | 99999 / Test |
    And user proceed to risks-1 tab
    Then risks-1 tab is active
    When user proceed to risks-2 tab
    Then risks-2 tab is active
    When user proceed to patient preparation tab
    Then patient preparation tab is active
    When user enter predicted time
      | WHO Surgical Safety Check List | Patient Positioning | Application of surgical drapes | Cleaning and sterilisation of skin | Marking skin site prior to procedure |
      |                              5 |                  10 |                             15 |                                 20 |                                   25 |
    Then predicted total time should be equal to "75"

  ##	When user proceed to patient preparation tab
  ##	Then patient preparation tab is active
  ##	When user proceed to procedure tab
  ##	Then procedure tab is active
  ##	And total procedure time must be equal to "75" for the predicted time
  @UI @TestRails(13970)
  Scenario: C13970 - Verify all specialties for colorectal surgery
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
      | Automation-00001 |          2000 | Male   | English   | null            | null            | null         | null         |
    And user proceed to medical team tab
    Then medical team tab is active
    When user enter medical team details
      | Specialty          | Surgeon Name       | Anesthetist Name | Theater      |
      | Colorectal Surgery | Automation Surgeon | Jon Dun          | 99999 / Test |
    And user proceed to risks-1 tab
    Then risks-1 tab is active
    When user proceed to risks-2 tab
    Then risks-2 tab is active
    When user proceed to patient preparation tab
    Then patient preparation tab is active
    When user proceed to procedure tab
    Then procedure tab is active
    When user select specialty
    Then user see all specialties for colorectal surgery

  @UI @TestRails(13988)
  Scenario: C13988 - Verify all specialties for orthopedic surgery
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
      | Automation-00001 |          2000 | Male   | English   | null            | null            | null         | null         |
    And user proceed to medical team tab
    Then medical team tab is active
    When user enter medical team details
      | Specialty           | Surgeon Name | Anesthetist Name | Theater      |
      | Orthopaedic Surgery | Tyler Dun    | Jon Dun          | 99999 / Test |
    And user proceed to risks-1 tab
    Then risks-1 tab is active
    When user proceed to risks-2 tab
    Then risks-2 tab is active
    When user proceed to patient preparation tab
    Then patient preparation tab is active
    When user proceed to procedure tab
    Then procedure tab is active
    When user select specialty
    Then user see all specialties for orthopaedic surgery

  ##should see 18 entries for orthopedic
  @UI @TestRails(13965)
  Scenario: C13965 - Verify successful adding all procedures
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
      | Automation-00001 |          2000 | Male   | English   | null            | null            | null         | null         |
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
      | Specialty             | Procedure method type | Predicted time | Procedure  |
      | Right colon structure | Open Wound Surgery    |             22 | Select all |
    Then "Set up camera" procedure is displayed

  @UI @TestRails(13966)
  Scenario: C13966 - Verify successful adding a procedure
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
      | Automation-00001 |          2000 | Male   | English   | null            | null            | null         | null         |
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
      | Specialty             | Procedure method type | Predicted time | Procedure      |
      | Right colon structure | Open Wound Surgery    |             22 | Port insertion |
    Then "Port insertion" procedure is displayed

  @UI @TestRails(13967)
  Scenario: C13967 - Verify deleting a procedure
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
      | Automation-00001 |          2000 | Male   | English   | null            | null            | null         | null         |
    And user proceed to medical team tab
    Then medical team tab is active
    When user enter medical team details
      | Specialty          | Surgeon Name       | Anesthetist Name | Theater      |
      | Colorectal Surgery | Automation Surgeon | null             | 99999 / Test |
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
      | Specialty             | Procedure method type | Predicted time | Procedure      |
      | Right colon structure | Open Wound Surgery    |             22 | Port insertion |
    Then "Port insertion" procedure is displayed
    When user delete "Port insertion" procedure
    Then "Port insertion" is deleted

  @UI @TestRails(13971) @Smoke
  Scenario: C13971 - Verify completing a POAP
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
      | Automation-00001 |          2000 | Male   | English   | null            | null            | null         | null         |
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
    When user enter predicted time
      | WHO Surgical Safety Check List | Patient Positioning | Application of surgical drapes | Cleaning and sterilisation of skin | Marking skin site prior to procedure |
      |                              5 |                  11 |                             15 |                                 21 |                                   25 |
    And user proceed to procedure tab
    Then procedure tab is active
    When user enter procedure details
      | Specialty             | Procedure method type | Predicted time | Procedure      |
      | Right colon structure | Open Wound Surgery    |             22 | Port insertion |
    Then total procedure time must be equal to "209" for the predicted time
    When user save the poap
    Then success message is displayed
    And user search patient id "Automation-00001" in POAPs
    Then patient id "Automation-00001" is displayed
    When user navigate to Surveys page from menu navigation
    Then user is successfully navigated to Surveys page
    When user uncheck display completed surveys
    And user search patient id "Automation-00001" in surveys page
    Then "Automation-00001" is displayed
    When user view "Automation-00001" survey
    Then user proceed to edit survey
    And user patient details should be displayed
      | Patient ID       | Patient Date of Birth Year | Surgery Date/Time | HospitalName  | Theater ID | Specialty          | Procedure             |
      | Automation-00001 |                       2000 | null              | Test Hospital |      99999 | Colorectal Surgery | Right colon structure |

  @UI @TestRails(13930) @Smoke
  Scenario: C13930 - Verify successful deletion of a POAP
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
      | Automation-00002 |          2000 | Male   | English   | null            | null            | null         | null         |
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
    When user search patient id "Automation-00002" in POAPs
    Then patient id "Automation-00002" is displayed
    When user navigate to Surveys page from menu navigation
    Then user is successfully navigated to Surveys page
    When user uncheck display completed surveys
    And user search patient id "Automation-00002" in surveys page
    Then "Automation-00002" is displayed
    When user navigate to POAPs page from menu navigation
    Then user is successfully navigated to POAP page
    When user search patient id "Automation-00002" in POAPs
    And user delete "Automation-00002" POAP
    Then the delete modal displayed
    When user select yes to delete POAP
    Then POAP deletion is successful
    When user search patient id "Automation-00002" in POAPs
    Then patient id "Automation-00002" is not displayed

  @UI @TestRails(13937)
  Scenario: C13937 - Verify successful filtering of a POAP
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to POAPs page from menu navigation
    Then user is successfully navigated to POAP page
    When user search patient id "Test2021A"
    Then user able to search patient id "Test2021A"
    When user search surgeon name "Automation Surgeon"
    Then user able to search surgeon name "Automation Surgeon"
    When user search anesthetist name "Jon Dun"
    Then user able to search anesthetist name "Jon Dun"
    When user search specialty "Colorectal Surgery"
    Then user able to search specialty "Colorectal Surgery"

  @UI @TestRails(13938) @Smoke
  Scenario: C13938 - Verify successful editing a POAP
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
      | Patient ID      | Date of birth | Gender | Ethnicity | Assessment Date | Assessment Time | Surgery Date | Surgery Time |
      | Automation-Test |          2000 | Male   | English   | null            | null            | null         | null         |
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
    When user enter predicted time
      | WHO Surgical Safety Check List | Patient Positioning | Application of surgical drapes | Cleaning and sterilisation of skin | Marking skin site prior to procedure |
      |                              5 |                  11 |                             15 |                                 21 |                                   25 |
    And user proceed to procedure tab
    Then procedure tab is active
    When user enter procedure details
      | Specialty             | Procedure method type | Predicted time | Procedure      |
      | Right colon structure | Open Wound Surgery    |             22 | Port insertion |
    Then total procedure time must be equal to "209" for the predicted time
    When user save the poap
    Then success message is displayed
    And user search patient id "Automation-Test" in POAPs
    Then patient id "Automation-Test" is displayed
    When user navigate to Surveys page from menu navigation
    Then user is successfully navigated to Surveys page
    When user uncheck display completed surveys
    And user search patient id "Automation-Test" in surveys page
    Then "Automation-Test" is displayed
    When user view "Automation-Test" survey
    Then user proceed to edit survey
    And user patient details should be displayed
      | Patient ID      | Patient Date of Birth Year | Surgery Date/Time | HospitalName  | Theater ID | Specialty          | Procedure             |
      | Automation-Test |                       2000 | null              | Test Hospital |      99999 | Colorectal Surgery | Right colon structure |
    When user navigate to POAPs page from menu navigation
    Then user is successfully navigated to POAP page
    And user search patient id "Automation-Test" in POAPs
    Then patient id "Automation-Test" is displayed
    When user edit "Automation-Test" POAP
    Then the user should be in POAP form
    When user enter patient details
      | Patient ID       | Date of birth | Gender | Ethnicity | Assessment Date | Assessment Time | Surgery Date | Surgery Time |
      | Automation-00007 |          1995 | Male   | English   | null            | null            | null         | null         |
    And user proceed to medical team tab
    Then medical team tab is active
    When user click procedure tab
    Then procedure tab is active
    When user enter procedure details
      | Specialty               | Procedure method type | Predicted time | Procedure |
      | Sigmoid colon structure | null                  | null           | null      |
    Then total procedure time must be equal to "209" for the predicted time
    When user save the poap
    Then success message is displayed
    And user search patient id "Automation-00007" in POAPs
    Then patient id "Automation-00007" is displayed
    When user navigate to Surveys page from menu navigation
    Then user is successfully navigated to Surveys page
    When user uncheck display completed surveys
    And user search patient id "Automation-00007" in surveys page
    Then "Automation-00007" is displayed
    When user view "Automation-00007" survey
    Then user proceed to edit survey
    And user patient details should be displayed
      | Patient ID       | Patient Date of Birth Year | Surgery Date/Time | HospitalName  | Theater ID | Specialty          | Procedure               |
      | Automation-00007 |                       1995 | null              | Test Hospital |      99999 | Colorectal Surgery | Sigmoid colon structure |
