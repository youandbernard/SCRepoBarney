package co.uk.pageobjects;

import org.openqa.selenium.By;

import co.uk.webelements.Button;
import co.uk.webelements.Element;
import co.uk.webelements.Tab;
import co.uk.webelements.TextBox;

public class PoapsPageRisks2Tab {

	private static Tab risk_2TabActive = new Tab("Risks-2 Tab Active",
			By.xpath("//a[@class='nav-link active']/span[text()='Risks-2']"));
	private static TextBox searchBox = new TextBox("Searchbox", By.xpath("//input[contains(@class,'ui-tree-filter')]"));
	private static Button nextButton = new Button("Next button",
			By.xpath("//tab[@class='tab-pane active']//button[contains(text(),'Next')]"));

	private static Element parentRisk(String parentRisk) {
		return new Element("Parent risk " + parentRisk, By.xpath(
				"//span[contains(@class,'ui-tree-toggler')]/parent::div[contains(@aria-label,'" + parentRisk + "')]"));
	}

	private static Element childRisk(String childRisk) {
		return new Element("Child risk " + childRisk,
				By.xpath("//ul[contains(@class,'ui-treenode-children')]//following::span[contains(text(),'" + childRisk
						+ "')]"));
	}

	private static Element treeToggle(String parentRiskName) {
		return new Element("Tree Toggle for " + parentRiskName, By.xpath(
				"//div[contains(@aria-label,'" + parentRiskName + "')]/span[contains(@class,'ui-tree-toggler')]"));
	}

	public static void verifyRisk_2TabisActive() {
		risk_2TabActive.verifyDisplayed();
	}

	public static void verifyParentRisk(String parentRisk) {
		parentRisk(parentRisk).verifyDisplayed();
	}

	public static void verifySubRisk(String subRisk) {
		childRisk(subRisk).verifyDisplayed();
	}

	public static void selectSubRisk(String subRisk) {
		childRisk(subRisk).click();
	}

	public static void selectParentRisk(String parentRisk) {
		parentRisk(parentRisk).click();
	}

	public static void searchRisk(String searchForRisk) {
		searchBox.setText(searchForRisk);
	}

	public static void nextButton() {
		nextButton.click();
	}

	public static void expandAndCollapseArrow(String parentRiskName) {
		treeToggle(parentRiskName).click();
	}
}
