Feature: Reports Functionality

  @UI @TestRails(13983)
  Scenario: C13983 - Verify successful enabling a reports menu
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Reporting Settings page from menu navigation
    Then user is successfully navigated to reporting settings page
    When user check hospital "Test Hospital"
    And user save the reporting settings
    Then reporting settings are saved successfully
    When user navigate to Dashboard page from menu navigation
    Then user is successfully navigated to home page
    And reports menu sections is displayed

  @UI @TestRails(13984)
  Scenario: C13984 - Verify successful disabling a reporting menu
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Reporting Settings page from menu navigation
    Then user is successfully navigated to reporting settings page
    When user uncheck hospital "Test Hospital"
    And user save the reporting settings
    Then reporting settings are saved successfully
    When user navigate to Dashboard page from menu navigation
    Then user is successfully navigated to home page
    And reports menu sections is not displayed

  @UI @TestRails(13987) @Smoke
  Scenario: C13987 - Verify viewing reports
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Reporting Settings page from menu navigation
    Then user is successfully navigated to reporting settings page
    When user check hospital "Test Hospital"
    And user save the reporting settings
    Then reporting settings are saved successfully
    When user navigate to Dashboard page from menu navigation
    Then user is successfully navigated to home page
    And reports menu sections is displayed
    When user open reports from menu sections
    Then reports pdf is displayed
