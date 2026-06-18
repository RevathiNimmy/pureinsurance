DDLDROPPROCEDURE 'spu_Select_SubAgents_CommissionOutput'
GO  
CREATE PROCEDURE spu_Select_SubAgents_CommissionOutput         
    @RatingSectionTypeCode VARCHAR(1000),        
    @product_id INT=NULL,        
    @risk_type_id INT=NULL,        
    @transaction_type_id INT=NULL,        
    @party_type_id INT=NULL,  
    @Band_Type VARCHAR(15) = 'Agent',  
    @Multiple_Flag TINYINT = 0,
    @party_cnt INT = 0
AS  
DECLARE @SQL nVARCHAR(4000)  
  
--This SP will not handle multiple sub agents  
--configured on the same effective date when Multiple_Flag = 0,  
--ie; when single row of sub agent is expected.  
SET @SQL='SELECT DISTINCT '  
SELECT @SQL = Case WHEN @Multiple_Flag = 0 THEN @SQL + 'TOP 1 '  
     ELSE  
    @SQL + ' '  
     END  
SELECT   @SQL = @SQL + '(select PA.party_cnt from party PA where PA.party_cnt = CA.party_cnt),  
    (select PA.shortname from party PA where PA.party_cnt = CA.party_cnt),  
    (select PA.resolved_name from party PA where PA.party_cnt = CA.party_cnt),  
    CA.rate,  
    CA.is_value,  
    CA.tax_group_id,  
    CA.Maximum_rate,  
    CA.effective_date,'  
    IF @Band_Type = 'SubAgent'  
  		SET @SQL=@SQL+'(Select Code from Commission_Band CB WHERE CB.Commission_Band_id = PT.sub_commission_band) AS band_code'  
 	ELSE  
  		SET @SQL=@SQL+'(Select Code from Commission_Band CB WHERE CB.Commission_Band_id = PT.lead_commission_band) AS band_code'  
  
 	SET @SQL=@SQL+',(Select Code from tax_group WHERE tax_group_id= CA.tax_group_id) AS tax_group_code '  
  
 SET @SQL=@SQL+'  
  	FROM Rating_Section_Type RS  
  	LEFT JOIN Peril_Type_Usage PTU ON RS.peril_group_id=PTU.peril_group_id  
  	LEFT JOIN Peril_Type PT ON PT.peril_type_id=PTU.peril_type_id  
  	LEFT JOIN Commission_Arrangement CA ON '  
  
 IF @Band_Type = 'SubAgent'  
  		SET @SQL=@SQL+' CA.commission_band_id IN (0,PT.sub_commission_band) WHERE'  
    ELSE  
  		SET @SQL=@SQL+' CA.commission_band_id IN (0,PT.lead_commission_band) WHERE'  
  
IF @product_id IS NOT NULL  
SELECT @SQL=@SQL+ ' Product_id IN (0, ' + convert(varchar,@product_id) + ') AND'  
  
IF @risk_type_id IS NOT NULL  
SELECT @SQL=@SQL+ ' risk_type_id IN (0, ' + convert(varchar,@risk_type_id) + ') AND '  
  
IF @transaction_type_id IS NOT NULL  
SELECT @SQL=@SQL+ ' Transaction_type_id IN (0, ' + convert(varchar,@transaction_type_id) + ') AND'  
  
IF @party_type_id IS NOT NULL  
SELECT @SQL=@SQL+ ' Party_type IN (' + convert(varchar,@party_type_id) + ') AND'  
  
IF @party_cnt  <> 0  AND UPPER(@Band_Type) = 'AGENT'
SELECT @SQL=@SQL+ ' Party_cnt IN (' + convert(varchar,@party_cnt) + ') AND'  
  
SELECT @SQL=@SQL+ ' (CA.is_deleted = 0)  
AND RS.code in (' + @RatingSectionTypeCode + ') '  
  
SELECT @SQL =  @SQL + 'AND CA.effective_date <= GetDate() '  
SELECT @SQL = @SQL  + 'ORDER BY CA.effective_date DESC '  
  
  
  
EXEC sp_executesql  @SQL  
