Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed 
	
	  Implements IDisposable
Private m_sUsername As String = ""
	
	' Password.
	Private m_sPassword As String = ""
	
	' User ID
	Private m_iUserID As Integer
	
	' Calling Application
	Private m_sCallingAppName As String = ""
    ' Source ID
	Private m_iSourceID As Integer
	' Language ID
	Private m_iLanguageID As Integer
	' Currency ID
	Private m_iCurrencyID As Integer
	' LogLevel
	Private m_iLogLevel As Integer
	Private m_lReturn As gPMConstants.PMEReturnCode
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	Private m_lPMAuthorityLevel As Integer
	

	Private m_oBusiness As bACTExportCashListItems.Business
	Private m_oObjectManager As bObjectManager.ObjectManager
	Private m_oSBODatabase As dPMDAO.Database
	Private m_oOrionDatabase As Object
	
Private Const ACClass As String = "Interface" 
	
	
	Public Property Status() As Integer
		Get
			Return m_lStatus
		End Get
		Set(ByVal Value As Integer)
			m_lStatus = Value
		End Set
	End Property
	
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			m_sCallingAppName = Value
		End Set
	End Property
	
	Public WriteOnly Property PMAuthorityLevel() As Integer
		Set(ByVal Value As Integer)
			m_lPMAuthorityLevel = Value
		End Set
	End Property
	
	Public Function Initialise() As Integer
		
		Dim result As Integer = 0 
        Dim sMessage As String 
		Dim vUserData(,) As Object 
		Dim sOrionPath As String = "" 
	
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Create an instance of the object manager
			m_oObjectManager = New bObjectManager.ObjectManager()
			
			' Call the initialise method.
			If m_sCallingAppName = CALLING_APP_WRAPPER Then
				
				'DC250504 PN11171 -change way logs on
				m_oSBODatabase = New dPMDAO.Database()
				
				m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_bNewInstanceCreated:=True, r_oCheckedDatabase:=m_oSBODatabase), gPMConstants.PMEReturnCode)
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					'        Set oComponentServices = Nothing
					result = gPMConstants.PMEReturnCode.PMFalse
					sMessage = "Failed to Get S4B Database"
					Throw New Exception()
				End If
				
				' Set Username and Password
				m_sUsername = "siriuscomm"
				m_sPassword = "NGMBMKqSMcc5"
				
				With m_oSBODatabase
					.Parameters.Clear()
					
					m_lReturn = .Parameters.Add(sName:="Username", vValue:=m_sUsername, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
					
					m_lReturn = .Parameters.Add(sName:="Password", vValue:=m_sPassword, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
					
					'Get the User details
					m_lReturn = .SQLSelect(sSQL:=ACSelectUserDetailsSQL, sSQLName:=ACSelectUserDetailsName, bStoredProcedure:=ACSelectUserDetailsStored, vResultArray:=vUserData)
					
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						result = gPMConstants.PMEReturnCode.PMFalse
						sMessage = "Unable to find User ID for User Sirius"
						Return result
					End If
					If Not Information.IsArray(vUserData) Then
						result = gPMConstants.PMEReturnCode.PMFalse
						sMessage = "Unable to find user ID for User Sirius"
						Return result
					End If
				End With
				

                m_iUserID = CInt(vUserData(0, 0))

                m_iLanguageID = CInt(vUserData(1, 0))
                m_iSourceID = 1
                m_iCurrencyID = 26
                m_iLogLevel = 9

                ' it's being called by the scheduler
                ' don't show a login prompt
                'DC210404 PN11171 -log on as siriuscomm a common logon for all sites and added actual sourceid being processed
                m_lReturn = m_oObjectManager.InitialiseWithUserState(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iCountryID:=1, iLanguageID:=m_iLanguageID, iLogLevel:=m_iLogLevel, iCurrencyID:=m_iCurrencyID, lPartyCnt:=0, sCallingAppName:=m_sCallingAppName)
                'DC250504 PN11171 -end -change way logs on

            Else
                ' it's being called via W/Mgr or similar
                ' pick up the existing login
                m_lReturn = m_oObjectManager.Initialise(sCallingAppName:=ACApp)
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.

                ' Set the object manager to nothing.
                m_oObjectManager = Nothing

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With m_oObjectManager
                m_iLanguageID = .LanguageID
                m_iSourceID = .SourceID
            End With

            Dim temp_m_oBusiness As Object 
            m_lReturn = m_oObjectManager.GetInstance(temp_m_oBusiness, "bACTExportCashListItems.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_oObjectManager = Nothing

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bACTExportCashListItems.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

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
                If m_oObjectManager IsNot Nothing Then
                    m_oObjectManager.Dispose()
                    m_oObjectManager = Nothing
                End If
                If Not (m_oSBODatabase Is Nothing) Then
                    m_oSBODatabase.CloseDatabase()


                End If
                m_oSBODatabase = Nothing
            End If
        End If
		Me.disposedValue = True
	End Sub


    Public Function Start() As Integer

        Dim result As Integer = 0
        Dim oForm As frmExportCashBook
        Dim vBranches(,) As Object
        Dim lMulti As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'DC220404 PN11171 -start -process all branches if run via scheduler or if core

            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'check if multi branch (lMulti=1) or core system (lMulti=0)

            m_lReturn = m_oBusiness.MultibranchCheck(lMulti)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to get MultiBranchCheck from bACTExportCashListItems", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            End If

            'if not run via scheduler, setup of location file is allowed
            If m_sCallingAppName <> CALLING_APP_WRAPPER Then

                oForm = New frmExportCashBook()

                If lMulti = 0 Then

                    'if not running via scheduler and core system,
                    'user should still be able to setup location of export file
                    oForm.CallingAppName = "SETUP"

                Else

                    'this allows setup and runs export for current branch logged in (multi)
                    oForm.CallingAppName = m_sCallingAppName

                End If
                'developer guide no. 9
                oForm.Initialise()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to initialise frmExportCashBook", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)

                    Return result
                End If

                With oForm
                    .Business = m_oBusiness
                    .SourceId = m_iSourceID
                End With

                m_lReturn = CType(oForm.Load_Renamed(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to load frmExportCashBook", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)

                    Return result
                End If

                oForm.Business = m_oBusiness

                oForm.ShowDialog()
                m_lStatus = oForm.Status
                oForm.Close()

                oForm = Nothing

            End If

            'The following runs the export for each branch if run via the scheduler
            'or if running core system
            If m_lStatus = gPMConstants.PMEReturnCode.PMOK Then

                'If Core system, then setup to run through all branches automatically
                'as if running via scheduler
                If lMulti = 0 Then
                    m_sCallingAppName = CALLING_APP_WRAPPER
                End If

                If m_sCallingAppName = CALLING_APP_WRAPPER Then

                    'get all branches to process

                    m_lReturn = m_oBusiness.GetAllBranches(vBranches)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to get All Branches from bACTExportCashListItems", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return result
                    End If


                    For lCounter As Integer = vBranches.GetLowerBound(1) To vBranches.GetUpperBound(1)


                        m_iSourceID = CInt(vBranches(0, lCounter))

                        '                m_lReturn = m_oObjectManager.InitialiseWithUserState( _
                        ''                                sUserName:="siriuscomm", _
                        ''                                sPassword:="NGMBMKqSMcc5", _
                        ''                                iUserID:=1, _
                        ''                                iSourceID:=m_iSourceID, _
                        ''                                iCountryID:=1, _
                        ''                                iLanguageID:=1, _
                        ''                                iLogLevel:=9, _
                        ''                                iCurrencyID:=26, _
                        ''                                lPartyCnt:=0, _
                        ''                                sCallingAppName:=m_sCallingAppName)
                        m_lReturn = m_oObjectManager.InitialiseWithUserState(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iCountryID:=1, iLanguageID:=m_iLanguageID, iLogLevel:=m_iLogLevel, iCurrencyID:=m_iCurrencyID, lPartyCnt:=0, sCallingAppName:=m_sCallingAppName)
                        'DC220404 PN11171 -end

                        oForm = New frmExportCashBook()

                        oForm.CallingAppName = m_sCallingAppName
                        oForm.Visible = False
                        'developer guide no. 9
                        m_lReturn = oForm.Initialise()

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            MessageBox.Show("Failed to initialise frmExportCashBook", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)

                            Return result
                        End If

                        With oForm
                            .Business = m_oBusiness
                            .SourceId = m_iSourceID
                        End With

                        m_lReturn = CType(oForm.Load_Renamed(), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            MessageBox.Show("Failed to load frmExportCashBook", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)

                            Return result
                        End If

                        oForm.Business = m_oBusiness

                        oForm.ShowDialog()

                        oForm.Close()

                        oForm = Nothing

                        'DC220404 PN11171 -process all branches via scheduler or if core
                    Next lCounter

                End If

            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function
	
	'UPGRADE_NOTE: (7001) The following declaration (GetBranchName) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function GetBranchName() As Integer
		'
		'Dim result As Integer = 0
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMFalse
			'
			'
			'Return gPMConstants.PMEReturnCode.PMTrue
		'
		'Catch excep As System.Exception
			'
			'
			'
			'result = gPMConstants.PMEReturnCode.PMError
			'
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBranchName failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranchName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result
		'End Try
	'End Function
	
Public Function SetKeys(ByVal vKeyArray(,) As Object ) As Integer
		'nothing
		Return gPMConstants.PMEReturnCode.PMTrue
	End Function
	
Public Function GetKeys(ByRef vKeyArray(,) As Object ) As Integer
		'nothing
		Return gPMConstants.PMEReturnCode.PMTrue
	End Function
	
	Public Function GetSummary(ByVal vSummaryArray As Object) As Integer
		'nothing
		Return gPMConstants.PMEReturnCode.PMTrue
	End Function
	
	Public Function SetProcessModes(ByVal vTask As Object, ByVal vNavigate As Object, ByVal vProcessMode As Object, ByVal vTransactionType As Object, ByVal vEffectiveDate As Object) As Integer
		
		Return gPMConstants.PMEReturnCode.PMTrue
		
	End Function
End Class
