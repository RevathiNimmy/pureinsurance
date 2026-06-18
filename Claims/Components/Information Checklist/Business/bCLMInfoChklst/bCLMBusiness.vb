Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient

'Developer Guide No: 129
Imports SharedFiles


<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    ' Date: 06/10/1998
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a CLMInfoChklst.
    ' Edit History:
    ' SJP14062002 - getUnderWritingOrAgency uses new product options scheme
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' Collection of CLMInfoChklsts (Private)
    Private m_oCLMInfoChklsts As bCLMInfoChklst.CLMInfoChklsts

    ' ************************************************
    ' Added to replace global variables 15/12/2003
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database
    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean
    ' Current Record Pointer
    Private m_lCurrentRecord As Integer
    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode
    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    ' Primary Keys to work with
    Private m_lClmExpServId As Integer
    'AM(11/12/2000) Claim Handler
    Private m_sUnderwritingOrAgency As String = ""
    Private m_oSystemOption As bSIROptions.Business
    Private m_lClaimId As Integer
    ' JMK 25/05/2001 InfoOnly
    Private m_iInfoOnly As Integer
    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business

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
                Case Is > m_oCLMInfoChklsts.Count()
                    m_lCurrentRecord = m_oCLMInfoChklsts.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property
    Public ReadOnly Property RecordCount() As Integer
        Get
            ' Return Number in Collection
            Return m_oCLMInfoChklsts.Count()
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
    Public Property ClmExpServId() As Integer
        Get
            Return m_lClmExpServId
        End Get
        Set(ByVal Value As Integer)
            m_lClmExpServId = Value
        End Set
    End Property
    Public Property ClaimId() As Integer
        Get
            Return m_lClaimId
        End Get
        Set(ByVal Value As Integer)
            m_lClaimId = Value
        End Set
    End Property
    'AM(11/12/2000) Claims Numbering.
    Public ReadOnly Property UnderwritingOrAgency() As String
        Get
            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If
            Return m_sUnderwritingOrAgency
        End Get
    End Property
    Public Property InfoOnly() As Integer
        Get
            Return m_iInfoOnly
        End Get
        Set(ByVal Value As Integer)
            m_iInfoOnly = Value
        End Set
    End Property

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

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



            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create CLMInfoChklsts Collection
            m_oCLMInfoChklsts = New bCLMInfoChklst.CLMInfoChklsts()

            ' Create PM Lookup Business Object
            m_oLookup = New bPMLookup.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()
                    m_oLookup = Nothing
                End If
                m_oCLMInfoChklsts = Nothing
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


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required CLMInfoChklsts and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(ByVal v_lCurrentRecord As Integer, Optional ByRef vClmExpServId As Object = Nothing, Optional ByRef vClaim_Id As Object = Nothing, Optional ByRef vExpServId As Object = Nothing, Optional ByRef vPrtyClmId As Object = Nothing, Optional ByRef vServTypeId As Object = Nothing, Optional ByRef vService As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vReference As Object = Nothing, Optional ByRef vContact As Object = Nothing, Optional ByRef vDateReq As Object = Nothing, Optional ByRef vDateCrit As Object = Nothing, Optional ByRef vDateRecv As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oCLMInfoChklst As bCLMInfoChklst.CLMInfoChklst
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oCLMInfoChklst = m_oCLMInfoChklsts.Item(v_lCurrentRecord)

            ' Get the CLMInfoChklst Property Values

            'developer guide no.98
            m_lReturn = oCLMInfoChklst.GetProperties(iStatus, vClmExpServId:=vClmExpServId, vClaim_Id:=vClaim_Id, vExpServId:=vExpServId, vPrtyClmId:=vPrtyClmId, vServTypeId:=vServTypeId, vService:=vService, vDescription:=vDescription, vReference:=vReference, vContact:=vContact, vDateReq:=vDateReq, vDateCrit:=vDateCrit, vDateRecv:=vDateRecv)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oCLMInfoChklst = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied CLMInfoChklst into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' JMK 25/05/2001 additional optional parameters:- vInfoStatus; vUnderwritingOrAgency
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, ByRef iStatus As Integer, Optional ByRef vClmExpServId As Object = Nothing, Optional ByRef vClaim_Id As Object = Nothing, Optional ByRef vExpServId As Object = Nothing, Optional ByRef vPrtyClmId As Object = Nothing, Optional ByRef vServTypeId As Object = Nothing, Optional ByRef vService As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vReference As Object = Nothing, Optional ByRef vContact As Object = Nothing, Optional ByRef vDateReq As Object = Nothing, Optional ByRef vDateCrit As Object = Nothing, Optional ByRef vDateRecv As Object = Nothing, Optional ByRef vInfoStatus As Object = Nothing, Optional ByRef vUnderwritingOrAgency As Object = Nothing) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim oCLMInfoChklst As bCLMInfoChklst.CLMInfoChklst

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oCLMInfoChklsts.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new CLMInfoChklst
            oCLMInfoChklst = New bCLMInfoChklst.CLMInfoChklst()
            m_lReturn = CType(oCLMInfoChklst.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            'developer guide no.98
            m_lReturn = oCLMInfoChklst.SetProperties(iStatus:=iStatus, vClmExpServId:=vClmExpServId, vClaim_Id:=vClaim_Id, vExpServId:=vExpServId, vPrtyClmId:=vPrtyClmId, vServTypeId:=vServTypeId, vService:=vService, vDescription:=vDescription, vReference:=vReference, vContact:=vContact, vDateReq:=vDateReq, vDateCrit:=vDateCrit, vDateRecv:=vDateRecv, vInfoStatus:=vInfoStatus, vUnderwritingOrAgency:=vUnderwritingOrAgency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCLMInfoChklst = Nothing
                Return result
            End If

            ' Add CLMInfoChklst to collection
            m_lReturn = CType(m_oCLMInfoChklsts.Add(oNewCLMInfoChklst:=oCLMInfoChklst), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oCLMInfoChklst = Nothing
                Return result
            End If

            oCLMInfoChklst = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the CLMInfoChklst
    '              specified and updates the CLMInfoChklst with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vClmExpServId As Object = Nothing, Optional ByRef vClaim_Id As Object = Nothing, Optional ByRef vExpServId As Object = Nothing, Optional ByRef vPrtyClmId As Object = Nothing, Optional ByRef vServTypeId As Object = Nothing, Optional ByRef vService As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vReference As Object = Nothing, Optional ByRef vContact As Object = Nothing, Optional ByRef vDateReq As Object = Nothing, Optional ByRef vDateCrit As Object = Nothing, Optional ByRef vDateRecv As Object = Nothing, Optional ByRef vInfoStatus As Object = Nothing, Optional ByRef vUnderwritingOrAgency As Object = Nothing) As gPMConstants.PMEReturnCode


        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim oCLMInfoChklst As bCLMInfoChklst.CLMInfoChklst
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oCLMInfoChklsts.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oCLMInfoChklst = m_oCLMInfoChklsts.Item(lRow)

            ' Check the Status of the CLMInfoChklst

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oCLMInfoChklst.DatabaseStatus
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

            ' Update CLMInfoChklst Attributes

            'developer guide no.98
            m_lReturn = oCLMInfoChklst.SetProperties(iStatus:=iStatus, vClmExpServId:=vClmExpServId, vClaim_Id:=vClaim_Id, vExpServId:=vExpServId, vPrtyClmId:=vPrtyClmId, vServTypeId:=vServTypeId, vService:=vService, vDescription:=vDescription, vReference:=vReference, vContact:=vContact, vDateReq:=vDateReq, vDateCrit:=vDateCrit, vDateRecv:=vDateRecv, vInfoStatus:=vInfoStatus, vUnderwritingOrAgency:=vUnderwritingOrAgency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCLMInfoChklst = Nothing
                Return result
            End If

            ' Release reference to CLMInfoChklst
            oCLMInfoChklst = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified CLMInfoChklst can be deleted
    '              and mark accordingly.
    '
    ' JMK 25/05/2001 additional optional parameters:- vInfoStatus; vUnderwritingOrAgency
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer, Optional ByRef vInfoStatus As Integer = 0, Optional ByRef vUnderwritingOrAgency As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oCLMInfoChklst As bCLMInfoChklst.CLMInfoChklst

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oCLMInfoChklsts.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oCLMInfoChklst = m_oCLMInfoChklsts.Item(lRow)

            ' Check the Status of the CLMInfoChklst

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oCLMInfoChklst.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oCLMInfoChklst.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                'JMK 29/05/2001 treat Info Only as "Add"
                If oCLMInfoChklst.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit And vInfoStatus Then
                    oCLMInfoChklst.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
                Else
                    oCLMInfoChklst.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
                End If
            End If

            ' Release reference to CLMInfoChklst
            oCLMInfoChklst = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            For lSub As Integer = 1 To m_oCLMInfoChklsts.Count()
                Select Case m_oCLMInfoChklsts.Item(lSub).DatabaseStatus
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim oCLMInfoChklst As bCLMInfoChklst.CLMInfoChklst
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oCLMInfoChklsts.Count()
                oCLMInfoChklst = m_oCLMInfoChklsts.Item(lSub)


                Select Case oCLMInfoChklst.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item
                        m_lReturn = CType(oCLMInfoChklst.AddItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Update Item
                        m_lReturn = CType(oCLMInfoChklst.UpdateItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMDelete

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Delete Item
                        m_lReturn = CType(oCLMInfoChklst.DeleteItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Retain the Primary Key of the CLMInfoChklst
            With oCLMInfoChklst
                ClmExpServId = .ClmExpServId
            End With

            ' Release last reference
            oCLMInfoChklst = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Refresh the Collection Items D/B Status
                    lSub = 1

                    ' For each item in the collection
                    Do While lSub <= m_oCLMInfoChklsts.Count()

                        ' With the item
                        With m_oCLMInfoChklsts.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oCLMInfoChklsts.Delete(lSub)

                                    ' Anything Else
                                Case Else
                                    ' Set Status to view
                                    .DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                                    lSub += 1

                            End Select

                        End With

                    Loop

                Else

                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DeleteClaim
    '
    ' Description:
    '
    ' History: 08/05/2001 Tomo - Created.
    '
    '           25/05/2001    Jude Killip     updated
    ' ***************************************************************** '
    Public Function DeleteClaim() As Integer
        Dim result As Integer = 0
        Dim lStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(m_lClaimId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClaim")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="status", vValue:=CStr(lStatus), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClaim")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLBeginTrans failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClaim")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteClaimSQL, sSQLName:=ACDeleteClaimName, bStoredProcedure:=ACDeleteClaimStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLAction failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClaim")

                m_lReturn = m_oDatabase.SQLRollbackTrans()

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lStatus = m_oDatabase.Parameters.Item("status").Value

            If lStatus <> 0 Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Status returned Error", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClaim")

                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLCommitTrans failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClaim")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClaim", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Sub New()
        MyBase.New()


        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a OpenClaim.
    '
    ' ***************************************************************** '
    Public Function GetLookupValues(ByVal iLookupType As Integer, ByRef vTableArray(,) As Object, ByVal iLanguageID As Integer, ByRef vResultarray(,) As Object) As Integer


        Dim result As Integer = 0
        Dim dtEffectiveDate As Date
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Default Effective Date to current date/time
            dtEffectiveDate = DateTime.Now

            ' Get the Lookup items

            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTableArray, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultarray:=vResultarray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name:         GetCoinsuranceRecoveries (Public)
    ' Description:  Gets the CoinsuranceRecoveries for the given Claim Id
    ' Returns    :  r_vResultArray: (2-dimensional Array) containing
    '
    ' Date:         20/06/2000
    ' Author:       SK
    ' ***************************************************************** '
    Public Function GetCoinsuranceRecoveries(ByRef r_vResultArray(,) As Object, ByVal v_lClaim_Id As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Claim Id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClmId", vValue:=CStr(v_lClaim_Id), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCoinsuranceRecoveries")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCoInsurerDetailsSQL, sSQLName:=ACGetCoInsurerDetailsName, bStoredProcedure:=ACGetCoInsurerDetailsStored, vResultarray:=r_vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCoinsuranceRecoveries")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            ' If NO record was found return Not Found
            If Not Information.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCoinsuranceRecoveries Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCoinsuranceRecoveries", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         GetReinsuranceRecoveries (Public)
    ' Description:  Gets the ReinsuranceRecoveries for the given Claim Id
    ' Returns    :  r_vResultArray: (2-dimensional Array) containing
    '
    ' Date:         20/06/2000
    ' Author:       SK
    ' ***************************************************************** '
    Public Function GetReinsuranceRecoveries(ByRef r_vResultArray(,) As Object, ByVal v_lClaim_Id As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Claim Id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClmId", vValue:=CStr(v_lClaim_Id), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReinsuranceRecoveries")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetReInsurerDetailsSQL, sSQLName:=ACGetReInsurerDetailsName, bStoredProcedure:=ACGetReInsurerDetailsStored, vResultarray:=r_vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReinsuranceRecoveries")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            ' If NO record was found return Not Found
            If Not Information.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetReinsuranceRecoveries Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReinsuranceRecoveries", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         GetPerilDetails (Public)
    ' Description:  Gets the GetPerilDetails for the given Claim Id
    ' Returns    :  r_vResultArray: (2-dimensional Array) containing
    '
    ' Date:         20/06/2000
    ' Author:       SK
    ' ***************************************************************** '
    Public Function GetPerilDetails(ByRef r_vResultArray(,) As Object, ByVal v_lClaim_Id As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Claim Id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(v_lClaim_Id), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPerilDetails")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPerilDetailsSQL, sSQLName:=ACGetPerilDetailsName, bStoredProcedure:=ACGetPerilDetailsStored, vResultarray:=r_vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPerilDetails")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            ' If NO record was found return Not Found
            If Not Information.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPerilDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPerilDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name:         GetDefaultCurrencyID (Public)
    ' Description:  Gets the the Default CurrencyID for the given Claim Id
    ' Returns    :  r_vResultArray: (2-dimensional Array) containing
    '
    ' Date:         20/06/2000
    ' Author:       SK
    ' ***************************************************************** '
    Public Function GetDefaultCurrencyID(ByRef r_vResultArray(,) As Object, ByVal v_lClaim_Id As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Claim Id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClmId", vValue:=CStr(v_lClaim_Id), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultCurrencyID")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDefaultCurrencyIDSQL, sSQLName:=ACGetDefaultCurrencyIDName, bStoredProcedure:=ACGetDefaultCurrencyIDStored, vResultarray:=r_vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultCurrencyID")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            ' If NO record was found return Not Found
            If Not Information.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaultCurrencyID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultCurrencyID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         GetExpServsAdd (Public)
    ' Description:  Gets the Exp Servs in the ADD mode for the given
    '               Risk Type Id
    ' Returns    :  r_vResultArray: (2-dimensional Array) containing
    '               0-Service_Type_Id
    '               1-Description
    ' Parameters    : ByRef r_vResultArray
    '                 ByVal v_lRisk_Type_Id
    ' Date:         20/06/2000
    ' Author:       SK
    '
    ' History:      JMK 25/05/2001 change id params to Long
    ' ***************************************************************** '
    Public Function GetExpServsAdd(ByRef r_vResultArray(,) As Object, ByVal v_lRisk_Type_Id As Integer) As Integer

        Dim result As Integer = 0
        Dim lRealRiskTypeId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'RWH(06/03/2001) Claims stores risk_cnt in Claim.risk_type_id (great !!)
            'and gets itself confused here. So we need to go get the proper risk_type_id.
            'Don't know at present if Broking need this fix. DC to investigate.

            m_lReturn = CType(GetRiskTypeId(v_lRiskCnt:=v_lRisk_Type_Id, r_lRiskTypeId:=lRealRiskTypeId), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            v_lRisk_Type_Id = lRealRiskTypeId


            'Add the Claim Id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_type_id", vValue:=CStr(v_lRisk_Type_Id), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExpServsAdd")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetExpServsAddSQL, sSQLName:=ACGetExpServsAddName, bStoredProcedure:=ACGetExpServsAddStored, vResultarray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExpServsAdd")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' If NO record was found return Not Found
            If Not Information.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetExpServsAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExpServsAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name:         GetExpServsEdit (Public)
    ' Description:  Gets the Exp Servs in the ADD mode for the given
    '               Risk Type Id
    ' Returns    :  r_vResultArray: (2-dimensional Array) containing
    ' Parameters    : ByRef r_vResultArray
    '                 ByVal v_lRisk_Type_Id
    ' Date:         20/06/2000
    ' Author:       SK
    '
    ' History:      change id params to Long
    ' ***************************************************************** '
    Public Function GetExpServsEdit(ByRef r_vResultArray(,) As Object, ByVal v_lClaim_Id As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Claim Id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(v_lClaim_Id), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExpServsEdit")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetExpServsEditSQL, sSQLName:=ACGetExpServsEditName, bStoredProcedure:=ACGetExpServsEditStored, vResultarray:=r_vResultArray)

            ' If NO record was found return Not Found
            If Not Information.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            Else
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExpServsEdit")

                    Return gPMConstants.PMEReturnCode.PMFalse

                End If
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetExpServsEdit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExpServsEdit", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         CollCount (Public)
    ' Description:  Get the current count of the items in the collection
    ' Author:       SK
    ' Date:         06/07/2000
    ' ***************************************************************** '
    Public Function CollCount() As Integer
        Return m_oCLMInfoChklsts.Count()
    End Function

    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' Created By:  'RWH(09/11/2000) For Claims Numbering.
    '
    ' SJP14062002 - getUnderWritingOrAgency uses new product options scheme
    ' ***************************************************************** '
    Public Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0
        Try


            Return bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingOrAgency)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnderwritingOrAgencyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnderwritingOrAgency", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetInfoOnlyStatus
    '
    ' Description:  Find out if Claim was previously Info Only
    '
    ' Created By:  Jude Killip 25/05/2001
    '
    ' ***************************************************************** '
    Public Function GetInfoOnlyStatus(ByVal v_lClaim_Id As Integer, ByRef r_bInfoStatus As Boolean) As Integer

        Dim result As Integer = 0
        Dim l_vResultArray As Object
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Claim Id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(v_lClaim_Id), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInfoOnlyStatus")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetInfoOnlyStatusSQL, sSQLName:=ACGetInfoOnlyStatusName, bStoredProcedure:=ACGetInfoOnlyStatusStored, vResultarray:=l_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInfoOnlyStatus")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'either PMTrue or PMNotFound
            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                r_bInfoStatus = False
            Else

                r_bInfoStatus = CBool(l_vResultArray(0, 0))
            End If

            Return m_lReturn

        Catch excep As System.Exception




            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInfoOnlyStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientAndPolicyID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CreateClaimHandlerTasks
    '
    ' Description:  gets information for time allowed for external claim handler
    '               tasks and supply
    '
    ' Created By:  AM 11/12/00 - for claim handler info
    '
    ' ***************************************************************** '
    Public Function CreateClaimHandlerTasks(ByVal v_sClaimNumber As String) As Integer

        Dim result As Integer = 0
        Dim dtTaskDate, dtSupplyDate As Date
        Dim sClientName, sClaimNumber, sTaskErrMes As String
        Dim vResultarray(,) As Object

        Try

            sTaskErrMes = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetClaimInfoFromClaimRef(v_sClaimNumber, vResultarray), gPMConstants.PMEReturnCode)

            If Information.IsArray(vResultarray) Then

                m_lClaimId = CInt(vResultarray(0, 0))
            End If

            m_lReturn = CType(GetProductDetails(v_lClaimID:=m_lClaimId, r_vExtClmHandlerAcknowledgedTaskAllowedTime:=dtTaskDate, r_vExtClmHandlerSupplyPreReportTaskAllowedTime:=dtSupplyDate), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sClaimNumber = v_sClaimNumber

            'To get this far we must have details for both task and supply dates
            'Get the client name from the claim number
            m_lReturn = CType(GetClientNameForTask(sClaimNumber, sClientName), gPMConstants.PMEReturnCode)

            ' Check for error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add task for external claim handler aknowledged task allowed time
            m_lReturn = CType(AddTaskToWorkManager(v_sClientName:=sClientName, v_sDescription:="Claim No. " & sClaimNumber & _
                        " - External Claims Handler Acknowledged Task", v_dtDueDate:=dtTaskDate), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'Add task for external claim handler preliminary report task allowed time
            m_lReturn = CType(AddTaskToWorkManager(v_sClientName:=sClientName, v_sDescription:="Claim No. " & sClaimNumber & _
                        " - External Claims Handler Preliminary Report Task", v_dtDueDate:=dtSupplyDate), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateClaimHandlerTasks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateClaimHandlerTasks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name : AddTaskToWorkManager
    '
    ' Desc : Add task to work manager
    '
    ' ***************************************************************** '
    Public Function AddTaskToWorkManager(ByVal v_sClientName As String, ByVal v_sDescription As String, ByVal v_dtDueDate As Date) As Integer

        Dim result As Integer = 0
        Dim bCloseDatabase As Boolean
        Dim oDatabase As Object
        Dim oWrkTaskInstance As bPMWrkTaskInstance.TaskControl
        Dim sSQL As String = ""
        Dim vResultarray As Object
        Dim lPMWrkTaskID, lPMWrkTaskGroupID, lPMUserGroupID As Integer
        Dim sCustomer As String = ""
        Dim lPMWrkTaskInstanceCnt As Integer

        Dim vTaskGroupID, vUserGroupID As Object

        Const PMTaskMemo As String = "MEMO"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'open architecture database

            If gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_bNewInstanceCreated:=bCloseDatabase, r_oCheckedDatabase:=oDatabase, v_vDatabase:=oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            oWrkTaskInstance = New bPMWrkTaskInstance.TaskControl
            If oWrkTaskInstance.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'get claim task group and claim user group from system option


            'developer guide no.98
            m_lReturn = GetProductDetails(v_lClaimID:=m_lClaimId, r_vClaimTaskGroup:=vTaskGroupID, r_vClaimUserGroup:=vUserGroupID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            lPMWrkTaskGroupID = CInt(vTaskGroupID)

            lPMUserGroupID = CInt(vUserGroupID)

            If lPMWrkTaskGroupID = 0 Or lPMUserGroupID = 0 Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If
            ' Get the task_id
            sSQL = "SELECT pmwrk_task_id FROM PMWrk_Task WHERE code = {task_code}"


            If oDatabase.Parameters.Add(sName:="task_code", vValue:=PMTaskMemo, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetTaskID", bStoredProcedure:=False, vResultarray:=vResultarray) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vResultarray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the task_id

            lPMWrkTaskID = CInt(vResultarray(0, 0))

            'create task

            If oWrkTaskInstance.CreateNew(v_lPMWrkTaskGroupID:=lPMWrkTaskGroupID, v_lPMWrkTaskID:=lPMWrkTaskID, v_sCustomer:=v_sClientName, v_dtTaskDueDate:=v_dtDueDate, v_lPMUserGroupID:=lPMUserGroupID, v_sDescription:=v_sDescription, v_iTaskStatus:=gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew, v_iIsUrgent:=0, r_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt, v_iUserID:=m_iUserID) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'close database
            If bCloseDatabase = gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = oDatabase.CloseDatabase()
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskToWorkManager Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetProductDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    ' ***************************************************************** '
    'developer guide no.98
    Private Function GetProductDetails(ByVal v_lClaimID As Integer, Optional ByRef r_vClaimTaskGroup As Object = Nothing, Optional ByRef r_vClaimUserGroup As Object = Nothing, Optional ByRef r_vExtClmHandlerAcknowledgedTaskAllowedTime As Object = Nothing, Optional ByRef r_vExtClmHandlerSupplyPreReportTaskAllowedTime As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetProductDetails"

        Const kIExtClmHandlerAcknowledgedTaskAllowedTime As Integer = 130
        Const kIExtClmHandlerSupplyPreReportTaskAllowedTime As Integer = 131
        Const kIClaimTaskGroup As Integer = 134
        Const kIClaimUserGroup As Integer = 135

        Dim lReturn As Integer
        Dim oBusiness As bSIRProduct.Business
        Dim bCloseDatabase As Boolean
        Dim oDatabase As Object
        Dim lProductID As Integer
        Dim iClaimTaskGroup, iClaimUserGroup As Integer
        Dim dExt_Clm_Handler_Acknowledged_Task_Allowed_Time, dExt_Clm_Handler_Supply_Pre_Report_Task_Allowed_Time As Double


        result = gPMConstants.PMEReturnCode.PMTrue

        'open architecture database

        If gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_bNewInstanceCreated:=bCloseDatabase, r_oCheckedDatabase:=oDatabase, v_vDatabase:=oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

            gPMFunctions.RaiseError(kMethodName, "CheckDatabase Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        oBusiness = New bSIRProduct.Business
        If oBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

            gPMFunctions.RaiseError(kMethodName, "Failed to get an instance of bSIRProduct.Business", gPMConstants.PMELogLevel.PMLogError)
        End If

        'get product details
        '    m_lReturn& = oBusiness.GetProductDetailsForClaim(v_lClaimID, vResultArray)

        m_lReturn = oBusiness.GetProductLevelOptionsForClaim(v_lClaimID:=m_lClaimId, r_iClaim_Task_Group:=iClaimTaskGroup, r_iClaim_User_Group:=iClaimUserGroup, r_dExt_Clm_Handler_Acknowledged_Task_Allowed_Time:=dExt_Clm_Handler_Acknowledged_Task_Allowed_Time, r_dExt_Clm_Handler_Supply_Pre_Report_Task_Allowed_Time:=dExt_Clm_Handler_Supply_Pre_Report_Task_Allowed_Time)


        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetProductDetails Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        'Developer Guide no. 118
        'start
        r_vClaimTaskGroup = iClaimTaskGroup

        r_vClaimUserGroup = iClaimUserGroup

        r_vExtClmHandlerAcknowledgedTaskAllowedTime = DateTime.Now.AddDays(dExt_Clm_Handler_Acknowledged_Task_Allowed_Time)

        r_vExtClmHandlerSupplyPreReportTaskAllowedTime = DateTime.Now.AddDays(dExt_Clm_Handler_Supply_Pre_Report_Task_Allowed_Time)

        'end

        oBusiness.Dispose()


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetOption (Private)
    '
    ' Description: Get an option.
    '
    ' ***************************************************************** '

    'Private Function GetOption(ByRef r_iOptionNumber As Integer, ByRef v_sOptionValue As String) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'If m_oSystemOption Is Nothing Then
    'm_oSystemOption = New bSIROptions.Business()
    '
    'm_lReturn = m_oSystemOption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the system option object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'Return result
    'End If
    'End If
    '
    'm_lReturn = m_oSystemOption.GetOption(iOptionNumber:=r_iOptionNumber, sValue:=v_sOptionValue)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the event", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'Return result
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    Private Function GetClientNameForTask(ByVal v_sClaimNumber As String, ByRef r_sClientName As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultarray(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        'Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        'Add the Claim Number parameter (INPUT)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_Number", vValue:=v_sClaimNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientNameForTask")

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClientNameSQL, sSQLName:=ACGetClientName, bStoredProcedure:=ACGetClientNameStored, vResultArray:=vResultarray)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientNameForTask")

            Return gPMConstants.PMEReturnCode.PMFalse

        End If

        If Not Information.IsArray(vResultarray) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the client short name

        r_sClientName = CStr(vResultarray(0, 0))

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: GetRiskTypeId
    '
    ' Description: Created to retrieve ACTUAL risk_type_id as the one passed
    '               thru' the claims system is really risk_cnt!!!!!!!!!!
    '
    ' History: 06/03/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetRiskTypeId(ByVal v_lRiskCnt As Integer, ByRef r_lRiskTypeId As Integer) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultarray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT risk_type_id" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "FROM Risk" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "WHERE risk_cnt = " & CStr(v_lRiskCnt)

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetRiskTypeId", bStoredProcedure:=False, vResultArray:=vResultarray)

            If Not (Information.IsArray(vResultarray)) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            r_lRiskTypeId = CInt(vResultarray(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskTypeId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskTypeId", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetCleintAndPolicyID
    '
    ' Description:
    '
    ' History: 06/03/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetClientAndPolicyID(ByVal v_lClaimID As Object, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Claim Number parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(v_lClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientAndPolicyID")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClientAndPolicyIDSQL, sSQLName:=ACGetClientAndPolicyID, bStoredProcedure:=ACGetClientAndPolicyIDStored, vResultArray:=r_vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientAndPolicyID")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            If Not Information.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClientAndPolicyID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientAndPolicyID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '*************************************************************************
    ' Name : GetSystemOption
    '
    ' Desc : get system option number
    '
    ' Hist : 01/06/2001 Tinny - Created
    '*************************************************************************
    Public Function GetSystemOption(ByVal v_lOptionNumber As Integer, ByRef r_vResult As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultarray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT value FROM system_options WHERE option_number like " & v_lOptionNumber

            result = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="get system option", bStoredProcedure:=False, vResultArray:=vResultarray, bKeepNulls:=True)


            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Information.IsArray(vResultarray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If



            r_vResult = vResultarray(0, 0)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemOption", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '************************************************************
    ' Name : GetInfoOnlyFlag
    '
    ' Desc : get info only flag from  table
    '
    ' Hist : 04 June 2001 Tinny - Created
    '************************************************************
    Public Function GetInfoOnlyFlag(ByVal v_lClaimID As Integer, ByRef r_bStatus As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResultarray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            r_bStatus = False

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_lClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACGetInfoOnlyFlagSQL, sSQLName:=ACGetInfoOnlyFlagName, bStoredProcedure:=ACGetInfoOnlyFlagStored, vResultArray:=vResultarray, bKeepNulls:=True)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Information.IsArray(vResultarray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Dim auxVar As Object = vResultarray(0, 0)


            If Not (Convert.IsDBNull(auxVar) Or IsNothing(auxVar)) Then

                r_bStatus = CBool(vResultarray(0, 0))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInfoOnlyFlag Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInfoOnlyFlag", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    '***********************************************************************
    ' Name : GetOriginalClaimID
    '
    ' Desc : get the original claim ID from  table
    '
    ' Hist : 15 June 2001 Tinny - Created
    '***********************************************************************
    Public Function GetOriginalClaimID(ByVal v_lClaimID As Integer, ByRef r_lOriginalClaimID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultarray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_lClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACGetOriginalClaimIDSQL, sSQLName:=ACGetOriginalClaimIDName, bStoredProcedure:=ACGetOriginalClaimIDStored, vResultArray:=vResultarray)


            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Information.IsArray(vResultarray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_lOriginalClaimID = CInt(vResultarray(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOriginalClaimID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOriginalClaimID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function ShowInfoCheckList(ByVal v_lRiskTypeId As Integer, ByRef r_lShowInfo As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultarray(,) As Object
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            With m_oDatabase

                .Parameters.Clear()

                lReturn = .Parameters.Add(sName:="v_lRiskTypeId", vValue:=CStr(v_lRiskTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                lReturn = .SQLSelect(sSQL:=ACAutoShowInfoChkLstSQL, sSQLName:=ACAutoShowInfoChkLstName, bStoredProcedure:=ACAutoShowInfoChkLstStored, vResultArray:=vResultarray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not (Information.IsArray(vResultarray)) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                r_lShowInfo = CInt(vResultarray(0, 0))

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowInfoCheckList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskTypeId", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: CreateEvent
    ' Desc: Adds an entry into the Event Log
    '
    ' Hist: 10/01/2006 - A.Robinson : Function Created.

    ' ***************************************************************** '
    Public Function CreateEvent(ByVal v_lEventTypeId As Integer, ByVal v_sDescription As String, ByVal v_ClaimId As Integer) As Integer

        Dim result As Integer = 0
        Const AC_PROCEDURE_NAME As String = "CreateEvent"

        Dim oEvent As bSIREvent.Business
        Dim sSQL As String = ""
        Dim lInsuranceFileCnt, lInsuranceFolderCnt, lPartyCnt As Integer
        Dim vResults(,) As Object

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT C.policy_id FROM claim C WHERE C.claim_id = " & v_ClaimId

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Policy Id for Claim", bStoredProcedure:=False, vResultArray:=vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(AC_PROCEDURE_NAME, "Failed to get Policy Id for Claim", gPMConstants.PMELogLevel.PMLogError)
            End If

            lInsuranceFileCnt = gPMFunctions.ToSafeLong(vResults(0, 0))

            m_lReturn = CType(GetClientPolicyDetails(v_lInsuranceFileCnt:=lInsuranceFileCnt, r_lPartyCnt:=lPartyCnt, r_lInsuranceFolderCnt:=lInsuranceFolderCnt), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(AC_PROCEDURE_NAME, "Failed to get Client details for Policy", gPMConstants.PMELogLevel.PMLogError)
            End If
            oEvent = New bSIREvent.Business
            m_lReturn = CType(oEvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(AC_PROCEDURE_NAME, "Failed to create business object bSIREvent.Business", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = oEvent.DirectAdd(vPartyCnt:=lPartyCnt, vInsuranceFolderCnt:=lInsuranceFolderCnt, vInsuranceFileCnt:=lInsuranceFileCnt, vClaimCnt:=v_ClaimId, vEventType:=v_lEventTypeId, vUserId:=m_iUserID, vEventDate:=DateTime.Today, vDescription:=v_sDescription)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(AC_PROCEDURE_NAME, "Failed to add event to Event Log", gPMConstants.PMELogLevel.PMLogError)
            End If


            Return result

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=AC_PROCEDURE_NAME & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=AC_PROCEDURE_NAME, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

            If Not (oEvent Is Nothing) Then

                oEvent.Dispose()
                oEvent = Nothing
            End If


        End Try
        Return result
    End Function
    ' ***************************************************************** '
    ' Name: GetClientPolicyDetails
    '
    ' Description:
    '
    ' History: 02/10/2002 sj - Created.
    ' RAM20021022 : Added Party Short Name Parameter
    '               Note : Updated spu_get_client_policy_details stored Procedure too
    '               (NRMA Changes - Sirius Process No 126)
    ' ***************************************************************** '
    Public Function GetClientPolicyDetails(ByVal v_lInsuranceFileCnt As Integer, Optional ByRef r_lPartyCnt As Integer = 0, Optional ByRef r_sPartyShortName As String = "", Optional ByRef r_lInsuranceFolderCnt As Integer = 0, Optional ByRef r_sInsuranceRef As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultarray(,) As Object

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed for insurance_file_cnt " & v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientPolicyDetails")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClientPolicyDetailsSQL, sSQLName:=ACGetClientPolicyDetailsName, bStoredProcedure:=ACGetClientPolicyDetailsStored, vResultArray:=vResultarray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLSelect Failed calling " & ACGetClientPolicyDetailsSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientPolicyDetails")
                Return result
            End If

            If Not Information.IsArray(vResultarray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_lPartyCnt = CInt(vResultarray(0, 0))

            r_sPartyShortName = CStr(vResultarray(1, 0)) ' RAM20021022 : Added Party Short Name too

            r_lInsuranceFolderCnt = CInt(vResultarray(2, 0))

            r_sInsuranceRef = CStr(vResultarray(3, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClientPolicyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientPolicyDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetClaimInfoFromClaimRef(ByVal v_sClaimRef As String, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Claim Number parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_Ref", vValue:=v_sClaimRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                gPMFunctions.RaiseError("GetclaimInfoFromClaimRef", "m_oDatabase.Parameters.Add Failed")
            End If

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_Get_ClaimInfo_FromClaimRef", sSQLName:="spu_Get_ClaimInfo_FromClaimRef", bStoredProcedure:=True, vResultArray:=r_vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetclaimInfoFromClaimRef", "spu_Get_ClaimInfo_FromClaimRef")
            End If

            If Not Information.IsArray(r_vResultArray) Then
                gPMFunctions.RaiseError("GetclaimInfoFromClaimRef", "spu_Get_ClaimInfo_FromClaimRef - No Data Found")
            End If


        Catch ex As Exception

            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetClaimInfoFromClaimRef", r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function
End Class
