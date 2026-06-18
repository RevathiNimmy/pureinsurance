SET QUOTED_IDENTIFIER ON
GO

EXECUTE DDLDropProcedure 'spu_Get_Previous_RiskData'
GO

CREATE PROCEDURE spu_Get_Previous_RiskData 
	  @sObjectName VARCHAR(60),
	  @sColumns VARCHAR(200)='*',
	  @sUIDValue VARCHAR(70)=NULL,
      @nInsuranceFileKey INT=0,
      @nClaimKey INT=0,
	  @nPreviousClaimKey INT=0    
AS  
BEGIN

	DECLARE @sSQL VARCHAR(500)
	DECLARE @lPolicyLinkID INT
	DECLARE @sDMCode VARCHAR(10)
	DECLARE @lPreviousInsuranceFileKey INT
	

	IF ISNULL(@nInsuranceFileKey,0)<>0
	EXECUTE spu_Get_Previous_InsuranceFileKey @nInsuranceFileKey,@lPreviousInsuranceFileKey OUTPUT

	IF ISNULL(@nPreviousClaimKey,0)=0 
	BEGIN
		IF ISNULL(@nClaimKey,0)<>0
		EXECUTE spu_Get_Previous_ClaimKey @nClaimKey,@nPreviousClaimKey OUTPUT
	END

	SELECT @lPolicyLinkID =GPL.gis_policy_link_id,
		   @sDMCode=RTRIM(GDM.code) 
	FROM GIS_Policy_Link GPL
	JOIN GIS_Data_Model GDM
	ON GPL.gis_data_model_id=GDM.gis_data_model_id
	WHERE (GPL.insurance_file_cnt=@lPreviousInsuranceFileKey OR GPL.claim_id=@nPreviousClaimKey)

	IF ISNULL(@lPolicyLinkID,0)<>0
	BEGIN
		DECLARE @sTableName VARCHAR(70)
		DECLARE @lGisObjectID INT

		SELECT @sTableName= table_name,@lGisObjectID=gis_object_id FROM GIS_Object WHERE object_name =@sObjectName
		IF ISNULL(@lGisObjectID,0)<>0
		BEGIN

			SELECT @sSQL = 'SELECT ' + @sColumns + ' FROM ' + @sTableName + ' WHERE ' +@sDMCode +'_Policy_binder_id = ' + CONVERT(VARCHAR(10),@lPolicyLinkID)
			IF ISNULL(@sUIDValue,'')<>''
			SELECT @sSQL = @sSQL + ' AND UID = ''' + @sUIDValue + ''''
			EXECUTE sp_executesql @sSQL
		END
	END  
END

GO

