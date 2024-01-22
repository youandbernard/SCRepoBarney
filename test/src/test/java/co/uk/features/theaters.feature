Feature: Theaters Functionality

  @UI @TestRails(13979) @Smoke
  Scenario: C13979 - Verify successful adding a theater
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Theaters page from menu navigation
    Then user is successfully navigated to theaters page
    When user create a new theater
    Then create theater modal is displayed
    And create theater save button is disabled
    When user cancel the theater creation
    Then create theater modal is not displayed
    When user create a new theater
    Then create theater modal is displayed
    When user enter theater details
      | Theater ID | Name         |
      | null       | AutoTheater1 |
    Then create theater save button is disabled
    And error message '"This field is required"' is displayed
    When user cancel the theater creation
    Then create theater modal is not displayed
    When user create a new theater
    Then create theater modal is displayed
    When user enter theater details
      | Theater ID | Name |
      |     110011 | null |
    Then create theater save button is disabled
    And error message '"This field is required"' is displayed
    When user cancel the theater creation
    Then create theater modal is not displayed
    When user create a new theater
    Then create theater modal is displayed
    When user enter theater details
      | Theater ID | Name         |
      |     110011 | AutoTheater1 |
    Then create theater save button is enabled
    When user save the new theater
    Then theater details are saved successfully
    When user search theater "AutoTheater1"
    Then theater "AutoTheater1" is displayed

  @UI @TestRails(13980) @Smoke
  Scenario: C13980 - Verify successful deleting a theater
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Theaters page from menu navigation
    Then user is successfully navigated to theaters page
    When user create a new theater
    Then create theater modal is displayed
    When user enter theater details
      | Theater ID | Name         |
      |     110012 | AutoTheater2 |
    And user save the new theater
    Then theater details are saved successfully
    When user search theater "AutoTheater2"
    Then theater "AutoTheater2" is displayed
    When user delete the theater search result
    And user cancel the theater deletion
    Then theater deletion is not successful
    When user search theater "AutoTheater2"
    Then theater "AutoTheater2" is displayed
    When user delete the theater search result
    And user confirm the theater deletion
    Then theater deletion is successful
    When user search theater "AutoTheater2"
    Then theater "AutoTheater2" is not displayed

  @UI @TestRails(13981) @Smoke
  Scenario: C13981 - Verify successful editing a theater
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Theaters page from menu navigation
    Then user is successfully navigated to theaters page
    When user create a new theater
    Then create theater modal is displayed
    When user enter theater details
      | Theater ID | Name         |
      |     110013 | AutoTheater3 |
    And user save the new theater
    Then theater details are saved successfully
    When user search theater "AutoTheater3"
    Then theater "AutoTheater3" is displayed
    When user edit the theater search result
    Then edit theater modal is displayed
    When user cancel the theater editing
    Then edit theater modal is not displayed
    When user edit the theater search result
    Then edit theater modal is displayed
    When user edit theater details
      | Theater ID | Name         |
      |     110014 | AutoTheater3 |
    And user save the new theater details
    Then theater details are saved successfully
    When user search theater "110014"
    Then theater "110014" is displayed
    And theater "110013" is not displayed
    When user edit the theater search result
    Then edit theater modal is displayed
    When user edit theater details
      | Theater ID | Name         |
      |     110014 | AutoTheater4 |
    And user save the new theater details
    Then theater details are saved successfully
    When user search theater "AutoTheater4"
    Then theater "AutoTheater4" is displayed
    And theater "AutoTheater3" is not displayed

  @UI @TestRails(13982)
  Scenario: C13982 - Verify successful searching a theater
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Theaters page from menu navigation
    Then user is successfully navigated to theaters page
    When user create a new theater
    Then create theater modal is displayed
    When user enter theater details
      | Theater ID | Name         |
      |     110015 | AutoTheater5 |
    And user save the new theater
    Then theater details are saved successfully
    When user search theater "AutoTheater5"
    Then theater "AutoTheater5" is displayed
    And theater "AutoTheater4" is not displayed
    When user search theater "110015"
    Then theater "110015" is displayed
    And theater "110014" is not displayed
