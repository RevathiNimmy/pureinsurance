Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
'developer guide no 129. 
Imports SharedFiles
Imports Artinsoft.VB6.Utils

<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    Dim m_frmInterface As frmInterface
    ' Property members
    Private m_sCallingAppName As String = ""
    Private m_lErrorNumber As Integer
    Private m_lStatus As Integer
    Private m_lNavigate As Integer
    Private m_sNavigatorTitle As String = ""
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_iTask As Integer
    Private m_sProcessStatus As String = ""
    Private m_sMapStatus As String = ""
    Private m_sStepStatus As String = ""

    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_oBusiness As bPMUser.Business
    Private m_vSourceArray(,) As Object = Nothing
    Private m_iSourceID As Integer
    Private m_sSourceCode As String = ""
    Private m_sSourceName As String = ""
    'PN29502
    Private m_iCountryID As Integer
    Private m_lProductId As Integer

    'TN20010706 start
    Private m_lPartyCnt As Integer
    'TN20010706 end

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    'PUBLIC PROPERTIES (Begin)
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property
    Public ReadOnly Property SourceArray() As Object
        Get

            ' Return the Source Array

            Return VB6.CopyArray(m_vSourceArray)

        End Get
    End Property

    'TN20010706 start
    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property
    'TN20010706 end

    Public Property ProductId() As Integer
        Get
            Return m_lProductId
        End Get
        Set(ByVal Value As Integer)
            m_lProductId = Value
        End Set
    End Property

    'PUBLIC PROPERTIES (End)
    ' PUBLIC Methods (Begin)
    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' If UserID is 0 assume that user cancelled logon
            If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                ' Abort application
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                Return result
            End If

            ' Store the language ID etc from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_sUsername.Value = .UserName
                g_sPassword.Value = .Password
                g_iUserID = .UserID
                g_iCurrencyID = .CurrencyID
                g_iLogLevel = .LogLevel
            End With

            ' Equate CompanyID to SourceID
            g_iCompanyId = g_iSourceID

            ' Get an instance of the business object via
            ' the object manager.
            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            Dim sMessage, sTitle As String
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If

            ' Initialise the process modes with default values
            m_lReturn = CType(SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTypeOfBusiness:=gPMConstants.PMTransactionTypeGeneric, vEffectiveDate:=DateTime.Now), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to set default process modes
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try



            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTypeOfBusiness As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

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


            If Not Information.IsNothing(vTypeOfBusiness) Then

                m_sTransactionType = CStr(vTypeOfBusiness)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            ' Set the process modes for the business object.
            If m_oBusiness Is Nothing Then

                'NIIT - Replaced with the Migrated code 1144 
                'm_lReturn = m_oBusiness.SetProcessModes(vTask:=vTask, vNavigate:=vNavigate, vProcessMode:=vProcessMode, vTypeOfBusiness:=vTypeOfBusiness, vEffectiveDate:=vEffectiveDate)
                m_lReturn = ReflectionHelper.Invoke(m_oBusiness, "SetProcessModes", New Object() {vTask, vNavigate, vProcessMode, vTypeOfBusiness, vEffectiveDate})

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes")

                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetSource (Standard Method)
    '
    ' Description: Select Source ID.
    '
    ' ***************************************************************** '
    'Public Function GetSource(ByRef iSourceID As Integer, _
    ''                            Optional ByVal vSourceCode As Variant, _
    ''                            Optional ByVal vSourceName As Variant) As Long
    'Dim lSources As Long
    '    On Error GoTo Err_GetSource
    '
    '    GetSource = PMTrue
    ''   m_lReturn = m_oBusiness.GetAllSources(r_vSourceArray:=m_vSourceArray)
    '    m_lReturn = m_oBusiness.GetUserSources(r_vSourceArray:=m_vSourceArray, v_vUserID:=g_iUserID)
    '
    '    If m_lReturn <> PMTrue Then
    '        GetSource = PMFalse
    '        LogMessagePopup _
    ''            iType:=PMLogOnError, _
    ''            sMsg:="Failed to get valid Branches", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="GetSource", _
    ''            vErrNo:=Err.Number, _
    ''            vErrDesc:=Err.Description
    '
    '        Exit Function
    '    End If
    '
    '    If IsArray(m_vSourceArray) = False Then
    '        GetSource = PMFalse
    '        LogMessagePopup _
    ''            iType:=PMLogOnError, _
    ''            sMsg:="Failed to get valid Branches", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="GetSource", _
    ''            vErrNo:=Err.Number, _
    ''            vErrDesc:=Err.Description
    '    End If
    '    lSources = UBound(m_vSourceArray, 2)
    '    If lSources = 1 Then
    '        iSourceID = CInt(m_vSourceArray(1, 1))
    '        If IsMissing(vSourceCode) = False Then
    '            vSourceCode = m_vSourceArray(2, 1)
    '        End If
    '        If IsMissing(vSourceName) = False Then
    '            vSourceName = m_vSourceArray(3, 1)
    '        End If
    '    Else
    '        m_lReturn = ProcessInterface(vSourceArray:=m_vSourceArray, iSourceID:=iSourceID)
    '        iSourceID = m_iSourceID
    '        If IsMissing(vSourceCode) = False Then
    '            vSourceCode = m_sSourceCode
    '        End If
    '        If IsMissing(vSourceName) = False Then
    '            vSourceName = m_sSourceName
    '        End If
    '    End If
    '    Exit Function
    '
    'Err_GetSource:
    '
    '    ' Error Section.
    '
    '    GetSource = PMError
    '
    '    ' Log Error.
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Failed to start the object", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="GetSource", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function
    ' ***************************************************************** '
    ' Name: GetSource (Standard Method)
    '
    ' Description: Select Source ID.
    '
    ' ***************************************************************** '
    Public Function GetSource(ByRef iSourceID As Integer, Optional ByRef vSourceCode As String = "", Optional ByRef vSourceName As String = "", Optional ByRef iCountryID As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim lSources As Integer
        Dim sSourceCode As String = ""
        Dim sSourceName As String = ""
        Dim sValue As String = ""
        'PN29502
        'Dim iCountryID As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetUserSources(r_vSourceArray:=m_vSourceArray, v_vUserID:=g_iUserID, lProductID:=m_lProductId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(m_vSourceArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("iSourceID", iSourceID)
                oDict.Add("vSourceCode", vSourceCode)
                oDict.Add("iCountryID", iCountryID)
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get valid Branches", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSource", oDicParms:=oDict)

                Return result
            End If

            'If we are passed a source ID just return the text info
            If iSourceID > 0 Then
                'developer guide no. 162
                For lSub As Integer = m_vSourceArray.GetLowerBound(1) To m_vSourceArray.GetUpperBound(1)
                    If iSourceID = CInt(m_vSourceArray(0, lSub)) Then

                        If Not Information.IsNothing(vSourceCode) Then
                            vSourceCode = CStr(m_vSourceArray(1, lSub))
                        End If

                        If Not Information.IsNothing(vSourceName) Then
                            vSourceName = CStr(m_vSourceArray(2, lSub))
                        End If
                        'PN29502
                        If Not False Then
                            iCountryID = CInt(m_vSourceArray(3, lSub))
                        End If
                    End If
                Next lSub
                Return result
            End If

            'See if we are multi tree accounting
            m_lReturn = CType(iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=g_iSourceID, r_vUnderwriting:=sValue), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for Multi Tree Accounting", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSource")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sValue = "1" Then
                'Multi tree accounting
                iSourceID = g_iSourceID
                Return result
            End If

            'We are NOT multi tree accounting
            lSources = m_vSourceArray.GetUpperBound(1)
            'developer guide no. 162
            If lSources = 0 Then
                'developer guide no. 162
                iSourceID = CInt(m_vSourceArray(0, 0))

                If Not Information.IsNothing(vSourceCode) Then
                    'developer guide no. 162
                    vSourceCode = CStr(m_vSourceArray(1, 0))
                End If

                If Not Information.IsNothing(vSourceName) Then
                    'developer guide no. 162
                    vSourceName = CStr(m_vSourceArray(2, 0))
                End If
                If Not False Then
                    'developer guide no. 162
                    iCountryID = CInt(m_vSourceArray(3, 0))
                End If

            Else
                'PN29502
                m_lReturn = CType(ProcessInterface(vSourceArray:=m_vSourceArray, iSourceID:=iSourceID, sSourceCode:=sSourceCode, sSourceName:=sSourceName, iCountryID:=iCountryID), gPMConstants.PMEReturnCode)
                iSourceID = m_iSourceID

                If Not Information.IsNothing(vSourceCode) Then
                    vSourceCode = sSourceCode
                End If

                If Not Information.IsNothing(vSourceName) Then
                    vSourceName = sSourceName
                End If
                'PN29502
                'DC240706 as parameter changed to integer now not required
                'vCountryID = iCountryID
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSource", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' RKS PN14431 01092004
    ' Name: GetSourceAccounts (Standard Method)
    '
    ' Description: Select Source ID.
    '              Implementing allow accounts on closed branches
    '              This function will allow to display and select the
    '              closed branches when allow accounting is enabled
    '
    ' ***************************************************************** '
    Public Function GetSourceAccounts(ByRef iSourceID As Integer, Optional ByRef vSourceCode As String = "", Optional ByRef vSourceName As String = "") As Integer
        Dim result As Integer = 0
        Dim lSources As Integer
        Dim sSourceCode As String = ""
        Dim sSourceName As String = ""
        Dim sValue As String = ""
        Dim vSourceArray(,) As Object = Nothing

        Dim oPMSource As bPMSource.Business
        Dim vIsDeleted As gPMConstants.PMEReturnCode
        Dim vAllowAccounts As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetUserSources(r_vSourceArray:=m_vSourceArray, v_vUserID:=g_iUserID, v_bIncludeDeletedSources:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(m_vSourceArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("iSourceID", iSourceID)
                oDict.Add("vSourceCode", vSourceCode)
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get valid Branches", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceAccounts", oDicParms:=oDict)

                Return result
            End If


            ' Get an instance of the business object via
            ' the object manager.
            Dim temp_oPMSource As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMSource, "bPMSource.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMSource = temp_oPMSource
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("iSourceID", iSourceID)
                oDict.Add("vSourceCode", vSourceCode)
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create an object of bPMSource.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceAccounts", oDicParms:=oDict)

                Return result
            End If



            'Filtering for valid sources (Branches)
            'Extracting the valid sources from m_vSourceArray to vSourceArray
            'don't extract if a Branch isdeleted and allow accounting is disabled
            'developer guide no. 162
            For lSub As Integer = m_vSourceArray.GetLowerBound(1) To m_vSourceArray.GetUpperBound(1)

                m_lReturn = oPMSource.GetDetails(vSourceID:=m_vSourceArray(0, lSub))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                    oDict.Add("iSourceID", iSourceID)
                    oDict.Add("vSourceCode", vSourceCode)
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to run oPMSource.GetDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceAccounts", oDicParms:=oDict)

                    Return result
                End If


                m_lReturn = oPMSource.GetNext(vIsDeleted:=vIsDeleted, vAllowAccounts:=vAllowAccounts)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                    oDict.Add("iSourceID", iSourceID)
                    oDict.Add("vSourceCode", vSourceCode)
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to run oPMSource.GetNext", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceAccounts", oDicParms:=oDict)

                    Return result
                End If

                If (vIsDeleted = gPMConstants.PMEReturnCode.PMFalse) Or (vIsDeleted = gPMConstants.PMEReturnCode.PMTrue And vAllowAccounts = gPMConstants.PMEReturnCode.PMTrue) Then
                    'developer guide no. 162 & referred VB Code.
                    If lSub = 0 Then
                        ReDim vSourceArray(2, 0)
                    Else
                        ReDim Preserve vSourceArray(2, vSourceArray.GetUpperBound(1) + 1)
                    End If



                    vSourceArray(0, vSourceArray.GetUpperBound(1)) = m_vSourceArray(0, lSub)


                    vSourceArray(1, vSourceArray.GetUpperBound(1)) = m_vSourceArray(1, lSub)


                    vSourceArray(2, vSourceArray.GetUpperBound(1)) = m_vSourceArray(2, lSub)
                Else
                    If lSub = 0 Then
                        ReDim vSourceArray(2, 0)
                    End If
                End If

            Next lSub



            oPMSource.Dispose()
            'updating with valid SourceArray

            m_vSourceArray = vSourceArray


            'If we are passed a source ID just return the text info
            If iSourceID > 0 Then
                'developer guide no. 162
                For lSub As Integer = m_vSourceArray.GetLowerBound(1) To m_vSourceArray.GetUpperBound(1)
                    If iSourceID = CInt(m_vSourceArray(0, lSub)) Then

                        If Not Information.IsNothing(vSourceCode) Then
                            vSourceCode = CStr(m_vSourceArray(1, lSub))
                        End If

                        If Not Information.IsNothing(vSourceName) Then
                            vSourceName = CStr(m_vSourceArray(2, lSub))
                        End If
                    End If
                Next lSub
                Return result
            End If

            'See if we are multi tree accounting
            m_lReturn = CType(iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=g_iSourceID, r_vUnderwriting:=sValue), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for Multi Tree Accounting", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceAccounts")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sValue = "1" Then
                'Multi tree accounting
                iSourceID = g_iSourceID
                Return result
            End If


            'We are NOT multi tree accounting
            lSources = m_vSourceArray.GetUpperBound(1)
            'developer guide no. 162
            If lSources = 0 Then
                'developer guide no. 162
                iSourceID = CInt(m_vSourceArray(0, 0))

                If Not Information.IsNothing(vSourceCode) Then
                    'developer guide no. 162
                    vSourceCode = CStr(m_vSourceArray(1, 0))
                End If

                If Not Information.IsNothing(vSourceName) Then
                    'developer guide no. 162
                    vSourceName = CStr(m_vSourceArray(2, 0))
                End If
            Else
                m_lReturn = CType(ProcessInterface(vSourceArray:=m_vSourceArray, iSourceID:=iSourceID, sSourceCode:=sSourceCode, sSourceName:=sSourceName), gPMConstants.PMEReturnCode)
                iSourceID = m_iSourceID

                If Not Information.IsNothing(vSourceCode) Then
                    vSourceCode = sSourceCode
                End If

                If Not Information.IsNothing(vSourceName) Then
                    vSourceName = sSourceName
                End If
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceAccounts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' ***************************************************************** '
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' ***************************************************************** '
    Private Function ProcessInterface(ByVal vSourceArray As Object, ByRef iSourceID As Integer, ByRef sSourceCode As String, ByRef sSourceName As String, Optional ByRef iCountryID As Integer = 0) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load the interface into memory.
        m_lReturn = CType(LoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Display the interface.
        m_lReturn = CType(ShowInterface(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to display the inteface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Destroy the interface from memory.
        m_lReturn = CType(UnLoadInterface(), gPMConstants.PMEReturnCode)

        iSourceID = m_iSourceID
        sSourceCode = m_sSourceCode
        sSourceName = m_sSourceName
        'PN29502
        iCountryID = m_iCountryID
        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to unload the interface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: LoadInterface (Standard Method)
    '
    ' Description: Loads the instance of the interface into memory and
    '              passes the parameters in.
    '
    ' ***************************************************************** '
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_frmInterface = New frmInterface()

        ' Assign the parameters to the interface properties.
        With m_frmInterface
            .CallingAppName = m_sCallingAppName
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate

            ' {* USER DEFINED CODE (Begin) *}
            'developer guide no 24. 
            .SourceArray = m_vSourceArray

            'TN20010706 start
            .PartyCnt = m_lPartyCnt
            'TN20010706 end

            ' {* USER DEFINED CODE (End) *}
        End With

        ' Load the instance of the interface into memory.

        ' Check if we have had an error so far.
        If m_frmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = m_frmInterface.ErrorNumber
        End If

        ' Set the status in the interface.
        m_lReturn = CType(m_frmInterface.SetStatus(sProcessStatus:=m_sProcessStatus, sMapStatus:=m_sMapStatus, sStepStatus:=m_sStepStatus), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to set the status.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UnLoadInterface (Standard Method)
    '
    ' Description: Unloads the instance of the interface from memory.
    '
    ' ***************************************************************** '
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        With m_frmInterface
            m_lStatus = .Status
            m_sStepStatus = .StepStatus

            ' {* USER DEFINED CODE (Begin) *}
            m_iSourceID = .SourceID
            m_sSourceCode = .SourceCode
            m_sSourceName = .SourceName
            'DC240706 pass back countryid
            m_iCountryID = .CountryID

            ' {* USER DEFINED CODE (End) *}
        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        m_frmInterface.Close()
        m_frmInterface = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ShowInterface (Standard Method)
    '
    ' Description: Displays the instance of the interface using the
    '              display state.
    '
    ' ***************************************************************** '
    Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display the interface.
        VB6.ShowForm(m_frmInterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            If m_frmInterface.ErrorNumber <> 0 Then
                result = m_frmInterface.ErrorNumber
            End If
        End If

        Return result

    End Function
    Private Shared _DefaultInstance As Interface_Renamed = Nothing
    Public Shared ReadOnly Property DefaultInstance() As Interface_Renamed
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New Interface_Renamed
            End If
            Return _DefaultInstance
        End Get
    End Property
End Class

