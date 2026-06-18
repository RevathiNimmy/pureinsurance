Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
Module Module1
	
	
#Const MULTI_VEHICLE = False ' True
	
	Public Sub main()
		
		'TestClaimDataset
		GISQuoteForDMC()
		
		'MTADiffAndMerge
	End Sub
	
	
	Sub GISQuoteForDMC()
		Dim iGIS, PMTrue As Object
		

		Dim oGIS As iGIS.Application = New iGIS.Application()
		
		Dim vPolicyLinkID As Integer
		Dim vPolicyKey, vVehicleKey, vVehicleKey2, vNCDKey, vNCDKey2, vNCDKey3, vDriverKey, vOccupationKey, vProposerKey, vPolicyAddOnKey, vCoverKey, vAddOnKey, vPolicyKeyProper As String
		
		Dim sConvictionKey, sClaimKey As String
		
		Dim vNCDValue As Object
		Dim bNCDAssumed As Boolean
		
		
		Dim vQteKeyArray, vQteKey, vRow, vMonthlyPrem, vNoOfInstalments, vAPR, vTotalPayable, vAnnualPrem, vIPTAmount, vTotalPrem, vDepositAmount, vPolicyExcess, vNCDPercent, vWindscreenExcess, vCarHireCost As Object
		
		Dim sPropertyName As String = ""
		Dim vDescription, vQuoteResult As Object
		Dim lRow As Integer
		Dim vList, vListCodes As Object
		
		Dim sTopOIKey As String = ""
		Dim vInsFileCnt As Object
		
		Dim sDS, sDSD As String
		Dim lNewPl As Integer
		
		Dim lNewPLID As Integer
		
		' Initialise & Create the dataset
		' if it doesn't already exist.
		

		Dim lReturn As Integer = CType(oGIS, SSP.S4I.Interfaces.ILocalInterface).Initialise()

		If lReturn <> CDbl(PMTrue) Then
			MessageBox.Show("Error", Application.ProductName)
			Environment.Exit(0)
		End If
		

		lReturn = oGIS.GenDatasetDefinitions("buscom")

		If lReturn <> CDbl(PMTrue) Then
			MessageBox.Show("Error", Application.ProductName)
			Environment.Exit(0)
		End If
		
		'    lReturn = oGIS.LoadRiskFromDB("BUSCOMB", 10, 1)
		Environment.Exit(0)
		

		lReturn = oGIS.CopyDataSet(v_sdatamodelcode:="BUSCOM", r_lnewgispolicylinkid:=lNewPLID, v_voldinsurancefoldercnt:=1, v_voldriskid:=3, v_vnewinsurancefoldercnt:=1, v_vnewriskid:=10)

		If lReturn <> CDbl(PMTrue) Then
			MessageBox.Show("Error", Application.ProductName)
			Environment.Exit(0)
		End If
		
		

		lReturn = oGIS.SaveToDB()

		If lReturn <> CDbl(PMTrue) Then
			MessageBox.Show("Error", Application.ProductName)
			Environment.Exit(0)
		End If
		
		Environment.Exit(0)
		

		lReturn = oGIS.NewRiskDataSet("buscom", vPolicyLinkID, sTopOIKey, 3,  , 1)

		If lReturn <> CDbl(PMTrue) Then
			MessageBox.Show("Error", Application.ProductName)
			Environment.Exit(0)
		End If
		

		oGIS.Risk.NewObject("vehicle")

		oGIS.Risk.Item("vehicle").Item("vin").Value = "vin"

		oGIS.Risk.Item("vehicle").Item("dateregistered").Value = "2003-03-01"

		oGIS.Risk.Item("vehicle").Item("reg").Value = "NU03 REG"
		

		oGIS.Risk.Item("vehicle").NewObject("driver")

		oGIS.Risk.Item("vehicle").Item("driver").Item("forename").Value = "very"

		oGIS.Risk.Item("vehicle").Item("driver").Item("surname").Value = "baddriver"
		

		oGIS.Risk.Item("vehicle").Item("driver").NewObject("claim")

		oGIS.Risk.Item("vehicle").Item("driver").Item("claim").Item("code").Value = "claimcode"

		oGIS.Risk.Item("vehicle").Item("driver").Item("claim").Item("claimdate").Value = "1/1/2003"
		
		

		oGIS.Risk.Item("vehicle").Item("driver").Item("claim").NewObject("conviction")

		oGIS.Risk.Item("vehicle").Item("driver").Item("claim").Item("conviction").Item("code").Value = "convcode"

		oGIS.Risk.Item("vehicle").Item("driver").Item("claim").Item("conviction").Item("convictiondate").Value = "1/1/2003"
		
		' Add a second Conviction to Claim 1

		oGIS.Risk.Item("vehicle").Item("driver").Item("claim").NewObject("conviction")

		oGIS.Risk.Item("vehicle").Item("driver").Item("claim").Item("conviction", 2).Item("code").Value = "convcode12"

		oGIS.Risk.Item("vehicle").Item("driver").Item("claim").Item("conviction", 2).Item("convictiondate").Value = "1/1/2002"
		
		' Add a second Claim

		oGIS.Risk.Item("vehicle").Item("driver").NewObject("claim")

		oGIS.Risk.Item("vehicle").Item("driver").Item("claim", 2).Item("code").Value = "claimcode12"

		oGIS.Risk.Item("vehicle").Item("driver").Item("claim", 2).Item("claimdate").Value = "1/1/2003"
		
		' Add a Conviction to Claim 2

		oGIS.Risk.Item("vehicle").Item("driver").Item("claim", 2).NewObject("conviction")

		oGIS.Risk.Item("vehicle").Item("driver").Item("claim", 2).Item("conviction").Item("code").Value = "convcode21"

		oGIS.Risk.Item("vehicle").Item("driver").Item("claim", 2).Item("conviction").Item("convictiondate").Value = "1/1/2003"
		
		'    ' Add a Second Driver with 2 claims but no convictions
		'    oGIS.Risk.Item("vehicle").NewObject ("driver")
		'    oGIS.Risk.Item("vehicle").Item("driver", 2).Item("forename").Value = "forename2"
		'    oGIS.Risk.Item("vehicle").Item("driver", 2).Item("surname").Value = "surname2"
		'    oGIS.Risk.Item("vehicle").Item("driver", 2).Item("dob").Value = "1/1/1970"
		'
		'    oGIS.Risk.Item("vehicle").Item("driver", 2).NewObject ("claim")
		'    oGIS.Risk.Item("vehicle").Item("driver", 2).Item("claim").Item("code").Value = "claimcode21"
		'    oGIS.Risk.Item("vehicle").Item("driver", 2).Item("claim").Item("claimdate").Value = "1/1/2003"
		'
		'    oGIS.Risk.Item("vehicle").Item("driver", 2).NewObject ("claim")
		'    oGIS.Risk.Item("vehicle").Item("driver", 2).Item("claim", 2).Item("code").Value = "claimcode22"
		'    oGIS.Risk.Item("vehicle").Item("driver", 2).Item("claim", 2).Item("claimdate").Value = "1/1/2003"
		'
		'    oGIS.Risk.Item("vehicle").Item("driver", 2).NewObject ("claim")
		'    oGIS.Risk.Item("vehicle").Item("driver", 2).Item("claim", 3).Item("code").Value = "claimcode23"
		'    oGIS.Risk.Item("vehicle").Item("driver", 2).Item("claim", 3).Item("claimdate").Value = "1/1/2003"
		

		lReturn = oGIS.ReturnAsXML(sDS)
		

		lReturn = oGIS.SaveToDB()

		If lReturn <> CDbl(PMTrue) Then
			MessageBox.Show("Error", Application.ProductName)
			Environment.Exit(0)
		End If
		
		Environment.Exit(0)
		
		'    lReturn = oGIS.GenDatasetDefinitions("CLAIM2")
		'    If (lReturn <> PMTrue) Then
		'        MsgBox "Error"
		'        End
		'    End If
		'
		'    End
		

		lReturn = oGIS.LoadRiskFromDB("BUSCOMB", 155, 7)
		'    If (lReturn <> PMTrue) Then
		'        MsgBox "Error"
		'        End
		'    End If
		
		'lReturn = oGIS.CopyDataSet("BUSCOMB", lNewPl, , , , , 155, 132, , 7, 6)
		

		lReturn = oGIS.NewRiskDataSet("CLAIM2", vPolicyLinkID, sTopOIKey, 155,  , 7)

		If lReturn <> CDbl(PMTrue) Then
			MessageBox.Show("Error", Application.ProductName)
			Environment.Exit(0)
		End If

		lReturn = oGIS.ReturnAsXML(sDS)
		
		

		oGIS.Risk.Item("Associated_Client").Item("extra").Value = "updated again"

		oGIS.Risk.Item("Associated_Client").Item("extra_int").Value = 399

		oGIS.Risk.Item("Associated_Client").Item("extra_date").Value = "5/2/2003"
		

		lReturn = oGIS.ReturnAsXML(sDS)
		
		'lReturn = oGIS.SaveToDB

		If lReturn <> CDbl(PMTrue) Then
			MessageBox.Show("Error", Application.ProductName)
			Environment.Exit(0)
		End If
		

		oGIS.Risk.Item("Associated_Client").NewObject("disclosure")

		MessageBox.Show(oGIS.Risk.Item("Associated_Client").Item("disclosure", 2).Item("code").Value, Application.ProductName)

		MessageBox.Show(oGIS.Risk.Item("Associated_Client").Item("disclosure", 2).Item("description").Value, Application.ProductName)

		MessageBox.Show(oGIS.Risk.Item("Associated_Client").Item("disclosure", 2).Item("sentence_code").Value, Application.ProductName)
		'lReturn = oGIS.SaveToDB

		If lReturn <> CDbl(PMTrue) Then
			
			MessageBox.Show("Error", Application.ProductName)
			Environment.Exit(0)
		End If
		

		oGIS.Risk.Item("Associated_Client").Item("disclosure", 3).Item("extra").Value = "extra"

		oGIS.Risk.Item("Associated_Client").Item("disclosure", 3).Item("extra_int").Value = 99

		oGIS.Risk.Item("Associated_Client").Item("disclosure", 3).Item("extra_date").Value = "1/1/2003"

		lReturn = oGIS.ReturnAsXML(sDS)
		

		lReturn = oGIS.SaveToDB

		If lReturn <> CDbl(PMTrue) Then
			MessageBox.Show("Error", Application.ProductName)
			Environment.Exit(0)
		End If
		
		Environment.Exit(0)
		
		'lReturn = oGIS.NewDataSet("BUSCOMB", vPolicyLinkID, sTopOIKey)

		If lReturn <> CDbl(PMTrue) Then
			MessageBox.Show("Error", Application.ProductName)
			Environment.Exit(0)
		End If
		
		'    lReturn = oGIS.NewRiskDataSet("BUSCOMB", vPolicyLinkID, sTopOIKey, sDSD, sDS, 155, , 7)
		'    If (lReturn <> PMTrue) Then
		'        MsgBox "Error"
		'        End
		'    End If
		
	End Sub
	
	
	Sub MTADiffAndMerge()
		Dim iGIS, PMTrue As Object
		

		Dim oGIS As iGIS.Application = New iGIS.Application()
		
		
		' Initialise & Create the dataset
		' if it doesn't already exist.
		

		Dim lReturn As Integer = CType(oGIS, SSP.S4I.Interfaces.ILocalInterface).Initialise()

		If lReturn <> CDbl(PMTrue) Then
			MessageBox.Show("Error", Application.ProductName)
			Environment.Exit(0)
		End If
		
		'    lReturn = oGIS.MTADiffAndMerge( _
		''        v_lInsuranceFolderCnt:=1, _
		''        v_lOMTARiskID:=1, _
		''        v_lNMTARiskID:=10, _
		''        v_lIMTARiskID:=2)
		'    If (lReturn <> PMTrue) Then
		'        MsgBox "Error"
		'        End
		'    End If
		
		Environment.Exit(0)
		
		
	End Sub
	
	Sub TestClaimDataset()
		Dim iGIS, PMTrue As Object
		

		Dim oGIS As iGIS.Application = New iGIS.Application()
		
		Dim vPolicyLinkID As Integer
		Dim vPolicyKey, vVehicleKey, vVehicleKey2, vNCDKey, vNCDKey2, vNCDKey3, vDriverKey, vOccupationKey, vProposerKey, vPolicyAddOnKey, vCoverKey, vAddOnKey, vPolicyKeyProper As String
		
		Dim sConvictionKey, sClaimKey As String
		
		Dim vNCDValue As Object
		Dim bNCDAssumed As Boolean
		
		
		Dim vQteKeyArray, vQteKey, vRow, vMonthlyPrem, vNoOfInstalments, vAPR, vTotalPayable, vAnnualPrem, vIPTAmount, vTotalPrem, vDepositAmount, vPolicyExcess, vNCDPercent, vWindscreenExcess, vCarHireCost As Object
		
		Dim sPropertyName As String = ""
		Dim vDescription, vQuoteResult As Object
		Dim lRow As Integer
		Dim vList, vListCodes As Object
		
		Dim sTopOIKey As String = ""
		Dim vInsFileCnt As Object
		
		Dim sDS, sDSD As String
		Dim lNewPl As Integer
		
		' Initialise & Create the dataset
		' if it doesn't already exist.
		

		Dim lReturn As Integer = CType(oGIS, SSP.S4I.Interfaces.ILocalInterface).Initialise()

		If lReturn <> CDbl(PMTrue) Then
			MessageBox.Show("Error", Application.ProductName)
			Environment.Exit(0)
		End If
		

		lReturn = oGIS.GenDatasetDefinitions("tc")

		If lReturn <> CDbl(PMTrue) Then
			MessageBox.Show("Error", Application.ProductName)
			Environment.Exit(0)
		End If
		
		Environment.Exit(0)
		
		'    lReturn = oGIS.NewClaimDataset("CLAIM2", vPolicyLinkID, sTopOIKey, 1, 1)
		'    If (lReturn <> PMTrue) Then
		'        MsgBox "Error"
		'        End
		'    End If
		

		lReturn = oGIS.LoadClaimFromDB("CLAIM2", 1)

		If lReturn <> CDbl(PMTrue) Then
			MessageBox.Show("Error", Application.ProductName)
			Environment.Exit(0)
		End If
		

		lReturn = oGIS.ReturnAsXML(sDS)

		If lReturn <> CDbl(PMTrue) Then
			MessageBox.Show("Error", Application.ProductName)
			Environment.Exit(0)
		End If
		

		MessageBox.Show(oGIS.Risk.Item("Claim").Count("Claim_Peril"), Application.ProductName)
		

		MessageBox.Show(oGIS.Risk.Item("Claim").Item("Policy_Number").Value, Application.ProductName)

		MessageBox.Show(oGIS.Risk.Item("Claim").Item("claim_number").Value, Application.ProductName)
		

		MessageBox.Show(oGIS.Risk.Item("Claim").Item("Claim_Peril").Item("description").Value, Application.ProductName)

		MessageBox.Show(oGIS.Risk.Item("Claim").Item("Claim_Peril").Item("comments").Value, Application.ProductName)
		
		'    oGIS.Risk.Item("Claim").Item("Policy_Number").Value = "changed pol again"
		'    oGIS.Risk.Item("Claim").Item("claim_number").Value = "changed claim again"
		

		oGIS.Risk.Item("Claim").Item("Claim_Peril").Item("description").Value = "cp1 description"

		oGIS.Risk.Item("Claim").Item("Claim_Peril").Item("comments").Value = "cp1 comments"
		

		oGIS.Risk.Item("Claim").Item("Claim_Peril", 2).Item("description").Value = "cp2 description"

		oGIS.Risk.Item("Claim").Item("Claim_Peril", 2).Item("comments").Value = "cp2 comments"
		
		'oGIS.Risk.Item("Claim").NewObject ("Claim_Peril")
		'oGIS.Risk.Item("Claim").Item("Claim_Peril", 3).Item("Claim_id").Value = 1
		'oGIS.Risk.Item("Claim").Item("Claim_Peril", 3).Item("Claim_peril_id").Value = 1
		'oGIS.Risk.Item("Claim").Item("Claim_Peril", 3).Item("peril_type_id").Value = 1
		

		oGIS.Risk.Item("Claim").Item("Claim_Peril", 3).Item("description").Value = "cp3 description"

		oGIS.Risk.Item("Claim").Item("Claim_Peril", 3).Item("comments").Value = "cp3 comments"
		
		

		lReturn = oGIS.ReturnAsXML(sDS)
		

		lReturn = oGIS.SaveToDB

		If lReturn <> CDbl(PMTrue) Then
			MessageBox.Show("Error", Application.ProductName)
			Environment.Exit(0)
		End If
		Environment.Exit(0)
		

		oGIS.Risk.Item("Associated_Client").NewObject("disclosure")
		
		

		MessageBox.Show(oGIS.Risk.Item("Associated_Client").Item("disclosure", 2).Item("code").Value, Application.ProductName)

		MessageBox.Show(oGIS.Risk.Item("Associated_Client").Item("disclosure", 2).Item("description").Value, Application.ProductName)

		MessageBox.Show(oGIS.Risk.Item("Associated_Client").Item("disclosure", 2).Item("sentence_code").Value, Application.ProductName)
		'lReturn = oGIS.SaveToDB

		If lReturn <> CDbl(PMTrue) Then
			
			MessageBox.Show("Error", Application.ProductName)
			Environment.Exit(0)
		End If
		

		oGIS.Risk.Item("Associated_Client").Item("disclosure", 3).Item("extra").Value = "extra"

		oGIS.Risk.Item("Associated_Client").Item("disclosure", 3).Item("extra_int").Value = 99

		oGIS.Risk.Item("Associated_Client").Item("disclosure", 3).Item("extra_date").Value = "1/1/2003"

		lReturn = oGIS.ReturnAsXML(sDS)
		

		lReturn = oGIS.SaveToDB

		If lReturn <> CDbl(PMTrue) Then
			MessageBox.Show("Error", Application.ProductName)
			Environment.Exit(0)
		End If
		
		Environment.Exit(0)
		
		'lReturn = oGIS.NewDataSet("BUSCOMB", vPolicyLinkID, sTopOIKey)

		If lReturn <> CDbl(PMTrue) Then
			MessageBox.Show("Error", Application.ProductName)
			Environment.Exit(0)
		End If
		
		'    lReturn = oGIS.NewRiskDataSet("BUSCOMB", vPolicyLinkID, sTopOIKey, sDSD, sDS, 155, , 7)
		'    If (lReturn <> PMTrue) Then
		'        MsgBox "Error"
		'        End
		'    End If
		
	End Sub
End Module