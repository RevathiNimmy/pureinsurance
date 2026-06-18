
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure spu_sir_calc_commission_rate
GO
CREATE PROCEDURE spu_sir_calc_commission_rate    
    @party_type_id INT,    
    @party_id INT,    
    @product_id INT,    
    @risk_type_id INT,    
    @transaction_type_id INT,    
    @commission_band_id INT,    
    @effective_date DATETIME,    
    @commission_rate NUMERIC(19, 10) OUTPUT,    
    @Is_Value TINYINT OUTPUT,    
    @tax_group_id INT OUTPUT,    
    @transaction_type VARCHAR(10)=NULL,    
    @insurance_file_cnt INT=NULL,    
    @is_lead_agent tinyint=NULL,    
    @maximum_rate NUMERIC(19, 4) = 0 OUTPUT ,     
 @commission_level int = NULL,  
 @is_amended tinyint = NULL OUTPUT       
AS    
    
BEGIN        
  PRINT @party_type_id
  PRINT @party_id
  PRINT @product_id
  PRINT @risk_type_id
  PRINT @transaction_type_id
  PRINT @commission_band_id
  PRINT @effective_date
  PRINT @Is_Value
  PRINT @tax_group_id
  PRINT @transaction_type
  PRINT @insurance_file_cnt
  PRINT @is_lead_agent
  PRINT @commission_level
SELECT        
 @commission_rate = 0,        
    @Is_value = 0,  
 @is_amended = 0        
        
SELECT TOP 1        
 @commission_rate=Rate,        
    @Is_value=Is_value,        
 @tax_group_id=tax_group_id,        
--Start - Renuka - (WPR64 Paralleling)    
 @maximum_rate=maximum_rate      
--End - Renuka - (WPR64 Paralleling)    
FROM Commission_arrangement        
WHERE ((Party_type = @party_type_id) OR (Party_type = 0))  
AND ((party_cnt = @party_id) OR (party_cnt = 0))
AND ((Product_id = @product_id) OR (Product_id = 0)) 
AND ((risk_type_id = @risk_type_id) OR (risk_type_id = 0)) 
AND ((Transaction_type_id = @transaction_type_id) OR (Transaction_type_id = 0)) 
AND ((Commission_band_id = @commission_band_id) OR (Commission_band_id = 0)) 
AND ((Commission_level_id = @commission_level) OR (Commission_level_id = 0)) 
--and Effective_date <= @Effective_date        
AND Effective_date <=        
(        
    SELECT MAX(effective_Date)        
    FROM commission_arrangement        
    WHERE effective_date <= @effective_date        
)        
AND is_deleted = 0        
ORDER BY        
 party_cnt DESC,        
 risk_type_id DESC,        
 product_id DESC,        
 party_type DESC,        
 transaction_type_id DESC,        
 commission_band_id DESC,
 effective_date DESC --PN: 72768    
        
  PRINT @commission_rate
END        

 DECLARE  @gis_object_commission_rate NUMERIC(19, 10)
	Set @gis_object_commission_rate=NULL 
	
	EXEC spu_sir_get_agent_commission_from_output @insurance_file_cnt =@insurance_file_cnt,     
		@party_id =@party_id,
		@commission_band_id =@commission_band_id, 
		@commission_rate =@gis_object_commission_rate OUTPUT

 If  @gis_object_commission_rate IS NOT NULL
 set @commission_rate=@gis_object_commission_rate

--Process further only if transaction type is MTA,Cancel Policy,Reinstate Policy,Renewal        
IF @transaction_type in ('MTA','MTC','MTR','REN')        
BEGIN        
 --Check for agent's use_override_commission_rate flag        
 IF exists(SELECT * FROM Party_Agent WHERE Party_cnt=@party_id and ((@transaction_type != 'REN' AND ISNULL(use_override_commission_rate,0)=1) OR (@transaction_type = 'REN' AND ISNULL(use_override_commission_renewal,0) = 1 )) )                
 BEGIN     
    
  --Get the last insurance_file_cnt from the insurance_file        
  --ignore insurance file with insurance_type 'QUOTE', 'MTAQTETEMP','MTAQREINS'        
   DECLARE        
    @last_insurance_file_cnt int,        
    @insurance_folder_cnt int        
        
   --Get the insurance_folder_cnt        
   SELECT @insurance_folder_cnt=insurance_folder_cnt FROM Insurance_File        
   WHERE insurance_file_cnt=@insurance_file_cnt        
        
   --GET the last insurance_file_cnt  
   IF @is_lead_agent=0
	   SELECT @last_insurance_file_cnt=MAX(InsFile.insurance_file_cnt) FROM Insurance_File InsFile JOIN
	   Insurance_File_Type IFT ON
	   IFT.insurance_file_type_id=InsFile.insurance_file_type_id
	   JOIN Agent_commission ac ON InsFile.insurance_file_cnt = ac.insurance_file_cnt  
	   WHERE NOT IFT.code IN('QUOTE','MTAQTETEMP','MTAQREINS','MTAQUOTE', 'MTAQCAN') AND
	   InsFile.insurance_folder_cnt=@insurance_folder_cnt
	   AND  cover_start_date <= (Select cover_start_date From insurance_file Where insurance_file_cnt=@insurance_file_cnt)
	   AND InsFile.insurance_file_cnt < @insurance_file_cnt
	   AND ac.risk_type_id = @risk_type_id
	   AND ac.commission_band_id = @commission_band_id

   ELSE
       SELECT *   
	   INTO #TempInsurance_File  
       FROM Insurance_File  
       WHERE insurance_folder_cnt = @insurance_folder_cnt
	   
	   SELECT @last_insurance_file_cnt=MAX(InsFile.insurance_file_cnt) FROM #TempInsurance_File InsFile JOIN        
	   Insurance_File_Type IFT ON        
	   IFT.insurance_file_type_id=InsFile.insurance_file_type_id
	   JOIN Agent_commission ac ON InsFile.insurance_file_cnt = ac.insurance_file_cnt  AND InsFile.lead_agent_cnt = ac.party_cnt         
	   WHERE NOT IFT.code IN('QUOTE','MTAQTETEMP','MTAQREINS','MTAQUOTE', 'MTAQCAN','RENEWAL') AND        
	   InsFile.insurance_folder_cnt=@insurance_folder_cnt
	   AND  cover_start_date <= (Select cover_start_date From insurance_file Where insurance_file_cnt=@insurance_file_cnt)  
	   AND InsFile.insurance_file_cnt < @insurance_file_cnt
	   AND ac.risk_type_id = @risk_type_id  
	   AND ac.commission_band_id = @commission_band_id

       Drop Table #TempInsurance_File	   
     --Pick the amended commission percentage if the commission got amended ever in the policy 

	--DECLARE @is_amended INT
	IF EXISTS(SELECT TOP 1  
	commission_percentage  
	FROM Agent_commission  AC
	WHERE is_lead_agent=@is_lead_agent AND party_cnt=@party_id AND  
	risk_type_id=@risk_type_id AND commission_band_id=@commission_band_id AND is_amended=1 
	AND insurance_file_cnt IN (select Insurance_File_cnt FROM Insurance_File InsFile INNER JOIN  Insurance_File_Type IFT ON
	IFT.insurance_file_type_id=InsFile.insurance_file_type_id  WHERE insurance_folder_cnt=@insurance_folder_cnt AND
	cover_start_date <= (Select cover_start_date From insurance_file Where insurance_file_cnt=@insurance_file_cnt)  
	AND insurance_file_cnt < @insurance_file_cnt and IFT.code NOT IN('QUOTE','MTAQTETEMP','MTAQREINS','MTAQUOTE', 'MTAQCAN','RENEWAL')))  
	BEGIN
		SET @is_amended=1
	END


	      
   --S4I SRF 28014 (1.8.11): Pick the last amended agent commission percentage.
   IF ISNULL(@is_amended,0)=1
   BEGIN
   IF EXISTS(      
   SELECT TOP 1      
    commission_percentage    
   FROM Agent_commission        
  WHERE insurance_file_cnt=@last_insurance_file_cnt AND is_lead_agent=@is_lead_agent AND party_cnt=@party_id AND          
    risk_type_id=@risk_type_id  AND commission_band_id=@commission_band_id  AND          
           insurance_file_cnt IN (select Insurance_File_cnt FROM Insurance_File WHERE insurance_folder_cnt=@insurance_folder_cnt))      
      
   BEGIN    
    SELECT TOP 1        
    @commission_rate=commission_percentage,        
    @Is_value=0,
	@tax_group_id=tax_group_id,  
    @is_amended = 1          
    FROM Agent_commission        
     WHERE insurance_file_cnt<=@last_insurance_file_cnt AND is_lead_agent=@is_lead_agent AND party_cnt=@party_id AND          
      risk_type_id=@risk_type_id AND commission_band_id=@commission_band_id AND         
             insurance_file_cnt IN (select Insurance_File_cnt FROM Insurance_File WHERE insurance_folder_cnt=@insurance_folder_cnt)       
    order by insurance_file_cnt DESC      
   END     
 END       
END 
END  
