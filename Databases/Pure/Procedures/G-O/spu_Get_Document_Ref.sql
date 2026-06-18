
EXECUTE DDLDropProcedure 'spu_Get_Document_Ref'
GO


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_Get_Document_Ref
       @InsuranceFileCnt int,
       @DocumentRef Varchar(25) OUTPUT
      
AS

BEGIN

SELECT @DocumentRef= RTRIM(Document_Ref)
FROM   Document
WHERE  insurance_file_cnt = @InsuranceFileCnt 

END 

GO