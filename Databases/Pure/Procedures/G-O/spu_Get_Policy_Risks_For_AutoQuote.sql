SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Get_Policy_Risks_For_AutoQuote' 
GO


CREATE PROCEDURE spu_Get_Policy_Risks_For_AutoQuote
    @Insurance_File_Cnt INT
AS 
BEGIN 
    SELECT DISTINCT
		RSK.Risk_Cnt, 
		RSK.Risk_Folder_Cnt,
		RSK.Risk_Number,
		IRL1.Insurance_File_Cnt,
		IFI.Insurance_Folder_Cnt,
		IFI.Product_ID,
		RSK.GIS_Screen_ID,    
		RSK.Description AS Risk_Description,
		RST.Risk_Status_ID,
		RST.Code AS Risk_Status_Code,      
		RST.Description AS Risk_Status_Description,
		RTY.Risk_Type_ID,
		RTY.Code AS Risk_Type_Code,
		RTY.Description AS Risk_Type_Description,
		CASE
			WHEN ISNULL(RS.is_amended,0) = 0
			THEN 1
			ELSE 0
		END AS Is_Auto_Rated,
		CASE 
			WHEN EXISTS(SELECT 
							1
						FROM
							RI_Arrangement RIA
							LEFT OUTER JOIN RI_Arrangement_Line RAL
								ON RAL.RI_Arrangement_ID=RIA.RI_Arrangement_ID
						WHERE
							RIA.Risk_Cnt IN(IRL2.Risk_Cnt,IRL2.Original_Risk_Cnt)
							AND RAL.Type IN ('F','FX')
					   ) THEN 1
			ELSE 0
		END AS Has_FAC_RI,
		isnull(rsk.Is_Mandatory_Risk,0)	AS Is_Mandatory_Risk					
    FROM    
		Insurance_File_Risk_Link IRL1  
		INNER JOIN Insurance_File_Risk_Link IRL2 ON IRL2.Risk_Cnt = IRL1.Risk_Cnt  
		INNER JOIN Insurance_File IFI ON IFI.Insurance_File_Cnt = IRL2.Insurance_File_Cnt  
		INNER JOIN Risk RSK ON RSK.Risk_Cnt = IRL2.Risk_Cnt
		LEFT JOIN (
				   SELECT risk_cnt,
						  COUNT(*) amended, 
						  is_amended 
				   FROM Rating_Section WHERE original_flag = 1 
				   AND risk_cnt IN 
				   (
					SELECT risk_cnt FROM insurance_file_risk_link 
					WHERE insurance_file_cnt = @insurance_file_cnt
				   ) 
				   AND ISNULL(is_amended,0) = 1 
				   GROUP BY risk_cnt,is_amended
				  ) RS ON RS.Risk_Cnt = RSK.Risk_Cnt
		INNER JOIN Risk_Type RTY ON RTY.Risk_Type_ID = RSK.Risk_Type_ID  
		LEFT OUTER JOIN Risk_Status RST ON RST.Risk_Status_ID = RSK.Risk_Status_ID
		LEFT JOIN Insurance_File_Status IFS ON IFS.insurance_file_status_id = IFI.insurance_file_status_id
    WHERE   
		IRL1.Insurance_File_Cnt = @Insurance_File_Cnt  
		--AND ISNULL(IFI.Insurance_File_Status_ID, 2) IN (1,2,3,4,5,6,309) 
		AND ISNULL(IFS.code,'LAP') IN ('CAN','LAP','REN','REP','REPDRI','REPPT','REPBDMTA') 		
		AND IRL2.Status_Flag <> 'U' 
	ORDER BY
		RSK.Risk_Number 
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
