SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXECUTE DDLDROPPROCEDURE 'spu_wp_PolicyLinePercentage'
go
CREATE  PROCEDURE spu_wp_PolicyLinePercentage 
	 @PartyCnt INT,
     @InsuranceFileCnt INT,
     @RiskID INT,
     @ClaimCnt INT,
     @DocumentRef VARCHAR(25),
     @Instance1 INT,
     @Instance2 INT,
     @Instance3 INT
AS

DECLARE
    
    @total_written_percentage numeric(19,4),
    @total_signed_percentage numeric(19,4)


SELECT
    @total_written_percentage = ROUND(sum(pc.written_line_percentage),2),
    @total_signed_percentage = ROUND(sum(pc.signed_line_percentage),2)
    
FROM policy_coinsurers pc
WHERE pc.insurance_file_cnt = @InsuranceFileCnt

SELECT
	'policy_total_written_percentage' = @total_written_percentage,
    'policy_total_signed_percentage' = @total_signed_percentage
    
    
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO