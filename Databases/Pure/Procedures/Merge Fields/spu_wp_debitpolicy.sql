SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_debitpolicy'
GO

CREATE PROCEDURE spu_wp_debitpolicy
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

SELECT
	eics.premium_including_tax iptable_amount,
	etc.percentage ipt_percentage,
	ei.vatable_amount,
	ei.vat_percentage
FROM event_insurance_file ei
JOIN event_log e
ON e.event_cnt = ei.insurance_file_cnt
JOIN transaction_export_folder tef
ON tef.transaction_export_folder_cnt = e.transaction_export_folder_cnt
JOIN event_insurance_cob_section eics on ei.insurance_file_cnt = eics.insurance_file_cnt 
JOIN tax_group tg on tg.tax_group_id = eics.tax_group_id
AND tg.code = 'IPT'
JOIN event_tax_calculation etc on etc.insurance_section_id = eics.insurance_section_id
AND etc.transtype <> 'TTIC' 
WHERE tef.document_ref = @DocumentRef
AND tef.insurance_file_cnt = @InsuranceFileCnt
AND tef.accounts_export_status = 'c'




GO