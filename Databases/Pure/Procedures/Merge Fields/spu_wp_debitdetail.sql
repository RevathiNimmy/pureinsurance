if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spu_wp_debitdetail]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spu_wp_debitdetail]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO



CREATE PROCEDURE spu_wp_debitdetail
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

--DC120203 -ISS1405 -start -document ref can now have a shared premium indicator on the end, so remove it
DECLARE @SharedIndicator int

SELECT @SharedIndicator = CHARINDEX('|',@DocumentRef)

If @SharedIndicator <> 0
BEGIN
	SELECT @DocumentRef = SUBSTRING(@DocumentRef, 1, @SharedIndicator -1)
END
--DC120203 -end

--sj 01/08/2002 - Start
/*
SELECT  ted.transaction_export_folder_cnt,
    ted.transaction_export_detail_id,
    ted.transaction_amount,
    ted.transaction_ledger_code,
    ted.account_type_code,
    ted.transaction_account_key,
    ted.ceded_ref,

    ted.cover_share_percent,
    ted.sum_insured_total,
    ted.charges_total,
    ted.taxes_total,
    ted.recoveries_total,
    ted.commission_excluded,
    ted.withholding_tax_excluded,
    ted.mapping_code,
    ted.spare

FROM    transaction_export_folder tef,
    transaction_export_detail ted
WHERE   tef.document_ref = @DocumentRef
AND tef.transaction_export_folder_cnt = ted.transaction_export_folder_cnt
AND ted.transaction_export_detail_id = @Instance1
*/

SELECT 0,                                --ted.transaction_export_folder_cnt
        td.transdetail_id,                   --ted.transaction_export_detail_id
        td.amount,                           --ted.transaction_amount
        l.ledger_short_name ,                --ted.transaction_ledger_code
        at.code,                             --ted.account_type_code
        a.account_key,                       --ted.transaction_account_key
        ' ',                                 --ted.ceded_ref
        0,                                   --ted.cover_share_percent
        0,                                   --ted.sum_insured_total
        i.vat_amount,                        --ted.charges_total
        i.tax_amount,                        --ted.taxes_total
        0,                                   --ted.recoveries_total
        0,                                   --ted.commission_excluded
        0,                                   --ted.withholding_tax_excluded
        p.shortname,                         --ted.mapping_code
        td.spare                             --ted.spare
    FROM document d,
         transdetail td,
         insurance_file i,
         party p,
         account a,
         accounttype at,
         ledger l
    WHERE d.document_ref = @DocumentRef
    AND   td.transdetail_id  = @Instance1
    AND   d.insurance_file_cnt = @InsuranceFileCnt
    AND   i.insurance_file_cnt = @InsuranceFileCnt
    AND   d.document_id = td.document_id
    AND   i.insured_cnt = p.party_cnt
    AND   td.account_id = a.account_id
    AND   at.accounttype_id = a.accounttype_id
    AND   a.ledger_id = l.ledger_id
    
    --sj 01/08/2002 - Start


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

