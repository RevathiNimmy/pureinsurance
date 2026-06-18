SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure spu_Update_Agent_Cert_Year
GO
CREATE PROCEDURE spu_Update_Agent_Cert_Year        
    @party_cnt INT ,      
    @Code char(10),      
    @Description varchar(255),      
    @StartDate Date,      
    @Enddate Date,      
    @UpdateStatus INT=0,
	@UserId INT = NULL,
	@UniqueId varchar(50) = NULL,
	@ScreenHierarchy varchar(500) = NULL
            
AS     
 BEGIN    
 IF EXISTS( SELECT * FROM party_certificate_year WHERE Party_Cnt =@party_cnt AND CertYearCode=@Code)  
 BEGIN   
 IF @UpdateStatus=1  
  BEGIN 
  UPDATE party_certificate_year SET UserId = @UserId,UniqueId = @UniqueId,ScreenHierarchy = @ScreenHierarchy WHERE Party_Cnt =@party_cnt AND CertYearCode=@Code     
  DELETE FROM party_certificate_year WHERE Party_Cnt =@party_cnt AND CertYearCode=@Code     
  END    
 ELSE  
  BEGIN  
  UPDATE party_certificate_year SET certyearenddate= @Enddate,UserId = @UserId,UniqueId = @UniqueId,ScreenHierarchy = @ScreenHierarchy WHERE Party_Cnt =@party_cnt AND CertYearCode=@Code     
  END  
 END 
 ELSE  
 BEGIN  
  IF @UpdateStatus=0       
  BEGIN      
  INSERT INTO party_certificate_year (Party_Cnt,CertYearCode  ,certyeardescription,certyearstartdate,certyearenddate,isdeleted,UserId,UniqueId,ScreenHierarchy)      
  VALUES(@party_cnt,@Code,@Description,@StartDate,@Enddate,0,@UserId,@UniqueId,@ScreenHierarchy)      
  END      
 END    
 END
 
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

