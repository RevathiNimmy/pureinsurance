SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SIR_Upd_Party_Document_Library'
GO

CREATE PROCEDURE spu_SIR_Upd_Party_Document_Library
    @PartyCnt INT ,  
    @DocumentLibrary VARCHAR(255)  
AS   
UPDATE Party SET DocumentLibrary=@DocumentLibrary  
WHERE Party_cnt=@PartyCnt