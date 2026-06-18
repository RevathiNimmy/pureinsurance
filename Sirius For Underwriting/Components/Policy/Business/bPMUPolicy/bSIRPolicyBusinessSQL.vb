Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Module BusinessSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: BusinessSQL
    '
    ' Created: PW301002
    '
    ' Description: Contains the SQL Statements required by the
    '              bPMUPolicy.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '
    'developer guide no 39. start

    'SQL Statements
    Public Const ACGetGracePeriodStored As Boolean = True
    Public Const ACGetGracePeriodName As String = "GetGracePeriod"

    Public Const ACGetGracePeriodSQL As String = "spu_get_grace_period"

    Public Const ACSetRisksUnquotedStored As Boolean = True
    Public Const ACSetRisksUnquotedName As String = "SetRisksUnquoted"

    Public Const ACSetRisksUnquotedSQL As String = "spu_update_risk_status_unquoted"

    Public Const ACSetRisksQuoteStatusNBStored As Boolean = True
    Public Const ACSetRisksQuoteStatusNBName As String = "SetRisksQuoteStatusNB"

    Public Const ACSetRisksQuoteStatusNBSQL As String = "spu_update_risk_status_Quote_StatusNB"

    Public Const ACSetRisksQuoteStatusMTAStored As Boolean = True
    Public Const ACSetRisksQuoteStatusMTAName As String = "SetRisksQuoteStatusMTA"

    Public Const ACSetRisksQuoteStatusMTASQL As String = "spu_update_risk_status_Quote_StatusMTA"

    'CJR 17/1/2003  added for a more generic risk status update.
    Public Const ACSetRisksStatusStored As Boolean = True
    Public Const ACSetRisksStatusName As String = "SetRisksUnquoted"

    Public Const ACSetRisksStatusSQL As String = "spu_update_risk_status"
    'Start (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.3.1)
    Public Const ACGetClientCode As String = "GetClientCode"
    Public Const ACGetClientCodeStored As Boolean = True

    Public Const ACGetClientCodeSQL As String = "spu_get_ClientCode"

    'End (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.3.1)
    'vivek 63205
    Public Const ACBackDatedCanAllowedStored As Boolean = True
    Public Const ACBackDatedCanAllowedName As String = "BackDatedCanAllowed"
    Public Const ACBackDatedCanAllowedSQL As String = "spe_BackdatedCan_Allowed"
    'vivek63205

    'Get Branch Default Agent
    Public Const ACGetBranchDefAgentName As String = "GetBranchDefAgentName"
    Public Const ACGetBranchDefAgentSQL As String = "Select agent_id,name " & Strings.Chr(13) & Strings.Chr(10) &
                                                    "From source_defaults  " & Strings.Chr(13) & Strings.Chr(10) &
                                                    "Inner Join Party on source_defaults.agent_id=party.party_cnt " & Strings.Chr(13) & Strings.Chr(10) &
                                                    "where source_defaults.source_id={Source_id}"

    'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTA - Product Option.doc) - (5.1.3.1)
    Public Const ACBackDatedMTAsAllowedStored As Boolean = True
    Public Const ACBackDatedMTAsAllowedName As String = "BackDatedMTAsAllowed"

    Public Const ACBackDatedMTAsAllowedSQL As String = "spe_BackdatedMTAs_Allowed"
    'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTA - Product Option.doc) - (5.1.3.1)
    'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs - MTA on a Cancelled Policy.doc) - (5.2.1.1)
    Public Const ACGetInsuranceFileStatusStored As Boolean = True
    Public Const ACGetInsuranceFileStatusName As String = "GetInsuranceFileStatus"

    Public Const ACGetInsuranceFileStatusSQL As String = "spu_SIR_GetInsuranceFileStatus"
    'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs - MTA on a Cancelled Policy.doc) - (5.2.1.1)

    'Defect #2748
    Public Const ACGetAssociatedAgent As String = "GetBranchDefAgentName"
    Public Const ACGetAssociatedAgentSQL As String = "Select p.party_cnt,  p.ShortName from Party P inner join PMUser PMU on P.party_cnt=PMU.party_cnt and PMU.user_id={User_id} Inner Join Party_Type PT on p.party_type_id = PT.party_type_id Where PT.party_type_id = 3"

    'Public Const ACGetAssociatedAgentSQL As String = "Select p.party_cnt,  p.ShortName from Party P inner join PMUser PMU on P.party_cnt=PMU.party_cnt and PMU.user_id={User_id}"
    'Public Const ACGetAssociatedAgentSQL As String = "Select p.party_cnt,  p.ShortName from Party P inner join Party_Agent_Branch PAB on P.party_cnt=PAB.party_cnt and PAB.Party_cnt=(Select distinct P.party_cnt from Party P inner join PMUser PMU on P.party_cnt=PMU.party_cnt ,Party_Agent_Branch PAB where PMU.user_id={User_id}) and   PAB.source_id ={Source_id}"
    'Defect #2748
    Public Const ACGetAssociatedAgentBranch As String = "GetSelectedBranchAgentName"
    Public Const ACGetAssociatedAgentBranchSQL As String = "Select p.party_cnt,  p.ShortName from Party P inner join Party_Agent_Branch PAB on P.party_cnt=PAB.party_cnt and PAB.Party_cnt=(Select distinct P.party_cnt from Party P inner join PMUser PMU on P.party_cnt=PMU.party_cnt ,Party_Agent_Branch PAB where PMU.user_id={User_id}) and   PAB.source_id ={Source_id}"
    Public Const ACSetRisksInceptionDateStored = True
    Public Const ACSetRisksInceptionDateName = "SetRisksInceptionDate"
    Public Const ACSetRisksInceptionDateSQL = "spu_Set_Risks_Inception_Date"

    Public Const ACGetInsFileTypeName As String = "Get Insurance_file Type"
    Public Const ACGetInsFileTypeSQL As String = "Select ift.code from Insurance_file Ifile " & Strings.Chr(13) & Strings.Chr(10) &
                                                 "Inner Join Insurance_file_type IFT ON Ifile.Insurance_file_type_id=IFT.Insurance_file_type_id " & Strings.Chr(13) & Strings.Chr(10) &
                                                 "And Ifile.insurance_file_cnt={InsFileCnt}"

    Public Const ACOutOfSequenceMTADetailsName As String = "Out of Sequence MTA Details"
    Public Const ACOutOfSequenceMTADetailsSQL As String = "spu_Get_out_of_sequence_mta_details"
    Public Const ACOutOfSequenceMTADetailsStored As Boolean = True

    Public Const ACSetRisksRIStored As Boolean = True
    Public Const ACSetRisksRIName As String = "DeleteRisksRI"
    Public Const ACSetRisksRISQL As String = "spu_sir_del_risks_ri"

    Public Const kGetAssosiatedAgentBranchStored As Boolean = True
    Public Const kGetAssosiatedAgentBranchName As String = "GetAssosiatedAgentBranch"
    Public Const kGetAssosiatedAgentBranchSQL As String = "spe_Agent_PLLSource"


    Public Const ACRetainRenewalQuoteOnMTAStored As Boolean = True
    Public Const ACRetainRenewalQuoteOnMTAName As String = "IsRetainRenQuoteVersion"
    Public Const ACRetainRenewalQuoteOnMTASQL As String = "spu_Is_Retain_Renewal_Quote_On_MTA"


End Module

