Feature: API Endpoints

  @API
  Scenario Outline: Api Test - <testName>
    Description: All endpoints with positive and negative scenarios are working as expected

    Given previous run result for API test: "<testName>" is deleted
    When user execute API test: "<testName>"
    Then user verify API test: "<testName>" passed

    Examples: 
      | testName  |
      | TokenAuth |
      | User      |

  @DataCleanUp
  Scenario: Test data clean-up - <testName>
    Given previous run result for API test: "DataCleanUp" is deleted
    When user execute API test: "DataCleanUp"
