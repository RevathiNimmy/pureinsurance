SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_MID_Batch_Export'
GO

CREATE Procedure spu_MID_Batch_Export
AS

BEGIN

	DECLARE @nMid_policy_id int,
			@nInsurance_file_cnt int,
			@nInsurance_folder_cnt int,
			@sMid_Type as varchar(4),
			@nMID_Status_id AS INT,
			@sUpdateTypePolicy As Varchar(4),
			@sUpdateTypeVehicle As Varchar(4),
			@nMid_policy_id_CurrentMax as INT

	SET @nMID_Status_id = (SELECT mid_status_id FROM MID_Status WHERE code='PENDING')
	SET @nMid_policy_id_CurrentMax = (SELECT MAX(mid_policy_id) From mid_policy)
	SET @sUpdateTypePolicy = 'A'
	SET @sUpdateTypeVehicle = 'A'
	
	BEGIN TRY 
	BEGIN TRAN TRNMID

	DECLARE mid_policy_cursor CURSOR FOR
	SELECT MP.mid_policy_id, INF.insurance_file_cnt, INF.insurance_folder_cnt, MP.mid_type
	FROM Mid_Policy MP
		INNER JOIN Insurance_File INF ON  MP.insurance_file_cnt = inf.insurance_file_cnt
		LEFT OUTER JOIN Insurance_File_Status IFS ON INF.insurance_file_status_id = IFS.insurance_file_status_id
		LEFT OUTER JOIN Insurance_File_Type IFT ON INF.insurance_file_type_id = IFT.insurance_file_type_id
	WHERE LTRIM(RTRIM(IFT.code)) = 'MTA TEMP'
		AND CONVERT(DATE, INF.expiry_date) = CONVERT(DATE, GETDATE())
		AND MP.mid_type = 'MID1'
		AND MP.mid_policy_id <= @nMid_policy_id_CurrentMax
		
	OPEN mid_policy_cursor
	FETCH NEXT FROM mid_policy_cursor 
	INTO @nMid_policy_id, @nInsurance_file_cnt, @nInsurance_folder_cnt, @sMid_Type

	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @nNext_temp_mta_inf_Id INT,
				@nNew_business_inf_id INT,
				@Mid_Policy_ID INT,
				@nInsuranceFile_cnt INT,
				@sOff_Date VARCHAR(50),
				@sStrSQL NVARCHAR(MAX)
		SET @nNext_temp_mta_inf_Id = (SELECT top 1 insurance_file_cnt from Insurance_File INF
											LEFT OUTER JOIN Insurance_File_Type IFT ON INF.insurance_file_type_id = IFT.insurance_file_type_id
										Where INF.insurance_folder_cnt = @nInsurance_folder_cnt
											AND LTRIM(RTRIM(IFT.code)) = 'MTA TEMP'
											AND CONVERT(DATE, INF.expiry_date) > CONVERT(DATE, GETDATE()) order by INF.expiry_date)

		SET @nNew_business_inf_id = (SELECT MIN(insurance_file_cnt) from Insurance_File INF
										Where INF.insurance_folder_cnt = @nInsurance_folder_cnt)

		IF (ISNULL(@nNext_temp_mta_inf_Id,0)>0)
			BEGIN
				SET @nInsuranceFile_cnt= @nNext_temp_mta_inf_Id
			END
		ELSE
			BEGIN
				SET @nInsuranceFile_cnt= @nNew_business_inf_id
			END

		SET @sOff_Date = (SELECT [expiry_date] FROM insurance_file where insurance_file_cnt =  @nInsuranceFile_cnt)

		INSERT INTO MID_POLICY	(insurance_folder_cnt,insurance_file_cnt,mid_type,mid_status_id,update_type)
						Values	(@nInsurance_folder_cnt,@nInsuranceFile_cnt,@sMid_Type,@nMID_Status_id,@sUpdateTypePolicy)

		SET @Mid_Policy_ID= SCOPE_IDENTITY()
		
		PRINT CAST(@@ROWCOUNT As VARCHAR(50))+' -  Records Inserted Into MID_POLICY table'
		
		SET @sStrSQL='INSERT INTO mid_vehicle (mid_status_id,mid_policy_id,update_type,Registration,is_foreign_registration,
			is_trade_registration,Make,Model,on_date,off_date,permitted_drivers,class_use)
			Select 
			'''+ CAST(@nMID_Status_id As VARCHAR(25))+ ''' As MID_status_id, 
			'''+ CAST(@Mid_Policy_ID As VARCHAR(25))+ ''' As Mid_Policy_ID,
			'''+ @sUpdateTypeVehicle +''' As UpdateType,
			Registration,
			is_foreign_registration,
			is_trade_registration,
			Make,
			Model,
			DATEADD(dd, 1, CONVERT(DATE,GETDATE())) AS on_date,
			'''+ CAST(@sOff_Date As VARCHAR(50))+ '''  AS off_date,
			permitted_drivers,
			Class_USE
			
			FROM MID_VEHICLE MV
				INNER JOIN MID_POLICY MP ON MP.mid_policy_id = MV.mid_policy_id
			WHERE MP.insurance_file_cnt = '+ CAST(@nInsuranceFile_cnt AS VARCHAR(50)) +''
				
					
		Print @sStrSQL
		EXECUTE sp_executesql @sStrSQL
					
		PRINT CAST(@@ROWCOUNT As VARCHAR(50))+' -  Records Inserted Into MID_Vehicle table'
		

		FETCH NEXT FROM mid_policy_cursor
		INTO @nMid_policy_id ,@nInsurance_file_cnt, @nInsurance_folder_cnt, @sMid_Type
	END

	CLOSE mid_policy_cursor
	DEALLOCATE mid_policy_cursor

	COMMIT TRAN TRNMID
	END TRY 
	BEGIN CATCH
		PRINT 'Error Occured'
		 SELECT  
        ERROR_NUMBER() AS ErrorNumber  
        ,ERROR_SEVERITY() AS ErrorSeverity  
        ,ERROR_STATE() AS ErrorState  
        ,ERROR_PROCEDURE() AS ErrorProcedure  
        ,ERROR_LINE() AS ErrorLine  
        ,ERROR_MESSAGE() AS ErrorMessage;  

		ROLLBACK TRAN TRNMID
	END CATCH  
END
GO

