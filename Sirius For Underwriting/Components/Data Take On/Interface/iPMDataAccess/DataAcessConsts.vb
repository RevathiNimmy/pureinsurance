Option Strict Off
Option Explicit On
Imports System
Imports SharedFiles
Module DataAcessConsts
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_oDatabase As dPMDAO.Database
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_oAccessDatabase As dPMDAO.Database
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_oArchitectureDatabase As dPMDAO.Database
	
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_oParty As bSIRParty.Services
	'Public m_oParty As bSIRParty.Services
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_oObjectManager As bObjectManager.ObjectManager
	
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_oInsuranceFile As bSIRInsuranceFile.Services
	'Public m_oInsuranceFile As bSIRInsuranceFile.Services
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_oBusiness As Object
	'Public m_oBusiness As bSIRRiskScreen.Business
	
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_oUserDetail As bGISUserDefDetail.Business
	'Public m_oUserDetail As bGISUserDefDetail.Business
	
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_oLookupMaintenance As bPMMaintainLookup.Business
	'Public m_oLookupMaintenance As bPMMaintainLookup.Business
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_oArchLookupMaintenance As bPMMaintainLookup.Business
	'Public m_oArchLookupMaintenance As bPMMaintainLookup.Business
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_oAddressObject As Object
	'Public m_oAddressObject As bSIRAddress.Business
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_oPolicyNumber As Object
	'Private m_oPolicyNumber As bSIRPolicyNumMaint.Business
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_oOrionUpdate As Object
	'Private m_oOrionUpdate As bSIROrionUpdate.Business
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_oGIS As iGIS.Application
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vResultArray As Object
	
	'Constants for personal table
	Public Const m_cPersonalID As Integer = 0
	Public Const m_cPersonalTypeOfClient As Integer = 1
	Public Const m_cPersonalClientCode As Integer = 2
	Public Const m_cPersonalTitle As Integer = 3
	Public Const m_cPersonalLastName As Integer = 4
	Public Const m_cPersonalForename As Integer = 5
	Public Const m_cPersonalInitials As Integer = 6
	Public Const m_cPersonalLeadAgent As Integer = 7
	Public Const m_cPersonalAreaCode As Integer = 8
	Public Const m_cPersonalFileCode As Integer = 9
	Public Const m_cPersonalAddressLine1 As Integer = 10
	Public Const m_cPersonalAddressLine2 As Integer = 11
	Public Const m_cPersonalAddressLine3 As Integer = 12
	Public Const m_cPersonalAddressLine4 As Integer = 13
	Public Const m_cPersonalPostcode As Integer = 14
	Public Const m_cPersonalDateOfBirth As Integer = 15
	Public Const m_cPersonalContactType1 As Integer = 16
	Public Const m_cPersonalDescription1 As Integer = 17
	Public Const m_cPersonalAreaCode1 As Integer = 18
	Public Const m_cPersonalTelNumber1 As Integer = 19
	Public Const m_cPersonalExtNumber1 As Integer = 20
	Public Const m_cPersonalContactType2 As Integer = 21
	Public Const m_cPersonalDescription2 As Integer = 22
	Public Const m_cPersonalAreaCode2 As Integer = 23
	Public Const m_cPersonalTelNumber2 As Integer = 24
	Public Const m_cPersonalExtNumber2 As Integer = 25
	Public Const m_cPersonalContactType3 As Integer = 26
	Public Const m_cPersonalDescription3 As Integer = 27
	Public Const m_cPersonalAreaCode3 As Integer = 28
	Public Const m_cPersonalTelNumber3 As Integer = 29
	Public Const m_cPersonalExtNumber3 As Integer = 30
	Public Const m_cPersonalContactType4 As Integer = 31
	Public Const m_cPersonalDescription4 As Integer = 32
	Public Const m_cPersonalAreaCode4 As Integer = 33
	Public Const m_cPersonalTelNumber4 As Integer = 34
	Public Const m_cPersonalExtNumber4 As Integer = 35
	Public Const m_cPersonalContactType5 As Integer = 36
	Public Const m_cPersonalDescription5 As Integer = 37
	Public Const m_cPersonalAreaCode5 As Integer = 38
	Public Const m_cPersonalTelNumber5 As Integer = 39
	Public Const m_cPersonalExtNumber5 As Integer = 40
	Public Const m_cPersonalContactType6 As Integer = 41
	Public Const m_cPersonalDescription6 As Integer = 42
	Public Const m_cPersonalAreaCode6 As Integer = 43
	Public Const m_cPersonalTelNumber6 As Integer = 44
	Public Const m_cPersonalExtNumber6 As Integer = 45
	Public Const m_cPersonalPartyCount As Integer = 46
	
	'Constants for Corporate Table
	Public Const m_cCorporateID As Integer = 0
	Public Const m_cCorporateTypeOfClient As Integer = 1
	Public Const m_cCorporateClientCode As Integer = 2
	Public Const m_cCorporateTradingName As Integer = 3
	Public Const m_cCorporateLeadAgent As Integer = 4
	Public Const m_cCorporateAreaCode As Integer = 5
	Public Const m_cCorporateFileCode As Integer = 6
	Public Const m_cCorporateAddressLine1 As Integer = 7
	Public Const m_cCorporateAddressLine2 As Integer = 8
	Public Const m_cCorporateAddressLine3 As Integer = 9
	Public Const m_cCorporateAddressLine4 As Integer = 10
	Public Const m_cCorporatePostcode As Integer = 11
	Public Const m_cCorporateContactType1 As Integer = 12
	Public Const m_cCorporateDescription1 As Integer = 13
	Public Const m_cCorporateAreaCode1 As Integer = 14
	Public Const m_cCorporateTelNumber1 As Integer = 15
	Public Const m_cCorporateExtNumber1 As Integer = 16
	Public Const m_cCorporateContactType2 As Integer = 17
	Public Const m_cCorporateDescription2 As Integer = 18
	Public Const m_cCorporateAreaCode2 As Integer = 19
	Public Const m_cCorporateTelNumber2 As Integer = 20
	Public Const m_cCorporateExtNumber2 As Integer = 21
	Public Const m_cCorporateContactType3 As Integer = 22
	Public Const m_cCorporateDescription3 As Integer = 23
	Public Const m_cCorporateAreaCode3 As Integer = 24
	Public Const m_cCorporateTelNumber3 As Integer = 25
	Public Const m_cCorporateExtNumber3 As Integer = 26
	Public Const m_cCorporateContactType4 As Integer = 27
	Public Const m_cCorporateDescription4 As Integer = 28
	Public Const m_cCorporateAreaCode4 As Integer = 29
	Public Const m_cCorporateTelNumber4 As Integer = 30
	Public Const m_cCorporateExtNumber4 As Integer = 31
	Public Const m_cCorporateContactType5 As Integer = 32
	Public Const m_cCorporateDescription5 As Integer = 33
	Public Const m_cCorporateAreaCode5 As Integer = 34
	Public Const m_cCorporateTelNumber5 As Integer = 35
	Public Const m_cCorporateExtNumber5 As Integer = 36
	Public Const m_cCorporateContactType6 As Integer = 37
	Public Const m_cCorporateDescription6 As Integer = 38
	Public Const m_cCorporateAreaCode6 As Integer = 39
	Public Const m_cCorporateTelNumber6 As Integer = 40
	Public Const m_cCorporateExtNumber6 As Integer = 41
	Public Const m_cCorporatePartyCount As Integer = 42
	
	'Constants for motor details
	Public Const m_cMotorID As Integer = 0
	Public Const m_cMotorClientCode As Integer = 1
	Public Const m_cMotorPolicyNumber As Integer = 2
	Public Const m_cMotorStatus As Integer = 3
	Public Const m_cMotorProductType As Integer = 4
	Public Const m_cMotorBranch As Integer = 5
	Public Const m_cMotorAgentCode As Integer = 6
	Public Const m_cMotorTypeOfClient As Integer = 7
	Public Const m_cMotorAnalysisCode As Integer = 8
	Public Const m_cMotorBusinessType As Integer = 9
	Public Const m_cMotorRegarding As Integer = 10
	Public Const m_cMotorCoverFrom As Integer = 11
	Public Const m_cMotorCoverTo As Integer = 12
	Public Const m_cMotorOriginalInception As Integer = 13
	Public Const m_cMotorRenewal As Integer = 14
	Public Const m_cMotorProposalDate As Integer = 15
	Public Const m_cMotorIssued As Integer = 16
	Public Const m_cMotorOldPolicyNumber As Integer = 17
	Public Const m_cMotorCoInsurance As Integer = 18
	Public Const m_cMotorReferAtRenewal As Integer = 19
	Public Const m_cMotorCover As Integer = 20
	Public Const m_cMotorIsland As Integer = 21
	Public Const m_cMotorVehicleYear As Integer = 22
	Public Const m_cMotorVehicleMake As Integer = 23
	Public Const m_cMotorVehicleModel As Integer = 24
	Public Const m_cMotorVehicleModelType As Integer = 25
	Public Const m_cMotorUse As Integer = 26
	Public Const m_cMotorVehicleValue As Integer = 27
	Public Const m_cMotorEngineCC As Integer = 28
	Public Const m_cMotorVehicleSerialNumber As Integer = 29
	Public Const m_cMotorPlateNumber As Integer = 30
	Public Const m_cMotorCertificateNumber As Integer = 31
	Public Const m_cMotorTimeOfIssue As Integer = 32
	Public Const m_cMotorMileage As Integer = 33
	Public Const m_cMotorAlarm As Integer = 34
	Public Const m_cMotorGaraged As Integer = 35
	Public Const m_cMotorImport As Integer = 36
	Public Const m_cMotorInsuranceHistory As Integer = 37
	Public Const m_cMotorNCBReceived As Integer = 38
	Public Const m_cMotorNCBYears As Integer = 39
	Public Const m_cMotorMultiVehicleDiscount As Integer = 40
	Public Const m_cMotorClaimLoading As Integer = 41
	Public Const m_cMotorDriverName1 As Integer = 42
	Public Const m_cMotorDriverIsland1 As Integer = 43
	Public Const m_cMotorDriverSex1 As Integer = 44
	Public Const m_cMotorDriverDOB1 As Integer = 45
	Public Const m_cMotorDriverLicenseDate1 As Integer = 46
	Public Const m_cMotorDriverName2 As Integer = 47
	Public Const m_cMotorDriverIsland2 As Integer = 48
	Public Const m_cMotorDriverSex2 As Integer = 49
	Public Const m_cMotorDriverDOB2 As Integer = 50
	Public Const m_cMotorDriverLicenseDate2 As Integer = 51
	Public Const m_cMotorDriverName3 As Integer = 52
	Public Const m_cMotorDriverIsland3 As Integer = 53
	Public Const m_cMotorDriverSex3 As Integer = 54
	Public Const m_cMotorDriverDOB3 As Integer = 55
	Public Const m_cMotorDriverLicenseDate3 As Integer = 56
	Public Const m_cMotorDriverName4 As Integer = 57
	Public Const m_cMotorDriverIsland4 As Integer = 58
	Public Const m_cMotorDriverSex4 As Integer = 59
	Public Const m_cMotorDriverDOB4 As Integer = 60
	Public Const m_cMotorDriverLicenseDate4 As Integer = 61
	Public Const m_cMotorDriverName5 As Integer = 62
	Public Const m_cMotorDriverIsland5 As Integer = 63
	Public Const m_cMotorDriverSex5 As Integer = 64
	Public Const m_cMotorDriverDOB5 As Integer = 65
	Public Const m_cMotorDriverLicenseDate5 As Integer = 66
	Public Const m_cMotorInsuranceFileCount As Integer = 67
	
	'Constants for HouseHold details
	Public Const m_cHouseHoldID As Integer = 0
	Public Const m_cHouseHoldClientCode As Integer = 1
	Public Const m_cHouseHoldPolicyNumber As Integer = 2
	Public Const m_cHouseHoldStatus As Integer = 3
	Public Const m_cHouseHoldProductType As Integer = 4
	Public Const m_cHouseHoldBranch As Integer = 5
	Public Const m_cHouseHoldAgentCode As Integer = 6
	Public Const m_cHouseHoldTypeOfClient As Integer = 7
	Public Const m_cHouseHoldAnalysisCode As Integer = 8
	Public Const m_cHouseHoldBusinessType As Integer = 9
	Public Const m_cHouseHoldRegarding As Integer = 10
	Public Const m_cHouseHoldCoverFrom As Integer = 11
	Public Const m_cHouseHoldCoverTo As Integer = 12
	Public Const m_cHouseHoldOriginalInception As Integer = 13
	Public Const m_cHouseHoldRenewal As Integer = 14
	Public Const m_cHouseHoldProposalDate As Integer = 15
	Public Const m_cHouseHoldIssued As Integer = 16
	Public Const m_cHouseHoldOldPolicyNumber As Integer = 17
	Public Const m_cHouseHoldCoInsurance As Integer = 18
	Public Const m_cHouseHoldReferAtRenewal As Integer = 19
	Public Const m_cHouseHoldRiskAddress1 As Integer = 20
	Public Const m_cHouseHoldRiskAddress2 As Integer = 21
	Public Const m_cHouseHoldRiskAddress3 As Integer = 22
	Public Const m_cHouseHoldRiskAddress4 As Integer = 23
	Public Const m_cHouseHoldIsland As Integer = 24
	Public Const m_cHouseHoldAccumulationCode As Integer = 25
	Public Const m_cHouseHoldFloodingArea As Integer = 26
	Public Const m_cHouseHoldWaterFront As Integer = 27
	Public Const m_cHouseHoldOtherPerils As Integer = 28
	Public Const m_cHouseHoldCatastropheCover As Integer = 29
	Public Const m_cHouseHoldConstructionType As Integer = 30
	Public Const m_cHouseHoldIndexLinking As Integer = 31
	Public Const m_cHouseHoldBuildingDesc1 As Integer = 32
	Public Const m_cHouseHoldBuildingRef1 As Integer = 33
	Public Const m_cHouseHoldBuildingSum1 As Integer = 34
	Public Const m_cHouseHoldBuildingDate1 As Integer = 35
	Public Const m_cHouseHoldBuildingDesc2 As Integer = 36
	Public Const m_cHouseHoldBuildingRef2 As Integer = 37
	Public Const m_cHouseHoldBuildingSum2 As Integer = 38
	Public Const m_cHouseHoldBuildingDate2 As Integer = 39
	Public Const m_cHouseHoldBuildingDesc3 As Integer = 40
	Public Const m_cHouseHoldBuildingRef3 As Integer = 41
	Public Const m_cHouseHoldBuildingSum3 As Integer = 42
	Public Const m_cHouseHoldBuildingDate3 As Integer = 43
	Public Const m_cHouseHoldBuildingDesc4 As Integer = 44
	Public Const m_cHouseHoldBuildingRef4 As Integer = 45
	Public Const m_cHouseHoldBuildingSum4 As Integer = 46
	Public Const m_cHouseHoldBuildingDate4 As Integer = 47
	Public Const m_cHouseHoldBuildingDesc5 As Integer = 48
	Public Const m_cHouseHoldBuildingRef5 As Integer = 49
	Public Const m_cHouseHoldBuildingSum5 As Integer = 50
	Public Const m_cHouseHoldBuildingDate5 As Integer = 51
	Public Const m_cHouseHoldWatersideSum As Integer = 52
	Public Const m_cHouseHoldOwnerLiability As Integer = 53
	Public Const m_cHouseHoldAlternativeAcc As Integer = 54
	Public Const m_cHouseHoldLossPayeeName As Integer = 55
	Public Const m_cHouseHoldLossPayeeAddr1 As Integer = 56
	Public Const m_cHouseHoldLossPayeeAddr2 As Integer = 57
	Public Const m_cHouseHoldLossPayeeAddr3 As Integer = 58
	Public Const m_cHouseHoldLossPayeeAddr4 As Integer = 59
	Public Const m_cHouseHoldDeductible As Integer = 60
	Public Const m_cHouseHoldContentsSumInsured As Integer = 61
	Public Const m_cHouseHoldSatelliteSumInsured As Integer = 62
	Public Const m_cHouseHoldMoney As Integer = 63
	Public Const m_cHouseHoldAccidentalDamage As Integer = 64
	Public Const m_cHouseHoldFullTheft As Integer = 65
	Public Const m_cHouseHoldTempContentsRemoval As Integer = 66
	Public Const m_cHouseHoldPedalCycleSum As Integer = 67
	Public Const m_cHouseHoldEmployersLiabilitySum As Integer = 68
	Public Const m_cHouseHoldContentsDeductible As Integer = 69
	Public Const m_cHouseHoldPersonalUnspecifiedSum As Integer = 70
	Public Const m_cHouseHoldPossessionDescription1 As Integer = 71
	Public Const m_cHouseHoldPossessionReference1 As Integer = 72
	Public Const m_cHouseHoldPossessionSumInsured As Integer = 73
	Public Const m_cHouseHoldDateAdded1 As Integer = 74
	Public Const m_cHouseHoldPossessionDescription2 As Integer = 75
	Public Const m_cHouseHoldPossessionReference2 As Integer = 76
	Public Const m_cHouseHoldPossessionSumInsured2 As Integer = 77
	Public Const m_cHouseHoldDateAdded2 As Integer = 78
	Public Const m_cHouseHoldPossessionDescription3 As Integer = 79
	Public Const m_cHouseHoldPossessionReference3 As Integer = 80
	Public Const m_cHouseHoldPossessionSumInsured3 As Integer = 81
	Public Const m_cHouseHoldDateAdded3 As Integer = 82
	Public Const m_cHouseHoldPossessionDescription4 As Integer = 83
	Public Const m_cHouseHoldPossessionReference4 As Integer = 84
	Public Const m_cHouseHoldPossessionSumInsured4 As Integer = 85
	Public Const m_cHouseHoldDateAdded4 As Integer = 86
	Public Const m_cHouseHoldPossessionDescription5 As Integer = 87
	Public Const m_cHouseHoldPossessionReference5 As Integer = 88
	Public Const m_cHouseHoldPossessionSumInsured5 As Integer = 89
	Public Const m_cHouseHoldDateAdded5 As Integer = 90
	Public Const m_cHouseHoldPossessionDescription6 As Integer = 91
	Public Const m_cHouseHoldPossessionReference6 As Integer = 92
	Public Const m_cHouseHoldPossessionSumInsured6 As Integer = 93
	Public Const m_cHouseHoldDateAdded6 As Integer = 94
	Public Const m_cHouseHoldPossessionDescription7 As Integer = 95
	Public Const m_cHouseHoldPossessionReference7 As Integer = 96
	Public Const m_cHouseHoldPossessionSumInsured7 As Integer = 97
	Public Const m_cHouseHoldDateAdded7 As Integer = 98
	Public Const m_cHouseHoldSingleItemLimit As Integer = 99
	Public Const m_cHouseHoldPossessionDescription8 As Integer = 100
	Public Const m_cHouseHoldPossessionReference8 As Integer = 101
	Public Const m_cHouseHoldPossessionSumInsured8 As Integer = 102
	Public Const m_cHouseHoldDateAdded8 As Integer = 103
	Public Const m_cHouseHoldPossessionDescription9 As Integer = 104
	Public Const m_cHouseHoldPossessionReference9 As Integer = 105
	Public Const m_cHouseHoldPossessionSumInsured9 As Integer = 106
	Public Const m_cHouseHoldDateAdded9 As Integer = 107
	Public Const m_cHouseHoldHighRiskDescription1 As Integer = 108
	Public Const m_cHouseHoldHighRiskReference1 As Integer = 109
	Public Const m_cHouseHoldHighRiskSum1 As Integer = 110
	Public Const m_cHouseHoldHighRiskDate1 As Integer = 111
	Public Const m_cHouseHoldHighRiskValuation1 As Integer = 112
	Public Const m_cHouseHoldHighRiskValuationDate1 As Integer = 113
	Public Const m_cHouseHoldHighRiskDescription2 As Integer = 114
	Public Const m_cHouseHoldHighRiskReference2 As Integer = 115
	Public Const m_cHouseHoldHighRiskSum2 As Integer = 116
	Public Const m_cHouseHoldHighRiskDate2 As Integer = 117
	Public Const m_cHouseHoldHighRiskValuation2 As Integer = 118
	Public Const m_cHouseHoldHighRiskValuationDate2 As Integer = 119
	Public Const m_cHouseHoldHighRiskDescription3 As Integer = 120
	Public Const m_cHouseHoldHighRiskReference3 As Integer = 121
	Public Const m_cHouseHoldHighRiskSum3 As Integer = 122
	Public Const m_cHouseHoldHighRiskDate3 As Integer = 123
	Public Const m_cHouseHoldHighRiskValuation3 As Integer = 124
	Public Const m_cHouseHoldHighRiskValuationDate3 As Integer = 125
	Public Const m_cHouseHoldHighRiskDescription4 As Integer = 126
	Public Const m_cHouseHoldHighRiskReference4 As Integer = 127
	Public Const m_cHouseHoldHighRiskSum4 As Integer = 128
	Public Const m_cHouseHoldHighRiskDate4 As Integer = 129
	Public Const m_cHouseHoldHighRiskValuation4 As Integer = 130
	Public Const m_cHouseHoldHighRiskValuationDate4 As Integer = 131
	Public Const m_cHouseHoldHighRiskDescription5 As Integer = 132
	Public Const m_cHouseHoldHighRiskReference5 As Integer = 133
	Public Const m_cHouseHoldHighRiskSum5 As Integer = 134
	Public Const m_cHouseHoldHighRiskDate5 As Integer = 135
	Public Const m_cHouseHoldHighRiskValuation5 As Integer = 136
	Public Const m_cHouseHoldHighRiskValuationDate5 As Integer = 137
	Public Const m_cHouseHoldHighRiskDescription6 As Integer = 138
	Public Const m_cHouseHoldHighRiskReference6 As Integer = 139
	Public Const m_cHouseHoldHighRiskSum6 As Integer = 140
	Public Const m_cHouseHoldHighRiskDate6 As Integer = 141
	Public Const m_cHouseHoldHighRiskValuation6 As Integer = 142
	Public Const m_cHouseHoldHighRiskValuationDate6 As Integer = 143
	Public Const m_cHouseHoldHighRiskDescription7 As Integer = 144
	Public Const m_cHouseHoldHighRiskReference7 As Integer = 145
	Public Const m_cHouseHoldHighRiskSum7 As Integer = 146
	Public Const m_cHouseHoldHighRiskDate7 As Integer = 147
	Public Const m_cHouseHoldHighRiskValuation7 As Integer = 148
	Public Const m_cHouseHoldHighRiskValuationDate7 As Integer = 149
	Public Const m_cHouseHoldHighRiskDescription8 As Integer = 150
	Public Const m_cHouseHoldHighRiskReference8 As Integer = 151
	Public Const m_cHouseHoldHighRiskSum8 As Integer = 152
	Public Const m_cHouseHoldHighRiskDate8 As Integer = 153
	Public Const m_cHouseHoldHighRiskValuation8 As Integer = 154
	Public Const m_cHouseHoldHighRiskValuationDate8 As Integer = 155
	Public Const m_cHouseHoldHighRiskDescription9 As Integer = 156
	Public Const m_cHouseHoldHighRiskReference9 As Integer = 157
	Public Const m_cHouseHoldHighRiskSum9 As Integer = 158
	Public Const m_cHouseHoldHighRiskDate9 As Integer = 159
	Public Const m_cHouseHoldHighRiskValuation9 As Integer = 160
	Public Const m_cHouseHoldHighRiskValuationDate9 As Integer = 161
	Public Const m_cHouseHoldInsuranceFileCount As Integer = 162
	
    'Variables to hold retrieved motor ID fields
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vMotorClientCode As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vMotorAnalysisCode As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vMotorBranch As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vMotorBusinessType As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vMotorStatus As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vMotorCover As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vMotorVehicleModel As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vMotorUse As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vMotorAlarm As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vMotorImport As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vMotorVehicleMake As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vMotorGaraged As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vMotorNCBYears As Object
	
    'Variables to hold retrieved household ID fields
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vHouseHoldClientCode As Byte
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vHouseHoldAnalysisCode As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vHouseHoldBranch As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vHouseHoldStatus As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vHouseHoldIsland As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vHouseHoldAccumCode As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vHouseHoldFloodingArea As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vHouseHoldConstruction As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vHouseHoldIndexLinking As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vHouseHoldDeductible As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vHouseHoldContentsDeductible As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vHouseHoldMoney As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vMotorArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vHouseHoldArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vPersonalArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vCorporateArray As Object
	
    'Copied lookup arrays
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vAlarmArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vConstructionTypeArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vContentsDeductibleArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vCoverArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vFloodingAreaArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vGaragedArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vHouseHoldDeductibleArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vImportArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vIndexLinkingArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vIslandArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vMoneyArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vMotorCoverArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vVehicleMakeArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vVehicleModelArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vVehicleUseArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vAccumulationCodeArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vAnalysisCodeArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
    Public m_vAreaCodeArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vBranchArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vBusinessTypeArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vStatusArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
    Public m_vNCBYearsArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vLeadAgentArray As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vLeadAgentIDArray As Object
	
    'Variables for object manager
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserId As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iCurrencyID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLogLevel As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sUserName As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sPassword As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sCallingAppName As String = ""
	
	Public Const ACAPP As String = "iPMDataAccess"
	Public Const ACClass As String = "clsAccessFunctions"
	
    'Variables for the party counts
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vPersonalPartyCount As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vCorporatePartyCount As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vMotorInsuranceFileCount As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vMotorInsuranceFolderCount As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vHouseHoldInsuranceFileCount As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vHouseHoldInsuranceFolderCount As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vPersonalContactCount As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vCorporateContactCount As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vPersonalAddressCount As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vCorporateAddressCount As Object
	
	'SQL Stored Procedures Constants
	Public Const ACSQLCaptionReturn As String = "{call spu_pm_caption_id_return(?,?,?)}"
	Public Const ACSQLCaptionReturnName As String = "GetCaptionID"
	Public Const ACSQLCaptionReturnStored As Boolean = True
	
	' Insert All Lookup Details SQL
	Public Const ACInsertLookupDetailsStored As Boolean = True
	Public Const ACInsertLookupDetailsName As String = "InsertLookupDetails"
	Public Const ACInsertLookupDetailsSQL As String = "{call spe_GIS_user_def_detail_add (?,?,?,?,?,?,?,?)}"
	
	' Check Party SQL
	Public Const ACCheckPartyStored As Boolean = False
	Public Const ACCheckPartyName As String = "ACCheckParty"
	Public Const ACCheckPartySQL As String = "Select party_cnt from Party where shortname = {shortname}"
	
	' Get Resolved Name SQL
	Public Const ACGetResolvedNameStored As Boolean = False
	Public Const ACGetResolvedNameName As String = "ACGetResolvedName"
	Public Const ACGetResolvedNameSQL As String = "Select resolved_name from Party where party_cnt = {party_cnt}"
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_iTask As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_lNavigate As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_lProcessMode As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_sTransactionType As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_dtEffectiveDate As Date
	
	Private m_lInsuranceFileCnt As Integer
	Private m_lProductID As Integer
	Private m_lScreenID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_lRiskID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_lRiskTypeID As Integer
End Module