SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Get_Numbering_Scheme'
GO


CREATE PROCEDURE spu_SAM_CLM_Get_Numbering_Scheme
    @language_id int,  
    @numbering_scheme_id int = NULL  
AS  
    
BEGIN  
  
  BEGIN TRAN;
	SELECT  
		a.numbering_scheme_id,  
		a.caption_id,  
		a.code,  
		a.description,  
		a.is_deleted,  
		a.effective_date,  
		a.numbering_scheme_type_id,     
		a.numbering_scheme,  
		a.is_generated,  
		a.mask_code,  
		a.fixed_code,  
		a.next_number,  
		a.highest_number,  
		a.step,  
		a.is_reuse_abandoned,  
		c.caption as 'type_description',
		--Start - Renuka - (WPR87 Paralleling)
		a.is_reset_number  
		--End - Renuka - (WPR87 Paralleling)
	FROM numbering_scheme  a WITH (UPDLOCK), 
		PMCaption c  
	WHERE a.numbering_scheme_id = @numbering_scheme_id  
	AND a.caption_id = c.caption_id  
	AND c.language_id = @language_id  
	AND a.is_deleted = 0  
  
	UPDATE numbering_scheme  
		SET next_number = next_number + step  
	WHERE   numbering_scheme_id = @numbering_scheme_id  
	AND step <> 0  
	
	UPDATE numbering_scheme  
		SET next_number = next_number + 1  
	WHERE   numbering_scheme_id = @numbering_scheme_id  
	AND step = 0 
	
	COMMIT TRAN;

END  





GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
