Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
'Developer Guide No. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 14/09/1998
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRInsuranceFile.
    '
    ' Edit History:
    ' SJP14062002 - getUnderwritingType uses new product options scheme
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 27/10/2003
    ' Username.

    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Collection of SIRInsuranceFiles (Private)
    Private m_oSIRInsuranceFiles As bSIRInsuranceFile.SIRInsuranceFiles

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Primary Keys to work with
    Private m_lInsuranceFileCnt As Integer
    'EK 05/09/99
    Private m_lTransInsuranceFileCnt As Integer

    ' Folder Primary Key to work with
    Private m_lInsuranceFolderCnt As Integer

    'EK 05/09/99 Hold the real Insurance Folder Cnt
    Private m_lTransInsuranceFolderCnt As Integer

    Private m_bEvent As Boolean

    Private m_bEventRaised As Boolean

    Private m_vEventDescription As Object
    'eck080900
    Private m_lLastTransType As Integer
    Private m_sTransDebitCredit As String = ""

    'TN20000807
    Private m_bPMRaiseEvent As Boolean 'set to true to create event

    Private m_lPartyCnt As Integer

    Private m_bOKToDelete As Boolean

    Private m_sNoDeleteReasons As String = ""

    Private m_bCheckedOKToDelete As Boolean

    ' PM Event Business Component (Private)
    Private m_oEvent As bSIREvent.Business
    'Private m_oEvent As bSIREvent.Business

    ' CTAF 220200 - TaxRate
    Private m_oTaxRate As Object

    Private m_oTaxCalculationBusiness As Object
    Private m_lPolicyTypeId As Integer

    'JMK 13/11/2001 - Underwriting hidden option
    Private m_sUnderwritingType As String = ""
    Private m_vPolicyDeductibles As Object

    Private m_vIsManualDescription As Object

    Private m_oCollectionFrequencyID As Object
    Private m_oPaymentTermsID As Object

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = 0
                Case Is > m_oSIRInsuranceFiles.Count()
                    m_lCurrentRecord = m_oSIRInsuranceFiles.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Number in Collection
            Return m_oSIRInsuranceFiles.Count()

        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get

            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property
    'EK 05/09/99 Live Insurance File Cnt for Transactions
    Public WriteOnly Property TransInsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)

            m_lTransInsuranceFileCnt = Value

        End Set
    End Property
    Public Property InsuranceFileCnt() As Integer
        Get

            Return m_lInsuranceFileCnt

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFileCnt = Value

        End Set
    End Property

    Public Property InsuranceFolderCnt() As Integer
        Get

            Return m_lInsuranceFolderCnt

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFolderCnt = Value

        End Set
    End Property
    'EK 05/09/99 Holds the real Insurance Folder Cnt
    Public Property TransInsuranceFolderCnt() As Integer
        Get

            Return m_lTransInsuranceFolderCnt

        End Get
        Set(ByVal Value As Integer)

            m_lTransInsuranceFolderCnt = Value

        End Set
    End Property
    'eck080900
    Public Property LastTransType() As Integer
        Get

            Return m_lLastTransType

        End Get
        Set(ByVal Value As Integer)

            m_lLastTransType = Value

        End Set
    End Property
    Public Property TransDebitCredit() As String
        Get

            Return m_sTransDebitCredit

        End Get
        Set(ByVal Value As String)

            m_sTransDebitCredit = Value

        End Set
    End Property

    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
    End Property

    Public Property FromEvent() As Boolean
        Get

            Return m_bEvent

        End Get
        Set(ByVal Value As Boolean)

            m_bEvent = Value

        End Set
    End Property

    Public Property EventRaised() As Boolean
        Get

            Return m_bEventRaised

        End Get
        Set(ByVal Value As Boolean)

            m_bEventRaised = Value

        End Set
    End Property

    'TN20000807
    Public Property PMRaiseEvent() As Boolean
        Get
            Return m_bPMRaiseEvent
        End Get
        Set(ByVal Value As Boolean)
            m_bPMRaiseEvent = Value
        End Set
    End Property

    Public Property EventDescription() As Object
        Get

            Return m_vEventDescription

        End Get
        Set(ByVal Value As Object)



            m_vEventDescription = Value

        End Set
    End Property

    ' Gaurav Changed
    Public Property IsManualDescription() As Object
        Get

            Return m_vIsManualDescription

        End Get
        Set(ByVal Value As Object)



            m_vIsManualDescription = Value

        End Set
    End Property

    Public ReadOnly Property OKToDelete() As Boolean
        Get

            If Not m_bCheckedOKToDelete Then
                m_lReturn = CheckOKToDelete()
            End If

            Return m_bOKToDelete

        End Get
    End Property

    Public ReadOnly Property NoDeleteReasons() As String
        Get

            Return m_sNoDeleteReasons

        End Get
    End Property

    ' JMK 13/11/2001 "A" for Underwriting Agency and "U" for Reinsurance
    Public ReadOnly Property UnderwritingType() As String
        Get

            If m_sUnderwritingType = "" Then
                m_lReturn = CInt(getUnderwritingType())
            End If

            Return m_sUnderwritingType

        End Get
    End Property

    Public Property CollectionFrequencyID() As Object
        Get
            Return m_oCollectionFrequencyID
        End Get
        Set(ByVal Value As Object)
            m_oCollectionFrequencyID = Value
        End Set
    End Property

    Public Property PaymentTermsID() As Object
        Get
            Return m_oPaymentTermsID
        End Get
        Set(ByVal Value As Object)
            m_oPaymentTermsID = Value
        End Set
    End Property
    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel



            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now
            ' Create SIRInsuranceFiles Collection
            m_oSIRInsuranceFiles = New bSIRInsuranceFile.SIRInsuranceFiles()
            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If m_oTaxRate IsNot Nothing Then
                    m_oTaxRate.Dispose()
                    m_oTaxRate = Nothing
                End If
                If m_oEvent IsNot Nothing Then
                    m_oEvent.Dispose()
                    m_oEvent = Nothing
                End If
                m_oSIRInsuranceFiles = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a SIRClaim.
    '
    'Modification: JT : Added one more Case for PMLookUpAllWithDeleted
    ' ***************************************************************** '
    'Developer Guide No. 17
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer
        Return GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTableArray, iLanguageID:=iLanguageID, vResultArray:=vResultArray, vPolicyRelationshipType:=Nothing)
    End Function

    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object, ByRef vPolicyRelationshipType As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRInsuranceFile As bSIRInsuranceFile.SIRInsuranceFile = Nothing
        Dim dtEffectiveDate As Date

        ' {* USER DEFINED CODE (Begin) *}
        Dim vTabArray As Object
        Dim vFieldArray As Object = Nothing
        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            'Developer Guide No. 12
            vResultArray = Nothing

            'Developer Guide No. 12
            vTableArray = Nothing

            ' {* USER DEFINED CODE (Begin) *}
            ' Setup Lookup Table Names

            If Informations.IsNothing(vPolicyRelationshipType) Then
                ReDim vTabArray(3, 15)
            Else
                ReDim vTabArray(3, 16)
            End If


            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = gSIRLibrary.SIRLookupInsFileStatus

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 1) = gSIRLibrary.SIRLookupRenewalFrequency

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 2) = gSIRLibrary.SIRLookupRenewalStopCode

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 3) = gSIRLibrary.SIRLookupLapsedReason

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 4) = gSIRLibrary.SIRLookupCurrency

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 5) = gSIRLibrary.SIRLookupRiskCode

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 6) = gSIRLibrary.SIRLookupAnalysisCode

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 7) = gSIRLibrary.SIRLookupInsuranceFileStructure

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 8) = gSIRLibrary.SIRLookupInsFileType

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 9) = gSIRLibrary.SIRLookupSource

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 10) = gSIRLibrary.SIRLookupProduct

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 11) = gSIRLibrary.SIRLookupBusinessType

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 12) = gSIRLibrary.SIRLookupCollectType
            'sj 19/07/2002 - start
            'vTabArray(PMLookupTableName, 13) = SIRLookupBranch

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 13) = gSIRLibrary.SIRLookupSubBranch
            'sj 19/07/2002 - end

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 14) = "language"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 15) = gSIRLibrary.SIRLookupRenewalMethod


            If Not Informations.IsNothing(vPolicyRelationshipType) Then

                vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 16) = "policy_relationship_type"
            End If

            ' {* USER DEFINED CODE (End) *}

            ' Do we have any records
            If m_lCurrentRecord < 1 Then
                ' No, we can only lookup all
                iLookupType = gPMConstants.PMELookupType.PMLookupAll
            Else
                ' Yes get current record
                oSIRInsuranceFile = m_oSIRInsuranceFiles.Item(m_lCurrentRecord)
            End If

            Select Case iLookupType
                Case gPMConstants.PMELookupType.PMLookupAll

                    ' Do not supply a key

                    For iArrayElements As Integer = 0 To vTabArray.GetUpperBound(0)

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, iArrayElements) = ""
                    Next

                Case gPMConstants.PMELookupType.PMLookupAllEffective

                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the interface program to set the list index.
                    With oSIRInsuranceFile

                        ' {* USER DEFINED CODE (Begin) *}

                        m_lReturn = .GetProperties(r_iStatus:=gPMConstants.PMEComponentAction.PMView, r_vFieldArray:=vFieldArray)
                        ' {* USER DEFINED CODE (End) *}

                    End With

                    Dim auxVar As Object = vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID)


                    If Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID)
                    End If
                    Dim auxVar_2 As Object = vFieldArray(InsuranceFileConst.ACRenewalFrequencyID)


                    If Convert.IsDBNull(auxVar_2) Or Informations.IsNothing(auxVar_2) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = vFieldArray(InsuranceFileConst.ACRenewalFrequencyID)
                    End If
                    Dim auxVar_3 As Object = vFieldArray(InsuranceFileConst.ACRenewalStopCodeID)


                    If Convert.IsDBNull(auxVar_3) Or Informations.IsNothing(auxVar_3) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = vFieldArray(InsuranceFileConst.ACRenewalStopCodeID)
                    End If
                    'ECK 07/07/99
                    '        If IsMissing(vPolicyRelationshipType) Then
                    '            vTabArray(PMLookupKey, 3) = ""
                    '        Else
                    '            vTabArray(PMLookupKey, 3) = vPolicyRelationshipType
                    '        End If
                    Dim auxVar_4 As Object = vFieldArray(InsuranceFileConst.ACLapsedReasonID)


                    If Convert.IsDBNull(auxVar_4) Or Informations.IsNothing(auxVar_4) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 3) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 3) = vFieldArray(InsuranceFileConst.ACLapsedReasonID)
                    End If
                    Dim auxVar_5 As Object = vFieldArray(InsuranceFileConst.ACCurrencyID)


                    If Convert.IsDBNull(auxVar_5) Or Informations.IsNothing(auxVar_5) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 4) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 4) = vFieldArray(InsuranceFileConst.ACCurrencyID)
                    End If
                    Dim auxVar_6 As Object = vFieldArray(InsuranceFileConst.ACRiskCodeId)


                    If Convert.IsDBNull(auxVar_6) Or Informations.IsNothing(auxVar_6) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 5) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 5) = vFieldArray(InsuranceFileConst.ACRiskCodeId)
                    End If
                    Dim auxVar_7 As Object = vFieldArray(InsuranceFileConst.ACAnalysisCodeId)


                    If Convert.IsDBNull(auxVar_7) Or Informations.IsNothing(auxVar_7) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 6) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 6) = vFieldArray(InsuranceFileConst.ACAnalysisCodeId)
                    End If

                    Dim auxVar_8 As Object = vFieldArray(InsuranceFileConst.ACInsuranceFileStructureID)


                    If Convert.IsDBNull(auxVar_8) Or Informations.IsNothing(auxVar_8) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 7) = ""
                    Else

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 7) = vFieldArray(InsuranceFileConst.ACInsuranceFileStructureID)
                    End If
                    Dim auxVar_9 As Object = vFieldArray(InsuranceFileConst.ACInsuranceFileTypeID)


                    If Convert.IsDBNull(auxVar_9) Or Informations.IsNothing(auxVar_9) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 8) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 8) = vFieldArray(InsuranceFileConst.ACInsuranceFileTypeID)
                    End If
                    Dim auxVar_10 As Object = vFieldArray(InsuranceFileConst.ACSourceID)


                    If Convert.IsDBNull(auxVar_10) Or Informations.IsNothing(auxVar_10) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 9) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 9) = vFieldArray(InsuranceFileConst.ACSourceID)
                    End If
                    Dim auxVar_11 As Object = vFieldArray(InsuranceFileConst.ACProductID)


                    If Convert.IsDBNull(auxVar_11) Or Informations.IsNothing(auxVar_11) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 10) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 10) = vFieldArray(InsuranceFileConst.ACProductID)
                    End If
                    Dim auxVar_12 As Object = vFieldArray(InsuranceFileConst.ACBusinessTypeID)


                    If Convert.IsDBNull(auxVar_12) Or Informations.IsNothing(auxVar_12) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 11) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 11) = vFieldArray(InsuranceFileConst.ACBusinessTypeID)
                    End If
                    Dim auxVar_13 As Object = vFieldArray(InsuranceFileConst.ACCollectTypeID)


                    If Convert.IsDBNull(auxVar_13) Or Informations.IsNothing(auxVar_13) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 12) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 12) = vFieldArray(InsuranceFileConst.ACCollectTypeID)
                    End If
                    'sj 19/07/2002 - start
                    '        If IsNull(vFieldArray(ACBranchID)) Then
                    '            vTabArray(PMLookupKey, 13) = ""
                    '        Else
                    '            vTabArray(PMLookupKey, 13) = vFieldArray(ACBranchID)
                    '        End If
                    Dim auxVar_14 As Object = vFieldArray(InsuranceFileConst.ACSubBranchID)


                    If Convert.IsDBNull(auxVar_14) Or Informations.IsNothing(auxVar_14) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 13) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 13) = vFieldArray(InsuranceFileConst.ACSubBranchID)
                    End If
                    'sj 19/07/2002 - end
                    Dim auxVar_15 As Object = vFieldArray(InsuranceFileConst.ACLanguageID)


                    If Convert.IsDBNull(auxVar_15) Or Informations.IsNothing(auxVar_15) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 14) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 14) = vFieldArray(InsuranceFileConst.ACLanguageID)
                    End If
                    Dim auxVar_16 As Object = vFieldArray(InsuranceFileConst.ACRenewalMethodID)


                    If Convert.IsDBNull(auxVar_16) Or Informations.IsNothing(auxVar_16) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 15) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 15) = vFieldArray(InsuranceFileConst.ACRenewalMethodID)
                    End If


                    If Not Informations.IsNothing(vPolicyRelationshipType) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 16) = vPolicyRelationshipType
                    End If
                Case gPMConstants.PMELookupType.PMLookupAllWithDeleted

                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the interface program to set the list index.
                    With oSIRInsuranceFile

                        ' {* USER DEFINED CODE (Begin) *}

                        m_lReturn = .GetProperties(r_iStatus:=gPMConstants.PMEComponentAction.PMView, r_vFieldArray:=vFieldArray)
                        ' {* USER DEFINED CODE (End) *}

                    End With

                    Dim auxVar_17 As Object = vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID)


                    If Convert.IsDBNull(auxVar_17) Or Informations.IsNothing(auxVar_17) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""
                    Else
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID)
                    End If
                    Dim auxVar_18 As Object = vFieldArray(InsuranceFileConst.ACRenewalFrequencyID)


                    If Convert.IsDBNull(auxVar_18) Or Informations.IsNothing(auxVar_18) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = vFieldArray(InsuranceFileConst.ACRenewalFrequencyID)
                    End If
                    Dim auxVar_19 As Object = vFieldArray(InsuranceFileConst.ACRenewalStopCodeID)


                    If Convert.IsDBNull(auxVar_19) Or Informations.IsNothing(auxVar_19) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = vFieldArray(InsuranceFileConst.ACRenewalStopCodeID)
                    End If
                    'ECK 07/07/99
                    '        If IsMissing(vPolicyRelationshipType) Then
                    '            vTabArray(PMLookupKey, 3) = ""
                    '        Else
                    '            vTabArray(PMLookupKey, 3) = vPolicyRelationshipType
                    '        End If
                    Dim auxVar_20 As Object = vFieldArray(InsuranceFileConst.ACLapsedReasonID)


                    If Convert.IsDBNull(auxVar_20) Or Informations.IsNothing(auxVar_20) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 3) = ""
                    Else

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 3) = vFieldArray(InsuranceFileConst.ACLapsedReasonID)
                    End If
                    Dim auxVar_21 As Object = vFieldArray(InsuranceFileConst.ACCurrencyID)


                    If Convert.IsDBNull(auxVar_21) Or Informations.IsNothing(auxVar_21) Then
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 4) = ""
                    Else
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 4) = vFieldArray(InsuranceFileConst.ACCurrencyID)
                    End If
                    Dim auxVar_22 As Object = vFieldArray(InsuranceFileConst.ACRiskCodeId)


                    If Convert.IsDBNull(auxVar_22) Or Informations.IsNothing(auxVar_22) Then
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 5) = ""
                    Else
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 5) = vFieldArray(InsuranceFileConst.ACRiskCodeId)
                    End If
                    Dim auxVar_23 As Object = vFieldArray(InsuranceFileConst.ACAnalysisCodeId)


                    If Convert.IsDBNull(auxVar_23) Or Informations.IsNothing(auxVar_23) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 6) = ""
                    Else
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 6) = vFieldArray(InsuranceFileConst.ACAnalysisCodeId)
                    End If

                    Dim auxVar_24 As Object = vFieldArray(InsuranceFileConst.ACInsuranceFileStructureID)


                    If Convert.IsDBNull(auxVar_24) Or Informations.IsNothing(auxVar_24) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 7) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 7) = vFieldArray(InsuranceFileConst.ACInsuranceFileStructureID)
                    End If
                    Dim auxVar_25 As Object = vFieldArray(InsuranceFileConst.ACInsuranceFileTypeID)


                    If Convert.IsDBNull(auxVar_25) Or Informations.IsNothing(auxVar_25) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 8) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 8) = vFieldArray(InsuranceFileConst.ACInsuranceFileTypeID)
                    End If
                    Dim auxVar_26 As Object = vFieldArray(InsuranceFileConst.ACSourceID)


                    If Convert.IsDBNull(auxVar_26) Or Informations.IsNothing(auxVar_26) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 9) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 9) = vFieldArray(InsuranceFileConst.ACSourceID)
                    End If
                    Dim auxVar_27 As Object = vFieldArray(InsuranceFileConst.ACProductID)


                    If Convert.IsDBNull(auxVar_27) Or Informations.IsNothing(auxVar_27) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 10) = ""
                    Else
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 10) = vFieldArray(InsuranceFileConst.ACProductID)
                    End If
                    Dim auxVar_28 As Object = vFieldArray(InsuranceFileConst.ACBusinessTypeID)


                    If Convert.IsDBNull(auxVar_28) Or Informations.IsNothing(auxVar_28) Then
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 11) = ""
                    Else
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 11) = vFieldArray(InsuranceFileConst.ACBusinessTypeID)
                    End If
                    Dim auxVar_29 As Object = vFieldArray(InsuranceFileConst.ACCollectTypeID)


                    If Convert.IsDBNull(auxVar_29) Or Informations.IsNothing(auxVar_29) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 12) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 12) = vFieldArray(InsuranceFileConst.ACCollectTypeID)
                    End If
                    'sj 19/07/2002 - start
                    '        If IsNull(vFieldArray(ACBranchID)) Then
                    '            vTabArray(PMLookupKey, 13) = ""
                    '        Else
                    '            vTabArray(PMLookupKey, 13) = vFieldArray(ACBranchID)
                    '        End If
                    Dim auxVar_30 As Object = vFieldArray(InsuranceFileConst.ACSubBranchID)


                    If Convert.IsDBNull(auxVar_30) Or Informations.IsNothing(auxVar_30) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 13) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 13) = vFieldArray(InsuranceFileConst.ACSubBranchID)
                    End If
                    'sj 19/07/2002 - end
                    Dim auxVar_31 As Object = vFieldArray(InsuranceFileConst.ACLanguageID)


                    If Convert.IsDBNull(auxVar_31) Or Informations.IsNothing(auxVar_31) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 14) = ""
                    Else
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 14) = vFieldArray(InsuranceFileConst.ACLanguageID)
                    End If
                    Dim auxVar_32 As Object = vFieldArray(InsuranceFileConst.ACRenewalMethodID)


                    If Convert.IsDBNull(auxVar_32) Or Informations.IsNothing(auxVar_32) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 15) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 15) = vFieldArray(InsuranceFileConst.ACRenewalMethodID)
                    End If


                    If Not Informations.IsNothing(vPolicyRelationshipType) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 16) = vPolicyRelationshipType
                    End If

                Case gPMConstants.PMELookupType.PMLookupSingle

                    ' Set keys from current record
                    With oSIRInsuranceFile

                        ' {* USER DEFINED CODE (Begin) *}

                        m_lReturn = .GetProperties(r_iStatus:=gPMConstants.PMEComponentAction.PMView, r_vFieldArray:=vFieldArray)
                        ' {* USER DEFINED CODE (End) *}

                    End With

                    Dim auxVar_33 As Object = vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID)


                    If Convert.IsDBNull(auxVar_33) Or Informations.IsNothing(auxVar_33) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID)
                    End If
                    Dim auxVar_34 As Object = vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID)


                    If Convert.IsDBNull(auxVar_34) Or Informations.IsNothing(auxVar_34) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID)
                    End If
                    Dim auxVar_35 As Object = vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID)


                    If Convert.IsDBNull(auxVar_35) Or Informations.IsNothing(auxVar_35) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 2) = vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID)
                    End If
                    'ECK 7/7/99
                    '        If IsMissing(vPolicyRelationshipType) Then
                    '            vTabArray(PMLookupKey, 3) = ""
                    '        Else
                    '            vTabArray(PMLookupKey, 3) = vPolicyRelationshipType
                    '        End If
                    Dim auxVar_36 As Object = vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID)


                    If Convert.IsDBNull(auxVar_36) Or Informations.IsNothing(auxVar_36) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 3) = ""
                    Else
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 3) = vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID)
                    End If
                    Dim auxVar_37 As Object = vFieldArray(InsuranceFileConst.ACCurrencyID)


                    If Convert.IsDBNull(auxVar_37) Or Informations.IsNothing(auxVar_37) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 4) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 4) = vFieldArray(InsuranceFileConst.ACCurrencyID)
                    End If
                    Dim auxVar_38 As Object = vFieldArray(InsuranceFileConst.ACRiskCodeId)


                    If Convert.IsDBNull(auxVar_38) Or Informations.IsNothing(auxVar_38) Then
                        'ECK 07/07/99 Added a default

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 5) = 1
                    Else
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 5) = vFieldArray(InsuranceFileConst.ACRiskCodeId)
                    End If

                    Dim auxVar_39 As Object = vFieldArray(InsuranceFileConst.ACAnalysisCodeId)

                    If Convert.IsDBNull(auxVar_39) Or Informations.IsNothing(auxVar_39) Then
                        'ECK 07/07/99 Added a default
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 6) = 6
                    Else
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 6) = vFieldArray(InsuranceFileConst.ACAnalysisCodeId)
                    End If

                    Dim auxVar_40 As Object = vFieldArray(InsuranceFileConst.ACInsuranceFileStructureID)


                    If Convert.IsDBNull(auxVar_40) Or Informations.IsNothing(auxVar_40) Then
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 7) = ""
                    Else
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 7) = vFieldArray(InsuranceFileConst.ACInsuranceFileStructureID)
                    End If

                    Dim auxVar_41 As Object = vFieldArray(InsuranceFileConst.ACInsuranceFileTypeID)

                    If Convert.IsDBNull(auxVar_41) Or Informations.IsNothing(auxVar_41) Then
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 8) = ""
                    Else
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 8) = vFieldArray(InsuranceFileConst.ACInsuranceFileTypeID)
                    End If

                    Dim auxVar_42 As Object = vFieldArray(InsuranceFileConst.ACSourceID)

                    If Convert.IsDBNull(auxVar_42) Or Informations.IsNothing(auxVar_42) Then
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 9) = ""
                    Else
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 9) = vFieldArray(InsuranceFileConst.ACSourceID)
                    End If

                    Dim auxVar_43 As Object = vFieldArray(InsuranceFileConst.ACProductID)

                    If Convert.IsDBNull(auxVar_43) Or Informations.IsNothing(auxVar_43) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 10) = ""
                    Else

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 10) = vFieldArray(InsuranceFileConst.ACProductID)
                    End If

                    Dim auxVar_44 As Object = vFieldArray(InsuranceFileConst.ACBusinessTypeID)

                    If Convert.IsDBNull(auxVar_44) Or Informations.IsNothing(auxVar_44) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 11) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 11) = vFieldArray(InsuranceFileConst.ACBusinessTypeID)
                    End If
                    Dim auxVar_45 As Object = vFieldArray(InsuranceFileConst.ACCollectTypeID)


                    If Convert.IsDBNull(auxVar_45) Or Informations.IsNothing(auxVar_45) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 12) = ""
                    Else
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 12) = vFieldArray(InsuranceFileConst.ACCollectTypeID)
                    End If
                    'sj 19/07/2002 - start
                    '        If IsNull(vFieldArray(ACBranchID)) Then
                    '            vTabArray(PMLookupKey, 13) = ""
                    '        Else
                    '            vTabArray(PMLookupKey, 13) = vFieldArray(ACBranchID)
                    '        End If
                    Dim auxVar_46 As Object = vFieldArray(InsuranceFileConst.ACSubBranchID)


                    If Convert.IsDBNull(auxVar_46) Or Informations.IsNothing(auxVar_46) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 13) = ""
                    Else
                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 13) = vFieldArray(InsuranceFileConst.ACSubBranchID)
                    End If
                    'sj 19/07/2002 - end
                    Dim auxVar_47 As Object = vFieldArray(InsuranceFileConst.ACLanguageID)


                    If Convert.IsDBNull(auxVar_47) Or Informations.IsNothing(auxVar_47) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 14) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 14) = vFieldArray(InsuranceFileConst.ACLanguageID)
                    End If
                    Dim auxVar_48 As Object = vFieldArray(InsuranceFileConst.ACRenewalMethodID)


                    If Convert.IsDBNull(auxVar_48) Or Informations.IsNothing(auxVar_48) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 15) = ""
                    Else


                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 15) = vFieldArray(InsuranceFileConst.ACRenewalMethodID)
                    End If

                    If Not Informations.IsNothing(vPolicyRelationshipType) Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 16) = vPolicyRelationshipType
                    End If

            End Select

            ' Default Effective Date to current date/time
            dtEffectiveDate = DateTime.Now

            ' Release SIRInsuranceFile reference
            oSIRInsuranceFile = Nothing
            ' PM Lookup Business Component (Private)
            Dim m_oLookup As BPMLOOKUP.Business
            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business
            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=m_sUsername,
                                             sPassword:=m_sPassword,
                                             iUserID:=m_iUserID,
                                             iSourceID:=m_iSourceID,
                                             iLanguageID:=m_iLanguageID,
                                             iCurrencyID:=m_iCurrencyID,
                                             iLogLevel:=m_iLogLevel,
                                             sCallingAppName:=ACApp,
                                             vDatabase:=m_oDatabase)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            ' Get the Lookup items

            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTabArray, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Table Array


            vTableArray = vTabArray

            vFieldArray = Nothing
            ' Destroy Lookup object
            'm_lReturn = m_oLookup.Terminate()
            m_oLookup.Dispose()
            m_oLookup = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'eck200400
    ' ***************************************************************** '
    ' Name: GetValidRiskCodes (Public)
    '
    ' Description: Gets the Lookup values for a SIRClaim.
    '
    ' Datasure Pass country Id
    ' ***************************************************************** '
    Public Function GetValidRiskCodes(ByRef iSourceID As Integer, ByRef iCountryId As Integer, ByRef vRiskCodes(,) As Object) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            With m_oDatabase

                .Parameters.Clear()


                m_lReturn = .Parameters.Add(sName:="source_id", vValue:=CStr(iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                '
                m_lReturn = .Parameters.Add(sName:="country_id", vValue:=CStr(iCountryId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Get the risk codes for the party
                m_lReturn = .SQLSelect(sSQL:=ACSelectRiskCodesForBranchSQL, sSQLName:=ACSelectRiskCodesForBranchName, bStoredProcedure:=ACSelectRiskCodesForBranchStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vRiskCodes)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With
            'return the data
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetValidRiskCodes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidRiskCodes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name : GetMidnightRenewal
    '
    ' Desc : Are we renewing at midnight?
    '
    ' Hist : 10 October 2001 Created - Tom
    '
    ' ***************************************************************** '
    Public Function GetMidnightRenewal(ByVal v_lProductId As Integer, ByRef r_iMidnightRenewal As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get is_midnight_renewal from the product table
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=CStr(v_lProductId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectIsMidnightRenewalSQL, sSQLName:=ACSelectIsMidnightRenewalName, bStoredProcedure:=ACSelectIsMidnightRenewalStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                r_iMidnightRenewal = 0
            Else
                Dim auxVar As Object = vResultArray(0, 0)


                If Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar) Then
                    r_iMidnightRenewal = 0
                Else

                    Dim dbNumericTemp As Double
                    If Double.TryParse(CStr(vResultArray(0, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                        r_iMidnightRenewal = CInt(vResultArray(0, 0))
                    Else
                        r_iMidnightRenewal = 0
                    End If
                End If
            End If


            'Developer Guide No. 12
            vResultArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMidnightRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMidnightRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name : GetMidnightRenewalSFB
    '
    ' Desc : Are we renewing at midnight?
    '
    ' Hist : 04/2005 - 2005Roadmap ECK
    '
    ' ***************************************************************** '
    Public Function GetMidnightRenewalSFB(ByVal m_lRiskCodeID As Integer, ByRef r_bMidnightRenewal As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get is_midnight_renewal from the product table
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_code_id", vValue:=CStr(m_lRiskCodeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectIsMidnightRenewalSFBSQL, sSQLName:=ACSelectIsMidnightRenewalSFBName, bStoredProcedure:=ACSelectIsMidnightRenewalSFBStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                r_bMidnightRenewal = 0
            Else
                Dim auxVar As Object = vResultArray(0, 0)


                If Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar) Then
                    r_bMidnightRenewal = 0
                Else

                    Dim dbNumericTemp As Double
                    If Double.TryParse(CStr(vResultArray(0, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                        r_bMidnightRenewal = CBool(vResultArray(0, 0))
                    Else
                        r_bMidnightRenewal = 0
                    End If
                End If
            End If


            'Developer Guide No. 12
            vResultArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMidnightRenewalSFB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMidnightRenewalSFB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetMonthsFromRenewalFrequency
    '
    ' Description: Gets the month from renewal frequency.
    '
    ' ***************************************************************** '
    Public Function GetMonthsFromRenewalFrequency(ByRef v_lId As Integer, ByRef r_lMonths As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()
                sSQL = "Select number_of_months from renewal_frequency where renewal_frequency_id = " & v_lId

                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetMonthsFromRenewalFrequency", bStoredProcedure:=False, vResultArray:=vResultArray)
            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If
            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            Else

                r_lMonths = CInt(vResultArray(0, 0))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetMonthsFromRenewalFrequency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMonthsFromRenewalFrequencyv", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name : GetInsuredName
    '
    ' Desc : What's the insured name?
    '
    ' Hist : 10 October 2001 Created - Tom
    '
    ' ***************************************************************** '
    'Developer Guide No 101
    Public Function GetInsuredName(ByVal v_lPartyCnt As Integer, ByRef r_vInsuredName As Object) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get is_midnight_renewal from the product table
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectInsuredNameSQL, sSQLName:=ACSelectInsuredNameName, bStoredProcedure:=ACSelectInsuredNameStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                r_vInsuredName = ""
            Else

                r_vInsuredName = CStr(vResultArray(0, 0))
            End If


            'Developer Guide No. 12
            vResultArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsuredName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsuredName", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single SIRInsuranceFile directly into the database.
    '        Note: The SIRInsuranceFile will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(ByVal v_vFieldArray As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRInsuranceFile As bSIRInsuranceFile.SIRInsuranceFile
        Dim lEventCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new SIRInsuranceFile
            oSIRInsuranceFile = New bSIRInsuranceFile.SIRInsuranceFile()
            m_lReturn = oSIRInsuranceFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            ' Populate SIRInsuranceFile Attributes

            m_lReturn = oSIRInsuranceFile.SetProperties(v_iStatus:=gPMConstants.PMEComponentAction.PMAdd, v_vFieldArray:=v_vFieldArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRInsuranceFile = Nothing
                Return result
            End If

            'sj 28/9/99 - start
            oSIRInsuranceFile.InsuranceFolderCnt = InsuranceFolderCnt
            'sj 28/9/99 - end

            'DN 16/08/01
            oSIRInsuranceFile.TransInsuranceFolderCnt = TransInsuranceFolderCnt

            ' Add the SIRInsuranceFile to the Database
            m_lReturn = oSIRInsuranceFile.AddItem()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRInsuranceFile = Nothing
                Return result
            End If

            ' Retain the Primary Key of the SIRInsuranceFile Added
            With oSIRInsuranceFile
                InsuranceFileCnt = .InsuranceFileCnt
            End With

            Const DOCUMET_ARCHIVE As Integer = 10
            Dim sDocumentArchive As String = ""
            ' Get system option to check what is selected in dropdown 'Document Archive' in system option
            m_lReturn = CType(GetOption(v_iOptionNumber:=DOCUMET_ARCHIVE, r_sOptionValue:=sDocumentArchive), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sDocumentArchive = "2" AndAlso m_sCallingAppName <> "iPMUDeferredRIAuto" Then
                'Generate a default Sharepoint folder (if Sharepoint is enabled)
                Dim Sharepoint As bSIRSharepoint.Business
                Sharepoint = New bSIRSharepoint.Business
                Sharepoint.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

                Sharepoint.GenerateDefaultPath(CInt(v_vFieldArray(InsuranceFileConst.ACInsuredCnt)), InsuranceFileCnt, 0, 0)
            End If

            If (EventDescription = "Quotation record created") Then
                m_lReturn = CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=CInt(v_vFieldArray(InsuranceFileConst.ACInsuredCnt)), v_vInsuranceFolderCnt:=InsuranceFolderCnt,
                            v_vInsuranceFileCnt:=oSIRInsuranceFile.InsuranceFileCnt, v_vClaimCnt:=DBNull.Value, v_vDocumentCnt:=DBNull.Value, v_vOldAddressCnt:=DBNull.Value,
                            v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentTypeId:=DBNull.Value, v_vReportTypeId:=DBNull.Value,
                            v_lEventTypeId:=PMBConst.PMBEventNewPolicy, v_dtEventDate:=DateTime.Today, v_vDescription:=EventDescription)
            Else
                m_lReturn = CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=CInt(v_vFieldArray(InsuranceFileConst.ACInsuredCnt)), v_vInsuranceFolderCnt:=InsuranceFolderCnt,
                            v_vInsuranceFileCnt:=oSIRInsuranceFile.InsuranceFileCnt, v_vClaimCnt:=DBNull.Value, v_vDocumentCnt:=DBNull.Value, v_vOldAddressCnt:=DBNull.Value,
                            v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentTypeId:=DBNull.Value, v_vReportTypeId:=DBNull.Value,
                            v_lEventTypeId:=PMBConst.PMBEventPolChange, v_dtEventDate:=DateTime.Today, v_vDescription:=EventDescription)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                'Don't exit - need to do the setting to nothing a little lower
            End If

            oSIRInsuranceFile = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single SIRInsuranceFile directly from the database.
    '        Note: The SIRInsuranceFile will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete() As Integer
        Return DirectDelete(vInsuranceFileCnt:=Nothing)
    End Function

    Public Function DirectDelete(ByRef vInsuranceFileCnt As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRInsuranceFile As bSIRInsuranceFile.SIRInsuranceFile
        Dim vFieldArray() As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new SIRInsuranceFile
            oSIRInsuranceFile = New bSIRInsuranceFile.SIRInsuranceFile()
            m_lReturn = oSIRInsuranceFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            ReDim vFieldArray(InsuranceFileConst.ACFieldArraySize)


            vFieldArray(InsuranceFileConst.ACInsuranceFileCnt) = vInsuranceFileCnt


            m_lReturn = oSIRInsuranceFile.SetProperties(v_iStatus:=gPMConstants.PMEComponentAction.PMDelete, v_vFieldArray:=vFieldArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRInsuranceFile = Nothing
                Return result
            End If

            ' Delete the SIRInsuranceFile from the Database
            m_lReturn = oSIRInsuranceFile.DeleteItem()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRInsuranceFile = Nothing
                Return result
            End If

            oSIRInsuranceFile = Nothing

            vFieldArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckID (Public)
    '
    ' Description: Checks to see if the supplied ID is a valid record.
    '
    ' ***************************************************************** '
    Public Function CheckID(ByRef vID As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the ID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="id", vValue:=CStr(vID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckIDSQL, sSQLName:=ACCheckIDName, bStoredProcedure:=ACCheckIDStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount < 1 Then
                ' No record found
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required SIRInsuranceFiles and populate the Collection
    '
    ' ***************************************************************** '
    'Developer Guide No 101
    Public Function GetDetails(Optional ByRef vLockMode As Integer = 0, Optional ByRef vInsuranceFileCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        'Developer Guide No 21. 
        Dim oFields As DataRow
        Dim oSIRInsuranceFile As bSIRInsuranceFile.SIRInsuranceFile

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oSIRInsuranceFiles.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            ' Check for Valid Primary Key
            Dim dbNumericTemp2 As Double

            If (Not Informations.IsNothing(vInsuranceFileCnt)) And (Not Double.TryParse(CStr(vInsuranceFileCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vInsuranceFileCnt=" & vInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If


            If Not Informations.IsNothing(vInsuranceFileCnt) Then

                ' Create New SIRInsuranceFile
                oSIRInsuranceFile = New bSIRInsuranceFile.SIRInsuranceFile()
                m_lReturn = oSIRInsuranceFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


                ' Set component primary keys
                With oSIRInsuranceFile
                    .InsuranceFileCnt = vInsuranceFileCnt

                    'And if we're coming from events
                    .FromEvent = FromEvent

                    m_lReturn = .SelectItem()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lInsuranceFolderCnt = .InsuranceFolderCnt
                End With

                ' Add SIRInsuranceFile to collection
                If m_oSIRInsuranceFiles.Count = 0 Then
                    m_oSIRInsuranceFiles.Add(Nothing)
                End If
                m_lReturn = m_oSIRInsuranceFiles.Add(oNewSIRInsuranceFile:=oSIRInsuranceFile)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oSIRInsuranceFile = Nothing

            Else

                ' No Key, Get All Records

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' How many records were selected
                lRecordCount = m_oDatabase.Records.Count()

                ' Do we have any records ?
                If lRecordCount < 1 Then
                    ' No Records, return PMFalse
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

                ' Yes, load them into the collection
                For lSub As Integer = 1 To lRecordCount

                    ' Create New
                    oSIRInsuranceFile = New bSIRInsuranceFile.SIRInsuranceFile()
                    'Developer Guide No 9

                    m_lReturn = oSIRInsuranceFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

                    ' Set oFields to refer to one Record
                    oFields = m_oDatabase.Records.Item(lSub - 1).Fields()

                    ' Set component primary keys from current record
                    With oSIRInsuranceFile
                        .InsuranceFileCnt = gPMFunctions.NullToLong(oFields("insurance_file_cnt"))

                        m_lReturn = .SelectItem()

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End With

                    ' Add SIRInsuranceFile to collection
                    If m_oSIRInsuranceFiles.Count = 0 Then
                        m_oSIRInsuranceFiles.Add(Nothing)
                    End If
                    m_lReturn = m_oSIRInsuranceFiles.Add(oNewSIRInsuranceFile:=oSIRInsuranceFile)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oSIRInsuranceFile = Nothing
                Next lSub
            End If

            m_lReturn = CInt(getUnderwritingType())

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetUnderwritingYear(ByVal dtDate As Date, ByRef r_lUnderwritingYearID As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetUnderwritingYear
        ' PURPOSE: Returns the Underwriting Year or Zero if not found
        ' AUTHOR: Danny Davis
        ' DATE: 01 April 2004, 11:23:33
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()
                'Developer Guide No. 40
                .Parameters.Add("required_date", dtDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)

                'Developer Guide No.85
                .Parameters.Add("underwriting_year_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
                'Developer Guide No 39. 

                m_lReturn = .SQLSelect("spu_get_underwriting_year", "Get Underwriting Year", True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Get the found value - not found returns zero
                r_lUnderwritingYearID = gPMFunctions.NullToLong(.Parameters.Item("underwriting_year_id").Value)
            End With
            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUnderwritingYear", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally

        End Try
        Return result
    End Function

    Public Function GetUnderwritingYearForNB(ByVal vInsuranceFolderCnt As Integer, ByRef r_lUnderwritingYearID As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetUnderwritingYearForNB
        ' PURPOSE: Returns the Underwriting Year from the New Business record
        ' AUTHOR: Danny Davis
        ' DATE: 01 April 2004, 11:23:33
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("insurance_folder_cnt", CStr(vInsuranceFolderCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'Developer Guide No. 85
                .Parameters.Add("underwriting_year_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
                'Developer Guide No 39. 

                m_lReturn = .SQLSelect("spu_get_underwriting_year_for_nb", "Get Underwriting Year for NB", True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Get the found value - not found returns zero
                r_lUnderwritingYearID = gPMFunctions.NullToLong(.Parameters.Item("underwriting_year_id").Value)
            End With
            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUnderwritingYearForNB", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AddSubAgents
    '
    ' Description: Add all sub agents for insurance_file_cnt
    '
    ' History : 18/08/2000 Tinny (Created)
    '
    ' ***************************************************************** '
    Public Function AddSubAgents(ByVal v_vInsuranceFileCnt As Object, ByRef r_vValueArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Dim dbNumericTemp As Double
            If (False Or False) Or (Not False And Not Double.TryParse(CStr(v_vInsuranceFileCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Invalid parameter list", vApp:=ACApp, vClass:=ACClass, vMethod:="AddSubAgents", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

            End If

            'AG - 28/10/2004 - PN 15991 - START
            'Delete all existing subagents.
            m_lReturn = BeginTrans()

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteSubAgentSQL, sSQLName:=ACDeleteSubAgentName, bStoredProcedure:=ACDeleteSubAgentStored)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'AG - 28/10/2004 - PN 15991 - END

            If Informations.IsArray(r_vValueArray) Then

                'add all subagents

                For lCount As Integer = 0 To r_vValueArray.GetUpperBound(1)
                    m_oDatabase.Parameters.Clear()


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit For
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(r_vValueArray(0, lCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit For
                    End If



                    m_lReturn = m_oDatabase.Parameters.Add(sName:="percentage", vValue:=CStr(r_vValueArray(3, lCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit For
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="amount", vValue:=CStr(r_vValueArray(4, lCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)


                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit For
                    End If

                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSubAgentSQL, sSQLName:=ACAddSubAgentName, bStoredProcedure:=ACAddSubAgentStored)


                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit For
                    End If

                Next

            End If

            'AG - 28/10/2004 - PN 15991 - START
            'Commit all changes in data
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                m_lReturn = CommitTrans()
            End If
            'AG - 28/10/2004 - PN 15991 - END

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddSubAgents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddSubAgents", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetSubAgents
    '
    ' Description: get all sub agents for insurance_file_cnt
    '
    ' History : 18/08/2000 Tinny (Created)
    '
    ' ***************************************************************** '
    Public Function GetSubAgents(ByVal v_vInsuranceFileCnt As Object, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Dim dbNumericTemp As Double
            If (False Or False) Or (Not False And Not Double.TryParse(CStr(v_vInsuranceFileCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Invalid parameter list", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubAgents", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Else
                m_oDatabase.Parameters.Clear()


                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectSubAgentSQL, sSQLName:=ACSelectSubAgentName, bStoredProcedure:=ACSelectSubAgentStored, vResultArray:=r_vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSubAgents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubAgents", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetFromTable
    '
    ' Description: get v_vFieldName from v_vTableName where v_vKeyField = v_vKeyID
    '
    ' History : 16/08/2000 Tinny (Created)
    '
    ' ***************************************************************** '
    'Developer Guide No 101
    Public Function GetFromTable(ByVal v_vTableName As Object, ByVal v_vFieldName As Object, ByVal v_vKeyField As Object, ByVal v_vKeyID As Object, ByRef r_vResult As Object) As Integer


        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If False Or False Or False Or False Or False Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Invalid parameter list", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFromTable", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

            End If

            sSQL = ""
            sSQL = sSQL & "SELECT " & v_vFieldName & " FROM " & v_vTableName & New String(" "c, 1)
            sSQL = sSQL & "WHERE " & v_vKeyField & " = " & v_vKeyID

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="SelectAFieldFromParty", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                'there should only be one record
                If Informations.IsArray(vResultArray) Then


                    r_vResult = vResultArray(0, 0)
                End If
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFromTable Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFromTable", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: GetLeadAgentUsingAgentCnt
    '
    ' Description: Get resolved_name, agent_cnt using agent_cnt of passed in party_cnt
    '
    ' History : 15/08/2000 Tinny (Created)
    '
    ' ***************************************************************** '
    'Developer Guide No 101
    Public Function GetLeadAgentUsingAgentCnt(ByVal v_vPartyCnt As Object, ByRef r_vResolvedName As String, ByRef r_vLeadAgentCnt As Object) As Integer
        Return GetLeadAgentUsingAgentCnt(v_vPartyCnt:=v_vPartyCnt, r_vResolvedName:=r_vResolvedName, r_vLeadAgentCnt:=r_vLeadAgentCnt, r_vDateCancelled:=Nothing)
    End Function

    Public Function GetLeadAgentUsingAgentCnt(ByVal v_vPartyCnt As Object, ByRef r_vResolvedName As String, ByRef r_vLeadAgentCnt As Object, ByRef r_vDateCancelled As Object) As Integer 'Moh 15-05-2003

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Dim dbNumericTemp As Double
            If (False Or False Or False) Or (Not False And Not Double.TryParse(CStr(v_vPartyCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Must pass in v_vPartyCnt, r_vResolvedName, r_vLeadAgentCnt and v_PartyCnt must be valid.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLeadAgentUsingAgentCnt", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

            End If


            If m_oDatabase.Parameters.Add("Party_Cnt", CStr(v_vPartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception()
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectLeadAgentUsingAgentCntSQL, sSQLName:=ACSelectLeadAgentUsingAgentCntName, bStoredProcedure:=ACSelectLeadAgentUsingAgentCntStored, lNumberRecords:=1, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                If Informations.IsArray(vResultArray) Then
                    'There should only be one record

                    r_vResolvedName = "" & CStr(vResultArray(0, 0))

                    r_vLeadAgentCnt = CStr(vResultArray(1, 0))
                    'Moh 15-05-2003

                    If vResultArray.GetUpperBound(0) > 1 Then

                        r_vDateCancelled = CStr(vResultArray(2, 0))
                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLeadAgentUsingAgentCnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLeadAgentUsingAgentCnt", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: IsAgentAllowCommissionUsingAgentCnt
    '
    ' Description: To Check that is Agent is Allowed ConsolidatedCommission
    '
    ' History : 30/06/2006 Created Deepak
    '
    '
    ' ***************************************************************** '
    Public Function IsAgentAllowCommissionUsingAgentCnt(ByVal v_vPartyCnt As Object, ByRef r_vAgentallowedCommission As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            If m_oDatabase.Parameters.Add("Party_Cnt", CStr(v_vPartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception()
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectAgentAllowCommissionusingAgentCntSQL, sSQLName:=ACSelectAgentAllowCommissionusingAgentCntName, bStoredProcedure:=ACSelectAgentAllowCommissionusingAgentCntStored, lNumberRecords:=1, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                If Informations.IsArray(vResultArray) Then
                    'There should only be one record

                    r_vAgentallowedCommission = CInt("" & CStr(vResultArray(0, 0)))
                End If
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsAgentAllowCommissionUsingAgentCnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsAgentAllowCommissionUsingAgentCnt", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetOtherDetails
    '
    ' Description: Get other details from DB, for related parties.
    '
    ' eck070301 Get the new Risk_Cnt for Risk Screens
    ' eck090301 Make this parameter optional
    ' SJP (CMG)     07/04/2003      PS235
    ' ***************************************************************** '
    'Developer Guide No 17 and 101
    'STRING CAN NOT CONVERTED INTO DBNULL
    Public Function GetOtherDetails(ByRef vInsurerCnt As Object, ByRef vInsurerName As Object, ByRef vBrokerCnt As Object, ByRef vBrokerName As Object, ByRef vRiskId As Object, ByRef vRiskDesc As Object, ByRef vRiskGroupId As Object, ByRef vAnalysisId As Object, ByRef vAnalysisDesc As Object, ByRef vHandlerCnt As Object, ByRef vHandlerName As Object, ByRef vAgentCnt As Object, ByRef vAgentName As Object, ByRef vInsuranceFileCnt As Object, ByRef vRelatedPolicyCnt As Object, ByRef vRelatedPolicyCode As Object, ByRef vRelationshipType As Object, ByRef vPolicyTypeId As Object, ByRef vPolicyTypeDesc As Object, ByRef vSchemeId As Integer, ByRef vSchemeDesc As Object, Optional ByRef vPolicyDeductiblesId As Object = Nothing, Optional ByRef vPolicyDeductibles As Object = Nothing, Optional ByRef vPolicyLimitsId As Object = Nothing, Optional ByRef vPolicyLimits As Object = Nothing, Optional ByRef vRiskCnt As Object = Nothing, Optional ByRef vExecutiveCnt As Object = Nothing, Optional ByRef vExecutiveName As String = "", Optional ByRef vFSAUnderwriterCnt As Object = Nothing, Optional ByRef vFSAUnderwriterName As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Not (Convert.IsDBNull(vInsurerCnt) Or Informations.IsNothing(vInsurerCnt)) Then

                'Get from DB

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartyDetailsSQL & CStr(vInsurerCnt), sSQLName:=ACGetPartyDetailsName, bStoredProcedure:=ACGetPartyDetailsStored, lNumberRecords:=1, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'return the data
                If Not Informations.IsArray(vResultArray) Then
                    vInsurerName = ""
                Else

                    vInsurerName = CStr(vResultArray(0, 0)).Trim()
                End If
            End If


            If Not (Convert.IsDBNull(vBrokerCnt) Or Informations.IsNothing(vBrokerCnt)) Then

                'Get from DB

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartyDetailsSQL & CStr(vBrokerCnt), sSQLName:=ACGetPartyDetailsName, bStoredProcedure:=ACGetPartyDetailsStored, lNumberRecords:=1, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'return the data
                If Not Informations.IsArray(vResultArray) Then
                    vBrokerName = ""
                Else

                    vBrokerName = CStr(vResultArray(0, 0)).Trim()
                End If
            End If


            If Not (Convert.IsDBNull(vHandlerCnt) Or Informations.IsNothing(vHandlerCnt)) Then

                'Get from DB

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartyDetailsSQL & CStr(vHandlerCnt), sSQLName:=ACGetPartyDetailsName, bStoredProcedure:=ACGetPartyDetailsStored, lNumberRecords:=1, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'return the data
                If Not Informations.IsArray(vResultArray) Then
                    vHandlerName = ""
                Else

                    vHandlerName = CStr(vResultArray(0, 0)).Trim()
                End If
            End If

            ' SJP (CMG) 07042003 PS235

            If Not Informations.IsNothing(vExecutiveCnt) Then

                If Not (Convert.IsDBNull(vExecutiveCnt) Or Informations.IsNothing(vExecutiveCnt)) Then

                    'Get from DB

                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartyDetailsSQL & CStr(vExecutiveCnt), sSQLName:=ACGetPartyDetailsName, bStoredProcedure:=ACGetPartyDetailsStored, lNumberRecords:=1, vResultArray:=vResultArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'return the data
                    If Not Informations.IsArray(vResultArray) Then
                        vExecutiveName = ""
                    Else

                        vExecutiveName = CStr(vResultArray(0, 0)).Trim()
                    End If
                End If
            End If

            ' FSA Phase III

            If Not Informations.IsNothing(vFSAUnderwriterCnt) Then

                If Not (Convert.IsDBNull(vFSAUnderwriterCnt) Or Informations.IsNothing(vFSAUnderwriterCnt)) Then

                    'Get from DB

                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartyDetailsSQL & CStr(vFSAUnderwriterCnt), sSQLName:=ACGetPartyDetailsName, bStoredProcedure:=ACGetPartyDetailsStored, lNumberRecords:=1, vResultArray:=vResultArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'return the data
                    If Not Informations.IsArray(vResultArray) Then
                        vFSAUnderwriterName = ""
                    Else

                        vFSAUnderwriterName = CStr(vResultArray(0, 0)).Trim()
                    End If
                End If
            End If
            'FSA Phase III End


            If Not (Convert.IsDBNull(vAgentCnt) Or Informations.IsNothing(vAgentCnt)) Then

                'Get from DB

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartyDetailsSQL & CStr(vAgentCnt), sSQLName:=ACGetPartyDetailsName, bStoredProcedure:=ACGetPartyDetailsStored, lNumberRecords:=1, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'return the data
                If Not Informations.IsArray(vResultArray) Then
                    vAgentName = ""
                Else

                    vAgentName = CStr(vResultArray(0, 0)).Trim()
                End If
            End If


            If Not (Convert.IsDBNull(vRiskId) Or Informations.IsNothing(vRiskId)) Then

                'Get from DB

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskDetailsSQL & CStr(vRiskId), sSQLName:=ACGetRiskDetailsName, bStoredProcedure:=ACGetRiskDetailsStored, lNumberRecords:=1, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'return the data
                If Not Informations.IsArray(vResultArray) Then
                    vRiskDesc = ""
                    vRiskGroupId = 0
                Else

                    vRiskDesc = CStr(vResultArray(0, 0)).Trim()

                    vRiskGroupId = CInt(vResultArray(1, 0))
                End If
            End If


            If Not (Convert.IsDBNull(vAnalysisId) Or Informations.IsNothing(vAnalysisId)) Then

                'Get from DB

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAnalysisDetailsSQL & CStr(vAnalysisId), sSQLName:=ACGetAnalysisDetailsName, bStoredProcedure:=ACGetAnalysisDetailsStored, lNumberRecords:=1, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'return the data
                If Not Informations.IsArray(vResultArray) Then
                    vAnalysisDesc = ""
                Else

                    vAnalysisDesc = CStr(vResultArray(0, 0)).Trim()
                End If
            End If

            If Not Informations.IsNothing(vPolicyDeductiblesId) Then

                If Not (Convert.IsDBNull(vPolicyDeductiblesId) Or Informations.IsNothing(vPolicyDeductiblesId)) Then
                    'Get from DB


                    'Developer Guide No. 98

                    m_lReturn = GetPolicyDeductibleDesc(v_iPolicyDeductiblesId:=vPolicyDeductiblesId, v_vPolicyDeductibles:=vPolicyDeductibles)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If
            End If


            If Not Informations.IsNothing(vPolicyLimitsId) Then

                If Not (Convert.IsDBNull(vPolicyLimitsId) Or Informations.IsNothing(vPolicyLimitsId)) Then
                    'Get from DB


                    'Developer Guide No. 98

                    m_lReturn = GetPolicyLimitsDesc(v_iPolicyLimitsId:=vPolicyLimitsId, v_vPolicyLimits:=vPolicyLimits)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If
            End If
            'Get from DB

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRelationshipDetailsSQL & CStr(vInsuranceFileCnt), sSQLName:=ACGetRelationshipDetailsName, bStoredProcedure:=ACGetRelationshipDetailsStored, lNumberRecords:=1, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'return the data
            If Not Informations.IsArray(vResultArray) Then
                vRelatedPolicyCnt = ""
                vRelationshipType = ""
            Else

                vRelatedPolicyCnt = CStr(vResultArray(0, 0)).Trim()

                vRelationshipType = CStr(vResultArray(1, 0)).Trim()
            End If

            If vRelatedPolicyCnt <> "" Then
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyDetailsSQL & vRelatedPolicyCnt, sSQLName:=ACGetPolicyDetailsName, bStoredProcedure:=ACGetPolicyDetailsStored, lNumberRecords:=1, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'return the data
                If Not Informations.IsArray(vResultArray) Then
                    vRelatedPolicyCode = ""
                Else

                    vRelatedPolicyCode = CStr(vResultArray(0, 0)).Trim()
                End If
            End If


            If Not (Convert.IsDBNull(vPolicyTypeId) Or Informations.IsNothing(vPolicyTypeId)) Then

                'Get from DB

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyTypeDetailsSQL & CStr(vPolicyTypeId), sSQLName:=ACGetPolicyTypeDetailsName, bStoredProcedure:=ACGetPolicyTypeDetailsStored, lNumberRecords:=1, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'return the data
                If Not Informations.IsArray(vResultArray) Then
                    vPolicyTypeDesc = ""
                Else

                    vPolicyTypeDesc = CStr(vResultArray(0, 0)).Trim()
                End If
            End If

            If TransInsuranceFolderCnt = 0 Then
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetProductDetailsSQL & CStr(m_lInsuranceFileCnt), sSQLName:=ACGetProductDetailsName, bStoredProcedure:=ACGetProductDetailsStored, lNumberRecords:=1, vResultArray:=vResultArray)

            Else
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetProductDetailsSQL & CStr(m_lTransInsuranceFileCnt), sSQLName:=ACGetProductDetailsName, bStoredProcedure:=ACGetProductDetailsStored, lNumberRecords:=1, vResultArray:=vResultArray)
            End If
            'sj 20/09/2002 - end
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'return the data
            If Not Informations.IsArray(vResultArray) Then
                vSchemeDesc = "Standard Non scheme Policy"
                vSchemeId = 0
            Else

                vSchemeId = CInt(vResultArray(1, 0))

                vSchemeDesc = CStr(vResultArray(0, 0)).Trim()
            End If
            '    End If

            'eck070301

            If Not Informations.IsNothing(vRiskCnt) Then

                If Not (Convert.IsDBNull(vRiskCnt) Or Informations.IsNothing(vRiskCnt)) Then

                    'Get from DB

                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyRiskCntSQL & CStr(vInsuranceFileCnt), sSQLName:=ACGetPolicyRiskCntName, bStoredProcedure:=ACGetPolicyRiskCntStored, lNumberRecords:=1, vResultArray:=vResultArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'return the data
                    If Not Informations.IsArray(vResultArray) Then
                        vRiskCnt = CStr(0)
                    Else

                        vRiskCnt = CStr(vResultArray(0, 0)).Trim()
                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetOtherDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOtherDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'eck200400
    'Need the source id in order to get the commission account
    ' ***************************************************************** '
    ' Name: GetCommissionAccount
    '
    ' Description: Get Commission Account for policy.
    '
    ' ***************************************************************** '
    Public Function GetCommissionAccount(ByRef vSourceID As Object, ByRef vCommissionCnt As Integer) As Integer
        Return GetCommissionAccount(vSourceID:=vSourceID, vCommissionCnt:=vCommissionCnt, vRiskCodeID:=-1, vAccountExecID:=-1)
    End Function

    Public Function GetCommissionAccount(ByRef vSourceID As Object, ByRef vCommissionCnt As Integer, ByVal vRiskCodeID As Integer) As Integer
        Return GetCommissionAccount(vSourceID:=vSourceID, vCommissionCnt:=vCommissionCnt, vRiskCodeID:=vRiskCodeID, vAccountExecID:=-1)
    End Function

    Public Function GetCommissionAccount(ByRef vSourceID As Object, ByRef vCommissionCnt As Integer, ByVal vRiskCodeID As Integer, ByVal vAccountExecID As Integer) As Integer
        'Public Function GetCommissionAccount(vRiskCodeID As Variant, _
        'vSourceID As Variant, _
        'vCommissionCnt As Variant) As Long

        Dim result As Integer = 0
        Dim vCommissionArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="source_id", vValue:=CStr(vSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                ' SET 30/04/2003 PS325
                If vRiskCodeID > 0 Then
                    ' default to searching by riskcode
                    m_lReturn = .Parameters.Add(sName:="risk_code_id", vValue:=CStr(vRiskCodeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    ' Developer Guide No. 85

                    m_lReturn = .Parameters.Add(sName:="account_executive_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                ElseIf vAccountExecID > 0 Then
                    ' search by account exec
                    m_lReturn = .Parameters.Add(sName:="account_executive_cnt", vValue:=CStr(vAccountExecID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    ' Developer Guide No. 85

                    m_lReturn = .Parameters.Add(sName:="risk_code_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else
                    ' error cos no parameters supplied
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Get all the Accounts for the party
                m_lReturn = .SQLSelect(sSQL:=ACGetCommissionAccountSQL, sSQLName:=ACGetCommissionAccountName, bStoredProcedure:=ACGetCommissionAccountStored, lNumberRecords:=0, vResultArray:=vCommissionArray)
                If Not Informations.IsArray(vCommissionArray) Then

                    .Parameters.Clear()
                    m_lReturn = .Parameters.Add(sName:="source_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If vRiskCodeID > 0 Then
                        ' search by riskcode
                        m_lReturn = .Parameters.Add(sName:="risk_code_id", vValue:=CStr(vRiskCodeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        'Developer Guide No. 85

                        m_lReturn = .Parameters.Add(sName:="account_executive_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    ElseIf vAccountExecID > 0 Then
                        ' search by account exec
                        m_lReturn = .Parameters.Add(sName:="account_executive_cnt", vValue:=CStr(vAccountExecID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        ' Develper Guide No. 85

                        m_lReturn = .Parameters.Add(sName:="risk_code_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    Else
                        ' error cos no parameters supplied
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Get all the Accounts for the party
                    m_lReturn = .SQLSelect(sSQL:=ACGetCommissionAccountSQL, sSQLName:=ACGetCommissionAccountName, bStoredProcedure:=ACGetCommissionAccountStored, lNumberRecords:=0, vResultArray:=vCommissionArray)

                End If
            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'return the data
            If Not Informations.IsArray(vCommissionArray) Then
                vCommissionCnt = 0
            Else

                vCommissionCnt = CInt(vCommissionArray(0, 0))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetCommissionAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCommissionAccount", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetRiskGroupForCode
    '
    ' Description:
    '
    ' History: 12/06/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetRiskGroupForCode(ByVal v_lRiskCodeID As Integer, ByRef r_lRiskGroupId As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the SQL
            sSQL = "SELECT risk_group_id FROM risk_code WHERE risk_code_id = {risk_code_id}"

            ' Clear and add the parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_code_id", vValue:=CStr(v_lRiskCodeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetRiskGroup", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on SQL : " & sSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskGroupForCode", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' If theres any results, then get them else return notfound
            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Retrieve the first value

            r_lRiskGroupId = CInt(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskGroupForCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskGroupForCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetFeeDetails
    '
    ' Description: Get Fee details for policy.
    '
    ' ***************************************************************** '
    Public Function GetFeeDetails(ByRef vInsuranceFileCnt As Object, ByRef vFee(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()


                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Get all the Fees for the party
                If FromEvent Then
                    m_lReturn = .SQLSelect(sSQL:=ACGetAllEventFeeSQL, sSQLName:=ACGetAllEventFeeName, bStoredProcedure:=ACGetAllEventFeeStored, lNumberRecords:=0, vResultArray:=vFee)
                Else
                    m_lReturn = .SQLSelect(sSQL:=ACGetAllFeeUWSQL, sSQLName:=ACGetAllFeeUWName, bStoredProcedure:=ACGetAllFeeUWStored, lNumberRecords:=0, vResultArray:=vFee)
                End If

            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetFeeDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFeeDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: DeleteFees
    '
    ' Description: Deletes the fees.
    '
    ' ***************************************************************** '
    Public Function DeleteFees(ByRef vInsuranceFileCnt As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()


                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                'Delete the Fees for the party

                If FromEvent Then
                    'Datasure 8/9/2005 need to delete event taxes
                    m_lReturn = .SQLAction(sSQL:=ACDeleteEventFeeTaxesSQL, sSQLName:=ACDeleteEventFeeTaxesName, bStoredProcedure:=ACDeleteEventFeeTaxesStored)

                    .Parameters.Clear()


                    m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    m_lReturn = .SQLAction(sSQL:=ACDeleteEventFeeSQL, sSQLName:=ACDeleteFeeName, bStoredProcedure:=ACDeleteFeeStored)
                Else
                    'Datasure Delete The Tax Calculations
                    m_lReturn = .SQLAction(sSQL:=ACDeleteFeeTaxesSQL, sSQLName:=ACDeleteFeeTaxesName, bStoredProcedure:=ACDeleteFeeTaxesStored)

                    .Parameters.Clear()


                    m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    m_lReturn = .SQLAction(sSQL:=ACDeleteFeeSQL, sSQLName:=ACDeleteFeeName, bStoredProcedure:=ACDeleteFeeStored)
                End If
            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="DeleteFees Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteFees", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddFees
    '
    ' Description: Adds the fees.
    '
    ' ***************************************************************** '
    Public Function AddFees(ByRef vInsuranceFileCnt As Object, ByRef vFees(,) As Object) As Integer
        Dim result As Integer = 0
        Dim vTaxPercentage As Double

        Dim vFsaTypeOfSale(,) As Object = Nothing
        Dim vAdditional(,) As Object = Nothing
        Dim sPartyTypeCode As String = ""
        Dim lTaxGroupId As Integer
        Dim vResults(,) As Object = Nothing
        Dim lPolicyFeeId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsArray(vFees) Then
                Return result
            End If

            For i As Integer = vFees.GetLowerBound(1) To vFees.GetUpperBound(1)
                If vFees.GetUpperBound(0) > 10 Then

                    If CDbl(vFees(11, i)) = 0 Then

                        vFees(11, i) = gPMFunctions.ToSafeCurrency(vFees(3, i), 0) + gPMFunctions.ToSafeCurrency(vFees(11, i), 0)
                    End If



                    If (CDbl(vFees(6, i)) = 0) And (CDbl(vFees(5, i)) <> 0) Then



                        vFees(6, i) = Math.Round((CDbl(vFees(5, i)) / 100) * CDbl(vFees(3, i)), 2)
                    End If
                End If

                With m_oDatabase
                    .Parameters.Clear()


                    m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = .Parameters.Add(sName:="fee_party_cnt", vValue:=CStr(vFees(4, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = .SQLSelect(sSQL:=ACGetFeeSaTypeOfSaleForPartyAndInsuranceFileSQL, sSQLName:=ACGetFeeSaTypeOfSaleForPartyAndInsuranceFileName, bStoredProcedure:=ACGetFeeSaTypeOfSaleForPartyAndInsuranceFileStored, lNumberRecords:=0, vResultArray:=vFsaTypeOfSale)

                    .Parameters.Clear()


                    m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = .Parameters.Add(sName:="fee_party_cnt", vValue:=CStr(vFees(4, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = .SQLSelect(sSQL:=ACGetAdditionalFeeDetailsForPartyAndInsuranceFileSQL, sSQLName:=ACGetAdditionalFeeDetailsForPartyAndInsuranceFileName, bStoredProcedure:=ACGetAdditionalFeeDetailsForPartyAndInsuranceFileStored, lNumberRecords:=0, vResultArray:=vAdditional)
                    If Informations.IsArray(vAdditional) Then


                        sPartyTypeCode = gPMFunctions.ToSafeString(CStr(vAdditional(1, vAdditional.GetUpperBound(1))), "")


                        lTaxGroupId = gPMFunctions.ToSafeLong(CStr(vAdditional(0, vAdditional.GetUpperBound(1))), 0)
                    End If


                    .Parameters.Clear()


                    m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(vFees(4, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = .Parameters.Add(sName:="fee_percentage", vValue:=CStr(vFees(2, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = .Parameters.Add(sName:="fee_amount", vValue:=CStr(vFees(3, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = .Parameters.Add(sName:="commission_percentage", vValue:=CStr(vFees(5, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = .Parameters.Add(sName:="commission_amount", vValue:=CStr(vFees(6, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = .Parameters.Add(sName:="isIPTable", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'TR - AddFees is used all over the place, not just by Accident Management.
                    'Normal Risk fees Do not have Extra_Scheme_IDs (only Schemes Addons/extras do)
                    If vFees.GetUpperBound(0) > 7 Then
                        ' AMB 14-Oct-03: 1.8.6 Accident Management development - Extra_scheme_id


                        m_lReturn = .Parameters.Add(sName:="extra_scheme_id", vValue:=CStr(If(CStr(vFees(8, i)) = "0", DBNull.Value, vFees(8, i))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    Else
                        ' TR 27/01/2004 - No extra_scheme_ID passed in

                        'Developer Guide No. 85

                        m_lReturn = .Parameters.Add(sName:="extra_scheme_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    End If
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    'Datasure New Parameters
                    m_lReturn = .Parameters.Add(sName:="base_currency_id", vValue:=CStr(m_iCurrencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    If vFees.GetUpperBound(0) > 9 Then

                        m_lReturn = .Parameters.Add(sName:="tax_amount", vValue:=CStr(vFees(10, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
                    Else
                        m_lReturn = .Parameters.Add(sName:="tax_amount", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
                    End If
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If vFees.GetUpperBound(0) > 10 Then

                        m_lReturn = .Parameters.Add(sName:="total_fee", vValue:=CStr(vFees(11, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
                    Else
                        m_lReturn = .Parameters.Add(sName:="total_fee", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
                    End If
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If vFees.GetUpperBound(0) > 11 Then

                        m_lReturn = .Parameters.Add(sName:="commission_tax_amount", vValue:=CStr(vFees(12, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
                    Else
                        m_lReturn = .Parameters.Add(sName:="commission_tax_amount", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
                    End If
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If vFees.GetUpperBound(0) > 12 Then

                        m_lReturn = .Parameters.Add(sName:="total_commission", vValue:=CStr(vFees(13, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
                    Else
                        m_lReturn = .Parameters.Add(sName:="total_commission", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
                    End If
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    If CStr(vFees(0, i)) = ACFeeTypeExtras And vFees.GetUpperBound(0) > 9 Then

                        m_lReturn = .Parameters.Add(sName:="FSA_Type_Of_Sale_Id", vValue:=CStr(vFees(10, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                    Else
                        'Check Fee_Amounts table to check for linked fsa_type_of_sale_id
                        'else write null.
                        If Informations.IsArray(vFsaTypeOfSale) And (sPartyTypeCode.Trim() = "EX") Then

                            m_lReturn = .Parameters.Add(sName:="FSA_Type_Of_Sale_Id", vValue:=CStr(vFsaTypeOfSale(0, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                        Else
                            m_lReturn = .Parameters.Add(sName:="FSA_Type_Of_Sale_Id", vValue:=CStr(-1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                        End If
                    End If


                    'Add the Fees
                    'EK 05/09/99 Add Editable event
                    If FromEvent Then
                        m_lReturn = .SQLSelect(sSQL:=ACInsertEventFeeSQL, sSQLName:=ACInsertFeeName, bStoredProcedure:=ACInsertFeeStored, vResultArray:=vResults)
                    Else
                        m_lReturn = .SQLSelect(sSQL:=ACInsertFeeSQL, sSQLName:=ACInsertFeeName, bStoredProcedure:=ACInsertFeeStored, vResultArray:=vResults)
                    End If


                    lPolicyFeeId = gPMFunctions.ToSafeLong(CStr(vResults(0, 0)))

                    If (lTaxGroupId > 0) And (vFees.GetUpperBound(0) > 9) Then
                        vTaxPercentage = 0


                        If CDbl(vFees(10, i)) > 0 And CDbl(vFees(3, i)) > 0 Then


                            vTaxPercentage = CDbl(vFees(10, i)) / CDbl(vFees(3, i))
                        End If

                        .Parameters.Clear()
                        .Parameters.Add("from_event", CStr(If(FromEvent, 1, 0)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                        .Parameters.Add("insurance_file_cnt", CStr(vInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        .Parameters.Add("policy_fee_id", CStr(lPolicyFeeId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        .Parameters.Add("tax_group_id", CStr(lTaxGroupId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        .Parameters.Add("tax_rate", CStr(vTaxPercentage), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                        .Parameters.Add("tax_amount", CStr(vFees(10, i)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                        m_lReturn = .SQLSelect(sSQL:=AC_SQL_AddPolicyAddOnTaxSQL, sSQLName:=AC_SQL_AddPolicyAddOnTaxName, bStoredProcedure:=AC_SQL_AddPolicyAddOnTaxStored, vResultArray:=vResults)
                    End If

                End With

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Next i

            '
            '    'Datasure Update Tax amounts
            '    m_lReturn = UpdateFeeTaxes(v_vInsFileCnt:=vInsuranceFileCnt)
            '    If (m_lReturn& <> PMTrue) Then
            '        AddFees = PMFalse
            '        Exit Function
            '    End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="AddFees Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddFees", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNarrativeDetails
    '
    ' Description: Get Narrative details for policy.
    '
    ' ***************************************************************** '
    Public Function GetNarrativeDetails(ByRef vInsuranceFileCnt As Object, ByRef vNarrative(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()


                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Get all the Narratives for the party
                If FromEvent Then
                    m_lReturn = .SQLSelect(sSQL:=ACGetAllEventNarrativeSQL, sSQLName:=ACGetAllEventNarrativeName, bStoredProcedure:=ACGetAllEventNarrativeStored, lNumberRecords:=0, vResultArray:=vNarrative)
                Else
                    m_lReturn = .SQLSelect(sSQL:=ACGetAllNarrativeSQL, sSQLName:=ACGetAllNarrativeName, bStoredProcedure:=ACGetAllNarrativeStored, lNumberRecords:=0, vResultArray:=vNarrative)
                End If

            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetNarrativeDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNarrativeDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteNarratives
    '
    ' Description: Deletes the Narratives.
    '
    ' ***************************************************************** '
    Public Function DeleteNarratives(ByRef vInsuranceFileCnt As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()


                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Delete the Fees for the party
                'EK 05/09/99 Deleve editable event
                If FromEvent Then
                    m_lReturn = .SQLAction(sSQL:=ACDeleteEventNarrativeSQL, sSQLName:=ACDeleteNarrativeName, bStoredProcedure:=ACDeleteNarrativeStored)
                Else
                    m_lReturn = .SQLAction(sSQL:=ACDeleteNarrativeSQL, sSQLName:=ACDeleteNarrativeName, bStoredProcedure:=ACDeleteNarrativeStored)
                End If

            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="DeleteNarratives Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteNarratives", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddNarratives
    '
    ' Description: Adds the narratives.
    '
    ' ***************************************************************** '
    Public Function AddNarratives(ByRef vInsuranceFileCnt As Object, ByRef vNarratives As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsArray(vNarratives) Then
                Return result
            End If


            For i As Integer = vNarratives.GetLowerBound(1) To vNarratives.GetUpperBound(1)
                With m_oDatabase

                    .Parameters.Clear()


                    m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    m_lReturn = .Parameters.Add(sName:="policy_narrative_id", vValue:=CStr(i + 1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                    m_lReturn = .Parameters.Add(sName:="narrative_code_id", vValue:=CStr(vNarratives(2, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                    'Add the Fees
                    'EK 05/09/99 Add Editable Event
                    If FromEvent Then
                        m_lReturn = .SQLAction(sSQL:=ACInsertEventNarrativeSQL, sSQLName:=ACInsertNarrativeName, bStoredProcedure:=ACInsertNarrativeStored)
                    Else
                        m_lReturn = .SQLAction(sSQL:=ACInsertNarrativeSQL, sSQLName:=ACInsertNarrativeName, bStoredProcedure:=ACInsertNarrativeStored)
                    End If
                End With

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Next i

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="AddNarratives Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNarratives", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetStandardWordings
    '
    ' Description: Gets the Standard Wordings.
    '
    ' ***************************************************************** '
    'Arul Bug fixing - PN 55153
    'Note: Tow Optional parameters (v_lProductId ,v_lSoruceId) are added
    'TO MAKE THE CORRECTION OF TYPE OF VARIABLES
    'Public Function GetStandardWordings(ByRef vInsuranceFileCnt As Byte, ByRef vStandardWordings As Object, Optional ByVal v_lProductId As Integer = 0, Optional ByVal v_lSoruceId As Integer = 0) As Integer
    Public Function GetStandardWordings(ByRef vInsuranceFileCnt As Object, ByRef vStandardWordings(,) As Object) As Integer
        Return GetStandardWordings(vInsuranceFileCnt:=vInsuranceFileCnt, vStandardWordings:=vStandardWordings, v_lProductId:=0, v_lSoruceId:=0)
    End Function

    Public Function GetStandardWordings(ByRef vInsuranceFileCnt As Object, ByRef vStandardWordings(,) As Object, ByVal v_lProductId As Integer) As Integer
        Return GetStandardWordings(vInsuranceFileCnt:=vInsuranceFileCnt, vStandardWordings:=vStandardWordings, v_lProductId:=v_lProductId, v_lSoruceId:=0)
    End Function

    Public Function GetStandardWordings(ByRef vInsuranceFileCnt As Object, ByRef vStandardWordings(,) As Object, ByVal v_lProductId As Integer, ByVal v_lSoruceId As Integer) As Integer

        'End Arul Bug fixing - PN 55153

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If Not FromEvent Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=CStr(v_lProductId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Source_id", vValue:=CStr(v_lSoruceId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If vInsuranceFileCnt = 0 Then
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClausesSetToDefaultInProductSQL, sSQLName:=ACGetClausesSetToDefaultInProductName, bStoredProcedure:=ACGetClausesSetToDefaultInProductStored, vResultArray:=vStandardWordings)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetStandardWordings", "The stored procedure " & ACGetClausesSetToDefaultInProductSQL & " failed to fetch the record", gPMConstants.PMELogLevel.PMLogError)
                End If
            Else

                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If FromEvent Then
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllEventStandardWordingSQL, sSQLName:=ACGetAllStandardWordingName, bStoredProcedure:=ACGetAllStandardWordingstored, vResultArray:=vStandardWordings)
                Else
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllStandardWordingSQL, sSQLName:=ACGetAllStandardWordingName, bStoredProcedure:=ACGetAllStandardWordingstored, vResultArray:=vStandardWordings)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetStandardWordings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStandardWordings", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteStandardWordings
    '
    ' Description: Deletes the StandardWordings.
    '
    ' ***************************************************************** '
    Public Function DeleteStandardWordings(ByRef vInsuranceFileCnt As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()


                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If FromEvent Then
                    m_lReturn = .SQLAction(sSQL:=ACDeleteEventStandardWordingSQL, sSQLName:=ACDeleteStandardWordingName, bStoredProcedure:=ACDeleteStandardWordingStored)
                Else
                    m_lReturn = .SQLAction(sSQL:=ACDeleteStandardWordingSQL, sSQLName:=ACDeleteStandardWordingName, bStoredProcedure:=ACDeleteStandardWordingStored)
                End If

            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="DeleteStandardWordings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteStandardWordings", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddStandardWordings
    '
    ' Description: Adds the Standard Wordings.
    '
    ' ***************************************************************** '
    Public Function AddStandardWordings(ByRef vInsuranceFileCnt As Object, ByRef vStandardWordings As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsArray(vStandardWordings) Then
                Return result
            End If


            For i As Integer = vStandardWordings.GetLowerBound(1) To vStandardWordings.GetUpperBound(1)
                With m_oDatabase

                    .Parameters.Clear()


                    m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    m_lReturn = .Parameters.Add(sName:="policy_standard_wording_id", vValue:=CStr(CInt(i) + 1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    m_lReturn = .Parameters.Add(sName:="document_template_id", vValue:=CStr(vStandardWordings.GetValue(2, CInt(i))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    m_lReturn = .Parameters.Add(sName:="do_not_merge", vValue:=CStr(vStandardWordings(3, CInt(i))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                    'Add the Fees
                    'EK 05/09/99 Add Editable Event
                    If FromEvent Then
                        m_lReturn = .SQLAction(sSQL:=ACInsertEventStandardWordingSQL, sSQLName:=ACInsertStandardWordingName, bStoredProcedure:=ACInsertStandardWordingStored)
                    Else
                        m_lReturn = .SQLAction(sSQL:=ACInsertStandardWordingSQL, sSQLName:=ACInsertStandardWordingName, bStoredProcedure:=ACInsertStandardWordingStored)
                    End If
                End With

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Next i

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="AddStandardWordings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddStandardWordings", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'EK 20/10/99
    ' ***************************************************************** '
    ' Name: DeleteCoInsurers
    '
    ' Description: Deletes the CoInsurers.
    '
    ' ***************************************************************** '
    Public Function DeleteCoInsurers(ByRef vInsuranceFileCnt As Object) As Integer

        'TN20000807 Event will be created later
        'Dim oSIRInsuranceFile As bSIRInsuranceFile.SIRInsuranceFile
        'Dim lEventCnt As Long

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ''EK 12/12/99 Hard coded event
            '    m_lReturn = CreateEvent(r_lEventCnt:=lEventCnt, _
            ''                            v_lPartyCnt:=PartyCnt, _
            ''                            v_vInsuranceFolderCnt:=InsuranceFolderCnt, _
            ''                            v_vInsuranceFileCnt:=InsuranceFileCnt, _
            ''                            v_vClaimCnt:=Null, _
            ''                            v_vDocumentCnt:=Null, _
            ''                            v_vOldAddressCnt:=Null, _
            ''                            v_vNewAddressCnt:=Null, _
            ''                            v_vCampaignId:=Null, _
            ''                            v_vDocumentTypeId:=Null, _
            ''                            v_vReportTypeId:=Null, _
            ''                            v_lEventTypeId:=PMBEventPolChange, _
            ''                            v_dtEventDate:=Date, _
            ''                            v_vDescription:="Deleted coinsurers")
            '
            '    If (m_lReturn <> PMTrue) Then
            '        DeleteCoInsurers = PMFalse
            '        Exit Function
            '    End If
            '
            '    EventRaised = True
            '
            '    'There's only one in this collection...
            '    Set oSIRInsuranceFile = m_oSIRInsuranceFiles.Item(1)
            '
            '    'Now keep a copy of the edited item
            '    m_lReturn = oSIRInsuranceFile.CopyToEvent(v_lEventCnt:=lEventCnt)
            '
            '    Set oSIRInsuranceFile = Nothing
            '
            '    If (m_lReturn <> PMTrue) Then
            '        DeleteCoInsurers = PMFalse
            '        Exit Function
            '    End If

            With m_oDatabase

                .Parameters.Clear()


                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Delete the CoInsurers for the policy
                If FromEvent Then
                    m_lReturn = .SQLAction(sSQL:=ACDeleteEventCoInsurerSQL, sSQLName:=ACDeleteCoInsurerName, bStoredProcedure:=ACDeleteCoInsurerStored)
                Else
                    m_lReturn = .SQLAction(sSQL:=ACDeleteCoInsurerSQL, sSQLName:=ACDeleteCoInsurerName, bStoredProcedure:=ACDeleteCoInsurerStored)
                End If
            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="DeleteCoInsurers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteCoInsurers", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetScheme(ByRef vSchemeId As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If TransInsuranceFolderCnt = 0 Then
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetProductDetailsSQL & CStr(m_lInsuranceFileCnt), sSQLName:=ACGetProductDetailsName, bStoredProcedure:=ACGetProductDetailsStored, lNumberRecords:=1, vResultArray:=vResultArray)
            Else
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetProductDetailsSQL & CStr(m_lTransInsuranceFileCnt), sSQLName:=ACGetProductDetailsName, bStoredProcedure:=ACGetProductDetailsStored, lNumberRecords:=1, vResultArray:=vResultArray)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Return the data
            If Not Informations.IsArray(vResultArray) Then
                vSchemeId = 0
            Else

                vSchemeId = CInt(vResultArray(1, 0))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetScheme Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetScheme", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function 'eck300101
    ' ***************************************************************** '
    ' Name: CheckForSections
    '
    ' Description: Checks the CoInsurers.
    '
    ' ***************************************************************** '
    Public Function CheckForSections(ByRef vInsuranceFileCnt As Object) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()


                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If Not FromEvent Then
                    sSQL = "Select max(insurance_section_id)from insurance_COB_section where insurance_file_cnt = {insurance_file_cnt}"
                Else
                    sSQL = "Select max(insurance_section_id)from event_insurance_COB_section where insurance_file_cnt = {insurance_file_cnt}"
                End If
                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="CheckForSections", bStoredProcedure:=False, vResultArray:=vResultArray)
            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception()
                Return result
            End If
            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vResultArray(0, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CheckForSections Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckForSections", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckCoInsurers
    '
    ' Description: Checks the CoInsurers.
    '
    ' ***************************************************************** '
    Public Function CheckCoInsurers(ByRef vInsuranceFileCnt As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim bMissingCoInsurers As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()


                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="missing_coinsurers", vValue:=CStr(bMissingCoInsurers), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

                'Check the CoInsurers for the policy
                'Datsure - stored procedure required to to check coinsured for all sections
                If FromEvent Then
                    m_lReturn = .SQLAction(sSQL:=ACCheckEventCoInsurersSQL, sSQLName:=ACCheckEventCoInsurersName, bStoredProcedure:=CBool(ACCheckEventCoInsurersStored))
                Else
                    m_lReturn = .SQLAction(sSQL:=ACCheckCoInsurersSQL, sSQLName:=ACCheckCoInsurersName, bStoredProcedure:=CBool(ACCheckCoInsurersStored))
                End If

                bMissingCoInsurers = gPMFunctions.ToSafeBoolean(.Parameters.Item("missing_coinsurers").Value, False)

            End With

            If bMissingCoInsurers Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CheckCoInsurers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckCoInsurers", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name : DelUnderWriterCoInsurer
    '
    ' Desc : delete co-insurer for policy
    '
    ' ***************************************************************** '
    Public Function DelUnderWriterCoInsurer(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If BeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'delete Coi_Value
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Developer Guide No. 85

            m_lReturn = m_oDatabase.Parameters.Add(sName:="coi_value_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelCoiValueSQL, sSQLName:=ACDelCoiValueName, bStoredProcedure:=ACDelCoiValueStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'delete Coi_Compulsory_Value
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Developer Guide No. 85

            m_lReturn = m_oDatabase.Parameters.Add(sName:="coi_compulsory_value_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelCoiCompulsorySQL, sSQLName:=ACDelCoiCompulsoryName, bStoredProcedure:=ACDelCoiCompulsoryStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'delete Coi_Arrangement
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelCoiArrangementSQL, sSQLName:=ACDelCoiArrangementName, bStoredProcedure:=ACDelCoiArrangementStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return CommitTrans()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="DelUnderWriterCoInsurer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DelUnderWriterCoInsurer", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    'EK03/09/99 New Method to Create Events for Transaction Processing
    ' ***************************************************************** '
    ' Name: CreateTransactionEvent (Public)
    '
    ' Description: Create an event record.
    '
    ' ***************************************************************** '
    Public Function CreateTransactionEvent(ByRef m_lEventCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim oSIRInsuranceFile As bSIRInsuranceFile.SIRInsuranceFile
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Loop round Collection

            For lSub As Integer = 1 To m_oSIRInsuranceFiles.Count()
                oSIRInsuranceFile = m_oSIRInsuranceFiles.Item(lSub)


                ' If we haven't already started a transaction start one.
                If Not bTransStarted Then
                    m_lReturn = BeginTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    bTransStarted = True
                End If
                'EK 26/10/ 99 create correct event type
                'we may already have raised the event, so
                If Not EventRaised Then
                    'Add the created event first

                    m_lReturn = CreateEvent(r_lEventCnt:=m_lEventCnt, v_lPartyCnt:=PartyCnt, v_vInsuranceFolderCnt:=oSIRInsuranceFile.InsuranceFolderCnt, v_vInsuranceFileCnt:=oSIRInsuranceFile.InsuranceFileCnt, v_vClaimCnt:=DBNull.Value, v_vDocumentCnt:=DBNull.Value, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentTypeId:=DBNull.Value, v_vReportTypeId:=DBNull.Value, v_lEventTypeId:=PMBConst.PMBEventTransaction, v_dtEventDate:=DateTime.Today, v_vDescription:=DBNull.Value)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Exit For
                    End If

                    EventRaised = True

                    '                    Now keep a copy of the edited item
                    'eck030801
                    '                   m_lReturn = oSIRInsuranceFile.CopyToEvent(v_lEventCnt:=m_lEventCnt)
                    m_lReturn = oSIRInsuranceFile.CopyToTransactionEvent(v_lEventCnt:=m_lEventCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Exit For
                    End If
                End If

                ' Update Item
                m_lReturn = oSIRInsuranceFile.UpdateItem()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Exit For
                End If



            Next lSub


            ' Release last reference
            oSIRInsuranceFile = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If Update() = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CommitTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                Else

                    m_lReturn = RollbackTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateTransactionEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTransactionEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    'EK03/09/99 New Method to Update Insurance file from Editable Events for Transaction Processing
    ' ***************************************************************** '
    ' Name: UpdateFromTransactionEvent (Public)
    '
    ' Description: Update from event record.
    '
    ' ***************************************************************** '
    Public Function UpdateFromTransactionEvent() As Integer

        Dim result As Integer = 0
        Dim oSIRInsuranceFile As bSIRInsuranceFile.SIRInsuranceFile
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Loop round Collection

            For lSub As Integer = 1 To m_oSIRInsuranceFiles.Count()
                oSIRInsuranceFile = m_oSIRInsuranceFiles.Item(lSub)


                ' If we haven't already started a transaction start one.
                If Not bTransStarted Then
                    m_lReturn = BeginTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    bTransStarted = True
                End If

                'pass the last transaction type
                'Now keep a copy of the edited item
                m_lReturn = oSIRInsuranceFile.CopyFromEvent(v_lEventCnt:=m_lTransInsuranceFileCnt, v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lInsuranceFolderCnt:=m_lTransInsuranceFolderCnt, v_lLastTransType:=m_lLastTransType, v_sTransDebitCredit:=m_sTransDebitCredit)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Exit For
                End If


            Next lSub


            ' Release last reference
            oSIRInsuranceFile = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If Update() = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CommitTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                Else

                    m_lReturn = RollbackTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateFromTransactionEvent Failed", vClass:=ACClass, vMethod:="UpdateFromTransactionEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    'EK03/09/99 New Method to remove Events Created for Transaction Processing
    ' ***************************************************************** '
    ' Name: DeleteTransactionEvent (Public)
    '
    ' Description: Delete an event record.
    '
    ' ***************************************************************** '
    Public Function DeleteTransactionEvent() As Integer

        Dim result As Integer = 0
        Dim oSIRInsuranceFile As bSIRInsuranceFile.SIRInsuranceFile
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Loop round Collection

            For lSub As Integer = 1 To m_oSIRInsuranceFiles.Count()
                oSIRInsuranceFile = m_oSIRInsuranceFiles.Item(lSub)


                ' If we haven't already started a transaction start one.
                If Not bTransStarted Then
                    m_lReturn = BeginTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    bTransStarted = True
                End If


                '                    Now keep a copy of the edited item
                m_lReturn = oSIRInsuranceFile.DeleteEvent(v_lEventCnt:=m_lTransInsuranceFileCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Exit For
                End If

            Next lSub


            ' Release last reference
            oSIRInsuranceFile = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If Update() = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CommitTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                Else

                    m_lReturn = RollbackTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteTransactionEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteTransactionEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required SIRInsuranceFiles and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(ByRef r_vFieldArray() As Object) As Integer


        Dim result As Integer = 0
        Dim oSIRInsuranceFile As bSIRInsuranceFile.SIRInsuranceFile
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oSIRInsuranceFiles.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oSIRInsuranceFile = m_oSIRInsuranceFiles.Item(m_lCurrentRecord)

            ' Get the SIRInsuranceFile Property Values

            m_lReturn = oSIRInsuranceFile.GetProperties(r_iStatus:=iStatus, r_vFieldArray:=r_vFieldArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRInsuranceFile = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied SIRInsuranceFile into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, ByRef r_vFieldArray() As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRInsuranceFile As bSIRInsuranceFile.SIRInsuranceFile

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oSIRInsuranceFiles.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new SIRInsuranceFile
            oSIRInsuranceFile = New bSIRInsuranceFile.SIRInsuranceFile()
            m_lReturn = oSIRInsuranceFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            ' Populate SIRInsuranceFile Attributes

            m_lReturn = oSIRInsuranceFile.SetProperties(v_iStatus:=gPMConstants.PMEComponentAction.PMAdd, v_vFieldArray:=r_vFieldArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSIRInsuranceFile = Nothing
                Return result
            End If

            ' Add SIRInsuranceFile to collection
            If m_oSIRInsuranceFiles.Count = 0 Then
                m_oSIRInsuranceFiles.Add(Nothing)
            End If
            m_lReturn = m_oSIRInsuranceFiles.Add(oNewSIRInsuranceFile:=oSIRInsuranceFile)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRInsuranceFile = Nothing
                Return result
            End If

            oSIRInsuranceFile = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the SIRInsuranceFile
    '              specified and updates the SIRInsuranceFile with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, ByRef r_vFieldArray() As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRInsuranceFile As bSIRInsuranceFile.SIRInsuranceFile
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIRInsuranceFiles.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oSIRInsuranceFile = m_oSIRInsuranceFiles.Item(lRow)

            ' Check the Status of the SIRInsuranceFile

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oSIRInsuranceFile.DatabaseStatus
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Leave Status as Add
                    iStatus = gPMConstants.PMEComponentAction.PMAdd
                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                    ' Error
                    result = gPMConstants.PMEReturnCode.PMFalse
                Case Else
                    ' Set Edit (Update) Status
                    iStatus = gPMConstants.PMEComponentAction.PMEdit
            End Select

            ' Update SIRInsuranceFile Attributes
            m_lReturn = oSIRInsuranceFile.SetProperties(v_iStatus:=iStatus, v_vFieldArray:=r_vFieldArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSIRInsuranceFile = Nothing
                Return result
            End If

            'Tomo200300
            'Quick, save the policy type...

            m_lPolicyTypeId = CInt(r_vFieldArray(InsuranceFileConst.ACPolicyTypeId))

            ' Release reference to SIRInsuranceFile
            oSIRInsuranceFile = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified SIRInsuranceFile can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oSIRInsuranceFile As bSIRInsuranceFile.SIRInsuranceFile

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIRInsuranceFiles.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oSIRInsuranceFile = m_oSIRInsuranceFiles.Item(lRow)

            ' Check the Status of the SIRInsuranceFile

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oSIRInsuranceFile.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oSIRInsuranceFile.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oSIRInsuranceFile.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to SIRInsuranceFile
            oSIRInsuranceFile = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Cancel (Public)
    '
    ' Description: Checks the Collection to see if Cancel is OK.
    '              i.e. Do we need any Adding, Deleting or Updating.
    '
    '              Returns PMTrue if all items are clean
    '                      (PMView or PMDummyDelete)
    '              Otherwise returns PMDataChanged.
    ' ***************************************************************** '
    Public Function Cancel() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop round Collection
            For lSub As Integer = 1 To m_oSIRInsuranceFiles.Count()
                Select Case m_oSIRInsuranceFiles.Item(lSub).DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                        result = gPMConstants.PMEReturnCode.PMDataChanged
                        Exit For
                End Select
            Next lSub

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Loops round the collection, doing any required
    '              Adds, Deletes or Updates.
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim lSub As Integer
        Dim oSIRInsuranceFile As bSIRInsuranceFile.SIRInsuranceFile = Nothing
        Dim bTransStarted As Boolean
        Dim bDeleteTax As Boolean
        Dim sSystemOption As String = ""
        Dim oRITax As bSIRRITax.Business = Nothing
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            getUnderwritingType()

            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=1007, r_sOptionValue:=sSystemOption)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(m_lReturn.ToString() + ", " + +", Failed to get system option for UW Tax.")
            End If

            'If the option is off then any previous tax needs to be deleted
            bDeleteTax = Not (sSystemOption = "1")

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection
            For lSub = 1 To m_oSIRInsuranceFiles.Count()
                oSIRInsuranceFile = m_oSIRInsuranceFiles.Item(lSub)


                Select Case oSIRInsuranceFile.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item
                        oSIRInsuranceFile.InsuranceFolderCnt = InsuranceFolderCnt
                        m_lReturn = oSIRInsuranceFile.AddItem()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                        'TN20000807 set flag to create event later
                        PMRaiseEvent = True

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If


                        'DN 16/08/01
                        oSIRInsuranceFile.TransInsuranceFolderCnt = TransInsuranceFolderCnt

                        ' Update Item
                        m_lReturn = oSIRInsuranceFile.UpdateItem()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                        '''''''''''''''''''''''''''''''''''''''''''''''''''
                        ' RAM20040203 : Added the SCHEMES Policy Type too.
                        '               Ref. PN Issue No 7814 - START
                        '''''''''''''''''''''''''''''''''''''''''''''''''''
                        'Only raise an event if it's one of our policies
                        'This may change later...
                        If (m_lPolicyTypeId = PMBConst.PMBPolicyTypeGeneral) Or (m_lPolicyTypeId = PMBConst.PMBPolicyTypeGIIMotor) Or (m_lPolicyTypeId = PMBConst.PMBPolicyTypeGIIHousehold) Or (m_lPolicyTypeId = PMBConst.PMBPolicyTypeGIIMotor) Or (m_lPolicyTypeId = PMBConst.PMBPolicyTypeSchemes) Then
                            PMRaiseEvent = True
                        End If
                        '''''''''''''''''''''''''''''''''''''''''''''''''''
                        ' RAM20040203 : PN Issue No 7814 - END
                        '''''''''''''''''''''''''''''''''''''''''''''''''''

                    Case gPMConstants.PMEComponentAction.PMDelete

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Delete Item
                        m_lReturn = oSIRInsuranceFile.DeleteItem()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

                If bDeleteTax Then
                    oRITax = New bSIRRITax.Business
                    If oRITax.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Update = gPMConstants.PMEReturnCode.PMFalse

                        Exit Function
                    End If

                    m_lReturn = oRITax.DeleteAllTaxes(v_lInsuranceFileCnt:=oSIRInsuranceFile.InsuranceFileCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(m_lReturn.ToString() + ", " + +", " + "Failed to delete all taxes for insurance_file_cnt=" & oSIRInsuranceFile.InsuranceFileCnt)
                    End If
                End If

                If Not (oRITax Is Nothing) Then
                    ' Terminate Tax Component
                    oRITax.Dispose()
                    ' Release Tax Reference
                    oRITax = Nothing

                End If

            Next lSub

            ' Retain the Primary Key of the SIRInsuranceFile
            With oSIRInsuranceFile
                InsuranceFileCnt = .InsuranceFileCnt
            End With

            ' Release last reference
            oSIRInsuranceFile = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CommitTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Refresh the Collection Items D/B Status
                    lSub = 1

                    ' For each item in the collection
                    Do While lSub <= m_oSIRInsuranceFiles.Count()

                        ' With the item
                        With m_oSIRInsuranceFiles.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oSIRInsuranceFiles.Delete(lSub)

                                    ' Anything Else
                                Case Else
                                    ' Set Status to view
                                    .DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                                    lSub += 1

                            End Select

                        End With

                    Loop

                    'Generate a default Sharepoint folder (if Sharepoint is enabled)
                    If m_sCallingAppName <> "iPMUDeferredRIAuto" Then
                        Dim Sharepoint As bSIRSharepoint.Business
                        Sharepoint = New bSIRSharepoint.Business
                        Sharepoint.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

                        Sharepoint.GenerateDefaultPath(PartyCnt, InsuranceFileCnt, 0, 0)
                    End If
                Else

                    m_lReturn = RollbackTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'EK 05/09/99 New Method to Save Editable Event for Transaction Processing
    ' ***************************************************************** '
    ' Name: UpdateEvent (Public)
    '
    ' Description: Loops round the collection, doing any required
    '              Adds, Deletes or Updates.
    '
    ' ***************************************************************** '
    Public Function UpdateEvent() As Integer

        Dim result As Integer = 0
        Dim lSub As Integer
        Dim oSIRInsuranceFile As bSIRInsuranceFile.SIRInsuranceFile = Nothing
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oSIRInsuranceFiles.Count()
                oSIRInsuranceFile = m_oSIRInsuranceFiles.Item(lSub)


                Select Case oSIRInsuranceFile.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing



                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        'DN 16/08/01
                        oSIRInsuranceFile.TransInsuranceFolderCnt = TransInsuranceFolderCnt

                        ' Update Item
                        m_lReturn = oSIRInsuranceFile.UpdateItem()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If


                End Select

            Next lSub

            ' Retain the Primary Key of the SIRInsuranceFile
            With oSIRInsuranceFile
                InsuranceFileCnt = .InsuranceFileCnt
            End With

            ' Release last reference
            oSIRInsuranceFile = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CommitTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Refresh the Collection Items D/B Status
                    lSub = 1

                    ' For each item in the collection
                    Do While lSub <= m_oSIRInsuranceFiles.Count()

                        ' With the item
                        With m_oSIRInsuranceFiles.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oSIRInsuranceFiles.Delete(lSub)

                                    ' Anything Else
                                Case Else
                                    ' Set Status to view
                                    .DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                                    lSub += 1

                            End Select

                        End With

                    Loop

                Else

                    m_lReturn = RollbackTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function CheckFeesAndNarratives(ByVal v_vOldFeeArray(,) As Object, ByVal v_vNewFeeArray(,) As Object, ByVal v_vOldNarrativeArray As Object, ByVal v_vNewNarrativeArray As Object, ByRef r_bDifferent As Boolean) As Integer

        Dim result As Integer = 0
        Dim bDifferent As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_bDifferent = False



            m_lReturn = CompareArrays(r_bDifferent:=bDifferent, v_vOldArray:=v_vOldFeeArray, v_vNewArray:=v_vNewFeeArray, v_bFees:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not bDifferent Then


                m_lReturn = CompareArrays(r_bDifferent:=bDifferent, v_vOldArray:=v_vOldNarrativeArray, v_vNewArray:=v_vNewNarrativeArray, v_bFees:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If Not bDifferent Then
                Return result
            End If

            r_bDifferent = True

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckFeesAndNarratives Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFeesAndNarratives", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CompareArrays (Public)
    '
    ' Description: compares the two arrays.
    '
    ' ***************************************************************** '
    'Developer Guide No 17. 
    Private Function CompareArrays(ByRef r_bDifferent As Boolean, ByVal v_vOldArray(,) As Object, ByVal v_vNewArray(,) As Object, ByVal v_bFees As Object) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        'First check that the arrays are different

        If Informations.IsArray(v_vOldArray) Then
            If Informations.IsArray(v_vNewArray) Then

                'More to do
            Else
                'Not the same, must raise event
                r_bDifferent = True
                Return result
            End If
        Else
            If Informations.IsArray(v_vNewArray) Then
                'Not the same, must raise event
                r_bDifferent = True
                Return result
            Else
                'Nothing was there, nothing is there, nothing to do
                r_bDifferent = False
                Return result
            End If
        End If

        'Now we have two arrays to compare...

        If (v_vOldArray.GetLowerBound(1) <> v_vNewArray.GetLowerBound(1)) Or (v_vOldArray.GetUpperBound(1) <> v_vNewArray.GetUpperBound(1)) Then
            r_bDifferent = True
            Return result
        End If

        'Perhaps need to be even more clever.  The array could just be rearranged...

        For iTemp As Integer = v_vOldArray.GetLowerBound(1) To v_vOldArray.GetUpperBound(1)
            If v_bFees Then
                '0 = type - string, 2 = percentage, 3 = amount, 4 = party cnt - all numbers








                If (CStr(v_vOldArray(0, iTemp)).Trim() <> CStr(v_vNewArray(0, iTemp)).Trim()) Or (CDbl(v_vOldArray(2, iTemp)) <> CDbl(v_vNewArray(2, iTemp))) Or (CDbl(v_vOldArray(3, iTemp)) <> CDbl(v_vNewArray(3, iTemp))) Or (CDbl(v_vOldArray(4, iTemp)) <> CDbl(v_vNewArray(4, iTemp))) Then
                    r_bDifferent = True
                    Return result
                End If
            Else
                '2 = narrative code id - number


                If CDbl(v_vOldArray(2, iTemp)) <> CDbl(v_vNewArray(2, iTemp)) Then
                    r_bDifferent = True
                    Return result
                End If
            End If
        Next iTemp

        r_bDifferent = False

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: MakeEvent
    '
    ' Description:
    '
    ' History: 30/11/1999 Tomo - Created.
    '
    ' ***************************************************************** '
    ' Gaurav Changed
    Public Function MakeEvent() As Integer

        Dim result As Integer = 0
        Dim oSIRInsuranceFile As bSIRInsuranceFile.SIRInsuranceFile
        Dim lEventCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            Dim iEventIDPolChange As Integer = PMBConst.PMBEventPolChange

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                iEventIDPolChange = PMBConst.PMBEventNewPolicy
            End If

            m_lReturn = CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=PartyCnt, v_vInsuranceFolderCnt:=InsuranceFolderCnt, v_vInsuranceFileCnt:=InsuranceFileCnt, v_vClaimCnt:=DBNull.Value, v_vDocumentCnt:=DBNull.Value, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentTypeId:=DBNull.Value, v_vReportTypeId:=DBNull.Value,
                                    v_lEventTypeId:=iEventIDPolChange, v_dtEventDate:=DateTime.Today, v_vDescription:=EventDescription, v_vIsmanualDescription:=m_vIsManualDescription)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TN20000808    EventRaised = True

            'There's only one in this collection...
            If m_oSIRInsuranceFiles.Count > 0 Then
                oSIRInsuranceFile = m_oSIRInsuranceFiles.Item(1)
                'Now keep a copy of the edited item
                m_lReturn = oSIRInsuranceFile.CopyToEvent(v_lEventCnt:=lEventCnt)
            Else
                oSIRInsuranceFile = Nothing
            End If

            oSIRInsuranceFile = Nothing

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MakeEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MakeEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckOKToDelete
    '
    ' Description:
    '
    ' History: 14/01/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function CheckOKToDelete() As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sString As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        m_bOKToDelete = True

        'There are 5 reasons in PMB why a client cannot be deleted
        '1. Block policy with sub policies
        'Not relevant - yet

        '2. Life accounting transactions
        'Not relevant

        '3. Live transactions

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetLiveTransactionDetailsSQL, sSQLName:=ACGetLiveTransactionDetailsName, bStoredProcedure:=ACGetLiveTransactionDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

        If Informations.IsArray(vResultArray) Then
            m_bOKToDelete = False
            sString = "Live Transactions exist for this policy"
            If m_sNoDeleteReasons = "" Then
                m_sNoDeleteReasons = sString
            Else
                m_sNoDeleteReasons = m_sNoDeleteReasons & Strings.ChrW(13) & Strings.ChrW(10) & sString
            End If
        End If

        vResultArray = Nothing

        '4. Claims exist for policy
        'DC050606 PN28736 check for claims attached to policy being deleted
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClaimsForPolicySQL, sSQLName:=ACGetClaimsForPolicyName, bStoredProcedure:=ACGetClaimsForPolicyStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

        If Informations.IsArray(vResultArray) Then
            m_bOKToDelete = False
            sString = "Claims exist for this policy"
            If m_sNoDeleteReasons = "" Then
                m_sNoDeleteReasons = sString
            Else
                m_sNoDeleteReasons = m_sNoDeleteReasons & Strings.ChrW(13) & Strings.ChrW(10) & sString
            End If
        End If

        '5. Policy is live and is not a quote (live policies should have there status changed before deletion)
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACIsLivePolicySQL, sSQLName:=ACIsLivePolicyName, bStoredProcedure:=ACIsLivePolicyStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

        If Informations.IsArray(vResultArray) Then
            m_bOKToDelete = False
            sString = "Policy status is live"
            If m_sNoDeleteReasons = "" Then
                m_sNoDeleteReasons = sString
            Else
                m_sNoDeleteReasons = m_sNoDeleteReasons & Strings.ChrW(13) & Strings.ChrW(10) & sString
            End If
        End If

        vResultArray = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: DeletePolicy
    '
    ' Description:
    '
    ' History: 14/01/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function DeletePolicy() As Integer

        Dim result As Integer = 0
        Dim lEventCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeletePolicySQL, sSQLName:=ACDeletePolicyName, bStoredProcedure:=ACDeletePolicyStored)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            m_lReturn = CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=PartyCnt, v_vInsuranceFolderCnt:=InsuranceFolderCnt, v_vInsuranceFileCnt:=InsuranceFileCnt, v_vClaimCnt:=DBNull.Value, v_vDocumentCnt:=DBNull.Value, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentTypeId:=DBNull.Value, v_vReportTypeId:=DBNull.Value, v_lEventTypeId:=PMBConst.PMBEventDelPolicy, v_dtEventDate:=DateTime.Now, v_vDescription:=DBNull.Value)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeletePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UndeletePolicy
    '
    ' Description:
    '
    ' History: 14/01/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function UndeletePolicy() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUndeletePolicySQL, sSQLName:=ACUndeletePolicyName, bStoredProcedure:=ACUndeletePolicyStored)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UndeletePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UndeletePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: UpdateFeeTaxes (Private)
    '
    ' Datasure
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (UpdateFeeTaxes) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function UpdateFeeTaxes(ByVal v_vInsFileCnt As Object) As Integer
    '
    '
    'Dim result As Integer = 0
    'Dim sSQL As String = ""
    'Dim vArray(,) As Object
    'Dim lPolicyFeeId As Integer
    'Dim vResultArray(,) As Object= nothing
    'Dim sDescription As String = ""
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Get the insurance file commission tax

    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Failed to process the interface.
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'Throw New Exception()
    'Return result
    'End If
    '
    '
    '
    'sSQL = ""
    '
    'If Not FromEvent Then

    'sSQL = "SELECT policy_fee_id from Policy_fee WHERE " &  _
    '       "insurance_file_cnt = " & CStr(CInt(v_vInsFileCnt))
    'Else

    'sSQL = "SELECT policy_fee_id from event_policy_fee WHERE " &  _
    '       "insurance_file_cnt = " & CStr(CInt(v_vInsFileCnt))
    'End If

    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '




    '

    'For 'i As Integer = 0 To vArray.GetUpperBound(1)

    'lPolicyFeeId = CInt(vArray(0, i))
    '




    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If


    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'If FromEvent Then
    'sSQL = "UPDATE event_policy_fee SET tax_amount = " &  _
    '       "ISNULL((SELECT Round(SUM(value),2) from event_tax_calculation where transtype = 'TTF' AND policy_fee_id = " & CStr(lPolicyFeeId) &  _
    '       "),0),total_fee = fee_amount + " &  _
    '       "ISNULL((SELECT Round(SUM(value),2) from event_tax_calculation where transtype = 'TTF' AND policy_fee_id = " & CStr(lPolicyFeeId) &  _
    '       "),0) WHERE policy_fee_id = " & CStr(lPolicyFeeId)
    'Else
    'sSQL = "UPDATE policy_fee SET tax_amount = " &  _
    '       "ISNULL((SELECT Round(SUM(value),2) from tax_calculation where transtype = 'TTF' AND policy_fee_id = " & CStr(lPolicyFeeId) &  _
    '       "),0), total_fee = fee_amount + " &  _
    '       "ISNULL((SELECT Round(SUM(value),2) from tax_calculation where transtype = 'TTF' AND policy_fee_id = " & CStr(lPolicyFeeId) &  _
    '       "),0) WHERE policy_fee_id = " & CStr(lPolicyFeeId)
    '
    'End If

    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    'If FromEvent Then
    'sSQL = "UPDATE event_policy_fee SET commission_tax_amount = " &  _
    '       "ISNULL((SELECT Round(SUM(value),2) from event_tax_calculation where transtype = 'TTFC' AND policy_fee_id = " & CStr(lPolicyFeeId) &  _
    '       "),0),total_commission = commission_amount + " &  _
    '       "ISNULL((SELECT Round(SUM(value),2) from event_tax_calculation where transtype = 'TTFC' AND policy_fee_id = " & CStr(lPolicyFeeId) &  _
    '       "),0) WHERE policy_fee_id = " & CStr(lPolicyFeeId)
    'Else
    'sSQL = "UPDATE policy_fee SET commission_tax_amount = " &  _
    '       "ISNULL((SELECT Round(SUM(value),2) from tax_calculation where transtype = 'TTFC' AND policy_fee_id = " & CStr(lPolicyFeeId) &  _
    '       "),0),total_commission = commission_amount + " &  _
    '       "ISNULL((SELECT Round(SUM(value),2) from tax_calculation where transtype = 'TTFC' AND policy_fee_id = " & CStr(lPolicyFeeId) &  _
    '       "),0) WHERE policy_fee_id = " & CStr(lPolicyFeeId)
    '
    'End If

    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    '
    '
    'Next i
    '
    'If Not (m_oTaxCalculationBusiness Is Nothing) Then
    ' Terminate the Tax Calculation business object


    '
    '

    '
    'End If
    '
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateFeeTaxes  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateEvent ", vErrNo:=informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Private Function BeginTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Private)
    '
    ' Description: Check Mandatory parameters have been passed.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CheckMandatory) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CheckMandatory(ByRef r_vFieldArray() As Object) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' {* USER DEFINED CODE (Begin) *}
    '

    'If Object.Equals(r_vFieldArray(InsuranceFileConst.ACInsuranceFileStructureID), Nothing) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    'If Object.Equals(r_vFieldArray(InsuranceFileConst.ACInsuranceFileTypeID), Nothing) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    'If Object.Equals(r_vFieldArray(InsuranceFileConst.ACInsuranceFileID), Nothing) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    'If Object.Equals(r_vFieldArray(InsuranceFileConst.ACSourceID), Nothing) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    'If Object.Equals(r_vFieldArray(InsuranceFileConst.ACInsuranceFolderCnt), Nothing) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    'If Object.Equals(r_vFieldArray(InsuranceFileConst.ACInsuranceRef), Nothing) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    'If Object.Equals(r_vFieldArray(InsuranceFileConst.ACProductID), Nothing) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    'If Object.Equals(r_vFieldArray(InsuranceFileConst.ACLeadInsurerCnt), Nothing) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    'If Object.Equals(r_vFieldArray(InsuranceFileConst.ACLeadAgentCnt), Nothing) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    'If Object.Equals(r_vFieldArray(InsuranceFileConst.ACAccountHandlerCnt), Nothing) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    'If Object.Equals(r_vFieldArray(InsuranceFileConst.ACInsuredCnt), Nothing) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    'If Object.Equals(r_vFieldArray(InsuranceFileConst.ACBusinessTypeID), Nothing) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    'If Object.Equals(r_vFieldArray(InsuranceFileConst.ACCollectionFromCnt), Nothing) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '
    'sj 19/07/2002 - start
    '    If (IsEmpty(r_vFieldArray(ACBranchID)) = True) Then
    '        CheckMandatory = PMMandatoryMissing
    '        Exit Function
    '    End If

    'If Object.Equals(r_vFieldArray(InsuranceFileConst.ACSubBranchID), Nothing) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    'sj 19/07/2002 - end
    '

    'If Object.Equals(r_vFieldArray(InsuranceFileConst.ACCurrencyID), Nothing) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    'If Object.Equals(r_vFieldArray(InsuranceFileConst.ACLanguageID), Nothing) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    'If Object.Equals(r_vFieldArray(InsuranceFileConst.ACCoverStartDate), Nothing) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    'If Object.Equals(r_vFieldArray(InsuranceFileConst.ACExpiryDate), Nothing) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    'If Object.Equals(r_vFieldArray(InsuranceFileConst.ACRenewalDate), Nothing) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    'If Object.Equals(r_vFieldArray(InsuranceFileConst.ACRenewalFrequencyID), Nothing) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    'If Object.Equals(r_vFieldArray(InsuranceFileConst.ACIsReferredAtRenewal), Nothing) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '

    'If Object.Equals(r_vFieldArray(InsuranceFileConst.ACIsReferredOnMta), Nothing) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '
    '
    ' {* USER DEFINED CODE (End) *}
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMandatory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' Gaurav Changed -
    ' ***************************************************************** '
    ' Name: CreateEvent (Private)
    '
    ' Description: Create an event record.
    '
    ' History: JSB 19/06/2001 - Made function Public, to enable it to be called directly from GII wrapper
    '
    ' ***************************************************************** '
    Public Function CreateEvent(ByRef r_lEventCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_vInsuranceFolderCnt As Object, ByVal v_vInsuranceFileCnt As Object, ByVal v_vClaimCnt As Object, ByVal v_vDocumentCnt As Object, ByVal v_vOldAddressCnt As Object, ByVal v_vNewAddressCnt As Object, ByVal v_vCampaignId As Object, ByVal v_vDocumentTypeId As Object, ByVal v_vReportTypeId As Object, ByVal v_lEventTypeId As Integer, ByVal v_dtEventDate As Date, ByVal v_vDescription As Object) As Integer
        Return CreateEvent(r_lEventCnt:=r_lEventCnt, v_lPartyCnt:=v_lPartyCnt, v_vInsuranceFolderCnt:=v_vInsuranceFolderCnt, v_vInsuranceFileCnt:=v_vInsuranceFileCnt, v_vClaimCnt:=v_vClaimCnt, v_vDocumentCnt:=v_vDocumentCnt, v_vOldAddressCnt:=v_vOldAddressCnt, v_vNewAddressCnt:=v_vNewAddressCnt, v_vCampaignId:=v_vCampaignId, v_vDocumentTypeId:=v_vDocumentTypeId, v_vReportTypeId:=v_vReportTypeId, v_lEventTypeId:=v_lEventTypeId, v_dtEventDate:=v_dtEventDate, v_vDescription:=v_vDescription, v_vIsmanualDescription:=Nothing)
    End Function

    Public Function CreateEvent(ByRef r_lEventCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_vInsuranceFolderCnt As Object, ByVal v_vInsuranceFileCnt As Object, ByVal v_vClaimCnt As Object, ByVal v_vDocumentCnt As Object, ByVal v_vOldAddressCnt As Object, ByVal v_vNewAddressCnt As Object, ByVal v_vCampaignId As Object, ByVal v_vDocumentTypeId As Object, ByVal v_vReportTypeId As Object, ByVal v_lEventTypeId As Integer, ByVal v_dtEventDate As Date, ByVal v_vDescription As Object, ByRef v_vIsmanualDescription As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oEvent Is Nothing Then
                m_oEvent = New bSIREvent.Business()

                m_lReturn = m_oEvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the event object", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
            End If

            If EventDescription = "Policy Copied To Renewal" Then v_lEventTypeId = PMBConst.PMBEventRenewal
            ' Directly add the event
            m_lReturn = m_oEvent.DirectAdd(vEventCnt:=r_lEventCnt, vPartyCnt:=v_lPartyCnt, vInsuranceFolderCnt:=v_vInsuranceFolderCnt, vInsuranceFileCnt:=v_vInsuranceFileCnt, vClaimCnt:=v_vClaimCnt, vDocumentCnt:=v_vDocumentCnt, vOldAddressCnt:=v_vOldAddressCnt, vNewAddressCnt:=v_vNewAddressCnt, vCampaignId:=v_vCampaignId, vDocumentType:=v_vDocumentTypeId, vReportType:=v_vReportTypeId, vEventType:=v_lEventTypeId, vUserId:=m_iUserID, vEventDate:=v_dtEventDate, vDescription:=v_vDescription, v_vIsManualDescription:=v_vIsmanualDescription, vDocument_Path:=Nothing)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the event", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    'eck230800
    ' ***************************************************************** '
    ' Name: DeleteEvent (Private)
    '
    ' Description: Delete an event record.
    '
    ' ***************************************************************** '
    Public Function DeleteEvent(ByRef r_lEventCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oEvent Is Nothing Then
                m_oEvent = New bSIREvent.Business()

                m_lReturn = m_oEvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the event object", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
            End If

            m_lReturn = m_oEvent.DirectDelete(vEventCnt:=r_lEventCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to delete the event", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUnderwritingType
    '
    ' Description:  Finds out Underwriting type - U or A
    '               For labelling: A - Insurer. U - Reinsurer
    '
    ' JMK 13/11/2001    Created
    ' 14/06/2002 SP - moved to uniform Product Options scheme
    ' ***************************************************************** '
    Private Function getUnderwritingType() As String

        Dim result As String = String.Empty


        'PSL 21/10/2003 Removed GSIRLibrary bit, it's not there anymore

        Return CStr(bPMFunc.getUnderwritingType(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingType))

    End Function
    ''' <summary>
    ''' Gets system option value
    ''' </summary>
    ''' <param name="v_iOptionNumber"></param>
    ''' <param name="r_sOptionValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim bSystemOption As bSIROptions.Business
            Dim sOptionValue As String = ""

            bSystemOption = New bSIROptions.Business()

            m_lReturn = bSystemOption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the system option object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If


            m_lReturn = bSystemOption.GetOption(iOptionNumber:=v_iOptionNumber, sValue:=sOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get system option", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            r_sOptionValue = sOptionValue

            If bSystemOption IsNot Nothing Then
                bSystemOption.Dispose()
                bSystemOption = Nothing
            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    'MSS210901 - Added from UW

    ' ***************************************************************** '
    ' Name : MTACancellation
    '
    ' Desc : cancel this mta
    '
    ' Hist : 19 April 2001 Created - Tinny
    ' ***************************************************************** '


    ''' <summary>
    ''' This Function is used to Cancel the Policy status during the MTA And Also Accessible From SAM Layer
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt">Pass InsuranceFileCnt For Cancelling the MTA Quote</param>
    ''' <param name="v_lInsuranceFolderCnt">Pass InsuranceFolderCnt For Cancelling the MTA Quote</param>
    ''' <param name="v_lPartyCnt">Pass Policy PartyCnt For which You Want to cancel the MTA Quote</param>
    ''' <param name="v_sDesc">Remarks If Any</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function MTACancellation(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer,
                                    ByVal v_lPartyCnt As Integer, Optional ByVal v_sDesc As String = "") As Integer
        Dim nResult As Integer = 0
        Dim oRenewalSelection As Object
        Dim nEventCnt As Integer
        Const kInsuranceFileStatus As Integer = 1
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue
            nResult = gPMComponentServices.CreateBusinessObject(r_oObject:=oRenewalSelection, v_sClassName:="bSIRRenSelection.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSIRRenSelection.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteWorkTask", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return nResult
            End If

            nResult = oRenewalSelection.DeleteWorkTask(v_sKeyName:="InsuranceFileKey", v_sKeyValue:=CType(v_lInsuranceFileCnt, String))
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            'clear parameters and add in required parameter
            m_oDatabase.Parameters.Clear()

            nResult = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If
            nResult = m_oDatabase.Parameters.Add(sName:="nInsuranceFileStatus", vValue:=kInsuranceFileStatus, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            'start transaction
            nResult = m_oDatabase.SQLBeginTrans()
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If
            nResult = m_oDatabase.SQLAction(sSQL:=kCancelMTASQL, sSQLName:=kCancelMTAName, bStoredProcedure:=kCancelMTAStored)

            'failed to update rollback transaction
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return nResult
            End If

            'commit transaction
            nResult = m_oDatabase.SQLCommitTrans()

            'log event on sucessfull
            If nResult = gPMConstants.PMEReturnCode.PMTrue Then

                'create event log

                m_lReturn = CreateEvent(r_lEventCnt:=nEventCnt, v_lPartyCnt:=v_lPartyCnt, v_vInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_vInsuranceFileCnt:=v_lInsuranceFileCnt, v_vClaimCnt:=DBNull.Value, v_vDocumentCnt:=DBNull.Value, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentTypeId:=DBNull.Value, v_vReportTypeId:=DBNull.Value, v_lEventTypeId:=PMBConst.PMBEventPolChange, v_dtEventDate:=DateTime.Today, v_vDescription:=If(v_sDesc = "", "Endorsement Cancelled", v_sDesc))


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create event log", vApp:=ACApp, vClass:=ACClass, vMethod:="MTACancellation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return nResult
                End If
            End If
            Return nResult
        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MTACancellation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTACancellation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        End Try
    End Function


    'DJM 12/04/2002 : Inserted following function to allow checking for existing
    '                 policy numbers. This is used when changing policy numbers
    '                 on the policy control.
    ' ***************************************************************** '
    ' Name: CheckIfPolicyNumberExists (Public)
    ' Description: Check If Policy Number Exists
    ' Edit History  :
    ' RAM20040102   : Code added to retun the status of the policy too.
    '                 Ref. PN Issue No. 5965 (SBO 1.8.5)
    '                 Ref. PN Issue No. 9030 (SBO 1.8.6)
    ' ***************************************************************** '
    'Developer Guide No 101
    Public Function CheckIfPolicyNumberExists(ByVal v_vPolicyNumber As Object, ByRef r_vInsuranceFileCnt As String) As Integer
        Return CheckIfPolicyNumberExists(v_vPolicyNumber:=v_vPolicyNumber, r_vInsuranceFileCnt:=r_vInsuranceFileCnt, r_vInsuranceFileStatus:=Nothing)
    End Function

    Public Function CheckIfPolicyNumberExists(ByVal v_vPolicyNumber As Object, ByRef r_vInsuranceFileCnt As String, ByRef r_vInsuranceFileStatus As Object) As Integer

        Dim result As Integer = 0
        Dim vPolicyArray(,) As Object = Nothing
        Dim sSQL As String = ""
        Dim iArraySize As Integer ' RAM20040102 : Declared this variable

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'sSQL = ""
            'sSQL = "SELECT MIN(insurance_file_cnt)" & vbCrLf
            'sSQL = sSQL & "FROM insurance_file" & vbCrLf
            'sSQL = sSQL & "WHERE insurance_ref = '" & CStr(v_vPolicyNumber) & "'"

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20040102   : Commented the above SQL Statement and replace the
            '                   SQL to fetch, Policy_version and Status as well
            '                 Ref. PN Issue 9030 - START
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            sSQL = ""

            'Modifying the inline query to make it compatible with SQL server 2005

            sSQL = sSQL & "SELECT INSF.insurance_file_cnt, INSFS.description, INSF.policy_version "
            sSQL = sSQL & " FROM insurance_file INSF"
            sSQL = sSQL & " LEFT OUTER JOIN insurance_file_status INSFS"
            sSQL = sSQL & " ON INSF.insurance_file_status_id = INSFS.insurance_file_status_id"

            sSQL = sSQL & "WHERE  INSF.insurance_ref = '" & CStr(v_vPolicyNumber) & "' "
            sSQL = sSQL & "ORDER BY policy_version"

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20040102   : Ref. PN Issue 9030 - END
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="CheckIfPolNoExists", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vPolicyArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20040102   : Ref. PN Issue 5965 - Changes
            '                 Return the Insurance File Cnt and Insurance File Status
            '                 of the MAX policy version
            '                 (ie, the ubound array row will conatin these values)
            '                 Ref. PN Issue 9030
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            If Not Informations.IsNothing(r_vInsuranceFileStatus) Then
                r_vInsuranceFileStatus = ""
            End If

            If Informations.IsArray(vPolicyArray) Then

                iArraySize = vPolicyArray.GetUpperBound(1) ' Get the Maximum no of elements

                If CStr(vPolicyArray(0, iArraySize)) = "" Then
                    r_vInsuranceFileCnt = CStr(0)
                Else
                    r_vInsuranceFileCnt = CStr(vPolicyArray(0, iArraySize))

                    r_vInsuranceFileStatus = CStr(vPolicyArray(1, iArraySize))
                End If
            Else
                r_vInsuranceFileCnt = CStr(0)
            End If

            'return the data
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckIfPolicyNumberExists Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckIfPolicyNumberExists", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: AddPolicyClient
    '
    ' Description: Add all policy->client links for insurance_folder_cnt
    '
    ' History :
    '   21/06/2002 PWF (Created)
    ' ***************************************************************** '
    Public Function AddPolicyClient(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_vValueArray(,) As Object) As Integer
        Return AddPolicyClient(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_vValueArray:=v_vValueArray, r_vReturnArray:=Nothing)
    End Function

    Public Function AddPolicyClient(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_vValueArray(,) As Object, ByRef r_vReturnArray() As Object) As Integer

        Dim result As Integer = 0
        Dim lLower, lUpper As Integer
        Dim vOld As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a valid policy array
            If Informations.IsArray(v_vValueArray) Then
                ' Run this whole procedure in a transaction
                m_lReturn = BeginTrans()

                'delete existing subAgents
                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteClientPolicySQL, sSQLName:=ACDeleteClientPolicyName, bStoredProcedure:=ACDeleteClientPolicyStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to delete existing policy->client links", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPolicyClient")
                    Return result
                End If

                ' Add all subagents
                lLower = v_vValueArray.GetLowerBound(1)
                lUpper = v_vValueArray.GetUpperBound(1)

                For lCount As Integer = lLower To lUpper
                    ' Clear parameters
                    m_oDatabase.Parameters.Clear()

                    ' Add insurance folder cnt
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit For
                    End If

                    ' Add party cnt

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_vValueArray(0, lCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit For
                    End If

                    ' Add lead flag

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="is_lead", vValue:=CStr(v_vValueArray(1, lCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit For
                    End If

                    ' Add correspondence flag

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="correspondence", vValue:=CStr(v_vValueArray(2, lCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit For
                    End If

                    ' Add the link
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddClientPolicySQL, sSQLName:=ACAddClientPolicyName, bStoredProcedure:=ACAddClientPolicyStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' If the add failed (or any previous parts) exit the for loop and
                        ' allow outer code to pick up error and rollback transaction.
                        Exit For
                    Else
                        ' Write a debug message to indicate success

                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogDebug1, sMsg:="Updated client->policy linkage " &
                                           v_lInsuranceFolderCnt & "," &
                                           CStr(v_vValueArray(0, lCount)), vApp:=ACApp, vClass:=ACClass, vMethod:="AddPolicyClient")
                    End If

                    ' Check for change of lead

                    If CBool(v_vValueArray(1, lCount)) Then
                        ' Get the old lead


                        vOld = CStr(v_vValueArray(6, lCount)).Split("|"c)

                        ' Check old lead


                        If Not v_vValueArray(0, lCount).Equals(vOld(0)) Then




                            m_lReturn = SetPolicyClientLead(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lPartyCnt:=CInt(v_vValueArray(0, lCount)), v_sResolvedName:=CStr(v_vValueArray(4, lCount)), v_lOldPartyCnt:=CInt(vOld(0)), v_sOldResolvedName:=CStr(vOld(2)))

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                ' If the add failed (or any previous parts) exit the for loop and
                                ' allow outer code to pick up error and rollback transaction.
                                Exit For
                            Else
                                ' Send a message back
                                If Informations.IsArray(r_vReturnArray) Then
                                    ReDim Preserve r_vReturnArray(r_vReturnArray.GetUpperBound(0) + 1)
                                Else
                                    ReDim r_vReturnArray(0)
                                End If

                                r_vReturnArray(r_vReturnArray.GetUpperBound(0)) = "Policy Lead Changed" & Strings.ChrW(13) & Strings.ChrW(10) & "Debt must be managed manually"
                            End If
                        End If
                    End If

                    ' Check for change of correspondence

                    If CBool(v_vValueArray(2, lCount)) Then
                        ' Get the old lead


                        vOld = CStr(v_vValueArray(7, lCount)).Split("|"c)

                        ' Check old lead


                        If Not v_vValueArray(0, lCount).Equals(vOld(0)) Then




                            m_lReturn = SetPolicyClientCorrespondence(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lPartyCnt:=CInt(v_vValueArray(0, lCount)), v_sResolvedName:=CStr(v_vValueArray(4, lCount)), v_lOldPartyCnt:=CInt(vOld(0)), v_sOldResolvedName:=CStr(vOld(2)))

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                ' If the add failed (or any previous parts) exit the for loop and
                                ' allow outer code to pick up error and rollback transaction.
                                Exit For
                            End If
                        End If
                    End If
                Next

                ' Check for inner loop error
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollbackTrans()

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to update existing policy->client links", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPolicyClient")
                    Return result
                Else
                    ' Looks okay, commit the transaction
                    m_lReturn = CommitTrans()
                End If
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddPolicyClient Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPolicyClient", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function
    ''' <summary>
    ''' Get all policy->client links for insurance_folder_cnt
    ''' </summary>
    ''' <param name="v_lInsuranceFolderCnt"></param>
    ''' <param name="v_lPartyCnt"></param>
    ''' <param name="r_vResultArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPolicyClient(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer
        Return GetPolicyClient(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, r_vResultArray:=r_vResultArray, v_lInsuranceFileCnt:=0)
    End Function

    ''' <summary>
    ''' Get all policy->client links for insurance_folder_cnt
    ''' </summary>
    ''' <param name="v_lInsuranceFolderCnt"></param>
    ''' <param name="v_lPartyCnt"></param>
    ''' <param name="r_vResultArray"></param>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPolicyClient(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByRef r_vResultArray(,) As Object, ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Retrieve the policy/client list
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectClientPolicySQL, sSQLName:=ACSelectClientPolicyName, bStoredProcedure:=ACSelectClientPolicyStored, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyClient Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyClient", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function




    ' ***************************************************************** '
    ' Name: SetPolicyClientCorrespondence
    '
    ' Description: Set the policy->client correspondence (only an event)
    '
    ' History :
    '   21/06/2002 PWF (Created)
    ' ***************************************************************** '
    Public Function SetPolicyClientCorrespondence(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_sResolvedName As String, ByVal v_lOldPartyCnt As Integer, ByVal v_sOldResolvedName As String) As Integer

        Dim result As Integer = 0
        Dim sDescription As String = ""
        Dim lEventCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Build description
            sDescription = "Correspondence client changed from '" & v_sOldResolvedName & "' to '" & v_sResolvedName & "'"

            ' Write the event

            m_lReturn = CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=v_lPartyCnt, v_vInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_vInsuranceFileCnt:=v_lInsuranceFileCnt, v_vClaimCnt:=DBNull.Value, v_vDocumentCnt:=DBNull.Value, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentTypeId:=DBNull.Value, v_vReportTypeId:=DBNull.Value, v_lEventTypeId:=PMBConst.PMBEventPolChange, v_dtEventDate:=DateTime.Now, v_vDescription:=sDescription)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to write correspondence change event", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPolicyClientCorrespondence")
                Return result
            Else
                ' Write a debug message to indicate success
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogDebug1, sMsg:="Correspondence change event written successfully", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPolicyClientCorrespondence")
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetPolicyClientCorrespondence Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPolicyClientCorrespondence", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SetPolicyClientLead
    '
    ' Description: Set the policy->client lead on insurance_file/folder
    '
    ' History :
    '   21/06/2002 PWF (Created)
    ' ***************************************************************** '
    Public Function SetPolicyClientLead(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_sResolvedName As String, ByVal v_lOldPartyCnt As Integer, ByVal v_sOldResolvedName As String) As Integer

        Dim result As Integer = 0
        Dim sDescription As String = ""
        Dim lEventCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear parameters
            m_oDatabase.Parameters.Clear()

            ' Add insurance folder cnt
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add insurance file cnt
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add party cnt
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add Resolved Name Paramater
            ' RFC16082002 - Issue 186, Changed resolvedname to resolved_name
            m_lReturn = m_oDatabase.Parameters.Add(sName:="resolved_name", vValue:=v_sResolvedName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the link
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACSetClientPolicyLeadSQL, sSQLName:=ACSetClientPolicyLeadName, bStoredProcedure:=ACSetClientPolicyLeadStored)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to update policy->client lead details", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPolicyClientLead")
                Return result
            End If

            ' Build description
            sDescription = "Lead client changed from '" & v_sOldResolvedName & "' to '" & v_sResolvedName & "'"

            ' Write the event

            m_lReturn = CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=v_lPartyCnt, v_vInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_vInsuranceFileCnt:=v_lInsuranceFileCnt, v_vClaimCnt:=DBNull.Value, v_vDocumentCnt:=DBNull.Value, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentTypeId:=DBNull.Value, v_vReportTypeId:=DBNull.Value, v_lEventTypeId:=PMBConst.PMBEventPolChange, v_dtEventDate:=DateTime.Now, v_vDescription:=sDescription)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to write lead change event", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPolicyClientLead")
                Return result
            Else
                ' Write a debug message to indicate success
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogDebug1, sMsg:="Lead change event written successfully", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPolicyClientLead")
            End If

            'PSL 21/10/2003 Issue 5020
            ' Write the event again for the old client

            m_lReturn = CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=v_lOldPartyCnt, v_vInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_vInsuranceFileCnt:=v_lInsuranceFileCnt, v_vClaimCnt:=DBNull.Value, v_vDocumentCnt:=DBNull.Value, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentTypeId:=DBNull.Value, v_vReportTypeId:=DBNull.Value, v_lEventTypeId:=PMBConst.PMBEventPolChange, v_dtEventDate:=DateTime.Now, v_vDescription:=sDescription)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to write lead change event", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPolicyClientLead")
                Return result
            Else
                ' Write a debug message to indicate success
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogDebug1, sMsg:="Lead change event written successfully", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPolicyClientLead")
            End If

            'Update any Claims

            ' Clear parameters
            m_oDatabase.Parameters.Clear()

            'Add the new paty cnt parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Party_Cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add insurance file cnt parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Update the claims
            'Developer Guide No 39. 

            m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_Claim_NewLeadClient", sSQLName:="spu_Claim_NewLeadClient", bStoredProcedure:=True)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update claims with change of lead client", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPolicyClientLead")
                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetPolicyClientLead Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPolicyClientLead", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetSubBranches
    '
    ' Description:
    '
    ' History: 11/06/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function GetSubBranches(ByVal v_lSourceID As Integer, ByRef r_vSubBranchArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Return SiriusCoreFunc.GetSubBranches(v_oDatabase:=m_oDatabase, v_lSourceID:=v_lSourceID, r_vSubBranchArray:=r_vSubBranchArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSubBranches Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubBranches", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         GetPartySubBranch

    ' Description:  This function fetch the related sub branch of the
    'related client
    ' ***************************************************************** '
    Public Function GetPartySubBranch(ByVal v_lPartyCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartySubBranchSQL, sSQLName:=ACGetPartySubBranchName, bStoredProcedure:=ACGetPartySubBranchStored, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartySubBranch Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartySubBranch", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    'Inserted as part of 1.6.9 --> 1.8.6 Catchup
    Public Function DeleteLogEvent() As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "DELETE from event_log "
            sSQL = sSQL & "WHERE event_cnt = " & CStr(m_lTransInsuranceFileCnt)

            With m_oDatabase
                .Parameters.Clear()

                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="DeleteLogEvent", bStoredProcedure:=False, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteLogEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteLogEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RetrieveCurrenciesForBranch
    '
    ' Description: get currencies used by branch
    '
    ' History:
    '     Moh 15/05/2003
    '     RDC 02062004 moved to bPMFunc
    ' New parameter added for deleted currency.
    ' ***************************************************************** '
    Public Function RetrieveCurrenciesForBranch(ByRef iSourceID As Integer, ByRef vReturnArray(,) As Object) As Integer
        Return RetrieveCurrenciesForBranch(iSourceID:=iSourceID, vReturnArray:=vReturnArray, bRestrictDeletedCurrency:=False)
    End Function

    Public Function RetrieveCurrenciesForBranch(ByRef iSourceID As Integer, ByRef vReturnArray(,) As Object, ByVal bRestrictDeletedCurrency As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = bPMFunc.GetBranchCurrencies(v_iSourceID:=iSourceID, v_oDatabase:=m_oDatabase, r_vReturnArray:=vReturnArray, v_bRestrictDeletedCurrency:=bRestrictDeletedCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(vReturnArray) Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RetrieveCurrenciesForBranch failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RetrieveCurrenciesForBranch", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetDefaultPreferredCorrespondence(ByVal v_lPartyCnt As Integer, ByRef vResultArray As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get is_midnight_renewal from the product table
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDefaultPreferredCorrespondenceSQL, sSQLName:=ACGetDefaultPreferredCorrespondenceName, bStoredProcedure:=ACGetDefaultPreferredCorrespondenceStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                vResultArray = Nothing
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaultClientPreferredCorrespondence Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultClientPreferredCorrespondence", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    Public Function GetExistingPreferredCorrespondence(ByVal v_lInsuranceFileCnt As Integer, ByRef vResultArray As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get is_midnight_renewal from the product table
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACExistingPreferredCorrespondenceSQL, sSQLName:=ACExistingPreferredCorrespondenceName, bStoredProcedure:=ACExistingPreferredCorrespondenceStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                vResultArray = Nothing
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetExistingPreferredCorrespondence Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExistingPreferredCorrespondence", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    'DJM 24/03/2004 : Get UW Product details.
    Public Function GetProductDetails(ByVal v_lProductId As Integer, ByVal v_lOption As Integer, ByRef r_vValue As Object) As Integer

        Dim result As Integer = 0
        Dim oProductBusiness As bSIRProduct.Business
        Dim vDetails(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            oProductBusiness = New bSIRProduct.Business
            m_lReturn = oProductBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oProductBusiness.GetProductDetails(v_lProductId:=v_lProductId, r_vResultArray:=vDetails)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_lOption < 0 Then


                r_vValue = vDetails
            Else
                Dim auxVar As Object = vDetails(v_lOption, 0)


                If Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar) Then

                    r_vValue = ""
                Else


                    r_vValue = vDetails(v_lOption, 0)
                End If
            End If


            oProductBusiness.Dispose()
            oProductBusiness = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProductDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProductDetails", vErrNo:=informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         GetBranches
    ' Description:  This function loads all open branches, PLUS the
    '               "v_lIncludeThisBranchID" one, even if it is closed!
    ' ***************************************************************** '
    Public Function GetBranches(ByVal v_lUserID As Integer, ByVal v_lIncludeThisBranchID As Integer, ByRef r_vResultArray(,) As Object) As Integer
        Return GetBranches(v_lUserID:=v_lUserID, v_lIncludeThisBranchID:=v_lIncludeThisBranchID, r_vResultArray:=r_vResultArray, v_lProductId:=0)
    End Function

    Public Function GetBranches(ByVal v_lUserID As Integer, ByVal v_lIncludeThisBranchID As Integer, ByRef r_vResultArray(,) As Object, ByVal v_lProductId As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="UserID", vValue:=CStr(v_lUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="IncludeBranchID", vValue:=CStr(v_lIncludeThisBranchID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_lProductId > 0 Then

                m_lReturn = m_oDatabase.Parameters.Add(sName:="ProductID", vValue:=CStr(v_lProductId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetBranchesSQL, sSQLName:=ACGetBranchesName, bStoredProcedure:=ACGetBranchesStored, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBranches Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranches", vErrNo:=informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         HasInstalment
    ' Description:  This function checks for the instalment plans asso
    '                ciated!
    ' ***************************************************************** '

    Public Function HasInstalment(ByRef v_insurance_file_cnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            m_oDatabase.Parameters.Clear()

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_insurance_file_cnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACHasInstalmentSQL, sSQLName:=ACHasInstalmentName, bStoredProcedure:=ACHasInstalmentStored, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If Not Informations.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If
            Return result

        Catch excep As System.Exception



            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="HasInstalment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="HasInstalment", vErrNo:=informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError
        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         GetBranchBaseCurrency
    ' Description:  This function returns the Base currency of the Branch Supplied
    '
    ' History:
    '     MKR 28/06/2005 - Created (PN 21989)
    ' ***************************************************************** '
    'PN30098 - Datasure
    Public Function GetBranchBaseCurrency(ByRef vSourceID As Object, ByRef iCurrencyID As Integer) As Integer
        Return GetBranchBaseCurrency(vSourceID:=vSourceID, iCurrencyID:=iCurrencyID, sCurSymbol:="")
    End Function

    Public Function GetBranchBaseCurrency(ByRef vSourceID As Object, ByRef iCurrencyID As Integer, ByRef sCurSymbol As String) As Integer

        Dim result As Integer = 0
        Try
            'PN30098 - Datasure

            'Developer Guide No. 98
            Return bPMFunc.GetBranchBaseCurrency(v_lSourceID:=vSourceID, v_oDatabase:=m_oDatabase, r_iCurrencyID:=iCurrencyID, r_sCurSymbol:=sCurSymbol)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetBranchBaseCurrency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranchBaseCurrency", vErrNo:=informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CalculateDiscount (Public)
    '
    ' Description: Calculate the discount for insurance_file on discount_percentage
    '
    ' RKS 16/09/2005  Policy Discount work
    ' ***************************************************************** '
    Public Function CalculateDiscount(ByRef vInsuranceFileCnt As Object, ByRef vDiscountPercentage As Object) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            With m_oDatabase

                .Parameters.Clear()



                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .Parameters.Add(sName:="discount_percentage", vValue:=CStr(vDiscountPercentage), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)


                m_lReturn = .SQLAction(sSQL:=ACCalculateDiscountSQL, sSQLName:=ACCalculateDiscountName, bStoredProcedure:=ACCalculateDiscountStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CalculateDiscount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculateDiscount", vErrNo:=informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: ApplyDiscount (Public)
    '
    ' Description: Apply the discount
    '
    ' RKS 16/09/2005  Policy Discount work
    ' ***************************************************************** '
    Public Function ApplyDiscount(ByRef vInsuranceFileCnt As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            With m_oDatabase

                .Parameters.Clear()



                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .SQLAction(sSQL:=ACApplyDiscountSQL, sSQLName:=ACApplyDiscountName, bStoredProcedure:=ACApplyDiscountStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ApplyDiscount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ApplyDiscount", vErrNo:=informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: AdjustDiscount (Public)
    '
    ' Description: Adjust the amount to match discounted premium
    '
    ' RKS 16/09/2005  Policy Discount work
    ' ***************************************************************** '
    Public Function AdjustDiscount(ByRef vInsuranceFileCnt As Object, ByRef vAdjutedDiscount As Object) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            With m_oDatabase

                .Parameters.Clear()



                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .Parameters.Add(sName:="adjusted_discount", vValue:=CStr(vAdjutedDiscount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)


                m_lReturn = .SQLAction(sSQL:=ACAdjustDiscountSQL, sSQLName:=ACAdjustDiscountName, bStoredProcedure:=ACAdjustDiscountStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AdjustDiscount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AdjustDiscount", vErrNo:=informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetPolicyDiscountTotalPremium
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function GetPolicyDiscountTotalPremium(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPolicyDiscountTotalPremium"


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetPolicyDiscountTotalPremiumSQL, sSQLName:=kGetPolicyDiscountTotalPremiumName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetPolicyDiscountTotalPremiumSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If
            Return result

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AddInputParameter
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddInputParameter"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add Parameter to database object

            lReturn = m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, " Failed to add parameter name:" & v_sName &
                                        ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), gPMConstants.PMELogLevel.PMLogError)
            End If
            Return result

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function



    ' ***************************************************************** '
    ' Name: GetSelectedRiskCount
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 13-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function GetSelectedRiskCount(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetSelectedRiskCount"



        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetSelectedRiskCountSQL, sSQLName:=kGetSelectedRiskCountName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetSelectedRiskCountSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If
            Return result

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: Get Payment Methods
    '
    ' Description: get all payment methods
    '
    ' History : 22/09/2006 (Created)
    '
    ' ***************************************************************** '
    Public Function GetPaymentMethod(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try
            Dim sSQL As String = ""
            Dim vResultArray(,) As Object = Nothing

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "select payment_method_id,Description from payment_method where is_deleted=0"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPaymentMethod", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' No values, so return not found
            If Not informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Return the array


            r_vResultArray = vResultArray

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPaymentMethod Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentMethod", vErrNo:=informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'Developer Guide No 101
    Public Function GetPolicyLimitsDesc(ByVal v_iPolicyLimitsId As Integer, ByRef v_vPolicyLimits As String) As Integer

        Dim result As Integer = 0
        Dim vPolicyLimits As Object = Nothing
        Dim vResultArray(,) As Object = Nothing

        Try


            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="policylimitsid", vValue:=CStr(v_iPolicyLimitsId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=CStr(vPolicyLimits), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyLimitsDescSQL, sSQLName:=ACGetPolicyLimitsDescName, bStoredProcedure:=ACGetPolicyLimitsDescStored, lNumberRecords:=1, vResultArray:=vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                'return the data
                v_vPolicyLimits = gPMFunctions.ToSafeString(m_oDatabase.Parameters.Item("description").Value, CStr(False))
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyLimitsDesc Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyLimitsDesc", vErrNo:=informations.Err().Number, vErrDesc:=informations.Err().Description)

        Return result
    End Function

    'Developer Guide No 101
    Public Function GetPolicyDeductibleDesc(ByVal v_iPolicyDeductiblesId As Integer, ByRef v_vPolicyDeductibles As Object) As Integer

        Dim result As Integer = 0
        Dim vPolicyDeductibles As Object = Nothing
        Dim vResultArray(,) As Object = Nothing

        Try

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="policydeductiblesid", vValue:=CStr(v_iPolicyDeductiblesId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=CStr(vPolicyDeductibles), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyDeductibleDescSQL, sSQLName:=ACGetPolicyDeductibleDescName, bStoredProcedure:=ACGetPolicyDeductibleDescStored, lNumberRecords:=1, vResultArray:=vResultArray)



            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                'return the data
                v_vPolicyDeductibles = gPMFunctions.ToSafeString(m_oDatabase.Parameters.Item("description").Value, CStr(False))
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

        Catch
        End Try


        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyDeductibleDesc Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyDeductibleDesc", vErrNo:=informations.Err().Number, vErrDesc:=informations.Err().Description)

        Return result
    End Function

    ' **************************************************************************** '
    ' Name: DeleteRiskData
    ' Description: Deletes all corresponding Risk data for a Policy
    ' **************************************************************************** '
    Public Function DeleteRiskData(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DeleteRiskData"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = BeginTrans()

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(CStr(m_lReturn), "Begin Trans Failed.")
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteRiskDataSQL, sSQLName:=ACDeleteRiskDataName, bStoredProcedure:=ACDeleteRiskDataStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(CStr(m_lReturn), "Stored procedure " & ACDeleteRiskDataName & " failed.")
            End If

            lReturn = CommitTrans()

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(CStr(m_lReturn), "Commit Trans Failed.")
            End If
            Return result

        Catch ex As Exception

            'DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            lReturn = RollbackTrans()

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetRiskCodeId
    ' Description: Find Risk Code Id for a policy
    ' ***************************************************************** '
    Public Function GetRiskCodeId(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetRiskCodeId"


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyRiskSQL, sSQLName:=ACGetPolicyRiskName, bStoredProcedure:=ACGetPolicyRiskStored, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetPolicyRiskSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            Return result

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    ' ***************************************************************** '
    ' Name: GetAgentDetail
    ' ***************************************************************** '
    Public Function GetAgentDetail(ByVal v_lAgentCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAgentDetail"



        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = AddInputParameter(v_sName:="party_cnt", v_vValue:=v_lAgentCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAgentDetailSQL, sSQLName:=ACGetAgentDetailName, bStoredProcedure:=ACGetAgentDetailStored, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetAgentDetailSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            Return result

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function ValidateProductBranch(ByVal v_lProductID As Integer, ByVal v_lBranchID As Integer, ByRef r_bIsValid As Boolean) As Integer
        Dim result As Integer = 0
        Dim r_vResultArray(,) As Object = Nothing
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ProductID", vValue:=CStr(v_lProductID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="BranchID", vValue:=CStr(v_lBranchID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add("IsValid", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_Check_Product_Branch", sSQLName:="spu_Check_Product_Branch", bStoredProcedure:=True, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_bIsValid = (m_oDatabase.Parameters.Item("IsValid").Value = 1)

            Return result

        Catch excep As System.Exception



            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateProductBranch Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateProductBranch", vErrNo:=informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_lRenewalFrequencyID"></param>
    ''' <param name="r_lRenewalFrequencyMonths"></param>
    ''' <returns></returns>
    Public Function GetRenewalFrequencyMonths(ByVal v_lRenewalFrequencyID As Integer, ByRef r_lRenewalFrequencyMonths As Integer) As Integer
        Dim nResult As Integer = 0
        Dim r_vResultArray(,) As Object = Nothing
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="RenewalFrequencyId", vValue:=CStr(v_lRenewalFrequencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRenewalFrequencyMonthsSQL, sSQLName:="GetRenewalFrequencyMonths", bStoredProcedure:=False, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_lRenewalFrequencyMonths = CInt(r_vResultArray(0, 0))

            Return nResult

        Catch excep As System.Exception

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRenewalFrequencyMonths Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRenewalFrequencyMonths", vErrNo:=informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

End Class
