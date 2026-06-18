EXECUTE DDLDropProcedure 'spu_PFRF_del'
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE spu_PFRF_del
    @PFRF_ID int,
    @StartDate datetime,
    @ProductFamily char(3),
	@UserId INT,
	@UniqueId VARCHAR(50),
	@ScreenHierarchy VARCHAR(500)=''
AS

UPDATE p SET UserId=@UserId,UniqueId=@UniqueId,ScreenHierarchy=ISNULL(ScreenHierarchy,@ScreenHierarchy + ' / ' + ProductFamily + '(' + pf.description + ')(' + CONVERT(VARCHAR(10), StartDate, 105) + '-' + CONVERT(VARCHAR(10), EndDate, 105) + ')' ) 
FROM PFRF p INNER JOIN PFFrequency pf ON p.pffrequency_id=pf.pffrequency_id 
WHERE
    PFRF_ID = @PFRF_ID
AND StartDate = @StartDate
AND ProductFamily = @ProductFamily
AND NOT EXISTS (SELECT 1 FROM PFPremiumFinance
	          WHERE PFRF_ID = @PFRF_ID)
DELETE FROM
    PFRF
WHERE
    PFRF_ID = @PFRF_ID
AND StartDate = @StartDate
AND ProductFamily = @ProductFamily
AND NOT EXISTS (SELECT 1 FROM PFPremiumFinance
	          WHERE PFRF_ID = @PFRF_ID)
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
