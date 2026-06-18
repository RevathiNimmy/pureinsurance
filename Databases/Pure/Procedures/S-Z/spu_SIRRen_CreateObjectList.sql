SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SIRRen_CreateObjectList'
GO

CREATE PROCEDURE spu_SIRRen_CreateObjectList
    @Gis_Data_Model_Id INT,
    @Parent_Object_Id INT = NULL,
    @ColumnName VARCHAR(50) = ''
AS
/* AK 12/12/2001 - Stored procedure to create list of all the objects for a given data-model in order of their heirarchy */
BEGIN
    DECLARE @Object_Id Int
    DECLARE @ColumnNm VARCHAR(50)

    IF @Parent_Object_Id is null BEGIN
        /* If the table exists already, drop it and then create it*/
        EXECUTE DDLDropTable 'TempObj'

        CREATE TABLE TempObj(
            Object_Id Int NOT NULL IDENTITY,
            GIS_Object_Id INT Not NULL,
            Table_Name varchar(50) NOT NULL,
            Parent_Object_Id int NULL,
            Is_Quote_Object int NULL,
            ColumnName varchar(50) NULL
        )

        /* Extract the top level objects first */
        INSERT INTO TempObj (
            GIS_Object_Id,
            Table_Name,
            Parent_Object_Id,
            Is_Quote_Object,
            ColumnName
        ) SELECT
            GIS_Object_Id,
            Table_Name,
            Parent_Object_Id,
            Is_Quote_Object,
            (
                CASE
                WHEN Is_Quote_object = 0 THEN Table_Name + '_Id'
                ELSE 'GIS_Policy_Link_Id'
                END
            )
            FROM GIS_Object
            WHERE GIS_Data_Model_Id = @Gis_Data_Model_Id
            AND Parent_object_id IS NULL

        /* for each of these (already extracted) objects we will have to fetch the child objects now */
        DECLARE Top_cursor CURSOR FAST_FORWARD FOR
            SELECT GIS_Object_Id, ColumnName
            FROM TempObj

        OPEN Top_cursor
        FETCH NEXT FROM Top_cursor INTO @Object_Id, @ColumnNm

        WHILE @@FETCH_STATUS = 0 BEGIN
            /* Now we can find all the child objects for this object by using this very procedure */
            EXECUTE spu_SIRRen_CreateObjectList @Gis_Data_Model_Id, @Object_Id, @ColumnNm

            FETCH NEXT FROM Top_cursor INTO @Object_Id, @ColumnNm
        END

        CLOSE Top_cursor
        DEALLOCATE Top_cursor
    END ELSE BEGIN
        /* Add all the child objects for this Object Id into temporary table */
        INSERT INTO TempObj (
            GIS_Object_Id,
            Table_Name,
            Parent_Object_Id,
            Is_Quote_Object,
            ColumnName
        ) SELECT
            GIS_Object_Id,
            Table_Name,
            Parent_Object_Id,
            Is_Quote_Object,
            @ColumnName
            FROM GIS_Object
            WHERE GIS_Data_Model_Id = @Gis_Data_Model_Id
            AND Parent_object_id = @Parent_Object_Id
    END
END
GO

