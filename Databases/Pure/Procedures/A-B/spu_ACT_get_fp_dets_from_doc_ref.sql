SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_get_fp_dets_from_doc_ref'
GO

CREATE PROCEDURE spu_ACT_get_fp_dets_from_doc_ref
    @document_ref VARCHAR(25),
    @company_id INT
AS

If @company_id = 0 
BEGIN
	SELECT @company_id = company_id from document where document_ref = @document_ref
END

If @company_id = 0 
BEGIN
 SELECT @company_id = company_id from document where document_ref = @document_ref
END

SELECT 
    pf.pfprem_finance_cnt,
    pf.pfprem_finance_version
FROM document d
JOIN transdetail td
    ON td.document_id = d.document_id
JOIN pfpremiumfinance pf
    ON pf.plantransaction_id = td.transdetail_id 
WHERE d.document_ref = @document_ref
AND d.company_id = @company_id

UNION

SELECT 
    pf.pfprem_finance_cnt,
    pf.pfprem_finance_version
FROM document d
JOIN transdetail td
    ON td.document_id = d.document_id
JOIN pfinstalments pf
    ON pf.pftransaction_id = td.transdetail_id 
WHERE d.document_ref = @document_ref
AND d.company_id = @company_id

GO