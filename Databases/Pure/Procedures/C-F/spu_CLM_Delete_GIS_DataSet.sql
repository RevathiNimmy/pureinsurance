SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Delete_GIS_DataSet'
GO

CREATE PROCEDURE spu_CLM_Delete_GIS_DataSet
 @Claim_Id Integer    
AS    
BEGIN    
  
 DECLARE @DataModelCode varchar(10)    
 DECLARE @GisPolicyLinkId integer    
 DECLARE @LiveClaimId integer    
 DECLARE @sSQL varchar(1000)    
   
 EXEC spu_Get_DataModel_Code_For_Claim    
  @Claim_Id,    
  @Datamodel_Code = @DataModelCode OUTPUT    
    
 SET @DataModelCode = LTRIM(RTRIM(@DataModelCode))    
    
 SELECT  @GisPolicyLinkID  = Gis_Policy_Link_ID,    
  @LiveClaimId   = claim_id    
 FROM  Gis_Policy_link WITH (NOLOCK)    
 WHERE  Claim_Id   = @Claim_ID    
   
 IF @GisPolicyLinkID IS NOT NULL    
 BEGIN    
  -- Delete GIS claim data    
  SET @sSQL = 'Delete from ' + @DataModelCode + '_claim WITH (ROWLOCK) Where claim_id = ' + Cast(@Claim_Id as varchar(10))    
  IF @@ERROR <> 0    
  PRINT 'ERROR OCCURED'    
  EXEC (@sSQL)    
   
  -- Delete GIS claim peril data    
  SET @sSQL = 'Delete from ' + @DataModelCode + '_claim_peril WITH (ROWLOCK) Where claim_id = ' + Cast(@Claim_Id as varchar(10))    
  IF @@ERROR <> 0    
  PRINT 'ERROR OCCURED'    
  EXEC (@sSQL)    
    
  -- Adding: so delete policy binder and policy link as they are not required now    
  SET @sSQL = 'Delete from ' + @DataModelCode + '_Policy_Binder WITH (ROWLOCK) Where Gis_Policy_Link_Id =' + Cast(@GisPolicyLinkId as varchar(10))    
  IF @@ERROR <> 0    
  PRINT 'ERROR OCCURED'    
  EXEC (@sSQL)    
    
  Delete from Gis_Policy_Link WITH (ROWLOCK) Where Gis_Policy_Link_Id = @GisPolicyLinkId    
  
 END  
END    
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
