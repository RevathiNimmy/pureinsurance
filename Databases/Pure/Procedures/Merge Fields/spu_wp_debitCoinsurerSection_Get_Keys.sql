ddldropprocedure spu_wp_debitCoinsurerSection_Get_Keys
go


CREATE PROCEDURE spu_wp_debitCoinsurerSection_Get_Keys
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

DECLARE @SharedIndicator INT

SELECT @SharedIndicator = CHARINDEX('|', @DocumentRef)

IF @SharedIndicator <> 0
	SELECT @DocumentRef = SUBSTRING(@DocumentRef, 1, @SharedIndicator - 1)

SELECT
	epcs.policy_coinsurers_section_id
FROM document d
	JOIN transaction_export_folder tef ON tef.document_ref = d.document_ref AND tef.source_id = d.company_id AND tef.accounts_export_status = 'c'
	JOIN event_log e ON e.event_cnt = tef.event_log_id
	join event_policy_coinsurers_section epcs on epcs.insurance_file_cnt = e.event_cnt
WHERE 	d.document_ref = @DocumentRef and epcs.party_cnt=@instance2