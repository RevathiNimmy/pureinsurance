
EXECUTE DDLDropProcedure 'spu_Get_LookUp_List'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_Get_LookUp_List
    @KeyIdFieldName varchar(128),
    @DescFieldName      varchar(128),
    @TableName      varchar(128)


--
-- PDB3072002 Retrieves Look up list with parameter depicting lookup table
--

AS

-- four * 128 + extra sql keywords
DECLARE @sql Nvarchar(600)

SET @SQL = N'SELECT ' + @KeyIdFieldName + ', ' + @DescFieldName + ' FROM ' + @TableName + ' ORDER BY ' + @DescFieldName

EXEC SP_EXECUTESQL @SQL

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO
