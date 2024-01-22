Feature: Device Video Settings Functionality

  @UI @Smoke @TestRails(13985)
  Scenario: C13985 - Verify device demo video page
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user navigate to Device Video Settings page from menu navigation
    Then user is successfully navigated to device video settings page
    And video is displayed
