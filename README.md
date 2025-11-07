# 🚀 C# Playwright Automation Infrastructure

פרויקט זה מציג תשתית בדיקות קצה-לקצה (E2E) מתקדמת, המבוססת על **Playwright** ו-**C\#** (.NET). הפתרון מדגים יישום קפדני של **עקרונות ארכיטקטורה נקייה**, יציבות גבוהה של הבדיקות, והפרדה יעילה בין שכבות ה-UI לבין שכבות ה-API.

---

## 🛠️ Key Technologies

* **Automation Tool:** Microsoft Playwright
* **Language:** C\# / .NET
* **Testing Framework:** **xUnit**

---

## 🎯 Architectural Design & Key Decisions

The project's design focuses on **maintainability, reusability, and stability**, crucial for production-grade automation:

### 1. Separation of Concerns (API & POM)
* **Generic API Client:** The `MediaWikiApiClient` is highly generic. It accepts the `pageTitle` in the constructor and the `sectionTitle` dynamically in the method (`GetSectionContentByTitleAsync`), maximizing the client's **reusability** across different wiki pages.
* **Specific Page Object Model ($\text{POM}$):** The $\text{POM}$ classes (e.g., `WikipediaPlaywrightPage`) are **specific to a single page's content**. This prevents the $\text{POM}$ from becoming bloated and ensures high clarity regarding its responsibilities.
* **Navigation Isolation:** Expensive navigation operations (`page.GotoAsync`) are explicitly **kept out of the $\text{POM}$** classes, residing only in the test or flow layer to better manage $\text{I/O}$ costs and test flow.

### 2. Robustness and Locator Strategy
* **Playwright Native Locators:** Avoided the use of JavaScript injection (`EvaluateAsync`) in favor of native $\text{Playwright Locators}$ and **XPath** (where needed for complex traversal). This ensures test stability by leveraging $\text{Playwright}$'s built-in **Auto-Waiting** mechanisms.

### 3. xUnit Fixtures and Resource Management
* **Setup/Cleanup:** Resource initialization and cleanup (e.g., `IPage`, `IAPIRequestContext`) are implemented using **xUnit Class or Collection Fixtures** and the **IDisposable interface**. This ensures proper state isolation and execution time optimization.

---

## 📁 Repository Structure

The project structure is organized for clear separation of responsibilities:
├── ApiClients/ # 🌐 API Layer: Generic API clients (e.g., MediaWikiApiClient) and JSON parsing.
├── Pages/ # 🖥️ UI Layer: Specific Page Object Models (Locators and UI interaction methods).
└── Tests/ # 🧪 Test Layer: Contains all xUnit test execution logic and setup.
├── Fixtures/ # 🔧 xUnit Setup: Fixture classes (IClassFixture/ICollectionFixture) for resource lifecycle.
└── TestClasses/ # Actual xUnit Test Classes (which consume the Fixtures).

## 🏃 Getting Started

1.  **Prerequisites:** Ensure the $\text{.NET SDK}$ is installed.
2.  **Clone the repository:**
    ```bash
    git clone
    ```
3.  **Install Playwright Browsers (One-time):**
    ```bash
    dotnet tool install --global Microsoft.Playwright.CLI
    playwright install
    ```
4.  **Run Tests:** Execute all tests using the $\text{.NET CLI}$:
    ```bash
    dotnet test
    ```
