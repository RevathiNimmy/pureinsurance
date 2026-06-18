Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 07/01/1999
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    '
    ' CJB 26/04/2004 - Added support for 13th & 14th report parameters
    ' ***************************************************************** '
    'Developer Guide No. 69
    Public frmInterface As frmInterface

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"
    Public m_sOutputPath As String = ""
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Public m_sReportType As String = ""
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""

    Private m_lPMAuthorityLevel As Integer

    ' {* USER DEFINED CODE (Begin) *}
    Private m_sReportName As String = ""
    Private m_sDescription As String = String.Empty
    Private m_iPrintReport As Integer

    Private m_oBusiness As Object

    ' Parameter values passed from Navigator
    Private m_sReportParam1Name As String = ""
    Private m_sReportParam1Value As String = ""
    Private m_sReportParam2Name As String = ""
    Private m_sReportParam2Value As String = ""
    Private m_sReportParam3Name As String = ""
    Private m_sReportParam3Value As String = ""
    Private m_sReportParam4Name As String = ""
    Private m_sReportParam4Value As String = ""
    Private m_sReportParam5Name As String = ""
    Private m_sReportParam5Value As String = ""
    Private m_sReportParam6Name As String = ""
    Private m_sReportParam6Value As String = ""
    Private m_sReportParam7Name As String = ""
    Private m_sReportParam7Value As String = ""
    'TF051202
    Private m_sReportParam8Name As String = ""
    Private m_sReportParam8Value As String = ""
    Private m_sReportParam9Name As String = ""
    Private m_sReportParam9Value As String = ""
    Private m_sReportParam10Name As String = ""
    Private m_sReportParam10Value As String = ""
    Private m_sReportParam11Name As String = ""
    Private m_sReportParam11Value As String = ""
    'DC270303 -ISS1911
    Private m_sReportParam12Name As String = ""
    Private m_sReportParam12Value As String = ""
    'CJB260404 - Folgate Dev Work
    Private m_sReportParam13Name As String = ""
    Private m_sReportParam13Value As String = ""
    Private m_sReportParam14Name As String = ""
    Private m_sReportParam14Value As String = ""
    Private m_sReportParam15Name As String = ""
    Private m_sReportParam15Value As String = ""

    '31/10/2002 - PWC - Added
    Private m_sFilterName As String = ""
    Private m_sFilterValue As String = ""

    Private m_bSaveParams As Boolean

    'Array to hold all Parameter values
    Private m_vParameters As Object

    Private m_vKeyDefaults As Object 'Array to hold passed default values
    Private m_vKeyPrompts(,) As Object 'Array to hold passed prompt values

    Private m_lCashListId As Integer

    ' {* USER DEFINED CODE (End) *}

    ' Stores the exit status of the interface.
    Private m_lStatus As Integer

    Private m_bAttachToScheduler As Boolean

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public Property ReportOutputPath() As String
        Get

            Return m_sOutputPath

        End Get
        Set(ByVal value As String)
            m_sOutputPath = value
        End Set
    End Property
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            m_lPMAuthorityLevel = Value

        End Set
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property
    Public WriteOnly Property ReportType() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sReportType = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    '8.5
    Public Property AttachToScheduler() As Boolean
        Get
            Return m_bAttachToScheduler
        End Get
        Set(ByVal Value As Boolean)
            m_bAttachToScheduler = Value
        End Set
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
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
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim sTitle As String
        Dim sMessage As String

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

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                'JMK 16/06/2001
                g_iUserID = .UserID
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            m_lReturn = g_oObjectManager.GetInstance( _
                oObject:=m_oBusiness, _
                sClassName:="bSIRReportPrint.Business", _
                vInstanceManager:=PMGetViaClientManager)

            ' Check for errors.
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to get an instance of the business object.
                Initialise = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.
                sTitle$ = iPMFunc.GetResData( _
                    iLangID:=g_iLanguageID%, _
                    lId:=ACBusinessFailTitle, _
                    iDataType:=gPMConstants.PMEResourseFileDataType.PMResString)

                sMessage$ = iPMFunc.GetResData( _
                    iLangID:=g_iLanguageID%, _
                    lId:=ACBusinessFail, _
                    iDataType:=gPMConstants.PMEResourseFileDataType.PMResString)

                ' Display message.
                MsgBox(sMessage$, vbCritical, sTitle$)

                Exit Function
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    '
    ' CJB 260404 - Added support for 13th & 14th report params
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lParamCount As Integer = -1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                ' Assign the parameter member with the
                ' correct key array item.

                ' {* USER DEFINED CODE (Begin) *}

                If Not vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow) Is Nothing Then
                    Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                        ' Specific report to be processed (eg. from Navigator)
                        Case PMNavKeyConst.PMKeyNameReportName

                            m_sReportName = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            ' 0=View, 1=Print, 2=View+Print, 3=Export to HTML
                        Case PMNavKeyConst.PMKeyNamePrintReport

                            m_iPrintReport = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            'added to print report as .doc
                            'Case PMNavKeyConst.PMKeyNameReportType

                            '    ' m_sCallingAppName = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            '    m_sReportType = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            '    ' 0=View, 1=Print, 2=View+Print, 3=Export to HTML
                            '    ' Specific value for parameter1
                        Case PMNavKeyConst.PMKeyNameParam1Name
                            'Wouldn't this be a good idea for the other parameters?
                            lParamCount += 1
                            ' This key value = name of parameter1

                            m_sReportParam1Name = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            ' Loop thro' keys to find key name = name of parameter1
                            For iCount As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                                If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iCount)) = m_sReportParam1Name Then
                                    'If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iCount)) = m_sReportParam1Name Then

                                    m_sReportParam1Value = gPMFunctions.NullToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iCount))
                                    Exit For
                                End If
                            Next iCount
                            ' Add to parameters array
                            ReDim m_vParameters(1, lParamCount)

                            m_vParameters(0, lParamCount) = m_sReportParam1Name

                            m_vParameters(1, lParamCount) = m_sReportParam1Value
                        Case PMNavKeyConst.PMKeyNameParam2Name
                            'Pon my soul - Yes it would!
                            lParamCount += 1

                            m_sReportParam2Name = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            For iCount As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                                If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iCount)) = m_sReportParam2Name Then

                                    m_sReportParam2Value = gPMFunctions.NullToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iCount))
                                    Exit For
                                End If
                            Next iCount
                            ReDim Preserve m_vParameters(1, lParamCount)

                            m_vParameters(0, lParamCount) = m_sReportParam2Name

                            m_vParameters(1, lParamCount) = m_sReportParam2Value

                        Case PMNavKeyConst.PMKeyNameParam3Name
                            lParamCount += 1

                            m_sReportParam3Name = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            For iCount As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                                If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iCount)) = m_sReportParam3Name Then

                                    m_sReportParam3Value = gPMFunctions.NullToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iCount))
                                    Exit For
                                End If
                            Next iCount
                            ReDim Preserve m_vParameters(1, lParamCount)

                            m_vParameters(0, lParamCount) = m_sReportParam3Name

                            m_vParameters(1, lParamCount) = m_sReportParam3Value

                        Case PMNavKeyConst.PMKeyNameParam4Name
                            lParamCount += 1

                            m_sReportParam4Name = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            For iCount As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                                If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iCount)) = m_sReportParam4Name Then

                                    m_sReportParam4Value = gPMFunctions.NullToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iCount))
                                    Exit For
                                End If
                            Next iCount
                            ReDim Preserve m_vParameters(1, lParamCount)

                            m_vParameters(0, lParamCount) = m_sReportParam4Name

                            m_vParameters(1, lParamCount) = m_sReportParam4Value

                        Case PMNavKeyConst.PMKeyNameParam5Name
                            lParamCount += 1

                            m_sReportParam5Name = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            For iCount As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                                If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iCount)) = m_sReportParam5Name Then

                                    m_sReportParam5Value = gPMFunctions.NullToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iCount))
                                    Exit For
                                End If
                            Next iCount
                            ReDim Preserve m_vParameters(1, lParamCount)

                            m_vParameters(0, lParamCount) = m_sReportParam5Name

                            m_vParameters(1, lParamCount) = m_sReportParam5Value

                        Case PMNavKeyConst.PMKeyNameParam6Name
                            lParamCount += 1

                            m_sReportParam6Name = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            For iCount As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                                If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iCount)) = m_sReportParam6Name Then

                                    m_sReportParam6Value = gPMFunctions.NullToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iCount))
                                    Exit For
                                End If
                            Next iCount
                            ReDim Preserve m_vParameters(1, lParamCount)

                            m_vParameters(0, lParamCount) = m_sReportParam6Name

                            m_vParameters(1, lParamCount) = m_sReportParam6Value

                        Case PMNavKeyConst.PMKeyNameParam7Name
                            lParamCount += 1

                            m_sReportParam7Name = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            For iCount As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                                If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iCount)) = m_sReportParam7Name Then

                                    m_sReportParam7Value = gPMFunctions.NullToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iCount))
                                    Exit For
                                End If
                            Next iCount
                            ReDim Preserve m_vParameters(1, lParamCount)

                            m_vParameters(0, lParamCount) = m_sReportParam7Name

                            m_vParameters(1, lParamCount) = m_sReportParam7Value

                            'TF121102 - Increased to match frmParameters
                        Case PMNavKeyConst.PMKeyNameParam8Name
                            lParamCount += 1

                            m_sReportParam8Name = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            For iCount As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                                If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iCount)) = m_sReportParam8Name Then

                                    m_sReportParam8Value = gPMFunctions.NullToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iCount))
                                    Exit For
                                End If
                            Next iCount
                            ReDim Preserve m_vParameters(1, lParamCount)

                            m_vParameters(0, lParamCount) = m_sReportParam8Name

                            m_vParameters(1, lParamCount) = m_sReportParam8Value

                        Case PMNavKeyConst.PMKeyNameParam9Name
                            lParamCount += 1

                            m_sReportParam9Name = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            For iCount As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                                If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iCount)) = m_sReportParam9Name Then

                                    m_sReportParam9Value = gPMFunctions.NullToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iCount))
                                    Exit For
                                End If
                            Next iCount
                            ReDim Preserve m_vParameters(1, lParamCount)

                            m_vParameters(0, lParamCount) = m_sReportParam9Name

                            m_vParameters(1, lParamCount) = m_sReportParam9Value

                        Case PMNavKeyConst.PMKeyNameParam10Name
                            lParamCount += 1

                            m_sReportParam10Name = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            For iCount As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                                If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iCount)) = m_sReportParam10Name Then

                                    m_sReportParam10Value = gPMFunctions.NullToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iCount))
                                    Exit For
                                End If
                            Next iCount
                            ReDim Preserve m_vParameters(1, lParamCount)

                            m_vParameters(0, lParamCount) = m_sReportParam10Name

                            m_vParameters(1, lParamCount) = m_sReportParam10Value

                        Case PMNavKeyConst.PMKeyNameParam11Name
                            lParamCount += 1

                            m_sReportParam11Name = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            For iCount As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                                If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iCount)) = m_sReportParam11Name Then

                                    m_sReportParam11Value = gPMFunctions.NullToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iCount))
                                    Exit For
                                End If
                            Next iCount
                            ReDim Preserve m_vParameters(1, lParamCount)

                            m_vParameters(0, lParamCount) = m_sReportParam11Name

                            m_vParameters(1, lParamCount) = m_sReportParam11Value

                        Case PMNavKeyConst.PMKeyNameParam12Name
                            lParamCount += 1

                            m_sReportParam12Name = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            For iCount As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                                If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iCount)) = m_sReportParam12Name Then

                                    m_sReportParam12Value = gPMFunctions.NullToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iCount))
                                    Exit For
                                End If
                            Next iCount
                            ReDim Preserve m_vParameters(1, lParamCount)

                            m_vParameters(0, lParamCount) = m_sReportParam12Name

                            m_vParameters(1, lParamCount) = m_sReportParam12Value

                            ' CJB 260404 - Added support for 13th & 14th report params
                        Case PMNavKeyConst.PMKeyNameParam13Name
                            lParamCount += 1

                            m_sReportParam13Name = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            For iCount As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                                If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iCount)) = m_sReportParam13Name Then

                                    m_sReportParam13Value = gPMFunctions.NullToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iCount))
                                    Exit For
                                End If
                            Next iCount
                            ReDim Preserve m_vParameters(1, lParamCount)

                            m_vParameters(0, lParamCount) = m_sReportParam13Name

                            m_vParameters(1, lParamCount) = m_sReportParam13Value

                        Case PMNavKeyConst.PMKeyNameParam14Name
                            lParamCount += 1

                            m_sReportParam14Name = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            For iCount As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                                If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iCount)) = m_sReportParam14Name Then

                                    m_sReportParam14Value = gPMFunctions.NullToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iCount))
                                    Exit For
                                End If
                            Next iCount
                            ReDim Preserve m_vParameters(1, lParamCount)

                            m_vParameters(0, lParamCount) = m_sReportParam14Name

                            m_vParameters(1, lParamCount) = m_sReportParam14Value

                        Case PMNavKeyConst.PMKeyNameParam15Name
                            lParamCount += 1

                            m_sReportParam15Name = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            For iCount As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                                If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iCount)) = m_sReportParam15Name Then

                                    m_sReportParam15Value = gPMFunctions.NullToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iCount))
                                    Exit For
                                End If
                            Next iCount
                            ReDim Preserve m_vParameters(1, lParamCount)

                            m_vParameters(0, lParamCount) = m_sReportParam15Name

                            m_vParameters(1, lParamCount) = m_sReportParam15Value

                            '31/10/2002 - PWC - Added check for new Key
                        Case PMKeyNameFilterReports
                            ' Caller specifed a filter so set the name

                            m_sFilterName = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            'Loop through the keys to set the value for this filter type
                            For iCount As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                                If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iCount)) = m_sFilterName Then

                                    m_sFilterValue = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iCount))
                                    Exit For
                                End If
                            Next iCount

                            '31/10/2002 - PWC - Caller must explicitly request to save params
                            '(Done this as to not alter the existing code path)
                        Case PMKeyNameSaveParams
                            m_bSaveParams = True

                        Case PMNavKeyConst.PMKeyNameKeyPrompts

                            m_vKeyPrompts = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)

                        Case PMNavKeyConst.ACTKeyNameCashListId

                            m_lCashListId = gPMFunctions.ToSafeLong(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    End Select
                End If
                ' {* USER DEFINED CODE (End) *}
            Next lRow

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vKeyArray(1, 0)

            ' Assign the key array with the parameter members.
            '    vKeyArray(PMKeyName, 0) = PMKeyNameID
            '    vKeyArray(PMKeyValue, 0) = m_iNameID%

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary (Standard Method)
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0
        Try

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Starts the interface processing.
            m_lReturn = ProcessInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0


        Const AC_REPORT_REMITTANCEADVICE As String = "remittanceadvice"
        Const AC_REPORT_NZ_SUFFIX As String = "_NZ"

        Dim vValue As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        'S4BDAT009 - use New Zealand specific report
        If m_sReportName.ToLower() = AC_REPORT_REMITTANCEADVICE Then
            m_lReturn = GenerateBrokerlinkEBordereau(v_lCashListId:=m_lCashListId)
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTNewZealandConfiguration, v_vBranch:=gPMConstants.SIRBCHHeadOffice, r_vUnderwriting:=vValue)
            m_sReportName = m_sReportName & (IIf(gPMFunctions.ToSafeString(vValue, "0") = "1", AC_REPORT_NZ_SUFFIX, ""))
        End If

        ' Load the interface into memory.
        m_lReturn = LoadInterface()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' May have updated report name (Navigator)
        m_sReportName = frmInterface.ReportName

        ' Produce Report without displaying Interface
        ' If report already known
        If m_sReportName > "" Then
            m_lReturn = frmInterface.ProduceReport()
        Else
            ' Display the interface.
            m_lReturn = ShowInterface(lDisplayState:=FormShowConstants.Modal)
        End If
        ReportOutputPath = m_sUserReportName
        ' Check for errors. (Alix: added: ignore if preview not wanted by user)
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
            ' Failed to display the inteface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Destroy the interface from memory.
        m_lReturn = UnLoadInterface()

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
        'Developer Guide No. 50
        frmInterface = New frmInterface
        ' Assign the parameters to the interface properties.
        With frmInterface
            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate

            ' {* USER DEFINED CODE (Begin) *}
            .ReportName = m_sReportName
            .Description = m_sDescription
            .PrintReport = m_iPrintReport

            'Developer Guide No. 24
            '.set_Parameters(m_vParameters)
            .Parameters = m_vParameters

            '31/10/2002 - PWC - Added
            .FilterName = m_sFilterName
            .FilterValue = m_sFilterValue
            .SaveParams = m_bSaveParams
            'Developer Guide No.
            '.set_KeyPrompts(m_vKeyPrompts)
            .KeyPrompts = VB6.CopyArray(m_vKeyPrompts)
            '8.5
            .AttachToScheduler = m_bAttachToScheduler

            If m_sReportName.Length > 0 Then
                .CallLoad()
            End If
            .Business = m_oBusiness
            ' {* USER DEFINED CODE (End) *}
        End With

        ' Load the instance of the interface into memory.
        ' Dim tempLoadForm As frmInterface = frmInterface
        ' tempLoadForm.Show()
        ' Check if we have had an error so far.
        If frmInterface.ErrorNumber <> 0 Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = frmInterface.ErrorNumber
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
        With frmInterface
            m_lStatus = .Status

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}
        End With

        ' Unload and destroy the instance of the interface
        ' from memory.

        frmInterface.Close()
        frmInterface = Nothing

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
        VB6.ShowForm(frmInterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            If frmInterface.ErrorNumber <> 0 Then
                result = frmInterface.ErrorNumber
            End If
        End If

        Return result

    End Function
    'PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    Private Function GenerateBrokerlinkEBordereau(ByVal v_lCashListId As Integer) As Integer
        Dim result As Integer = 0
        Dim oQem As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_oQem As Object = Nothing
        m_lReturn = g_oObjectManager.GetInstance(temp_oQem, "bGisQemBl.Sbo", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oQem = temp_oQem

        m_lReturn = oQem.GenerateEBordereau(v_lCashListId:=v_lCashListId)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bGisQemBl to GenerateEBordereau", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateBrokerlinkEBordereau", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
        End If

        oQem.Dispose()
        oQem = Nothing

        Return result

    End Function
End Class
