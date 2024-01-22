Feature: Login Functionality

  @UI @TestRails(13756) @Smoke
  Scenario: C13756 - Validate login using valid credentials
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login

  @UI @TestRails(13755)
  Scenario: C13755 - Validate login using invalid credentials
    Given user is in casemix login page
    When user enter username "admin" and password "pass123"
    Then login failed message is displayed for invalid credentials

  @UI @TestRails(13831)
  Scenario: C13831 - Validate login button is disable if username or password empty
    Given user is in casemix login page
    When user login with credentials
      | Username | Password  |
      | null     | Pass@1234 |
    Then error message '"This field is required"' is displayed
    When page is refreshed
    And user login with credentials
      | Username      | Password |
      | adminUsername | null     |
    Then error message '"This field is required"' is displayed
