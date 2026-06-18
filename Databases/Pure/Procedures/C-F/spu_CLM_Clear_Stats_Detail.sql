EXECUTE DDLDropProcedure 'spu_CLM_Clear_Stats_Detail'
GO
CREATE PROCEDURE spu_CLM_Clear_Stats_Detail
@nPaymentID INT=0,
@nLossID INT=0,
@sPerilTypeCode varchar(255),
@bDeleteStatsFolder TINYINT =0
AS

BEGIN

DECLARE @lStatsFolderCnt INT
DECLARE @nPerilTypeID INT

SELECT @nPerilTypeID= peril_type_id  FROM Peril_Type WITH(NOLOCK) WHERE code =@sPerilTypeCode
IF @nPaymentID<>0
BEGIN

	SELECT 
		@lStatsFolderCnt=stats_folder_cnt,
		@nLossID =loss_id

	FROM  stats_folder WITH(NOLOCK)
	WHERE payment_id=@nPaymentID
END
ELSE
BEGIN
	SELECT @lStatsFolderCnt=stats_folder_cnt
	FROM  stats_folder WITH(NOLOCK)
	WHERE loss_id=@nLossID
	AND ISNULL(payment_id ,0)=0

END


DELETE from Stats_Detail
WHERE  stats_folder_cnt =ISNULL(@lStatsFolderCnt,0)
AND peril_type_id=@nPerilTypeID
END


IF (NOT EXISTS(SELECT stats_folder_cnt FROM Stats_Detail WHERE stats_folder_cnt=@lStatsFolderCnt) AND @bDeleteStatsFolder=1)

DELETE FROM Stats_Folder WHERE stats_folder_cnt=@lStatsFolderCnt
Go

