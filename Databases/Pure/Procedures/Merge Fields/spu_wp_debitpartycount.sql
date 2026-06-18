SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_debitpartycount'
GO


CREATE PROCEDURE spu_wp_debitpartycount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
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

--sj 01/08/2002 -Start

/*
SELECT  count(ted.transaction_export_detail_id) "how_many"

FROM    transaction_export_folder tef,
    transaction_export_detail ted,
    party p,
    party_type pt
WHERE   tef.document_ref = @DocumentRef
AND tef.transaction_export_folder_cnt = ted.transaction_export_folder_cnt
AND ted.transaction_account_key = p.party_id
AND p.party_type_id = pt.party_type_id
AND pt.code IN ("PC", "CC", "GC")
GO */

SELECT count(td.transdetail_id) "how_many"  
        
    FROM document d,
         transdetail td,
         insurance_file i,
         party p,
         party_type pt
         
    WHERE d.document_ref = @DocumentRef
    AND   d.insurance_file_cnt = @InsuranceFileCnt
    AND   i.insurance_file_cnt = @InsuranceFileCnt
    AND   d.document_id = td.document_id
    AND   i.insured_cnt = p.party_cnt
    AND   p.party_type_id = pt.party_type_id
    AND   pt.code IN ('PC', 'CC', 'GC') 


--sj 01/08/2002 -End