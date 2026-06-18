Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Developer Guide No: 129
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 30/09/2000
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a CLMDefnFlds.
    '
    ' Edit History: DG
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 18/12/2003
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

    ' Collection of CLMDefnFldss (Private)
    Private m_oCLMPerilTypeReserveTypes As bCLMPerilReserveType.CLMPerilRsrvTypes

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
    Private m_lPerilTypeReserveType As Integer

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business
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
                Case Is > m_oCLMPerilTypeReserveTypes.Count()
                    m_lCurrentRecord = m_oCLMPerilTypeReserveTypes.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Number in Collection
            Return m_oCLMPerilTypeReserveTypes.Count()

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


    Public Property PerilTypeReserveType() As Integer
        Get

            Return m_lPerilTypeReserveType

        End Get
        Set(ByVal Value As Integer)

            m_lPerilTypeReserveType = Value

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
    Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUserName
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

            ' Create CLMDefnFldss Collection
            m_oCLMPerilTypeReserveTypes = New bCLMPerilReserveType.CLMPerilRsrvTypes()

            ' Create PM Lookup Business Object
            m_oLookup = New bPMLookup.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUserName:=sUserName, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

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
                m_oCLMPerilTypeReserveTypes = Nothing
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

    '' ***************************************************************** '
    '' Name: GetLookupValues (Public)
    ''
    '' Description: Gets the Lookup values for a CLMDefnFlds.
    ''
    ''
    '' ***************************************************************** '
    'Public Function GetLookupValues( _
    ''    iLookupType As Integer, _
    ''    vTableArray As Variant, _
    ''    iLanguageID As Integer, _
    ''    vResultArray As Variant) As Long
    '
    'Dim oCLMDefnFlds As bCLMDefnFlds.CLMDefnFlds
    'Dim dtEffectiveDate As Date
    '
    '' {* USER DEFINED CODE (Begin) *}
    'Dim vTabArray(3, 0) As Variant
    'Dim vReserveID As Variant
    ''Dim vEventTypeID As Variant
    '' {* USER DEFINED CODE (End) *}
    '
    '    On Error GoTo Err_GetLookupValues
    '
    '    GetLookupValues = PMTrue
    '
    '    ' Reset Result Array
    '    vResultArray = ""
    '    ' Reset Table Array
    '    vTableArray = ""
    '
    '    ' {* USER DEFINED CODE (Begin) *}
    '
    '    ' Setup Lookup Table Names
    '    vTabArray(PMLookupTableName, 0) = SIRLookupContactType
    ''    vTabArray(PMLookupTableName, 1) = PMLookupEventType
    '
    '    ' {* USER DEFINED CODE (End) *}
    '
    '    ' Do we have any records
    '    If (m_lCurrentRecord& < 1) Then
    '        ' No, we can only lookup all
    '        iLookupType = PMLookupAll
    '    Else
    '        ' Yes get current record
    '        Set oCLMDefnFlds = m_oCLMPerilTypeReserveTypes.Item(m_lCurrentRecord&)
    '    End If
    '
    '    Select Case iLookupType%
    '      Case PMLookupAll
    '
    '        ' Do not supply a key
    '        vTabArray(PMLookupKey, 0) = ""
    ''        vTabArray(PMLookupKey, 1) = ""
    '
    '      Case PMLookupAllEffective
    '
    '        ' Use keys and effective date from current record
    '        ' Note: The keys are not used for the select, but are used by
    '        '       the iterface program to set the list index.
    '        With oCLMDefnFlds
    '
    '            ' {* USER DEFINED CODE (Begin) *}
    '            m_lReturn& = .GetProperties(iStatus:=PMView, _
    ''                                        vReserveID:=vReserveID)
    '
    '            vTabArray(PMLookupKey, 0) = vReserveID
    '            ' {* USER DEFINED CODE (End) *}
    '
    '        End With
    '
    '      Case PMLookupSingle
    '
    '        ' Set keys from current record
    '        With oCLMDefnFlds
    '
    '            ' {* USER DEFINED CODE (Begin) *}
    '            m_lReturn& = .GetProperties(iStatus:=PMView, _
    ''                                        vReserveID:=vReserveID)
    '
    '            vTabArray(PMLookupKey, 0) = vReserveID
    '            'vTabArray(PMLookupKey, 1) = vEventTypeID
    '            ' {* USER DEFINED CODE (End) *}
    '
    '        End With
    '
    '    End Select
    '
    '    ' Default Effective Date to current date/time
    '    dtEffectiveDate = Now
    '
    '    ' Release CLMDefnFlds reference
    '    Set oCLMDefnFlds = Nothing
    '
    '    ' Get the Lookup items
    '    m_lReturn& = m_oLookup.GetLookupValues( _
    ''        iLookupType:=iLookupType, _
    ''        vTableArray:=vTabArray, _
    ''        iLanguageID:=iLanguageID, _
    ''        dtEffectiveDate:=dtEffectiveDate, _
    ''        vResultArray:=vResultArray)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        GetLookupValues = PMFalse
    '        Exit Function
    '    End If
    '
    '    ' Return the Table Array
    '    vTableArray = vTabArray
    '
    '    Exit Function
    '
    '
    'Err_GetLookupValues:
    '
    '    GetLookupValues = PMError
    '
    '    ' Log Error Message
    '    LogMessage m_sUsername, _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="GetLookupValues Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="GetLookupValues", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function




    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied CLMDefnFlds into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vPerilTypeReserveTypeId As Object = Nothing, Optional ByRef vReserveTypeId As Object = Nothing, Optional ByRef vPerilTypeId As Object = Nothing, Optional ByRef vMainReserve As Object = Nothing, Optional ByRef vMode As Object = Nothing) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim oCLMPerilTypeReserveType As bCLMPerilReserveType.CLMPerilRsrvType

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oCLMPerilTypeReserveTypes.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new CLMDefnFlds
            oCLMPerilTypeReserveType = New bCLMPerilReserveType.CLMPerilRsrvType()
            m_lReturn = CType(oCLMPerilTypeReserveType.Initialise(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            ' Populate CLMDefnFlds Attributes
            m_lReturn = CType(oCLMPerilTypeReserveType.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPerilTypeReserveTypeId:=vPerilTypeReserveTypeId, vReserveTypeId:=vReserveTypeId, vPerilTypeId:=vPerilTypeId, vMainReserve:=vMainReserve), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCLMPerilTypeReserveType = Nothing
                Return result
            End If

            ' Add CLMDefnFlds to collection
            m_lReturn = CType(m_oCLMPerilTypeReserveTypes.Add(oCLMPerilTypeReserveType), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oCLMPerilTypeReserveType = Nothing
                Return result
            End If

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
    ' Description: Validates that this action is valid on the CLMDefnFlds
    '              specified and updates the CLMDefnFlds with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vPerilTypeReserveTypeId As Integer = 0, Optional ByRef vReserveTypeId As Object = Nothing, Optional ByRef vPerilTypeId As Object = Nothing, Optional ByRef vMainReserve As Object = Nothing, Optional ByRef vMode As Object = Nothing) As gPMConstants.PMEReturnCode


        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim oCLMPerilTypeReserveType As bCLMPerilReserveType.CLMPerilRsrvType

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oCLMPerilTypeReserveTypes.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new CLMDefnFlds
            oCLMPerilTypeReserveType = New bCLMPerilReserveType.CLMPerilRsrvType()
            m_lReturn = CType(oCLMPerilTypeReserveType.Initialise(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            ' Populate CLMDefnFlds Attributes
            m_lReturn = CType(oCLMPerilTypeReserveType.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMEdit, vPerilTypeReserveTypeId:=vPerilTypeReserveTypeId, vReserveTypeId:=vReserveTypeId, vPerilTypeId:=vPerilTypeId, vMainReserve:=vMainReserve), gPMConstants.PMEReturnCode)

            'Assign the Reserve Type ID (PK) to the property of SINGLE
            oCLMPerilTypeReserveType.PerilTypeReserveType = vPerilTypeReserveTypeId


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCLMPerilTypeReserveType = Nothing
                Return result
            End If

            ' Add CLMDefnFlds to collection
            m_lReturn = CType(m_oCLMPerilTypeReserveTypes.Add(oCLMPerilTypeReserveType), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oCLMPerilTypeReserveType = Nothing
                Return result
            End If

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
    ' Description: Validate that the specified CLMDefnFlds can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer, ByVal vPerilTypeReserveTypeId As Object, ByVal vMode As Object) As Integer

        Dim result As Integer = 0
        Dim oCLMPerilTypeReserveType As bCLMPerilReserveType.CLMPerilRsrvType

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oCLMPerilTypeReserveTypes.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new CLMDefnFlds
            oCLMPerilTypeReserveType = New bCLMPerilReserveType.CLMPerilRsrvType()
            m_lReturn = CType(oCLMPerilTypeReserveType.Initialise(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            ' Populate CLMDefnFlds Attributes
            m_lReturn = CType(oCLMPerilTypeReserveType.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMDelete, vPerilTypeReserveTypeId:=vPerilTypeReserveTypeId), gPMConstants.PMEReturnCode)


            'Assign the Reserve Type ID (PK) to the property of SINGLE
            oCLMPerilTypeReserveType.PerilTypeReserveType = vPerilTypeReserveTypeId

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCLMPerilTypeReserveType = Nothing
                Return result
            End If

            ' Add CLMDefnFlds to collection
            m_lReturn = CType(m_oCLMPerilTypeReserveTypes.Add(oCLMPerilTypeReserveType), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oCLMPerilTypeReserveType = Nothing
                Return result
            End If

            oCLMPerilTypeReserveType = Nothing

            '*******************************

            ''    'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            ''    If (lRow& < 1) _
            '''    Or (lRow& > m_oCLMPerilTypeReserveTypes.Count) Then
            ''        EditDelete = PMFalse
            ''        Exit Function
            ''    End If
            ''
            ''    ' Get a reference to the row to delete
            ''    Set oCLMPerilTypeReserveType = m_oCLMPerilTypeReserveTypes.Item(lRow)
            ''
            ''    ' Check the Status of the CLMDefnFlds
            ''
            ''    'If status is Added (i.e. It is not in the database),
            ''    'then set to Dummy Delete else set to Delete
            ''    If (oCLMPerilTypeReserveType.DatabaseStatus = PMAdd) Then
            ''        oCLMPerilTypeReserveType.DatabaseStatus = PMDummyDelete
            ''    Else
            ''        oCLMPerilTypeReserveType.DatabaseStatus = PMDelete
            ''    End If
            ''
            ''    ' Release reference to CLMDefnFlds
            ''    Set oCLMPerilTypeReserveType = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name:         ClearColl (Public)
    ' Description:  Clears the collection & sets it to zero
    ' ***************************************************************** '
    Public Sub ClearColl()
        m_oCLMPerilTypeReserveTypes.Clear()
    End Sub

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
            For lSub As Integer = 1 To m_oCLMPerilTypeReserveTypes.Count()
                Select Case m_oCLMPerilTypeReserveTypes.Item(lSub).DatabaseStatus
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
        Dim oCLMPerilTypeReserveType As bCLMPerilReserveType.CLMPerilRsrvType
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oCLMPerilTypeReserveTypes.Count()
                oCLMPerilTypeReserveType = m_oCLMPerilTypeReserveTypes.Item(lSub)


                Select Case oCLMPerilTypeReserveType.DatabaseStatus
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
                        m_lReturn = CType(oCLMPerilTypeReserveType.AddItem(), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(oCLMPerilTypeReserveType.UpdateItem(), gPMConstants.PMEReturnCode)
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

                        '                'Check if the reserve Type is not being used by any claim
                        '                m_lReturn& = GetClmForResvType(lRecordCount, vResultArray, oCLMPerilTypeReserveType.PerilTypeReserveType)
                        '
                        '                If (m_lReturn& <> PMTrue) Then
                        '                    Update = PMFalse
                        '                    Exit For
                        '                End If
                        '
                        '                If lRecordCount < 1 Then
                        ' Delete Item
                        m_lReturn = CType(oCLMPerilTypeReserveType.DeleteItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If
                        '                Else
                        '                    MsgBox "Reserve Type " & UCase(vResultArray(1, 0)) & " is being used in live claims.", vbOKOnly, "Invalid Value"
                        '                    Update = PMFalse
                        '                End If

                End Select

            Next lSub

            ' Retain the Primary Key of the CLMDefnFlds
            With oCLMPerilTypeReserveType
                PerilTypeReserveType = .PerilTypeReserveType

                ''        g_lPerilTypeReserveType = .PerilTypeReserveType
            End With


            ' Release last reference
            oCLMPerilTypeReserveType = Nothing

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
                    Do While lSub <= m_oCLMPerilTypeReserveTypes.Count()

                        ' With the item
                        With m_oCLMPerilTypeReserveTypes.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oCLMPerilTypeReserveTypes.Delete(lSub)

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


    Public Function GetAllReserveTypes(ByRef r_vDataArray As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_vDataArray = Nothing

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(ACGetAllReserveTypesSQL, ACGetAllReserveTypesName, ACGetAllReserveTypesStored, , r_vDataArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllReserveTypes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllReserveTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function ChckRsrvTypExstInPrlRsrTyp(ByVal v_lReserveType As Integer, ByVal v_lPerilType As Integer, ByRef r_bExist As Boolean) As Integer
        Dim result As Integer = 0
        Try
            Dim vDataArray(,) As Object
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ReserveType", vValue:=CStr(v_lReserveType), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Periltype", vValue:=CStr(v_lPerilType), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(ACChckRsrvTypExstInPrlRsrTypSQL, ACChckRsrvTypExstInPrlRsrTypName, ACChckRsrvTypExstInPrlRsrTypStored, , vDataArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vDataArray) Then

                r_bExist = CBool(vDataArray(0, 0))
            Else
                r_bExist = False
            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ChckRsrvTypExstInPrlRsrTyp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChckRsrvTypExstInPrlRsrTyp", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function GetRsrvTypForPrlTyp(ByVal v_lPerilType As Integer, ByRef r_vDataArray(,) As Object) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Periltype", vValue:=CStr(v_lPerilType), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(ACGetRsrvTypForPrlTypSQL, ACGetRsrvTypForPrlTypName, ACGetRsrvTypForPrlTypStored, , r_vDataArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If Not Information.IsArray(r_vDataArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRsrvTypForPrlTyp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRsrvTypForPrlTyp", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    Public Function ChckDelForPrlRskTyp(ByVal v_lPerilTypeId As Integer, ByVal v_lReserveTypeId As Integer, ByRef r_bCanExists As Boolean) As Integer
        Dim result As Integer = 0
        Try
            Dim vDataArray(,) As Object
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="PerilType", vValue:=CStr(v_lPerilTypeId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ReserveType", vValue:=CStr(v_lReserveTypeId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(ACChckDelForPrlRskTypSQL, ACChckDelForPrlRskTypName, ACChckDelForPrlRskTypStored, , vDataArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vDataArray) Then

                r_bCanExists = CBool(vDataArray(0, 0))
            Else
                r_bCanExists = False
            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ChckDelForPrlRskTyp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChckDelForPrlRskTyp", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
End Class

