SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SIR_Get_Party_Document_Library'
GO
CREATE proc spu_SIR_Get_Party_Document_Library        
    @PartyCnt INT,    
    @PartyShortName VARCHAR(60)    
AS        
    
IF(@PartyCnt!=0)  
BEGIN  
SELECT ISNULL(DocumentLibrary,'') AS DocumentLibrary ,shortname ,party_cnt FROM Party  with(nolock)  
WHERE Party_cnt=@PartyCnt  
END  
ELSE  
SELECT ISNULL(DocumentLibrary,'')  AS DocumentLibrary,shortname, party_cnt FROM Party  with(nolock)  
WHERE shortname =@PartyShortName