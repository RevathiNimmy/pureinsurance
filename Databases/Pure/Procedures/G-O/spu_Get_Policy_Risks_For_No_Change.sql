
EXECUTE DDLDropProcedure 'spu_Get_Policy_Risks_For_No_Change'
GO

CREATE PROCEDURE spu_Get_Policy_Risks_For_No_Change
    @Insurance_File_Cnt INT,
    @Risk_Cnt INT=0
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
			WHEN EXISTS(SELECT 
							1 
						FROM 
							Risk_Type_Rule_Set RTR 
						WHERE 
							RTR.Risk_Type_ID=RTY.Risk_Type_ID 
							AND RTR.Is_Deleted=0 
							AND RTR.Live=1 
							AND RTR.Type='RT')
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
		rsk.Is_Mandatory_Risk			
    FROM    
		Insurance_File_Risk_Link IRL1  
		INNER JOIN Insurance_File_Risk_Link IRL2 ON IRL2.Risk_Cnt = IRL1.Risk_Cnt  
		INNER JOIN Insurance_File IFI ON IFI.Insurance_File_Cnt = IRL2.Insurance_File_Cnt  
		INNER JOIN Risk RSK ON RSK.Risk_Cnt = IRL2.Risk_Cnt 
		INNER JOIN Risk_Type RTY ON RTY.Risk_Type_ID = RSK.Risk_Type_ID  
		LEFT OUTER JOIN Risk_Status RST ON RST.Risk_Status_ID = RSK.Risk_Status_ID
	WHERE   
		IRL1.Insurance_File_Cnt = @Insurance_File_Cnt  
		AND IRL2.Status_Flag <> 'U' 
        AND (@Risk_Cnt=0 OR (@Risk_Cnt <>0 AND RSK.RISK_CNT=@Risk_Cnt))
	ORDER BY
		RSK.Risk_Number 
END

