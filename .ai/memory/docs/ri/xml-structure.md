# RI XML Structure and Processing

## XML Format

The RI arrangement data is stored in session as XML with this structure:

```xml
<rows>
  <RIBAND Name="Current_1" OverrideReasonId="0">
    <ArrangementRow Name="Band Total" ... Premium="1875.00" SumInsured="250000.00" />
    <ArrangementRow Name="2014 Quota Share" Type="T" IsObligatory="True" ... />
    <ArrangementRow Name="Net of FAC" Placement="GROSS NET" ... />
    <ArrangementRow Name="proportinal xol treaty" Type="PX" ... />
    <ArrangementRow Name="2014 Liability XOL 1" Type="TX" ... />
    <ArrangementRow Name="Retained" Type="R" ... />
    <ArrangementRow Name="2011 Property Surplus MPL" Type="TFS" ... />
    <ArrangementRow Name="2025 QSH" Type="T" ManuallyAdded="True" ... />
    <ArrangementRow Name="Allocated" ... />
    <ArrangementRow Name="Unallocated" ... />
  </RIBAND>
  <RIBAND Name="Original_1" />
</rows>
```

## Node Types

| Type | Placement | Description | Processing |
|------|-----------|-------------|------------|
| T | Treaty QSH | Obligatory (IsObligatory=True) or manual | CalculateFACQSH (obligatory) or priority loop |
| TFS | Treaty Surplus | Non-obligatory surplus | Priority loop |
| TX | Treaty XOL | Non-proportional XOL | Priority loop |
| PX | Treaty Prop XOL | Proportional XOL | Priority loop |
| R | Treaty RET | Retained line | Remainder after all allocations |
| F | FAC Prop | Facultative proportional | Inline in Recalculate |
| FX | FAC XOL | Facultative XOL | Inline in Recalculate |

## Key Attributes

| Attribute | Description | Tax Impact |
|-----------|-------------|------------|
| `Tax` | Premium tax amount | Set by SAM or proportional derivation |
| `CommissionTax` | Commission tax amount | Set by SAM or proportional derivation |
| `Premium` | Premium value | Base for tax calculation |
| `Commission` | Commission value | Base for commission tax calculation |
| `CommissionPerc` | Commission percentage | Used to derive commission from premium |
| `PartyKey` | Always "0" for treaty nodes | Cannot use CalculateRITax |
| `TreatyCode` | Treaty identifier | Used by SAM to resolve party/tax group |
| `TreatyId` | Treaty numeric ID | Used by SAM to resolve party/tax group |
| `IsObligatory` | True for obligatory QSH | Processed by CalculateFACQSH |
| `ManuallyAdded` | True for user-added treaties | Different tax handling path |
| `IsEditedDB` | True when user has edited | Triggers preservation guard in Recalculate |
| `IsPremiumEdited` | True when premium was user-edited | Tells Recalculate to honour premium |

## Processing Order in Recalculate

1. **Band Total** → Read dband_si, dband_premium
2. **Obligatory T-type** → `CalculateFACQSH` (skipped in priority loop later)
3. **FAC Prop (F)** → Calculate SI/Premium from DefaultPerc against band
4. **FAC XOL (FX)** → Calculate SI from limits
5. **Net of FAC** → Create/update summary node
6. **Priority Loop** (sorted by Priority, TreatyTypeID, LineLimit):
   - Skip QSR nodes (handled later by ApplyQSRSplit)
   - Skip obligatory T nodes (already processed)
   - IsEditedDB guard → preserve persisted values, derive tax proportionally
   - Proportional (TreatyTypeID=1) → CalculateProportionalTreaty
   - Non-Proportional (TreatyTypeID=2) → CalculateNonProportionalTreaty
7. **Retained (R)** → Remainder premium = GrossPremium - SumOfNonRetPremiums
8. **QSR split** → ApplyQSRSplit on R node
9. **Allocated/Unallocated** → Summary calculation

## ManuallyAdded Attribute Gotcha

The `ManuallyAdded` attribute EXISTS on ALL nodes (even when value is "False"). 

```vb
' WRONG — enters for ManuallyAdded="False" too!
If oNode.Attributes("ManuallyAdded") IsNot Nothing Then
    ' This runs for ALL nodes!

' CORRECT — parse the boolean value
Dim bIsManuallyAdded As Boolean = False
If oNode.Attributes("ManuallyAdded") IsNot Nothing Then
    Boolean.TryParse(oNode.Attributes("ManuallyAdded").Value, bIsManuallyAdded)
End If
If bIsManuallyAdded Then ...
```

## Session Management

- `Session(CNRIXMLData)` — The RI XML string (entire arrangement)
- `Session(CNRIBandKey)` — Current band ID (e.g. "1")
- `Session(CNRIArrangementkey)` — Current arrangement ID
- `Session(CNRITransactionType)` — "NB", "MTA", etc.
- `Session(CNQuote)` — The quote object (has InsuranceFileKey, Risks)
- `Session(CNCurrentRiskKey)` — Current risk index
