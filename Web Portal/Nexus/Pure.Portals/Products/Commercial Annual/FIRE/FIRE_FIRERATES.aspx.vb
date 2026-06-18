
Namespace Nexus

		Partial Class FIRE_FIRERATES : Inherits BaseRisk
		
		Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            SetPageProgress(3)
            Dim sRILimit As String = GetValue("GROUP_FIRE", "RILIMITNUM")
            Dim sTypeConstruction As String = GetValue("GROUP_FIRE", "TYPE_CONSTRUCTION")

            Dim sLDRateThreeYearLoss As String = GetValue("GROUP_FIRE", "THREEYEARLOSS")
            Dim sLDRateBrMktAdj As String = GetValue("GROUP_FIRE", "BRANCH_MARKET_ADJUST")
            Dim sLDRateAdjBrMKT As String = GetValue("GROUP_FIRE", "ADJUST_BRANCH_MKT")
            Dim sLDRateSpecials As String = GetValue("GROUP_FIRE", "SPECIALS")

            'hvTabVisited.Value = GetValue("FIRE", "RD1_RATE")
            If sRILimit = "3152" AndAlso sTypeConstruction = "3168" Then '1 & 'Standard
                hvBookRateFE.Value = "0.1850"
                hvBookRateSP.Value = "0.0500"
                hvBookRateEQ.Value = "0.0200"
                hvBookRateMD.Value = "0.0100"
            ElseIf sRILimit = "3152" AndAlso sTypeConstruction = "3169" Then '1 & 'Non-Standard
                hvBookRateFE.Value = "0.2000"
                hvBookRateSP.Value = "0.0600"
                hvBookRateEQ.Value = "0.0300"
                hvBookRateMD.Value = "0.0200"
            ElseIf sRILimit = "3152" AndAlso sTypeConstruction = "3170" Then '1 & 'Thatch
                hvBookRateFE.Value = "0.5000"
                hvBookRateSP.Value = "0.1000"
                hvBookRateEQ.Value = "0.0800"
                hvBookRateMD.Value = "0.0500"
            ElseIf sRILimit = "3153" AndAlso sTypeConstruction = "3168" Then '2 & 'Standard
                hvBookRateFE.Value = "0.2000"
                hvBookRateSP.Value = "0.0600"
                hvBookRateEQ.Value = "0.0300"
                hvBookRateMD.Value = "0.0200"
            ElseIf sRILimit = "3153" AndAlso sTypeConstruction = "3169" Then '2 & 'Non-Standard
                hvBookRateFE.Value = "0.4000"
                hvBookRateSP.Value = "0.0800"
                hvBookRateEQ.Value = "0.0850"
                hvBookRateMD.Value = "0.0900"
            ElseIf sRILimit = "3153" AndAlso sTypeConstruction = "3170" Then '2 & 'Thatch
                hvBookRateFE.Value = "0.6000"
                hvBookRateSP.Value = "0.7000"
                hvBookRateEQ.Value = "0.0800"
                hvBookRateMD.Value = "0.9500"
            ElseIf sRILimit = "3154" AndAlso sTypeConstruction = "3168" Then '1 & 'Standard Contruction 
                hvBookRateFE.Value = "0.2500"
                hvBookRateSP.Value = "0.0650"
                hvBookRateEQ.Value = "0.0350"
                hvBookRateMD.Value = "0.0250"
            ElseIf sRILimit = "3154" AndAlso sTypeConstruction = "3169" Then '1 & 'Non Standard Contruction 
                hvBookRateFE.Value = "0.5500"
                hvBookRateSP.Value = "0.8500"
                hvBookRateEQ.Value = "0.9500"
                hvBookRateMD.Value = "0.9550"
            ElseIf sRILimit = "3154" AndAlso sTypeConstruction = "3170" Then '1 &  'Thatch 
                hvBookRateFE.Value = "0.0900"
                hvBookRateSP.Value = "0.0300"
                hvBookRateEQ.Value = "0.0350"
                hvBookRateMD.Value = "0.0400"
            ElseIf sRILimit = "3163" AndAlso sTypeConstruction = "3168" Then '3 & 'Standard
                hvBookRateFE.Value = "0.2500"
                hvBookRateSP.Value = "0.0650"
                hvBookRateEQ.Value = "0.0350"
                hvBookRateMD.Value = "0.0250"
            ElseIf sRILimit = "3163" AndAlso sTypeConstruction = "3169" Then '3 & 'Non-Standard
                hvBookRateFE.Value = "0.5500"
                hvBookRateSP.Value = "0.8500"
                hvBookRateEQ.Value = "0.9500"
                hvBookRateMD.Value = "0.9550"
            ElseIf sRILimit = "3163" AndAlso sTypeConstruction = "3170" Then '3 & 'Thatch
                hvBookRateFE.Value = "0.0900"
                hvBookRateSP.Value = "0.0300"
                hvBookRateEQ.Value = "0.0350"
                hvBookRateMD.Value = "0.0400"
            ElseIf sRILimit = "3155" AndAlso sTypeConstruction = "3168" Then '4 & 'Standard
                hvBookRateFE.Value = "0.1000"
                hvBookRateSP.Value = "0.1500"
                hvBookRateEQ.Value = "0.2500"
                hvBookRateMD.Value = "0.3500"
            ElseIf sRILimit = "3155" AndAlso sTypeConstruction = "3169" Then '4 & 'Non-Standard
                hvBookRateFE.Value = "0.3000"
                hvBookRateSP.Value = "0.3500"
                hvBookRateEQ.Value = "0.4500"
                hvBookRateMD.Value = "0.6500"
            ElseIf sRILimit = "3155" AndAlso sTypeConstruction = "3170" Then '4 & 'Thatch
                hvBookRateFE.Value = "0.4500"
                hvBookRateSP.Value = "0.6500"
                hvBookRateMD.Value = "0.3700"
                hvBookRateEQ.Value = "0.6500"
            ElseIf sRILimit = "3156" AndAlso sTypeConstruction = "3168" Then '5 & 'Standard
                hvBookRateFE.Value = "0.6000"
                hvBookRateSP.Value = "0.4500"
                hvBookRateEQ.Value = "0.8500"
                hvBookRateMD.Value = "0.0900"
            ElseIf sRILimit = "3156" AndAlso sTypeConstruction = "3169" Then '5 & 'Non-Standard
                hvBookRateFE.Value = "0.7500"
                hvBookRateSP.Value = "0.2500"
                hvBookRateEQ.Value = "1.0500"
                hvBookRateMD.Value = "0.0800"
            ElseIf sRILimit = "3156" AndAlso sTypeConstruction = "3170" Then '5 & 'Thatch
                hvBookRateFE.Value = "0.9000"
                hvBookRateSP.Value = "0.0500"
                hvBookRateEQ.Value = "1.2500"
                hvBookRateMD.Value = "0.0700"
            ElseIf sRILimit = "3157" AndAlso sTypeConstruction = "3168" Then '6 & 'Standard
                hvBookRateFE.Value = "1.0500"
                hvBookRateSP.Value = "0.6900"
                hvBookRateEQ.Value = "1.4500"
                hvBookRateMD.Value = "0.0600"
            ElseIf sRILimit = "3157" AndAlso sTypeConstruction = "3169" Then '6 & 'Non-Standard
                hvBookRateFE.Value = "1.2000"
                hvBookRateSP.Value = "0.7600"
                hvBookRateEQ.Value = "1.6500"
                hvBookRateMD.Value = "0.0500"
            ElseIf sRILimit = "3157" AndAlso sTypeConstruction = "3170" Then '6 & 'Thatch
                hvBookRateFE.Value = "1.3500"
                hvBookRateSP.Value = "0.8300"
                hvBookRateEQ.Value = "1.8500"
                hvBookRateMD.Value = "0.0400"
            ElseIf sRILimit = "3158" AndAlso sTypeConstruction = "3168" Then '7 & 'Standard
                hvBookRateFE.Value = "1.5000"
                hvBookRateSP.Value = "0.9000"
                hvBookRateEQ.Value = "0.7800"
                hvBookRateMD.Value = "0.5600"
            ElseIf sRILimit = "3158" AndAlso sTypeConstruction = "3169" Then '7 & 'Non-Standard
                hvBookRateFE.Value = "1.6500"
                hvBookRateSP.Value = "0.9700"
                hvBookRateEQ.Value = "0.8600"
                hvBookRateMD.Value = "1.0800"
            ElseIf sRILimit = "3158" AndAlso sTypeConstruction = "3170" Then '7 & 'Thatch
                hvBookRateFE.Value = "1.8000"
                hvBookRateSP.Value = "1.0400"
                hvBookRateEQ.Value = "0.9400"
                hvBookRateMD.Value = "1.6000"
            ElseIf sRILimit = "3159" AndAlso sTypeConstruction = "3168" Then '19a & 'Standard
                hvBookRateFE.Value = "1.9500"
                hvBookRateSP.Value = "1.1100"
                hvBookRateEQ.Value = "1.0200"
                hvBookRateMD.Value = "1.6500"
            ElseIf sRILimit = "3159" AndAlso sTypeConstruction = "3169" Then '19a & 'Non-Standard
                hvBookRateFE.Value = "2.1000"
                hvBookRateSP.Value = "1.1800"
                hvBookRateEQ.Value = "1.1000"
                hvBookRateMD.Value = "1.7000"
            ElseIf sRILimit = "3159" AndAlso sTypeConstruction = "3170" Then '19a & 'Thatch
                hvBookRateFE.Value = "2.2500"
                hvBookRateSP.Value = "1.2500"
                hvBookRateEQ.Value = "1.1800"
                hvBookRateMD.Value = "1.7500"
            ElseIf sRILimit = "3160" AndAlso sTypeConstruction = "3168" Then '3 & 'Standard
                hvBookRateFE.Value = "2.4000"
                hvBookRateSP.Value = "1.3200"
                hvBookRateEQ.Value = "1.2600"
                hvBookRateMD.Value = "1.8000"
            ElseIf sRILimit = "3160" AndAlso sTypeConstruction = "3169" Then '3 & 'Non-Standard
                hvBookRateFE.Value = "2.5500"
                hvBookRateSP.Value = "1.3900"
                hvBookRateEQ.Value = "1.3400"
                hvBookRateMD.Value = "1.8000"
            ElseIf sRILimit = "3160" AndAlso sTypeConstruction = "3170" Then '3 & 'Thatch
                hvBookRateFE.Value = "2.7000"
                hvBookRateSP.Value = "1.4600"
                hvBookRateEQ.Value = "1.4200"
                hvBookRateMD.Value = "1.8000"
            ElseIf sRILimit = "3161" AndAlso sTypeConstruction = "3168" Then '1 & 'Standard Contruction 
                hvBookRateFE.Value = "0.1850"
                hvBookRateSP.Value = "0.0500"
                hvBookRateEQ.Value = "0.0200"
                hvBookRateMD.Value = "0.0100"
            ElseIf sRILimit = "3161" AndAlso sTypeConstruction = "3169" Then '1 & 'Non Standard Contruction 
                hvBookRateFE.Value = "0.2000"
                hvBookRateSP.Value = "0.0600"
                hvBookRateEQ.Value = "0.0300"
                hvBookRateMD.Value = "0.0200"
            ElseIf sRILimit = "3161" AndAlso sTypeConstruction = "3170" Then '1 &  'Thatch 
                hvBookRateFE.Value = "0.5000"
                hvBookRateSP.Value = "0.1000"
                hvBookRateEQ.Value = "0.0800"
                hvBookRateMD.Value = "0.0500"
            ElseIf sRILimit = "3162" AndAlso sTypeConstruction = "3168" Then '1 & 'Standard Contruction 
                hvBookRateFE.Value = "0.2000"
                hvBookRateSP.Value = "0.0600"
                hvBookRateEQ.Value = "0.0300"
                hvBookRateMD.Value = "0.0200"
            ElseIf sRILimit = "3162" AndAlso sTypeConstruction = "3169" Then '1 & 'Non Standard Contruction 
                hvBookRateFE.Value = "0.4000"
                hvBookRateSP.Value = "0.0800"
                hvBookRateEQ.Value = "0.0850"
                hvBookRateMD.Value = "0.0900"
            ElseIf sRILimit = "3162" AndAlso sTypeConstruction = "3170" Then '1 &  'Thatch 
                hvBookRateFE.Value = "0.4000"
                hvBookRateSP.Value = "0.0800"
                hvBookRateEQ.Value = "0.0850"
                hvBookRateMD.Value = "0.0900"
            ElseIf sRILimit = "3162" AndAlso sTypeConstruction = "3168" Then '1 & 'Standard Contruction 
                hvBookRateFE.Value = "0.2000"
                hvBookRateSP.Value = "0.0600"
                hvBookRateEQ.Value = "0.0300"
                hvBookRateMD.Value = "0.0200"
            ElseIf sRILimit = "3162" AndAlso sTypeConstruction = "3169" Then '1 & 'Non Standard Contruction 
                hvBookRateFE.Value = "0.4000"
                hvBookRateSP.Value = "0.0800"
                hvBookRateEQ.Value = "0.0850"
                hvBookRateMD.Value = "0.0900"
            ElseIf sRILimit = "3162" AndAlso sTypeConstruction = "3170" Then '1 &  'Thatch 
                hvBookRateFE.Value = "0.4000"
                hvBookRateSP.Value = "0.0800"
                hvBookRateEQ.Value = "0.0850"
                hvBookRateMD.Value = "0.0900"
            End If


            '<70 Y Y Y 
            If sLDRateThreeYearLoss = "3164" AndAlso sLDRateBrMktAdj = "1" AndAlso sLDRateAdjBrMKT = "1" AndAlso sLDRateSpecials = "1" Then '
                hvLDRate1.Value = "0.0800"
                hvLDRate2.Value = "0.0267"
                hvLDRate3.Value = "0.0107"
                hvLDRate4.Value = "0.0053"
            End If

            '<70  Y N Y   

            If sLDRateThreeYearLoss = "3164" AndAlso sLDRateBrMktAdj = "2" AndAlso sLDRateAdjBrMKT = "1" AndAlso sLDRateSpecials = "1" Then '
                hvLDRate1.Value = "0.1700"
                hvLDRate2.Value = "0.0567"
                hvLDRate3.Value = "0.0227"
                hvLDRate4.Value = "0.0113"
            End If

            '>70 Y Y Y  
            If sLDRateThreeYearLoss = "3165" AndAlso sLDRateBrMktAdj = "1" AndAlso sLDRateAdjBrMKT = "1" AndAlso sLDRateSpecials = "1" Then '
                hvLDRate1.Value = "0.0300"
                hvLDRate2.Value = "0.0100"
                hvLDRate3.Value = "0.0040"
                hvLDRate4.Value = "0.0020"
            End If


            '>70 Y N Y 
            If sLDRateThreeYearLoss = "3165" AndAlso sLDRateBrMktAdj = "1" AndAlso sLDRateAdjBrMKT = "2" AndAlso sLDRateSpecials = "1" Then
                hvLDRate1.Value = "0.1900"
                hvLDRate2.Value = "0.0633"
                hvLDRate3.Value = "0.0253"
                hvLDRate4.Value = "0.0127"
            End If

            '<70 N N Y  

            If sLDRateThreeYearLoss = "3164" AndAlso sLDRateBrMktAdj = "2" AndAlso sLDRateAdjBrMKT = "2" AndAlso sLDRateSpecials = "1" Then '
                hvLDRate1.Value = "0.0400"
                hvLDRate2.Value = "0.0133"
                hvLDRate3.Value = "0.0053"
                hvLDRate4.Value = "0.0027"
            End If


            '>70 N Y Y 
            If sLDRateThreeYearLoss = "3165" AndAlso sLDRateBrMktAdj = "2" AndAlso sLDRateAdjBrMKT = "1" AndAlso sLDRateSpecials = "1" Then '
                hvLDRate1.Value = "0.2500"
                hvLDRate2.Value = "0.0833"
                hvLDRate3.Value = "0.0333"
                hvLDRate4.Value = "0.0167"
            End If

            '>70 N N Y  

            If sLDRateThreeYearLoss = "3165" AndAlso sLDRateBrMktAdj = "2" AndAlso sLDRateAdjBrMKT = "2" AndAlso sLDRateSpecials = "1" Then '
                hvLDRate1.Value = "0.2600"
                hvLDRate2.Value = "0.0867"
                hvLDRate3.Value = "0.0347"
                hvLDRate4.Value = "0.0173"
            End If

            '<70 Y Y N  

            If sLDRateThreeYearLoss = "3164" AndAlso sLDRateBrMktAdj = "1" AndAlso sLDRateAdjBrMKT = "1" AndAlso sLDRateSpecials = "2" Then '
                hvLDRate1.Value = "0.0850"
                hvLDRate2.Value = "0.0283"
                hvLDRate3.Value = "0.0113"
                hvLDRate4.Value = "0.0057"
            End If

            '<70 N Y N  
            If sLDRateThreeYearLoss = "3164" AndAlso sLDRateBrMktAdj = "2" AndAlso sLDRateAdjBrMKT = "1" AndAlso sLDRateSpecials = "2" Then '
                hvLDRate1.Value = "0.4500"
                hvLDRate2.Value = "0.1500"
                hvLDRate3.Value = "0.0600"
                hvLDRate4.Value = "0.0300"
            End If

            '>70 Y Y N  

            If sLDRateThreeYearLoss = "3165" AndAlso sLDRateBrMktAdj = "1" AndAlso sLDRateAdjBrMKT = "1" AndAlso sLDRateSpecials = "2" Then '
                hvLDRate1.Value = "0.3100"
                hvLDRate2.Value = "0.1033"
                hvLDRate3.Value = "0.0413"
                hvLDRate4.Value = "0.0207"
            End If

            '>70 N Y N   

            If sLDRateThreeYearLoss = "3165" AndAlso sLDRateBrMktAdj = "1" AndAlso sLDRateAdjBrMKT = "2" AndAlso sLDRateSpecials = "2" Then
                hvLDRate1.Value = "0.7000"
                hvLDRate2.Value = "0.2333"
                hvLDRate3.Value = "0.0933"
                hvLDRate4.Value = "0.0467"
            End If


            '<70 Y N N 

            If sLDRateThreeYearLoss = "3164" AndAlso sLDRateBrMktAdj = "1" AndAlso sLDRateAdjBrMKT = "2" AndAlso sLDRateSpecials = "2" Then '
                hvLDRate1.Value = "0.2600"
                hvLDRate2.Value = "0.0867"
                hvLDRate3.Value = "0.0347"
                hvLDRate4.Value = "0.0173"
            End If

            '<70 Y N Y

            If sLDRateThreeYearLoss = "3164" AndAlso sLDRateBrMktAdj = "1" AndAlso sLDRateAdjBrMKT = "2" AndAlso sLDRateSpecials = "1" Then '
                hvLDRate1.Value = "0.0900"
                hvLDRate2.Value = "0.0300"
                hvLDRate3.Value = "0.0120"
                hvLDRate4.Value = "0.0060"
            End If

            '>70 Y N N  

            If sLDRateThreeYearLoss = "3165" AndAlso sLDRateBrMktAdj = "2" AndAlso sLDRateAdjBrMKT = "1" AndAlso sLDRateSpecials = "2" Then '
                hvLDRate1.Value = "0.6500"
                hvLDRate2.Value = "0.2167"
                hvLDRate3.Value = "0.0867"
                hvLDRate4.Value = "0.0433"
            End If

            '<70 N N N 

            If sLDRateThreeYearLoss = "3164" AndAlso sLDRateBrMktAdj = "2" AndAlso sLDRateAdjBrMKT = "2" AndAlso sLDRateSpecials = "2" Then '
                hvLDRate1.Value = "0.5000"
                hvLDRate2.Value = "0.1667"
                hvLDRate3.Value = "0.0667"
                hvLDRate4.Value = "0.0333"
            End If

            '>70 N N N 

            If sLDRateThreeYearLoss = "3165" AndAlso sLDRateBrMktAdj = "2" AndAlso sLDRateAdjBrMKT = "2" AndAlso sLDRateSpecials = "2" Then '
                hvLDRate1.Value = "0.8000"
                hvLDRate2.Value = "0.2667"
                hvLDRate3.Value = "0.1067"
                hvLDRate4.Value = "0.0533"
            End If



        End Sub

		Public Overrides Sub PostDataSetWrite()
		End Sub

		Public Overrides Sub PreDataSetWrite()
		End Sub
		
	End Class
	
End Namespace
		