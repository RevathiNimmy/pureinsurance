Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("iPMBFieldManager.Interface_Renamed")> _
<ComClass(Interface_Renamed.ClassId, Interface_Renamed.InterfaceId, Interface_Renamed.EventsId)> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
#Region "COM GUIDs"
    ' These  GUIDs provide the COM identity for this class 
    ' and its COM interfaces. If you change them, existing 
    ' clients will no longer be able to access the class.
    Public Const ClassId As String = "a2785f37-d967-4925-96b1-8713b2d5f804"
    Public Const InterfaceId As String = "5e9f4a90-4cce-4275-b7d6-0de8abc4061c"
    Public Const EventsId As String = "c57e761f-9842-4b36-b1c0-207f3ff3cfd8"
#End Region

    ' A creatable COM class must have a Public Sub New() 
    ' with no parameters, otherwise, the class will not be 
    ' registered in the COM registry and cannot be created 
    ' via CreateObject.
    Public Sub New()
        MyBase.New()
    End Sub

    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 30/07/1997
    '
    ' Description: Interface Class
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Reference to Object Manager
    Private m_oObjectManager As bObjectManager.ObjectManager

    ' Reference to Business object

    Private m_oBusiness As bSIRFieldManager.Business

    ' Reference to the Interface form
    Private m_frmInterface As frmInterface

    ' Return value
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_bCalledFromSwift As Boolean ' RAM20050104 - Added for Swift


    Public WriteOnly Property Visible() As Boolean
        Set(ByVal Value As Boolean)

            If Not (m_frmInterface Is Nothing) Then
                m_frmInterface.FormVisible = Value
            End If

        End Set
    End Property

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0



        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            m_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = m_oObjectManager.Initialise(sCallingAppName:=ACApp)

            If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                Return gPMConstants.PMEReturnCode.PMCancel
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                m_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With m_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With

            m_dtEffectiveDate = DateTime.Now

            ' Get an instance of the business object via
            ' the public object manager.

            ' REMOTE
            Dim temp_m_oBusiness As Object
            m_lReturn = m_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRFieldManager.Business", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness

            ' LOCAL
            'm_lReturn& = m_oObjectManager.GetInstance( _
            'oObject:=m_oBusiness, _
            'sClassName:="bFieldManager.Business")

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")


                Return result
            End If

            'm_lReturn& = m_oBusiness.Initialise("sa", "", 1, 1, 1, 1, 1, "test")

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
                If Not (m_frmInterface Is Nothing) Then
                    m_frmInterface.FormVisible = False
                    m_frmInterface.Close()
                    m_frmInterface = Nothing
                End If
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If
                If m_oObjectManager IsNot Nothing Then
                    m_oObjectManager.Dispose()
                    m_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub



    ' Set up the Application object
    Public Function SetApp(ByRef oApp As Object, ByRef sAppName As String) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get reference to the Application
        g_oCallingApp = oApp

        ' Load an interface form
        m_frmInterface = New frmInterface()

        m_frmInterface.tvwFields.SuspendLayout()
        'TODO
        'Load(m_frmInterface)
        m_frmInterface.frmInterfaceLoad()

        ' Get the Application Name
        g_sCallingAppName = sAppName

        ' RAM20050104 - Added for Swift
        m_frmInterface.CalledFromSwift = m_bCalledFromSwift

        'DN 07/01/02 - Use new FillFieldTree if Underwriting

        If m_oBusiness.UnderwritingOrAgency = "U" Then
            ' Fill the Field Tree on the form for Underwriting
            m_lReturn = CType(m_frmInterface.FillFieldTree(m_oBusiness), gPMConstants.PMEReturnCode)
            SSTabHelper.SetTabCaption(m_frmInterface.tabFields, 4, "4 " & SSTabHelper.GetTabCaption(m_frmInterface.tabFields, 4))
        Else
            ' Fill the Field Tree on the form for SBO
            m_lReturn = CType(m_frmInterface.FillFieldTreeCool(m_oBusiness), gPMConstants.PMEReturnCode)
            SSTabHelper.SetTabVisible(m_frmInterface.tabFields, 3, False)
            m_frmInterface.fraRiskLoop.Visible = False
            m_frmInterface.Frame3.Visible = False
            SSTabHelper.SetTabCaption(m_frmInterface.tabFields, 4, "3 " & SSTabHelper.GetTabCaption(m_frmInterface.tabFields, 4))
        End If

        If m_bCalledFromSwift Then
            'We don 't need to display these tabs if we called from SWIFT
            SSTabHelper.SetTabVisible(m_frmInterface.tabFields, 1, False)
            SSTabHelper.SetTabVisible(m_frmInterface.tabFields, 2, False)
            SSTabHelper.SetTabVisible(m_frmInterface.tabFields, 3, False)
            SSTabHelper.SetTabCaption(m_frmInterface.tabFields, 4, "2 " & SSTabHelper.GetTabCaption(m_frmInterface.tabFields, 4))
        Else
            ' We don't need to fill the ClauseList if we called from SWIFT
            ' Fill the Clause List on the form
            m_lReturn = CType(m_frmInterface.FillClauseList(m_oBusiness), gPMConstants.PMEReturnCode)
        End If

        ' Now Populate the Sub-Documents List
        m_lReturn = CType(m_frmInterface.FillSubDocumentsList(m_oBusiness), gPMConstants.PMEReturnCode)

        'RWH(24/08/2000) RSAIB Process 12
        'Create array of names of existing loops in template.
        'm_lReturn = CType(frmInterface.GetAllLoopsInTemplate(), gPMConstants.PMEReturnCode)
        'm_lReturn = CType(frmInterface.GetAllLoopsInTemplate(), gPMConstants.PMEReturnCode)
        m_lReturn = CType(m_frmInterface.GetAllLoopsInTemplate(), gPMConstants.PMEReturnCode)
        m_frmInterface.tvwFields.ResumeLayout(False)
        m_frmInterface.tvwFields.PerformLayout()
        Return result
    End Function

    ' ***************************************************************** '
    ' Name          : SetKeys (Standard Method)
    '
    ' Description   : Stores all of the parameter members with the key
    '                   array.
    ' Edit History  :
    ' RAM20050104   : Added
    ' ***************************************************************** '
    Public Function SetKeys(ByVal vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

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


                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNameCalledFromSwift

                        m_bCalledFromSwift = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                End Select

                ' {* USER DEFINED CODE (End) *}
            Next lRow

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
