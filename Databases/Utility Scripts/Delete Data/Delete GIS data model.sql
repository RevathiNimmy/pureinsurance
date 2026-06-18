DECLARE @gis_data_model_code VARCHAR(20),
    @gis_data_model_id INT,
    @gis_object_id INT,
    @table_name VARCHAR(70),
    @SQL VARCHAR(1000),
    @sp_name VARCHAR(100)

SELECT  @gis_data_model_code = 'GIIHouse'

SELECT  @gis_data_model_id = gis_data_model_id
FROM    gis_data_model
WHERE   code = @gis_data_model_code

IF @gis_data_model_id IS NULL
    RETURN

/* Get these backwards because it guarantees that there'll be no nastyness with dependencies */
DECLARE object_cursor CURSOR FOR
    SELECT  gis_object_id,
        table_name
    FROM    gis_object
    WHERE   gis_data_model_id = @gis_data_model_id
    ORDER BY gis_object_id DESC

/* Before we can clear down the screens, we need to reset the data model id for child screens...
   Let's assume no more than 3 levels of screens */

PRINT 'Updating sub-screens'
UPDATE  gis_screen
SET gis_data_model_id = @gis_data_model_id
WHERE   parent_id IN
(   SELECT  gis_screen_id
    FROM    gis_screen
    WHERE   gis_data_model_id = @gis_data_model_id
)
AND gis_data_model_id = 0

PRINT 'Updating sub-sub-screens'
UPDATE  gis_screen
SET gis_data_model_id = @gis_data_model_id
WHERE   parent_id IN
(   SELECT  gis_screen_id
    FROM    gis_screen
    WHERE   gis_data_model_id = @gis_data_model_id
)
AND gis_data_model_id = 0

PRINT 'Updating sub-sub-sub-screens'
UPDATE  gis_screen
SET gis_data_model_id = @gis_data_model_id
WHERE   parent_id IN
(   SELECT  gis_screen_id
    FROM    gis_screen
    WHERE   gis_data_model_id = @gis_data_model_id
)
AND gis_data_model_id = 0

/* Clear down the screen details */
PRINT   'Deleting From GIS_screen_detail'
DELETE
FROM    gis_screen_detail
WHERE   gis_screen_id IN
(   SELECT  gis_screen_id
    FROM    gis_screen
    WHERE   gis_data_model_id = @gis_data_model_id
)

/* Clear down the screens */
PRINT   'Deleting From GIS_screen'
DELETE
FROM    gis_screen
WHERE   gis_data_model_id = @gis_data_model_id


/* Drop the sum insured table - it's not in the GIS */
PRINT   'Dropping ' + @gis_data_model_code + '_sum_insured'
SELECT  @SQL = 'DROP TABLE ' + @gis_data_model_code + '_sum_insured'
EXEC    (@SQL)

/* Drop the standrd wording table - it's not in the GIS */
PRINT   'Dropping ' + @gis_data_model_code + '_standard_wording'
SELECT  @SQL = 'DROP TABLE ' + @gis_data_model_code + '_standard_wording'
EXEC    (@SQL)

OPEN object_cursor
FETCH NEXT FROM object_cursor INTO @gis_object_id, @table_name

WHILE (@@FETCH_STATUS = 0)
BEGIN

    /* Drop the table */
    PRINT   'Dropping ' + @table_name
    SELECT  @SQL = 'DROP TABLE ' + @table_name
    EXEC    (@SQL)

    /* Delete its properties */
    PRINT   'Deleting properties for ' + @table_name
    DELETE
    FROM    GIS_Cobol_Linkage
    WHERE   gis_object_id = @gis_object_id

    DELETE
    FROM    GIS_Scheme_Property
    WHERE   gis_object_id = @gis_object_id

    DELETE
    FROM    GIS_Property
    WHERE   gis_object_id = @gis_object_id

    FETCH NEXT FROM object_cursor INTO @gis_object_id, @table_name
END

CLOSE object_cursor
DEALLOCATE object_cursor

/* Now delete the objects - didn't do it above because I was unsure what effect that would have on the cursor */
PRINT   'Deleting objects'
DELETE
FROM    GIS_Object
WHERE   gis_data_model_id = @gis_data_model_id

/* Now delete gis_policy_link */
PRINT   'Deleting GIS policy link'
DELETE
FROM    GIS_Policy_Schemes_Sel
WHERE   gis_policy_link_id IN (
    SELECT  gis_policy_link_id
    FROM    GIS_Policy_Link
    WHERE   gis_data_model_id = @gis_data_model_id)

DELETE
FROM    PVD_Results
WHERE   gis_policy_link_id IN (
    SELECT  gis_policy_link_id
    FROM    GIS_Policy_Link
    WHERE   gis_data_model_id = @gis_data_model_id)

DELETE
FROM    GIS_Policy_Link
WHERE   gis_data_model_id = @gis_data_model_id

/* Finally remove the data model record */
PRINT   'Deleting data model'
DELETE
FROM    GIS_QEM_Usage
WHERE   gis_data_model_id = @gis_data_model_id

DELETE
FROM    GIS_Data_Model_Business
WHERE   gis_data_model_id = @gis_data_model_id

DELETE
FROM    GIS_New_Object
WHERE   gis_data_model_id = @gis_data_model_id

DELETE
FROM    GIS_Data_Model
WHERE   gis_data_model_id = @gis_data_model_id

/* Let's be thorough and get rid of any document production stored procedures */

DECLARE sp_wp_cursor CURSOR FOR
    SELECT  name
    FROM    sysobjects
    WHERE   name like 'sp_wp_' + @gis_data_model_code + '%'
    AND OBJECTPROPERTY(id, N'IsProcedure') = 1
    ORDER BY name

OPEN sp_wp_cursor
FETCH NEXT FROM sp_wp_cursor INTO @sp_name

WHILE (@@FETCH_STATUS = 0)
BEGIN

    /* Drop the table */
    PRINT   'Dropping ' + @sp_name
    SELECT  @SQL = 'DROP PROCEDURE ' + @sp_name
    EXEC    (@SQL)

    FETCH NEXT FROM sp_wp_cursor INTO @sp_name
END

CLOSE sp_wp_cursor
DEALLOCATE sp_wp_cursor

