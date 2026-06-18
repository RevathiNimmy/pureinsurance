ddldropprocedure spu_wp_debitSection_Get_Keys
go

CREATE PROCEDURE spu_wp_debitSection_Get_Keys
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
	eics.insurance_section_id
FROM
	insurance_file inf
	JOIN document d ON d.company_id=inf.source_id AND d.insurance_file_cnt=inf.insurance_file_cnt
	JOIN transaction_export_folder tef ON tef.document_ref = d.document_ref AND tef.source_id = d.company_id AND tef.accounts_export_status = 'c'
	JOIN event_log e ON e.event_cnt = tef.event_log_id
	join event_insurance_cob_section eics on eics.insurance_file_cnt = e.event_cnt
	join insurance_file iff on iff.insurance_file_cnt=tef.insurance_file_cnt
	left outer join risk_tax_usage rtu on rtu.risk_code_id=iff.risk_code_id and rtu.cob_rating_section_id = eics.cob_rating_section_id
WHERE
	inf.insurance_file_cnt=@InsuranceFileCnt AND
	d.document_ref = @DocumentRef
order by rtu.sequence