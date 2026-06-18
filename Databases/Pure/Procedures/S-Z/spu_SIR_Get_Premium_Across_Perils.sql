EXEC DDLDropProcedure 'spu_SIR_Get_Premium_Across_Perils'
GO

CREATE PROCEDURE spu_SIR_Get_Premium_Across_Perils
	@risk_cnt INT,
	@peril_total_premium INT OUTPUT,
	@peril_total_original_premium INT OUTPUT
AS
	SELECT
		@peril_total_premium=SUM(P.this_premium)
        FROM    Peril P
        JOIN    Insurance_File_Risk_Link IFR   ON IFR.risk_cnt = P.risk_cnt
        JOIN    Peril_Type PT                  ON P.peril_type_id = PT.peril_type_id  
        JOIN    Rating_Section RS              ON P.rating_section_id = RS.rating_section_id  
                                              AND P.Risk_cnt = RS.Risk_cnt  
        JOIN    Class_Of_Business CB           ON P.class_of_business_id = CB.class_of_business_id  
        JOIN    Risk R                         ON P.risk_cnt = R.risk_cnt  
        JOIN    Risk_Type RT                   ON R.risk_type_id = RT.risk_type_id  
        LEFT JOIN  
                Policy_Section_Type PS         ON RS.policy_section_type_id = PS.policy_section_type_id  
		JOIN    Insurance_file ifile 	       ON Ifile.insurance_file_cnt =  IFR.insurance_file_cnt  
        WHERE   P.risk_cnt = @risk_cnt
        AND     IFR.status_flag <> 'U' 
		AND 	RS.original_flag = 0 
		AND    (P.is_premium = 1
             OR P.is_sum_insured = 1  
             OR IsNull(P.is_levy_tax, 0) = 0)
	SELECT
		@peril_total_original_premium=SUM(P.this_premium)
        FROM    Peril P
        JOIN    Insurance_File_Risk_Link IFR   ON IFR.risk_cnt = P.risk_cnt
        JOIN    Peril_Type PT                  ON P.peril_type_id = PT.peril_type_id  
        JOIN    Rating_Section RS              ON P.rating_section_id = RS.rating_section_id  
                                              AND P.Risk_cnt = RS.Risk_cnt  
        JOIN    Class_Of_Business CB           ON P.class_of_business_id = CB.class_of_business_id  
        JOIN    Risk R                         ON P.risk_cnt = R.risk_cnt  
        JOIN    Risk_Type RT                   ON R.risk_type_id = RT.risk_type_id  
        LEFT JOIN  
                Policy_Section_Type PS         ON RS.policy_section_type_id = PS.policy_section_type_id  
		JOIN    Insurance_file ifile 	       ON Ifile.insurance_file_cnt =  IFR.insurance_file_cnt  
        WHERE   P.risk_cnt = @risk_cnt
        AND     IFR.status_flag <> 'U' 
		AND 	RS.original_flag = 1 
		AND    (P.is_premium = 1
             OR P.is_sum_insured = 1  
             OR IsNull(P.is_levy_tax, 0) = 0)
 
	SELECT 	@peril_total_premium = ISNULL(@peril_total_premium,0),
	 		@peril_total_original_premium = ISNULL(@peril_total_original_premium,0)

GO
