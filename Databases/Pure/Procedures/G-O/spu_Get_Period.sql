SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_Period'
GO

CREATE PROCEDURE spu_Get_Period
AS
DECLARE @vOutputDate        DATETIME 
DECLARE @OutPutDate varchar(30)
DECLARE @OutPutDateTime datetime
    SET @vOutputDate = CAST(YEAR(Getdate()) AS VARCHAR(4)) + '/' + 
                     CAST(MONTH(Getdate()) AS VARCHAR(2)) + '/01'
    SET @vOutputDate = DATEADD(DD, -1, DATEADD(M, 1, @vOutputDate))
 
SET @OutPutDate = CONVERT(varchar(11), @vOutputDate,106)
SET @OutPutDate= @OutPutDate+ ' 23:59:59.000'
 
SET @OutPutDateTime = convert(datetime,@OutPutDate,106) 
SELECT period_id  
FROM period  
WHERE period_end_date=@OutPutDateTime  
GO
