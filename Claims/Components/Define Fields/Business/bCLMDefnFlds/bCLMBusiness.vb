Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
'Developer Guide No: 129
Imports SharedFiles
Imports System.Text
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 06/10/1998
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a CLMDefnFlds.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 11/12/2003
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


    Private m_sUnderwritingOrAgency As String = ""

    ' Collection of CLMDefnFldss (Private)
    Private m_oCLMDefnFldss As bCLMDefnFlds.CLMDefnFldss

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
    Private m_lDataDefnID As Integer

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
                Case Is > m_oCLMDefnFldss.Count()
                    m_lCurrentRecord = m_oCLMDefnFldss.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Number in Collection
            Return m_oCLMDefnFldss.Count()

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


    Public Property DataDefnID() As Integer
        Get

            Return m_lDataDefnID

        End Get
        Set(ByVal Value As Integer)

            m_lDataDefnID = Value

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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


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
            m_oCLMDefnFldss = New bCLMDefnFlds.CLMDefnFldss()


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
                m_oCLMDefnFldss = Nothing
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
    '        Set oCLMDefnFlds = m_oCLMDefnFldss.Item(m_lCurrentRecord&)
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
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vDataDefnID As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vMandatory As Object = Nothing, Optional ByRef vCaption As Object = Nothing, Optional ByRef vRiskTypeID As Object = Nothing, Optional ByRef vType As Object = Nothing, Optional ByRef vDispOrd As Object = Nothing, Optional ByRef vReadOnly As Object = Nothing, Optional ByRef vClmPrtyTypeID As Object = Nothing, Optional ByRef vClmLookupID As Object = Nothing, Optional ByRef vTabID As Object = Nothing, Optional ByRef vTabCaption As Object = Nothing, Optional ByRef vMode As Object = Nothing) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim oCLMDefnFlds As bCLMDefnFlds.CLMDefnFlds

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oCLMDefnFldss.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new CLMDefnFlds
            oCLMDefnFlds = New bCLMDefnFlds.CLMDefnFlds()
            m_lReturn = CType(oCLMDefnFlds.Initialise(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            ' Populate CLMDefnFlds Attributes


            m_lReturn = CType(oCLMDefnFlds.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vDescription:=CStr(vDescription), vMandatory:=CInt(vMandatory), vCaption:=CStr(vCaption), vRiskTypeID:=CInt(vRiskTypeID), vType:=CInt(vType), vDispOrd:=vDispOrd, vReadOnly:=CInt(vReadOnly), vClmPrtyTypeID:=vClmPrtyTypeID, vClmLookupID:=vClmLookupID, vTabID:=CInt(vTabID), vTabCaption:=CStr(vTabCaption), vMode:=CInt(vMode)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCLMDefnFlds = Nothing
                Return result
            End If

            ' Add CLMDefnFlds to collection
            m_lReturn = CType(m_oCLMDefnFldss.Add(oNewCLMDefnFlds:=oCLMDefnFlds), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oCLMDefnFlds = Nothing
                Return result
            End If

            oCLMDefnFlds = Nothing

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
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vDataDefnID As Integer = 0, Optional ByRef vDescription As Object = Nothing, Optional ByRef vMandatory As Object = Nothing, Optional ByRef vCaption As Object = Nothing, Optional ByRef vRiskTypeID As Object = Nothing, Optional ByRef vType As Object = Nothing, Optional ByRef vDispOrd As Object = Nothing, Optional ByRef vReadOnly As Object = Nothing, Optional ByRef vClmPrtyTypeID As Object = Nothing, Optional ByRef vClmLookupID As Object = Nothing, Optional ByRef vTabID As Object = Nothing, Optional ByRef vTabCaption As Object = Nothing, Optional ByRef vMode As Object = Nothing) As gPMConstants.PMEReturnCode


        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim oCLMDefnFlds As bCLMDefnFlds.CLMDefnFlds

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oCLMDefnFldss.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new CLMDefnFlds
            oCLMDefnFlds = New bCLMDefnFlds.CLMDefnFlds()
            m_lReturn = CType(oCLMDefnFlds.Initialise(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            ' Populate CLMDefnFlds Attributes

            m_lReturn = CType(oCLMDefnFlds.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMEdit, vDataDefnID:=vDataDefnID, vDescription:=CStr(vDescription), vMandatory:=CInt(vMandatory), vCaption:=CStr(vCaption), vRiskTypeID:=CInt(vRiskTypeID), vType:=CInt(vType), vDispOrd:=vDispOrd, vReadOnly:=CInt(vReadOnly), vClmPrtyTypeID:=vClmPrtyTypeID, vClmLookupID:=vClmLookupID, vTabID:=CInt(vTabID), vTabCaption:=CStr(vTabCaption), vMode:=CInt(vMode)), gPMConstants.PMEReturnCode)


            'Assign the Reserve Type ID (PK) to the property of SINGLE
            oCLMDefnFlds.DataDefnID = vDataDefnID


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCLMDefnFlds = Nothing
                Return result
            End If

            ' Add CLMDefnFlds to collection
            m_lReturn = CType(m_oCLMDefnFldss.Add(oNewCLMDefnFlds:=oCLMDefnFlds), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oCLMDefnFlds = Nothing
                Return result
            End If

            oCLMDefnFlds = Nothing


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
    Public Function EditDelete(ByVal lRow As Integer, ByVal vDataDefnID As Integer, ByVal vMode As Object) As Integer

        Dim result As Integer = 0
        Dim oCLMDefnFlds As bCLMDefnFlds.CLMDefnFlds

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oCLMDefnFldss.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new CLMDefnFlds
            oCLMDefnFlds = New bCLMDefnFlds.CLMDefnFlds()
            m_lReturn = CType(oCLMDefnFlds.Initialise(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            ' Populate CLMDefnFlds Attributes

            m_lReturn = CType(oCLMDefnFlds.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMDelete, vDataDefnID:=vDataDefnID, vMode:=CInt(vMode)), gPMConstants.PMEReturnCode)


            'Assign the Reserve Type ID (PK) to the property of SINGLE
            oCLMDefnFlds.DataDefnID = vDataDefnID

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCLMDefnFlds = Nothing
                Return result
            End If

            ' Add CLMDefnFlds to collection
            m_lReturn = CType(m_oCLMDefnFldss.Add(oNewCLMDefnFlds:=oCLMDefnFlds), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oCLMDefnFlds = Nothing
                Return result
            End If

            oCLMDefnFlds = Nothing

            '*******************************

            ''    'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            ''    If (lRow& < 1) _
            '''    Or (lRow& > m_oCLMDefnFldss.Count) Then
            ''        EditDelete = PMFalse
            ''        Exit Function
            ''    End If
            ''
            ''    ' Get a reference to the row to delete
            ''    Set oCLMDefnFlds = m_oCLMDefnFldss.Item(lRow)
            ''
            ''    ' Check the Status of the CLMDefnFlds
            ''
            ''    'If status is Added (i.e. It is not in the database),
            ''    'then set to Dummy Delete else set to Delete
            ''    If (oCLMDefnFlds.DatabaseStatus = PMAdd) Then
            ''        oCLMDefnFlds.DatabaseStatus = PMDummyDelete
            ''    Else
            ''        oCLMDefnFlds.DatabaseStatus = PMDelete
            ''    End If
            ''
            ''    ' Release reference to CLMDefnFlds
            ''    Set oCLMDefnFlds = Nothing

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
    ' Author:       SK
    ' Date:         06/07/2000
    ' ***************************************************************** '
    Public Sub ClearColl()
        m_oCLMDefnFldss.Clear()
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
            For lSub As Integer = 1 To m_oCLMDefnFldss.Count()
                Select Case m_oCLMDefnFldss.Item(lSub).DatabaseStatus
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
        Dim oCLMDefnFlds As bCLMDefnFlds.CLMDefnFlds
        Dim bTransStarted As Boolean


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oCLMDefnFldss.Count()
                oCLMDefnFlds = m_oCLMDefnFldss.Item(lSub)


                Select Case oCLMDefnFlds.DatabaseStatus
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
                        m_lReturn = CType(oCLMDefnFlds.AddItem(), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(oCLMDefnFlds.UpdateItem(), gPMConstants.PMEReturnCode)
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
                        '                m_lReturn& = GetClmForResvType(lRecordCount, vResultArray, oCLMDefnFlds.DataDefnID)
                        '
                        '                If (m_lReturn& <> PMTrue) Then
                        '                    Update = PMFalse
                        '                    Exit For
                        '                End If
                        '
                        '                If lRecordCount < 1 Then
                        ' Delete Item
                        m_lReturn = CType(oCLMDefnFlds.DeleteItem(), gPMConstants.PMEReturnCode)
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
            With oCLMDefnFlds
                DataDefnID = .DataDefnID

                ''        g_lDataDefnID = .DataDefnID
            End With

            'this value set in the global variable so that it can be retrieved
            'from the frmInterface to set the id in the listview item tag property
            'while Adding item to database


            ' Release last reference
            oCLMDefnFlds = Nothing

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
                    Do While lSub <= m_oCLMDefnFldss.Count()

                        ' With the item
                        With m_oCLMDefnFldss.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oCLMDefnFldss.Delete(lSub)

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
    ' Name: ApplyMergeCodes(Public)
    '
    ' Description: Calls data object to genete the stored proc for this
    '              risk or peril type and also add the wp_fields records
    '
    ' ***************************************************************** '
    Public Function ApplyMergeCodes(ByRef lMode As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' start a transaction
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add call to generate stored procedure and also wp_fields
            'entries for this risk/peril
            m_lReturn = CType(GenerateStoredProcedure(lMode), gPMConstants.PMEReturnCode)

            ' Commit if OK, or Rollback any errors
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Apply Merge Codes failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ApplyMergeCodes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

    ' ***************************************************************** '
    ' Name: CheckMandatory (Private)
    '
    ' Description: Check Mandatory parameters have been passed.
    '
    ' ***************************************************************** '

    'Private Function CheckMandatory(Optional ByRef vDataDefnID As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vMandatory As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCaption As Object = Nothing, Optional ByRef vRevisedReserve As Object = Nothing, Optional ByRef vReceivedToDate As Object = Nothing, Optional ByRef vRevisionCount As Object = Nothing, Optional ByRef vReceiptId As Object = Nothing, Optional ByRef vPartyClaimID As Object = Nothing, Optional ByRef vReceiptAmount As Object = Nothing, Optional ByRef vDateofReceipt As Object = Nothing, Optional ByRef vPaymentId As Object = Nothing, Optional ByRef vClaimID As Object = Nothing, Optional ByRef vPaymentAmount As Object = Nothing, Optional ByRef vDateofPayment As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vTable As Object = Nothing) As Integer
    '
    ''    On Error GoTo Err_CheckMandatory
    '
    ''    CheckMandatory = PMTrue
    ''
    ''    ' {* USER DEFINED CODE (Begin) *}
    ''
    ''    If (IsMissing(vReserveID) = True) _
    '''    Or (IsEmpty(vReserveID) = True) Then
    ''        CheckMandatory = PMMandatoryMissing
    ''        Exit Function
    ''    End If
    ''
    ''    If (IsMissing(vSourceID) = True) _
    '''    Or (IsEmpty(vSourceID) = True) Then
    ''        CheckMandatory = PMMandatoryMissing
    ''        Exit Function
    ''    End If
    ''
    ''    If (IsMissing(vMandatory) = True) _
    '''    Or (IsEmpty(vMandatory) = True) Then
    ''        CheckMandatory = PMMandatoryMissing
    ''        Exit Function
    ''    End If
    ''
    ''    If (IsMissing(vDescription) = True) _
    '''    Or (IsEmpty(vDescription) = True) Then
    ''        CheckMandatory = PMMandatoryMissing
    ''        Exit Function
    ''    End If
    ''
    ''    If (IsMissing(vNumber) = True) _
    '''    Or (IsEmpty(vNumber) = True) Then
    ''        CheckMandatory = PMMandatoryMissing
    ''        Exit Function
    ''    End If
    ''
    ''    If (IsMissing(vCreatedByID) = True) _
    '''    Or (IsEmpty(vCreatedByID) = True) Then
    ''        CheckMandatory = PMMandatoryMissing
    ''        Exit Function
    ''    End If
    ''
    ''    If (IsMissing(vDateCreated) = True) _
    '''    Or (IsEmpty(vDateCreated) = True) Then
    ''        CheckMandatory = PMMandatoryMissing
    ''        Exit Function
    ''    End If
    ''
    ''
    ''    ' {* USER DEFINED CODE (End) *}
    ''
    ''    Exit Function
    ''
    ''
    ''Err_CheckMandatory:
    ''
    ''    CheckMandatory = PMError
    ''
    ''    ' Log Error Message
    ''    LogMessage m_sUsername, _
    '''        iType:=PMLogOnError, _
    '''        sMsg:="CheckMandatory Failed", _
    '''        vApp:=ACApp, _
    '''        vClass:=ACClass, _
    '''        vMethod:="CheckMandatory", _
    '''        vErrNo:=Err.Number, _
    '''        vErrDesc:=Err.Description
    ''
    ''    Exit Function
    '
    'End Function
    ' PRIVATE Methods (End)


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

    ' ***************************************************************** '
    ' Name:         GetRiskDataDefn (Public)
    ' Description:  wrapper func which ids used to get all the
    '               Risk Data Defn details in the database
    ' Returns    :  r_vResultArray: (2-dimensional Array) containing
    '               values in the following order
    '               0-Peril_data_defn_id
    '               1-Caption
    '               2-Description
    '               3-type
    '               4-display_order
    '               5-Mandatory
    '               6-read_only
    '               7-Claim_party_type_id
    '               8-Claim_party_type_desc
    '               9-Claim_Lookup_id
    '               10-Claim_Lookup_name
    '               its calling SP-spu_get_risk_data_defn
    ' Date:         06/09/2000
    ' Author:       SK
    ' ***************************************************************** '
    Public Function GetRiskDataDefn(ByRef r_vResultArray(,) As Object, ByVal v_lRiskTypeId As Integer, ByRef r_lRecordsFound As Integer) As Integer

        Dim result As Integer = 0
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Insurance Cnt parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_type_id", vValue:=CStr(v_lRiskTypeId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskDataDefn")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskDataDefnSQL, sSQLName:=ACGetRiskDataDefnName, bStoredProcedure:=ACGetRiskDataDefnStored, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' If NO Insurers were found return Not Found
            If Not Information.IsArray(r_vResultArray) Then
                r_lRecordsFound = gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskDataDefn Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskDataDefn", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name:         GetPerilDataDefn (Public)
    ' Description:  wrapper func which ids used to get all the
    '               Peril Data Defn details in the database
    ' Returns    :  r_vResultArray: (2-dimensional Array) containing
    '               values in the following order
    '               0-Peril_data_defn_id
    '               1-Caption
    '               2-Description
    '               3-type
    '               4-display_order
    '               5-Mandatory
    '               6-read_only
    '               7-Claim_party_type_id
    '               8-Claim_party_type_desc
    '               9-Claim_Lookup_id
    '               10-Claim_Lookup_name
    '               its calling SP-spu_get_peril_data_defn
    ' Date:         06/09/2000
    ' Author:       SK
    ' ***************************************************************** '
    Public Function GetPerilDataDefn(ByRef r_vResultArray(,) As Object, ByVal v_lPerilTypeId As Integer, ByRef r_lRecordsFound As Integer) As Integer

        Dim result As Integer = 0
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Insurance Cnt parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Peril_type_id", vValue:=CStr(v_lPerilTypeId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPerilDataDefn")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPerilDataDefnSQL, sSQLName:=ACGetPerilDataDefnName, bStoredProcedure:=ACGetPerilDataDefnStored, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' If NO Insurers were found return Not Found
            If Not Information.IsArray(r_vResultArray) Then
                r_lRecordsFound = gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPerilDataDefn Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPerilDataDefn", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         ChkCaptionExists (Public)
    ' Description:  wrapper func to find if a CAPTION being passed
    '               Exists in the database, before we attempt to Add it
    '               SP-spu_chk_resv_type_name_exists
    ' Returns:      No. of records returned by the query
    ' Author:       SK
    ' Date:         06/09/2000
    ' ***************************************************************** '
    Public Function ChkCaptionExists(ByRef r_lRecordCount As Integer, ByVal v_sCaption As String, ByVal v_iTypeId As Integer, ByVal iMode As Integer, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Const RiskMode As Integer = 0
        Const PerilMode As Integer = 1

        Try

            result = True

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Caption parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Caption", vValue:=v_sCaption, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChkCaptionExists")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Add the Type_id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Type_id", vValue:=CStr(v_iTypeId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChkCaptionExists")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Add the Type_id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="iType", vValue:=CStr(v_iType), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChkCaptionExists")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If iMode = RiskMode Then
                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACChkRiskCaptionExistsSQL, sSQLName:=ACChkRiskCaptionExistsName, bStoredProcedure:=ACChkRiskCaptionExistsStored)

            ElseIf iMode = PerilMode Then
                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACChkPerilCaptionExistsSQL, sSQLName:=ACChkPerilCaptionExistsName, bStoredProcedure:=ACChkPerilCaptionExistsStored)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()


            r_lRecordCount = lRecordCount


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ChkCaptionExists Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChkCaptionExists", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name:         ChkDispOrdExists (Public)
    ' Description:  wrapper func to find if a DispOrd being passed
    '               Exists in the database, before we attempt to Add it
    '               SP-spu_chk_resv_type_name_exists
    ' Returns:      No. of records returned by the query
    ' Author:       SK
    ' Date:         06/09/2000
    ' ***************************************************************** '
    Public Function ChkDispOrdExists(ByRef r_lRecordCount As Integer, ByVal v_sDispOrd As String, ByVal v_iTypeId As Integer, ByVal iMode As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Const RiskMode As Integer = 0
        Const PerilMode As Integer = 1

        Try

            result = True

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Insurance Cnt parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="display_order", vValue:=v_sDispOrd, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChkDispOrdExists")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Add the Insurance Cnt parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Type_id", vValue:=CStr(v_iTypeId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChkDispOrdExists")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            If iMode = RiskMode Then
                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACChkRiskDispOrdExistsSQL, sSQLName:=ACChkRiskDispOrdExistsName, bStoredProcedure:=ACChkRiskDispOrdExistsStored)

            ElseIf iMode = PerilMode Then
                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACChkPerilDispOrdExistsSQL, sSQLName:=ACChkPerilDispOrdExistsName, bStoredProcedure:=ACChkPerilDispOrdExistsStored)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()


            r_lRecordCount = lRecordCount


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ChkDispOrdExists Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChkDispOrdExists", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         SelLookupTables (Public)
    ' Description:  Selects all the Lookup_tablename records in the
    '               Claim_Lookup Table
    ' Returns:      0-claim_lookup_id
    '               1-Lookup_tablename
    '               2-description
    ' Author:       SK
    ' Date:         06/09/2000
    ' ***************************************************************** '
    Public Function SelLookupTables(ByRef rvResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oFields As DataRow
        Dim vResultArray(,) As Object
        Dim lRecordCount As Integer

        Try

            result = True

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelLookupTablesSQL, sSQLName:=ACSelLookupTablesName, bStoredProcedure:=ACSelLookupTablesStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount < 1 Then
                ' No record found
                Return False
            End If

            'array will have 3 columns, indefinate no. of rows
            ReDim vResultArray(2, 0)

            For nRecCount As Integer = 1 To lRecordCount
                'ReDim the arrasy since we are changing its dimensions
                '& preserve it since we need tosave the values of the array
                ReDim Preserve vResultArray(2, nRecCount - 1)

                oFields = m_oDatabase.Records.Fields.Table.Rows(nRecCount - 1)

                vResultArray(0, nRecCount - 1) = oFields("claim_lookup_id")

                vResultArray(1, nRecCount - 1) = oFields("Lookup_tablename")

                vResultArray(2, nRecCount - 1) = oFields("description")

            Next


            rvResultArray = vResultArray


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelLookupTables Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelLookupTables", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         SelPartyTypes (Public)
    ' Description:  Selects all the Party Types records in the
    '               Party_Type Table
    ' Returns:      0-Claim_Party_type_id
    '               1-description
    ' Author:       SK
    ' Date:         06/09/2000
    ' ***************************************************************** '
    Public Function SelPartyTypes(ByRef rvResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oFields As ADODB.Fields
        Dim vResultArray(,) As Object
        Dim lRecordCount As Integer

        Try

            result = True

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelPartyTypesSQL, sSQLName:=ACSelPartyTypesName, bStoredProcedure:=ACSelPartyTypesStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount < 1 Then
                ' No record found
                Return False
            End If

            'array will have 2 columns, indefinate no. of rows
            ReDim vResultArray(1, 0)

            For nRecCount As Integer = 1 To lRecordCount
                'ReDim the arrasy since we are changing its dimensions
                '& preserve it since we need tosave the values of the array
                ReDim Preserve vResultArray(1, nRecCount - 1)

                oFields = m_oDatabase.Records.Item(nRecCount).Fields()


                vResultArray(0, nRecCount - 1) = oFields("Claim_Party_type_id")

                vResultArray(1, nRecCount - 1) = oFields("Description")

            Next


            rvResultArray = vResultArray


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelPartyTypes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelPartyTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         ChkDataDefnIDExists (Public)
    ' Description:  wrapper func to find if a DataDefnID being passed
    '               Exists in the database, before we attempt to Delete it
    '               SP-spu_chk_resv_type_name_exists
    ' Returns:      No. of records returned by the query
    ' Author:       SK
    ' Date:         06/09/2000
    ' ***************************************************************** '
    Public Function ChkDataDefnIDExists(ByRef r_lRecordCount As Integer, ByVal v_lDataDefnID As Integer, ByVal iMode As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        'Const RiskMode As Integer = 0
        'Const PerilMode As Integer = 1

        Try

            result = True

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Data Definition Id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="data_defn_id", vValue:=CStr(v_lDataDefnID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChkDataDefnIDExists")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            'Add the Data Definition Id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Mode", vValue:=CStr(iMode), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChkDataDefnIDExists")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACChkDataDefnIDExistsSQL, sSQLName:=ACChkDataDefnIDExistsName, bStoredProcedure:=ACChkDataDefnIDExistsStored)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()


            r_lRecordCount = lRecordCount


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ChkDataDefnIDExists Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChkDataDefnIDExists", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         ChkDataDefnIDForPartyExists (Public)
    ' Description:  wrapper func to find if a DataDefnID being passed
    '               Exists in the database, before we attempt to Delete it
    '               SP-spu_chk_resv_type_name_exists
    ' Returns:      No. of records returned by the query
    ' Author:       SK
    ' Date:         06/09/2000
    ' ***************************************************************** '
    Public Function ChkDataDefnIDForPartyExists(ByRef r_lRecordCount As Integer, ByVal v_lTypeID As Integer, ByVal v_lPrtyTypeID As Integer, ByVal iMode As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        'Const RiskMode As Integer = 0
        'Const PerilMode As Integer = 1

        Try

            result = True

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Data Definition Id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="type_id", vValue:=CStr(v_lTypeID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChkDataDefnIDForPartyExists")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add the Data Definition Id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_type_id", vValue:=CStr(v_lPrtyTypeID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChkDataDefnIDForPartyExists")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Add the Data Definition Id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Mode", vValue:=CStr(iMode), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChkDataDefnIDForPartyExists")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACChkDataDefnIDForPartyExistsSQL, sSQLName:=ACChkDataDefnIDForPartyExistsName, bStoredProcedure:=ACChkDataDefnIDForPartyExistsStored, vResultArray:=vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected

            r_lRecordCount = CInt(vResultArray(0, 0))




            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ChkDataDefnIDForPartyExists Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChkDataDefnIDForPartyExists", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'function not being used
    ' ***************************************************************** '
    ' Name:         ChkLookupExists (Public)
    ' Description:  wrapper func to find if a Lookup being passed
    '               Exists in the database, before we attempt to Add it
    '               SP-spu_chk_resv_type_name_exists
    ' Returns:      No. of records returned by the query
    ' Author:       SK
    ' Date:         06/09/2000
    ' ***************************************************************** '
    Public Function ChkLookupExists(ByRef r_lRecordCount As Integer, ByVal v_iLookup As Integer, ByVal v_iTypeId As Integer, ByVal iMode As Integer) As Integer

        Dim result As Integer = 0

        Dim lRecordCount As Integer

        'Const RiskMode As Integer = 0
        'Const PerilMode As Integer = 1

        Try

            result = True

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Insurance Cnt parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Lookup", vValue:=CStr(v_iLookup), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChkLookupExists")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Add the Insurance Cnt parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Type_id", vValue:=CStr(v_iTypeId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChkLookupExists")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            '    If iMode = RiskMode Then
            '        ' Execute SQL Statement
            '        m_lReturn& = m_oDatabase.SQLSelect( _
            ''            sSQL:=ACChkRiskLookupExistsSQL, _
            ''            sSQLName:=ACChkRiskLookupExistsName, _
            ''            bStoredProcedure:=ACChkRiskLookupExistsStored)
            '
            '    ElseIf iMode = PerilMode Then
            '        ' Execute SQL Statement
            '        m_lReturn& = m_oDatabase.SQLSelect( _
            ''            sSQL:=ACChkPerilLookupExistsSQL, _
            ''            sSQLName:=ACChkPerilLookupExistsName, _
            ''            bStoredProcedure:=ACChkPerilLookupExistsStored)
            '    End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()


            r_lRecordCount = lRecordCount


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ChkLookupExists Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChkLookupExists", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GenerateStoredProcedure
    '
    ' Description:
    '
    ' History: 18/05/01 DC  - Dynamically generate stored procedure to retreive
    '                         wp_fields values
    '
    ' ***************************************************************** '

    Private Function GenerateStoredProcedure(ByRef lMode As Integer) As Integer

        Dim result As Integer = 0
        Dim sProcedureName, sWhereList As String
        Dim sFinalSelectLines As New StringBuilder
        Dim sParameterList As New StringBuilder
        Dim sSelectLines As New StringBuilder
        Dim lTemp3 As Integer
        Dim sTemp As String
        Dim sSQL As New StringBuilder
        'DC020701 set to max of 30 chars
        Dim sFieldName2 As New FixedLengthString(25)
        Dim sColumnNameUse, sSubGroup As String
        Dim sColumnName As New StringBuilder
        Dim sDisplayName As New StringBuilder
        Dim sFieldName As New StringBuilder
        Dim lFormat As gPMConstants.PMEFormatStyle
        Dim vArray(,) As Object
        Dim sPrefix, sTypeCode, sTypeCode2, sOldCode, sTypeDescription As String
        Dim sSelect As String = ""

        Dim sFromList As String = ""




        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = CType(bPMFunc.getUnderwritingOrAgency(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, r_vUnderwriting:=m_sUnderwritingOrAgency), gPMConstants.PMEReturnCode)

        'define whether risk or peril
        If lMode = ACRiskMode Then
            sProcedureName = "spu_wp_claimrisk"
            sSubGroup = "Risk UD - "
            sPrefix = "CRUD"
        Else
            sProcedureName = "spu_wp_claimperil"
            sSubGroup = "Peril UD - "
            sPrefix = "CPUD"
        End If

        'Delete the records on wp_fields
        sSQL = New StringBuilder("")

        sSQL.Append("DELETE FROM wp_fields" & Strings.Chr(13) & Strings.Chr(10) & _
                    "WHERE Sub_Group Like '" & sSubGroup & "%'" & Strings.Chr(13) & Strings.Chr(10))

        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="Delete " & sSubGroup & " WPFields", bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        sSQL = New StringBuilder("")

        If lMode = ACRiskMode Then

            'TN20010820 - underwriting boys and girls : we no longer use claim_party_type
            'table. I won't bother changing this function as it won't make
            'any different if we select it


            'Modifying the inline query to make it compatible with SQL server 2005

            sSelectLines = New StringBuilder("SELECT rt.code, rt.description, rdf.caption, ")
            sSelectLines.Append("rdf.description, rdf.type, ")
            sSelectLines.Append("rdf.claim_party_type_id, cpt.Description, ")
            sSelectLines.Append("rdf.claim_lookup_id, cl.lookup_tablename ")

            sFromList = "FROM risk_data_definition rdf "
            sFromList = sFromList & "INNER JOIN risk_type rt "
            sFromList = sFromList & "ON rt.risk_type_id = rdf.risk_type_id "

            sFromList = sFromList & "LEFT OUTER JOIN claim_party_type cpt "
            sFromList = sFromList & "ON rdf.claim_party_type_id = cpt.claim_party_type_id "
            sFromList = sFromList & "LEFT OUTER JOIN claim_lookup cl "
            sFromList = sFromList & "ON rdf.claim_lookup_id = cl.claim_lookup_id "

            'After restructuring, the where list only contains an ORDER BY clause
            sWhereList = "ORDER BY rt.code "

        Else

            'Modifying the inline query to make it compatible with SQL server 2005

            sSelectLines = New StringBuilder("SELECT pt.code, pt.description, pdf.caption, ")
            sSelectLines.Append("pdf.description, pdf.type, ")
            sSelectLines.Append("pdf.claim_party_type_id, cpt.Description, ")
            sSelectLines.Append("pdf.claim_lookup_id, cl.lookup_tablename ")

            sFromList = "FROM peril_data_definition pdf "
            sFromList = sFromList & "INNER JOIN peril_type pt "
            sFromList = sFromList & "ON pt.peril_type_id = pdf.peril_type_id "
            sFromList = sFromList & "LEFT OUTER JOIN claim_party_type cpt "
            sFromList = sFromList & "ON pdf.claim_party_type_id = cpt.claim_party_type_id "
            sFromList = sFromList & "LEFT OUTER JOIN claim_lookup cl "
            sFromList = sFromList & "ON pdf.claim_lookup_id = cl.claim_lookup_id "

            'After restructuring, the where list only contains an ORDER BY clause
            sWhereList = "ORDER BY pt.code "

        End If

        sSQL = New StringBuilder(sSelectLines.ToString() & sFromList & sWhereList)

        If lMode = ACRiskMode Then

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:=ACSelectClaimRiskDetailsName, bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray, bKeepNulls:=True)

        Else

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:=ACSelectClaimPerilDetailsName, bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray, bKeepNulls:=True)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Information.IsArray(vArray) Then

            sOldCode = ""
            'loop

            For lTemp2 As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)


                If CDbl(vArray(4, lTemp2)) <> 6 Then


                    sTemp = CStr(vArray(2, lTemp2)).Trim()

                    sColumnName = New StringBuilder("")
                    sFieldName = New StringBuilder("")
                    sDisplayName = New StringBuilder("")

                    While sTemp.IndexOf(" "c) >= 0
                        lTemp3 = (sTemp.IndexOf(" "c) + 1)
                        sFieldName.Append(sTemp.Substring(0, lTemp3 - 1))
                        sDisplayName.Append(sTemp.Substring(0, lTemp3 - 1) & " ")
                        sColumnName.Append(sTemp.Substring(0, lTemp3 - 1) & "_")
                        sTemp = sTemp.Substring(lTemp3)
                    End While

                    sFieldName.Append(sTemp)
                    sDisplayName.Append(sTemp)
                    sColumnName.Append(sTemp)

                    sTypeCode = CStr(vArray(0, lTemp2)).Trim()

                    sTypeDescription = CStr(vArray(1, lTemp2)).Trim()

                    If sTypeCode.Trim() <> sOldCode.Trim() Then

                        If sOldCode <> "" Then

                            'Get rid of the trailing comma and vbCrLf
                            sParameterList = New StringBuilder(sParameterList.ToString().Substring(0, sParameterList.ToString().Length - 3) & Strings.Chr(13) & Strings.Chr(10))
                            sSelectLines = New StringBuilder(sSelectLines.ToString().Substring(0, sSelectLines.ToString().Length - 3) & Strings.Chr(13) & Strings.Chr(10))
                            sFinalSelectLines = New StringBuilder(sFinalSelectLines.ToString().Substring(0, sFinalSelectLines.ToString().Length - 3) & Strings.Chr(13) & Strings.Chr(10))

                            sSQL.Append( _
                                        sParameterList.ToString() & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                        sSelectLines.ToString() & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                        sFinalSelectLines.ToString())

                            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="Create " & sProcedureName & sOldCode & " StoredProcedure", bStoredProcedure:=False)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            'Set permissions

                            sSQL = New StringBuilder("GRANT EXECUTE ON " & sProcedureName & sOldCode & " TO PUBLIC" & Strings.Chr(13) & Strings.Chr(10))

                            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="Grant " & sProcedureName & sOldCode & " StoredProcedure", bStoredProcedure:=False)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                        End If

                        sOldCode = sTypeCode

                        sParameterList = New StringBuilder("DECLARE ")
                        sSelectLines = New StringBuilder("")
                        sFinalSelectLines = New StringBuilder("SELECT ")

                        'Drop it if it's already there
                        sSQL = New StringBuilder("")

                        sSQL.Append("if exists (select * from sysobjects where id = object_id('" & _
                                    sProcedureName & sTypeCode & "') and sysstat & 0xf = 4)" & Strings.Chr(13) & Strings.Chr(10) & _
                                    "drop procedure " & sProcedureName & sTypeCode & Strings.Chr(13) & Strings.Chr(10) & _
                                    Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10))

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="Drop " & sProcedureName & sTypeCode & " StoredProcedure", bStoredProcedure:=False)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'Set Top Of New Stored Procedure
                        sSQL = New StringBuilder("")

                        'DC091003 -PN6966 add RiskId parameter
                        sSQL.Append("CREATE PROCEDURE " & sProcedureName & sTypeCode & Strings.Chr(13) & Strings.Chr(10) & _
                                    "@PartyCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                                    "@InsuranceFileCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                                    "@RiskID INT," & Strings.Chr(13) & Strings.Chr(10) & _
                                    "@ClaimCnt INT," & Strings.Chr(13) & Strings.Chr(10) & _
                                    "@DocumentRef VARCHAR(25)," & Strings.Chr(13) & Strings.Chr(10) & _
                                    "@Instance1 INT," & Strings.Chr(13) & Strings.Chr(10) & _
                                    "@Instance2 INT," & Strings.Chr(13) & Strings.Chr(10) & _
                                    "@Instance3 INT" & Strings.Chr(13) & Strings.Chr(10) & _
                                    "AS" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10))

                    End If

                    sColumnNameUse = sColumnName.ToString()

                    sParameterList.Append("@" & sColumnName.ToString() & " ")

                    Select Case vArray(4, lTemp2)
                        '                            Case PMString
                        Case 1

                            sParameterList.Append("VARCHAR(255)," & Strings.Chr(13) & Strings.Chr(10))

                            lFormat = gPMConstants.PMEFormatStyle.PMFormatString

                            '                            Case PMInteger
                        Case 2

                            sParameterList.Append("NUMERIC(19,4)," & Strings.Chr(13) & Strings.Chr(10))

                            lFormat = gPMConstants.PMEFormatStyle.PMFormatLong

                            '                            Case PMDate
                        Case 3

                            sParameterList.Append("DATETIME," & Strings.Chr(13) & Strings.Chr(10))

                            lFormat = gPMConstants.PMEFormatStyle.PMFormatDateMedium


                            '                            Case PMBoolean
                        Case 4

                            sParameterList.Append("VARCHAR(3)," & Strings.Chr(13) & Strings.Chr(10))

                            lFormat = gPMConstants.PMEFormatStyle.PMFormatBoolean

                            sColumnNameUse = sColumnName.ToString() & "_num"
                            sParameterList.Append("@" & sColumnNameUse)
                            sParameterList.Append(" TINYINT," & Strings.Chr(13) & Strings.Chr(10))

                            sFinalSelectLines.Append( _
                                                     "CASE @" & sColumnNameUse & Strings.Chr(13) & Strings.Chr(10) & _
                                                     "WHEN 1 THEN 'Yes'" & Strings.Chr(13) & Strings.Chr(10) & _
                                                     "ELSE 'No'" & Strings.Chr(13) & Strings.Chr(10) & _
                                                     "END AS '" & sColumnName.ToString() & "'," & Strings.Chr(13) & Strings.Chr(10))

                        Case 5

                            sParameterList.Append("VARCHAR(255)," & Strings.Chr(13) & Strings.Chr(10))

                            lFormat = gPMConstants.PMEFormatStyle.PMFormatString

                        Case Else

                            sParameterList.Append("VARCHAR(255)," & Strings.Chr(13) & Strings.Chr(10))

                            lFormat = gPMConstants.PMEFormatStyle.PMFormatString

                    End Select


                    If CDbl(vArray(4, lTemp2)) <> 4 Then

                        sFinalSelectLines.Append( _
                                                 "'" & sColumnName.ToString() & "' = @" & sColumnName.ToString() & "," & Strings.Chr(13) & Strings.Chr(10))

                    End If


                    If CDbl(vArray(4, lTemp2)) = 5 Then

                        If lMode = ACRiskMode Then

                            sSelectLines.Append("SELECT @" & sColumnNameUse & " = l.description" & Strings.Chr(13) & Strings.Chr(10))

                            sSelectLines.Append("FROM " & CStr(vArray(8, lTemp2)) & " l, risk_data_definition rdd, " & Strings.Chr(13) & Strings.Chr(10))
                            sSelectLines.Append("claim_user_defined_risk_data cudrd, " & Strings.Chr(13) & Strings.Chr(10))

                            sSelectLines.Append("risk_type rt" & Strings.Chr(13) & Strings.Chr(10))


                            sSelectLines.Append("WHERE l." & CStr(vArray(8, lTemp2)) & "_id = cudrd.value" & Strings.Chr(13) & Strings.Chr(10))
                            sSelectLines.Append("AND rt.code = '" & sTypeCode & "' " & Strings.Chr(13) & Strings.Chr(10))
                            sSelectLines.Append("AND cudrd.claim_id = @ClaimCnt" & Strings.Chr(13) & Strings.Chr(10))
                            sSelectLines.Append("AND rdd.risk_type_id = rt.risk_type_id" & Strings.Chr(13) & Strings.Chr(10))

                            sSelectLines.Append("AND rdd.caption = '" & sDisplayName.ToString() & "'" & Strings.Chr(13) & Strings.Chr(10))
                            sSelectLines.Append("AND cudrd.risk_data_defn_id = rdd.risk_data_defn_id" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10))

                        Else

                            sSelectLines.Append("SELECT @" & sColumnNameUse & " = l.description" & Strings.Chr(13) & Strings.Chr(10))

                            sSelectLines.Append("FROM " & CStr(vArray(8, lTemp2)) & " l, peril_data_definition pdd, " & Strings.Chr(13) & Strings.Chr(10))
                            sSelectLines.Append("user_defined_peril_data udpd, peril_type pt" & Strings.Chr(13) & Strings.Chr(10))

                            sSelectLines.Append("WHERE l." & CStr(vArray(8, lTemp2)) & "_id = udpd.value" & Strings.Chr(13) & Strings.Chr(10))
                            sSelectLines.Append("AND pt.code = '" & sTypeCode & "' " & Strings.Chr(13) & Strings.Chr(10))
                            sSelectLines.Append("AND udpd.claim_id = @ClaimCnt" & Strings.Chr(13) & Strings.Chr(10))
                            sSelectLines.Append("AND pdd.peril_type_id = pt.peril_type_id" & Strings.Chr(13) & Strings.Chr(10))
                            sSelectLines.Append("AND pdd.caption = '" & sDisplayName.ToString() & "'" & Strings.Chr(13) & Strings.Chr(10))
                            sSelectLines.Append("AND udpd.peril_data_defn_id = pdd.peril_data_defn_id" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10))

                        End If


                    Else

                        If lMode = ACRiskMode Then

                            sSelectLines.Append("SELECT @" & sColumnNameUse & " = cudrd.value" & Strings.Chr(13) & Strings.Chr(10))
                            sSelectLines.Append("FROM risk_data_definition rdd, claim_user_defined_risk_data cudrd, risk_type rt" & Strings.Chr(13) & Strings.Chr(10))

                            sSelectLines.Append("WHERE rt.code = '" & sTypeCode & "' " & Strings.Chr(13) & Strings.Chr(10))
                            sSelectLines.Append("AND cudrd.claim_id = @ClaimCnt" & Strings.Chr(13) & Strings.Chr(10))
                            sSelectLines.Append("AND rdd.risk_type_id = rt.risk_code_id" & Strings.Chr(13) & Strings.Chr(10))

                            sSelectLines.Append("AND rdd.caption = '" & sDisplayName.ToString() & "'" & Strings.Chr(13) & Strings.Chr(10))
                            sSelectLines.Append("AND cudrd.risk_data_defn_id = rdd.risk_data_defn_id" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10))

                        Else

                            sSelectLines.Append("SELECT @" & sColumnNameUse & " = udpd.value" & Strings.Chr(13) & Strings.Chr(10))
                            sSelectLines.Append("FROM peril_data_definition pdd, user_defined_peril_data udpd, peril_type pt" & Strings.Chr(13) & Strings.Chr(10))
                            sSelectLines.Append("WHERE pt.code = '" & sTypeCode & "' " & Strings.Chr(13) & Strings.Chr(10))
                            sSelectLines.Append("AND udpd.claim_id = @ClaimCnt" & Strings.Chr(13) & Strings.Chr(10))
                            sSelectLines.Append("AND pdd.peril_type_id = pt.peril_type_id" & Strings.Chr(13) & Strings.Chr(10))
                            sSelectLines.Append("AND pdd.caption = '" & sDisplayName.ToString() & "'" & Strings.Chr(13) & Strings.Chr(10))
                            'Steve Watton PN 11907, 13/05/2004. Add extra VBcrlf, otherwise Create Proc is failing.
                            sSelectLines.Append("AND udpd.peril_data_defn_id = pdd.peril_data_defn_id" & Strings.Chr(13) & Strings.Chr(10))
                            'End Steve Watton
                            'MKW PN11817 START

                            If CDbl(vArray(4, lTemp2)) = 2 Then
                                sSelectLines.Append("and isnumeric(udpd.value)=1" & Strings.Chr(13) & Strings.Chr(10))
                            End If
                            sSelectLines.Append(Strings.Chr(13) & Strings.Chr(10))
                            'MKW PN11817 END
                        End If

                    End If
                    'Add the record to wp_fields
                    'DC020701 set up field name - max 30 chars
                    'DC231101 added code as part of sub group, incase description of one code is same as another
                    'SP070102 - Merge catch up
                    'DC230605 PN21878 remove underscore from code for fieldname
                    sTypeCode2 = sTypeCode.Replace("_", "")
                    sFieldName2.Value = sPrefix.Trim() & sTypeCode2.Trim() & sFieldName.ToString().Trim()

                    m_lReturn = CType(AddToWPFields(sFieldName:=sFieldName2.Value, sSQL:=sProcedureName & sTypeCode, sColumnName:=sColumnName.ToString(), lColumnType:=lFormat, sMainGroup:="Claim", sSubGroup:=sSubGroup & " (" & sTypeCode & ") " & sTypeDescription, sDisplayName:=sDisplayName.ToString(), iIsDisplayed:=1, sLoop1:="", sLoop2:="", sLoop3:="", lProductFamily:=PMProductFamily, vDataModel:=DBNull.Value, vPropertyId:=DBNull.Value, vSubGroup2:=DBNull.Value, vSubGroup3:=DBNull.Value), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            Next lTemp2

            sParameterList = New StringBuilder(sParameterList.ToString().Substring(0, sParameterList.ToString().Length - 3) & Strings.Chr(13) & Strings.Chr(10))
            sSelectLines = New StringBuilder(sSelectLines.ToString().Substring(0, sSelectLines.ToString().Length - 3) & Strings.Chr(13) & Strings.Chr(10))
            sFinalSelectLines = New StringBuilder(sFinalSelectLines.ToString().Substring(0, sFinalSelectLines.ToString().Length - 3) & Strings.Chr(13) & Strings.Chr(10))

            sSQL.Append( _
                        sParameterList.ToString() & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                        sSelectLines.ToString() & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                        sFinalSelectLines.ToString())

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="Create " & sProcedureName & sOldCode & " StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set permissions

            sSQL = New StringBuilder("GRANT EXECUTE ON " & sProcedureName & sOldCode & " TO PUBLIC" & Strings.Chr(13) & Strings.Chr(10))

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="Grant " & sProcedureName & sOldCode & " StoredProcedure", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: AddToWPFields
    '
    ' Description: Add record to wp_fields for user defined claims merge codes
    '
    ' History: 13/11/00 CT - created
    '
    ' ***************************************************************** '
    Private Function AddToWPFields(ByRef sFieldName As String, ByRef sSQL As String, ByRef sColumnName As String, ByRef lColumnType As Integer, ByRef sMainGroup As String, ByRef sSubGroup As String, ByRef sDisplayName As String, ByRef iIsDisplayed As Integer, ByRef sLoop1 As String, ByRef sLoop2 As String, ByRef sLoop3 As String, ByRef lProductFamily As Integer, ByRef vDataModel As Object, ByRef vPropertyId As Object, ByRef vSubGroup2 As Object, ByRef vSubGroup3 As Object) As Integer

        Dim result As Integer = 0
        Dim vLoop1 As String = ""
        Dim vLoop2 As String = ""
        Dim vLoop3 As String = ""
        Dim nLength As Integer
        Dim sXtra As String = ""
        Dim nCount As Integer
        Dim bAllOkay As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        If sLoop1.Trim() = "" Then

            vLoop1 = Nothing
        Else
            vLoop1 = sLoop1.Trim()
        End If

        If sLoop2.Trim() = "" Then

            vLoop2 = Nothing
        Else
            vLoop2 = sLoop2.Trim()
        End If

        If sLoop3.Trim() = "" Then

            vLoop3 = Nothing
        Else
            vLoop3 = sLoop3.Trim()
        End If

        'DC180102 loop around adding number on end if merge code has same fieldname as another - to make unique
        'DC091003 PN 6966 -was a space
        sXtra = " "
        nCount = 0
        bAllOkay = False

        While (Not bAllOkay And nCount < 10)

            m_oDatabase.Parameters.Clear()

            'DC081003 -PN6966 -trim fieldname otherwise trailing spaces cause merge to fail
            sFieldName = sFieldName.Substring(0, 17).Trim()
            nLength = sFieldName.Length
            sFieldName = sFieldName.Substring(0, nLength)

            If sXtra <> " " Then
                sFieldName = sFieldName & sXtra
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="field_name", vValue:=sFieldName, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="sql", vValue:=sSQL, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="column_name", vValue:=sColumnName, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="column_type", vValue:=CStr(lColumnType), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="main_group", vValue:=sMainGroup, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="sub_group", vValue:=sSubGroup, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="display_name", vValue:=sDisplayName, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_displayed", vValue:=CStr(iIsDisplayed), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="loop1", vValue:=vLoop1, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="loop2", vValue:=vLoop2, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="loop3", vValue:=vLoop3, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="product_family", vValue:=CStr(lProductFamily), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="data_model", vValue:=CStr(vDataModel), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="property_id", vValue:=CStr(vPropertyId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC240501 added the new fields added to wpfields


            m_lReturn = m_oDatabase.Parameters.Add(sName:="sub_group2", vValue:=CStr(vSubGroup2), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="sub_group3", vValue:=CStr(vSubGroup3), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'DC240501

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertWPFieldsSQL, sSQLName:=ACInsertWPFieldsName, bStoredProcedure:=ACInsertWPFieldsStored)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                bAllOkay = True

            Else

                nCount += 1
                'DC091003 PN6966 -include space
                sXtra = "-" & nCount
                'AddToWPFields = PMFalse
                'Exit Function

            End If

        End While

        Return result

    End Function

    Public Function ListClaimTabs(ByRef r_vResultArray(,) As Object, ByRef r_lRecordsFound As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACTabsListSQL, sSQLName:=ACTabsListName, bStoredProcedure:=ACTabsListStored, vResultArray:=r_vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not Information.IsArray(r_vResultArray) Then
                    ' ISS1745 Logica-CMG(SJP) 17/02/2003
                    ' start
                    result = gPMConstants.PMEReturnCode.PMNotFound
                    ' end
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListClaimTabs", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
