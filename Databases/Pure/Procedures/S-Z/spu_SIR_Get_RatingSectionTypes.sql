SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure spu_SIR_Get_RatingSectionTypes  
GO 
    
CREATE PROCEDURE spu_SIR_Get_RatingSectionTypes      
 @mode AS INT,      
 @risk_cnt AS INT,      
 @rating_section_type_id AS INT,      
 @original_risk_cnt AS INT 
AS      
--PN-72146
declare @inception_Dt as datetime

select @inception_Dt=cover_start_date from insurance_file iFile 
			inner join insurance_file_risk_link iFileLnk
			on iFile.insurance_file_cnt=iFileLnk.insurance_file_cnt
where iFileLnk.risk_cnt=@risk_cnt

IF @inception_Dt is Null
	BEGIN
		SET @inception_Dt=GETDATE()
	END
--PN-72146
IF @mode = 1 -- Add      
	BEGIN
		SELECT Distinct rst.rating_section_type_id, rst.description       
		FROM Rating_Section_Type rst    
		INNER JOIN Risk_Type_Rating_Section_Type rtrst       
		ON rtrst.rating_section_type_id = rst.rating_section_type_id      
		INNER JOIN Risk_Type rt ON     
		rt.risk_type_id = rtrst.risk_type_id       
		INNER JOIN Risk r ON rt.risk_type_id = r.risk_type_id       
		WHERE r.risk_cnt = @risk_cnt       
		--AND rst.is_deleted = 0 AND rst.effective_date < GETDATE()
		AND rst.is_deleted = 0 AND rst.effective_date <= @inception_Dt   --PN-72146    
		--AND rt.is_deleted = 0 AND rt.effective_date < GETDATE()
		AND rt.is_deleted = 0 AND rt.effective_date <= @inception_Dt     --PN-72146 
		ORDER BY rst.description      
	END    

IF @mode = 2 -- Edit      
BEGIN
	IF @rating_section_type_id = 0
	BEGIN
	 IF @original_risk_cnt = 0      
	  BEGIN    
		SELECT Distinct rst.rating_section_type_id, rst.description       
		FROM Rating_Section_Type rst      
		INNER JOIN Risk_Type_Rating_Section_Type rtrst       
		ON rtrst.rating_section_type_id = rst.rating_section_type_id      
		INNER JOIN Risk_Type rt ON rt.risk_type_id = rtrst.risk_type_id     
		INNER JOIN Risk r ON rt.risk_type_id = r.risk_type_id    
		WHERE r.risk_cnt = @risk_cnt       
		AND (    
		   rtrst.rating_section_type_id = rst.rating_section_type_id       
		   AND rst.is_deleted = 0       
		   --AND rst.effective_date < GETDATE()      
		   AND rst.effective_date < @inception_Dt  --PN-72146
		   --AND rt.is_deleted = 0 AND rt.effective_date < GETDATE()    
		   AND rt.is_deleted = 0 AND rt.effective_date <= @inception_Dt   --PN-72146
			  )      
		ORDER BY rst.description      
	  END       
	 ELSE      
	  BEGIN    
		SELECT Distinct rst.rating_section_type_id, rst.description       
		FROM Rating_Section_Type rst      
		INNER JOIN Risk_Type_Rating_Section_Type rtrst       
		ON rtrst.rating_section_type_id = rst.rating_section_type_id      
		INNER JOIN Risk_Type rt ON rt.risk_type_id = rtrst.risk_type_id     
		INNER JOIN Risk r ON rt.risk_type_id = r.risk_type_id    
		WHERE     
		r.risk_cnt IN (@risk_cnt, @original_risk_cnt)     
		AND (    
		   rtrst.rating_section_type_id = rst.rating_section_type_id       
		   AND rst.is_deleted = 0       
		   --AND rst.effective_date < GETDATE ()     
		   AND rst.effective_date < @inception_Dt  --PN-72146
		   --AND rt.is_deleted = 0 AND rt.effective_date < GETDATE()    
		   AND rt.is_deleted = 0 AND rt.effective_date <=@inception_Dt   --PN-72146
			)      
		OR rtrst.rating_section_type_id = @rating_section_type_id      
		ORDER BY rst.description      
	   END
	END
	ELSE -- if rating_section_type_id is passed    
		SELECT Distinct rating_section_type_id, description       
			FROM Rating_Section_Type 
				WHERE rating_section_type_id = @rating_section_type_id      
			ORDER BY description      

END  
