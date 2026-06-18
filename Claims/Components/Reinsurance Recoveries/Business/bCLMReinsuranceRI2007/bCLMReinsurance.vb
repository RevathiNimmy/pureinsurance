Option Strict Off
Option Explicit On
Module MainModule
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '

    ' Main public constant for all functions to identify which application this is.
    Public Const ACApp As String = "bCLMReinsurance"

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "MainModule"


    ' ***************************************************************** '
    '                   REINSURANCE ARRAY ENUMERATORS
    ' ***************************************************************** '
    Public Enum RIArrangementEnum
        DBCRIArrangementID
        DBCRIBandID
        DBCRIModelID
        DBCRISumInsured
        DBCRIReserveToDate
        DBCRIPaymentToDate
        DBCRIThisReserve
        DBCRIThisPayment
        DBCRIIsModified
        DBCRICatastropheCodeID
        DBCRIXolClmModelID
        DBCRIXolClmLimit
        DBCRIXolCatModelID
        DBCRIXolCatLimit
        DBCRIXolCatReinstatements
        DBCRIRIArrangementVersion
        DBCRIRecoveredToDate
        DBCRIXOLRIModelId
        DBCRIIncurredToDate
        ' Max count for array manipulation
        'Start-(Arul Stephen)-(Bug Fixing-PN58889)
        DBCRIMax = RIArrangementEnum.DBCRIIncurredToDate
        'End-(Arul Stephen)-(Bug Fixing-PN58889)
    End Enum



    Public Enum RI2007ArrangementLineEnum
        ' Grid fields
        DBCRIL2007Placement
        DBCRIL2007Name
        DBCRIL2007Retained
        DBCRIL2007DefaultShare
        DBCRIL2007ThisShare
        DBCRIL2007LowerLimit
        DBCRIL200UpperLimit
        DBCRIL2007SumInsured
        DBCRIL2007ReserveToDate
        DBCRIL2007ThisReserve
        DBCRIL2007PaymentToDate
        DBCRIL2007ThisPayment
        'Start-(Arul Stephen)-(Bug Fixing-PN56548)
        DBCRI2007RecoveredToDate
        'End-(Arul Stephen)-(Bug Fixing-PN56548)
        DBCRIL2007Balance
        DBCRIL2007IncurredToDate
        DBCRIL2007AgreementCode
        DBCRIL2007IsDomiciledForTax
        DBCRIL2007AddedMode
        DBCRIL2007GroupingID
        DBCRIL2007IsRIBroker
        ' Supporting fields
        DBCRIL2007LineID
        DBCRIL2007Type
        DBCRIL2007TreatyID
        DBCRIL2007PartyCnt
        DBCRIL2007XOLID
        DBCRIL2007Priority
        DBCRIL2007Lines
        DBCRIL2007LineLimit
        ' XOL fields
        DBCRIL2007Layer
        DBCRIL2007CatastropheID
        DBCRIL2007Catastrophe
        DBCRIL2007XolModelID
        DBCRIL2007XolNextModelID
        DBCRIL2007XolNextLimit
        ' Max count for array manipulation
        DBCRIL2007Max = RI2007ArrangementLineEnum.DBCRIL2007XolNextLimit
    End Enum

    'Broker participant
    Public Const ACIBrokerShortName As Integer = 0
    Public Const ACIBrokerLongName As Integer = 1
    Public Const ACIBrokerParticipant_percent As Integer = 2
    Public Const ACIBrokerAssociationPartyCnt As Integer = 3
    Public Const ACIBrokerPartyCnt As Integer = 4
End Module