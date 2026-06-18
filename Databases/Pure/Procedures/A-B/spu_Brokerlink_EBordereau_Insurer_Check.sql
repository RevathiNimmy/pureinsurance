ddldropprocedure 'spu_Brokerlink_EBordereau_Insurer_Check'
go

CREATE PROCEDURE spu_Brokerlink_EBordereau_Insurer_Check
    @cashlist_id INT,
    @PartyType VARCHAR(10) OUTPUT,
    @PartyShortname VARCHAR(20) OUTPUT,
    @BlSubaccount INT OUTPUT,
    @BlUnderwriterId VARCHAR(10) OUTPUT
AS

SELECT 
	@PartyType = RTRIM(PT.code),
	@PartyShortname = p.shortname,
	@BlSubaccount = pi.Brokerlink_Subaccount,
	@BlUnderwriterId = pi.Brokerlink_UW_ID
FROM Party_Type PT
JOIN Party P
    ON P.party_type_id = PT.party_type_id
left outer join party_insurer PI
    ON PI.party_cnt = p.party_cnt
JOIN Account A
    ON A.account_key = P.party_cnt
JOIN CashListItem CLI
    ON CLI.account_id = A.account_id
WHERE CLI.cashlist_id = @cashlist_id
