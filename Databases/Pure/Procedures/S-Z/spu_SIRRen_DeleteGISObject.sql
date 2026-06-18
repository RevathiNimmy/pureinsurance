SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SIRRen_DeleteGISObject'
GO

CREATE PROCEDURE spu_SIRRen_DeleteGISObject
    @Gis_Policy_Link_Id int
AS
/* AK 12/12/2001 - Stored procedure to delete GIS data, including all the child objects data recursively */
BEGIN
    DECLARE @Gis_Data_Model_Id int
    DECLARE @Table_Name varchar(50)
    DECLARE @LinkColumnName varchar(50)

    EXECUTE DDLDropTable 'TempObj'

    /* Get the datamodelId for ths policy */
    SELECT @GIS_Data_Model_Id = Gis_Data_Model_Id
        FROM GIs_Policy_Link
        WHERE GIS_Policy_Link_Id = @Gis_Policy_Link_Id

    /* Create the Table List now */
    EXECUTE spu_SIRRen_CreateObjectList @GIS_Data_Model_Id

    /* Get list of all the these objects in reverse order */
    DECLARE Obj_cursor CURSOR FAST_FORWARD FOR
        SELECT Table_Name, ColumnName
        FROM TempObj
        ORDER BY GIS_Object_Id DESC

    OPEN Obj_cursor
    FETCH NEXT FROM Obj_cursor INTO @Table_Name, @LinkColumnName

    WHILE @@FETCH_STATUS = 0 BEGIN
        /* Now we can delete the data after building the DELETE statement */
        DECLARE @SQL varchar(255)

        SELECT @SQL = 'DELETE ' + @Table_Name + ' WHERE ' + @LinkColumnName + ' = ' + CONVERT(varchar(10), @GIS_Policy_Link_Id)
        EXECUTE (@SQL)

        FETCH NEXT FROM Obj_cursor INTO @Table_Name, @LinkColumnName
    END

    CLOSE Obj_cursor
    DEALLOCATE Obj_cursor

    /* Now we have deleted all the Risk Information, so we can remove GIS_Policy_Link as well */
    DELETE GIS_Policy_Link
        WHERE GIS_Policy_Link_Id = @GIS_Policy_Link_Id

END
GO

