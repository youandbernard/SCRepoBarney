Feature: User Management

  @UI @Smoke @TestRails(13901)
  Scenario: C13901 - Verify successful creating a surgeon
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname  | Username     | Password     | ConfirmPassword | EmailAddress          | IsActive |
      | Auto | Surgeon1 | autoSurgeon1 | Password@123 | Password@123    | autosurgeon1@test.com | Yes      |
    And user proceed to User roles tab
    And user select role "Surgeon"
    And user select experience "6 - 7 years"
    And user proceed to User Hospital tab
    And user select hospital "Test Hospital"
    And user proceed to Specialties tab
    And user select a specialty "Orthopaedic Surgery"
    And user save the new user profile
    Then user details are saved successfully
    When user search user "autoSurgeon1" with IsActive status "Yes"
    Then user "autoSurgeon1" is displayed
    When user logout
    Then user is in login page
    When user enter username "autoSurgeon1" and password "Password@123"
    Then user is successfully login

  @UI @Smoke @TestRails(13903)
  Scenario: C13903 - Verify successful creating an anesthetists
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname       | Username          | Password     | ConfirmPassword | EmailAddress               | IsActive |
      | Auto | Anaesthetist1 | autoAnaesthetist1 | Password@123 | Password@123    | autoanaesthetist1@test.com | Yes      |
    And user proceed to User roles tab
    And user select role "Anaesthetist"
    And user proceed to User Hospital tab
    And user select hospital "Test Hospital"
    And user save the new user profile
    Then user details are saved successfully
    When user search user "autoAnaesthetist1" with IsActive status "Yes"
    Then user "autoAnaesthetist1" is displayed
    When user logout
    Then user is in login page
    When user enter username "autoAnaesthetist1" and password "Password@123"
    Then user is successfully login

  @UI @Smoke @TestRails(13904)
  Scenario: C13904 - Verify successful creating an admin
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname | Username   | Password     | ConfirmPassword | EmailAddress        | IsActive |
      | Auto | Admin1  | autoAdmin1 | Password@123 | Password@123    | autoadmin1@test.com | Yes      |
    And user proceed to User roles tab
    And user select role "Admin"
    And user proceed to User Hospital tab
    And user select hospital "Test Hospital"
    And user save the new user profile
    Then user details are saved successfully
    When user search user "autoAdmin1" with IsActive status "Yes"
    Then user "autoAdmin1" is displayed
    When user logout
    Then user is in login page
    When user enter username "autoAdmin1" and password "Password@123"
    Then user is successfully login

  @UI @TestRails(13915)
  Scenario: C13915 - Verify successful creating a user with surgeon and anesthetist
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname | Username   | Password     | ConfirmPassword | EmailAddress        | IsActive |
      | Auto | Mixed1  | autoMixed1 | Password@123 | Password@123    | automixed1@test.com | Yes      |
    And user proceed to User roles tab
    And user select role "Surgeon"
    And user select experience "6 - 7 years"
    And user select role "Anaesthetist"
    And user proceed to User Hospital tab
    And user select hospital "Test Hospital"
    And user proceed to Specialties tab
    And user select a specialty "Orthopaedic Surgery"
    And user save the new user profile
    Then user details are saved successfully
    When user search user "autoMixed1" with IsActive status "Yes"
    Then user "autoMixed1" is displayed
    When user logout
    Then user is in login page
    When user enter username "autoMixed1" and password "Password@123"
    Then user is successfully login

  @UI @TestRails(13916)
  Scenario: C13916 - Verify successful creating a user with surgeon, anesthetist and admin
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname | Username   | Password     | ConfirmPassword | EmailAddress        | IsActive |
      | Auto | Mixed2  | autoMixed2 | Password@123 | Password@123    | automixed2@test.com | Yes      |
    And user proceed to User roles tab
    And user select role "Surgeon"
    And user select experience "6 - 7 years"
    And user select role "Admin"
    And user select role "Anaesthetist"
    And user proceed to User Hospital tab
    And user select hospital "Test Hospital"
    And user proceed to Specialties tab
    And user select a specialty "Orthopaedic Surgery"
    And user save the new user profile
    Then user details are saved successfully
    When user search user "autoMixed2" with IsActive status "Yes"
    Then user "autoMixed2" is displayed
    When user logout
    Then user is in login page
    When user enter username "autoMixed2" and password "Password@123"
    Then user is successfully login

  @UI @TestRails(13902)
  Scenario: C13902 - Verify the successful creation of an inactive surgeon
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user search user "autoSurgeon2" with IsActive status "All"
    And user delete the user if existing
    And user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname  | Username     | Password     | ConfirmPassword | EmailAddress          | IsActive |
      | Auto | Surgeon2 | autoSurgeon2 | Password@123 | Password@123    | autosurgeon2@test.com | No       |
    And user proceed to User roles tab
    And user select role "Surgeon"
    And user select experience "6 - 7 years"
    And user proceed to User Hospital tab
    And user select hospital "Test Hospital"
    And user proceed to Specialties tab
    And user select a specialty "Orthopaedic Surgery"
    And user save the new user profile
    Then user details are saved successfully
    When user search user "autoSurgeon2" with IsActive status "No"
    Then user "autoSurgeon2" is displayed
    When user logout
    Then user is in login page
    When user enter username "autoSurgeon2" and password "Password@123"
    Then login failed message is displayed for inactive user "autoSurgeon2"

  @UI @TestRails(13905)
  Scenario: C13905 - Verify the successful creation of an inactive anesthetists
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user search user "autoAnaesthetist2" with IsActive status "All"
    And user delete the user if existing
    And user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname       | Username          | Password     | ConfirmPassword | EmailAddress               | IsActive |
      | Auto | Anaesthetist2 | autoAnaesthetist2 | Password@123 | Password@123    | autoanaesthetist2@test.com | No       |
    And user proceed to User roles tab
    And user select role "Anaesthetist"
    And user proceed to User Hospital tab
    And user select hospital "Test Hospital"
    And user save the new user profile
    Then user details are saved successfully
    When user search user "autoAnaesthetist2" with IsActive status "No"
    Then user "autoAnaesthetist2" is displayed
    When user logout
    Then user is in login page
    When user enter username "autoAnaesthetist2" and password "Password@123"
    Then login failed message is displayed for inactive user "autoAnaesthetist2"

  @UI @TestRails(13906)
  Scenario: C13906 - Verify the successful creation of an inactive admin
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user search user "autoAdmin2" with IsActive status "All"
    And user delete the user if existing
    And user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname | Username   | Password     | ConfirmPassword | EmailAddress        | IsActive |
      | Auto | Admin2  | autoAdmin2 | Password@123 | Password@123    | autoadmin2@test.com | No       |
    And user proceed to User roles tab
    And user select role "Admin"
    And user proceed to User Hospital tab
    And user select hospital "Test Hospital"
    And user save the new user profile
    Then user details are saved successfully
    When user search user "autoAdmin2" with IsActive status "Yes"
    Then user "autoAdmin2" is displayed
    When user logout
    Then user is in login page
    When user enter username "autoAdmin2" and password "Password@123"
    Then login failed message is displayed for inactive user "autoAdmin2"

  @UI @TestRails(13908)
  Scenario: C13908 - Verify successful creating of a surgeon without hospital
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname  | Username     | Password     | ConfirmPassword | EmailAddress          | IsActive |
      | Auto | Surgeon3 | autoSurgeon3 | Password@123 | Password@123    | autosurgeon3@test.com | Yes      |
    And user proceed to User roles tab
    And user select role "Surgeon"
    And user select experience "6 - 7 years"
    And user proceed to Specialties tab
    And user select a specialty "Orthopaedic Surgery"
    And user save the new user profile
    Then user details are saved successfully
    When user search user "autoSurgeon3" with IsActive status "Yes"
    Then user "autoSurgeon3" is not displayed
    When user logout
    Then user is in login page
    When user enter username "autoSurgeon3" and password "Password@123"
    Then user is successfully login
    And hospital name is not displayed in the header

  @UI @TestRails(13909)
  Scenario: C13909 - Verify successful creating of an anesthetist without hospital
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname       | Username          | Password     | ConfirmPassword | EmailAddress               | IsActive |
      | Auto | Anaesthetist3 | autoAnaesthetist3 | Password@123 | Password@123    | autoanaesthetist3@test.com | Yes      |
    And user proceed to User roles tab
    And user select role "Anaesthetist"
    And user save the new user profile
    Then user details are saved successfully
    When user search user "autoAnaesthetist3" with IsActive status "Yes"
    Then user "autoAnaesthetist3" is not displayed
    When user logout
    Then user is in login page
    When user enter username "autoAnaesthetist3" and password "Password@123"
    Then user is successfully login
    And hospital name is not displayed in the header

  @UI @TestRails(13910)
  Scenario: C13910 - Verify successful creating of an admin without hospital
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname | Username   | Password     | ConfirmPassword | EmailAddress        | IsActive |
      | Auto | Admin3  | autoAdmin3 | Password@123 | Password@123    | autoadmin3@test.com | Yes      |
    And user proceed to User roles tab
    And user select role "Admin"
    And user save the new user profile
    Then user details are saved successfully
    When user search user "autoAdmin3" with IsActive status "Yes"
    Then user "autoAdmin3" is not displayed
    When user logout
    Then user is in login page
    When user enter username "autoAdmin3" and password "Password@123"
    Then user is successfully login
    And hospital name is not displayed in the header

  @UI @TestRails(13907)
  Scenario: C13907 - Verify creating a user without providing any user details
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user create a new user
    Then create user modal is displayed
    And create user save button is disabled
    When user save the new user profile
    Then user creation is not successful
    And create user modal is displayed

  @UI @TestRails(13986)
  Scenario: C13986 - Verify field error messages for empty mandatory field
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname | Username   | Password     | ConfirmPassword | EmailAddress        | IsActive |
      | Auto | Admin4  | autoAdmin4 | Password@123 | Password@123    | autoadmin4@test.com | Yes      |
    Then create user save button is enabled
    When user cancel the new user creation
    Then create user modal is not displayed
    When user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname | Username   | Password     | ConfirmPassword | EmailAddress        | IsActive |
      | null | Admin4  | autoAdmin4 | Password@123 | Password@123    | autoadmin4@test.com | Yes      |
    Then error message '"This field is required"' is displayed
    And create user save button is disabled
    When user cancel the new user creation
    And user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname | Username   | Password     | ConfirmPassword | EmailAddress        | IsActive |
      | Auto | null    | autoAdmin4 | Password@123 | Password@123    | autoadmin4@test.com | Yes      |
    Then error message '"This field is required"' is displayed
    And create user save button is disabled
    When user cancel the new user creation
    And user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname | Username | Password     | ConfirmPassword | EmailAddress        | IsActive |
      | Auto | Admin4  | null     | Password@123 | Password@123    | autoadmin4@test.com | Yes      |
    Then error message '"This field is required"' is displayed
    And create user save button is disabled
    When user cancel the new user creation
    And user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname | Username   | Password | ConfirmPassword | EmailAddress        | IsActive |
      | Auto | Admin4  | autoAdmin4 | null     | Password@123    | autoadmin4@test.com | Yes      |
    Then error message '"This field is required"' is displayed
    And create user save button is disabled
    When user cancel the new user creation
    And user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname | Username   | Password     | ConfirmPassword | EmailAddress        | IsActive |
      | Auto | Admin4  | autoAdmin4 | Password@123 | null            | autoadmin4@test.com | Yes      |
    And error message '"Passwords do not match"' is displayed
    And create user save button is disabled
    When user cancel the new user creation
    And user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname | Username   | Password     | ConfirmPassword | EmailAddress        | IsActive |
      | Auto | Admin4  | autoAdmin4 | Password@123 | Password@12     | autoadmin4@test.com | Yes      |
    Then error message '"Passwords do not match"' is displayed
    And create user save button is disabled
    When user cancel the new user creation
    And user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname | Username   | Password    | ConfirmPassword | EmailAddress        | IsActive |
      | Auto | Admin4  | autoAdmin4 | Password123 | Password@123    | autoadmin4@test.com | Yes      |
    Then error message '"Passwords must be at least 8 characters, contain a lowercase, uppercase, special characters and number"' is displayed
    And create user save button is disabled
    When user cancel the new user creation
    And user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname | Username   | Password  | ConfirmPassword | EmailAddress        | IsActive |
      | Auto | Admin4  | autoAdmin4 | Password@ | Password@123    | autoadmin4@test.com | Yes      |
    Then error message '"Passwords must be at least 8 characters, contain a lowercase, uppercase, special characters and number"' is displayed
    And create user save button is disabled
    When user cancel the new user creation
    And user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname | Username   | Password     | ConfirmPassword | EmailAddress        | IsActive |
      | Auto | Admin4  | autoAdmin4 | PASSWORD@123 | Password@123    | autoadmin4@test.com | Yes      |
    Then error message '"Passwords must be at least 8 characters, contain a lowercase, uppercase, special characters and number"' is displayed
    And create user save button is disabled
    When user cancel the new user creation
    And user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname | Username   | Password     | ConfirmPassword | EmailAddress        | IsActive |
      | Auto | Admin4  | autoAdmin4 | password@123 | Password@123    | autoadmin4@test.com | Yes      |
    Then error message '"Passwords must be at least 8 characters, contain a lowercase, uppercase, special characters and number"' is displayed
    And create user save button is disabled
    When user cancel the new user creation
    And user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname | Username   | Password | ConfirmPassword | EmailAddress        | IsActive |
      | Auto | Admin4  | autoAdmin4 | Pass@12  | Password@123    | autoadmin4@test.com | Yes      |
    Then error message '"Passwords must be at least 8 characters, contain a lowercase, uppercase, special characters and number"' is displayed
    And create user save button is disabled
    When user cancel the new user creation
    And user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname | Username   | Password     | ConfirmPassword | EmailAddress | IsActive |
      | Auto | Admin4  | autoAdmin4 | Password@123 | Password@123    | null         | Yes      |
    Then error message '"This field is required"' is displayed
    And create user save button is disabled
    When user cancel the new user creation
    And user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname | Username   | Password     | ConfirmPassword | EmailAddress | IsActive |
      | Auto | Admin4  | autoAdmin4 | Password@123 | Password@123    | autoadmin4   | Yes      |
    Then error message '"Invalid"' is displayed
    And create user save button is disabled

  @UI @TestRails(13911)
  Scenario: C13911 - Verify creating a user with existing email - negative test
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname  | Username     | Password     | ConfirmPassword | EmailAddress     | IsActive |
      | Auto | Surgeon4 | autoSurgeon4 | Password@123 | Password@123    | surgeon@test.com | Yes      |
    And user proceed to User roles tab
    And user select role "Surgeon"
    And user select experience "6 - 7 years"
    And user proceed to User Hospital tab
    And user select hospital "Test Hospital"
    And user proceed to Specialties tab
    And user select a specialty "Orthopaedic Surgery"
    And user save the new user profile
    Then user creation is not successful
    And error message for existing email '"surgeon@test.com"' is displayed
    When user search user "autoSurgeon4" with IsActive status "Yes"
    Then user "autoSurgeon4" is not displayed
    When user logout
    Then user is in login page
    When user enter username "autoSurgeon1" and password "Password@123"
    Then user is not successfully login

  @UI @TestRails(13912)
  Scenario: C13912 - Verify creating a user with existing username - negative test
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname  | Username          | Password     | ConfirmPassword | EmailAddress          | IsActive |
      | Auto | Surgeon5 | automationSurgeon | Password@123 | Password@123    | autosurgeon5@test.com | Yes      |
    And user proceed to User roles tab
    And user select role "Surgeon"
    And user select experience "6 - 7 years"
    And user proceed to User Hospital tab
    And user select hospital "Test Hospital"
    And user proceed to Specialties tab
    And user select a specialty "Orthopaedic Surgery"
    And user save the new user profile
    Then user creation is not successful
    And error message for existing username '"automationSurgeon"' is displayed
    When user search user "autosurgeon5@test.com" with IsActive status "Yes"
    Then user "autosurgeon5@test.com" is not displayed
    When user logout
    Then user is in login page
    When user enter username "autosurgeon5@test.com" and password "Password@123"
    Then user is not successfully login

  @UI @TestRails(13913)
  Scenario: C13913 - Verify creating a user without a role - negative test
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname | Username  | Password     | ConfirmPassword | EmailAddress       | IsActive |
      | Auto | User1   | autoUser1 | Password@123 | Password@123    | autouser1@test.com | Yes      |
    And user proceed to User Hospital tab
    And user select hospital "Test Hospital"
    And user save the new user profile
    Then user details are saved successfully
    When user search user "autoUser1" with IsActive status "Yes"
    Then user "autoUser1" is displayed
    When user logout
    Then user is in login page
    When user enter username "autoUser1" and password "Password@123"
    Then user is successfully login
    And "Test Hospital" is displayed in the header
    And user has no available left navigation menu
    And user has no available menu section

  @UI @TestRails(13914)
  Scenario: C13914 - Verify creating a user without role and hospital - negative test
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname | Username  | Password     | ConfirmPassword | EmailAddress       | IsActive |
      | Auto | User2   | autoUser2 | Password@123 | Password@123    | autouser2@test.com | Yes      |
    And user save the new user profile
    Then user details are saved successfully
    When user logout
    Then user is in login page
    When user enter username "autoUser2" and password "Password@123"
    Then user is successfully login
    And hospital name is not displayed in the header
    And user has no available left navigation menu
    And user has no available menu section

  @UI @TestRails(13920) @Smoke
  Scenario: C13920 - Verify successful changing a user hospitals
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname  | Username     | Password     | ConfirmPassword | EmailAddress          | IsActive |
      | Auto | Surgeon6 | autoSurgeon6 | Password@123 | Password@123    | autosurgeon6@test.com | Yes      |
    And user proceed to User roles tab
    And user select role "Surgeon"
    And user select experience "6 - 7 years"
    And user proceed to User Hospital tab
    And user select hospital "Test Hospital"
    And user proceed to Specialties tab
    And user select a specialty "Orthopaedic Surgery"
    And user save the new user profile
    Then user details are saved successfully
    When user search user "autoSurgeon6" with IsActive status "Yes"
    Then user "autoSurgeon6" is displayed
    When user edit the user search result
    Then edit user modal is displayed
    When user proceed to User Hospital tab
    And user select hospital "Test Hospital"
    And user select hospital "THE PARK HOSPITAL"
    And user save the new user profile
    Then user details are saved successfully
    When user search user "autoSurgeon6" with IsActive status "Yes"
    Then user "autoSurgeon6" is not displayed
    When user navigate to Dashboard page from menu navigation
    Then user is successfully navigated to home page
    When user select "THE PARK HOSPITAL" from hospital dropdown
    Then "THE PARK HOSPITAL" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user search user "autoSurgeon6" with IsActive status "Yes"
    Then user "autoSurgeon6" is displayed
    When user logout
    Then user is in login page
    When user enter username "autoSurgeon6" and password "Password@123"
    Then user is successfully login
    And "THE PARK HOSPITAL" is displayed in the header

  @UI @TestRails(13921)
  Scenario: C13921 - Verify successful adding a user hospitals
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname  | Username     | Password     | ConfirmPassword | EmailAddress          | IsActive |
      | Auto | Surgeon7 | autoSurgeon7 | Password@123 | Password@123    | autosurgeon7@test.com | Yes      |
    And user proceed to User roles tab
    And user select role "Surgeon"
    And user select experience "6 - 7 years"
    And user proceed to User Hospital tab
    And user select hospital "Test Hospital"
    And user proceed to Specialties tab
    And user select a specialty "Orthopaedic Surgery"
    And user save the new user profile
    Then user details are saved successfully
    When user search user "autoSurgeon7" with IsActive status "Yes"
    Then user "autoSurgeon7" is displayed
    When user edit the user search result
    Then edit user modal is displayed
    When user proceed to User Hospital tab
    And user select hospital "THE PARK HOSPITAL"
    And user save the new user profile
    Then user details are saved successfully
    When user search user "autoSurgeon7" with IsActive status "Yes"
    Then user "autoSurgeon7" is displayed
    When user navigate to Dashboard page from menu navigation
    Then user is successfully navigated to home page
    When user select "THE PARK HOSPITAL" from hospital dropdown
    Then "THE PARK HOSPITAL" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user search user "autoSurgeon7" with IsActive status "Yes"
    Then user "autoSurgeon7" is displayed
    When user logout
    Then user is in login page
    When user enter username "autoSurgeon7" and password "Password@123"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user select "THE PARK HOSPITAL" from hospital dropdown
    Then "THE PARK HOSPITAL" is displayed in the header

  @UI @TestRails(13922)
  Scenario: C13922 - Verify successful changing a user role
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname | Username  | Password     | ConfirmPassword | EmailAddress       | IsActive |
      | Auto | User3   | autoUser3 | Password@123 | Password@123    | autouser3@test.com | Yes      |
    And user proceed to User roles tab
    And user select role "Surgeon"
    And user select experience "6 - 7 years"
    And user proceed to User Hospital tab
    And user select hospital "Test Hospital"
    And user proceed to Specialties tab
    And user select a specialty "Orthopaedic Surgery"
    And user save the new user profile
    Then user details are saved successfully
    When user logout
    Then user is in login page
    When user enter username "autoUser3" and password "Password@123"
    Then user is successfully login
    And available menu sections for "Surgeon" are displayed
    And available left navigation menu for "Surgeon" are displayed
    When user logout
    Then user is in login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user search user "autoUser3" with IsActive status "Yes"
    Then user "autoUser3" is displayed
    When user edit the user search result
    Then edit user modal is displayed
    When user proceed to User roles tab
    And user select role "Surgeon"
    And user select role "Admin"
    And user save the new user profile
    Then user details are saved successfully
    When user logout
    Then user is in login page
    When user enter username "autoUser3" and password "Password@123"
    Then user is successfully login
    And available menu sections for "Admin" are displayed
    And available left navigation menu for "Admin" are displayed
    When user logout
    Then user is in login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user search user "autoUser3" with IsActive status "Yes"
    Then user "autoUser3" is displayed
    When user edit the user search result
    Then edit user modal is displayed
    When user proceed to User roles tab
    And user select role "Admin"
    And user select role "Anaesthetist"
    And user save the new user profile
    Then user details are saved successfully
    When user logout
    Then user is in login page
    When user enter username "autoUser3" and password "Password@123"
    Then user is successfully login
    And available menu sections for "Anaesthetist" are displayed
    And available left navigation menu for "Anaesthetist" are displayed

  @UI @TestRails(13923)
  Scenario: C13923 - Verify successful  changing a surgeon specialties
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname  | Username     | Password     | ConfirmPassword | EmailAddress          | IsActive |
      | Auto | Surgeon8 | autoSurgeon8 | Password@123 | Password@123    | autosurgeon8@test.com | Yes      |
    And user proceed to User roles tab
    And user select role "Surgeon"
    And user select experience "6 - 7 years"
    And user proceed to User Hospital tab
    And user select hospital "Test Hospital"
    And user proceed to Specialties tab
    And user select a specialty "Orthopaedic Surgery"
    And user save the new user profile
    Then user details are saved successfully
    When user search user "autoSurgeon8" with IsActive status "Yes"
    Then user "autoSurgeon8" is displayed
    When user edit the user search result
    Then edit user modal is displayed
    When user proceed to Specialties tab
    And user select a specialty "General Surgery"
    And user save the new user profile
    Then user details are saved successfully

  @UI @TestRails(13924)
  Scenario: C13924 - Verify successful changing a surgeon experience
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname  | Username     | Password     | ConfirmPassword | EmailAddress          | IsActive |
      | Auto | Surgeon9 | autoSurgeon9 | Password@123 | Password@123    | autosurgeon9@test.com | Yes      |
    And user proceed to User roles tab
    And user select role "Surgeon"
    And user select experience "6 - 7 years"
    And user proceed to User Hospital tab
    And user select hospital "Test Hospital"
    And user proceed to Specialties tab
    And user select a specialty "Orthopaedic Surgery"
    And user save the new user profile
    Then user details are saved successfully
    When user search user "autoSurgeon9" with IsActive status "Yes"
    Then user "autoSurgeon9" is displayed
    When user edit the user search result
    Then edit user modal is displayed
    When user proceed to User roles tab
    And user select experience "8 - 9 years"
    And user save the new user profile
    Then user details are saved successfully

  @UI @TestRails(13928)
  Scenario: C13928 - Verify successful changing active status
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user search user "autoSurgeon10" with IsActive status "All"
    And user delete the user if existing
    And user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname   | Username      | Password     | ConfirmPassword | EmailAddress           | IsActive |
      | Auto | Surgeon10 | autoSurgeon10 | Password@123 | Password@123    | autosurgeon10@test.com | Yes      |
    And user proceed to User roles tab
    And user select role "Surgeon"
    And user select experience "6 - 7 years"
    And user proceed to User Hospital tab
    And user select hospital "Test Hospital"
    And user proceed to Specialties tab
    And user select a specialty "Orthopaedic Surgery"
    And user save the new user profile
    Then user details are saved successfully
    When user search user "autoSurgeon10" with IsActive status "Yes"
    Then user "autoSurgeon10" is displayed
    When user edit the user search result
    Then edit user modal is displayed
    When user edit user details
      | Name | Surname   | Username      | EmailAddress           | IsActive |
      | Auto | Surgeon10 | autoSurgeon10 | autosurgeon10@test.com | No       |
    And user save the new user profile
    Then user details are saved successfully
    When user logout
    Then user is in login page
    When user enter username "autoSurgeon10" and password "Password@123"
    Then login failed message is displayed for inactive user "autoSurgeon10"

  @UI @TestRails(13925)
  Scenario: C13925 - Verify editing a user without roles -negative test case
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname | Username  | Password     | ConfirmPassword | EmailAddress       | IsActive |
      | Auto | User3   | autoUser3 | Password@123 | Password@123    | autouser3@test.com | Yes      |
    And user proceed to User roles tab
    And user select role "Surgeon"
    And user select experience "6 - 7 years"
    And user proceed to User Hospital tab
    And user select hospital "Test Hospital"
    And user proceed to Specialties tab
    And user select a specialty "Orthopaedic Surgery"
    And user save the new user profile
    Then user details are saved successfully
    When user search user "autoUser3" with IsActive status "Yes"
    Then user "autoUser3" is displayed
    When user edit the user search result
    Then edit user modal is displayed
    When user proceed to User roles tab
    And user select role "Surgeon"
    And user save the new user profile
    Then user details are saved successfully
    When user logout
    Then user is in login page
    When user enter username "autoUser3" and password "Password@123"
    Then user is successfully login
    And user has no available menu section
    And user has no available left navigation menu

  @UI @TestRails(13926)
  Scenario: C13926 - Verify editing a user without user hospitals -negative test case
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname       | Username          | Password     | ConfirmPassword | EmailAddress               | IsActive |
      | Auto | Anaesthetist4 | autoAnaesthetist4 | Password@123 | Password@123    | autoanaesthetist4@test.com | Yes      |
    And user proceed to User roles tab
    And user select role "Anaesthetist"
    And user proceed to User Hospital tab
    And user select hospital "Test Hospital"
    And user save the new user profile
    Then user details are saved successfully
    When user search user "autoAnaesthetist4" with IsActive status "Yes"
    Then user "autoAnaesthetist4" is displayed
    When user edit the user search result
    Then edit user modal is displayed
    When user proceed to User Hospital tab
    And user select hospital "Test Hospital"
    And user save the new user profile
    Then user details are saved successfully
    When user logout
    Then user is in login page
    When user enter username "autoAnaesthetist4" and password "Password@123"
    Then user is successfully login
    And hospital name is not displayed in the header

  @UI @TestRails(13917) @Smoke
  Scenario: C13917 - Verify deleting a user
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname       | Username          | Password     | ConfirmPassword | EmailAddress               | IsActive |
      | Auto | Anaesthetist5 | autoAnaesthetist5 | Password@123 | Password@123    | autoanaesthetist5@test.com | Yes      |
    And user proceed to User roles tab
    And user select role "Anaesthetist"
    And user proceed to User Hospital tab
    And user select hospital "Test Hospital"
    And user save the new user profile
    Then user details are saved successfully
    When user search user "autoAnaesthetist5" with IsActive status "Yes"
    Then user "autoAnaesthetist5" is displayed
    When user delete the user search result
    And user confirm the user deletion
    Then user deletion is successful
    When user search user "autoAnaesthetist5" with IsActive status "Yes"
    Then user "autoAnaesthetist5" is not displayed
    When user logout
    Then user is in login page
    When user enter username "autoAnaesthetist5" and password "Password@123"
    Then user is not successfully login

  @UI @TestRails(13918)
  Scenario: C13918 - Verify cancel deleting a user
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname       | Username          | Password     | ConfirmPassword | EmailAddress               | IsActive |
      | Auto | Anaesthetist6 | autoAnaesthetist6 | Password@123 | Password@123    | autoanaesthetist6@test.com | Yes      |
    And user proceed to User roles tab
    And user select role "Anaesthetist"
    And user proceed to User Hospital tab
    And user select hospital "Test Hospital"
    And user save the new user profile
    Then user details are saved successfully
    When user search user "autoAnaesthetist6" with IsActive status "Yes"
    Then user "autoAnaesthetist6" is displayed
    When user delete the user search result
    And user cancel the user deletion
    Then user deletion is not successful
    When user search user "autoAnaesthetist6" with IsActive status "Yes"
    Then user "autoAnaesthetist6" is displayed
    When user logout
    Then user is in login page
    When user enter username "autoAnaesthetist6" and password "Password@123"
    Then user is successfully login

  @UI @TestRails(13929)
  Scenario: C13929 - Verify searching a user
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname | Username   | Password     | ConfirmPassword | EmailAddress        | IsActive |
      | Auto | Admin5  | autoAdmin5 | Password@123 | Password@123    | autoadmin5@test.com | Yes      |
    And user proceed to User roles tab
    And user select role "Admin"
    And user proceed to User Hospital tab
    And user select hospital "Test Hospital"
    And user save the new user profile
    Then user details are saved successfully
    When user search user "Auto" with IsActive status "Yes"
    Then user "autoAdmin5" is displayed
    When user search user "Admin5" with IsActive status "Yes"
    Then user "autoAdmin5" is displayed
    When user search user "autoAdmin5" with IsActive status "Yes"
    Then user "autoAdmin5" is displayed
    When user search user "autoadmin5@test.com" with IsActive status "Yes"
    Then user "autoAdmin5" is displayed
    When user search user "autoAdmin5" with IsActive status "All"
    Then user "autoAdmin5" is displayed
    When user search user "autoAdmin5" with IsActive status "No"
    Then user "autoAdmin5" is not displayed
    When user search user "Admin6" with IsActive status "Yes"
    Then user "autoAdmin5" is not displayed
    When user navigate to Dashboard page from menu navigation
    Then user is successfully navigated to home page
    When user select "THE PARK HOSPITAL" from hospital dropdown
    Then "THE PARK HOSPITAL" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user search user "autoAdmin5" with IsActive status "Yes"
    Then user "autoAdmin5" is not displayed

  @UI @TestRails(13927)
  Scenario: C13927 - Verify successful reset password
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user create a new user
    Then create user modal is displayed
    When user enter user details
      | Name | Surname       | Username          | Password     | ConfirmPassword | EmailAddress               | IsActive |
      | Auto | Anaesthetist7 | autoAnaesthetist7 | Password@123 | Password@123    | autoanaesthetist7@test.com | Yes      |
    And user proceed to User roles tab
    And user select role "Anaesthetist"
    And user proceed to User Hospital tab
    And user select hospital "Test Hospital"
    And user save the new user profile
    Then user details are saved successfully
    When user search user "autoAnaesthetist7" with IsActive status "Yes"
    Then user "autoAnaesthetist7" is displayed
    When user reset the password of user search result
    Then reset password modal is displayed
    When user cancel the reset password
    Then reset password modal is not displayed
    When user reset the password of user search result
    Then reset password modal is displayed
    When user enter admin password
    And user save the new password
    Then password reset is successful
    When user logout
    Then user is in login page
    When user enter username "autoAnaesthetist7" and the new password
    Then user is successfully login
    When user logout
    Then user is in login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    When user search user "autoAnaesthetist7" with IsActive status "Yes"
    Then user "autoAnaesthetist7" is displayed
    When user delete the user search result
    And user confirm the user deletion
    Then user deletion is successful
