SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_Get_TransType_By_RiskKey
GO
CREATE PROC spu_Get_TransType_By_RiskKey 
@risk_cnt INT=0,
@Insurance_File_Cnt INT =0
AS

BEGIN

DECLARE @Code AS VARCHAR(20)

IF @risk_cnt<>0 
BEGIN
SELECT @Code=code FROM Insurance_file_type WHERE Insurance_file_type_ID  IN 
(SELECT Insurance_file_type_ID FROM Insurance_file WHERE Insurance_file_cnt IN 
(SELECT insurance_file_cnt FROM insurance_file_risk_link WHERE risk_cnt = @risk_cnt ))
END

IF @Insurance_File_Cnt<>0 
BEGIN
SELECT @Code=code FROM Insurance_file_type WHERE Insurance_file_type_ID  IN 
(SELECT Insurance_file_type_ID FROM Insurance_file WHERE Insurance_file_cnt =@Insurance_File_Cnt)
END

SELECT @CODE=LTRIM(RTRIM(@Code))
IF @Code='POLICY'  OR  @Code='QUOTE' OR @Code='WRITTEN'     
BEGIN   
SELECT  'NB'
END
ELSE IF @Code='MTAQUOTE' OR @Code='MTA PERM' OR @Code='MTA TEMP' OR @Code='MTAQTETEMP' OR @Code='MTAREINS' OR @Code='MTAQREINS'
BEGIN 
SELECT 'MTA'
END           
ELSE IF @Code='RENEWAL'
BEGIN 
SELECT 'REN'   
END
ELSE IF @Code='MTACAN' OR @Code='MTAQCAN'
BEGIN 
SELECT 'MTC'   
END
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
