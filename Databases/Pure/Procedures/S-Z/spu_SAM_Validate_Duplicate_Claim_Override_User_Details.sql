SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Validate_Duplicate_Claim_Override_User_Details'
GO

--Start (Jigar Pandya) - (Tech Spec-UIIC WR24-OpenClaim-Duplicate Claims Check.doc) - (5.1.1)  
CREATE  Procedure spu_SAM_Validate_Duplicate_Claim_Override_User_Details    
@username varchar(255),    
@password varchar(255)= NULL,  
@user_id int OUTPUT    
    
AS     
    
IF @password IS NULL
	BEGIN
		SELECT @user_id = ua.User_Id  
		FROM  
		user_authorities ua INNER JOIN pmuser pmu ON ua.user_id=pmu.user_id  
		where  
		ua.can_override_duplicate_claims=1  
		and pmu.username=@username  
	END  
ELSE
	BEGIN
		SELECT @user_id = ua.User_Id  
		FROM  
		user_authorities ua INNER JOIN pmuser pmu ON ua.user_id=pmu.user_id  
		where  
		ua.can_override_duplicate_claims=1  
		and pmu.username=@username  
		and pmu.secure_password=@password  
	END
    
  
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
