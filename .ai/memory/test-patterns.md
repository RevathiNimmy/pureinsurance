---
title: Test Patterns
description: Actual testing infrastructure, manual harness patterns, and test project inventory for Pure Insurance
ms.date: 2026-04-28
---

## Critical Finding: No Automated Test Framework

**There is no automated test framework in active use.** All ~23 test-related projects found in the solution are manually-invoked WinForms or console harnesses, not automated unit or integration test suites.

This is a significant finding with implications for all new development:

- There are no test runners to integrate with CI/CD pipelines
- There is no existing test pattern to follow for automated tests
- Changes cannot be verified by running tests — developers rely on manual testing via harnesses and system-level QA

The `.ai/rules/testing-requirements.md` file in this repository describes aspirational testing standards. This file documents what actually exists.

---

## Test Project Inventory

23 projects were identified with "test", "harness", "stub", or "testbed" in their name. All are manual harnesses:

| Project | Type | Purpose |
|---------|------|---------|
| `WindowsService.TestHarness.vbproj` | WinForms | Manually trigger batch jobs in `ProcessJobs` — used by developers to test Windows Service behaviour without running the service |
| `PrjTestHarness.vbproj` (under `uctPMURITax`) | WinForms | Manual test host for RI Tax user control |
| `PrjTestHarness.vbproj` (under `uctPMUFees`) | WinForms | Manual test host for Fees user control |
| `PrjTestHarness.vbproj` (under `uctPartyTax`) | WinForms | Manual test host for Party Tax user control |
| `iTestGEMLookup.vbproj` | WinForms harness | Gemini lookup manual tests |
| `iTestGEMListManager.vbproj` | WinForms harness | Gemini list manager manual tests |
| `TestbGEMListUser.vbproj` | WinForms harness | Gemini list user manual tests |
| `TestbGEMListUpdate.vbproj` | WinForms harness | Gemini list update manual tests |
| `TestbGEMLists.vbproj` | WinForms harness | Gemini lists manual tests |
| `TestbGEMListCustom.vbproj` | WinForms harness | Gemini list custom manual tests |
| `WCF.TestClient.vbproj` | WinForms harness | SAM WCF service manual test client |
| `SiriusFS.SAM.NUnit.SAMForInsurance.vbproj` | NUnit (legacy) | See note below |

---

## Windows Service Test Harness (Most Detailed Example)

**File:** `Pure Service/WindowsService.TestHarness/`

This is the most sophisticated test harness in the solution. It provides a WinForms UI that allows developers to:

1. Connect to the same SQL database as production
2. Browse available batch jobs defined in `ProcessJobs`
3. Trigger individual jobs manually without running the Windows Service
4. View job output/logs in the harness UI

**Pattern (typical across harnesses):**

```vbnet
' Direct instantiation of the production business component
Private m_oProcessJobs As ProcessJobs.ProcessJobs

Private Sub btnRunJob_Click(...) Handles btnRunJob.Click
    Try
        m_oProcessJobs = New ProcessJobs.ProcessJobs
        m_oProcessJobs.Initialise(sUsername, ...)
        m_oProcessJobs.RunJob(lstJobs.SelectedItem.ToString())
    Catch excep As Exception
        MessageBox.Show("Error: " & excep.Message)
    End Try
End Sub
```

---

## User Control Test Harness Pattern

Each reusable user control (`uct*` project) in `Sirius Back Office Core/` has a sibling `PrjTestHarness.vbproj` that hosts the control on a plain WinForms form:

```
Sirius Back Office Core/
    Components/
        Tax/
            uctPartyTax/
                uctPartyTax.vbproj        ← production control
                PrjTestHarness.vbproj      ← manual test host
```

The harness embeds the user control and provides test buttons to exercise specific methods.

---

## NUnit — Legacy Reference Only

`SiriusFS.SAM.NUnit.SAMForInsurance.vbproj` references NUnit and contains test classes, but is considered a legacy/reference project. There is no evidence of it being run as part of any build or CI process.

**Project structure (as explored):**
- References `nunit.framework` package
- Contains SAM-related test fixtures
- Not included in the main `Pure.slnf` solution filter
- No CI pipeline configuration references this project

---

## Gemini List Manager Tests (Largest Test Group)

The Gemini List Manager component has 6 test projects — the most test coverage in the solution. All are WinForms harnesses, not NUnit:

```
Sirius Back Office Core/
    Components/
        Gemini List Manager/
            iTestGEMLookup.vbproj
            iTestGEMListManager.vbproj
            TestbGEMListUser.vbproj
            TestbGEMListUpdate.vbproj
            TestbGEMLists.vbproj
            TestbGEMListCustom.vbproj
```

Each harness tests a specific aspect of the Gemini component. The pattern appears to be one harness per class or feature area rather than one harness per component.

---

## Naming Conventions Observed

| Prefix/Suffix | Meaning |
|--------------|---------|
| `PrjTestHarness` | Generic harness name — hosts a user control or component for manual testing |
| `*TestHarness*` | WinForms harness for a service or batch job |
| `iTest*` | Interface-level test harness (Gemini pattern) |
| `Testb*` | Business-level test harness (Gemini pattern) |
| `WCF.TestClient` | WCF service test consumer |
| `*NUnit*` | NUnit (legacy only) |

**Warning:** The naming does NOT consistently distinguish manual harnesses from automated tests. `SiriusFS.SAM.NUnit.SAMForInsurance` contains automated NUnit tests but is named like a harness. When exploring new components, always check whether a "test" project is a WinForms harness or an actual automated test suite.

---

## What Is Absent

The following testing infrastructure found in modern .NET solutions is **not present** in this codebase:

| Missing | Impact |
|---------|--------|
| xUnit / NUnit / MSTest in active use | No automated unit tests can be run |
| Integration test projects against test database | No automated integration testing |
| Mock/stub framework (Moq, NSubstitute, etc.) | Dependency isolation not feasible without refactoring |
| Test data builders / factories | No structured test data management |
| CI/CD test execution step | Tests not run in pipelines |
| Code coverage tooling | Coverage is unknown |
| Any `[TestClass]`, `[TestFixture]`, or `[Fact]` attributes in active code | No test runners can discover tests |

---

## Guidance for New Development

When `.ai/rules/testing-requirements.md` requires tests on new code:

1. **If creating a new .NET Standard / C# component** — add a proper NUnit or xUnit test project; this is the only area where a test framework can be introduced without VB.NET constraints.

2. **If creating a new VB.NET component** — add a `PrjTestHarness.vbproj` sibling project following the existing pattern; this will at minimum provide a manual test host.

3. **If modifying existing VB.NET code** — no automated test infrastructure exists to add tests to; document the manual test steps in the task PR description.

4. **Do not introduce automated tests into projects that reference `Artinsoft.VB6` or COM interfaces** — the test framework cannot instantiate these components without the full application environment.
