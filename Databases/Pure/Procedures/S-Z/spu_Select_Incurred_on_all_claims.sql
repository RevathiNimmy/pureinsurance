SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF 
GO
EXECUTE DDLDropProcedure spu_Select_Incurred_on_all_claims
GO
CREATE PROCEDURE spu_Select_Incurred_on_all_claims
@party_cnt INT
AS


CREATE TABLE #tmpClaim(version_id INT,Claim_Number VARCHAR(30))

INSERT INTO #tmpClaim SELECT MAX(version_id),Claim_Number FROM  claim(NOLOCK) c JOIN Insurance_File(NOLOCK) ifi ON c.Policy_id = ifi.insurance_file_cnt WHERE ifi.insured_cnt = @party_cnt and is_dirty=0 GROUP BY Claim_Number


SELECT  
         ISNULL(SUM(r.Initial_reserve + r.revised_reserve),0) as Claim_Incurred

    FROM

        Reserve(NOLOCK) r

        join claim_peril(NOLOCK) cp on r.claim_peril_id = cp.claim_peril_id

        join claim(NOLOCK) c on cp.claim_id = c.claim_id

		join Insurance_File(NOLOCK) ifi on ifi.insurance_file_cnt = c.Policy_id

    WHERE r.version_id=(SELECT MAX(version_id) FROM  #tmpClaim WHERE claim_Number= c.claim_Number )

	And ifi.insured_cnt = @party_cnt

    DROP TABLE #tmpClaim 
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO