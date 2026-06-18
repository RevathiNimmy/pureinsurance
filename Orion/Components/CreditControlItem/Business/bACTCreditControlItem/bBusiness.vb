Option Strict Off
Option Explicit On
Imports SSP.Shared
'Developer Guide No 129

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' *****************************************************************
    ' Class Name: Business
    '
    ' Date: 03/10/2002
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a Credit Control Item.
    '
    ' Edit History:
    ' *****************************************************************


    ' ************************************************
    ' Added to replace global variables 09/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    ' Task
    Private m_iTask As Integer
    ' Navigate
    Private m_lNavigate As Integer
    ' Process Mode
    Private m_lProcessMode As Integer
    ' Type of Business
    Private m_sTransactionType As String = ""
    ' Effective
    Private m_dtEffectiveDate As Date

    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business

    'SP to Select a list of Credit Control Items
    Private Const ACSelAllName As String = "SelAllCreditControlItem"
    Private Const ACSelAllSQL As String = "spu_ACT_SelAll_Credit_Control_Item"

    'SP to Select a single Credit Control Item
    Private Const ACSelName As String = "SelectCreditControlItem"
    Private Const ACSelSQL As String = "spu_ACT_Select_Credit_Control_Item"

    'SP to Add a Credit Control Item
    Private Const ACDirectAddName As String = "AddCreditControlItem"
    Private Const ACDirectAddSQL As String = "spu_ACT_Add_Credit_Control_Item"

    'SP to Edit a Credit Control Item
    Private Const ACDirectEditName As String = "UpdateCreditControlItem"
    Private Const ACDirectEditSQL As String = "spu_ACT_Update_Credit_Control_Item"

    'SP to Delete a Credit Control Item
    Private Const ACDirectDeleteName As String = "DeleteCreditControlItem"
    Private Const ACDirectDeleteSQL As String = "spu_ACT_Delete_Credit_Control_Item"

    'SP to Select a Credit Control Item for an instalment plan
    Private Const ACSelectItemForPlanName As String = "SelectItemForPlan"
    Private Const ACSelectItemForPlanSQL As String = "spu_ACT_Select_Credit_Control_Item_For_Plan"

    'SP to Get Credit Control Item details via the instalment plan
    Private Const ACGetPlanDetailsName As String = "GetPlanDetails"
    Private Const ACGetPlanDetailsSQL As String = "spu_ACT_Credit_Control_Item_Get_Plan_Details"

    Private Const kGetCreditControlDetailsForInstalmentSQL As String = "spu_ACT_Get_Credit_Control_Details_For_Instalment"
    Private Const kGetCreditControlDetailsForInstalmentName As String = "spu_ACT_Get_Credit_Control_Details_For_Instalment"

    Private Const kUpdateCreditControlItemSQL As String = "spu_ACT_Credit_Control_Item_Update"
    Private Const kUpdateCreditControlItemName As String = "spu_ACT_Credit_Control_Item_Update"

    'Result Array columns for GetDetails for Credit Control Item
    Private Const ACCreditControlItemID As Integer = 0
    Private Const ACCreditControlReason As Integer = 1
    Private Const ACAccountID As Integer = 2
    Private Const ACDocumentID As Integer = 3
    Private Const ACDocumentDate As Integer = 4
    Private Const ACInsuranceFileCnt As Integer = 5
    Private Const ACPFPremFinanceCnt As Integer = 6
    Private Const ACPFPremFinanceVersion As Integer = 7
    Private Const ACAmount As Integer = 8
    Private Const ACCanAutoCancel As Integer = 9
    Private Const ACWillAutoCancel As Integer = 10
    Private Const ACCreditControlStepID As Integer = 11
    Private Const ACCreatedDate As Integer = 12
    Private Const ACDueDate As Integer = 13
    Private Const ACLetterSent As Integer = 14
    Private Const ACRecurrenceCount As Integer = 15
    ' This next one is extra one that is returned when we call the
    ' spu_ACT_Select_Credit_Control_Item_For_Plan procedure

    ' PW191103 - CQ3102 - change constant to reflect changes to SP
    Private Const ACNextStepID As Integer = 25

    'jmf 28/7/2003
    Private Const ACPMUserGroupId As Integer = 17
    Private Const ACPMUserId As Integer = 18
    Private Const ACClaimId As Integer = 19
    Private Const ACClaimDebtId As Integer = 20
    Private Const ACClaimDebtVersion As Integer = 21
    Private Const ACPartialAmount As Integer = 22
    Private Const ACIsDeleted As Integer = 23
    Private Const ACPFInstalmentsId As Integer = 24

    ' Constant for add or edit
    Private Const ACUpdateAdd As Byte = 1
    Private Const ACUpdateEdit As Byte = 2
    ' PRIVATE Data Members (End)

    ' *****************************************************************
    ' Name: AddInstalment (Public)
    '
    ' Description:
    '
    ' *****************************************************************
    Public Function AddInstalment(ByVal v_lPFInstalmentsID As Integer, ByVal v_sReason As String, ByVal v_cAmount As Decimal) As Integer

        ' retained interface for compatibility
        ' always return error as method is no longer supported
        Return gPMConstants.PMEReturnCode.PMError

    End Function

    '' *****************************************************************
    '
    '' Name: AddInstalment (Public)
    ''
    '' Description:
    ''
    '' *****************************************************************
    'Public Function AddInstalment(ByVal v_lPFInstalmentsID As Long, _
    ''                              ByVal v_sReason As String, _
    ''                              ByVal v_cAmount As Currency) As Long
    '
    'Dim vResultArray As Variant
    'Dim lCreditControlItemId As Long
    'Dim lPFPremFinanceCnt As Long
    'Dim lPFPremFinanceVersion As Long
    '
    '    On Error GoTo Err_AddInstalment
    '
    '    AddInstalment = PMTRue
    ''
    '' Check if there is already a credit control item for this Plan
    ''
    '    ' Clear the Database Parameters Collection
    '    m_oDatabase.Parameters.Clear
    '
    '    ' Add the premium finance count parameter
    '    m_lReturn = m_oDatabase.Parameters.Add( _
    ''            sName:="pfinstalments_id", _
    ''            vValue:=v_lPFInstalmentsID, _
    ''            idirection:=PMParamInput, _
    ''            iDataType:=PMLong)
    '
    '    If (m_lReturn <> PMTRue) Then
    '        AddInstalment = PMFalse
    '        Exit Function
    '    End If
    '
    '    ' Execute SQL Statement
    '    m_lReturn = m_oDatabase.SQLSelect( _
    ''        sSQL:=ACSelectItemForPlanSQL, _
    ''        sSQLName:=ACSelectItemForPlanName, _
    ''        bStoredProcedure:=True, _
    ''        lNumberRecords:=PMAllRecords, _
    ''        vResultArray:=vResultArray)
    '
    '    If (m_lReturn <> PMTRue) Then
    '        AddInstalment = PMFalse
    '        Exit Function
    '    End If
    '
    ''
    '' If there is already a credit control item, update it
    ''
    '    If IsArray(vResultArray) Then
    '
    '        m_lReturn = DirectEdit( _
    ''            v_vCreditControlItemID:=vResultArray(ACCreditControlItemID, 0), _
    ''            v_vCreditControlReason:=v_sReason, _
    ''            v_vAccountID:=vResultArray(ACAccountID, 0), _
    ''            v_vDocumentID:=vResultArray(ACDocumentID, 0), _
    ''            v_vDocumentDate:=vResultArray(ACDocumentDate, 0), _
    ''            v_vInsuranceFileCnt:=vResultArray(ACInsuranceFileCnt, 0), _
    ''            v_vPFPremFinanceCnt:=vResultArray(ACPFPremFinanceCnt, 0), _
    ''            v_vPFPremFinanceVersion:=vResultArray(ACPFPremFinanceVersion, 0), _
    ''            v_vAmount:=v_cAmount, _
    ''            v_vCanAutoCancel:=vResultArray(ACCanAutoCancel, 0), _
    ''            v_vWillAutoCancel:=vResultArray(ACWillAutoCancel, 0), _
    ''            v_vCreditControlStepID:=vResultArray(ACNextStepID, 0), _
    ''            v_vCreatedDate:=vResultArray(ACCreatedDate, 0), _
    ''            v_vDueDate:=vResultArray(ACDueDate, 0), _
    ''            v_vLetterSent:=vResultArray(ACLetterSent, 0), _
    ''            v_vRecurrenceCount:=vResultArray(ACRecurrenceCount, 0), _
    ''            v_vPMUserGroupId:=vResultArray(ACPMUserGroupId, 0), _
    ''            v_vPMUserId:=vResultArray(ACPMUserId, 0), _
    ''            v_vClaimId:=vResultArray(ACClaimId, 0), _
    ''            v_vClaimDebtId:=vResultArray(ACClaimDebtId, 0), _
    ''            v_vClaimDebtVersion:=vResultArray(ACClaimDebtVersion, 0), _
    ''            v_vPartialAmount:=vResultArray(ACPartialAmount, 0), _
    ''            v_vIsDeleted:=0, _
    ''            v_vPFInstalmentsId:=vResultArray(ACPFInstalmentsId, 0))
    '
    '        If (m_lReturn <> PMTRue) Then
    '            AddInstalment = PMFalse
    '            Exit Function
    '        End If
    '
    ''
    '' If there is not already a credit control item, create it
    ''
    '    Else
    '        ' Get details relating to plan:
    '
    '
    '
    '        ' Execute SQL Statement
    '        m_lReturn = m_oDatabase.SQLSelect( _
    ''            sSQL:=ACGetPlanDetailsSQL, _
    ''            sSQLName:=ACGetPlanDetailsName, _
    ''            bStoredProcedure:=True, _
    ''            lNumberRecords:=PMAllRecords, _
    ''            vResultArray:=vResultArray)
    '
    '        If (m_lReturn <> PMTRue) Then
    '            AddInstalment = PMFalse
    '            Exit Function
    '        End If
    '
    '        ' Add the credit control item record
    '        m_lReturn = DirectAdd( _
    ''            r_vCreditControlItemID:=lCreditControlItemId, _
    ''            v_vCreditControlReason:=v_sReason, _
    ''            v_vAccountID:=Val(vResultArray(0, 0)), _
    ''            v_vDocumentID:=Val(vResultArray(1, 0)), _
    ''            v_vDocumentDate:=Now, _
    ''            v_vInsuranceFileCnt:=Val(vResultArray(2, 0)), _
    ''            v_vPFPremFinanceCnt:=vResultArray(6, 0), _
    ''            v_vPFPremFinanceVersion:=vResultArray(7, 0), _
    ''            v_vAmount:=v_cAmount, _
    ''            v_vCanAutoCancel:=Val(vResultArray(3, 0)), _
    ''            v_vWillAutoCancel:="0", _
    ''            v_vCreditControlStepID:=Val(vResultArray(4, 0)), _
    ''            v_vCreatedDate:=Now, _
    ''            v_vDueDate:=DateAdd("d", Val(vResultArray(5, 0)), Now), _
    ''            v_vLetterSent:="0", _
    ''            v_vRecurrenceCount:="0", _
    ''            v_vPMUserGroupId:=0, _
    ''            v_vPMUserId:=0, _
    ''            v_vClaimId:=0, _
    ''            v_vClaimDebtId:=0, _
    ''            v_vClaimDebtVersion:=0, _
    ''            v_vPartialAmount:=0, _
    ''            v_vIsDeleted:=0, _
    ''            v_vPFInstalmentsId:=v_lPFInstalmentsID)
    '
    '         If (m_lReturn <> PMTRue) Then
    '            AddInstalment = PMFalse
    '            Exit Function
    '        End If
    '
    '    End If
    '
    '    Exit Function
    '
    'Err_AddInstalment:
    '
    '    ' Error.
    '    AddInstalment = PMError
    '
    '    ' Log Error Message
    '    LogMessage m_sUsername, _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="AddInstalment Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="AddInstalment", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    'End Function

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
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


    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' *****************************************************************
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' *****************************************************************
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            ' Initialisation Code.

            ' Set Username and Password

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            ' Get Reference to Database


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' *****************************************************************
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' *****************************************************************
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' *****************************************************************
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' *****************************************************************
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



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' *****************************************************************
    ' Name: GetList (Public)
    '
    ' Description: Select multiple Credit Control Rule records from
    ' the database.
    '
    ' *****************************************************************
    Public Function GetList(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelAllSQL, sSQLName:=ACSelAllName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get all Credit Control Item records", vApp:=ACApp, vClass:=ACClass, vMethod:="GetList", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' *****************************************************************
    ' Name: GetDetails (Public)
    '
    ' Description: Select a single Credit Control Item record from the
    ' database.
    '
    ' *****************************************************************
    Public Function GetDetails(ByVal v_lCreditControlItemId As Integer, Optional ByRef r_vCreditControlReason As Object = Nothing, Optional ByRef r_vAccountID As Object = Nothing, Optional ByRef r_vDocumentID As Object = Nothing, Optional ByRef r_vDocumentDate As Object = Nothing, Optional ByRef r_vInsuranceFileCnt As Object = Nothing, Optional ByRef r_vPFPremFinanceCnt As Object = Nothing, Optional ByRef r_vPFPremFinanceVersion As Object = Nothing, Optional ByRef r_vAmount As Object = Nothing, Optional ByRef r_vCanAutoCancel As Object = Nothing, Optional ByRef r_vWillAutoCancel As Object = Nothing, Optional ByRef r_vCreditControlStepID As Object = Nothing, Optional ByRef r_vCreatedDate As Object = Nothing, Optional ByRef r_vDueDate As Object = Nothing, Optional ByRef r_vLetterSent As Object = Nothing, Optional ByRef r_vRecurrenceCount As Object = Nothing, Optional ByVal v_vPMUserGroupId As Object = Nothing, Optional ByVal v_vPMUserId As Object = Nothing, Optional ByVal v_vClaimId As Object = Nothing, Optional ByVal v_vClaimDebtId As Object = Nothing, Optional ByVal v_vClaimDebtVersion As Object = Nothing, Optional ByVal v_vPartialAmount As Object = Nothing, Optional ByVal v_vIsDeleted As Object = Nothing, Optional ByRef v_vPFInstalmentsId As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Const klFirstRow As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Credit Control Item id INPUT parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="credit_control_item_id", vValue:=v_lCreditControlItemId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelSQL, sSQLName:=ACSelName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Populate the params



            r_vCreditControlReason = vResultArray(ACCreditControlReason, klFirstRow)

            r_vAccountID = vResultArray(ACAccountID, klFirstRow)
            r_vDocumentID = vResultArray(ACDocumentID, klFirstRow)
            r_vDocumentDate = vResultArray(ACDocumentDate, klFirstRow)
            r_vInsuranceFileCnt = vResultArray(ACInsuranceFileCnt, klFirstRow)
            r_vPFPremFinanceCnt = vResultArray(ACPFPremFinanceCnt, klFirstRow)
            r_vPFPremFinanceVersion = vResultArray(ACPFPremFinanceVersion, klFirstRow)
            r_vAmount = vResultArray(ACAmount, klFirstRow)
            r_vCanAutoCancel = vResultArray(ACCanAutoCancel, klFirstRow)
            r_vWillAutoCancel = vResultArray(ACWillAutoCancel, klFirstRow)
            r_vCreditControlStepID = vResultArray(ACCreditControlStepID, klFirstRow)
            r_vCreatedDate = vResultArray(ACCreatedDate, klFirstRow)
            r_vDueDate = vResultArray(ACDueDate, klFirstRow)
            r_vLetterSent = vResultArray(ACLetterSent, klFirstRow)
            r_vRecurrenceCount = vResultArray(ACRecurrenceCount, klFirstRow)

            'jmf 28/7/2003

            v_vPMUserGroupId = vResultArray(ACPMUserGroupId, klFirstRow)

            v_vPMUserId = vResultArray(ACPMUserId, klFirstRow)
            v_vClaimId = vResultArray(ACClaimId, klFirstRow)
            v_vClaimDebtId = vResultArray(ACClaimDebtId, klFirstRow)
            v_vClaimDebtVersion = vResultArray(ACClaimDebtVersion, klFirstRow)
            v_vPartialAmount = vResultArray(ACPartialAmount, klFirstRow)
            v_vIsDeleted = vResultArray(ACIsDeleted, klFirstRow)
            v_vPFInstalmentsId = vResultArray(ACPFInstalmentsId, klFirstRow)


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get the Credit Control Item", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' *****************************************************************
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a Credit Control Item record to the database
    '
    ' *****************************************************************
    Public Function DirectAdd(ByRef r_vCreditControlItemID As Integer, ByVal v_vCreditControlReason As Object, ByVal v_vAccountID As Integer, ByVal v_vDocumentID As Object, ByVal v_vDocumentDate As Object, ByVal v_vInsuranceFileCnt As Integer, ByVal v_vPFPremFinanceCnt As Object, ByVal v_vPFPremFinanceVersion As Object, ByVal v_vAmount As Object, ByVal v_vCanAutoCancel As Object, ByVal v_vWillAutoCancel As Object, ByVal v_vCreditControlStepID As Object, ByVal v_vCreatedDate As Object, ByVal v_vDueDate As Object, ByVal v_vLetterSent As Object, ByVal v_vRecurrenceCount As Object, ByVal v_vPMUserGroupId As Object, ByVal v_vPMUserId As Object, ByVal v_vClaimId As Object, ByVal v_vClaimDebtId As Object, ByVal v_vClaimDebtVersion As Object, ByVal v_vPartialAmount As Object, ByVal v_vIsDeleted As Object, ByVal v_vPFInstalmentsId As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call the update method
            m_lReturn = CType(DirectUpdate(r_vCreditControlItemID:=r_vCreditControlItemID, v_vCreditControlReason:=v_vCreditControlReason, v_vAccountID:=v_vAccountID, v_vDocumentID:=v_vDocumentID, v_vDocumentDate:=v_vDocumentDate, v_vInsuranceFileCnt:=v_vInsuranceFileCnt, v_vPFPremFinanceCnt:=v_vPFPremFinanceCnt, v_vPFPremFinanceVersion:=v_vPFPremFinanceVersion, v_vAmount:=v_vAmount, v_vCanAutoCancel:=v_vCanAutoCancel, v_vWillAutoCancel:=v_vWillAutoCancel, v_vCreditControlStepID:=v_vCreditControlStepID, v_vCreatedDate:=v_vCreatedDate, v_vDueDate:=v_vDueDate, v_vLetterSent:=v_vLetterSent, v_vRecurrenceCount:=v_vRecurrenceCount, v_bAction:=ACUpdateAdd, v_vPMUserGroupId:=v_vPMUserGroupId, v_vPMUserId:=v_vPMUserId, v_vClaimId:=v_vClaimId, v_vClaimDebtId:=v_vClaimDebtId, v_vClaimDebtVersion:=v_vClaimDebtVersion, v_vPartialAmount:=v_vPartialAmount, v_vIsDeleted:=v_vIsDeleted, v_vPFInstalmentsId:=v_vPFInstalmentsId), gPMConstants.PMEReturnCode)



            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' *****************************************************************
    ' Name: DirectUpdate (Private)
    '
    ' Description: Adds or edits a Credit Control Item record
    '
    ' *****************************************************************
    Private Function DirectUpdate(ByRef r_vCreditControlItemID As Integer, ByVal v_vCreditControlReason As Object, ByVal v_vAccountID As Integer, ByVal v_vDocumentID As Object, ByVal v_vDocumentDate As Object, ByVal v_vInsuranceFileCnt As Integer, ByVal v_vPFPremFinanceCnt As Object, ByVal v_vPFPremFinanceVersion As Object, ByVal v_vAmount As Object, ByVal v_vCanAutoCancel As Object, ByVal v_vWillAutoCancel As Object, ByVal v_vCreditControlStepID As Object, ByVal v_vCreatedDate As Object, ByVal v_vDueDate As Object, ByVal v_vLetterSent As Object, ByVal v_vRecurrenceCount As Object, ByVal v_bAction As Byte, ByVal v_vPMUserGroupId As Object, ByVal v_vPMUserId As Object, ByVal v_vClaimId As Object, ByVal v_vClaimDebtId As Object, ByVal v_vClaimDebtVersion As Object, ByVal v_vPartialAmount As Object, ByVal v_vIsDeleted As Object, ByVal v_vPFInstalmentsId As Object) As Integer


        Dim result As Integer = 0
        Dim lRecordsAffected As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add Credit Control Step id

            If v_bAction = ACUpdateAdd Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="credit_control_item_id", vValue:=CInt(r_vCreditControlItemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else

                m_lReturn = m_oDatabase.Parameters.Add(sName:="credit_control_item_id", vValue:=CInt(r_vCreditControlItemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add credit control reason as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="credit_control_reason", vValue:=v_vCreditControlReason, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add account id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=v_vAccountID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add document id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_id", vValue:=v_vDocumentID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add document date as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_date", vValue:=v_vDocumentDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add insurance file count as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_vInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add Premium Finance Count as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pfprem_finance_cnt", vValue:=v_vPFPremFinanceCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add premium finance version as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pfprem_finance_version", vValue:=v_vPFPremFinanceVersion, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add amount as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="amount", vValue:=v_vAmount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add can auto cancel as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="can_auto_cancel", vValue:=v_vCanAutoCancel, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add will auto cancel as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="will_auto_cancel", vValue:=v_vWillAutoCancel, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add credit control step id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="credit_control_step_id", vValue:=v_vCreditControlStepID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add created date as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="created_date", vValue:=v_vCreatedDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add Due date as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="due_date", vValue:=v_vDueDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add letter sent as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="letter_sent", vValue:=v_vLetterSent, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add recurrence count as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="recurrence_count", vValue:=v_vRecurrenceCount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=v_vPMUserGroupId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmuser_id", vValue:=v_vPMUserId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=v_vClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_debt_id", vValue:=v_vClaimDebtId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_debt_version", vValue:=v_vClaimDebtVersion, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="partial_amount", vValue:=v_vPartialAmount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_deleted", vValue:=v_vIsDeleted, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="PFInstalments_Id", vValue:=v_vPFInstalmentsId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            If v_bAction = ACUpdateAdd Then
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDirectAddSQL, sSQLName:=ACDirectAddName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)
            Else
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDirectEditSQL, sSQLName:=ACDirectEditName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If record was added, retrieve the ID
            If lRecordsAffected > 0 Then
                If v_bAction = ACUpdateAdd Then

                    r_vCreditControlItemID = m_oDatabase.Parameters.Item("credit_control_item_id").Value
                End If
            Else
                ' Nothing affected, so set to error
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function
    ' *****************************************************************
    ' Name: DirectEdit (Public)
    '
    ' Description: Edits a Credit Control Item record in the database
    '
    ' *****************************************************************
    Public Function DirectEdit(ByVal v_vCreditControlItemID As Integer, ByVal v_vCreditControlReason As Object, ByVal v_vAccountID As Integer, ByVal v_vDocumentID As Object, ByVal v_vDocumentDate As Object, ByVal v_vInsuranceFileCnt As Integer, ByVal v_vPFPremFinanceCnt As Object, ByVal v_vPFPremFinanceVersion As Object, ByVal v_vAmount As Object, ByVal v_vCanAutoCancel As Object, ByVal v_vWillAutoCancel As Object, ByVal v_vCreditControlStepID As Object, ByVal v_vCreatedDate As Object, ByVal v_vDueDate As Object, ByVal v_vLetterSent As Object, ByVal v_vRecurrenceCount As Object, ByVal v_vPMUserGroupId As Object, ByVal v_vPMUserId As Object, ByVal v_vClaimId As Object, ByVal v_vClaimDebtId As Object, ByVal v_vClaimDebtVersion As Object, ByVal v_vPartialAmount As Object, ByVal v_vIsDeleted As Object, ByVal v_vPFInstalmentsId As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call the update method
            m_lReturn = CType(DirectUpdate(r_vCreditControlItemID:=v_vCreditControlItemID, v_vCreditControlReason:=v_vCreditControlReason, v_vAccountID:=v_vAccountID, v_vDocumentID:=v_vDocumentID, v_vDocumentDate:=v_vDocumentDate, v_vInsuranceFileCnt:=v_vInsuranceFileCnt, v_vPFPremFinanceCnt:=v_vPFPremFinanceCnt, v_vPFPremFinanceVersion:=v_vPFPremFinanceVersion, v_vAmount:=v_vAmount, v_vCanAutoCancel:=v_vCanAutoCancel, v_vWillAutoCancel:=v_vWillAutoCancel, v_vCreditControlStepID:=v_vCreditControlStepID, v_vCreatedDate:=v_vCreatedDate, v_vDueDate:=v_vDueDate, v_vLetterSent:=v_vLetterSent, v_vRecurrenceCount:=v_vRecurrenceCount, v_bAction:=ACUpdateEdit, v_vPMUserGroupId:=v_vPMUserGroupId, v_vPMUserId:=v_vPMUserId, v_vClaimId:=v_vClaimId, v_vClaimDebtId:=v_vClaimDebtId, v_vClaimDebtVersion:=v_vClaimDebtVersion, v_vPartialAmount:=v_vPartialAmount, v_vIsDeleted:=v_vIsDeleted, v_vPFInstalmentsId:=v_vPFInstalmentsId), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception
            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectEdit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectEdit", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' DirectDelete-Deletes a Credit Control Item record
    ''' </summary>
    ''' <param name="v_lCreditControlItemId"></param>
    ''' <param name="v_bDeletePermanent"></param>
    ''' <param name="v_iLetterSent"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DirectDelete(ByVal v_lCreditControlItemId As Integer) As Integer
        Return DirectDelete(v_lCreditControlItemId:=v_lCreditControlItemId, v_bDeletePermanent:=False, v_iLetterSent:=0)
    End Function

    Public Function DirectDelete(ByVal v_lCreditControlItemId As Integer, ByVal v_bDeletePermanent As Boolean, Optional ByVal v_iLetterSent As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add Credit Control step id as an input param
            m_lReturn = m_oDatabase.Parameters.Add(sName:="nCredit_control_item_id", vValue:=v_lCreditControlItemId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add Credit Control step id as an input param
            m_lReturn = m_oDatabase.Parameters.Add(sName:="bDelete_Permanent", vValue:=v_bDeletePermanent, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Add Credit Control step id as an input param
            m_lReturn = m_oDatabase.Parameters.Add(
                sName:="nLetter_Sent",
                vValue:=v_iLetterSent,
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMInteger)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                DirectDelete = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDirectDeleteSQL, sSQLName:=ACDirectDeleteName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' *****************************************************************
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' *****************************************************************
    'UPGRADE_NOTE: (7001) The following declaration (BeginTrans) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function BeginTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLBeginTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' *****************************************************************
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' *****************************************************************
    'UPGRADE_NOTE: (7001) The following declaration (CommitTrans) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CommitTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLCommitTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' *****************************************************************
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' *****************************************************************
    'UPGRADE_NOTE: (7001) The following declaration (RollbackTrans) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function RollbackTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLRollbackTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ''' <summary>
    ''' This function is used to avoid multi code of line while initializing the Stored procedure .
    ''' </summary>
    ''' <param name="v_sName"></param>
    ''' <param name="v_vValue"></param>
    ''' <param name="v_iType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "AddInputParameter"

        Try

            ' Add Parameter to database object
            nResult = m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=v_vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, " Failed to add parameter name:" & v_sName &
                                        ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            nResult = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
        End Try
        Return nResult
    End Function

    ' ***************************************************************** '
    ' Name: UpdateCreditControlItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 17-05-2007 : Instalment Import Changes
    ' ***************************************************************** '
    Private Function UpdateCreditControlItem(ByVal v_lCreditControlItemId As Integer, ByVal v_sCreditControlReason As String, ByVal v_lCreditControlStepId As Integer, ByVal v_dtDueDate As Date) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "UpdateCreditControlItem"

        Try

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            nResult = CType(AddInputParameter(v_sName:="credit_control_item_id", v_vValue:=v_lCreditControlItemId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            nResult = CType(AddInputParameter(v_sName:="credit_control_reason", v_vValue:=(v_sCreditControlReason), v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            nResult = CType(AddInputParameter(v_sName:="credit_control_step_id", v_vValue:=v_lCreditControlStepId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            nResult = CType(AddInputParameter(v_sName:="due_date", v_vValue:=v_dtDueDate, v_iType:=gPMConstants.PMEDataType.PMDate), gPMConstants.PMEReturnCode)

            ' Execute Action Query
            nResult = m_oDatabase.SQLAction(sSQL:=kUpdateCreditControlItemSQL, sSQLName:=kUpdateCreditControlItemName, bStoredProcedure:=True)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kUpdateCreditControlItemSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch excep As Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            ' If you want to rollback a transaction or something, do it here
            Return nResult
        End Try

        Return nResult
    End Function


    ''' <summary>
    ''' Instalment Import Changes
    ''' </summary>
    ''' <param name="v_lInstalmentId"></param>
    ''' <param name="v_sDefaultCreditControlItemReason"></param>
    ''' <param name="v_lProcessMode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetupCreditControlItemForInstalment(ByVal v_lInstalmentId As Integer, ByVal v_sDefaultCreditControlItemReason As String) As Integer
        Return SetupCreditControlItemForInstalment(v_lInstalmentId:=v_lInstalmentId, v_sDefaultCreditControlItemReason:=v_sDefaultCreditControlItemReason, v_lProcessMode:=0)
    End Function

    Public Function SetupCreditControlItemForInstalment(ByVal v_lInstalmentId As Integer, ByVal v_sDefaultCreditControlItemReason As String, ByVal v_lProcessMode As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupCreditControlItemForInstalment"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim vCreditControlDetails As Object = Nothing
        Dim lCreditControlItemId, lAccountID, lPlanTransactionId, lInsuranceFileCnt, lCanAutoCancel, lCreditControlStepId, lDueDays, lPlanId, lPlanVersion As Integer
        Dim dtInstalmentDueDate, dtCreditControlDueDate As Date
        Dim sCreditControlReason As String = ""
        Dim crAmount As Decimal
        Dim sSourceDescription, sFrequencyDescription, sCreditControlBusinessType As String
        Dim lInstalmentFailureCount, lPolicyIsPaid As Integer
        Dim sInsuranceFileStatusDescription, sPfInstalmentResultDescription As String
        Dim nProcessingDays As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get details for specified instalment

            lReturn = CType(GetCreditControlDetailsForInstalment(v_lInstalmentId, vCreditControlDetails, v_lProcessMode), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetCreditControlDetailsForInstalment Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get the required details for both add and update

            nProcessingDays = gPMFunctions.ToSafeLong(CStr(vCreditControlDetails(25, 0)), 0)

            lCreditControlItemId = gPMFunctions.ToSafeLong(CStr(vCreditControlDetails(16, 0)), 0)

            lCreditControlStepId = gPMFunctions.ToSafeLong(CStr(vCreditControlDetails(4, 0)), 0)

            lDueDays = gPMFunctions.ToSafeLong(CStr(vCreditControlDetails(5, 0)), 0)

            'If Informations.IsDate(gPMFunctions.ToSafeDate(CStr(vCreditControlDetails(26, 0)))) Then

            '    dtInstalmentDueDate = gPMFunctions.ToSafeDate(CStr(vCreditControlDetails(26, 0)), DateTime.Now)
            'Else

            dtInstalmentDueDate = gPMFunctions.ToSafeDate(CStr(vCreditControlDetails(11, 0)), DateTime.Now)
            'End If
            dtCreditControlDueDate = DateTime.Now.AddDays(nProcessingDays)


            lInstalmentFailureCount = gPMFunctions.ToSafeLong(CStr(vCreditControlDetails(22, 0)), 0)

            ' if the instalment does have a pfinstalments_result_id
            ' use the the results description as the credit control reason

            If CStr(vCreditControlDetails(20, 0)) <> "" Then

                sCreditControlReason = CStr(vCreditControlDetails(20, 0))
            Else
                ' otherwise use the default
                sCreditControlReason = v_sDefaultCreditControlItemReason
            End If

            Dim msg As String = ""
            If lCreditControlStepId = 0 Then



                sFrequencyDescription = CStr(vCreditControlDetails(17, 0))

                sSourceDescription = CStr(vCreditControlDetails(18, 0))


                sCreditControlBusinessType = CStr(vCreditControlDetails(21, 0))

                lPolicyIsPaid = gPMFunctions.ToSafeLong(CStr(vCreditControlDetails(22, 0))) ' Sankar - PN 61484

                sInsuranceFileStatusDescription = CStr(vCreditControlDetails(23, 0))

                sPfInstalmentResultDescription = CStr(vCreditControlDetails(20, 0))

                ' cancellation
                If v_lProcessMode = 1 Then
                    msg = "Failed to create credit control item for pfinstalments_id " & v_lInstalmentId &
                          " as failed to locate a credit control step on a credit control rule" &
                          " with the following configuration - business_type:=" & sCreditControlBusinessType &
                          " source:= " & sSourceDescription &
                          " frequency:= " & sFrequencyDescription &
                          " policy_is_paid:= " & CStr(lPolicyIsPaid) &
                          " insurance_file_status:= " & sInsuranceFileStatusDescription
                    ' rejection
                ElseIf v_lProcessMode = 2 Then
                    msg = "Failed to create credit control item for pfinstalments_id " & v_lInstalmentId &
                          " as failed to locate a credit control step on a credit control rule" &
                          " with the following configuration - business_type:=" & sCreditControlBusinessType &
                          " source:= " & sSourceDescription &
                          " frequency:= " & sFrequencyDescription &
                          " instalment_failure_count:= " & CStr(lInstalmentFailureCount) &
                          " pfinstalment_result:= " & sPfInstalmentResultDescription
                Else
                    msg = "Failed to create credit control item for pfinstalments_id " & v_lInstalmentId &
                          " as failed to locate a credit control step on a credit control rule" &
                          " with the following configuration - business_type:=" & sCreditControlBusinessType &
                          " source:= " & sSourceDescription &
                          " frequency:= " & sFrequencyDescription
                End If

                'LogMessage m_sUsername, PMLogInfo, msg, ACApp, ACClass, kMethodName
                'SetupCreditControlItemForInstalment = PMFail
                Return result

            End If

            ' if a credit control item doesnt exist - create one
            If lCreditControlItemId = 0 Then

                ' get required credit control details for add

                lAccountID = gPMFunctions.ToSafeLong(CStr(vCreditControlDetails(0, 0)), 0)

                lPlanTransactionId = gPMFunctions.ToSafeLong(CStr(vCreditControlDetails(1, 0)), 0)

                lInsuranceFileCnt = gPMFunctions.ToSafeLong(CStr(vCreditControlDetails(2, 0)), 0)

                lCanAutoCancel = gPMFunctions.ToSafeLong(CStr(vCreditControlDetails(3, 0)), 0)

                lPlanId = gPMFunctions.ToSafeLong(CStr(vCreditControlDetails(7, 0)), 0)

                lPlanVersion = gPMFunctions.ToSafeLong(CStr(vCreditControlDetails(8, 0)), 0)

                crAmount = gPMFunctions.ToSafeCurrency(CStr(vCreditControlDetails(19, 0)))
                m_lReturn = DeleteCreditControlItem(nInsuranceFileCnt:=lInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("DeleteCreditControlItem", "v_lInsuranceFileCnt:=" & lInsuranceFileCnt,
                               gPMConstants.PMELogLevel.PMLogError)
                End If

                ' Add the credit control item record

                lReturn = CType(DirectAdd(r_vCreditControlItemID:=lCreditControlItemId, v_vCreditControlReason:=sCreditControlReason, v_vAccountID:=lAccountID, v_vDocumentID:=DBNull.Value, v_vDocumentDate:=DBNull.Value, v_vInsuranceFileCnt:=lInsuranceFileCnt, v_vPFPremFinanceCnt:=lPlanId, v_vPFPremFinanceVersion:=lPlanVersion, v_vAmount:=crAmount, v_vCanAutoCancel:=lCanAutoCancel, v_vWillAutoCancel:="0", v_vCreditControlStepID:=lCreditControlStepId, v_vCreatedDate:=DateTime.Now, v_vDueDate:=dtCreditControlDueDate, v_vLetterSent:="0", v_vRecurrenceCount:="0", v_vPMUserGroupId:=0, v_vPMUserId:=0, v_vClaimId:=0, v_vClaimDebtId:=0, v_vClaimDebtVersion:=0, v_vPartialAmount:=0, v_vIsDeleted:=0, v_vPFInstalmentsId:=v_lInstalmentId), gPMConstants.PMEReturnCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "DirectAdd Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            Else

                lReturn = CType(UpdateCreditControlItem(v_lCreditControlItemId:=lCreditControlItemId, v_sCreditControlReason:=sCreditControlReason, v_lCreditControlStepId:=lCreditControlStepId, v_dtDueDate:=dtCreditControlDueDate), gPMConstants.PMEReturnCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "UpdateCreditControlItem Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If


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
    ' Name: GetCreditControlDetailsForInstalment
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 17-05-2007 : Instalment Import
    ' ***************************************************************** '
    Public Function GetCreditControlDetailsForInstalment(ByVal v_lInstalmentId As Integer, ByRef r_vResults(,) As Object) As Integer
        Return GetCreditControlDetailsForInstalment(v_lInstalmentId:=v_lInstalmentId, r_vResults:=r_vResults, v_lProcessMode:=0)
    End Function

    Public Function GetCreditControlDetailsForInstalment(ByVal v_lInstalmentId As Integer, ByRef r_vResults(,) As Object, ByVal v_lProcessMode As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCreditControlDetailsForInstalment"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Get details relating to plan:
            AddInputParameter("pfinstalments_id", v_lInstalmentId, gPMConstants.PMEDataType.PMLong)
            AddInputParameter("processMode", v_lProcessMode, gPMConstants.PMEDataType.PMLong)

            ' Execute SQL Statement
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetCreditControlDetailsForInstalmentSQL, sSQLName:=kGetCreditControlDetailsForInstalmentName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResults)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetCreditControlDetailsForInstalmentSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result

            '		Resume 
            '		Return result
        End Try
        Return result
    End Function
    ''' <summary>
    ''' DeleteCreditControlItem
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DeleteCreditControlItem(ByVal nInsuranceFileCnt As Long) As Long

        Const kMethodName As String = "DeleteCreditControlItem"
        Dim nReturn As Long = 0

        Try

            DeleteCreditControlItem = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="Insurance_File_Cnt", v_vValue:=nInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = AddInputParameter(v_sName:="nDeleteNonInstalment", v_vValue:=1, v_iType:=gPMConstants.PMEDataType.PMInteger)

            ' Execute Action Query
            If m_oDatabase.SQLAction(sSQL:=kDeleteCreditControlItemSQL, sSQLName:=kDeleteCreditControlItemName, bStoredProcedure:=kDeleteCreditControlItemSQLStored) <> gPMConstants.PMEReturnCode.PMTrue Then

                RaiseError(kMethodName, " Execute Action Query Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch excep As Exception

            ' DO Not Call any functions before here or the error will be lost

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOriginalInsuranceFileCnt " & "Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return gPMConstants.PMEReturnCode.PMError
        End Try
    End Function

End Class
