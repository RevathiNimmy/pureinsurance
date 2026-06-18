SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure Spu_Get_Unique_Clause_Code
GO
CREATE PROCEDURE Spu_Get_Unique_Clause_Code
    @Code VARCHAR(10)
AS
BEGIN
    Declare @PreviousCode varchar(20)
    Declare @NewCode varchar(20)
    Declare @CodeSearch varchar(15)
    Declare @Counter varchar(4)
	Declare @StringAfterLastUnderScore varchar(20)
    SET @CodeSearch=@Code+'_'
    
	SELECT @PreviousCode=MAX(SUBSTRING(code, CHARINDEX('_',code)+1,20))  FROM document_template WHERE code like @CodeSearch+'%'+ right(CONVERT(varchar,GETDATE(),112),6) 
	IF CHARINDEX('_', @PreviousCode) > 0
	BEGIN
	SELECT @StringAfterLastUnderScore = SUBSTRING(RTRIM(LTRIM(@PreviousCode)),(RTRIM(LTRIM(LEN(@PreviousCode))) - CHARINDEX('_',REVERSE(RTRIM(LTRIM(@PreviousCode))))+ 2),20)
	SELECT @Counter =REPLACE(LEFT(@StringAfterLastUnderScore, LEN(@StringAfterLastUnderScore)-6),'ED','')
	END
	ELSE
	BEGIN
	SELECT @Counter =SUBSTRING(SUBSTRING(@PreviousCode,1,LEN(@PreviousCode)-6), CHARINDEX('_',@PreviousCode)+1,4)
    END 
	
	SELECT  @Counter = ISNULL(LEFT(Val,PATINDEX('%[^0-9]%', Val+'a')-1),0) + 1 from(
    SELECT SUBSTRING(@Counter, PATINDEX('%[0-9]%', @Counter), LEN(@Counter)) Val
)x
	
    IF LEN(@Counter) =1 
	SET @Counter = '0'+CONVERT(varchar,@Counter,2)

    SET @NewCode= @CodeSearch+@Counter+right(CONVERT(varchar,GETDATE(),112),6)

    SELECT @NewCode AS NewCode
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
