# 🚀 C# Playwright Automation Infrastructure

This project demonstrates an advanced End-to-End (E2E) testing infrastructure built with **Playwright** and **C#** (.NET). The solution strictly adheres to **Clean Architecture principles**, prioritizing test stability, high resilience, and effective separation between the UI and API testing layers.

---

## 🛠️ Key Technologies

* **Automation Tool:** Microsoft Playwright
* **Language:** C\# / .NET
* **Testing Framework:** **xUnit**
* **Data Handling:** System.Text.Json (for API response parsing)
* **Version Control:** Git

---

## 🎯 Architectural Design & Key Decisions

The project's design focuses on **maintainability, reusability, and stability**, crucial for production-grade automation:

### 1. Robustness and Locator Strategy
* **Resilience over Simplicity:** Utilized **XPath following-sibling** selectors for reliable $\text{DOM}$ traversal, addressing complex navigation challenges without relying on brittle $\text{IDs}$.
* **Playwright Native Locators:** Avoided the use of JavaScript injection (`EvaluateAsync`) in favor of native $\text{Playwright Locators}$. This ensures test stability by leveraging $\text{Playwright}$'s built-in **Auto-Waiting** mechanisms.

### 2. Separation of Concerns (API & POM)
* **Generic API Client:** The `MediaWikiApiClient` is highly generic. It accepts the `pageTitle` in the constructor and the `sectionTitle` dynamically in the method (`GetSectionContentByTitleAsync`), maximizing the client's **reusability** across different wiki pages.
* **Specific Page Object Model ($\text{POM}$):** The $\text{POM}$ classes (e.g., `WikipediaPlaywrightPage`) are **specific to a single page's content**. This prevents the $\text{POM}$ from becoming bloated and ensures high clarity regarding its responsibilities.
* **Navigation Isolation:** Expensive navigation operations (`page.GotoAsync`) are explicitly **kept out of the $\text{POM}$** classes, residing only in the test or flow layer to better manage $\text{I/O}$ costs and test flow.

### 3. xUnit Setup and Resource Management
* **Resource Lifecycle:** Resource setup (e.g., initializing `IPage` and `IAPIRequestContext`) is handled using the **xUnit test class constructor**. Resource cleanup is guaranteed through the **IDisposable interface**, ensuring proper state management for each test class.

### 4. Code Standards and Error Handling
* **Fail Fast Principle:** Implemented logic to throw an **InvalidOperationException** when a critical precondition is missing (e.g., missing $\text{sectionIndex}$). This ensures tests fail early and explicitly.
* **$\text{NULL}$ Safety:** Used the **$\text{??}$ operator** ($\text{Null-Coalescing}$) to guarantee that asynchronous string methods (`GetString()`) return `string.Empty` instead of $\text{null}$, thereby preventing $\text{NullReferenceExceptions}$ ($\text{NRE}$) and honoring $\text{C\#}$ non-nullable string signatures.
* **Async Naming:** All asynchronous methods strictly adhere to $\text{C\#}$ standards by ending with the **$\text{Async}$** suffix.

---

## 📁 Repository Structure

The project structure is organized for clear separation of responsibilities:
├── ApiClients/ # Generic API client classes (e.g., MediaWikiApiClient) and JSON parsing logic. ├── Pages/ # Specific Page Object Models (UI Locators and interaction methods). └── Tests/ # xUnit Test Classes, setup/cleanup logic, and test assertions.
---

## 🏃 Getting Started

1.  **Prerequisites:** Ensure the $\text{.NET SDK}$ is installed.
2.  **Clone the repository:**
    ```bash
    git clone [REPO_URL]
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
