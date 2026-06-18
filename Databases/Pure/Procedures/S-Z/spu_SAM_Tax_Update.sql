SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Tax_Update'
GO
-- =============================================
-- Author:  <Dharmendra Kumar>
-- Create date: <15/06/2012>
-- Description: <Update is_value,percentage Isedited>
-- =============================================
CREATE PROCEDURE spu_SAM_Tax_Update  
 -- Add the parameters for the stored procedure here  
 @tax_calculation_cnt int,  
 @IsValue tinyint,  
 @TaxPercentage float,  
 @TaxValue decimal(18,2)  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
  --Get Suppress Decimal flag to round whole number  
    DECLARE @SuppressDecimalOption AS INT=112  
 DECLARE @bIsSuppressDecimal As TINYINT=(Select ISNULL(Value,0) from Hidden_options WHERE option_number=@SuppressDecimalOption)  
  
    IF ISNULL(@IsValue,0) = 0  
      SET @TaxValue = (SELECT premium * @TaxPercentage/100  
         FROM Tax_Calculation  
       WHERE tax_calculation_cnt=@tax_calculation_cnt)  
 else        set @TaxPercentage= (select @TaxValue * 100/premium FROM Tax_Calculation  
       WHERE tax_calculation_cnt=@tax_calculation_cnt and premium<>0)  
  
 UPDATE Tax_Calculation  
  SET  
    is_value=@IsValue,  
    percentage=@TaxPercentage,  
    is_manually_changed = 1,  
    value= (CASE WHEN @bIsSuppressDecimal=1 THEN ROUND(@TaxValue,0) Else @TaxValue END)  
  WHERE tax_calculation_cnt=@tax_calculation_cnt  
END  
GO



