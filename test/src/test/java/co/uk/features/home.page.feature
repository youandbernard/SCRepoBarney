Feature: Home Page Functionality

  @UI @TestRails(13972) @Smoke
  Scenario: C13972 - Verify successful changing hospitals
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    When user click the hospital dropdown
    Then user is not able to see not assigned hospital "Szeged University Hospital"
    When user select "Test Hospital" from hospital dropdown
    Then "Test Hospital" is displayed in the header
    When user select "THE PARK HOSPITAL" from hospital dropdown
    Then "THE PARK HOSPITAL" is displayed in the header
    When user navigate to Configuration page from menu navigation
    Then user is successfully navigated to configuration page
    And "THE PARK HOSPITAL" is displayed in the header
    When user navigate to Reporting Settings page from menu navigation
    Then user is successfully navigated to reporting settings page
    And "THE PARK HOSPITAL" is displayed in the header
    When user navigate to Device Video Settings page from menu navigation
    Then user is successfully navigated to device video settings page
    And "THE PARK HOSPITAL" is displayed in the header
    When user navigate to Survey Settings page from menu navigation
    Then user is successfully navigated to survey settings page
    And "THE PARK HOSPITAL" is displayed in the header
    #    When user navigate to Devices page from menu navigation
    #    Then user is successfully navigated to devices page
    #	And "THE PARK HOSPITAL" is displayed in the header
    When user navigate to Theaters page from menu navigation
    Then user is successfully navigated to theaters page
    And "THE PARK HOSPITAL" is displayed in the header
    When user navigate to Roles page from menu navigation
    Then user is successfully navigated to roles page
    And "THE PARK HOSPITAL" is displayed in the header
    When user navigate to Users page from menu navigation
    Then user is successfully navigated to user page
    And "THE PARK HOSPITAL" is displayed in the header
    When user navigate to POAPs page from menu navigation
    Then user is successfully navigated to POAP page
    And "THE PARK HOSPITAL" is displayed in the header
    When user navigate to Surveys page from menu navigation
    Then user is successfully navigated to Surveys page
    And "THE PARK HOSPITAL" is displayed in the header
    When user navigate to Dashboard page from menu navigation
    Then user is successfully navigated to home page
    And "THE PARK HOSPITAL" is displayed in the header

  @UI @TestRails(13977)
  Scenario: C13977 - Verify available menu in left navigation and menu block items in homepage for surgeon or anesthetist
    Given user is in casemix login page
    When user login as "surgeon"
    Then user is successfully login
    And message: "Our Value Proposition" is displayed
    And message: ""In everything we do, we believe in making transformational change possible. Our quality data and human insights enable better care and delivery. We guide and maximise healthcare planning and productivity ...we just happen to humanise data."" is displayed
    And available menu sections for "Surgeon" are displayed
    And available left navigation menu for "Surgeon" are displayed

  @UI @TestRails(13978)
  Scenario: C13978 - Verify available menu in left navigation and menu block items in homepage for admin
    Given user is in casemix login page
    When user login as "admin"
    Then user is successfully login
    And message: "Our Value Proposition" is displayed
    And message: ""In everything we do, we believe in making transformational change possible. Our quality data and human insights enable better care and delivery. We guide and maximise healthcare planning and productivity ...we just happen to humanise data."" is displayed
    And available menu sections for "Admin" are displayed
    And available left navigation menu for "Admin" are displayed
