SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Event_Public_Text_add'
GO
--Start (Sriram P) - (Tech Specs - WR42A_SAM_Party_Enquiry - AddEventNote.doc) - (6)
CREATE PROCEDURE spu_SAM_Event_Public_Text_add      
    @event_cnt int,      
    @event_public_text_id int OUTPUT,      
    @text_line varchar(7500)    
AS      
BEGIN     
    Select @event_public_text_id = Max(event_public_text_id) From Event_Public_Text where event_cnt = @event_cnt    
    IF @event_public_text_id > 0  
    BEGIN  
        select @event_public_text_id = @event_public_text_id + 1    
    END  
    ELSE  
    BEGIN  
        select @event_public_text_id = 1  
    END  
END    
BEGIN      
INSERT INTO Event_Public_Text (      
    event_cnt ,      
    event_public_text_id ,      
    text_line )      
VALUES (      
    @event_cnt,      
    @event_public_text_id,      
    @text_line)      
END 
--End (Sriram P) - (Tech Specs - WR42A_SAM_Party_Enquiry - AddEventNote.doc) - (6)    
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO    
