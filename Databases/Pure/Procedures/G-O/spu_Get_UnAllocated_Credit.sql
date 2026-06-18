SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_UnAllocated_Credit'
GO
CREATE Procedure spu_Get_UnAllocated_Credit
    @insurance_file_cnt integer,
    @party_type varchar(10),      
    @Party_cnt bigint =null        
    -- Will be passed in case we choose paynow when doing Renewal by Selecting more than one Policy  
AS
  --IF Agent_Cnt is Passed ( Renewal if More than one policy Selected)    
    IF @Party_cnt IS NOT NULL OR @Party_cnt>0       
    BEGIN    
  
      SELECT TD.Transdetail_id,DOC.Document_ref,ISNULL(mediatype.Description,'') MediaType,      
        ISNULL(CLI.Media_ref ,TD.SPARE)Reference,TD.outstanding_currency_amount Amount, TD.Account_ID, CLI.collection_date      
        FROM Transdetail TD      
        LEFT Join CashListItem CLI      
        ON      
        TD.Transdetail_id = CLI.Transdetail_id      
        LEFT Join Document DOC ON      
        TD.Document_id = DOC.Document_id      
        LEFT JOIN mediatype ON      
        CLI.MediaType_id = mediatype.MediaType_id      
        LEFT JOIN Account on TD.Account_id=Account.Account_id      
        JOIN Party on Party.Party_cnt=Account.Account_Key            
        WHERE TD.outstanding_currency_amount<0    
        AND Party.Party_cnt=@Party_cnt      
        AND (TD.balance_type IS NULL OR TD.balance_type = '')    
    RETURN      
    END    
    IF @party_type='AGENT' 
    BEGIN
	SELECT TD.Transdetail_id,DOC.Document_ref,ISNULL(mediatype.Description,'') MediaType,  
	ISNULL(CLI.Media_ref ,TD.SPARE)Reference,TD.outstanding_currency_amount Amount, TD.Account_ID, CASE WHEN DOC.documenttype_id=18 THEN cover_start_date ELSE CLI.collection_date END  
	FROM Transdetail TD
	LEFT Join CashListItem CLI 
	ON 
	TD.Transdetail_id = CLI.Transdetail_id 
	LEFT Join Document DOC ON  
	TD.Document_id = DOC.Document_id  
	LEFT JOIN mediatype ON  
	CLI.MediaType_id = mediatype.MediaType_id  
	LEFT JOIN Account on TD.Account_id=Account.Account_id   
	JOIN Party on Party.Party_cnt=Account.Account_Key  
	JOIN Insurance_File ifl ON  
	ifl.Lead_agent_cnt=Party.Party_cnt
	WHERE TD.outstanding_currency_amount<0  
	AND ifl.insurance_file_cnt=@insurance_file_cnt
	AND (TD.balance_type IS NULL OR TD.balance_type = '')
    END
    ELSE  -- Party Type='Client'
    BEGIN
	SELECT TD.Transdetail_id,DOC.Document_ref,ISNULL(mediatype.Description,'') MediaType,  
	ISNULL(CLI.Media_ref ,TD.SPARE)Reference,TD.outstanding_currency_amount Amount, TD.Account_ID, CASE WHEN DOC.documenttype_id=18 THEN cover_start_date ELSE CLI.collection_date END  
	FROM Transdetail TD
	LEFT Join CashListItem CLI 
	ON 
	TD.Transdetail_id = CLI.Transdetail_id 
	LEFT Join Document DOC ON  
	TD.Document_id = DOC.Document_id  
	LEFT JOIN mediatype ON  
	CLI.MediaType_id = mediatype.MediaType_id  
	LEFT JOIN Account on TD.Account_id=Account.Account_id   
	JOIN Party on Party.Party_cnt=Account.Account_Key  
	JOIN Insurance_File ifl ON  
	ifl.insured_cnt = Party.party_cnt  
	WHERE TD.outstanding_currency_amount<0  
	AND ifl.insurance_file_cnt=@insurance_file_cnt        
	AND (TD.balance_type IS NULL OR TD.balance_type = '')
    END

