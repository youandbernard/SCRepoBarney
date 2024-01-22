package co.uk.pageobjects;

import org.openqa.selenium.By;

import co.uk.webelements.Button;
import co.uk.webelements.CheckBox;
import co.uk.webelements.Element;
import co.uk.webelements.RadioButton;
import co.uk.webelements.Tab;
import co.uk.webelements.TextBox;

public class UsersPage {

    private static Button buttonCreate = new Button("Create", By.xpath("//a[contains(@class,'btn') and contains(text(),'Create')]"));
    private static TextBox textBoxSearch = new TextBox("Search", By.xpath("//input[@name='keyword']"));
    private static Button buttonSearch = new Button("Search", By.xpath("//button[contains(text(),'Search')]"));
    private static Button buttonClear = new Button("Clear", By.xpath("//button[contains(text(),'Clear')]"));
    private static Element searchDropDown = new Element("Search drop down", By.xpath("//i[contains(@class,'down')]/parent::button"));
    private static Element headerUsers = new Element("Users header", By.xpath("//h1[text()='Users']"));
    private static Element messageSavedSuccessfully = new Element("Saved successfully message",
            By.xpath("//*[contains(text(),'Saved successfully')]"));
    private static Element messagePasswordReset = new Element("Password Reset message", By.xpath("//*[contains(text(),'Password Reset')]"));
    private static Element messageSuccessfullyDeleted = new Element("Successfully deleted message",
            By.xpath("//*[contains(text(),'Successfully deleted')]"));

    private static Element radioButtonIsActiveStatus(String status) {
        String forAttribute = "";
        if (status.equals("All")) {
            forAttribute = "isActiveAll";
        } else if (status.equals("Yes")) {
            forAttribute = "isActiveActive";
        } else if (status.equals("No")) {
            forAttribute = "isActivePassive";
        }
        return new Element("Status: " + status, By.xpath("//label[@for='" + forAttribute + "']"));
    }

    private static Element user(String userDetail) {
        return new Element("User : " + userDetail, By.xpath("//td[text()='" + userDetail + "']"));
    }

    private static Element valueUserIdByIndex(int index) {
        return new Element("User Id", By.xpath("(//tr/td[1])[" + index + "]"));
    }

    private static Element valueUsernameByIndex(int index) {
        return new Element("Username", By.xpath("(//tr/td[2])[" + index + "]"));
    }

    private static Element valueFullnameByIndex(int index) {
        return new Element("Full name", By.xpath("(//tr/td[3])[" + index + "]"));
    }

    private static Element valueEmailAddressByIndex(int index) {
        return new Element("Email address", By.xpath("(//tr/td[4])[" + index + "]"));
    }

    private static Button buttonEditByIndex(int index) {
        return new Button("Edit", By.xpath("(//button[contains(text(),'Edit')])[" + index + "]"));
    }

    private static Button buttonDeleteByIndex(int index) {
        return new Button("Delete", By.xpath("(//button[contains(text(),'Delete')])[" + index + "]"));
    }

    private static Button buttonResetPasswordByIndex(int index) {
        return new Button("Reset Password", By.xpath("(//button[contains(text(),'Reset Password')])[" + index + "]"));
    }

    public static void verifyUsernameIsDisplayed(String userDetail) {
        user(userDetail).verifyDisplayed();
    }

    public static void verifyUserIsNotDisplayed(String userDetail) {
        user(userDetail).verifyNotDisplayed();
    }

    public static void verifySavedSuccessfullyMessageIsDisplayed() {
        messageSavedSuccessfully.verifyDisplayed();
    }

    public static void verifyPasswordResetMessageIsDisplayed() {
        messagePasswordReset.verifyDisplayed();
    }

    public static void verifySavedSuccessfullyMessageIsNotDisplayed() {
        messageSavedSuccessfully.verifyNotDisplayed();
    }

    public static void verifySuccessfullyDeletedMessageIsDisplayed() {
        messageSuccessfullyDeleted.verifyDisplayed();
    }

    public static void verifySuccessfullyDeletedMessageIsNotDisplayed() {
        messageSuccessfullyDeleted.verifyNotDisplayed();
    }

    public static void verifyUsersPage() {
        headerUsers.verifyDisplayed();
    }

    public static void clickCreateButton() {
        buttonCreate.click();
    }

    public static void enterSearchText(String searchText) {
        textBoxSearch.setText(searchText);
    }

    public static void clickSearchButton() {
        buttonSearch.click();
    }

    public static void clickClearButton() {
        buttonClear.click();
    }

    public static void clickSearchDropDown() {
        if (!buttonSearch.isDisplayed()) {
            searchDropDown.click();
        }
    }

    public static void tickIsActiveRadioButton(String status) {
        radioButtonIsActiveStatus(status).click();
    }

    public static void verifyUserIdByIndexContains(int index, String value) {
        valueUserIdByIndex(index).verifyTextContains(value);
    }

    public static void verifyUsernameIdByIndexContains(int index, String value) {
        valueUsernameByIndex(index).verifyTextContains(value);
    }

    public static void verifyFullnameByIndexContains(int index, String value) {
        valueFullnameByIndex(index).verifyTextContains(value);
    }

    public static void verifyEmailAddressByIndexContains(int index, String value) {
        valueEmailAddressByIndex(index).verifyTextContains(value);
    }

    public static void clickEditButton(int index) {
        buttonEditByIndex(index).click();
    }

    public static void clickDeleteButton(int index) {
        buttonDeleteByIndex(index).click();
    }
    
    public static boolean isDeleteButtonExist(int index) {
        return buttonDeleteByIndex(index).isDisplayed();
    }

    public static void clickResetPasswordButton(int index) {
        buttonResetPasswordByIndex(index).click();
    }

    public static class Modal {

        private static Tab tabUserDetails = new Tab("User Details", By.xpath("//*[@role='tab']/*[text()='User details']"));
        private static Tab tabUserRoles = new Tab("User Roles", By.xpath("//*[@role='tab']/*[text()='User roles']"));
        private static Tab tabUserHospitals = new Tab("User Hospitals", By.xpath("//*[@role='tab']/*[text()='User hospitals']"));
        private static Tab tabSpecialties = new Tab("Specialties", By.xpath("//*[@role='tab']/*[text()='Specialties']"));
        private static Button buttonCancel = new Button("Cancel", By.xpath("//*[contains(@class,'modal')]/button[contains(text(),'Cancel')]"));
        private static Button buttonSave = new Button("Save", By.xpath("//*[contains(@class,'modal')]/button[contains(text(),'Save')]"));
        private static TextBox textBoxName = new TextBox("Name", By.xpath("//input[@id='name']"));
        private static TextBox textBoxSurname = new TextBox("Surname", By.xpath("//input[@id='surname']"));
        private static TextBox textBoxUsername = new TextBox("Username", By.xpath("//input[@id='userName']"));
        private static TextBox textBoxPassword = new TextBox("Password", By.xpath("//input[@id='password']"));
        private static TextBox textBoxConfirmPassword = new TextBox("Confirm Password", By.xpath("//input[@id='confirmPassword']"));
        private static TextBox textBoxEmailAddress = new TextBox("Email Address", By.xpath("//input[@id='emailAddress']"));
        private static CheckBox checkBoxIsActive = new CheckBox("Is Active", By.xpath("//input[@id='isActive']"));
        private static Element modalCreateUser = new Element("Create user modal",
                By.xpath("//h4[@class='modal-title' and text()='Create new user']"));
        private static Element modalEditUser = new Element("Edit user modal", By.xpath("//h4[@class='modal-title' and text()='Edit user']"));
        private static Element modalResetPassword = new Element("Reset password modal",
                By.xpath("//h4[@class='modal-title' and text()='Reset Password']"));
        private static Button buttonDeleteModalCancel = new Button("Cancel", By.xpath("//button[contains(@class,'cancel')]"));
        private static Button buttonDeleteModalYes = new Button("Yes", By.xpath("//button[contains(@class,'confirm')]"));
        private static TextBox textBoxAdminPassword = new TextBox("Admin Password", By.xpath("//input[@id='adminPassword']"));
        private static TextBox textBoxNewPassword = new TextBox("New Password", By.xpath("//input[@id='newPassword']"));

        private static Element messageExistingEmail(String email) {
            return new Element("Message for xisting email: " + email, By.xpath("//*[text()=\"Email '" + email + "' is already taken.\"]"));
        }

        private static Element messageExistingUsername(String username) {
            return new Element("Message for xisting username: " + username,
                    By.xpath("//*[text()=\"User name '" + username + "' is already taken.\"]"));
        }

        private static CheckBox checkBoxUserRole(String userRole) {
            return new CheckBox("User Role: " + userRole, By.xpath("//label[contains(text(),'" + userRole + "')]"));
        }

        private static CheckBox checkBoxExperience(String experience) {
            return new CheckBox("Experience: " + experience, By.xpath("//label[contains(text(),'" + experience + "')]"));
        }

        private static CheckBox checkBoxUserHospital(String userHospital) {
            return new CheckBox("User Hospital: " + userHospital, By.xpath("//label[contains(text(),'" + userHospital + "')]"));
        }

        private static RadioButton radioButtonSpecialties(String specialties) {
            return new RadioButton("Specialties: " + specialties, By.xpath("//label[contains(text(),'" + specialties + "')]"));
        }

        public static void enterAdminPassword(String password) {
            textBoxAdminPassword.setPassword(password);
        }

        public static String getNewPassword() {
            return textBoxNewPassword.getAttribute("value");
        }

        public static void verifyExistingEmailErrorMessageIsDisplayed(String email) {
            messageExistingEmail(email).verifyDisplayed();
        }

        public static void verifyExistingUsernameErrorMessageIsDisplayed(String username) {
            messageExistingUsername(username).verifyDisplayed();
        }

        public static void verifyCreateUserModalIsDisplayed() {
            modalCreateUser.verifyDisplayed();
        }

        public static void verifyCreateUserModalIsNotDisplayed() {
            modalCreateUser.verifyNotDisplayed();
        }

        public static void verifyEditUserModalIsDisplayed() {
            modalEditUser.verifyDisplayed();
        }

        public static void verifyResetPasswordModalIsDisplayed() {
            modalResetPassword.verifyDisplayed();
        }

        public static void verifyResetPasswordModalIsNotDisplayed() {
            modalResetPassword.verifyNotDisplayed();
        }

        public static void verifySaveButtonIsDisabled() {
            buttonSave.verifyDisabled();
        }

        public static void verifySaveButtonIsEnabled() {
            buttonSave.verifyEnabled();
        }

        public static void clickUserDetailsTab() {
            tabUserDetails.click();
        }

        public static void clickUserRolesTab() {
            tabUserRoles.click();
        }

        public static void clickUserHospitalsTab() {
            tabUserHospitals.click();
        }

        public static void clickSpecialtiesTab() {
            tabSpecialties.click();
        }

        public static void clickSaveButton() {
            buttonSave.click();
        }

        public static void clickCancelButton() {
            buttonCancel.click();
        }

        public static void enterName(String name) {
            textBoxName.setText(name);
        }

        public static void clearName() {
            textBoxName.clear();
        }

        public static void enterSurname(String surname) {
            textBoxSurname.setText(surname);
        }

        public static void clearSurname() {
            textBoxSurname.clear();
        }

        public static void enterPassword(String password) {
            textBoxPassword.setText(password);
        }

        public static void clearPassword() {
            textBoxPassword.clear();
        }

        public static void enterConfirmPassword(String confirmPassword) {
            textBoxConfirmPassword.setText(confirmPassword);
        }

        public static void clearConfirmPassword() {
            textBoxConfirmPassword.clear();
        }

        public static void enterUsername(String userName) {
            textBoxUsername.setText(userName);
        }

        public static void clearUsername() {
            textBoxUsername.clear();
        }

        public static void enterEmailAddress(String emailAddress) {
            textBoxEmailAddress.setText(emailAddress);
        }

        public static void clearEmailAddress() {
            textBoxEmailAddress.clear();
        }

        public static void clickIsActiveCheckbox() {
            checkBoxIsActive.clickJS();
        }

        public static void clickUserRoleCheckbox(String userRole) {
            checkBoxUserRole(userRole).click();
        }

        public static void clickExperienceCheckbox(String experience) {
            checkBoxExperience(experience).click();
        }

        public static void clickUserHospitalCheckbox(String userHospital) {
            checkBoxUserHospital(userHospital).click();
        }

        public static void tickSpecialtiesRadioButton(String specialties) {
            radioButtonSpecialties(specialties).click();
        }

        public static void clickDeleteModalCancelButton() {
            buttonDeleteModalCancel.click();
        }

        public static void clickDeleteModalYesButton() {
            buttonDeleteModalYes.click();
        }

    }

}
