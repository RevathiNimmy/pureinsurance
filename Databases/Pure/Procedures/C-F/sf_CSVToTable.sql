/****** Object:  UserDefinedFunction [dbo].[CSVToTable]    Script Date: 04/28/2013 10:45:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

EXECUTE DDLDropFunction'sf_CSVToTable'
GO

CREATE FUNCTION [dbo].[sf_CSVToTable] (@InStr VARCHAR(MAX))
RETURNS @TempTab TABLE
   (id int not null)
AS
/*
	Created By:		George Harris
	Creation Date:	20 Sep 2018
	Description:	Takes in a csv string a coverts/returns it as a table

*/
BEGIN
    ;-- Ensure input ends with comma
	SET @InStr = REPLACE(@InStr + ',', ',,', ',')
	DECLARE @SP INT
DECLARE @VALUE VARCHAR(1000)
WHILE PATINDEX('%,%', @INSTR ) <> 0 
BEGIN
   SELECT  @SP = PATINDEX('%,%',@INSTR)
   SELECT  @VALUE = LEFT(@INSTR , @SP - 1)
   SELECT  @INSTR = STUFF(@INSTR, 1, @SP, '')
   INSERT INTO @TempTab(id) VALUES (@VALUE)
END
	RETURN
END
GO