Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("interface_Renamed_NET.interface_Renamed")> _
Public NotInheritable Class interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 4/2/98
    '
    ' Description: Main public class to accompany the interface form.
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


    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    'ND 071100 - So scan station can be hidden to choose new folder to scan to
    Private m_bHiddenForFolderSelect As Boolean
    Private frmInterface As frmInterface

    Public ReadOnly Property HiddenForFolderSelect() As Boolean
        Get

            Return m_bHiddenForFolderSelect

        End Get
    End Property
    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef bStandAlone As Boolean) As Integer
        Dim result As Integer = 0

        Try

            ' Initialise the splash object
            g_oSplash = New iDOCSplash.Interface_Renamed()

            'm_lReturn = CType(g_oSplash, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            m_lReturn = g_oSplash.Initialise()

            result = gPMConstants.PMEReturnCode.PMTrue

            bIsStandAlone = bStandAlone

            ' Application not closing at the moment
            bAppClosing = False

            ' Create an instance of the object manager.
#If PD_EARLYBOUND = 1 Then

			Set g_oObjectManager = New bObjectManager.ObjectManager
#Else
            g_oObjectManager = New bObjectManager.ObjectManager()
#End If

            ' Call the initialise method.
            m_lReturn = CType(g_oObjectManager.Initialise(sCallingAppName:=ACApp), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' User pressed Cancel when logging in?
                If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                    Return gPMConstants.PMEReturnCode.PMCancel
                End If

                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result

            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager

                g_iSourceID = .SourceID
                g_sUsername = .UserName
                g_iUserID = .UserID
                g_iSourceID = .SourceID
                g_iLanguageID = .LanguageID
                g_iCurrencyID = .CurrencyID
                g_iLogLevel = .LogLevel

            End With

            sCurrentUserName = g_sUsername
            iCurrentLogLevel = g_iLogLevel

            ' object manager creates it
            'Set m_oDOCScan = New bDOCScan.Form

            Dim temp_m_oDOCScan As Object
            m_lReturn = CType(g_oObjectManager.GetInstance(temp_m_oDOCScan, "bDOCScan.Form", vInstanceManager:=PMGetLocalBusiness), gPMConstants.PMEReturnCode)
            m_oDOCScan = temp_m_oDOCScan
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to initialise bDOCScan.", vApp:=ACApp, vClass:="interface", vMethod:="Initialise", excep:=New Exception(Information.Err().Description))
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDOCScan.PostInitialise(bStandAlone:=bIsStandAlone)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to post initialise bDOCScan.", vApp:=ACApp, vClass:="interface", vMethod:="Initialise", excep:=New Exception(Information.Err().Description))
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create an instance of the viewbatch class
            ' Set g_oViewBatch = New iDOCViewBatch.interface

            'RAM20021223 : Changed the early binding to Late Binding
            'TODO: Needs to be reffered after iDOCViewBatch is ready
            g_oViewBatch = New iDOCViewBatch.Interface_Renamed()

            ' Initialise it

            m_lReturn = g_oViewBatch.Initialise(bStandAlone:=bStandAlone)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to initialise View Batch object.", vApp:=ACApp, vClass:="interface", vMethod:="Initialise", excep:=New Exception(Information.Err().Description))
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Scan (Public)
    '
    ' Description: Initialises the scan form, and displays it modally
    '
    ' ND 071100    If form is just hidden to allow user to select a
    '              new scan folder do not unload/load form again
    ' ***************************************************************** '
    Public Function Scan(ByRef vFolderTree(,) As Object) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not bIsStandAlone Then

                If vFolderTree.GetUpperBound(1) > 0 Then
                    ' Get the folders

                    sCurrentFolder = CStr(vFolderTree(1, 0))

                    sParentFolder = CStr(vFolderTree(1, 1))
                Else
                    ' It has no parent folder, so just blank it out

                    sCurrentFolder = CStr(vFolderTree(1, 0))
                    sParentFolder = ""
                End If


                lFolderNumber = CInt(vFolderTree(0, 0))

            Else

                sCurrentFolder = "Scan Folder"
                sParentFolder = "N/A - Standalone"

                lFolderNumber = 0

            End If

            ' Reset the z
            g_lDocumentsScanned = 0
            g_lPagesScanned = 0
            g_lCurrentPage = 0

            ' Get this from the registry
            m_lReturn = GetDOCRegSettings(vScanDirectory:=g_sScanDirectory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Hide the splash screen
                m_lReturn = g_oSplash.Hide()

                LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="Scan", excep:=New Exception("Unable to get registry settings."))

                Return result

            End If

            ' Reset the error variable
            iScanError = gPMConstants.PMEReturnCode.PMFalse

            frmInterface = New frmInterface

            'DN 13/02/01 - Moved from start of section to here so it is after GetRegSettings
            m_bHiddenForFolderSelect = frmInterface.HiddenForFolderSelect

            If Not m_bHiddenForFolderSelect Then
                m_lReturn = g_oSplash.Show(DOCSplash_Message, "Loading DocuMaster ScanStation. Please wait...")
                Threading.Thread.Sleep(1000)
            End If

            ' Load and initialise the form
            If Not m_bHiddenForFolderSelect Then
                '  Dim tempLoadForm As frmInterface = frmInterface
                frmInterface.frmInterface_Load()
                Application.DoEvents()
            End If

            frmInterface.txtScanFolder.Text = sCurrentFolder
            frmInterface.txtParentFolderName.Text = sParentFolder

            'MS 310101 commit document into batch
            m_lReturn = CType(frmInterface.PrepareNextDocument(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' This error will have already been logged
                Return result
            End If

            ' Hide the splash screen
            m_lReturn = g_oSplash.Hide()

            ' If there wasnt an error then display the form
            If iScanError = gPMConstants.PMEReturnCode.PMFalse Then

                frmInterface.HiddenForFolderSelect = False

                frmInterface.ShowDialog()

                m_bHiddenForFolderSelect = frmInterface.HiddenForFolderSelect
                If Not m_bHiddenForFolderSelect Then
                    frmInterface.Close()
                End If

            Else
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="Scan", excep:=excep)


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Public)
    '
    ' Description: Entry point for any initialisation code for this
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
                If g_oSplash IsNot Nothing Then
                    g_oSplash.Dispose()

                End If
                g_oSplash = Nothing
                If g_oViewBatch IsNot Nothing Then
                    g_oViewBatch.Dispose()

                End If
                g_oViewBatch = Nothing
                If m_oDOCScan IsNot Nothing Then
                    m_oDOCScan.Dispose()
                End If
                m_oDOCScan = Nothing
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
                frmInterface = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub

End Class
