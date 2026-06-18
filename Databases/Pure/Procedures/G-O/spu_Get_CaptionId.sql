SET QUOTED_IDENTIFIER OFF 
GO

EXECUTE DDLDropProcedure 'spu_Get_CaptionId'
GO
CREATE PROCEDURE spu_Get_CaptionId
	@CodeTVP CodeTableType  READONLY

AS  
DECLARE @Code VARCHAR(10), @captionId INT

DECLARE Cur Cursor for
SELECT Code From @CodeTVP WHERE RTRIM(CODE)<>''

OPEN Cur
FETCH NEXT FROM cur into @Code
WHILE @@FETCH_STATUS = 0 
BEGIN
	
	exec spu_pm_caption_id_return @language_id=1,@caption=@Code,@caption_id=@captionId output
	FETCH NEXT FROM cur into @Code

END
Close Cur
Deallocate Cur

SELECT a.Code, PMCaption.Caption_id FROM @CodeTVP as a LEFT JOIN PMCAption 
ON CONVERT(varbinary(255),PMCaption.caption) = CONVERT(varbinary(255), a.Code)   and Language_id = 1




