EXEC DDLDropProcedure spu_rptSSRS_CanceledLapsedPolicies
GO
CREATE PROCEDURE spu_rptSSRS_CanceledLapsedPolicies
	@BranchName VARCHAR(255)
	,@DateFrom DATETIME
	,@DateTo DATETIME
AS

BEGIN

	SELECT Policy_Insurance_ref AS 'Policy Number'
			, insured_name AS 'Name of Insured'
			,(
				SELECT SUM(Amount) 
				FROM [AccountsBalanceStatement] AB
				INNER JOIN [qryAllPolicies] on [qryAllPolicies].party_cnt = AB.ClientID
				) AS 'RefundAmount'
			, lapsed_description
			,ISNULL(lapsed_date,[expiry_date]) as [CancelLapseDate]
	FROM [dbo].[qryAllPolicies]
	WHERE FileStatus IN ('Cancelled','Lapsed')
		AND Branch = @BranchName	
		AND ISNULL(lapsed_date,[expiry_date]) BETWEEN @DateFrom AND @DateTo

END