Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 4/2/98
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    '
    ' JH040199 new property UserIsAdministrator so iDOCMan can pass
    ' variable in. 'NEW' and 'REMOVE' buttons made invisible if non-admin
    ' so normal users cannot add or remove keywords from the list
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"
    'developer guide no. 69
    Private frmInterface As frmInterface

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""


    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' for property
    Private m_bUserIsAdministrator As Boolean

    'JH040199

    Public Property UserIsAdministrator() As Boolean
        Get

            Return m_bUserIsAdministrator

        End Get
        Set(ByVal Value As Boolean)

            m_bUserIsAdministrator = Value

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

            bIsStandAlone = False

            'Set m_oKeyword = New bDOCKeywordAdmin.Form

            'm_lreturn = m_oKeyword.Initialise( _
            ''   sUsername:=sUsername, _
            ''  sPassword:=sPassword, _
            '' iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, _
            ''iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, _
            ''sCallingAppName:=sCallingAppName)


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
#If PD_EARLYBOUND = 1 Then

			Set g_oObjectManager = New bObjectManager.ObjectManager
#Else
            g_oObjectManager = New bObjectManager.ObjectManager()
#End If

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_sUsername = .UserName
                g_iUserID = .UserID
                g_iLogLevel = .LogLevel
            End With

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
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
		Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: AdministerKeywords
    '
    ' Description: Display the administer keywords form
    '
    ' ***************************************************************** '

    Public Function AdministerKeywords() As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            bAdminMode = True

            ' load form
            Dim frmInterface As frmInterface = New frmInterface

            ' Disable the de/attach buttons
            frmInterface.cmdAttach.Visible = False
            frmInterface.cmdDetach.Visible = False

            ' Hide the column headers

            'TODO: Needs to be checked
            'frmInterface.lvwKeywords.HideColumnHeaders = True


            ' Set the caption of the form
            frmInterface.Text = "Administer Keywords"
            frmInterface.BringToFront()
            ' Show the form
            frmInterface.ShowDialog()

            ' unload form
            frmInterface.Close()

            frmInterface = Nothing

            Return result

        Catch




            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: AttachKeywords
    '
    ' Description: Display the attach keywords form
    '
    ' ***************************************************************** '

    Public Function AttachKeywords(ByRef vKeywordID As Object, Optional ByRef lDocNum As Integer = 0) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        bAdminMode = False

        Try

            If Information.IsNothing(lDocNum) Then
                bHasDocNum = False
            Else
                lDocumentNum = lDocNum
                bHasDocNum = True
            End If

            ' load form
            'Dim tempLoadForm As frmInterface = frmInterface
            frmInterface = New frmInterface
            ' Enable the de/attach buttons
            frmInterface.cmdAttach.Visible = True
            frmInterface.cmdDetach.Visible = True

            ' Show the column headers

            'TODO: Needs to be checked
            'frmInterface.lvwKeywords.HideColumnHeaders = False

            ' Set the caption of the form
            frmInterface.Text = "Attach Keywords"

            'only administrator can maintain keywords
            If Not m_bUserIsAdministrator Then
                frmInterface.cmdRemove.Visible = False
                frmInterface.cmdNew.Visible = False
            Else
                frmInterface.cmdRemove.Visible = True
                frmInterface.cmdNew.Visible = True
            End If

            ' Show the form
            frmInterface.ShowDialog()

            ' Get the keywords if OK was clicked (and Cancel wasnt)
            If Not frmInterface.Canceled Then


                m_lReturn = GetSelectedKeywords(vKeywordID:=vKeywordID)

            End If

            ' unload form
            frmInterface.Close()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("vKeywordID", vKeywordID)
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to obtain attach keywords.", vApp:=ACApp, vClass:=ACClass, vMethod:="AttachKeywords", excep:=excep, oDicParms:=oDict)

            Return result

        End Try
    End Function

    Private Function GetSelectedKeywords(ByRef vKeywordID() As Object) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lCount, lIDCount As Integer
        Dim sKey As String = ""

        Dim vIDs As Object



        lIDCount = 0

        ReDim vIDs(0)

        lCount = frmInterface.lvwKeywords.Items.Count

        For lLoop1 As Integer = 0 To lCount - 1

            If ListViewHelper.GetListViewSubItem(frmInterface.lvwKeywords.Items.Item(lLoop1), 1).Text = "Yes" Then

                ReDim Preserve vIDs(lIDCount)

                sKey = frmInterface.lvwKeywords.Items.Item(lLoop1).Name
                sKey = sKey.Substring(sKey.Length - (sKey.Length - 1))

                vIDs(lIDCount) = sKey
                lIDCount += 1

            End If

        Next lLoop1


        vKeywordID = vIDs

        Return result

    End Function
End Class

