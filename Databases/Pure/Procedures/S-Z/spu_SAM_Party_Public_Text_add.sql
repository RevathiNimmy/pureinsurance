SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Party_Public_Text_add'
GO

--Start (Sriram P) - (Tech Specs - WR42A_SAM_Party_Enquiry - AddEvent.doc) - (6)
CREATE   PROCEDURE spu_SAM_Party_Public_Text_add    
@party_cnt int,    
@party_public_text_id int OUTPUT,    
@text_line varchar(255)    
AS    
BEGIN    
select @party_public_text_id = max(Party_Public_Text_id) from Party_Public_Text where party_cnt=@party_cnt    
if (@party_public_text_id is null)  
BEGIN  
select @party_public_text_id=1  
END  
else  
BEGIN  
select @party_public_text_id = @party_public_text_id + 1  
END  
END    
BEGIN    
INSERT INTO Party_Public_Text (    
party_cnt ,    
party_public_text_id ,    
text_line )    
VALUES (    
@party_cnt,    
@party_public_text_id,    
@text_line)    
END 
 
--End (Sriram P) - (Tech Specs - WR42A_SAM_Party_Enquiry - AddEvent.doc) - (6)   
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

