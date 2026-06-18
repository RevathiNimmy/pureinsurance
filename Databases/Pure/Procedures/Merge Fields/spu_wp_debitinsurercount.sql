SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_debitinsurercount'
GO


CREATE PROCEDURE spu_wp_debitinsurercount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

/*Remove shared indicator from document_ref if it is there*/
DECLARE @SharedIndicator int

SELECT @SharedIndicator = CHARINDEX('|',@DocumentRef)

If @SharedIndicator <> 0
BEGIN
    SELECT @DocumentRef = SUBSTRING(@DocumentRef, 1, @SharedIndicator -1)
END

/*Get the number of insurers on this account*/
SELECT COUNT(DISTINCT a.account_id) 'how_many'
FROM document d
JOIN transdetail td
    ON td.document_id = d.document_id 
JOIN account a
    ON a.account_id = td.account_id
JOIN party p
    ON p.party_cnt = a.account_key
JOIN party_type pt
    ON pt.party_type_id = p.party_type_id
WHERE d.document_ref = @DocumentRef
AND d.insurance_file_cnt = @InsuranceFileCnt
AND pt.code = 'IN'


