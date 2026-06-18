# Testing Requirements — Pure Insurance

All code changes must include tests. This document defines testing standards, patterns, and requirements.

**Last Updated**: 2026-04-28

---

## Testing Principles

1. **Test behaviour, not implementation** — verify what the code does, not how it does it
2. **Tests are documentation** — test names and structure should communicate intent clearly
3. **Independent tests** — tests must not depend on execution order or shared mutable state
4. **Reliable tests** — flaky tests must be fixed or removed; they erode trust

---

## Test Coverage Requirements

### Minimum Coverage Targets

| Type | Coverage Target | Applies To |
|------|----------------|-----------|
| Unit Tests | 80% of new business logic | Business layer (`b*` projects), shared libraries, REST API handler |
| Integration Tests | Key data access paths | `dPMDAO` / stored procedure interactions |
| Smoke Tests | Critical business flows | NB, MTA, renewal, claim open/close, payment processing |

**Coverage exceptions:**

- Auto-generated code
- WinForms UI event handlers (test via integration/smoke tests instead)
- Navigator XM XML configuration files
- Trivial pass-through properties

**New code target**: All new business logic should have 90% coverage.

---

## Test Types

### Unit Tests

**Purpose**: Test individual functions, classes, or modules in isolation.

**Characteristics:**

- Fast (< 100ms per test)
- No external dependencies (database, network, file system)
- Use mocks/stubs for `dPMDAO` and external service dependencies
- Test one behaviour at a time

**When to write:**

- Business logic in `b*` VB.NET components
- Calculation and transformation functions (premium, tax, reserve calculations)
- Validation functions
- C# utility code in `Sspi.Common.*` and `SSP.PureInsuranceRestAPIHandler`

**VB.NET Unit Test Pattern (MSTest or NUnit):**

```vbnet
<TestClass>
Public Class PremiumCalculatorTests

    <TestMethod>
    Public Sub CalculatePremium_GivenValidRisk_ReturnsPremium()
        ' Arrange
        Dim dao As New MockPMDAO()
        Dim calculator As New PremiumCalculator(dao)
        Dim risk As New RiskRecord With {.SumInsured = 100000, .RiskType = "MOTOR"}

        ' Act
        Dim result = calculator.CalculatePremium(risk)

        ' Assert
        Assert.AreEqual(500.0D, result.GrossPremium)
    End Sub

    <TestMethod>
    Public Sub CalculatePremium_GivenZeroSumInsured_ThrowsArgumentException()
        ' Arrange
        Dim calculator As New PremiumCalculator(New MockPMDAO())
        Dim risk As New RiskRecord With {.SumInsured = 0}

        ' Act / Assert
        Assert.ThrowsException(Of ArgumentException)(
            Function() calculator.CalculatePremium(risk))
    End Sub

End Class
```

**C# Unit Test Pattern (xUnit or MSTest):**

```csharp
public class ApiClientTests
{
    [Fact]
    public void DeserializeJson_ValidJson_ReturnsObject()
    {
        // Arrange
        var json = "{\"access_token\":\"abc123\",\"expires_in\":3600}";

        // Act
        var result = ApiClient.DeserializeJson<TokenModel>(json);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("abc123", result.AccessToken);
    }

    [Fact]
    public void DeserializeJson_NullInput_ReturnsDefault()
    {
        var result = ApiClient.DeserializeJson<TokenModel>(null);
        Assert.Null(result);
    }
}
```

---

### Integration Tests

**Purpose**: Verify that components interact correctly with the database or external services.

**Characteristics:**

- Require a test SQL Server database with known seed data
- Test stored procedure inputs/outputs and data state changes
- Slower than unit tests — run as part of CI/CD but not on every save

**When to write:**

- Any new or modified stored procedure
- `dPMDAO` integration points
- REST API Handler token lifecycle against a test STS endpoint

**Pattern:**

```vbnet
<TestClass>
Public Class ClaimRepositoryIntegrationTests

    Private _dao As dPMDAO
    Private _testClaimRef As String = "TST-001"

    <TestInitialize>
    Public Sub Setup()
        ' Connect to test database
        _dao = New dPMDAO(TestConfiguration.ConnectionString)
    End Sub

    <TestMethod>
    Public Sub OpenClaim_ValidClaimRef_CreatesClaimRecord()
        ' Act
        Dim success = _dao.ExecuteProcedure("usp_claim_open",
            New With {.claim_ref = _testClaimRef, .user_id = 1})

        ' Assert
        Assert.IsTrue(success)
        ' Verify in DB
        Dim claim = _dao.ExecuteQuery(Of ClaimRecord)("usp_claim_get",
            New With {.claim_ref = _testClaimRef})
        Assert.AreEqual("OPEN", claim.Status)
    End Sub

    <TestCleanup>
    Public Sub Teardown()
        ' Clean up test data
        _dao.ExecuteProcedure("usp_claim_delete_test", New With {.claim_ref = _testClaimRef})
        _dao.Dispose()
    End Sub

End Class
```

---

### Smoke / End-to-End Tests

**Purpose**: Verify critical business flows work end-to-end.

**Mandatory smoke test coverage:**

| Flow | Description |
|------|-------------|
| New Business | Create a new policy (NB) through to document production |
| MTA | Apply a mid-term adjustment to an existing policy |
| Renewal | Process a renewal through the renewal workflow |
| Claim Open | Open a new claim, record a reserve, log a diary entry |
| Claim Close | Close a claim with a settled reserve |
| Payment | Record a premium payment and verify ledger |

These are run manually or via a dedicated test harness (`WindowsService.TestHarness`) prior to releases.

---

## Test Naming Convention

Use the pattern: `[MethodName]_[Scenario]_[ExpectedResult]`

```
CalculatePremium_GivenValidMotorRisk_ReturnsPremium
OpenClaim_WhenClaimAlreadyOpen_ThrowsDuplicateException
GetToken_WhenTokenExpired_RefreshesAndReturnsNewToken
ProcessPayment_WithNegativeAmount_ThrowsArgumentException
```

---

## Test Project Organisation

Place tests in a dedicated test project adjacent to the component under test:

```
Sirius For Underwriting/
  Components/
    Policy/
      bPolicy.vbproj            <- production code
      bPolicy.Tests.vbproj      <- unit tests for business layer
```

For C# libraries:
```
SSP.PureInsuranceRestAPIHandler/
  SSP.PureInsuranceRestAPIHandler/      <- production code
  SSP.PureInsuranceRestAPIHandler.Tests/ <- unit tests
```

---

## What NOT to Test

- VB.NET WinForms `InitializeComponent` generated code
- Navigator XM XML roadmap files (validated by schema, not unit tests)
- Trivial property getters/setters with no logic
- Third-party library internals (Crystal Reports, Word interop)

---

## CI/CD Integration

- Unit tests must pass before a task branch PR can be merged
- Integration tests run against a shared test database in the CI pipeline
- Test failures block deployment to `test` and `prod` environments
- Test output directory should mirror the production output path convention
