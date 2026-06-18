SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_delete_risk_cancelled_on_add'
GO
/********************************************************************************************************/
/* Deletes the partial back office risk details that would be left behind when an add is cancelled      */
/********************************************************************************************************/
/* Revision Description of Modification Date        Who                                                 */
/* -------- --------------------------- ----        ---                                                 */
/* 1.0      Original                    1/12/2002  CLG                                                 */
/********************************************************************************************************/

CREATE PROCEDURE spu_delete_risk_cancelled_on_add

    @risk_cnt INT
    
AS 
BEGIN
	DECLARE @risk_folder_cnt 	INT  
	DECLARE @gis_policy_link_id INT  
	DECLARE @table_name 		VARCHAR(255)  
	DECLARE @SQL 				NVARCHAR(1000)  
	DECLARE @LocalRiskCnt 		INT
	DECLARE @GIS_Data_Model_Code CHAR(10)

	SELECT @LocalRiskCnt = @risk_cnt

    SELECT @risk_folder_cnt = risk_folder_cnt  
    FROM risk  
    WHERE risk_cnt = @LocalRiskCnt  
  
	If EXISTS (select NULL from insurance_file_risk_link where risk_cnt = @LocalRiskCnt and original_risk_cnt IS NULL)  
		DELETE FROM Tax_Calculation WHERE risk_cnt = @LocalRiskCnt  

    DELETE FROM insurance_file_risk_link WHERE risk_cnt = @LocalRiskCnt  
	DELETE FROM Peril WHERE risk_cnt = @LocalRiskCnt  
	DELETE FROM Rating_Section WHERE risk_cnt = @LocalRiskCnt  
	DELETE FROM RI_Arrangement_Line WHERE ri_arrangement_id in (SELECT ri_arrangement_id FROM RI_Arrangement WHERE risk_cnt = @LocalRiskCnt)  
	DELETE FROM RI_Arrangement WHERE risk_cnt = @LocalRiskCnt  
    DELETE FROM risk WHERE risk_cnt = @LocalRiskCnt  
  
	IF NOT EXISTS (SELECT 1 FROM risk WHERE risk_folder_cnt = @risk_folder_cnt)  
	BEGIN  
		DELETE FROM risk_folder	WHERE risk_folder_cnt = @risk_folder_cnt  
	END  

	SELECT @GIS_Data_Model_Code = LTRIM(RTRIM(GDM.Code))
			,@GIS_Policy_Link_ID = CAST(gis_policy_link_id AS VARCHAR(20))
	FROM gis_policy_link GPL
	INNER JOIN GIS_Data_Model GDM ON GDM.gis_data_model_id = GPL.gis_data_model_id
	WHERE risk_id = @LocalRiskCnt 

	--Get the correct GIS table name from the risk and concatenate the correct delete statement based on the risk type
	SELECT  @SQL = N'DELETE FROM ' + LTRIM(RTRIM(@GIS_Data_Model_Code)) + '_Policy_Binder WHERE gis_policy_link_id = ' + STR(@GIS_Policy_Link_ID)
	EXECUTE sp_executeSQL @SQL

    DELETE FROM gis_policy_link WHERE risk_id = @LocalRiskCnt

END
GO