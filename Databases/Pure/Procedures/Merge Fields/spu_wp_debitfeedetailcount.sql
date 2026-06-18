SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_debitfeedetailcount'
GO

CREATE PROCEDURE spu_wp_debitfeedetailcount
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
BEGIN
   SELECT @DocumentRef = SUBSTRING(@DocumentRef, 1, @SharedIndicator -1)
END

select 	count(td.transdetail_id) how_many
from document d
join transdetail td on d.document_id = td.document_id
join account a on td.account_id = a.account_id
join ledger l on a.ledger_id = l.ledger_id
join party p on a.account_key = p.party_cnt
join party_type pt on p.party_type_id = pt.party_type_id
join accounttype at ON at.accounttype_id = a.accounttype_id
where 	pt.code = 'FE'
and	(d.document_ref = @DocumentRef OR ISNULL(@DocumentRef,'')='')
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



