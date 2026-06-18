Option Strict Off
Option Explicit On
Imports System
Module MainModule
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '

    ' Main public constant for all functions to identify which application this is.
    Public Const ACApp As String = "bSIRReinsurance"

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "MainModule"


    ' Tax transaction type constants
    Public Const TREATYPREMIUMTAXTYPE As String = "TTRITP"
    Public Const TREATYCOMMISSIONTAXTYPE As String = "TTRITC"
    Public Const FACPREMIUMTAXTYPE As String = "TTRIFP"
    Public Const FACCOMMISSIONTAXTYPE As String = "TTRIFC"



    ' ***************************************************************** '
    '                   REINSURANCE ARRAY ENUMERATORS
    ' ***************************************************************** '
    Public Enum RIArrangementEnum
        DBRIArrangementID
        DBRIBandID
        DBRIModelID
        DBRISumInsured
        DBRIPremium
        DBRIOriginalFlag
        DBRIIsModified
        DBRIFacPremiumType
        DBRIModelCode
        DBRIExtendedLimitAmount
        DBRIIsExtendedLimitApplied
        ' E007
        DBRIXOLRIModelId
        ' Max count for array manipulation
        DBRIMax = RIArrangementEnum.DBRIXOLRIModelId
    End Enum

    Public Enum RIArrangementLineEnumRI2007
        ' Grid fields
        DBRIL2007Placement
        DBRIL2007Name
        DBRIL2007Retained
        DBRIL2007DefaultShare
        DBRIL2007ThisShare
        DBRIL2007LowerLimit
        DBRIL2007LineLimit
        DBRIL2007SumInsured
        DBRIL2007Premium
        DBRIL2007PremiumTax
        DBRIL2007CommPercent
        DBRIL2007Commission
        DBRIL2007CommissionTax
        DBRIL2007AgreementCode
        DBRIL2007IsReinsurerApproved
        DBRIL2007AddedMode
        DBRIL2007GroupingID
        DBRI2007IsRIBroker
        ' Supporting fields
        DBRIL2007LineID
        DBRIL2007Type
        DBRIL2007TreatyID
        DBRIL2007PartyCnt
        DBRIL2007Priority
        DBRIL2007Lines
        'DBRI2007LineLimit
        DBRIL2007PremiumPercent
        DBRIL2007IsCommissionModified
        DBRIL2007IsCedePremiumOnly
        DBRIL2007IsObligatory
        DBRIL2007ReinsuranceTypeCode
        ' Max count for array manipulation
        DBRIL2007Max = RIArrangementLineEnumRI2007.DBRIL2007ReinsuranceTypeCode
    End Enum

    'Broker participant
    Public Const ACIBrokerShortName As Integer = 0
    Public Const ACIBrokerLongName As Integer = 1
    Public Const ACIBrokerParticipant_percent As Integer = 2
    Public Const ACIBrokerAssociationPartyCnt As Integer = 3
    Public Const ACIBrokerPartyCnt As Integer = 4
End Module