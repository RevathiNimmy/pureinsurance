Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date:
    '
    ' Description: Main interface.
    '
    ' Edit History: Saurabh
    ' ***************************************************************** '
    ''Start(Saurabh Agrawal) Tech Spec QBEZCR005 Client Portfolio transfer(5.2)
    Private bCancelTheForm As Boolean = False
    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As Integer

    Private m_iLanguageID As Integer
    Private m_iSourceID As Integer
    Private m_iUserId As Integer

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_iTask As Integer

    Private m_lReturn As Integer

    Private m_lFromClientCnt As Integer
    Private m_lToClientCnt As Integer
    Private m_sFromClientCode As String = ""
    Private m_sToClientCode As String = ""
    Private m_sFromClientName As String = ""
    Private m_sToclientName As String = ""
    Private m_sFromClientType As String = ""
    Private m_sToClientType As String = ""

    Public ReadOnly Property FromClientCode() As String
        Get

            Return m_sFromClientCode

        End Get
    End Property

    Public ReadOnly Property ToClientCode() As String
        Get

            Return m_sToClientCode

        End Get
    End Property

    Public ReadOnly Property FromClientCnt() As Integer
        Get

            Return m_lFromClientCnt

        End Get
    End Property

    Public ReadOnly Property ToClientCnt() As Integer
        Get

            Return m_lToClientCnt

        End Get
    End Property

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.
            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.
            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.
            ' Set the navigate flag.
            m_lNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.
            ' Set the process mode.
            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.
            ' Set the type of business.
            m_sTransactionType = Value

        End Set
    End Property


    'DC180202
    Public Property Task() As Integer
        Get

            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            m_iTask = Value

        End Set
    End Property

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        Dim iYesNo As DialogResult = MessageBox.Show("Cancelling will losse any changes." & Strings.Chr(13) & Strings.Chr(10) & "Do you wish to cancel?", "Cancel Portfolio Transfer", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If iYesNo = System.Windows.Forms.DialogResult.Yes Then
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
            bCancelTheForm = True
            Me.Close()
        End If

    End Sub

    Private Sub cmdFromClientSelect_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFromClientSelect.Click

        m_lReturn = GetFromClientDetails()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
            gPMFunctions.RaiseError("CmdClient_Click", "GetFromClientDetails Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


    End Sub


    Public Function GetFromClientDetails() As Integer
        Dim result As Integer = 0
        Dim iPMBFindParty As Object


        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Const kMethodName As String = "GetFromClientDetails"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Find Party object
            Dim temp_oFindParty As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindParty = temp_oFindParty

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMBFindParty.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Set component properties and start interface

            oFindParty.CallingAppName = ACApp

            oFindParty.IgnoreDPAQuestions = True

            oFindParty.NotEditable = 1

            oFindParty.EnableNewParty = False


            m_lReturn = oFindParty.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "oFindParty.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then
                'Retrieve party details

                m_sFromClientCode = oFindParty.ShortName

                m_sFromClientName = oFindParty.LongName

                m_lFromClientCnt = oFindParty.PartyCnt

                m_sFromClientType = oFindParty.PartyType



                ' Display Agent on form
                txtFromClientName.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sFromClientName.Trim())

                txtFromClientCode.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sFromClientCode.Trim())


                txtFromClientType.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=oFindParty.PartyTypeDesc.Trim())


                ' Destroy Find Party object

                oFindParty.Dispose()
            ElseIf oFindParty.Status = gPMConstants.PMEReturnCode.PMCancel Then
                result = gPMConstants.PMEReturnCode.PMCancel
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally
            '        Return result
            '        Resume



            '        Return result
        End Try
        Return result
    End Function




    Public Function GetToClientDetails() As Integer
        Dim result As Integer = 0
        Dim iPMBFindParty As Object


        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Const kMethodName As String = "GetToClientDetails"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Find Party object
            Dim temp_oFindParty As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindParty = temp_oFindParty

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMBFindParty.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Set component properties and start interface

            oFindParty.CallingAppName = ACApp

            oFindParty.IgnoreDPAQuestions = True

            oFindParty.NotEditable = 1

            oFindParty.EnableNewParty = True


            m_lReturn = oFindParty.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "oFindParty.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then
                'Retrieve party details

                m_sToClientCode = oFindParty.ShortName

                m_sToclientName = oFindParty.LongName

                m_lToClientCnt = oFindParty.PartyCnt

                m_sToClientType = oFindParty.PartyType



                ' Display Agent on form
                txtToClientName.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sToclientName.Trim())

                txtToClientCode.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sToClientCode.Trim())



                txtToClientType.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=oFindParty.PartyTypeDesc.Trim())


                ' Destroy Find Party object

                oFindParty.Dispose()
            ElseIf oFindParty.Status = gPMConstants.PMEReturnCode.PMCancel Then
                result = gPMConstants.PMEReturnCode.PMCancel
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally
            '        Return result
            '        Resume



            '        Return result
        End Try
        Return result
    End Function


    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
        m_lReturn = Validate_Renamed()

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            m_lStatus = gPMConstants.PMEReturnCode.PMOK
            bCancelTheForm = False
            Me.Hide()
        End If

    End Sub

    Private Sub cmdToClientSelect_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdToClientSelect.Click
        m_lReturn = GetToClientDetails()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
            gPMFunctions.RaiseError("CmdClient_Click", "GetToClientDetails Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
    End Sub


    Private Function Validate_Renamed() As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "Validate"
        Try
            ''Do All the Validation here.

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lFromClientCnt = 0 Then
                MessageBox.Show("A Client From must be specified to continue", "Portfolio Transfer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                cmdFromClientSelect.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lToClientCnt = 0 Then
                MessageBox.Show("A Client To must be specified to continue", "Portfolio Transfer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                cmdToClientSelect.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lFromClientCnt = m_lToClientCnt Then
                MessageBox.Show("From Client cannot be same as To Client", "Portfolio Transfer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                cmdToClientSelect.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_sFromClientType <> m_sToClientType Then
                MessageBox.Show("Clients must be of the same type to continue", "Portfolio Transfer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                cmdToClientSelect.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally




        End Try
        Return result
    End Function

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)


        ' Forms query unload event.

        Try
            If bCancelTheForm Then
                ' set status to cancel wants to Close
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
            End If
        Catch excep As System.Exception



            ' Error Section.
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub
End Class